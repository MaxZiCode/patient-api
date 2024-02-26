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

        public async Task<IReadOnlyCollection<Patient>> SearchByBirthDate(DateSearchCriteria dateSearchCriteria)
        {
            IQueryable<Patient> query = PATIENT_CONTEXT.Patients.AsNoTracking().Include(p => p.PatientName);
            foreach (Period equalPeriod in dateSearchCriteria.EqualPeriods)
                query = query.Where(pat => equalPeriod.DateFrom <= pat.BirthDate && pat.BirthDate <= equalPeriod.DateTo);

            foreach (Period notEqualPeriod in dateSearchCriteria.NotEquialPeriods)
                query = query.Where(pat => pat.BirthDate < notEqualPeriod.DateFrom || notEqualPeriod.DateTo < pat.BirthDate);

            foreach (DateTime fromDate in dateSearchCriteria.FromDates)
                query = query.Where(pat => fromDate <= pat.BirthDate);

            foreach (DateTime toDate in dateSearchCriteria.ToDates)
                query = query.Where(pat => pat.BirthDate <= toDate);

            return await query.ToListAsync();
        }
    }
}
