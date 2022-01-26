using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiAnterExpressDummy.Models
{
    public class ShipmentType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double CostPerKg { get; set; }
        [Required]
        public double CostPerKm { get; set; }
        [Required]
        public int DeliveryTimePerKm { get; set; }

        public ICollection<Shipment> Shipment { get; set; }
    }
}