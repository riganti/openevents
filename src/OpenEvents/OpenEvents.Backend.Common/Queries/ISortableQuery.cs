using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OpenEvents.Backend.Common.Queries
{
    public interface ISortableQuery<TResult>
    {

        IList<(Expression<Func<TResult, object>> expression, bool descending)> SortExpressions { get; set; }

    }
}