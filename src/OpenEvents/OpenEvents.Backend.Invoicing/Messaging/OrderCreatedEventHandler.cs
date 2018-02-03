using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;
using OpenEvents.Backend.Invoicing.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Invoicing.Messaging
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreated>
    {
        private readonly InvoicingService invoicingService;
        private readonly IOrdersApi ordersApi;

        public OrderCreatedEventHandler(InvoicingService invoicingService, IOrdersApi ordersApi)
        {
            this.invoicingService = invoicingService;
            this.ordersApi = ordersApi;
        }

        public async Task ProcessEvent(OrderCreated data)
        {
            var order = await ordersApi.ApiOrdersByOrderIdGetAsync(data.OrderId);
            await invoicingService.CreateInvoiceForNewOrder(order);
        }
    }
}
