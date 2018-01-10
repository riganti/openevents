using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Common.Messaging
{
    public interface IPublisher<TEvent>
    {

        Task PublishEvent(TEvent data);

    }
}