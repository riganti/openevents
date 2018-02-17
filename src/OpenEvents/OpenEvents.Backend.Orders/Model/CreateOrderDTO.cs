using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class CreateOrderDTO : IValidatableObject
    {
        public string LanguageCode { get; set; }

        public string DiscountCode { get; set; }

        public AddressDTO BillingAddress { get; set; } = new AddressDTO();

        public OrderCustomerDataDTO CustomerData { get; set; } = new OrderCustomerDataDTO();
        
        public List<CreateOrderItemDTO> OrderItems { get; set; } = new List<CreateOrderItemDTO>();
        
        public List<ExtensionDataDTO> ExtensionData { get; set; } = new List<ExtensionDataDTO>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!OrderItems.Any())
            {
                yield return new ValidationResult("The order must have at least one item!", new [] { nameof(OrderItems) });
            }
        }
    }
}