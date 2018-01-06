using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common;

namespace OpenEvents.Backend.Model
{
    public class OrderPaymentDTO
    {

        public string Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public OrderPaymentMethod Method { get; set; }

    }
}