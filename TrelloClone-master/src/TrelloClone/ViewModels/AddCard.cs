using System;
using System.ComponentModel.DataAnnotations;

namespace TrelloClone.ViewModels
{
    public class AddCard
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Requirement { get; set; }

        [Required]
        public DateTime Term { get; set; }
    }
}
