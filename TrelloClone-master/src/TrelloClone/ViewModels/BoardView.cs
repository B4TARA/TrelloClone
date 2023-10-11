using System.Collections.Generic;

namespace TrelloClone.ViewModels
{
    public class BoardView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EmployeeName { get;set; }
        public List<Column> Columns { get; set; } = new List<Column>();

        public class Column
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Card> Cards { get; set; } = new List<Card>();
        }

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
        }
    }
}
