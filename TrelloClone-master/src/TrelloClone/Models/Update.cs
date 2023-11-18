using System;

namespace TrelloClone.Models
{
    public class Update
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }  
        public DateTime Date { get; set; }
        public string Content { get; set; }
    }
}
