using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Common.Messaging
{
    public interface ISubscription<TEvent> : IAppInitializerTask
    {
    }
}