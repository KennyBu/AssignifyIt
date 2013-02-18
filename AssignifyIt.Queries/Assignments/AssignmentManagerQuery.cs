using System.Collections.Generic;
using AssignifyIt.Models;
using PetaPoco;

namespace AssignifyIt.Queries.Assignments
{
    public interface IAssignmentManagerQuery
    {
        IEnumerable<Assignee> GetAssignees();
        IEnumerable<Assignee> GetAssignees(string search);
    }
    
    public class AssignmentManagerQuery : IAssignmentManagerQuery
    {
        private readonly Database _db;

        public AssignmentManagerQuery(string connectionString)
        {
            _db = new Database(connectionString, "System.Data.SqlClient");
        }

        public IEnumerable<Assignee> GetAssignees()
        {
            return _db.Query<Assignee>("SELECT * FROM Assignee");
        }

        public IEnumerable<Assignee> GetAssignees(string search)
        {
            return _db.Query<Assignee>("SELECT * FROM Assignee WHERE Name Like {0}",string.Concat("%",search,"%"));
        }
    }
}