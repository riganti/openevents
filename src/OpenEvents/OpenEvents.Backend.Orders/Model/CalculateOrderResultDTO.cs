using System.Collections.Generic;
using OpenEvents.Backend.Orders.Data;

namespace OpenEvents.Backend.Orders.Model
{
    public class CalculateOrderResultDTO
    {
        public PriceDataDTO TotalPrice { get; set; }

        public List<PriceDataDTO> OrderItemPrices { get; set; }

    }
}