using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using OpenEvents.Backend.Common.Data;
using OpenEvents.Backend.Common.Exceptions;
using OpenEvents.Backend.Common.Queries;

namespace OpenEvents.Backend.Common.Facades
{
    public abstract class CrudFacadeBase<TEntity, TDTO, TFilterDTO> where TEntity : IIdentifiable where TDTO : IIdentifiable
    {

        protected readonly IMongoCollection<TEntity> collection;
        private readonly Func<IFilteredQuery<TDTO, TFilterDTO>> queryFactory;

        public CrudFacadeBase(IMongoCollection<TEntity> collection, Func<IFilteredQuery<TDTO, TFilterDTO>> queryFactory)
        {
            this.collection = collection;
            this.queryFactory = queryFactory;
        }

        public async Task<List<TDTO>> GetAll(TFilterDTO filter)
        {
            var query = queryFactory();
            query.Filter = filter;

            return (await query.Execute()).ToList();
        }

        public async Task<TDTO> GetById(string id)
        {
            var entity = await collection.FindByIdAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            return Mapper.Map<TDTO>(entity);
        }

        public async Task Insert(TDTO data)
        {
            var entity = Mapper.Map<TEntity>(data);

            data.Id = entity.Id = Guid.NewGuid().ToString();

            await collection.InsertOneAsync(entity);
        }

        public async Task Update(string id, TDTO data)
        {
            var entity = await collection.FindByIdAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            Mapper.Map(data, entity);
            await collection.ReplaceOneAsync(x => x.Id == id, entity);
        }

        public async Task Delete(string id)
        {
            var result = await collection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount == 0)
            {
                throw new EntityNotFoundException();
            }
        }

    }
}