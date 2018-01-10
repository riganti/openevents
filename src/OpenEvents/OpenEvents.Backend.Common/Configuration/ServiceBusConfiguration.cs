using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEvents.Backend.Common.Configuration
{
    public class ServiceBusConfiguration
    {

        public string ConnectionString { get; set; }

        public string Environment { get; set; }


        public string ResourceGroup { get; set; }

        public string NamespaceName { get; set; }


        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string SubscriptionId { get; set; }

        public string TenantId { get; set; }
    }
}
