using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Common.Data
{
    public interface IVersionedEntity
    {

        string ETag { get; set; }

    }
}