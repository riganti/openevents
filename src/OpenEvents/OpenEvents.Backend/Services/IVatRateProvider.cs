using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Services
{
    public interface IVatRateProvider
    {

        decimal GetVatRate(CalculateAddressDTO address);

    }
}
