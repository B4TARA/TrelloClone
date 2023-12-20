using System.Collections.Generic;
using TrelloClone.Models.Enum;

namespace TrelloClone.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
        public string? SspName { get; set; }
        public string? ImagePath { get; set; }
        public long? SupervisorId { get; set; }
        public string? SupervisorName { get; set; }
        public Roles Role { get; set; }
        public bool IsBlocked { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>();
    }
}
