using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEvents.Backend.Common.Services
{
    public interface IDateTimeProvider
    {

        DateTime Now { get; }

    }
}
