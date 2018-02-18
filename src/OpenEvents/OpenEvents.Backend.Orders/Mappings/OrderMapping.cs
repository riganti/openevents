using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OpenEvents.Backend.Common.Mappings;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Model;

namespace OpenEvents.Backend.Orders.Mappings
{
    public class OrderMapping : IMapping
    {

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Order, OrderDTO>();
            mapper.CreateMap<OrderCustomerData, OrderCustomerDataDTO>();
            mapper.CreateMap<OrderDocument, OrderDocumentDTO>();
            mapper.CreateMap<OrderItem, OrderItemDTO>();
            mapper.CreateMap<OrderPaymentData, OrderPaymentDataDTO>();

            mapper.CreateMap<PriceData, PriceDataDTO>();
            mapper.CreateMap<ExtensionData, ExtensionDataDTO>();

            mapper.CreateMap<CreateOrderDTO, Order>()
                .ForMember(s => s.Id, m => m.Ignore())
                .ForMember(s => s.ETag, m => m.Ignore())
                .ForMember(s => s.EventId, m => m.Ignore())
                .ForMember(s => s.EventTitle, m => m.Ignore())
                .ForMember(s => s.CreatedDate, m => m.Ignore())
                .ForMember(s => s.PaymentData, m => m.Ignore())
                .ForMember(s => s.TotalPrice, m => m.Ignore())
                .ForMember(s => s.OrderDocuments, m => m.Ignore())
                .ForMember(s => s.CanceledDate, m => m.Ignore())
                .ForMember(s => s.ReplacedByOrderId, m => m.Ignore());

            mapper.CreateMap<CreateOrderItemDTO, OrderItem>()
                .ForMember(s => s.Type, m => m.Ignore())
                .ForMember(s => s.Description, m => m.Ignore())
                .ForMember(s => s.Price, m => m.Ignore())
                .ForMember(s => s.EventRegistrationId, m => m.Ignore())
                .ForMember(s => s.Amount, m => m.UseValue(1));
        }

    }
}