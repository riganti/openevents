using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;

namespace OpenEvents.Backend.Invoicing.Messaging
{
    public class OrderChangedSubscription : Subscription<OrderChanged>
    {
        public OrderChangedSubscription(ServiceBusConfiguration config, ServiceBusProvisioningService serviceBusProvisioningService, IServiceScopeFactory serviceScopeFactory) : base(config, serviceBusProvisioningService, serviceScopeFactory)
        {
        }

        public override string TopicName => nameof(OrderChanged);

        public override string SubscriptionName => nameof(OrderChanged) + nameof(Invoicing);
    }
}