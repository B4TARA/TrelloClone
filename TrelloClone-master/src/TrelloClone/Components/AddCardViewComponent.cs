using Microsoft.AspNetCore.Mvc;
using System;
using TrelloClone.Models.Term;
using TrelloClone.ViewModels;

namespace TrelloClone.Components
{
    public class AddCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            DateTime FakeToday = Term.GetFakeDate();

            var quarter = Term.GetQuarter(FakeToday);
            var min = Term.GetMin(quarter, FakeToday);
            var max = Term.GetMax(quarter, FakeToday);

            var model = new AddCard { Min = min, Max = max, Term = FakeToday };

            return View("AddCard", model);
        }
    }
}
