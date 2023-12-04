using System;
using System.Collections.Generic;

namespace TrelloClone.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Requirement { get; set; }
        public DateTime StartTerm { get; set; }
        public DateTime Term { get; set; }
        public DateTime? FactTerm { get; set; }
        public int? EmployeeAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public int? SupervisorAssessment { get; set; }
        public string? SupervisorComment { get; set; }
        public int ColumnId { get; set; }
        public Column Column { get; set; }
        public int UserId { get; set; }
        public bool IsRelevant { get; set; }
        public bool IsDeleted { get; set; }
        public bool ReadyToReport { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<File> Files { get; set; } = new List<File>();
        public List<Update> Updates { get; set; } = new List<Update>();
    }
}
