using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace congestion.calculator.Domain
{
    public class Car : Vehicle
    {
        public string GetVehicleType() => nameof(Car);
    }
}