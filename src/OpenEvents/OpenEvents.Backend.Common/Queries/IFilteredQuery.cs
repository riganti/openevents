using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenEvents.Backend.Common.Queries
{
    public interface IFilteredQuery<TResult, TFilter> : IQuery<TResult>
    {

        TFilter Filter { get; set; }

    }
}