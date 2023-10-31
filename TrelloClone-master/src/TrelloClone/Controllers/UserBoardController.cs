using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        public IActionResult AddCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCard(AddCard viewModel)
        {
            viewModel.Id = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            //ViewBag добавить ошибки и выводить их в этой формочке (если они есть)
            if (!ModelState.IsValid) return RedirectToAction(nameof(ListMyCards));

            _userBoardService.AddCard(viewModel);

            return RedirectToAction(nameof(ListMyCards));
        }

        [HttpGet]
        public IActionResult GetCardDetailsViewComponent(int cardId)
        {
            return ViewComponent("CardDetails", new { cardId = cardId });
        }
    }
}