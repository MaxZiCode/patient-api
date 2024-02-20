using PatientApi.Constants;

namespace PatientApi.Actions.Read
{
    public class ReadPatientDto
    {
        public Guid Id { get; set; }

        public ReadPatientNameDto Name { get; set; } = new ReadPatientNameDto();

        public Gender? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }
    }
}
