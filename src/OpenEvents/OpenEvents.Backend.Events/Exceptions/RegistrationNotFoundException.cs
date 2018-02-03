using System;
using System.Collections.Generic;
using System.Linq;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Events.Exceptions
{
    public class RegistrationNotFoundException : EntityNotFoundException
    {

        public RegistrationNotFoundException() : base("Registration not found!")
        {
        }

    }
}