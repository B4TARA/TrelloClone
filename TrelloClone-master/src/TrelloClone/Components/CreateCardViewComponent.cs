using Microsoft.AspNetCore.Mvc;

namespace TrelloClone.Components
{
    public class CreateCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("CreateCard");
        }
    }
}
