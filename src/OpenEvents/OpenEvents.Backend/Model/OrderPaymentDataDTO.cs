using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Model
{
    public class OrderPaymentDataDTO
    {

        public DateTime VatDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public List<OrderPaymentDTO> Payments { get; set; } = new List<OrderPaymentDTO>();

    }
}