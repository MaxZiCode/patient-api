using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PatientApi.Actions.Read;
using PatientApi.Constants;
using PatientApi.Database.Repositories;
using PatientApi.Models;
using PatientApi.Services;

using System.ComponentModel.DataAnnotations;

namespace PatientApi.Actions.Search
{
    [ApiController]
    [Route(Routes.Patient)]
    [Tags("Patient")]
    [Produces("application/json")]
    public class SearchPatientController : ControllerBase
    {
        private readonly IPatientRepository PATIENT_REPOSITORY;
        private readonly IMapper MAPPER;
        private readonly IDateSearchCriteriaFactory DATE_SEARCH_CRITERIA_FACTORY;

        public SearchPatientController(IPatientRepository patientRepository, IMapper mapper, IDateSearchCriteriaFactory dateSearchCriteriaFactory)
        {
            PATIENT_REPOSITORY = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            MAPPER = mapper ?? throw new ArgumentNullException(nameof(mapper));
            DATE_SEARCH_CRITERIA_FACTORY = dateSearchCriteriaFactory ?? throw new ArgumentNullException(nameof(dateSearchCriteriaFactory));
        }

        /// <summary>
        /// Searches Patients by birth date.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ReadPatientDto>), 200)]
        public async Task<IActionResult> SearchPatientsAsync([FromQuery(Name = "date")][Required] string[] dateFilterParams)
        {
            var dateSearchCriteria = DATE_SEARCH_CRITERIA_FACTORY.GetDateSearchCriteria(dateFilterParams);

            IReadOnlyCollection<Patient> patients = await PATIENT_REPOSITORY.SearchByBirthDate(dateSearchCriteria);
            List<ReadPatientDto> dtos = MAPPER.Map<List<ReadPatientDto>>(patients);
            return Ok(dtos);
        }
    }
}
