using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        public async Task<IViewComponentResult> InvokeAsync(int cardId, int userId)
        {
            var user = await _repository.UserRepository.GetUserById(false, userId);

            ////
            var cards = await _repository.CardRepository.FindBy(x => x.Id == cardId, x => x.Comments);
            var card = cards.FirstOrDefault();
            ////
            
            var column = await _repository.ColumnRepository.GetColumnById(false, card.ColumnId);

            var model = new CardDetails
            {
                Id = card.Id,
                Name = card.Name,
                Requirement = card.Requirement,
                Term = card.Term,
                EmployeeAssessment = card.EmployeeAssessment,
                EmployeeComment = card.EmployeeComment,
                SupervisorAssessment = card.SupervisorAssessment,
                SupervisorComment = card.SupervisorComment,
                Column = column.Number, // map current column
                ColumnId = column.Id,
                IsActive = card.IsActive,
                IsActiveLikeEmployee = user.IsActiveLikeEmployee,
                IsActiveLikeSupervisor = user.IsActiveLikeSupervisor,
                UserId = card.UserId,
                Comments = card.Comments,
            };

            return View("CardDetails", model);
        }
    }
}
