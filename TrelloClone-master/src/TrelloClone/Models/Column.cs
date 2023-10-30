using System.Collections.Generic;

namespace TrelloClone.Models
{
  public class Column
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public int UserId { get; set; }
    }
}
