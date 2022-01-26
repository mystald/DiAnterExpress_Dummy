using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiAnterExpressDummy.Dtos
{
    public class DtoShipmentFeeAllOutput
    {
        public int ShipmentTypeId { get; set; }
        public string Name { get; set; }
        public double TotalCost { get; set; }
        public int DeliveryTime { get; set; }
    }
}