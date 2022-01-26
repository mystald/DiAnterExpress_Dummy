using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DiAnterExpressDummy.Dtos
{
    public class ShipmentFeeInput
    {
        public double SenderLat { get; set; }
        public double SenderLong { get; set; }
        public double ReceiverLat { get; set; }
        public double ReceiverLong { get; set; }
        public double Weight { get; set; }
        public int ShipmentTypeId { get; set; }
    }

    public class ShipmentFeeAllInput
    {
        public double SenderLat { get; set; }
        public double SenderLong { get; set; }
        public double ReceiverLat { get; set; }
        public double ReceiverLong { get; set; }
        public double Weight { get; set; }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}