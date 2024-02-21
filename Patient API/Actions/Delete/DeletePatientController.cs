using Microsoft.AspNetCore.Mvc;

using PatientApi.Actions.Read;
using PatientApi.Constants;
using PatientApi.Database.Repositories;
using PatientApi.Models;

namespace PatientApi.Actions.Delete
{
    [ApiController]
    [Route(Routes.Patient)]
    [Tags("Patient")]
    [Produces("application/json")]
    public class DeletePatientController : ControllerBase
    {
        private readonly IPatientRepository PATIENT_REPOSITORY;

        public DeletePatientController(IPatientRepository patientRepository)
        {
            PATIENT_REPOSITORY = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        /// <summary>
        /// Deletes a specific Patient.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
