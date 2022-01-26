using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiAnterExpressDummy.Dtos;
using DiAnterExpressDummy.Models;

namespace DiAnterExpressDummy.Data
{
    public interface IShipment : ICrud<Shipment>
    {
        public Task<double> GetShipmentFee(ShipmentFeeInput input, double costPerKm, double costPerKg);
        int GetShipmentDeliveryTime(ShipmentFeeInput input, int deliveryTimePerKm);
    }
}