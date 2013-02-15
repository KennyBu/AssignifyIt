using System.Collections.Generic;

namespace AssignifyIt.Models
{
    public class AboutViewModel
    {
        public string Message { get; set; }
        public List<AssigneeViewModel> Assignees { get; set; }
    }

    public class AssigneeViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; } 
    }
}