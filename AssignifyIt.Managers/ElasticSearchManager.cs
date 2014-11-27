using System;
using System.Collections.Generic;
using System.Linq;
using AssignifyIt.Models;
using Nest;

namespace AssignifyIt.Managers
{
    public interface IElasticSearchManager
    {
        void CreateIndex(List<Assignee> assignees);
        void Reindex(List<Assignee> assignees);
        List<Assignee> Search(string search);
    }
    
    public class ElasticSearchManager : IElasticSearchManager
    {
        private const string IndexKey = "assigneeindex";
        
        private readonly ElasticClient _elasticClient;
        
        public ElasticSearchManager(string uri)
        {
            var uriString = uri;
            var searchBoxUri = new Uri(uriString);
            var elasticSettings = new ConnectionSettings(searchBoxUri)
                .SetDefaultIndex(IndexKey);

            _elasticClient = new ElasticClient(elasticSettings);
        }

        public void CreateIndex(List<Assignee> assignees)
        {
            
        }

        public void Reindex(List<Assignee> assignees)
        {
            
        }

        public List<Assignee> Search(string search)
        {
            var result = _elasticClient.Search<Assignee>(s => s
            .Query(q => q.Prefix(f => f.Name, search))
            );

            return result.Documents.ToList();
        }
    }
}