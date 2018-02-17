using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEvents.Backend.Common.Messaging.Contracts
{
    public class OrderCreated
    {

        public string OrderId { get; set; }

        public string EventId { get; set; }

    }
}
