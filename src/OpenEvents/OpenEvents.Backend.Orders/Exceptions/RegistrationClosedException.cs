using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Orders.Exceptions
{
    public class RegistrationClosedException : ConflictException
    {

        public RegistrationClosedException() : base("Registration is closed!")
        {
        }

    }
}
