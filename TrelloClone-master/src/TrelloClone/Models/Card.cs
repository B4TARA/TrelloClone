using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrelloClone.Models
{
  public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Requirement { get; set; }
        public DateTime Term { get; set; }
        public int? EmployeeAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public int? SupervisorAssessment { get; set; }
        public string? SupervisorComment { get; set; }
        public int ColumnId { get; set; }
        public Column Column { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public bool IsRelevant { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<File> Files { get; set; } = new List<File>();
    }
}
