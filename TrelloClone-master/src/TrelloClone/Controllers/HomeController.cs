using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TrelloClone.Services;
using TrelloClone.ViewModels;

namespace TrelloClone.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly BoardService _boardService;

        public HomeController(BoardService boardService)
        {
            _boardService = boardService;
        }

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

            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            var model = _boardService.ListBoard(userId);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewBoard viewModel)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            viewModel.UserId = userId;

            _boardService.AddBoard(viewModel);
            
            return RedirectToAction(nameof(Index));
        }

        //[Authorize]
        [HttpPost]
        public IActionResult Delete(BoardView boardView)
        {
            try
            {
                _boardService.DeleteBoard(boardView.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
  
    }
}