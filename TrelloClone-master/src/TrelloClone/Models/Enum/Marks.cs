using System.ComponentModel.DataAnnotations;

namespace TrelloClone.Models.Enum
{
    public enum Marks
    {
        [Display(Name = "В процессе исполнения")]
        InProgress = 0,
        [Display(Name = "Выполнено в надлежащем качестве")] 
        Good = 1,
        [Display(Name = "Выполнено в надлежащем качестве в установленный срок")]
        GoodInTerm = 2,
        [Display(Name = "Выполнено в надлежащем качестве с нарушением срока до 7 календарных дней включительно")]
        GoodOutTerm1 = 3,
        [Display(Name = "Выполнено в надлежащем качестве с нарушением срока от 8 до 30 календарных дней включительно")]
        GoodOutTerm2 = 4,
        [Display(Name = "Выполнено в надлежащем качестве с нарушением срока свыше 30 календарных дней")]
        GoodOutTerm3 = 5,
        [Display(Name = "Выполнено в ненадлежащем качестве в установленный срок")]
        BadInTerm = 6,
        [Display(Name = "Не выполнено")]
        Bad = 7,
        [Display(Name = "Просрочено")]
        Overdue = 8,
        [Display(Name = "Перенос")]
        Closed = 9,
    }

    public struct MarksAndPoints
    {
        public static readonly double[] Points = { 1, 1, 0.9, 0.8, 0.5, 0.3, 0, 0};
    }
}
