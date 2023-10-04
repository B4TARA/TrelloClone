using System.ComponentModel.DataAnnotations;

namespace TrelloClone.Models
{
  public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Requirement { get; set; }
        public string Term { get; set; }
        public string? EmployeeAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public string? SupervisorAssessment { get; set; }
        public string? SupervisorComment { get; set; }
        public double? Points { get; set; }
        public int ColumnId { get; set; }
        public Column Column { get; set; }
    }
}
