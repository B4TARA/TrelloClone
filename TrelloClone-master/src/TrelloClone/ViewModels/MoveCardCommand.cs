using System;
using TrelloClone.Models;

namespace TrelloClone.ViewModels
{
    public class MoveCardCommand
    {
        public int CardId { get; set; }
        public int ColumnId { get; set; }
        public string Name { get; set; }
        public DateTime Term { get; set; }
        public string Requirement { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public int EmployeeId { get; set; }
    }
}