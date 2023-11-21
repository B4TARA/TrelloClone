using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Services;
using TrelloClone.ViewModels;
using StatusCodes = TrelloClone.Models.Enum.StatusCodes;

namespace TrelloClone.Controllers
{
    public class CardController : Controller
    {
        private readonly CardService _cardService;
        private readonly UserService _userService;

        public CardController(CardService cardService, UserService userService)
        {
            _cardService = cardService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Update(CardDetails card)
        {
            var userName = User.FindFirst("Name").Value;

            var userImg = Convert.ToString(User.FindFirst("ImagePath").Value);

            var action = Request.Headers.Referer.ToString().Split("/")[4];

            var response = await _cardService.Update(card, userName, userImg);

            if (response.StatusCode == StatusCodes.OK)
            {
                TempData["Message"] = "Данные обновлены";

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

        [HttpPost]
        public async Task<IActionResult> GiveSupervisorRating(int CardId, string Name, DateTime Term, DateTime FactTerm, string Requirement, int SupervisorAssessment, string SupervisorComment)
        {
            var userName = User.FindFirst("Name").Value;

            var userImg = Convert.ToString(User.FindFirst("ImagePath").Value);

            var action = Request.Headers.Referer.ToString().Split("/")[4];

            var response = await _cardService.GiveSupervisorRating(CardId, Name, Term, FactTerm, Requirement, SupervisorAssessment, SupervisorComment, userName, userImg);

            if (response.StatusCode == StatusCodes.OK)
            {
                TempData["Message"] = "Данные обновлены";

                var employeeId = Convert.ToInt32(action.Split("=")[1]);
                action = action.Split("?")[0];
                return RedirectToAction(action, "UserBoard", new { employeeId = employeeId });
            }

            return NotFound(response.Description);
        }

        [HttpPost]
        public async Task<IActionResult> GiveEmployeeRating(int CardId, string Name, DateTime Term, string Requirement, int EmployeeAssessment, string EmployeeComment)
        {
            var userName = User.FindFirst("Name").Value;

            var userImg = Convert.ToString(User.FindFirst("ImagePath").Value);

            var response = await _cardService.GiveEmployeeRating(CardId, Name, Term, Requirement, EmployeeAssessment, EmployeeComment, userName, userImg);

            if (response.StatusCode == StatusCodes.OK)
            {
                TempData["Message"] = "Данные обновлены";

                return RedirectToAction("ListMyCards", "UserBoard");

            }

            return NotFound(response.Description);
        }

        [HttpPost]
        public IActionResult Create(AddCard viewModel)
        {
            var action = Request.Headers.Referer.ToString().Split("/")[4];

            viewModel.Id = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            _cardService.Create(viewModel);
           
            return RedirectToAction(action, "UserBoard");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            int cardId = Convert.ToInt32(Request.Form["cardId"]);

            _cardService.Delete(cardId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var userId = Convert.ToInt32(User.FindFirst("Id").Value);

            var userName = User.FindFirst("Name").Value;

            var userImg = Convert.ToString(User.FindFirst("ImagePath").Value);

            IFormFile fileToUpload = Request.Form.Files[0];
            int cardId = Convert.ToInt32(Request.Form["cardId"]);

            var response = await _cardService.UploadFile(fileToUpload, userId, cardId, userName, userImg);

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Файл успешно добавлен!");
            }

            return NotFound(response.Description);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile()
        {
            var userName = User.FindFirst("Name").Value;

            var userImg = Convert.ToString(User.FindFirst("ImagePath").Value);

            int fileId = Convert.ToInt32(Request.Form["fileId"]);
            int cardId = Convert.ToInt32(Request.Form["cardId"]);

            var response = await _cardService.DeleteFile(fileId, cardId, userName, userImg);

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Файл успешно удалён!");
            }

            return NotFound(response.Description);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment()
        {
            var userId = Convert.ToInt32(User.FindFirst("Id").Value);

            var userName = User.FindFirst("Name").Value;

            var userImage = User.FindFirst("ImagePath").Value;

            string comment = Request.Form["comment"];
            int cardId = Convert.ToInt32(Request.Form["cardId"]);

            var response = await _cardService.AddComment(comment, userId, userName, userImage, cardId);

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Файл успешно добавлен!");
            }

            return NotFound(response.Description);
        }
    }
}