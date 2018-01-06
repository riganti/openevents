using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenEvents.Backend.Orders.Model
{
    public class AddressDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ZIP { get; set; }

        public string State { get; set; }

        [Required]
        public string CountryCode { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public string VatNumber { get; set; }

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [Required]
        public string ContactPhone { get; set; }
    }
}