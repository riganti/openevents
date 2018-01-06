using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class CalculateAddressDTO
    {

        [Required]
        public string CountryCode { get; set; }
        
        public string VatNumber { get; set; }

    }
}