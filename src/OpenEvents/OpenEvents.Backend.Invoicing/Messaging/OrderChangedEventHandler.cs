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
    public class OrderChangedEventHandler : IEventHandler<OrderChanged>
    {
        private readonly InvoicingService invoicingService;
        private readonly IOrdersApi ordersApi;

        public OrderChangedEventHandler(InvoicingService invoicingService, IOrdersApi ordersApi)
        {
            this.invoicingService = invoicingService;
            this.ordersApi = ordersApi;
        }

        public async Task ProcessEvent(OrderChanged data)
        {
            var oldOrder = await ordersApi.ApiOrdersByOrderIdGetAsync(data.OrderId);
            var newOrder = await ordersApi.ApiOrdersByOrderIdGetAsync(oldOrder.ReplacedByOrderId);

            await invoicingService.CreateInvoiceForChangedOrder(oldOrder, newOrder);
        }
    }
}