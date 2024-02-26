namespace PatientApi.Models
{
    public class Period
    {
        public DateTime DateFrom { get; private set; } = default;
        public DateTime DateTo { get; private set; } = default;

        public Period(
            int year,
            int? month = null,
            int? day = null,
            int? hour = null,
            int? minute = null,
            int? second = null,
            int? millisecond = null,
            TimeSpan? offset = null)
        {
            var datePartsAndAddFuncs = new List<(int? DatePart, Func<DateTime, DateTime> AddTimeFunc)>
            {
                (month, (d) => d.AddYears(1).AddTicks(-1)),
                (day,  (d) => d.AddMonths(1).AddTicks(-1)),
                (hour, (d) => d.AddDays(1).AddTicks(-1)),
                (minute,  (d) => d.AddHours(1).AddTicks(-1)),
                (second, (d) => d.AddMinutes(1).AddTicks(-1)),
                (millisecond,  (d) => d.AddSeconds(1).AddTicks(-1)),
                (null, (d) => d.AddMilliseconds(1).AddTicks(-1))
            };

            int nullDatePartIndex = datePartsAndAddFuncs.FindIndex(d => d.DatePart == null);
            Func<DateTime, DateTime> addTimeFunc = datePartsAndAddFuncs[nullDatePartIndex].AddTimeFunc;
            int[] dateParts = datePartsAndAddFuncs.Take(nullDatePartIndex).Select(d => d.DatePart!.Value).ToArray();

            SetPeriod(addTimeFunc, year, dateParts, offset ?? TimeSpan.Zero);
        }

        private void SetPeriod(
            Func<DateTime, DateTime> addTimeFunc,
            int year,
            int[] dateParts,
            TimeSpan offset)
        {
            int month = dateParts.Length > 0 ? dateParts[0] : 1;
            int day = dateParts.Length > 1 ? dateParts[1] : 1;
            int hour = dateParts.Length > 2 ? dateParts[2] : 0;
            int minute = dateParts.Length > 3 ? dateParts[3] : 0;
            int second = dateParts.Length > 4 ? dateParts[4] : 0;
            int millisecond = dateParts.Length > 5 ? dateParts[5] : 0;

            DateFrom = new DateTime(year, month, day, hour, minute, second, millisecond).Add(offset);
            DateTo = addTimeFunc(DateFrom).Add(offset);
        }
    }
}
