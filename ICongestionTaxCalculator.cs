using congestion.calculator.Domain;
using System;
using System.Collections.Generic;


public interface ICongestionTaxCalculator
{
    public int GetTax(Vehicle vehicle, DateTime[] dates);
}

public interface IDatabase
{
    List<BaseInfoHoliday> Holidaies { get; }
    List<BaseInfoTollPriceConfig> TollPriceConfigs { get; }
}

public interface BaseCongestionTaxCalculator: ICongestionTaxCalculator
{
    public int GetTax(Vehicle vehicle, DateTime[] dates);
}