using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                DateTime FakeToday = new DateTime(2023, 3, 20);

                var card = await _repository.CardRepository.GetCardById(false, command.CardId);
                var columns = await _repository.ColumnRepository.GetColumnsByUser(false, card.UserId);

                if (card.Name != command.Name)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = command.UserName,
                        UserImg = command.UserImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Название\" с \"" + card.Name + "\" на \"" + command.Name + "\""
                    });

                    card.Name = command.Name;
                }

                //перенос
                if (card.Term != command.Term)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = command.UserName,
                        UserImg = command.UserImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Плановый срок реализации\" с \"" + card.Term + "\" на \"" + command.Term + "\""
                    });

                    //перенос на другой квартал
                    if (Term.GetQuarter(card.Term) != Term.GetQuarter(command.Term))
                    {
                        card.ColumnId = columns.First(x => x.Number == 3).Id;
                    }

                    card.Term = command.Term;
                }

                else
                {
                    if (Term.GetQuarter(card.Term) == Term.GetQuarter(FakeToday) || Term.GetPreviousQuarter(card.Term) == Term.GetQuarter(FakeToday))
                    {
                        card.ColumnId = card.ColumnId + 1;
                    }

                    else if (Term.GetQuarter(card.Term) == Term.GetPreviousQuarter(FakeToday))
                    {
                        card.ColumnId = card.ColumnId + 2;
                    }
                }

                if (card.Requirement != command.Requirement)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = command.UserName,
                        UserImg = command.UserImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Требование к SMART-задаче\" с \"" + card.Requirement + "\" на \"" + command.Requirement + "\""
                    });

                    card.Requirement = command.Requirement;
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
