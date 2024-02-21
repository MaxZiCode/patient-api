using PatientApi.Constants;

using System.ComponentModel.DataAnnotations;

namespace PatientApi.Actions.Update
{
    public class UpdatePatientDto
    {
        public UpdatePatientNameDto Name { get; set; } = new UpdatePatientNameDto();

        public Gender? Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }
    }
}
