using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IDoctorRepository : IAsyncRepository<Doctor, Guid>, IRepository<Doctor, Guid>
{
    Task<List<Doctor>> GetAvailableDoctorsAsync(int departmentId, Guid? doctorID, DateOnly? date);
}