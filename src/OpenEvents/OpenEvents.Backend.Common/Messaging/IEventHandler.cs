using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Common.Messaging
{
    public interface IEventHandler<TEvent>
    {

        Task ProcessEvent(TEvent data);

    }
}
