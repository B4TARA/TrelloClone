using System;
using System.Collections.Generic;
using TrelloClone.Models;

namespace TrelloClone.ViewModels
{
    public class CardDetails
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Requirement { get; set; } = string.Empty;
        public DateTime Term { get; set; }
        public DateTime? FactTerm { get; set; }
        public int? EmployeeAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public int? SupervisorAssessment { get; set; }
        public string? SupervisorComment { get; set; }
        public int Column { get; set; } = 1;
        public int UserId { get; set; }
        public int ColumnId { get; set; }

        public List<Comment> Comments = new List<Comment>();
        public List<File> Files = new List<File>();
        public List<Update> Updates = new List<Update>();
    }
}
