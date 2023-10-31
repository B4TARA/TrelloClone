using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.ViewModels;

namespace TrelloClone.Services
{
    public class CardService
    {
        private readonly TrelloCloneDbContext _dbContext;
        private readonly RepositoryManager _repository;
        private readonly UserBoardService _userBoardService;

        public CardService(TrelloCloneDbContext dbContext, RepositoryManager repository, UserBoardService userBoardService)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userBoardService = userBoardService;
        }
       
        public void Create(AddCard viewModel)
        {
            var user = _dbContext.Users
                .Include(b => b.Columns)
                .SingleOrDefault(x => x.Id == viewModel.Id);

            if (user != null)
            {
                var firstColumn = user.Columns.First();

                firstColumn.Cards.Add(new Card
                {
                    Name = viewModel.Name,
                    Requirement = viewModel.Requirement,
                    Term = viewModel.Term,
                    UserId = viewModel.Id,
                    IsActive = true,
                });;
            }

            _dbContext.SaveChanges();
        }

        public async Task<IBaseResponse<object>> Update(CardDetails cardDetails)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardDetails.Id);
                card.Name = cardDetails.Name;
                card.Requirement = cardDetails.Requirement;
                card.Term = cardDetails.Term;
                card.EmployeeAssessment = cardDetails.EmployeeAssessment;
                card.EmployeeComment = cardDetails.EmployeeComment;
                card.SupervisorAssessment = cardDetails.SupervisorAssessment;
                card.SupervisorComment = cardDetails.SupervisorComment;

                if(card.SupervisorAssessment != 0
                    && card.SupervisorAssessment != 8
                    && card.SupervisorAssessment != 9)
                {
                    card.Points = MarksAndPoints.Points[cardDetails.SupervisorAssessment];
                }
                
                if (card.SupervisorAssessment == 8)
                {
                    var column = await _repository.ColumnRepository.GetColumnById(false, card.ColumnId);

                    var addCard = new AddCard();
                    addCard.Id = column.UserId;
                    addCard.Name = cardDetails.Name;
                    addCard.Requirement = cardDetails.Requirement;
                    addCard.Term = cardDetails.Term.AddMonths(1);

                    Create(addCard);
                }

                else if (card.SupervisorAssessment == 9)
                {
                    var column = await _repository.ColumnRepository.GetColumnById(false, card.ColumnId);

                    var addCard = new AddCard();
                    addCard.Id = column.UserId;
                    addCard.Name = cardDetails.Name;
                    addCard.Requirement = cardDetails.Requirement;
                    addCard.Term = cardDetails.Term.AddMonths(1);

                    Create(addCard);
                }

                _repository.CardRepository.Update(card);
                await _repository.Save();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK,
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[Update] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public void Delete(int id)
        {
            var card = _dbContext.Cards.SingleOrDefault(x => x.Id == id);
            _dbContext.Remove(card ?? throw new Exception($"Could not remove {(Card) null}"));
            
            _dbContext.SaveChanges();
        }
        
    }
}