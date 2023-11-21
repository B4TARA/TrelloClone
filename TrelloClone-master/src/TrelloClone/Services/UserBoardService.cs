using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.Models.Term;
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
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Comments)
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Files)
                .SingleOrDefault(x => x.Id == userId);

            model.IsActiveLikeEmployee = user.IsActiveLikeEmployee;
            model.IsActiveToAddCard = user.IsActiveToAddCard;
            model.Id = userId;
            model.Name = user.Name;

            foreach (var column in user.Columns)
            {
                var modelColumn = new UserBoardView.Column
                {
                    Id = column.Id,
                    Number = column.Number,
                    Title = column.Title
                };

                foreach (var card in column.Cards.Where(x=>x.IsRelevant))
                {
                    var modelCard = new UserBoardView.Card
                    {
                        Id = card.Id,
                        Name = card.Name,
                        Requirement = card.Requirement,
                        Term = card.Term,
                        EmployeeAssessment = card.EmployeeAssessment,
                        SupervisorAssessment = card.SupervisorAssessment,
                        CountOfComments = card.Comments.Count(),
                        CountOfFiles = card.Files.Count(),
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
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Comments)
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Files)
                .SingleOrDefault(x => x.Id == employeeId);

            var supervisor = _dbContext.Users.SingleOrDefault(x => x.Id == supervisorId);

            model.IsActiveLikeSupervisor = supervisor.IsActiveLikeSupervisor;
            model.ImgPath = employee.ImagePath;

            model.Id = employee.Id;
            model.Name = employee.Name;

            foreach (var column in employee.Columns)
            {
                var modelColumn = new UserBoardView.Column
                {
                    Title = column.Title,
                    Number = column.Number,
                    Id = column.Id
                };

                foreach (var card in column.Cards.Where(x => x.IsRelevant))
                {
                    var modelCard = new UserBoardView.Card
                    {
                        Id = card.Id,
                        Name = card.Name,
                        Requirement = card.Requirement,
                        Term = card.Term,
                        EmployeeAssessment = card.EmployeeAssessment,
                        SupervisorAssessment = card.SupervisorAssessment,
                        CountOfComments = card.Comments.Count(),
                        CountOfFiles = card.Files.Count(),
                    };

                    modelColumn.Cards.Add(modelCard);
                }

                model.Columns.Add(modelColumn);
            }

            return model;
        }

        public UserBoardView ListArchiveCards(int userId)
        {
            var model = new UserBoardView();

            var user = _dbContext.Users
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Comments)
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Files)
                .SingleOrDefault(x => x.Id == userId);

            model.IsActiveLikeEmployee = user.IsActiveLikeEmployee;
            model.IsActiveToAddCard = user.IsActiveToAddCard;
            model.Id = userId;
            model.Name = user.Name;

            foreach (var column in user.Columns)
            {
                var modelColumn = new UserBoardView.Column
                {
                    Id = column.Id,
                    Number = column.Number,
                    Title = column.Title
                };

                foreach (var card in column.Cards.Where(x => !x.IsRelevant))
                {
                    var modelCard = new UserBoardView.Card
                    {
                        Id = card.Id,
                        Name = card.Name,
                        Requirement = card.Requirement,
                        Term = card.Term,
                        EmployeeAssessment = card.EmployeeAssessment,
                        SupervisorAssessment = card.SupervisorAssessment,
                        CountOfComments = card.Comments.Count(),
                        CountOfFiles = card.Files.Count(),
                    };

                    modelColumn.Cards.Add(modelCard);
                }

                model.Columns.Add(modelColumn);
            }

            return model;
        }

        public async Task<IBaseResponse<User>> GetFirstEmployee(int supervisorId)
        {
            try
            {
                var supervisor = await _repository.UserRepository.GetUserById(false, supervisorId);
                var employees = await _repository.UserRepository.GetByCondition(x => x.SupervisorName == supervisor.Name, false);

                return new BaseResponse<User>()
                {
                    Data = employees.FirstOrDefault(),
                    StatusCode = StatusCodes.OK,
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = $"[Move] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> Move(MoveCardCommand command)
        {
            try
            {
                DateTime FakeToday = new DateTime(2023, 10, 1);

                var card = await _repository.CardRepository.GetCardById(false, command.CardId);
                var cardColumn = await _repository.ColumnRepository.GetColumnById(false, card.ColumnId);

                if(cardColumn.Number != 1 || FakeToday.Day < 25)
                {
                    card.IsActive = false;
                }

                //25 числа все карточки с 1 колонки автоматически на 2 колонку, если они в нужном квартале
                if (Term.GetQuarter(card.Term) == Term.GetQuarter(FakeToday))
                {
                    card.ColumnId = card.ColumnId + 1;
                }

                //1 числа все карточки все карточки переходят со 2 колонки на 4 если они с предыдущего квартала
                else if (Term.GetQuarter(card.Term) == Term.GetPreviousQuarter(FakeToday))
                {
                    card.ColumnId = card.ColumnId + 2;
                    //card.IsActive = true;
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
                    Description = $"[Move] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> Reject(int CardId)
        {
            try
            {              
                var card = await _repository.CardRepository.GetCardById(false, CardId);

                card.ColumnId = card.ColumnId - 1;

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
                    Description = $"[Reject] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }
    }
}
