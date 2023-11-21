using System;

namespace TrelloClone.ViewModels
{
    public class AddCard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Requirement { get; set; }

        public DateTime Term { get; set; }

        public DateTime Min { get; set; }

        public DateTime Max { get; set; }
    }
}
