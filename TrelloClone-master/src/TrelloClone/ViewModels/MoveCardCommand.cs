using TrelloClone.Models;

namespace TrelloClone.ViewModels
{
    public class MoveCardCommand
    {
        public int UserId { get; set; }
        public int ColumnId { get; set; }
    }
}