using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Rest;
using OpenEvents.Backend.Common.Configuration;

namespace OpenEvents.Backend.Common.Messaging
{
    public class ServiceBusProvisioningService
    {
        private readonly IServiceBusNamespace serviceBusNamespace;

        public ServiceBusProvisioningService(AzureCredentials credentials, ServiceBusConfiguration config)
        {
            var serviceBusManager = ServiceBusManager.Authenticate(credentials, config.SubscriptionId);
            serviceBusNamespace = serviceBusManager.Namespaces.GetByResourceGroup(config.ResourceGroup, config.NamespaceName);
        }

        public async Task<ITopic> EnsureTopicExists(string topicName)
        {
            var topics = await serviceBusNamespace.Topics.ListAsync();
            var topic = topics.FirstOrDefault(t => t.Name == topicName);

            if (topic == null)
            {
                topic = await serviceBusNamespace.Topics.Define(topicName).CreateAsync();
            }

            return topic;
        }

        public async Task<ISubscription> EnsureTopicAndSubscriptionExists(string topicName, string subscriptionName)
        {
            var topic = await EnsureTopicExists(topicName);
            
            var subscriptions = await topic.Subscriptions.ListAsync();
            var subscription = subscriptions.FirstOrDefault(s => s.Name == subscriptionName);

            if (subscription == null)
            {
                await topic.Subscriptions.Define(subscriptionName).CreateAsync();
            }

            return subscription;
        }
    }
}
