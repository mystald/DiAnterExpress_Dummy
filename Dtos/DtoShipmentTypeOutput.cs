using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiAnterExpressDummy.Dtos
{
    public class DtoShipmentTypeOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CostPerKg { get; set; }
        public double CostPerKm { get; set; }
        public int DeliveryTimePerKm { get; set; }
    }
}