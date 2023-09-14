using System.Linq.Expressions;
using MongoDB.Driver;
using Audits.Domain.Contracts;
using Audits.Domain.Models.Attributes;
using Audits.Infrastructure.BBDD.Contracts;

namespace Audits.Infrastructure.BBDD.Repositories
{
    public class MongoRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IBaseEntity<TKey>, ISoftDeleteEntity where TKey : struct
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoClient client, string databaseName)
        {
            var database = client.GetDatabase(databaseName);
            var collectionNameAttribute = (BsonCollectionAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(BsonCollectionAttribute));
            var collectionName = collectionNameAttribute?.CollectionName ?? typeof(TEntity).Name;
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        public IQueryable<TEntity> GetAll() => _collection.AsQueryable().Where(entity => entity.DeletedAt == null);

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _collection.AsQueryable().Where(expression).Where(entity => entity.DeletedAt == null);
        }

        public async Task<TEntity> GetByIdAsync(TKey id) => await _collection.Find(entity => entity.Id.Equals(id) && entity.DeletedAt == null).FirstOrDefaultAsync();

        public async Task UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}
