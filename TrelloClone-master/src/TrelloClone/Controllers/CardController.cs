using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Update(EditCard cardDetails)
        {
            var response = await _cardService.Update(cardDetails);

            if (response.StatusCode == StatusCodes.OK)
            {
                TempData["Message"] = "Saved card Details";

                return RedirectToAction(nameof(Details), new { id = cardDetails.Id });
            }

            return NotFound(response.Description);           
        }
    }
}