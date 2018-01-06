using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Services
{
    public class CzechRepublicVatRateProvider : IVatRateProvider
    {
        public decimal GetVatRate(CalculateAddressDTO address)
        {
            if (address.CountryCode != "CZ" && !string.IsNullOrEmpty(address.VatNumber))
            {
                return 1m;
            }

            return 1.21m;
        }
        
    }
}