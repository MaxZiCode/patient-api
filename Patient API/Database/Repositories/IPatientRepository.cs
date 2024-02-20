using PatientApi.Models;

namespace PatientApi.Database.Repositories
{
    public interface IPatientRepository
    {
        Task AddAsync(Patient patient);
        Task<Patient?> GetAsync(Guid id);
        Task RemoveAsync(Patient patient);
        Task UpdateAsync(Patient patient);
    }
}