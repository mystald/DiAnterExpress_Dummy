using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiAnterExpressDummy.Dtos
{
    public class DtoShipmentCreateTokopodia
    {
        [Required]
        public int transactionId { get; set; }
        public string senderName { get; set; }
        public string senderContact { get; set; }
        [Required]
        public double senderLat { get; set; }
        [Required]
        public double senderLong { get; set; }
        public string receiverName { get; set; }
        public string receiverContact { get; set; }
        [Required]
        public double receiverLat { get; set; }
        [Required]
        public double receiverLong { get; set; }
        [Required]
        public double totalWeight { get; set; }
        [Required]
        public int shipmentTypeId { get; set; }
    }

    public class DtoShipmentCreateReturn
    {
        public int shipmentId { get; set; }
        public string statusOrder { get; set; }
    }
}