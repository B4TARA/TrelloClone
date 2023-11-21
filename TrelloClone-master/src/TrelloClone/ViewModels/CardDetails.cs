using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrelloClone.Models;

namespace TrelloClone.ViewModels
{
    public class CardDetails
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Requirement { get; set; }

        [Required]
        public DateTime Term { get; set; }
        public DateTime? FactTerm { get; set; }
        public int? EmployeeAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public int? SupervisorAssessment { get; set; }
        public string? SupervisorComment { get; set; }
        public int Column { get; set; } = 1;
        public bool IsActiveLikeEmployee { get; set; }
        public bool IsActiveLikeSupervisor { get; set; }
        public int UserId { get; set; }
        public int ColumnId { get; set; }

        public List<Comment> Comments = new List<Comment>();

        public List<File> Files = new List<File>();

        public List<Update> Updates = new List<Update>();
    }
}
