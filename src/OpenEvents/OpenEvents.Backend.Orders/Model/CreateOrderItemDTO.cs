using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class CreateOrderItemDTO
    {

        public string Sku { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<ExtensionDataDTO> ExtensionData { get; set; } = new List<ExtensionDataDTO>();

    }
}