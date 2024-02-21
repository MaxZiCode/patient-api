using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PatientApi.Actions.Read;
using PatientApi.Constants;
using PatientApi.Database.Repositories;
using PatientApi.Models;

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

        public SearchPatientController(IPatientRepository patientRepository, IMapper mapper)
        {
            PATIENT_REPOSITORY = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            MAPPER = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Searches Patients by birth date.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ReadPatientDto>), 200)]
        public async Task<IActionResult> SearchPatientsAsync([FromQuery(Name = "date")][Required] string[] dateParams, DateTime? currentTime = null)
        {
            currentTime ??= DateTime.Now;
            const int prefixLength = 2;

            DateTime? dateEqual = null;
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            HashSet<DateTime> datesNotEqual = new HashSet<DateTime>();

            // TODO: Incapsulate logic
            foreach (string dateParam in dateParams)
            {
                string prefix = dateParam[..prefixLength];
                DateTime dateTime = DateTime.Parse(dateParam[prefixLength..]);
                switch (prefix)
                {
                    case "eq":
                    {
                        dateEqual ??= dateTime;
                        break;
                    }
                    case "ne":
                    {
                        datesNotEqual.Add(dateTime);
                        break;
                    }
                    case "sa":
                    case "gt":
                    {
                        DateTime greaterThan = dateTime.AddTicks(1);
                        if (dateFrom is null || greaterThan > dateFrom)
                            dateFrom = greaterThan;
                        break;
                    }
                    case "eb":
                    case "lt":
                    {
                        DateTime lessThan = dateTime.AddTicks(-1);
                        if (dateTo is null || lessThan < dateTo )
                            dateTo = lessThan;
                        break;
                    }
                    case "ge":
                    {
                        DateTime greaterOrEqual = dateTime;
                        if (dateFrom is null || greaterOrEqual > dateFrom)
                            dateFrom = greaterOrEqual;
                        break;
                    }
                    case "le":
                    {
                        DateTime lessOrEqual = dateTime;
                        if (dateTo is null || lessOrEqual < dateTo)
                            dateTo = lessOrEqual;
                        break;
                    }
                    case "ap":
                    {
                        TimeSpan gap = currentTime.Value - dateTime;
                        if (gap < TimeSpan.Zero)
                            break;

                        if (gap == TimeSpan.Zero)
                        {
                            dateEqual = dateTime;
                            break;
                        }

                        TimeSpan gapPart = gap * 0.1;

                        DateTime greaterOrEqual = dateTime - gapPart;
                        if (dateFrom is null || greaterOrEqual > dateFrom)
                            dateFrom = greaterOrEqual;

                        DateTime lessOrEqual = dateTime + gapPart;
                        if (dateTo is null || lessOrEqual < dateTo)
                            dateTo = lessOrEqual;
                        break;
                    }

                    default:
                        break;
                }
            }

            IReadOnlyCollection<Patient> patients = await PATIENT_REPOSITORY.SearchByBirthDate(dateEqual, dateFrom, dateTo, datesNotEqual.ToArray());
            List<ReadPatientDto> dtos = MAPPER.Map<List<ReadPatientDto>>(patients);
            return Ok(dtos);
        }
    }
}
