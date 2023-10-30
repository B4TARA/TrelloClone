using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace TrelloClone.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            else if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            else if (User.IsInRole("Supervisor"))
            {
                return RedirectToAction("ListSubordinateEmployees", "UserBoard");
            }

            else if (User.IsInRole("Employee"))
            {
                return RedirectToAction("ListMyCards", "UserBoard");
            }

            else if (User.IsInRole("Combined"))
            {
                return RedirectToAction("ListMyCards", "UserBoard");
            }

            return RedirectToAction("Login", "Account");
        }

      
    }
}