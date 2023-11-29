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

       
    }
}