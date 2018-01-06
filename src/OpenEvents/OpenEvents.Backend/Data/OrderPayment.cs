using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Data
{
    public class OrderPayment
    {

        public string Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public OrderPaymentMethod Method { get; set; }

    }
}