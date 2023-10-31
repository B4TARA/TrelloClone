using EmailService;
using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.ViewModels;
using TrelloClone.ViewModels.XML;

namespace TrelloClone.Services
{
    public class UserService
    {
        private readonly TrelloCloneDbContext _dbContext;
        private readonly RepositoryManager _repository;
        private readonly EmailSender _emailSender;

        public UserService(TrelloCloneDbContext dbContext, RepositoryManager repository, EmailSender emailSender)
        {
            _dbContext = dbContext;
            _repository = repository;
            _emailSender = emailSender;
        }

        public async Task<IBaseResponse<object>> CheckForNotifications(List<User> users)
        {
            try
            {
                var cards = await _repository.CardRepository.GetAll(false);

                DateTime FakeToday = new DateTime(2023, 4, 8);

                foreach (var card in cards)
                {

                    if (FakeToday.Month == 3
                        || FakeToday.Month == 6
                        || FakeToday.Month == 9
                        || FakeToday.Month == 12)
                    {

                        if (FakeToday.Day == 20)
                        {
                            card.IsActive = true;

                            _repository.CardRepository.Update(card);
                            await _repository.Save();
                        }

                        else if (FakeToday.Day == 25)
                        {
                            card.IsActive = true;

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
                            card.ColumnId = card.ColumnId + 1;
                            card.IsActive = true;

                            _repository.CardRepository.Update(card);
                            await _repository.Save();

                        }

                        else if (FakeToday.Day == 8)
                        {
                            card.IsActive = true;

                            _repository.CardRepository.Update(card);
                            await _repository.Save();

                        }
                    }
                }

                foreach (var user in users/*.Where(x=>x.Name.Contains("Томчик"))*/)
                {

                    if (FakeToday.Month == 3
                        || FakeToday.Month == 6
                        || FakeToday.Month == 9
                        || FakeToday.Month == 12)
                    {

                        if (FakeToday.Day == 20 && (user.Role == Roles.Employee || user.Role == Roles.Combined))
                        {                          
                            user.IsActiveToAddCard = true;
                            user.IsActiveLikeEmployee = true;
                            user.Notifications.Add("Внесите задачи на каждый месяц будущего квартала до 24");

                            var message = new Message(new string[] { "yatomchik@mtb.minsk.by" }, "Напоминание", "Внесите задачи на каждый месяц будущего квартала до 24", null);
                            //await _emailSender.SendEmailAsync(message);
                        }

                        else if (FakeToday.Day == 25)
                        {                           
                            user.IsActiveToAddCard = false;
                            user.IsActiveLikeEmployee = false;

                            if (user.Role == Roles.Supervisor || user.Role == Roles.Combined)
                            {
                                user.IsActiveLikeSupervisor = true;
                                user.Notifications.Add("Согласуйте задачи на каждый месяц будущего квартала до конца месяца");

                                var message = new Message(new string[] { "evgeniybaturel@gmail.com" }, "Напоминание", "Согласуйте задачи на каждый месяц будущего квартала до конца месяца", null);
                                //await _emailSender.SendEmailAsync(message);
                            }
                        }
                    }

                    else if (FakeToday.Month == 4
                        || FakeToday.Month == 7
                        || FakeToday.Month == 10
                        || FakeToday.Month == 1)
                    {

                        if (FakeToday.Day == 1)
                        {                      
                            user.IsActiveLikeSupervisor = false;

                            if (user.Role == Roles.Employee || user.Role == Roles.Combined)
                            {
                                user.IsActiveLikeEmployee = true;
                                user.Notifications.Add("Внесите оценки по задачам отчетного квартала до 7");

                                var message = new Message(new string[] { "yatomchik@mtb.minsk.by" }, "Напоминание", "Внесите оценки по задачам отчетного квартала до 7", null);
                                //await _emailSender.SendEmailAsync(message);
                            }
                        }

                        else if (FakeToday.Day == 8)
                        {                         
                            user.IsActiveLikeEmployee = false;

                            if (user.Role == Roles.Supervisor || user.Role == Roles.Combined)
                            {
                                user.IsActiveLikeSupervisor = true;
                                user.Notifications.Add("Согласуйте оценки по задачам отчетного квартала до 14");

                                var message = new Message(new string[] { "evgeniybaturel@gmail.com" }, "Напоминание", "Согласуйте оценки по задачам отчетного квартала до 14", null);
                                //await _emailSender.SendEmailAsync(message);
                            }
                        }

                        else if (FakeToday.Day == 14)
                        {
                            user.IsActiveLikeSupervisor = false;
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

                var notificationsResponse = await CheckForNotifications(users.ToList());
                if (notificationsResponse.StatusCode != StatusCodes.OK)
                {
                    Console.WriteLine("Unable to check for notifications : " + notificationsResponse.Description);
                    throw new Exception(notificationsResponse.Description);
                }

                foreach (var user in users)
                {
                    //set image, login, birthday and etc.//
                    ExtendedUser? extendedUserInfoRecord = extendedUserInfoRecords.FirstOrDefault(x => x.lastname == user.Name.Split(" ").FirstOrDefault());

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

                        userTemp.IsActiveLikeEmployee = user.IsActiveLikeEmployee;
                        userTemp.IsActiveLikeSupervisor = user.IsActiveLikeSupervisor;
                        userTemp.IsActiveToAddCard = user.IsActiveToAddCard;

                        foreach (var notification in user.Notifications)
                        {
                            if (!userTemp.Notifications.Contains(notification))
                            {
                                userTemp.Notifications.Add(notification);
                            }
                        }

                        userTemp.Login = user.Login;
                        userTemp.Password = user.Password;
                        userTemp.ImagePath = user.ImagePath;

                        _repository.UserRepository.Update(userTemp);
                        await _repository.Save();
                    }

                    else
                    {
                        if (user.Role == Roles.Employee || user.Role == Roles.Combined)
                        {
                            var firstColumn = new Column { Title = "Составление задач", Number = 1 };
                            var secondColumn = new Column { Title = "Согласование задач", Number = 2 };
                            var thirdColumn = new Column { Title = "Задачи согласованы", Number = 3 };
                            var fourthColumn = new Column { Title = "Оценка директора, Начальника ССП", Number = 4 };
                            var fifthColumn = new Column { Title = "Оценка Куратора/Директора", Number = 5 };
                            var sixthColumn = new Column { Title = "Оценка согласована", Number = 6 };

                            user.Columns.Add(firstColumn);
                            user.Columns.Add(secondColumn);
                            user.Columns.Add(thirdColumn);
                            user.Columns.Add(fourthColumn);
                            user.Columns.Add(fifthColumn);
                            user.Columns.Add(sixthColumn);
                        }

                        await _repository.UserRepository.CreateUser(user);
                        await _repository.Save();
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
                FileStream fStream = File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fStream);
                DataSet resultDataSet = excelDataReader.AsDataSet();
                var table = resultDataSet.Tables[1];

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
                //string cols_array = "C:\\Users\\tomchikadm\\Documents\\GitHub\\TrelloClone\\TrelloClone-master\\files\\cols_array.xml";
                string cols_array = "C:\\Users\\evgen\\OneDrive\\Документы\\GitHub\\TrelloClone\\TrelloClone-master\\files\\cols_array.xml";

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
    }
}
