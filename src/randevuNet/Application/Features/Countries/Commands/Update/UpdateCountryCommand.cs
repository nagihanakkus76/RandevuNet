using Application.Features.Countries.Constants;
using Application.Features.Countries.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Countries.Constants.CountriesOperationClaims;

namespace Application.Features.Countries.Commands.Update;

public class UpdateCountryCommand : IRequest<UpdatedCountryResponse>, ISecuredRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public string[] Roles => [Admin, Write, CountriesOperationClaims.Update];

    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, UpdatedCountryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly CountryBusinessRules _countryBusinessRules;

        public UpdateCountryCommandHandler(IMapper mapper, ICountryRepository countryRepository,
                                         CountryBusinessRules countryBusinessRules)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
            _countryBusinessRules = countryBusinessRules;
        }

        public async Task<UpdatedCountryResponse> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            Country? country = await _countryRepository.GetAsync(predicate: c => c.Id == request.Id, cancellationToken: cancellationToken);
            await _countryBusinessRules.CountryShouldExistWhenSelected(country);
            country = _mapper.Map(request, country);

            await _countryRepository.UpdateAsync(country!);

            UpdatedCountryResponse response = _mapper.Map<UpdatedCountryResponse>(country);
            return response;
        }
    }
}