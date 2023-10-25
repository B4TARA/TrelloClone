using Microsoft.AspNetCore.Mvc;
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

        public UserBoardController(UserBoardService boardService)
        {
            _userBoardService = boardService;
        }

        [HttpPost("movecard")]
        public async Task<IActionResult> MoveCard([FromBody] MoveCardCommand command)
        {
            var response = await _userBoardService.Move(command);

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok(new
                {
                    Moved = true
                });
            }

            return NotFound(response.Description);           
        }
    }
}