using System;

namespace AssignifyIt.Models
{
    public class DailyText
    {
        public Guid Id { get; set; } 
        public string DateLine { get; set; } 
        public string Header { get; set; } 
        public string Body { get; set; }
        public DateTime DateEntered { get; set; } 
    }
}