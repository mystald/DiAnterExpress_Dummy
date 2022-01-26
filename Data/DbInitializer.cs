using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiAnterExpressDummy.Models;

namespace DiAnterExpressDummy.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.ShipmentTypes.Any())
            {
                context.ShipmentTypes.Add(
                    new ShipmentType
                    {
                        Name = "Di Entar Aja",
                        CostPerKg = 5000,
                        CostPerKm = 100,
                        DeliveryTimePerKm = 12,
                    }
                );
                context.ShipmentTypes.Add(
                    new ShipmentType
                    {
                        Name = "Di Anter",
                        CostPerKg = 5000,
                        CostPerKm = 200,
                        DeliveryTimePerKm = 6,
                    }
                );
                context.ShipmentTypes.Add(
                    new ShipmentType
                    {
                        Name = "Di Anter Super",
                        CostPerKg = 5000,
                        CostPerKm = 500,
                        DeliveryTimePerKm = 3,
                    }
                );

                context.SaveChanges();
            }
        }
    }
}