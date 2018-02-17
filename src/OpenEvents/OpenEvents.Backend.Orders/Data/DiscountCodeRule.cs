using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Data
{
    public class DiscountCodeRule
    {

        public string[] ApplicableSku { get; set; }

        public decimal MaxAmount { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? DiscountPercent { get; set; }

    }
}