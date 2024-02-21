using PatientApi.Constants;

using System.ComponentModel.DataAnnotations;

namespace PatientApi.Actions.Create
{
    public class CreatePatientDto
    {
        public CreatePatientNameDto Name { get; set; } = new CreatePatientNameDto();

        public Gender? Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }
    }
}
