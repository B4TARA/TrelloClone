using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            string supervisorName = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;

            var model = await _boardService.ListBoard(userId);

            return View(model);
        }

        public async Task<IActionResult> IndexSupervisor()
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            string supervisorName = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;

            var model = await _boardService.ListBoardSupervisor(supervisorName);

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

            string employeeName = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;

            viewModel.UserId = userId;

            viewModel.EmployeeName = employeeName;

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