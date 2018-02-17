﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;

namespace OpenEvents.Backend.Invoicing.Messaging
{
    public class OrderCreatedSubscription : Subscription<OrderCreated, OrderCreatedEventHandler>
    {
        public OrderCreatedSubscription(ServiceBusConfiguration config, ServiceBusProvisioningService serviceBusProvisioningService, IServiceScopeFactory serviceScopeFactory) : base(config, serviceBusProvisioningService, serviceScopeFactory)
        {
        }

        public override string TopicName => nameof(OrderCreated);

        public override string SubscriptionName => nameof(OrderCreated) + nameof(Invoicing);
    }
}
