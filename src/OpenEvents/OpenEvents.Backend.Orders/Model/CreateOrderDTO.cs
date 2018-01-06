using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class CreateOrderDTO
    {
        
        public AddressDTO BillingAddress { get; set; } = new AddressDTO();

        public OrderCustomerDataDTO CustomerData { get; set; } = new OrderCustomerDataDTO();
        
        public List<CreateOrderItemDTO> OrderItems { get; set; } = new List<CreateOrderItemDTO>();
        
        public List<ExtensionDataDTO> ExtensionData { get; set; } = new List<ExtensionDataDTO>();

    }
}