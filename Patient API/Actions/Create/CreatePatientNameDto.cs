namespace PatientApi.Actions.Create
{
    public class CreatePatientNameDto
    {
        public string? Use { get; set; }

        public string Family { get; set; } = string.Empty;

        public List<string>? Given { get; set; }
    }
}
