using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Orders.Exceptions
{
    public class InvalidVATException : ConflictException
    {

        public InvalidVATException() : base((string) "The VAT is not valid!")
        {
        }

    }
}