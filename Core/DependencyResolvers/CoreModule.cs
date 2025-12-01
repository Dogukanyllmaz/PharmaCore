using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Redis;
using Core.CrossCuttingConcerns.Logging.Loggers;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.ElasticSearch;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, RedisCacheManager>();
            serviceCollection.AddSingleton<ILoggerService, SerilogLogger>();
            serviceCollection.AddSingleton(new ElasticSearchOptions
            {
                Uri = "http://localhost:9200",
                IndexName = "pharma_index"
            });
            serviceCollection.AddScoped(typeof(IElasticSearchService<>), typeof(ElasticSearchManager<>));
        }
    }
}
