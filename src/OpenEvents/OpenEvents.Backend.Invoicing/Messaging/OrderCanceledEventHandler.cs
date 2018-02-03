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
    public class OrderCanceledEventHandler : IEventHandler<OrderCanceled>
    {
        private readonly InvoicingService invoicingService;
        private readonly IOrdersApi ordersApi;

        public OrderCanceledEventHandler(InvoicingService invoicingService, IOrdersApi ordersApi)
        {
            this.invoicingService = invoicingService;
            this.ordersApi = ordersApi;
        }

        public async Task ProcessEvent(OrderCanceled data)
        {
            var order = await ordersApi.ApiOrdersByOrderIdGetAsync(data.OrderId);
            await invoicingService.CreateCreditNoteForCanceledOrder(order);
        }
    }
}