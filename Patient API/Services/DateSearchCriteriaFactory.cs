using PatientApi.Models;

using System.Text.RegularExpressions;

namespace PatientApi.Services
{
    public class DateSearchCriteriaFactory : IDateSearchCriteriaFactory
    {
        private const int PREFIX_LENGTH = 2;

        // https://www.hl7.org/fhir/datatypes.html#dateTime
        private const string DATETIME_PATTERN = "([0-9]([0-9]([0-9][1-9]|[1-9]0)|[1-9]00)|[1-9]000)(-(0[1-9]|1[0-2])(-(0[1-9]|[1-2][0-9]|3[0-1])(T([01][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60)(\\.[0-9]{1,9})?)?)?(Z|(\\+|-)((0[0-9]|1[0-3]):[0-5][0-9]|14:00)?)?)?";

        private readonly Regex DATETIME_REGEX = new(DATETIME_PATTERN);

        public DateSearchCriteria GetDateSearchCriteria(IEnumerable<string> dateFilters, DateTimeOffset? currentDateTimeOffset = null)
        {
            currentDateTimeOffset ??= DateTimeOffset.Now;

            List<Period> equalPeriods = new();
            List<Period> notEqualPeriods = new();
            List<DateTime> fromDates = new();
            List<DateTime> toDates = new();

            foreach (string dateFilter in dateFilters)
            {
                string prefix = dateFilter[..PREFIX_LENGTH];
                string dateString = dateFilter[PREFIX_LENGTH..];

                Match datePartMatch = DATETIME_REGEX.Match(dateString);
                if (!datePartMatch.Success)
                    throw new ArgumentException($"{dateFilter} is not valid date.", nameof(dateFilters));

                IList<Group> datePartGroups = datePartMatch.Groups;

                int year = int.Parse(datePartGroups.ElementAt(1).Value);

                string? monthString = datePartGroups.ElementAtOrDefault(5)?.Value;
                int? month = string.IsNullOrEmpty(monthString) ? null : int.Parse(monthString);

                string? dayString = datePartGroups.ElementAtOrDefault(7)?.Value;
                int? day = string.IsNullOrEmpty(dayString) ? null : int.Parse(dayString);

                string? timeString = datePartGroups.ElementAtOrDefault(8)?.Value;
                TimeSpan? time = string.IsNullOrEmpty(timeString) ? null : TimeSpan.Parse(timeString[1..]);

                string? millisecondsString = datePartGroups.ElementAtOrDefault(11)?.Value;
                int? milliseconds = string.IsNullOrEmpty(millisecondsString) || time is null ? null : time.Value.Milliseconds;

                string? offsetString = datePartGroups.ElementAtOrDefault(12)?.Value;
                TimeSpan? offset =
                    string.IsNullOrEmpty(offsetString) || offsetString.Equals("Z", StringComparison.OrdinalIgnoreCase)
                    ? null
                    : currentDateTimeOffset.Value.Offset - TimeSpan.Parse(offsetString);

                Period period = new Period(year, month, day, time?.Hours, time?.Minutes, time?.Seconds, milliseconds, offset);

                switch (prefix)
                {
                    case "eq":
                    {
                        equalPeriods.Add(period);
                        break;
                    }
                    case "ne":
                    {
                        notEqualPeriods.Add(period);
                        break;
                    }
                    case "sa":
                    case "gt":
                    {
                        DateTime greaterThan = period.DateTo.AddTicks(1);
                        fromDates.Add(greaterThan);
                        break;
                    }
                    case "eb":
                    case "lt":
                    {
                        DateTime lessThan = period.DateFrom.AddTicks(-1);
                        toDates.Add(lessThan);
                        break;
                    }
                    case "ge":
                    {
                        DateTime greaterOrEqual = period.DateTo;
                        fromDates.Add(greaterOrEqual);
                        break;
                    }
                    case "le":
                    {
                        DateTime lessOrEqual = period.DateFrom;
                        toDates.Add(lessOrEqual);
                        break;
                    }
                    case "ap":
                    {
                        if (period.DateFrom <= currentDateTimeOffset && currentDateTimeOffset <= period.DateTo)
                        {
                            equalPeriods.Add(period);
                            break;
                        }

                        TimeSpan gap = currentDateTimeOffset.Value - period.DateTo;
                        if (gap < TimeSpan.Zero)
                            gap = gap.Negate();

                        TimeSpan gapPart = gap * 0.1;

                        DateTime greaterOrEqual = period.DateFrom - gapPart;
                        fromDates.Add(greaterOrEqual);

                        DateTime lessOrEqual = period.DateTo + gapPart;
                        toDates.Add(lessOrEqual);
                        break;
                    }

                    default:
                        break;
                }
            }

            return new DateSearchCriteria(equalPeriods, notEqualPeriods, fromDates, toDates);
        }
    }
}
