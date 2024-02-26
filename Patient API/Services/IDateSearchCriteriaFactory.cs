using PatientApi.Models;

namespace PatientApi.Services
{
    public interface IDateSearchCriteriaFactory
    {
        DateSearchCriteria GetDateSearchCriteria(IEnumerable<string> dateFilters, DateTimeOffset? currentDateTimeOffset = null);
    }
}