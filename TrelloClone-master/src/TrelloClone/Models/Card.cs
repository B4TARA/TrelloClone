using System;
using System.ComponentModel.DataAnnotations;

namespace TrelloClone.Models
{
  public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Requirement { get; set; }
        public DateTime Term { get; set; }
        public int EmployeeAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public int SupervisorAssessment { get; set; }
        public string? SupervisorComment { get; set; }
        public double? Points { get; set; }
        public int ColumnId { get; set; }
        public Column Column { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
