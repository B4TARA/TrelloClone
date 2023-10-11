using System.Collections.Generic;
using TrelloClone.Models.Enum;

namespace TrelloClone.Models
{
    public class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Position { get; set; }
        public string? SspName { get; set; }
        public string? ImagePath { get; set; }
        public long? SupervisorId { get; set; }
        public string? SupervisorName { get; set; }          
        public Role Role { get; set; }
    }
}
