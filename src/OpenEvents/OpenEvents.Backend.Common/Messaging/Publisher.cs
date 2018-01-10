using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using OpenEvents.Backend.Common.Configuration;

namespace OpenEvents.Backend.Common.Messaging
{
    public abstract class Publisher<TEvent> : IPublisher<TEvent>
    {
        private readonly ServiceBusConfiguration config;
        private readonly ServiceBusProvisioningService serviceBusProvisioningService;

        private object locker = new object();
        private bool isInitialized = false;


        private TopicClient topicClient;


        public abstract string TopicName { get; }

        protected virtual TimeSpan MaxAutoRenewDuration => TimeSpan.FromMinutes(1);



        public Publisher(ServiceBusConfiguration config, ServiceBusProvisioningService serviceBusProvisioningService)
        {
            this.config = config;
            this.serviceBusProvisioningService = serviceBusProvisioningService;
        }

        private string GetTopicName()
        {
            return config.Environment + "-" + TopicName;
        }

        private async Task Initialize()
        {
            if (isInitialized) return;
            
            await serviceBusProvisioningService.EnsureTopicExists(GetTopicName());
            topicClient = new TopicClient(config.ConnectionString, GetTopicName());

            isInitialized = true;
        }

        public async Task PublishEvent(TEvent data)
        {
            await Initialize();

            var json = JsonConvert.SerializeObject(data);
            await topicClient.SendAsync(new Message(Encoding.UTF8.GetBytes(json)));
        }

    }
}
