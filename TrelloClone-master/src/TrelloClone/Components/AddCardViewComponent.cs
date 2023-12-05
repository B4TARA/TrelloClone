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
            var model = GetModel();

            return View("AddCard", model);
        }

        private AddCard GetModel()
        {
            DateTime FakeToday = Term.GetFakeDate();

            var quarter = Term.GetQuarter(FakeToday);
            var min = Term.GetMin(quarter, FakeToday);
            var max = Term.GetMax(quarter, FakeToday);

            return new AddCard
            {
                Min = min,
                Max = max,
                Term = FakeToday
            };
        }
    }
}
