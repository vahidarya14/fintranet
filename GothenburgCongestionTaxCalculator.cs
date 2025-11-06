using System;
using System.Collections.Generic;
using System.Linq;
using congestion.calculator.Domain;

public class GothenburgCongestionTaxCalculator : ICongestionTaxCalculator
{
    private readonly IDatabase _database;

    public GothenburgCongestionTaxCalculator(IDatabase database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }

    const int LimitFeePerHour = 60;

    private class FeeEntry
    {
        public DateTime Dt { get; set; }
        public int Fee { get; set; }
    }

    private static readonly HashSet<string> TollFreeVehicleTypes = [.. Enum.GetNames(typeof(TollFreeVehicles))];


    public int GetTax(Vehicle vehicle, DateTime[] dates)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
        if (dates == null) throw new ArgumentNullException(nameof(dates));
        if (dates.Length == 0) return 0;

        dates = [.. dates.OrderBy(x => x)];
        List<FeeEntry> calculatedFee = [];
        for (var i = 0; i < dates.Length; i++)
        {
            var date = dates[i];
            var fee = GetTollFee(date, vehicle);

            if (calculatedFee.Count > 0 && (calculatedFee.Last().Dt - date).TotalMinutes < 60)
            {
                if (fee > calculatedFee.Last().Fee)
                {
                    calculatedFee.Last().Fee = fee;
                }
            }
            else
            {
                calculatedFee.Add(new FeeEntry { Dt = date, Fee = fee });
            }
        }

        return Math.Min(LimitFeePerHour, calculatedFee.Sum(x => x.Fee));
    }



    private int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        var tollFee = _database.TollPriceConfigs.FirstOrDefault(x => x.Hour == hour && x.FromMinute <= minute && x.ToMinute >= minute);
        return tollFee?.Fee ?? 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        if (date.Month == Months.July) return true;
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (_database.Holidaies.Any(x => x.IsHolidayOrDateBefore(date)))
            return true;

        return false;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        string vehicleType = vehicle.GetVehicleType();
        return TollFreeVehicleTypes.Contains(vehicleType);
    }
}
