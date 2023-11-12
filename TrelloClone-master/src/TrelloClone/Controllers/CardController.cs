using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Models.Enum;
using TrelloClone.Services;
using TrelloClone.ViewModels;

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

            var action = Request.Headers.Referer.ToString().Split("/")[4];

            var response = await _cardService.Update(card, userId);

            if (response.StatusCode == StatusCodes.OK)
            {
                TempData["Message"] = "Данные обновлены";

                if (action == "ListMyCards")
                {
                    return RedirectToAction(action, "UserBoard");
                }

                else
                {
                    await _userService.SendNotification(card.UserId, "Уведомление", "Ваша карточка " + card.Name + " была изменена");
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

        
    }
}