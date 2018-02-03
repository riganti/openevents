using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace OpenEvents.Backend.Common.Queries
{
    public abstract class MongoCollectionQuery<TResult, TEntity> : ISortableQuery<TEntity>, IPageableQuery<TEntity>, IQuery<TResult>
    {
        private readonly IMongoCollection<TEntity> collection;

        public IList<(Expression<Func<TEntity, object>> expression, bool descending)> SortExpressions { get; set; } = new List<(Expression<Func<TEntity, object>> expression, bool descending)>();

        public int? Skip { get; set; }

        public int? Take { get; set; }

        protected abstract IQueryable<TEntity> GetQueryable(IMongoQueryable<TEntity> collection);

        public MongoCollectionQuery(IMongoCollection<TEntity> collection)
        {
            this.collection = collection;
        }

        public virtual async Task<IList<TResult>> Execute()
        {
            var query = GetQueryable(collection.AsQueryable());

            foreach (var expression in SortExpressions)
            {
                if (expression.descending)
                {
                    query = query.OrderByDescending(expression.expression);
                }
                else
                {
                    query = query.OrderBy(expression.expression);
                }
            }

            if (Skip != null)
            {
                query = query.Skip(Skip.Value);
            }

            if (Take != null)
            {
                query = query.Take(Take.Value);
            }

            var results = await ((IMongoQueryable<TEntity>) query).ToListAsync();

            return results
                .Select(PostProcessResult)
                .ToList();
        }

        protected virtual TResult PostProcessResult(TEntity item)
        {
            if (typeof(TResult) == typeof(TEntity))
            {
                return (TResult)(object)item;
            }

            return Mapper.Map<TResult>(item);
        }

        public Task<int> GetTotalItemsCount()
        {
            return ((IMongoQueryable<TEntity>)GetQueryable(collection.AsQueryable())).CountAsync();
        }
    }
}