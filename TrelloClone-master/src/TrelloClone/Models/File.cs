﻿namespace TrelloClone.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int CardId { get; set; }
        public int UserId { get; set; }
    }
}
