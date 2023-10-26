﻿using System;
using System.Collections.Generic;

namespace TrelloClone.ViewModels
{
    public class UserBoardView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActiveLikeEmployee { get; set; }
        public bool IsActiveLikeSupervisor { get; set; }
        public List<string> Notifications { get; set; } = new List<string>();
        public List<Column> Columns { get; set; } = new List<Column>();

        public class Column
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public List<Card> Cards { get; set; } = new List<Card>();
        }

        public class Card
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Requirement { get; set; }
            public DateTime Term { get; set; }
            public int? EmployeeAssessment { get; set; }
            public string? EmployeeComment { get; set; }
            public int? SupervisorAssessment { get; set; }
            public string? SupervisorComment { get; set; }
            public double? Points { get; set; }
        }
    }
}