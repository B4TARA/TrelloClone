using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Assessment;
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

        public UserBoardView ListEmployeeCards(int employeeId, int supervisorId)
        {
            var model = new UserBoardView();

            var employee = _dbContext.Users
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Comments)
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Files)
                .SingleOrDefault(x => x.Id == employeeId);

            var supervisor = _dbContext.Users.SingleOrDefault(x => x.Id == supervisorId);

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

        public ListArchiveCards ListArchiveCards(int userId)
        {
            var model = new ListArchiveCards();

            var user = _dbContext.Users
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Comments)
                .Include(b => b.Columns).ThenInclude(c => c.Cards).ThenInclude(d => d.Files)
                .SingleOrDefault(x => x.Id == userId);

            model.Id = userId;
            model.Name = user!.Name;

            foreach (var column in user.Columns)
            {
                foreach (var card in column.Cards.Where(x => !x.IsRelevant))
                {
                    var modelCard = new ListArchiveCards.Card
                    {
                        Id = card.Id,
                        Name = card.Name,
                        Requirement = card.Requirement,
                        Term = card.Term,
                        CountOfComments = card.Comments.Count(),
                        CountOfFiles = card.Files.Count(),
                    };

                    if (card.FactTerm != null)
                    {
                        modelCard.FactTerm = (DateTime)card.FactTerm;
                    }
                    else
                    {
                        modelCard.FactTerm = card.Term;
                    }

                    if (AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.EmployeeAssessment) != null)
                    {
                        modelCard.EmployeeAssessment = AssessmentsForDropdown.GetAssessments().First(x => x.Id == card.EmployeeAssessment).Value;
                    }

                    if (AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment) != null)
                    {
                        modelCard.SupervisorAssessment = AssessmentsForDropdown.GetAssessments().First(x => x.Id == card.SupervisorAssessment).Value;
                    }

                    model.Cards.Add(modelCard);
                }
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
                DateTime FakeToday = Term.GetFakeDate();

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

                    //перенос на этот квартал 
                    else
                    {
                        card.ColumnId = card.ColumnId + 1;
                    }

                    card.Term = command.Term;
                }

                else
                {
                    if (card.ColumnId == columns.First(x => x.Number == 3).Id)
                    {
                        card.ColumnId = card.ColumnId + 1;
                    }

                    else if (Term.GetQuarter(card.Term) == Term.GetQuarter(FakeToday) || Term.GetPreviousQuarter(card.Term) == Term.GetQuarter(FakeToday))
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

        public async Task<IBaseResponse<string>> GetReport(string supervisorName)
        {
            try
            {
                DateTime FakeToday = Term.GetFakeDate();

                var quarter = Term.GetPreviousQuarter(FakeToday);
                var months = Term.GetQuarterMonths(quarter);

                var workbook = new XLWorkbook();

                foreach (var month in months)
                {
                    workbook.AddWorksheet(Term.GetMonthName(month));
                    var ws = workbook.Worksheet(Term.GetMonthName(month));

                    int row = 1;

                    ws.Cell("A" + row.ToString()).Value = "Наименование ССП";
                    ws.Cell("A" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("B" + row.ToString()).Value = "ФИО работника";
                    ws.Cell("B" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("C" + row.ToString()).Value = "Должность";
                    ws.Cell("C" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("D" + row.ToString()).Value = "Непосредственный руководитель";
                    ws.Cell("D" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("E" + row.ToString()).Value = "#";
                    ws.Cell("E" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("F" + row.ToString()).Value = "Наименование SMART-задачи";
                    ws.Cell("F" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("G" + row.ToString()).Value = "Требование к задаче (что считается исполнением задачи)";
                    ws.Cell("G" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("H" + row.ToString()).Value = "Плановый срок реализации задачи";
                    ws.Cell("H" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("I" + row.ToString()).Value = "Оценочное суждение работника";
                    ws.Cell("I" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("J" + row.ToString()).Value = "Комментарий работника";
                    ws.Cell("J" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("K" + row.ToString()).Value = "Оценочное суждение непосредственного руководителя";
                    ws.Cell("K" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("L" + row.ToString()).Value = "Комментарий непосредственного руководителя";
                    ws.Cell("L" + row.ToString()).Style.Font.Bold = true;

                    ws.Cell("M" + row.ToString()).Value = "% выполнения задачи в зависимости от присвоенного оценочного суждения непосредственного руководителя";
                    ws.Cell("M" + row.ToString()).Style.Font.Bold = true;

                    row++;

                    var employees = await _repository.UserRepository.GetByCondition(x => x.SupervisorName == supervisorName, false);
                    foreach (var employee in employees)
                    {
                        var cards = await _repository.CardRepository.GetUserCards(false, employee.Id);
                        foreach (var card in cards.Where(x => x.ReadyToReport))
                        {
                            ws.Cell("A" + row.ToString()).Value = "-";
                            if (employee.SspName != null)
                            {
                                ws.Cell("A" + row.ToString()).Value = employee.SspName.ToString();
                            }

                            ws.Cell("B" + row.ToString()).Value = "-";
                            if (employee.Name != null)
                            {
                                ws.Cell("B" + row.ToString()).Value = employee.Name.ToString();
                            }

                            ws.Cell("C" + row.ToString()).Value = "-";
                            if (employee.Position != null)
                            {
                                ws.Cell("C" + row.ToString()).Value = employee.Position.ToString();
                            }

                            ws.Cell("D" + row.ToString()).Value = "-";
                            if (employee.SupervisorName != null)
                            {
                                ws.Cell("D" + row.ToString()).Value = employee.SupervisorName.ToString();
                            }

                            ws.Cell("E" + row.ToString()).Value = "-";
                            if (employee.SupervisorName != null)
                            {
                                ws.Cell("E" + row.ToString()).Value = employee.SupervisorName.ToString();
                            }

                            ws.Cell("F" + row.ToString()).Value = "-";
                            if (card.Name != null)
                            {
                                ws.Cell("F" + row.ToString()).Value = card.Name.ToString();
                            }

                            ws.Cell("G" + row.ToString()).Value = "-";
                            if (card.Requirement != null)
                            {
                                ws.Cell("G" + row.ToString()).Value = card.Requirement.ToString();
                            }

                            ws.Cell("H" + row.ToString()).Value = card.Term.ToString();

                            ws.Cell("I" + row.ToString()).Value = "-";
                            if (AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.EmployeeAssessment) != null)
                            {
                                ws.Cell("I" + row.ToString()).Value = AssessmentsForDropdown.GetAssessments().First(x => x.Id == card.EmployeeAssessment).Text;
                            }

                            ws.Cell("J" + row.ToString()).Value = "-";
                            if (card.EmployeeComment != null)
                            {
                                ws.Cell("J" + row.ToString()).Value = card.EmployeeComment.ToString();
                            }

                            ws.Cell("K" + row.ToString()).Value = "-";
                            if (AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment) != null)
                            {
                                ws.Cell("K" + row.ToString()).Value = AssessmentsForDropdown.GetAssessments().First(x => x.Id == card.SupervisorAssessment).Text;
                            }

                            ws.Cell("L" + row.ToString()).Value = "-";
                            if (card.SupervisorComment != null)
                            {
                                ws.Cell("L" + row.ToString()).Value = card.SupervisorComment.ToString();
                            }

                            //Выставление баллов
                            if (card.SupervisorAssessment == 1)
                            {
                                if (card.Term.Month > month)
                                {
                                    ws.Cell("M" + row.ToString()).Value = "-";
                                }

                                else if (card.Term.Month == month)
                                {
                                    ws.Cell("M" + row.ToString()).Value = AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment).Value;
                                }

                                else
                                {
                                    ws.Cell("M" + row.ToString()).Value = "-";
                                }
                            }

                            else if (card.SupervisorAssessment == 2 ||
                                    card.SupervisorAssessment == 3 ||
                                    card.SupervisorAssessment == 4)
                            {
                                if (card.FactTerm.Value.Month > month && card.Term.Month <= month)
                                {
                                    ws.Cell("M" + row.ToString()).Value = "0%";
                                }

                                else if (card.FactTerm.Value.Month > month && card.Term.Month > month)
                                {
                                    ws.Cell("M" + row.ToString()).Value = "-";
                                }

                                else if (card.FactTerm.Value.Month == month)
                                {
                                    ws.Cell("M" + row.ToString()).Value = AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment).Value;
                                }

                                else
                                {
                                    ws.Cell("M" + row.ToString()).Value = "-";
                                }
                            }

                            else if (card.SupervisorAssessment == 5)
                            {
                                if (card.Term.Month > month)
                                {
                                    ws.Cell("M" + row.ToString()).Value = "-";
                                }

                                else if (card.Term.Month == month)
                                {
                                    ws.Cell("M" + row.ToString()).Value = AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment).Value;
                                }

                                else
                                {
                                    ws.Cell("M" + row.ToString()).Value = "-";
                                }
                            }

                            else if (card.SupervisorAssessment == 6
                                || card.SupervisorAssessment == 7
                                || card.SupervisorAssessment == 8
                                || card.SupervisorAssessment == 9)
                            {
                                ws.Cell("M" + row.ToString()).Value = AssessmentsForDropdown.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment).Value;
                            }
                            row++;
                        }

                        ws.Columns().AdjustToContents();
                    }
                }

                string prefixPath = "../TrelloClone/wwwroot/";
                string savePath = "Отчёт.xlsx";
                workbook.SaveAs(prefixPath + savePath);

                return new BaseResponse<string>()
                {
                    Data = savePath,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $"[GetReport] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}
