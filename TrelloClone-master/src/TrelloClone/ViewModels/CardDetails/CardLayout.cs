using System.Collections.Generic;
using TrelloClone.Models;

namespace TrelloClone.ViewModels.CardDetails
{
    public class CardLayout
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ColumnId { get; set; }
        public int Column { get; set; } = 1;

        public List<Comment> Comments = new List<Comment>();
    }
}
