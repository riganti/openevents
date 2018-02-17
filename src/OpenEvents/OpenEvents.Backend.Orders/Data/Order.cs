using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common.Data;

namespace OpenEvents.Backend.Orders.Data
{
    public class Order : IIdentifiable, IVersionedEntity
    {

        public string Id { get; set; }

        public string ETag { get; set; }

        public string EventId { get; set; }

        public string EventTitle { get; set; }

        public DateTime CreatedDate { get; set; }

        public Address BillingAddress { get; set; }

        public OrderCustomerData CustomerData { get; set; } = new OrderCustomerData();

        public OrderPaymentData PaymentData { get; set; } = new OrderPaymentData();

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public PriceData TotalPrice { get; set; } = new PriceData();

        public List<OrderDocument> OrderDocuments { get; set; } = new List<OrderDocument>();

        public DateTime? CanceledDate { get; set; }

        public string DiscountCode { get; set; }

        public string ReplacedByOrderId { get; set; }

        public List<ExtensionData> ExtensionData { get; set; } = new List<ExtensionData>();
    }
}