using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Models.Enum;
using TrelloClone.Services;

namespace TrelloClone.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserBoardService _userBoardService;

        public HomeController(UserBoardService boardService)
        {
            _userBoardService = boardService;
        }

        public async Task<IActionResult> Index()
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
                var supervisorId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var response = await _userBoardService.GetFirstEmployee(supervisorId);

                if (response.StatusCode == StatusCodes.OK)
                {
                    return RedirectToAction("ListEmployeeCards", "UserBoard", new { employeeId = response.Data.Id });
                }

                return NotFound(response.Description);         
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