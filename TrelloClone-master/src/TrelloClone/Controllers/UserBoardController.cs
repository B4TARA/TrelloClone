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
        public IActionResult ListMyCardsTable()
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
        public IActionResult ListEmployeeCardsTable(int employeeId)
        {
            int supervisorId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            var model = _userBoardService.ListEmployeeCards(employeeId, supervisorId);

            return View(model);
        }

        [HttpGet]
        public IActionResult ListArchiveCards(int userId)
        {
            var model = _userBoardService.ListArchiveCards(userId);

            return View(model);
        }

        [HttpGet]
        public IActionResult AddCardViewComponent()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("AddCard");
        }      

        [HttpGet]
        public IActionResult GetCardLayoutViewComponent(int cardId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("CardLayout", new { cardId = cardId, userId = userId});
        }

        [HttpGet]
        public IActionResult GetCardInfoViewComponent(int cardId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("CardInfo", new { cardId = cardId, userId = userId });
        }

        [HttpGet]
        public IActionResult GetCardAssessmentViewComponent(int cardId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("CardAssessment", new { cardId = cardId, userId = userId });
        }

        [HttpGet]
        public IActionResult GetCardHistoryViewComponent(int cardId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("CardHistory", new { cardId = cardId, userId = userId });
        }
    }
}