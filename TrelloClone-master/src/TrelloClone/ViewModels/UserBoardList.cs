using System;
using System.Collections.Generic;

namespace TrelloClone.ViewModels
{
    public class UserBoardList
    {
        public List<UserBoard> Users = new List<UserBoard>();
        public List<string> Notifications { get; set; } = new List<string>();

        public class UserBoard
        {
            public int Id { get; set; }
            public string Name { get; set; }   
        }
    }
}
