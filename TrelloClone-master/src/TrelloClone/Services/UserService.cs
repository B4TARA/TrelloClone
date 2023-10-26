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

        public UserService(TrelloCloneDbContext dbContext, RepositoryManager repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<IBaseResponse<object>> CheckForNotifications(List<User> users)
        {
            try
            {
                DateTime FakeToday = new DateTime(2023, 10, 20);

                foreach(var user in users)
                {
                    switch(user.Role)
                    {
                        case Roles.Employee:
                            {

                                if(FakeToday.Day >= 20
                                    && FakeToday.Day <= 24)
                                {
                                    user.IsActiveLikeEmployee = true;
                                    user.Notifications.Add("Внесите задачи на каждый месяц будущего квартала до 24");
                                }

                                else if(FakeToday.Day >= 1
                                    && FakeToday.Day <= 7)
                                {
                                    user.IsActiveLikeEmployee = true;
                                    user.Notifications.Add("Внесите оценки по задачам отчетного квартала до 7");
                                }

                                break;
                            }
                        case Roles.Supervisor:
                            {
                                if (FakeToday.Day >= 25
                                   && FakeToday.Day <= 31)
                                {
                                    user.IsActiveLikeSupervisor = true;
                                    user.Notifications.Add("Согласуйте задачи на каждый месяц будущего квартала до конца месяца");
                                }

                                else if (FakeToday.Day >= 8
                                    && FakeToday.Day <= 14)
                                {
                                    user.IsActiveLikeSupervisor = true;
                                    user.Notifications.Add("Согласуйте оценки по задачам отчетного квартала до 14");
                                }

                                break;
                            }
                        case Roles.Combined:
                            {
                                if (FakeToday.Day >= 20
                                     && FakeToday.Day <= 24)
                                {
                                    user.IsActiveLikeEmployee = true;
                                    user.Notifications.Add("Внесите задачи на каждый месяц будущего квартала до 24");
                                }

                                else if (FakeToday.Day >= 1
                                    && FakeToday.Day <= 7)
                                {
                                    user.IsActiveLikeEmployee = true;
                                    user.Notifications.Add("Внесите оценки по задачам отчетного квартала до 7");
                                }

                                else if (FakeToday.Day >= 25
                                  && FakeToday.Day <= 31)
                                {
                                    user.IsActiveLikeSupervisor = true;
                                    user.Notifications.Add("Согласуйте задачи на каждый месяц будущего квартала до конца месяца");
                                }

                                else if (FakeToday.Day >= 8
                                    && FakeToday.Day <= 14)
                                {
                                    user.IsActiveLikeSupervisor = true;
                                    user.Notifications.Add("Согласуйте оценки по задачам отчетного квартала до 14");
                                }

                                break;
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
                        //user.IsActive = true;
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
    }
}
