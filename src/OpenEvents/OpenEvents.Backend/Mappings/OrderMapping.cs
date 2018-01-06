using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OpenEvents.Backend.Data;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Mappings
{
    public class OrderMapping
    {

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Order, OrderDTO>();
            mapper.CreateMap<OrderCustomerData, OrderCustomerDataDTO>();
            mapper.CreateMap<OrderDocument, OrderDocumentDTO>();
            mapper.CreateMap<OrderItem, OrderItemDTO>();
            mapper.CreateMap<OrderPayment, OrderPaymentDTO>();
            mapper.CreateMap<OrderPaymentData, OrderPaymentDataDTO>();
            mapper.CreateMap<PriceData, PriceDataDTO>();
            mapper.CreateMap<ExtensionData, ExtensionDataDTO>();

            mapper.CreateMap<CreateOrderDTO, Order>();
            mapper.CreateMap<CreateOrderItemDTO, OrderItem>();
        }

    }
}