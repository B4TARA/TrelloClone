using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrelloClone.Models;

namespace TrelloClone.ViewModels.CardDetails
{
    public class CardAssessment
    {
        public int Id { get; set; }     
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
    }
}
