using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PatientApi.Actions.Read;
using PatientApi.Constants;
using PatientApi.Database.Repositories;
using PatientApi.Models;

namespace PatientApi.Actions.Update
{
    [ApiController]
    [Route(Routes.Patient)]
    public class UpdatePatientController : ControllerBase
    {
        private readonly IPatientRepository PATIENT_REPOSITORY;
        private readonly IMapper MAPPER;

        public UpdatePatientController(IPatientRepository patientRepository, IMapper mapper)
        {
            PATIENT_REPOSITORY = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            MAPPER = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatientAsync([FromRoute] Guid id, UpdatePatientDto updateDto)
        {
            Patient? patient = await PATIENT_REPOSITORY.GetAsync(id);
            if (patient is null)
                return NotFound();

            MAPPER.Map(updateDto, patient);
            await PATIENT_REPOSITORY.UpdateAsync(patient);

            ReadPatientDto readDto = MAPPER.Map<ReadPatientDto>(patient);
            return Ok(readDto);
        }
    }
}
