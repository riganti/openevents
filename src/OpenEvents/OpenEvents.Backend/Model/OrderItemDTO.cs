using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common;

namespace OpenEvents.Backend.Model
{
    public class OrderItemDTO
    {

        public string Sku { get; set; }

        public OrderItemType Type { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PriceDataDTO Price { get; set; } = new PriceDataDTO();

        public List<ExtensionDataDTO> ExtensionData { get; set; } = new List<ExtensionDataDTO>();

        public string EventRegistrationId { get; set; }

    }
}