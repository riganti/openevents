using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Orders.Model;

namespace OpenEvents.Backend.Orders.Services
{
    public interface IVatRateProvider
    {

        decimal GetVatRate(DateTime date, CalculateAddressDTO address);

    }
}
