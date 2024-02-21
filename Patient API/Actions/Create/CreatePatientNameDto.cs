using System.ComponentModel.DataAnnotations;

namespace PatientApi.Actions.Create
{
    public class CreatePatientNameDto
    {
        public string? Use { get; set; }

        [Required]
        public string Family { get; set; } = string.Empty;

        public List<string>? Given { get; set; }
    }
}
