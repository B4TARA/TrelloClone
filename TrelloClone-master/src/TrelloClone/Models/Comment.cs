namespace TrelloClone.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string UserImg { get; set; }
    }
}
