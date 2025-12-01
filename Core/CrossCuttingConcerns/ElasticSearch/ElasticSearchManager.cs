using Nest;

namespace Core.CrossCuttingConcerns.ElasticSearch
{
    public class ElasticSearchManager<T> : IElasticSearchService<T> where T : class
    {
        private readonly IElasticClient _client;

        public ElasticSearchManager(ElasticSearchOptions options)
        {
            var setting = new ConnectionSettings(new Uri(options.Uri))
                .DefaultIndex(options.IndexName);
            _client = new ElasticClient(setting);
        }

        public async Task DeleteDocumentAsync(string id)
        {
            await _client.DeleteAsync<T>(id);   
        }

        public async Task<T> GetDocumentAsync(string id)
        {
            var response = await _client.GetAsync<T>(id);
            return response.Source;
        }

        public async Task IndexDocumentAsync(T entity)
        {
            await _client.IndexDocumentAsync(entity);
        }

        public async Task<IEnumerable<T>> SearchAsync(string query)
        {
            var response = await _client.SearchAsync<T>(s => s
                .Query(q => q
                    .QueryString(d => d
                        .Query(query)
                    )
                )
            );
            return response.Documents;
        }

        public async Task UpdateDocumentAsync(string id, T entity)
        {
            await _client.UpdateAsync<T>(id, u => u.Doc(entity));   
        }
    }
}
