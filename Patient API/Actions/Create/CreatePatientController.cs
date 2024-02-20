using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PatientApi.Actions.Read;
using PatientApi.Constants;
using PatientApi.Database.Repositories;
using PatientApi.Models;

namespace PatientApi.Actions.Create
{
    [ApiController]
    [Route(Routes.Patient)]
    public class CreatePatientController : ControllerBase
    {
        private readonly IPatientRepository PATIENT_REPOSITORY;
        private readonly IMapper MAPPER;

        public CreatePatientController(IPatientRepository patientRepository, IMapper mapper)
        {
            PATIENT_REPOSITORY = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            MAPPER = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatientAsync(CreatePatientDto createDto)
        {
            Patient patient = MAPPER.Map<Patient>(createDto);

            await PATIENT_REPOSITORY.AddAsync(patient);

            ReadPatientDto readDto = MAPPER.Map<ReadPatientDto>(patient);
            return new ObjectResult(readDto) { StatusCode = 201 };
        }
    }
}
