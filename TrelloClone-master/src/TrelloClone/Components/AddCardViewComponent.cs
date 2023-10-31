using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrelloClone.ViewModels;

namespace TrelloClone.Components
{
    public class AddCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("AddCard");
        }
    }
}
