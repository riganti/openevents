using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Data
{
    public class PriceData
    {

        public decimal BasePrice { get; set; }

        public decimal DiscountPercent { get; set; }

        public decimal Price { get; set; }

        public decimal VatRate { get; set; }

        public decimal PriceInclVat { get; set; }

        public string CurrencyCode { get; set; }
    }
}