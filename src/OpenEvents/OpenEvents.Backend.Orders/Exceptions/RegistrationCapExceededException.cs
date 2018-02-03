using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Orders.Exceptions
{
    public class RegistrationCapExceededException : ConflictException
    {

        public RegistrationCapExceededException() : base("Registration cap exceeded!")
        {
        }

    }
}