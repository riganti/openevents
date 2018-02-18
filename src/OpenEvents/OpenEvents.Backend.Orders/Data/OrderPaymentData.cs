using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Data
{
    public class OrderPaymentData
    {

        public DateTime VatDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? PaidDate { get; set; }
        
    }
    
}