using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Data
{
    public interface IIdentifiable
    {

        string Id { get; set; }

    }

    public interface IVersionedEntity
    {

        string ETag { get; set; }

    }
}
