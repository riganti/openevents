using System.Collections.Generic;

namespace OpenEvents.Backend.Model
{
    public class CalculateOrderDTO
    {

        public CalculateAddressDTO Address { get; set; } = new CalculateAddressDTO();

        public List<CalculateOrderItemDTO> OrderItems { get; set; } = new List<CalculateOrderItemDTO>();

    }
}