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

        public async Task<UserBoardList> ListSubordinateUsers(int userId)
        {
            var supervisor = await _repository.UserRepository.GetUserById(false, userId);

            var model = new UserBoardList();
            model.Notifications = supervisor.Notifications;

            var subordinateUsers = await _repository.UserRepository.GetByCondition(x => x.SupervisorName == supervisor.Name, false);
            foreach (var user in subordinateUsers)
            {
                model.Users.Add(new UserBoardList.UserBoard
                {
                    Id = user.Id,
                    Name = user.Name
                });
            }

            return model;
        }

        public UserBoardView ListMyCards(int userId)
        {
            var model = new UserBoardView();

            var user = _dbContext.Users
                .Include(b => b.Columns)
                .ThenInclude(c => c.Cards)
                .SingleOrDefault(x => x.Id == userId);

            model.IsActiveLikeEmployee = user.IsActiveLikeEmployee;
            model.Id = userId;
            model.Name = user.Name;
            model.Notifications = user.Notifications;

            foreach (var column in user.Columns)
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
                var firstColumn = user.Columns.FirstOrDefault();
                var secondColumn = user.Columns.FirstOrDefault();
                var thirdColumn = user.Columns.FirstOrDefault();
                var fourthColumn = user.Columns.FirstOrDefault();
                var fifthColumn = user.Columns.FirstOrDefault();
                var sixthColumn = user.Columns.FirstOrDefault();

                if (firstColumn == null || secondColumn == null || thirdColumn == null)
                {
                    firstColumn = new Column { Title = "Составление задач" };
                    secondColumn = new Column { Title = "Согласование задач" };
                    thirdColumn = new Column { Title = "Задачи согласованы" };
                    fourthColumn = new Column { Title = "Оценка директора, Начальника ССП" };
                    fifthColumn = new Column { Title = "Оценка Куратора/Директора" };
                    sixthColumn = new Column { Title = "Оценка согласована" };

                    user.Columns.Add(firstColumn);
                    user.Columns.Add(secondColumn);
                    user.Columns.Add(thirdColumn);
                    user.Columns.Add(fourthColumn);
                    user.Columns.Add(fifthColumn);
                    user.Columns.Add(sixthColumn);
                }

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
