using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using TrelloClone.Models;
using TrelloClone.Services;
using TrelloClone.ViewModels;

namespace TrelloClone.Controllers
{
    public class UserBoardController : Controller
    {
        private readonly UserBoardService _userBoardService;

        public UserBoardController(UserBoardService boardService)
        {
            _userBoardService = boardService;
        }

        [HttpGet]
        public IActionResult ListMyCards()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            var model = _userBoardService.ListMyCards(userId);

            return View(model);
        }

        [HttpGet]
        public IActionResult ListEmployeeCards(int employeeId)
        {
            int supervisorId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            var model = _userBoardService.ListEmployeeCards(employeeId, supervisorId);

            return View(model);
        }
        
        [HttpGet]
        public IActionResult AddCardViewComponent()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("AddCard");
        }      

        [HttpGet]
        public IActionResult GetCardDetailsViewComponent(int cardId, string view)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("CardDetails", new { cardId = cardId, userId = userId});
        }
    }
}