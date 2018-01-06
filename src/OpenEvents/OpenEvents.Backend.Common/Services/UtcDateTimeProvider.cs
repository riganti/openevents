using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Common.Services
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;

    }
}