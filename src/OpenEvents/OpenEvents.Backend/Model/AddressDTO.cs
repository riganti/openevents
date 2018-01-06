using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Model
{
    public class AddressDTO
    {
        public string Name { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string ZIP { get; set; }

        public string State { get; set; }

        public string CountryCode { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public string VatNumber { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }
    }
}