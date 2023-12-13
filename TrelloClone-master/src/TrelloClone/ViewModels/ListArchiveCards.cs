using System;
using System.Collections.Generic;

namespace TrelloClone.ViewModels
{
    public class ListArchiveCards
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImgPath { get; set; }
        public List<Card> ArchivedCards { get; set; } = new List<Card>();
        public List<Card> DeletedCards { get; set; } = new List<Card>();

        public class Card
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Requirement { get; set; } = string.Empty;
            public DateTime Term { get; set; }
            public DateTime FactTerm { get; set; }
            public string EmployeeAssessment { get; set; } = string.Empty;
            public string SupervisorAssessment { get; set; } = string.Empty;
            public int CountOfComments { get; set; }
            public int CountOfFiles { get; set; }
        }
    }
}
