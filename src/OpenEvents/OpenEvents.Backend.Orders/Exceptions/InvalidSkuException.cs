using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Orders.Exceptions
{
    public class InvalidSkuException : ConflictException
    {

        public InvalidSkuException() : base("Invalid SKU!")
        {
            
        }

    }
}
