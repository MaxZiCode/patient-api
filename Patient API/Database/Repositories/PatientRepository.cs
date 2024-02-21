using Microsoft.EntityFrameworkCore;

using PatientApi.Database.Contexts;
using PatientApi.Models;

namespace PatientApi.Database.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        protected readonly PatientContext PATIENT_CONTEXT;

        public PatientRepository(PatientContext patientContext)
        {
            PATIENT_CONTEXT = patientContext ?? throw new ArgumentNullException(nameof(patientContext));
        }

        public async Task<Patient?> GetAsync(Guid id)
        {
            return await PATIENT_CONTEXT.Patients.Include(p => p.PatientName).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Patient patient)
        {
            PATIENT_CONTEXT.Patients.Add(patient);
            await PATIENT_CONTEXT.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            PATIENT_CONTEXT.Patients.Update(patient);
            await PATIENT_CONTEXT.SaveChangesAsync();
        }

        public async Task RemoveAsync(Patient patient)
        {
            PATIENT_CONTEXT.Patients.Remove(patient);
            await PATIENT_CONTEXT.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Patient>> SearchByBirthDate(DateTime? dateEqual = null, DateTime? dateFrom = null, DateTime? dateTo = null, params DateTime[] datesNotEqual)
        {
            IQueryable<Patient> query = PATIENT_CONTEXT.Patients.AsNoTracking().Include(p => p.PatientName);
            if (dateEqual != null)
                query = query.Where(p => p.BirthDate == dateEqual);

            if (dateFrom != null)
                query = query.Where(p => p.BirthDate >= dateFrom);

            if (dateTo != null)
                query = query.Where(p => p.BirthDate <= dateTo);

            if (datesNotEqual.Length > 0)
                query = query.Where(p => !datesNotEqual.Contains(p.BirthDate));

            return await query.ToListAsync();
        }
    }
}
