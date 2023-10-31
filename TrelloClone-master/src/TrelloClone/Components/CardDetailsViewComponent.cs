using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.ViewModels;

namespace TrelloClone.Components
{
    public class CardDetailsViewComponent : ViewComponent
    {
        private readonly RepositoryManager _repository;
        public CardDetailsViewComponent(RepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int cardId)
        {          
            var card = await _repository.CardRepository.GetCardById(false, cardId);

            var model = new CardDetails
            {
                Id = card.Id,
                Name = card.Name,
                Requirement = card.Requirement,
                Term = card.Term,
                //Columns = availableColumns.ToList(), // list available columns
                Column = card.ColumnId // map current column
            };

            return View("CardDetails", model);
        }
    }
}
