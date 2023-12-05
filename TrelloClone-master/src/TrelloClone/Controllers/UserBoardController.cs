using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Models.Enum;
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
        public IActionResult GetCardDetailsViewComponent(int cardId)
        {
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            return ViewComponent("CardDetails", new { cardId = cardId, userId = userId });
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

        [HttpPost]
        public async Task<IActionResult> MoveCard(int ColumnId, int CardId, string Name, DateTime Term, string Requirement)
        {
            var action = Request.Headers.Referer.ToString().Split("/")[4];

            var userName = User.FindFirst("Name").Value;

            var userImg = Convert.ToString(User.FindFirst("ImagePath").Value);

            MoveCardCommand command = new MoveCardCommand();
            command.ColumnId = ColumnId;
            command.CardId = CardId;
            command.Name = Name;
            command.Term = Term;
            command.Requirement = Requirement;
            command.UserName = userName;
            command.UserImg = userImg;

            var response = await _userBoardService.Move(command);

            if (response.StatusCode == StatusCodes.OK)
            {
                //TempData["Message"] = "Äàííûå îáíîâëåíû";
                if (action == "ListMyCards")
                {
                    return RedirectToAction(action, "UserBoard");
                }

                else
                {
                    var employeeId = Convert.ToInt32(action.Split("=")[1]);
                    action = action.Split("?")[0];
                    return RedirectToAction(action, "UserBoard", new { employeeId = employeeId });
                }
            }

            return NotFound(response.Description);
        }

        [HttpPost("rejectcard")]
        public async Task<IActionResult> RejectCard(int CardId)
        {
            var action = Request.Headers.Referer.ToString().Split("/")[4];

            var response = await _userBoardService.Reject(CardId);

            if (response.StatusCode == StatusCodes.OK)
            {
                //TempData["Message"] = "Äàííûå îáíîâëåíû";

                var employeeId = Convert.ToInt32(action.Split("=")[1]);
                action = action.Split("?")[0];
                return RedirectToAction(action, "UserBoard", new { employeeId = employeeId });
            }

            return NotFound(response.Description);
        }

        [HttpGet]
        public async Task<JsonResult> GetReport()
        {
            try
            {
                var supervisorName = User.FindFirst("Name").Value;

                var response = await _userBoardService.GetReport(supervisorName);
                if (response.StatusCode != StatusCodes.OK)
                {
                    return Json("Упс... Что-то пошло не так: " + response.Description);
                }

                return Json(response.Data);
            }

            catch (Exception ex)
            {               
                return Json("Упс... Что-то пошло не так: " + ex.Message);
            }
        }
    }
}