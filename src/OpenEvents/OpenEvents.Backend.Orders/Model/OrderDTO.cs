using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class OrderDTO
    {

        public string Id { get; set; }

        public string EventId { get; set; }

        public string EventTitle { get; set; }

        public DateTime CreatedDate { get; set; }

        public AddressDTO BillingAddress { get; set; } = new AddressDTO();

        public OrderCustomerDataDTO CustomerData { get; set; } = new OrderCustomerDataDTO();

        public OrderPaymentDataDTO PaymentData { get; set; } = new OrderPaymentDataDTO();

        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();

        public PriceDataDTO TotalPrice { get; set; } = new PriceDataDTO();

        public List<OrderDocumentDTO> OrderDocuments { get; set; } = new List<OrderDocumentDTO>();

        public DateTime? CanceledDate { get; set; }

        public string ReplacedByOrderId { get; set; }

        public List<ExtensionDataDTO> ExtensionData { get; set; } = new List<ExtensionDataDTO>();

    }
}