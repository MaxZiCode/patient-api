namespace PatientApi.Actions.Read
{
    public class ReadPatientNameDto
    {
        public Guid Id { get; set; }

        public string? Use { get; set; }

        public string Family { get; set; } = string.Empty;

        public List<string>? Given { get; set; }
    }
}
