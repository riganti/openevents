using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Orders.Model;

namespace OpenEvents.Backend.Orders.Services
{
    public interface IVatNumberValidator
    { 

        bool IsValidVat(CalculateAddressDTO address);

    }
}