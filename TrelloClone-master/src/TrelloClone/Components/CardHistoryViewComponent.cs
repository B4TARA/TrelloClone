using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.ViewModels;

namespace TrelloClone.Components
{
    public class CardHistoryViewComponent : ViewComponent
    {
        private readonly RepositoryManager _repository;
        public CardHistoryViewComponent(RepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int cardId)
        {
            var model = await GetModelAsync(cardId);

            return View("CardHistory", model);
        }

        private async Task<CardDetails> GetModelAsync(int cardId)
        {
            var cards = await _repository.CardRepository.FindBy(x => x.Id == cardId, x => x.Comments, x => x.Files, x => x.Updates);
            var card = cards.FirstOrDefault();

            var column = await _repository.ColumnRepository.GetColumnById(false, card!.ColumnId);

            return new CardDetails
            {
                Id = card.Id,
                Name = card.Name,
                Requirement = card.Requirement,
                Term = card.Term,
                FactTerm = card.FactTerm,
                EmployeeAssessment = card.EmployeeAssessment,
                SupervisorAssessment = card.SupervisorAssessment,
                EmployeeComment = card.EmployeeComment,
                SupervisorComment = card.SupervisorComment,
                Column = column!.Number,
                ColumnId = column.Id,
                UserId = card.UserId,
                Comments = card.Comments,
                Files = card.Files,
                Updates = card.Updates,
            };
        }
    }
}
