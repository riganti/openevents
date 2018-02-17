using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenEvents.Backend.Common.Configuration;

namespace OpenEvents.Backend.Common.Messaging
{
    public abstract class Subscription<TEvent, THandler> : ISubscription<TEvent> where THandler : IEventHandler<TEvent>
    {
        private readonly ServiceBusConfiguration config;
        private readonly ServiceBusProvisioningService serviceBusProvisioningService;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private SubscriptionClient subscriptionClient;


        public abstract string TopicName { get; }

        public abstract string SubscriptionName { get; }

        protected virtual TimeSpan MaxAutoRenewDuration => TimeSpan.FromMinutes(1);



        public Subscription(ServiceBusConfiguration config, ServiceBusProvisioningService serviceBusProvisioningService, IServiceScopeFactory serviceScopeFactory)
        {
            this.config = config;
            this.serviceBusProvisioningService = serviceBusProvisioningService;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        private string GetSubscriptionName()
        {
            return config.Environment + "-" + SubscriptionName;
        }

        private string GetTopicName()
        {
            return config.Environment + "-" + TopicName;
        }

        public async Task Initialize()
        {
            await serviceBusProvisioningService.EnsureTopicAndSubscriptionExists(GetTopicName(), GetSubscriptionName());

            subscriptionClient = new SubscriptionClient(config.ConnectionString, GetTopicName(), GetSubscriptionName());
            subscriptionClient.RegisterMessageHandler(OnMessage, new MessageHandlerOptions(OnException)
            {
                AutoComplete = true,
                MaxConcurrentCalls = 1,
                MaxAutoRenewDuration = MaxAutoRenewDuration
            });
        }

        protected async Task OnMessage(Message message, CancellationToken cancellationToken)
        {
            var json = Encoding.UTF8.GetString(message.Body);
            var data = JsonConvert.DeserializeObject<TEvent>(json);

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<THandler>();
                await handler.ProcessEvent(data);
            }
        }

        protected virtual Task OnException(ExceptionReceivedEventArgs ex)
        {
            // TODO: logging
            return Task.CompletedTask;
        }
    }
}
