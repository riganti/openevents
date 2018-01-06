using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Services
{
    public interface IVatValidator
    { 

        bool IsValidVat(CalculateAddressDTO address);

    }
}