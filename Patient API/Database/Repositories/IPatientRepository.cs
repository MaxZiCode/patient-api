using PatientApi.Models;

namespace PatientApi.Database.Repositories
{
    public interface IPatientRepository
    {
        Task AddAsync(Patient patient);
        Task<Patient?> GetAsync(Guid id);
        Task RemoveAsync(Patient patient);
        Task<IReadOnlyCollection<Patient>> SearchByBirthDate(DateTime? dateEqual = null, DateTime? dateFrom = null, DateTime? dateTo = null, params DateTime[] datesNotEqual);
        Task UpdateAsync(Patient patient);
    }
}