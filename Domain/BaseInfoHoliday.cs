using System;

namespace congestion.calculator.Domain
{
    public class BaseInfoHoliday
    {
        public int Month { get; }
        public int Day { get; }

        public BaseInfoHoliday(int month, int day)
        {
            Month = month;
            Day = day;
        }

        public bool IsHolidayOrDateBefore(DateTime date)
        {
            var dayBefore = new DateTime(date.Year, Month, Day).AddDays(-1);
            return (Day == date.Day && Month == date.Month) || date.Date == dayBefore.Date;
        }
    }

}