using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace OpenEvents.Backend.Common.Queries
{
    public interface IQuery<TResult>
    {

        Task<IList<TResult>> Execute();

    }
    
}
