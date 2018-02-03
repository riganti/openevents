using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Exceptions;

namespace OpenEvents.Backend.Events.Exceptions
{
    public class EventNotFoundException : EntityNotFoundException
    {

        public EventNotFoundException() : base("Event not found!")
        {
        }

    }
}
