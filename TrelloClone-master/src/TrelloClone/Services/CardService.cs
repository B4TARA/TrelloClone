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
        
        public EditCard GetDetails(int id)
        {
            var card = _dbContext
                .Cards
                .Include(c => c.Column)
                .SingleOrDefault(x => x.Id == id);

            if (card == null) 
                return new EditCard();
           
            // retrieve users
            var user = _dbContext
                .Users
                .Include(b => b.Columns)
                .SingleOrDefault(x => x.Id == card.Column.UserId);

            // map user columns
            if (user != null) 
            {
                var availableColumns = user
                    .Columns
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    });


                return new EditCard
                {
                    Id = card.Id,
                    Name = card.Name,
                    Requirement = card.Requirement,
                    Term = card.Term,
                    Columns = availableColumns.ToList(), // list available columns
                    Column = card.ColumnId // map current column
                };
            }
            return null;
        }

        public async Task<IBaseResponse<object>> Update(EditCard cardDetails)
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

                    _userBoardService.AddCard(addCard);
                }

                else if (card.SupervisorAssessment == 9)
                {
                    var column = await _repository.ColumnRepository.GetColumnById(false, card.ColumnId);

                    var addCard = new AddCard();
                    addCard.Id = column.UserId;
                    addCard.Name = cardDetails.Name;
                    addCard.Requirement = cardDetails.Requirement;
                    addCard.Term = cardDetails.Term.AddMonths(1);

                    _userBoardService.AddCard(addCard);
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