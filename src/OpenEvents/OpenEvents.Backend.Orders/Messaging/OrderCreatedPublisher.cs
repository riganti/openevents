using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;

namespace OpenEvents.Backend.Orders.Messaging
{
    public class OrderCreatedPublisher : Publisher<OrderCreated>
    {
        public OrderCreatedPublisher(ServiceBusConfiguration config, ServiceBusProvisioningService serviceBusProvisioningService) : base(config, serviceBusProvisioningService)
        {
        }

        public override string TopicName => nameof(OrderCreated);
    }
}
