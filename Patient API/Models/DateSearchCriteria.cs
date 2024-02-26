namespace PatientApi.Models
{
    public record DateSearchCriteria(
        IReadOnlyCollection<Period> EqualPeriods,
        IReadOnlyCollection<Period> NotEquialPeriods,
        IReadOnlyCollection<DateTime> FromDates,
        IReadOnlyCollection<DateTime> ToDates);
}
