namespace PatientApi.Actions.Update
{
    public class UpdatePatientNameDto
    {
        public string? Use { get; set; }

        public string Family { get; set; } = string.Empty;

        public List<string>? Given { get; set; }
    }
}
