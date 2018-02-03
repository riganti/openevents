using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class CreateOrderItemDTO
    {

        [Required]
        public string Sku { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<ExtensionDataDTO> ExtensionData { get; set; } = new List<ExtensionDataDTO>();

    }
}