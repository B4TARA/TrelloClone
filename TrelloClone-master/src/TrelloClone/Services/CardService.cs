using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.ViewModels;

namespace TrelloClone.Services
{
    public class CardService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly TrelloCloneDbContext _dbContext;
        private readonly RepositoryManager _repository;
        private readonly UserBoardService _userBoardService;

        public CardService(TrelloCloneDbContext dbContext, RepositoryManager repository, UserBoardService userBoardService, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userBoardService = userBoardService;
            _hostingEnvironment = hostingEnvironment;
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
                    IsRelevant = true
                });;
            }

            _dbContext.SaveChanges();
        }

        public async Task<IBaseResponse<object>> Update(CardDetails cardDetails, int userId, string userImg)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardDetails.Id);
                card.Name = cardDetails.Name;
                card.Requirement = cardDetails.Requirement;              
                card.EmployeeAssessment = cardDetails.EmployeeAssessment;
                card.EmployeeComment = cardDetails.EmployeeComment;
                card.SupervisorAssessment = cardDetails.SupervisorAssessment;
                card.SupervisorComment = cardDetails.SupervisorComment;  
                
                if(cardDetails.Comment != null)
                {
                    card.Comments.Add(new Comment { CardId = cardDetails.Id, UserId = userId, Content = cardDetails.Comment, UserImg = userImg});
                }

                if(cardDetails.File != null)
                {
                    string path = "/files/" + cardDetails.File.FileName;
                    using (var fileStream = new FileStream(_hostingEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await cardDetails.File.CopyToAsync(fileStream);
                    }
                    Models.File file = new Models.File { Name = cardDetails.File.FileName, Path = path, CardId = cardDetails.Id, UserId = userId };
                    card.Files.Add(file);
                }

                //выставление баллов
                if(card.SupervisorAssessment != 0
                    && card.SupervisorAssessment != 8
                    && card.SupervisorAssessment != 9)
                {
                    card.Points = MarksAndPoints.Points[cardDetails.SupervisorAssessment];
                }
                
                //просрочено
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

                //перенос
                if (card.Term != cardDetails.Term)
                {
                    card.Term = cardDetails.Term;

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

        public void DeleteFile(int id)
        {
            var file = _dbContext.Files.SingleOrDefault(x => x.Id == id);
            _dbContext.Remove(file ?? throw new Exception($"Could not remove {(Card)null}"));

            _dbContext.SaveChanges();
        }

        public void AddComment(int userId, int cardId, string comment)
        {
            var card = _dbContext.Cards
                 .Include(b => b.Comments)
                 .SingleOrDefault(x => x.Id == cardId);

            if (card != null)
            {

                card.Comments.Add(new Comment
                {
                    CardId = cardId,
                    UserId = userId,
                    Content = comment,                   
                }); ;
            }

            _dbContext.SaveChanges();
        }
    }
}