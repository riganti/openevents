using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;

namespace OpenEvents.Backend.Orders.Messaging
{
    public class EventChangedSubscription : Subscription<EventChanged>
    {
        public EventChangedSubscription(ServiceBusConfiguration config, ServiceBusProvisioningService serviceBusProvisioningService, IServiceScopeFactory serviceScopeFactory) : base(config, serviceBusProvisioningService, serviceScopeFactory)
        {
        }

        public override string TopicName => nameof(EventChanged);

        public override string SubscriptionName => nameof(EventChanged) + nameof(Orders);
    }
}