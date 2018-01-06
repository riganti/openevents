using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Data
{
    public class OrderItem
    {

        public string Sku { get; set; }

        public OrderItemType Type { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PriceData Price { get; set; } = new PriceData();

        public List<ExtensionData> ExtensionData { get; set; } = new List<ExtensionData>();

        public string EventRegistrationId { get; set; }

    }
}