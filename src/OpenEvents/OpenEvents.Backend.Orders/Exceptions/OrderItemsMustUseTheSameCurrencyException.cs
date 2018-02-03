using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Orders.Exceptions
{
    public class OrderItemsMustUseTheSameCurrencyException : ConflictException
    {

        public OrderItemsMustUseTheSameCurrencyException() : base("Order items must use the same currency!")
        {
        }

    }
}
