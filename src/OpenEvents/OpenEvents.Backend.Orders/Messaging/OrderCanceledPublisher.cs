using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;

namespace OpenEvents.Backend.Orders.Messaging
{
    public class OrderCanceledPublisher : Publisher<OrderCanceled>
    {
        public OrderCanceledPublisher(ServiceBusConfiguration config, ServiceBusProvisioningService serviceBusProvisioningService) : base(config, serviceBusProvisioningService)
        {
        }

        public override string TopicName => nameof(OrderCanceled);
    }
}