using System.ComponentModel.DataAnnotations;

namespace TrelloClone.ViewModels
{
    public class NewBoard
    {
        [Required]
        public int UserId { get; set; }

        [RegularExpression(@"^[А-Я]+[а-яА-Я\s]*$", ErrorMessage ="Пожалуйста, сделайте первую букву заглавной и используйте только русские символы.")]
        [Required]
        public string Title { get; set; }
        public string EmployeeName { get; set; }
    }
}
