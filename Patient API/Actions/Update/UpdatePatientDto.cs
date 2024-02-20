using PatientApi.Constants;

namespace PatientApi.Actions.Update
{
    public class UpdatePatientDto
    {
        public UpdatePatientNameDto Name { get; set; } = new UpdatePatientNameDto();

        public Gender? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }
    }
}
