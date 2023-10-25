using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrelloClone.ViewModels
{
    public class NewBoard
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime Title { get; set; } = new DateTime();
        public string EmployeeName { get; set; }
        public List<string> Months { get; } = new List<string>()
        {
            "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
        };
    }
}
