using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiAnterExpressDummy.Dtos;
using DiAnterExpressDummy.Models;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;

namespace DiAnterExpressDummy.Data
{
    public class DALShipment : IShipment
    {
        private ApplicationDbContext _db;
        public DALShipment(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Shipment>> GetAll()
        {
            var results = await _db.Shipments.ToListAsync();
            return results;
        }

        public async Task<Shipment> GetById(int id)
        {
            var result = await _db.Shipments.Where(s => s.Id == Convert.ToInt32(id)).SingleOrDefaultAsync<Shipment>();
            if (result != null)
                return result;
            else
                throw new Exception("Data tidak ditemukan !");
        }

        public Task<double> GetShipmentFee(ShipmentFeeInput input, double costPerKm, double costPerKg)
        {
            if (input.Weight == 0) return Task.FromResult((double)0);
            var senderLocation = new Point(input.SenderLat, input.SenderLong) { SRID = 4326 };
            var receiverLocation = new Point(input.ReceiverLat, input.ReceiverLong) { SRID = 4326 };
            var distance = Math.Ceiling(senderLocation.Distance(receiverLocation) / 1000);
            var fee = ((distance * costPerKm) + costPerKg) * Math.Ceiling(input.Weight);

            return Task.FromResult(fee % 1000 >= 500 ? fee + 1000 - fee % 1000 : fee - fee % 1000);
        }

        public int GetShipmentDeliveryTime(ShipmentFeeInput input, int deliveryTimePerKm)
        {
            var senderLocation = new Point(input.SenderLat, input.SenderLong) { SRID = 4326 };
            var receiverLocation = new Point(input.ReceiverLat, input.ReceiverLong) { SRID = 4326 };

            var distance = senderLocation.Distance(receiverLocation) == 0 ? 1 : senderLocation.Distance(receiverLocation) / 1000;

            return (int)Math.Ceiling((distance * deliveryTimePerKm) / 24);
        }

        Task<Shipment> ICrud<Shipment>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Shipment> Insert(Shipment obj)
        {
            try
            {
                var shipmentType = await _db.ShipmentTypes.Where(x => x.Id == obj.ShipmentTypeId).FirstOrDefaultAsync();
                if (shipmentType == null)
                {
                    throw new Exception("NotFound", new Exception("shipmentTypeId not found"));
                }

                var cost = await GetShipmentFee(
                    new ShipmentFeeInput
                    {
                        SenderLat = obj.SenderAddress.X,
                        SenderLong = obj.SenderAddress.Y,
                        ReceiverLat = obj.ReceiverAddress.X,
                        ReceiverLong = obj.ReceiverAddress.Y,
                        Weight = obj.TotalWeight,
                    },
                    shipmentType.CostPerKm, shipmentType.CostPerKg
                );

                obj.Cost = cost;

                await _db.Shipments.AddAsync(obj);
                await _db.SaveChangesAsync();

                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public async Task<Shipment> Update(int id, Shipment obj)
        {
            try
            {
                var oldShipment = await GetById(id);

                oldShipment.Status = obj.Status;

                return oldShipment;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}