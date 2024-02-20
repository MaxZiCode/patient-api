using Microsoft.AspNetCore.Mvc;

using PatientApi.Constants;
using PatientApi.Database.Repositories;
using PatientApi.Models;

namespace PatientApi.Actions.Delete
{
    [ApiController]
    [Route(Routes.Patient)]
    public class DeletePatientController : ControllerBase
    {
        private readonly IPatientRepository PATIENT_REPOSITORY;

        public DeletePatientController(IPatientRepository patientRepository)
        {
            PATIENT_REPOSITORY = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatientAsync([FromRoute] Guid id)
        {
            Patient? patient = await PATIENT_REPOSITORY.GetAsync(id);
            if (patient is null)
                return NotFound();

            await PATIENT_REPOSITORY.RemoveAsync(patient);
            return NoContent();
        }
    }
}
