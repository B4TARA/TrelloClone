using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.ViewModels.CardDetails;

namespace TrelloClone.Components
{
    public class CardInfoViewComponent : ViewComponent
    {
        private readonly RepositoryManager _repository;
        public CardInfoViewComponent(RepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int cardId, int userId)
        {
            var user = await _repository.UserRepository.GetUserById(false, userId);

            ////
            var cards = await _repository.CardRepository.FindBy(x => x.Id == cardId, x => x.Comments, x => x.Files, x => x.Updates);
            var card = cards.FirstOrDefault();
            ////

            var column = await _repository.ColumnRepository.GetColumnById(false, card.ColumnId);

            var model = new CardInfo
            {
                Id = card.Id,
                Name = card.Name,
                Requirement = card.Requirement,
                Term = card.Term,
                Column = column.Number,
                ColumnId = column.Id,
                IsActiveLikeEmployee = user.IsActiveLikeEmployee,
                IsActiveLikeSupervisor = user.IsActiveLikeSupervisor,
                UserId = card.UserId,
                Files = card.Files,
            };

            return View("CardInfo", model);
        }
    }
}
