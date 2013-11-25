using System;
using System.Collections.Generic;
using System.Linq;
using AssignifyIt.Models;
using AssignifyIt.Queries.Assignments;

namespace AssignifyIt.Managers
{
    public interface IAssigneeManager
    {
        IEnumerable<Assignee> GetAssignees();
        IEnumerable<Assignee> GetAssignees(string search);
        void IndexAssignees();
        void Reindex();
    }

    public class AssigneeManager : IAssigneeManager
    {
        private readonly IAssignmentManagerQuery _assignmentManagerQuery;
        private readonly IElasticSearchManager _elasticSearchManager;

        public AssigneeManager(IAssignmentManagerQuery assignmentManagerQuery, 
            IElasticSearchManager elasticSearchManager)
        {
            _assignmentManagerQuery = assignmentManagerQuery;
            _elasticSearchManager = elasticSearchManager;
        }

        public IEnumerable<Assignee> GetAssignees()
        {
            return _assignmentManagerQuery.GetAssignees();
        }

        public IEnumerable<Assignee> GetAssignees(string search)
        {
            //return _assignmentManagerQuery.GetAssignees(search);
            var assignees = _elasticSearchManager.Search(search);
            return assignees;
        }

        public void IndexAssignees()
        {
            var assignees = _assignmentManagerQuery.GetAssignees().ToList();
            _elasticSearchManager.CreateIndex(assignees);
        }

        public void Reindex()
        {
            var assignees = _assignmentManagerQuery.GetAssignees().ToList();
            _elasticSearchManager.Reindex(assignees);
        }
    }
}