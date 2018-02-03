using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Common.Queries
{
    public interface IPageableQuery<TResult>
    {

        int? Skip { get; set; }

        int? Take { get; set; }

        Task<int> GetTotalItemsCount();

    }
}