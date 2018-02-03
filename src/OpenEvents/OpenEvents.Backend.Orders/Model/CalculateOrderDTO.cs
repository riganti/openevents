using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class CalculateOrderDTO
    {

        public CalculateAddressDTO BillingAddress { get; set; } = new CalculateAddressDTO();

        public List<CalculateOrderItemDTO> OrderItems { get; set; } = new List<CalculateOrderItemDTO>();

    }
}