using System;
using System.Collections.Generic;
using System.Linq;
using congestion.calculator.Domain;

public  class GothenburgCongestionTaxCalculator : ICongestionTaxCalculator
{
    //in real project we inject this
    DatabaseMock _databaseMock = new DatabaseMock();

    const int LinitFeePerHour = 60;

    /**
    * Calculate the total toll fee for one day
    *
    * @param vehicle - the vehicle
    * @param dates   - date and time of all passes on one day
    * @return - the total congestion tax for that day
    */
    public int GetTax(Vehicle vehicle, DateTime[] dates)
    {
        dates = dates.OrderBy(x => x).ToArray();
        List<(DateTime dt, int fee)> calculatedFee = new List<(DateTime dt, int fee)>();
        foreach (var date in dates)
        {
            var fee = GetTollFee(date, vehicle);
            var lastFee = calculatedFee.Last();

            if (calculatedFee.Count > 0 && (lastFee.dt - date).TotalMinutes < 60)
            {
                if (fee > lastFee.fee)
                    lastFee.fee = fee;
            }
            else
                calculatedFee.Add((date, fee));
        }

        return Math.Min(LinitFeePerHour, calculatedFee.Sum(x=>x.fee));


        //DateTime intervalStart = dates[0];
        //int totalFee = 0;
        //foreach (DateTime date in dates)
        //{
        //    int nextFee = GetTollFee(date, vehicle);
        //    int tempFee = GetTollFee(intervalStart, vehicle);

        //    long diffInMillies = date.Millisecond - intervalStart.Millisecond;
        //    long minutes = diffInMillies / 1000 / 60;

        //    if (minutes <= 60)
        //    {
        //        if (totalFee > 0) totalFee -= tempFee;
        //        if (nextFee >= tempFee) tempFee = nextFee;
        //        totalFee += tempFee;
        //    }
        //    else
        //    {
        //        totalFee += nextFee;
        //    }
        //}
        //if (totalFee > 60) totalFee = 60;
        //return totalFee;
    }



    private int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        var tollFee = _databaseMock.TollPriceConfigs.FirstOrDefault(x => x.Hour == hour && x.FromMinute <= minute && x.ToMinute >= minute);
        return tollFee?.Fee ?? 0;

        //if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        //else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        //else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        //else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        //else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        //else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        //else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        //else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        //else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        //else return 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        if (date.Year != 2013) return false;
        if (date.Month == Months.July) return true;
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (_databaseMock.Holidaies.Any(x => x.IsHolidayOrDateBefore(date)))
            return true;

        //if (month == 1 && day == 1 ||
        //    month == 3 && (day == 28 || day == 29) ||
        //    month == 4 && (day == 1 || day == 30) ||
        //    month == 5 && (day == 1 || day == 8 || day == 9) ||
        //    month == 6 && (day == 5 || day == 6 || day == 21) ||
        //    month == Months.July ||
        //    month == 11 && day == 1 ||
        //    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
        //{
        //    return true;
        //}

        return false;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return Enum.GetValues(typeof(TollFreeVehicles)).Cast<TollFreeVehicles>().Any(v => v.ToString() == vehicleType);

        //vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
        //   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
        //   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
        //   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
        //   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
        //   vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }
}
