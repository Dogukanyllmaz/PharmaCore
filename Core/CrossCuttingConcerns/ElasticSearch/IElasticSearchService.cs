using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.ElasticSearch
{
    public interface IElasticSearchService<T> where T : class
    {
        Task IndexDocumentAsync(T entity);
        Task UpdateDocumentAsync(string id, T entity);
        Task DeleteDocumentAsync(string id);
        Task<T> GetDocumentAsync(string id);
        Task<IEnumerable<T>> SearchAsync(string query);
    }
}
