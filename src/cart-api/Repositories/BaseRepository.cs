using cart_api.Models;
using cart_api.DataAccess;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace cart_api.Repositories
{
    public abstract class BaseRepository<TDocument> : IBaseRepository<TDocument> where TDocument : BaseDocument
    {
        protected readonly ICartDBContext _context;

        protected readonly IMongoCollection<TDocument> _collection;

        protected BaseRepository(ICartDBContext context)
        {
            _context = context;
            _collection = _context.Get<TDocument>(typeof(TDocument).Name);
        }

        protected UpdateDefinitionBuilder<TDocument> UpdateDefinitionBuilder => Builders<TDocument>.Update;

        protected FilterDefinitionBuilder<TDocument> FilterDefinitionBuilder => Builders<TDocument>.Filter;

        protected ProjectionDefinitionBuilder<TDocument> ProjectionDefinitionBuilder => Builders<TDocument>.Projection;

        protected SortDefinitionBuilder<TDocument> SortDefinitionBuilder => Builders<TDocument>.Sort;

        public async Task<IEnumerable<TDocument>> GetAllDocumentAsync() => await (await _collection.FindAsync<TDocument>(f => true)).ToListAsync();

        public virtual async Task<TDocument> GetDocumentAsync(Expression<Func<TDocument, bool>> filter) => await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();

        public virtual async Task AddDocumentAsync(TDocument document)
        {
            document.CreatedAt = DateTime.Now;
            await _collection.InsertOneAsync(document);
        }

        public virtual async Task<bool> RemoveDocumentAsync(Expression<Func<TDocument, bool>> filter)
        {
            var result = await _collection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public virtual async Task<bool> UpdateDocumentAsync<TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value)
        {
            var updateDef = UpdateDefinitionBuilder.Set(field, value);
            updateDef = updateDef.Set(f => f.LastModifiedAt, DateTime.Now);
            return await UpdateDocumentAsync(filter, updateDef);
        }

        protected virtual async Task<bool> UpdateDocumentAsync(Expression<Func<TDocument, bool>> filter, UpdateDefinition<TDocument> update)
        {
            update = update.Set(f => f.LastModifiedAt, DateTime.Now);
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        protected virtual async Task<TDocument> GetDocumentAsync(Expression<Func<TDocument, bool>> filter, ProjectionDefinition<TDocument> projectionDefinition)
        {
            var findOption = new FindOptions<TDocument>()
            {
                Projection = projectionDefinition
            };
            return await (await _collection.FindAsync(filter, findOption)).FirstOrDefaultAsync();
        }

        protected virtual ObjectId GetObjectId(string id)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                return objectId;
            }

            throw new InvalidOperationException("Invalid id");
        }
    }
}