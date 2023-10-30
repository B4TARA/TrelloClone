using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.ViewModels;

namespace TrelloClone.Services
{
    public class UserBoardService
    {
        private readonly TrelloCloneDbContext _dbContext;
        private readonly RepositoryManager _repository;

        public UserBoardService(TrelloCloneDbContext dbContext, RepositoryManager repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public UserBoardView ListMyCards(int userId)
        {
            var model = new UserBoardView();

            var user = _dbContext.Users
                .Include(b => b.Columns)
                .ThenInclude(c => c.Cards)
                .SingleOrDefault(x => x.Id == userId);

            model.IsActiveLikeEmployee = user.IsActiveLikeEmployee;
            model.IsActiveToAddCard = user.IsActiveToAddCard;
            model.Id = userId;
            model.Name = user.Name;
            model.Notifications = user.Notifications;

            foreach (var column in user.Columns)
            {
                var modelColumn = new UserBoardView.Column
                {
                    Id = column.Id,
                    Number = column.Number,
                    Title = column.Title
                };

                foreach (var card in column.Cards)
                {
                    var modelCard = new UserBoardView.Card
                    {
                        Id = card.Id,
                        Name = card.Name,
                        Requirement = card.Requirement,
                        Term = card.Term,
                        EmployeeAssessment = card.EmployeeAssessment,
                        EmployeeComment = card.EmployeeComment,
                        SupervisorAssessment = card.SupervisorAssessment,
                        SupervisorComment = card.SupervisorComment,
                        Points = card.Points
                    };

                    modelColumn.Cards.Add(modelCard);
                }

                model.Columns.Add(modelColumn);
            }

            return model;
        }

        public UserBoardView ListEmployeeCards(int employeeId, int supervisorId)
        {
            var model = new UserBoardView();

            var employee = _dbContext.Users
                .Include(b => b.Columns)
                .ThenInclude(c => c.Cards)
                .SingleOrDefault(x => x.Id == employeeId);

            var supervisor = _dbContext.Users
                .Include(b => b.Columns)
                .ThenInclude(c => c.Cards)
                .SingleOrDefault(x => x.Id == supervisorId);

            model.IsActiveLikeSupervisor = supervisor.IsActiveLikeSupervisor;
            model.ImgPath = employee.ImagePath;
            model.Notifications = supervisor.Notifications;

            model.Id = employee.Id;
            model.Name = employee.Name;           

            foreach (var column in employee.Columns)
            {
                var modelColumn = new UserBoardView.Column
                {
                    Title = column.Title,
                    Id = column.Id
                };

                foreach (var card in column.Cards)
                {
                    var modelCard = new UserBoardView.Card
                    {
                        Id = card.Id,
                        Name = card.Name,
                        Requirement = card.Requirement,
                        Term = card.Term,
                        EmployeeAssessment = card.EmployeeAssessment,
                        EmployeeComment = card.EmployeeComment,
                        SupervisorAssessment = card.SupervisorAssessment,
                        SupervisorComment = card.SupervisorComment,
                        Points = card.Points
                    };

                    modelColumn.Cards.Add(modelCard);
                }

                model.Columns.Add(modelColumn);
            }

            return model;
        }

        public void AddCard(AddCard viewModel)
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
                    Term = viewModel.Term
                });
            }

            _dbContext.SaveChanges();
        }

        public async Task<IBaseResponse<object>> Move(MoveCardCommand command)
        {
            try
            {
                var cards = await _repository.CardRepository.GetByCondition(x => x.ColumnId == command.ColumnId, false);
                var user = await _repository.UserRepository.GetUserById(false, command.UserId);

                foreach (var card in cards)
                {
                    card.ColumnId = command.ColumnId + 1;

                    _repository.CardRepository.Update(card);
                    await _repository.Save();
                }

                user.IsActiveLikeEmployee = false;
                user.IsActiveLikeSupervisor = false;
                user.Notifications.Clear();

                _repository.UserRepository.Update(user);
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
                    Description = $"[Move] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }
    }
}
