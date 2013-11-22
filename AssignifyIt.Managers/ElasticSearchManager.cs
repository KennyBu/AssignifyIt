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
        List<Assignee> Search(string search);
    }
    
    public class ElasticSearchManager : IElasticSearchManager
    {
        private readonly ElasticClient _elasticClient;
        
        public ElasticSearchManager(string uri)
        {
            var uriString = uri;
            var searchBoxUri = new Uri(uriString);
            var elasticSettings = new ConnectionSettings(searchBoxUri)
                .SetDefaultIndex("assigneeindex");

            _elasticClient = new ElasticClient(elasticSettings);
        }

        public void CreateIndex(List<Assignee> assignees)
        {
            _elasticClient.CreateIndex("assigneeindex", new IndexSettings());

            _elasticClient.Index(assignees);    
        }

        public List<Assignee> Search(string search)
        {
            var result = _elasticClient.Search<Assignee>(body => body.Query(query =>
            query.QueryString(qs => qs.Query(search))));

            return result.Documents.ToList();
        }
    }
}