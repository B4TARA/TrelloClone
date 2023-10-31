using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.Services;
using TrelloClone.ViewModels;

namespace TrelloClone.Controllers
{
    public class CardController : Controller
    {
        private readonly CardService _cardService;

        public CardController(CardService cardService)
        {
            _cardService = cardService;
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            var viewModel = _cardService.GetDetails(id);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CardDetails card)
        {
            var response = await _cardService.Update(card);

            if (response.StatusCode == StatusCodes.OK)
            {
                TempData["Message"] = "Saved card Details";

                return RedirectToAction(nameof(Details), new { id = card.Id });
            }

            return NotFound(response.Description);           
        }
    }
}