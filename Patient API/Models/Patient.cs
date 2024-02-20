using PatientApi.Constants;

namespace PatientApi.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        public Guid PatientNameId { get; set; }

        public PatientName PatientName { get; set; } = new PatientName();

        public Gender? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public bool? Active { get; set; }
    }
}
