using EmailService;
using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.Models.Term;
using TrelloClone.ViewModels;
using TrelloClone.ViewModels.XML;

namespace TrelloClone.Services
{
    public class UserService
    {
        private readonly RepositoryManager _repository;
        private readonly EmailSender _emailSender;

        public UserService(RepositoryManager repository, EmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }

        public async Task<IBaseResponse<object>> CheckForNotifications(User user)
        {
            try
            {
                var cards = await _repository.CardRepository.GetUserCards(false, user.Id);

                DateTime FakeToday = new DateTime(2023, 1, 8);

                foreach (var card in cards)
                {
                    var cardColumn = await _repository.ColumnRepository.GetColumnById(false, card.ColumnId);

                    if (FakeToday.Month == 3
                        || FakeToday.Month == 6
                        || FakeToday.Month == 9
                        || FakeToday.Month == 12)
                    {                       

                        if (FakeToday.Day == 25)
                        {
                            //25 числа все карточки с 1 колонки автоматически на 2 колонку, если они в нужном квартале
                            if(cardColumn.Number == 1 && Term.GetQuarter(card.Term) == Term.GetNextQuarter(FakeToday))
                            {
                                card.ColumnId = card.ColumnId + 1;
                            }

                            _repository.CardRepository.Update(card);
                            await _repository.Save();
                        }
                    }

                    else if (FakeToday.Month == 4
                        || FakeToday.Month == 7
                        || FakeToday.Month == 10
                        || FakeToday.Month == 1)
                    {

                        if (FakeToday.Day == 1)
                        {
                            //1 числа все карточки из 6 колонки, кроме переноса, отправялются в архив
                            if (cardColumn.Number == 6 && card.SupervisorAssessment != 7)
                            {
                                card.IsRelevant = false;
                            }
                            
                            //1 числа все карточки предыдущего квартала автоматически переходят с 3 колонки на 4
                            else if (cardColumn.Number == 3 && Term.GetQuarter(card.Term) == Term.GetPreviousQuarter(FakeToday))
                            {
                                card.ColumnId = card.ColumnId + 1;
                            }

                            
                            else if (cardColumn.Number == 2)
                            {
                                //1 числа все карточки все карточки переходят со 2 колонки на 3 если они в текущем квартале
                                if (Term.GetQuarter(card.Term) == Term.GetQuarter(FakeToday))
                                {
                                    card.ColumnId = card.ColumnId + 1;
                                }

                                //1 числа все карточки все карточки переходят со 2 колонки на 4 если они с предыдущего квартала
                                else if (Term.GetQuarter(card.Term) == Term.GetPreviousQuarter(FakeToday))
                                {
                                    card.ColumnId = card.ColumnId + 2;
                                }                            
                            }

                            _repository.CardRepository.Update(card);
                            await _repository.Save();
                        }

                        else if (FakeToday.Day == 8)
                        {
                            //8 числа все карточки автоматически переходят с 4 колонки на 5 и становятся активными
                            if (cardColumn.Number == 4)
                            {
                                card.ColumnId = card.ColumnId + 1;
                            }

                            _repository.CardRepository.Update(card);
                            await _repository.Save();
                        }
                    }
                }

                if (FakeToday.Month == 3
                    || FakeToday.Month == 6
                    || FakeToday.Month == 9
                    || FakeToday.Month == 12)
                {

                    if (FakeToday.Day == 20 && (user.Role == Roles.Employee || user.Role == Roles.Combined))
                    {
                        user.IsActiveLikeEmployee = true;
                        user.IsActiveLikeSupervisor = false;

                        await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[0]);
                    }

                    else if (FakeToday.Day == 23 && (user.Role == Roles.Employee || user.Role == Roles.Combined))
                    {
                        await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[1]);
                    }

                    else if (FakeToday.Day == 25)
                    {
                        //user.IsActiveLikeEmployee = false;

                        if (user.Role == Roles.Supervisor || user.Role == Roles.Combined)
                        {
                            user.IsActiveLikeSupervisor = true;

                            await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[2]);
                        }
                    }

                    else if (FakeToday.Day == 28 && (user.Role == Roles.Supervisor || user.Role == Roles.Combined))
                    {
                        await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[3]);
                    }           
                }

                else if (FakeToday.Month == 4
                    || FakeToday.Month == 7
                    || FakeToday.Month == 10
                    || FakeToday.Month == 1)
                {

                    if (FakeToday.Day == 1)
                    {
                        if (user.Role == Roles.Employee || user.Role == Roles.Combined)
                        {
                            user.IsActiveLikeEmployee = true;

                            await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[4]);
                        }
                    }

                    if (FakeToday.Day == 5 && (user.Role == Roles.Employee || user.Role == Roles.Combined))
                    {
                        await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[5]);
                    }

                    else if (FakeToday.Day == 8)
                    {
                        user.IsActiveLikeEmployee = false;

                        if (user.Role == Roles.Supervisor || user.Role == Roles.Combined)
                        {
                            user.IsActiveLikeSupervisor = true;

                            await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[6]);
                        }
                    }

                    if (FakeToday.Day == 11 && (user.Role == Roles.Employee || user.Role == Roles.Combined))
                    {
                        await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[7]);
                    }

                    else if (FakeToday.Day == 14)
                    {
                        user.IsActiveLikeSupervisor = false;
                        await SendNotification(user.Id, "Напоминание", Models.Mailing.Mailing.GetMails()[7]);
                    }
                }


                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[CheckForNotifications] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> ImportUsers(IEnumerable<User> users)
        {
            try
            {
                var response = GetJsonInfoByXml();
                if (response.StatusCode != StatusCodes.OK)
                {
                    return new BaseResponse<object>()
                    {
                        Description = response.Description,
                        StatusCode = StatusCodes.InternalServerError
                    };
                }
                IEnumerable<ExtendedUser> extendedUserInfoRecords = response.Data;           

                foreach (var user in users)
                {
                    //set image, login, birthday and etc.//
                    ExtendedUser? extendedUserInfoRecord = extendedUserInfoRecords.FirstOrDefault(x => x.lastname == user.Name.Split(" ")[0] &&  x.firstname == user.Name.Split(' ')[1]);

                    if (extendedUserInfoRecord != null
                        && extendedUserInfoRecord.pict_url != null
                        && extendedUserInfoRecord.fullname != null
                        && extendedUserInfoRecord.birth_date != null
                        && extendedUserInfoRecord.f_type != null
                        && extendedUserInfoRecord.email != null
                        && extendedUserInfoRecord.email.Contains("@"))
                    {
                        Base64Decode.Base64Decode.Base64ToImage(extendedUserInfoRecord.pict_url, extendedUserInfoRecord.fullname.Replace(" ", string.Empty), extendedUserInfoRecord.birth_date, extendedUserInfoRecord.f_type);

                        user.Login = extendedUserInfoRecord.email.Split('@').FirstOrDefault();
                        user.Password = user.Login;
                        if (extendedUserInfoRecord.f_type != "" && extendedUserInfoRecord.pict_url != "")
                        {
                            user.ImagePath = "../image/user_image/" + extendedUserInfoRecord.fullname.Replace(" ", string.Empty) + extendedUserInfoRecord.birth_date.Replace(".", string.Empty) + "." + extendedUserInfoRecord.f_type;
                        }
                        else
                        {
                            user.ImagePath = "../image/default_profile_icon.svg";
                        }
                    }

                    User? userTemp = await _repository.UserRepository.GetUserByName(false, user.Name);

                    if (userTemp != null)
                    {
                        userTemp.Position = user.Position;
                        userTemp.SspName = user.SspName;
                        userTemp.SupervisorName = user.SupervisorName;
                        userTemp.Login = user.Login;
                        userTemp.Password = user.Password;
                        userTemp.ImagePath = user.ImagePath;

                        _repository.UserRepository.Update(userTemp);
                        await _repository.Save();

                        var notificationsResponse = await CheckForNotifications(userTemp);
                        if (notificationsResponse.StatusCode != StatusCodes.OK)
                        {
                            Console.WriteLine("Unable to check for notifications for user " + userTemp.Id + " : " + notificationsResponse.Description);
                            throw new Exception(notificationsResponse.Description);
                        }                
                    }

                    else
                    {
                        if (user.Role == Roles.Employee || user.Role == Roles.Combined)
                        {
                            var firstColumn = new Column { Title = "Составление SMART-задач работником", Number = 1 };
                            var secondColumn = new Column { Title = "Согласование SMART-задач руководителем", Number = 2 };
                            var thirdColumn = new Column { Title = "SMART-задачи к исполнению", Number = 3 };
                            var fourthColumn = new Column { Title = "Самооценка работника", Number = 4 };
                            var fifthColumn = new Column { Title = "Оценка непосредственного руководителя", Number = 5 };
                            var sixthColumn = new Column { Title = "Итоговая оценка по SMART-задачам", Number = 6 };

                            user.Columns.Add(firstColumn);
                            user.Columns.Add(secondColumn);
                            user.Columns.Add(thirdColumn);
                            user.Columns.Add(fourthColumn);
                            user.Columns.Add(fifthColumn);
                            user.Columns.Add(sixthColumn);
                        }

                        await _repository.UserRepository.CreateUser(user);
                        await _repository.Save();

                        var notificationsResponse = await CheckForNotifications(user);
                        if (notificationsResponse.StatusCode != StatusCodes.OK)
                        {
                            Console.WriteLine("Unable to check for notifications for user " + userTemp.Id + " : " + notificationsResponse.Description);
                            throw new Exception(notificationsResponse.Description);
                        }
                    }
                }

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[ImportUsers] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public IBaseResponse<IEnumerable<User>> ExportUsers(string path)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                FileStream fStream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fStream);
                DataSet resultDataSet = excelDataReader.AsDataSet();
                var table = resultDataSet.Tables[0];

                List<User> users = new List<User>();

                //Добавление непосредственных руководителей
                for (int rowCounter = 2; rowCounter <= table.Rows.Count - 1; rowCounter++)
                {
                    if (table.Rows[rowCounter][4] != DBNull.Value
                        && Convert.ToString(table.Rows[rowCounter][4]) != ""
                        //если еще не попадался
                        && users.FirstOrDefault(x => x.Name == Convert.ToString(table.Rows[rowCounter][4])) == null)
                    {
                        User user = new User();

                        user.Name = Convert.ToString(table.Rows[rowCounter][4]);
                        user.Role = Roles.Supervisor;

                        users.Add(user);
                    }
                }

                //Добавление сотрудников
                for (int rowCounter = 2; rowCounter <= table.Rows.Count - 1; rowCounter++)
                {
                    User user = new User();

                    for (int colCounter = 1; colCounter <= table.Columns.Count - 1; colCounter++)
                    {
                        switch (colCounter)
                        {
                            case 1:
                                if (table.Rows[rowCounter][colCounter] != DBNull.Value
                                    && Convert.ToString(table.Rows[rowCounter][colCounter]) != "")
                                {
                                    user.Name = Convert.ToString(table.Rows[rowCounter][colCounter]);
                                }
                                break;

                            case 2:
                                if (table.Rows[rowCounter][colCounter] != DBNull.Value && Convert.ToString(table.Rows[rowCounter][colCounter]) != "")
                                {
                                    user.Position = Convert.ToString(table.Rows[rowCounter][colCounter]);
                                }
                                break;

                            case 3:
                                if (table.Rows[rowCounter][colCounter] != DBNull.Value && Convert.ToString(table.Rows[rowCounter][colCounter]) != "")
                                {
                                    user.SspName = Convert.ToString(table.Rows[rowCounter][colCounter]);
                                }
                                break;

                            case 4:
                                if (table.Rows[rowCounter][colCounter] != DBNull.Value && Convert.ToString(table.Rows[rowCounter][colCounter]) != "")
                                {
                                    user.SupervisorName = Convert.ToString(table.Rows[rowCounter][colCounter]);
                                }
                                break;
                        }
                    }

                    //если не попадался как руководитель
                    if (users.FirstOrDefault(x => x.Name == user.Name) == null)
                    {
                        user.Role = Roles.Employee;
                        users.Add(user);
                    }

                    else
                    {
                        users.FirstOrDefault(x => x.Name == user.Name).Position = user.Position;
                        users.FirstOrDefault(x => x.Name == user.Name).SspName = user.SspName;
                        users.FirstOrDefault(x => x.Name == user.Name).SupervisorName = user.SupervisorName;
                        users.FirstOrDefault(x => x.Name == user.Name).Role = Roles.Combined;
                    }
                }

                excelDataReader.Close();

                return new BaseResponse<IEnumerable<User>>()
                {
                    StatusCode = StatusCodes.OK,
                    Data = users
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<User>>()
                {
                    Description = $"[ExportUsers] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public IBaseResponse<IEnumerable<ExtendedUser>> GetJsonInfoByXml()
        {
            try
            {
                string cols_array = "C:\\Users\\tomchikadm\\Documents\\GitHub\\TrelloClone\\TrelloClone-master\\files\\cols_array.xml";
                //string cols_array = "C:\\Users\\evgen\\OneDrive\\Документы\\GitHub\\TrelloClone\\TrelloClone-master\\files\\cols_array.xml";

                Values values = Deserealization.Deserealization.DeserializeToObject<Values>(cols_array);
                List<ExtendedUser> extendedUserInfoRecords = new List<ExtendedUser>();
                foreach (var value in values.values)
                {
                    ExtendedUser? extendedUserInfoRecord = JsonConvert.DeserializeObject<ExtendedUser>(value.json);
                    if (extendedUserInfoRecord == null)
                    {
                        continue;
                    }

                    extendedUserInfoRecord.fullname = extendedUserInfoRecord.lastname + " " + extendedUserInfoRecord.firstname + " " + extendedUserInfoRecord.middlename;

                    extendedUserInfoRecords.Add(extendedUserInfoRecord);
                }

                return new BaseResponse<IEnumerable<ExtendedUser>>()
                {
                    Data = extendedUserInfoRecords,
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ExtendedUser>>()
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<object>> SendNotification(int userId, string subject, string content)
        {
            try
            {
                var user = await _repository.UserRepository.GetUserById(false, userId);

                var message = new Message(new string[] { user.Login + "@mtb.minsk.by" }, subject, content, user.Name);
                await _emailSender.SendEmailAsync(message);

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK,
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[SendNotification] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }
    }
}
