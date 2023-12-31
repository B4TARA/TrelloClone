﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace TrelloClone.Models.Assessment
{
    public static class AssessmentsForDropdown
    {
        public static IEnumerable<Assessment> GetAssessments()
        {
            return new List<Assessment>
        {
            new Assessment{
                Id = 1,
                Value = 1,
                Text = "Выполнено в надлежащем качестве в установленный срок / Выполнено в надлежащем качестве",
                Description = "100%"
            },

            new Assessment{
                Id = 2,
                Value = 1,
                Text = "Выполнено в надлежащем качестве с нарушением срока до 7 календарных дней, включительно",
                Description = "90%"
            },

            new Assessment{
                Id = 3,
                Value = 0.9,
                Text = "Выполнено в надлежащем качестве с нарушением срока от 8 до 30 календарных дней, включительно",
                Description = "80%"
            },

            new Assessment{
                Id = 4,
                Value = 0.8,
                Text = "Выполнено в надлежащем качестве с нарушением срока свыше 30 календарных дней",
                Description = "50%"
            },

            new Assessment{
                Id = 5,
                Value = 0.5,
                Text = "Выполнено в ненадлежащем качестве в установленный срок",
                Description = "30%"
            },

            new Assessment{
                Id = 6,
                Value = 0.3,
                Text = "Не выполнено",
                Description = "0%\r\nПрисваивается, когда задача не выполнена в надлежащем качестве и в установленный срок, но дальнейшему мониторингу не подлежит.\r\n"
            },

            new Assessment{
                Id = 7,
                Value = 0,
                Text = "Просрочено",
                Description = "0%\r\nПрисваивается, когда задача ещё не выполнена, но её действие продолжается, и она подлежит дальнейшему мониторингу. Задача участвует в текущей оценке, а также переносится в план следующего оцениваемого периода.\r\n"
            },

            new Assessment{
                Id = 8,
                Value = 0,
                Text = "В процессе выполнения",
                Description = "В расчетах не учитывается.\r\nПрисваивается, когда задача находится в процессе выполнения, но плановый срок её исполнения ещё не наступил, поэтому задача не оценивается в отчётном периоде.\r\n"
            },

            new Assessment{
                Id = 9,
                Value = null,
                Text = "Закрыто",
                Description = "В расчетах не учитывается.\r\nПрисваивается, когда задача не подлежит оценке и дальнейшему мониторингу (например, в связи с неактуальностью).\r\n"
            },
        };
        }
    }
}
