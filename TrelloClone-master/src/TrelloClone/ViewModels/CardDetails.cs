using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public int EmployeeAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public int SupervisorAssessment { get; set; }
        public string? SupervisorComment { get; set; }
        public double? Points { get; set; }
        public int Column { get; set; } = 1;
        public List<SelectListItem> Columns { get; set; } = new List<SelectListItem>();
    }
}
