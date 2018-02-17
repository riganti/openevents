using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Common
{
    public enum OrderItemType
    {
        CancellationFee = 0,
        EventPrice = 1,
        Discount = 2
    }
}