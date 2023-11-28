using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Models.Enum;
using TrelloClone.Services;
using TrelloClone.ViewModels;

namespace TrelloClone.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBoardController : ControllerBase
    {
        private readonly UserBoardService _userBoardService;
        private readonly CardService _cardService;

        public UserBoardController(UserBoardService boardService, CardService cardService)
        {
            _userBoardService = boardService;
            _cardService = cardService;

        }

        [HttpPost("movecard")]
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
                //TempData["Message"] = "Данные обновлены";
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
                //TempData["Message"] = "Данные обновлены";

                var employeeId = Convert.ToInt32(action.Split("=")[1]);
                action = action.Split("?")[0];
                return RedirectToAction(action, "UserBoard", new { employeeId = employeeId });
            }

            return NotFound(response.Description);
        }
    }
}