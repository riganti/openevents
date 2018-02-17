using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Client;

namespace OpenEvents.Backend.Invoicing.Services
{
    public class InvoicingService
    {

        public Task CreateInvoiceForNewOrder(OrderDTO order)
        {
            return CreateInvoice(order, order.TotalPrice);
        }

        public async Task CreateInvoiceForChangedOrder(OrderDTO oldOrder, OrderDTO newOrder)
        {
            // discount or VAT changed, generate credit note and new invoice
            if (oldOrder.TotalPrice.VatRate != newOrder.TotalPrice.VatRate)
            {
                await CreateCreditNote(oldOrder, oldOrder.TotalPrice);
                await CreateInvoice(newOrder, newOrder.TotalPrice);
            }
            else if (oldOrder.TotalPrice.Price < newOrder.TotalPrice.Price)
            {
                // generate an invoice for the amount difference
                var difference = new PriceDataDTO()
                {
                    VatRate = newOrder.TotalPrice.Price,
                    BasePrice = newOrder.TotalPrice.BasePrice - oldOrder.TotalPrice.BasePrice,
                    Price = newOrder.TotalPrice.Price - oldOrder.TotalPrice.Price,
                    PriceInclVat = newOrder.TotalPrice.Price - oldOrder.TotalPrice.Price
                };
                await CreateInvoice(newOrder, difference);
            }
            else if (oldOrder.TotalPrice.Price > newOrder.TotalPrice.Price)
            {
                // generate a credit note for the amount difference
                var difference = new PriceDataDTO()
                {
                    VatRate = newOrder.TotalPrice.Price,
                    BasePrice = oldOrder.TotalPrice.BasePrice - newOrder.TotalPrice.BasePrice,
                    Price = oldOrder.TotalPrice.Price - newOrder.TotalPrice.Price,
                    PriceInclVat = oldOrder.TotalPrice.Price - newOrder.TotalPrice.Price
                };
                await CreateCreditNote(newOrder, difference);
            }
        }

        public Task CreateCreditNoteForCanceledOrder(OrderDTO order)
        {
            return CreateCreditNote(order, order.TotalPrice);
        }


        private async Task CreateInvoice(OrderDTO order, PriceDataDTO price)
        {
            throw new NotImplementedException();
        }

        private async Task CreateCreditNote(OrderDTO order, PriceDataDTO price)
        {
            throw new NotImplementedException();
        }

    }
}
