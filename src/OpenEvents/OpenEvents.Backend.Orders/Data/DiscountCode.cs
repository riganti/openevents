using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Data;

namespace OpenEvents.Backend.Orders.Data
{
    public class DiscountCode : IIdentifiable
    {

        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string OriginOrderId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public List<DiscountCodeRule> Rules { get; set; } = new List<DiscountCodeRule>();
        
        public DateTime? ClaimedDate { get; set; }
        
    }
}
