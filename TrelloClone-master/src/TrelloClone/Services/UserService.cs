using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using ExcelDataReader;
using System.Linq;
using TrelloClone.Models.Enum;

namespace TrelloClone.Services
{
    public class UserService
    {
        private readonly TrelloCloneDbContext _dbContext;

        public UserService(TrelloCloneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IBaseResponse<object>> ImportUsers(IEnumerable<User> users)
        {
            try
            {
                foreach (var user in users)
                {
                    User? userTemp = _dbContext.Users.FirstOrDefault(x => x.Name == user.Name);

                    if (userTemp != null)
                    {
                        userTemp.Position = user.Position;
                        userTemp.SspName = user.SspName;
                        userTemp.SupervisorName = user.SupervisorName;

                        _dbContext.Users.Update(userTemp);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        _dbContext.Add(user);
                        _dbContext.SaveChanges();
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
                        user.Role = Role.Supervisor;

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
                        user.Role = Role.Employee;
                        users.Add(user);
                    }
                    else
                    {
                        users.FirstOrDefault(x => x.Name == user.Name).Position = user.Position;
                        users.FirstOrDefault(x => x.Name == user.Name).SspName = user.SspName;
                        users.FirstOrDefault(x => x.Name == user.Name).SupervisorName = user.SupervisorName;
                        users.FirstOrDefault(x => x.Name == user.Name).Role = Role.Combined;
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
    }
}
