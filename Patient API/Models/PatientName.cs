namespace PatientApi.Models
{
    public class PatientName
    {
        public Guid Id { get; set; }

        public string? Use { get; set; }

        public string Family { get; set; } = string.Empty;

        public List<string>? Given { get; set; }
    }
}
