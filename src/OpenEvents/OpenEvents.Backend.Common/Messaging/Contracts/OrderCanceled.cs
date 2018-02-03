using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Common.Messaging.Contracts
{
    public class OrderCanceled
    {

        public string OrderId { get; set; }

    }
}