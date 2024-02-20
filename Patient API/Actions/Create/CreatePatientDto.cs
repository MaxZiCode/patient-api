using PatientApi.Constants;

namespace PatientApi.Actions.Create
{
    public class CreatePatientDto
    {
        public CreatePatientNameDto Name { get; set; } = new CreatePatientNameDto();

        public Gender? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }
    }
}
