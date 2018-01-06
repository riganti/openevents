using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Orders.Model;

namespace OpenEvents.Backend.Orders.Services
{
    public class CzechRepublicVatRateProvider : IVatRateProvider
    {
        public decimal GetVatRate(DateTime date, CalculateAddressDTO address)
        {
            if (address.CountryCode != "CZ" && !string.IsNullOrEmpty(address.VatNumber))
            {
                return 1m;
            }

            return 1.21m;
        }
        
    }
}