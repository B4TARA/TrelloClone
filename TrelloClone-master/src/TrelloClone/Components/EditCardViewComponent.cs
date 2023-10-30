using Microsoft.AspNetCore.Mvc;

namespace TrelloClone.Components
{
    public class EditCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("EditCard");
        }
    }
}
