using congestion.calculator.Domain;
using System;
using System.Collections.Generic;


public interface ICongestionTaxCalculator
{
    public int GetTax(Vehicle vehicle, DateTime[] dates);
}


public interface BaeCongestionTaxCalculator: ICongestionTaxCalculator
{
    public int GetTax(Vehicle vehicle, DateTime[] dates);
}