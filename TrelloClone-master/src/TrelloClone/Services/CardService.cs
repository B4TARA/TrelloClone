using EmailService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Term;
using TrelloClone.ViewModels;
using StatusCodes = TrelloClone.Models.Enum.StatusCodes;

namespace TrelloClone.Services
{
    public class CardService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly TrelloCloneDbContext _dbContext;
        private readonly RepositoryManager _repository;
        private readonly EmailSender _emailSender;

        public CardService(TrelloCloneDbContext dbContext, RepositoryManager repository, IHostingEnvironment hostingEnvironment
            , EmailSender emailSender)
        {
            _dbContext = dbContext;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
        }

        DateTime FakeToday = new DateTime(2025, 1, 1, 12, 10, 25);

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
                    IsRelevant = true,
                    IsDeleted = false,
                });
            }

            _dbContext.SaveChanges();
        }

        public async Task<IBaseResponse<object>> Update(CardDetails cardDetails, string userName, string userImg)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardDetails.Id);
                var columns = await _repository.ColumnRepository.GetColumnsByUser(false, card.UserId);

                if (card.Name != cardDetails.Name)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Название\" с \"" + card.Name + "\" на \"" + cardDetails.Name + "\""
                    });

                    card.Name = cardDetails.Name;
                }

                //перенос
                if (card.Term != cardDetails.Term)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Плановый срок реализации\" с \"" + card.Term + "\" на \"" + cardDetails.Term + "\""
                    });

                    //перенос на другой квартал
                    if (Term.GetQuarter(card.Term) != Term.GetQuarter(cardDetails.Term))
                    {
                        card.ColumnId = columns.First(x => x.Number == 3).Id;
                    }

                    card.Term = cardDetails.Term;
                }

                if (card.Requirement != cardDetails.Requirement)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Требование к SMART-задаче\" с \"" + card.Requirement + "\" на \"" + cardDetails.Requirement + "\""
                    });

                    card.Requirement = cardDetails.Requirement;
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

        public async Task<IBaseResponse<object>> GiveSupervisorRating(
            int cardId,
            string Name,
            DateTime Term,
            DateTime FactTerm,
            string Requirement,
            int SupervisorAssessment,
            string SupervisorComment,
            string userName,
            string userImg)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardId);
                var columns = await _repository.ColumnRepository.GetColumnsByUser(false, card.UserId);

                if (card.Name != Name)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Название\" с \"" + card.Name + "\" на \"" + Name + "\""
                    });

                    card.Name = Name;
                }

                //Фактическая дата исполнения
                card.FactTerm = FactTerm;

                card.SupervisorAssessment = SupervisorAssessment;
                card.SupervisorComment = SupervisorComment;

                //перенос
                if (card.Term != Term)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Плановый срок реализации\" с \"" + card.Term + "\" на \"" + Term + "\""
                    });

                    //перенос на другой квартал
                    if (Models.Term.Term.GetQuarter(card.Term) != Models.Term.Term.GetQuarter(Term))
                    {
                        card.ColumnId = columns.First(x => x.Number == 3).Id;
                        card.SupervisorAssessment = 8;
                    }

                    card.Term = Term;
                }

                if (card.Requirement != Requirement)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Требование к SMART-задаче\" с \"" + card.Requirement + "\" на \"" + Requirement + "\""
                    });

                    card.Requirement = Requirement;
                }

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    UserName = userName,
                    UserImg = userImg,
                    Date = FakeToday,
                    Content = "Выставил(а) оценочное суждение непосредственного руководителя",
                });

                //просрочка
                if (card.SupervisorAssessment == 7)
                {
                    card.ColumnId = card.ColumnId - 2;
                    card.Term = FakeToday;
                }

                else if (card.SupervisorAssessment != 8)
                {
                    card.ColumnId = card.ColumnId + 1;
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
                    Description = $"[GiveSupervisorRating] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> GiveEmployeeRating(
            int cardId,
            string Name,
            DateTime Term,
            string Requirement,
            int EmployeeAssessment,
            string EmployeeComment,
            string userName,
            string userImg)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardId);
                var columns = await _repository.ColumnRepository.GetColumnsByUser(false, card.UserId);

                if (card.Name != Name)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Название\" с \"" + card.Name + "\" на \"" + Name + "\""
                    });

                    card.Name = Name;
                }

                card.EmployeeAssessment = EmployeeAssessment;
                card.EmployeeComment = EmployeeComment;

                //перенос
                if (card.Term != Term)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Плановый срок реализации\" с \"" + card.Term + "\" на \"" + Term + "\""
                    });

                    //перенос на другой квартал
                    if (Models.Term.Term.GetQuarter(card.Term) != Models.Term.Term.GetQuarter(Term))
                    {
                        card.ColumnId = columns.First(x => x.Number == 3).Id;
                    }

                    card.Term = Term;
                }

                if (card.Requirement != Requirement)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        UserName = userName,
                        UserImg = userImg,
                        Date = FakeToday,
                        Content = "Изменил(а) \"Требование к SMART-задаче\" с \"" + card.Requirement + "\" на \"" + Requirement + "\""
                    });

                    card.Requirement = Requirement;
                }

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    UserName = userName,
                    UserImg = userImg,
                    Date = FakeToday,
                    Content = "Выставил(а) оценочное суждение работника",
                });


                card.ColumnId = card.ColumnId + 1;
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
                    Description = $"[GiveEmployeeRating] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> Delete(int id)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, id);

                card.IsRelevant = false;
                card.IsDeleted = true;

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
                    Description = $"[Delete] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<object>> UploadFile(IFormFile fileToUpload, int userId, int cardId, string userName, string userImg)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardId);

                string path = "/files/" + fileToUpload.FileName;
                using (var fileStream = new FileStream(_hostingEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await fileToUpload.CopyToAsync(fileStream);
                }

                card.Files.Add(new Models.File
                {
                    Name = fileToUpload.FileName,
                    Size = fileToUpload.Length,
                    Type = fileToUpload.ContentType,
                    Path = path,
                    CardId = cardId,
                    UserId = userId
                });

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    UserName = userName,
                    UserImg = userImg,
                    Date = FakeToday,
                    Content = "Прикрепил(а) файл : \"" + fileToUpload.FileName + "\""
                });

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
                    Description = $"[UploadFile] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> DeleteFile(int fileId, int cardId, string userName, string userImg)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardId);

                var fileToDelete = await _repository.FileRepository.GetFileById(false, fileId);

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    UserName = userName,
                    UserImg = userImg,
                    Date = FakeToday,
                    Content = "Открепил(а) файл : \"" + fileToDelete.Name + "\""
                });

                if (System.IO.File.Exists(fileToDelete.Path))
                {
                    System.IO.File.Delete(fileToDelete.Path);
                }

                _repository.FileRepository.Delete(fileToDelete);
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
                    Description = $"[DeleteFile] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> AddComment(string comment, int userId, string userName, string userImg, int cardId)
        {
            try
            {
                var card = await _repository.CardRepository.GetCardById(false, cardId);

                card.Comments.Add(new Comment
                {
                    CardId = cardId,
                    UserId = userId,
                    UserName = userName.Split(" ")[0] + " " + userName.Split(" ")[1],
                    Content = comment,
                    UserImg = userImg
                });

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    UserName = userName,
                    UserImg = userImg,
                    Date = FakeToday,
                    Content = "Добавил(а) комментарий"
                });

                //Уведомление//
                var content = "<div>" +
                    "Информируем, что ‘ФИО того, кто внес комментарий’ оставил(а) комментарий в задаче ‘Наименование задачи’." +
                    "Заполнение SMART-задач доступно по ссылке:" +
                        "<a href= \"https://10.117.11.77:44370/Account/LogOut\" target = \"blanc\">Посмотреть задачу можно по ссылке<a/>" +
                        "<br>" +
                        "<div>" + "или через ярлык на рабочем столе" + "<div>" +
                        "<span style=\"width:50px; height:50px;\">" +
                            "<img style=\"width:50px; height:50px;\" src='cid:{0}'>" +
                        "</span>" +
                "</div>";

                var message = new Message(new string[] { "yatomchik@mtb.minsk.by" }, "Уведомление", content, "yatomchik@mtb.minsk.by");
                //await _emailSender.SendEmailAsync(message);
                ///////////////

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
                    Description = $"[AddComment] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }
    }
}