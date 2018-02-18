using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Facades;
using OpenEvents.Backend.Orders.Common;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Queries;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrdersFacade : CrudFacadeBase<Order, OrderDTO, OrderFilterDTO>
    {
        
        public OrdersFacade(IMongoCollection<Order> collection, Func<OrderListQuery> queryFactory) : base(collection, queryFactory)
        {
        }

        public async Task<OrderDTO> UpdateAddress(string id, AddressDTO address)
        {
            await collection.ChangeOneSafeAsync(id, order =>
            {
                order.BillingAddress = Mapper.Map<Address>(address); 
            });

            return await GetById(id);
        }

        public async Task<OrderDTO> UpdateCustomerData(string id, OrderCustomerDataDTO customerData)
        {
            await collection.ChangeOneSafeAsync(id, order =>
            {
                order.CustomerData = Mapper.Map<OrderCustomerData>(customerData);
            });

            return await GetById(id);
        }

        public async Task<OrderDTO> UpdatePaidDate(string id, DateTime? paidDate)
        {
            await collection.ChangeOneSafeAsync(id, order =>
            {
                order.PaymentData.PaidDate = paidDate;
            });

            return await GetById(id);
        }

        public async Task AddDocument(string id, OrderDocumentType type, string url)
        {
            await collection.ChangeOneSafeAsync(id, order =>
            {
                order.OrderDocuments.Add(new OrderDocument()
                {
                    CreatedDate = DateTime.UtcNow,
                    Type = type,
                    Url = url
                });
            });
        }

        public async Task RemoveDocument(string id, string url)
        {
            await collection.ChangeOneSafeAsync(id, order =>
            {
                order.OrderDocuments.RemoveAll(d => d.Url == url);
            });
        }
    }
}
