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
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            var userImg = Convert.ToString(User.Claims.FirstOrDefault(c => c.Type == "ImagePath").Value);

            var action = Request.Headers.Referer.ToString().Split("/")[4];

            var response = await _cardService.Update(card, userId, userImg);

            if (response.StatusCode == StatusCodes.OK)
            {
                TempData["Message"] = "Данные обновлены";

                if (action == "ListMyCards")
                {
                    return RedirectToAction(action, "UserBoard");
                }

                else
                {
                    //await _userService.SendNotification(card.UserId, "Уведомление", "Ваша карточка " + card.Name + " была изменена");
                    var employeeId = Convert.ToInt32(action.Split("=")[1]);
                    action = action.Split("?")[0];
                    return RedirectToAction(action, "UserBoard", new { employeeId = employeeId });
                }
            }

            return NotFound(response.Description);
        }

        [HttpPost]
        public IActionResult Create(AddCard viewModel)
        {
            viewModel.Id = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            //ViewBag добавить ошибки и выводить их в этой формочке (если они есть)
            if (!ModelState.IsValid) return RedirectToAction("ListMyCards", "UserBoard");

            _cardService.Create(viewModel);

            return RedirectToAction("ListMyCards", "UserBoard");
        }

        [HttpPost]
        public IActionResult Delete(UserBoardView userBoardView)
        {
            _cardService.Delete(userBoardView.Id);

            return RedirectToAction("ListMyCards", "UserBoard");
        }
       
        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var userId = Convert.ToInt32(User.FindFirst("Id").Value);

            IFormFile fileToUpload = Request.Form.Files[0];
            int cardId = Convert.ToInt32(Request.Form["cardId"]);

            var response = await _cardService.UploadFile(fileToUpload, userId, cardId);

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Файл успешно добавлен!");
            }

            return NotFound(response.Description);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile()
        {
            int fileId = Convert.ToInt32(Request.Form["fileId"]);
            int cardId = Convert.ToInt32(Request.Form["cardId"]);

            var response = await _cardService.DeleteFile(fileId, cardId);

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