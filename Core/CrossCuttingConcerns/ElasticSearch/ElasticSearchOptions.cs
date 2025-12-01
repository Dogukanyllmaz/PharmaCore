using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.ElasticSearch
{
    public class ElasticSearchOptions
    {
        public string Uri { get; set; } = "http://localhost:9200";
        public string IndexName { get; set; } = "pharma_index";
    }
}
