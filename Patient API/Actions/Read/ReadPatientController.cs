using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PatientApi.Constants;
using PatientApi.Database.Repositories;
using PatientApi.Models;

namespace PatientApi.Actions.Read
{
    [ApiController]
    [Route(Routes.Patient)]
    public class ReadPatientController : ControllerBase
    {
        private readonly IPatientRepository PATIENT_REPOSITORY;
        private readonly IMapper MAPPER;

        public ReadPatientController(IPatientRepository patientRepository, IMapper mapper)
        {
            PATIENT_REPOSITORY = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            MAPPER = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadPatientAsync([FromRoute] Guid id)
        {
            Patient? patient = await PATIENT_REPOSITORY.GetAsync(id);
            if (patient is null)
                return NotFound();

            ReadPatientDto dto = MAPPER.Map<ReadPatientDto>(patient);
            return Ok(dto);
        }
    }
}
