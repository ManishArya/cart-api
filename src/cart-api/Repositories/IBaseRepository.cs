using cart_api.Models;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cart_api.Repositories
{
    public interface IBaseRepository<TDocument> where TDocument : BaseDocument
    {
        Task<IEnumerable<TDocument>> GetAllDocumentAsync();

        Task<TDocument> GetDocumentAsync(Expression<Func<TDocument, bool>> filter);

        Task AddDocumentAsync(TDocument document);

        Task<bool> UpdateDocumentAsync<TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value);

        Task<bool> RemoveDocumentAsync(Expression<Func<TDocument, bool>> filter);

    }
}