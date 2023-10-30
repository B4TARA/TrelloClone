using System.Collections.Generic;
using TrelloClone.Models.Enum;

namespace TrelloClone.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Position { get; set; }
        public string? SspName { get; set; }
        public string? ImagePath { get; set; }
        public long? SupervisorId { get; set; }
        public string? SupervisorName { get; set; }
        public Roles Role { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>();

        public bool IsActiveLikeEmployee { get; set; } = false;
        public bool IsActiveLikeSupervisor { get; set; } = false;
        public bool IsActiveToAddCard { get; set; } = false;
        public List<string> Notifications { get; set; } = new List<string>();
    }
}
