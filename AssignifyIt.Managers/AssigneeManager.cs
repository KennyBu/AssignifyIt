using System.Collections.Generic;
using AssignifyIt.Models;
using AssignifyIt.Queries.Assignments;

namespace AssignifyIt.Managers
{
    public interface IAssigneeManager
    {
        IEnumerable<Assignee> GetAssignees();
    }

    public class AssigneeManager : IAssigneeManager
    {
        private readonly IAssignmentManagerQuery _assignmentManagerQuery;

        public AssigneeManager(IAssignmentManagerQuery assignmentManagerQuery)
        {
            _assignmentManagerQuery = assignmentManagerQuery;
        }

        public IEnumerable<Assignee> GetAssignees()
        {
            return _assignmentManagerQuery.GetAssignees();
        }
    }
}