using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.ViewModels;

namespace TrelloClone.Services
{
    public class AccountService
    {
        private readonly RepositoryManager _repository;
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public AccountService(RepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                User? user = await _repository.UserRepository.GetUserByLogin(false, model.Login);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.Password != model.Password)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль"
                    };
                }

                var result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = Models.Enum.StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                //logger.Error(ex, $"[AccountService.Login] error: {ex.Message}");
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Login] : {ex.Message}",
                    StatusCode = Models.Enum.StatusCodes.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {

            List<Claim> claims = new List<Claim>
            {

                new Claim(ClaimTypes.Role, user.Roles.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Roles.ToString()),
                new Claim("Id", user.Id.ToString())
            };

            if (user.Login == null)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, "Default"));
            }
            else
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login));
            }

            if (user.Name == null)
            {
                claims.Add(new Claim("Name", "Default"));
            }
            else
            {
                claims.Add(new Claim("Name", user.Name));
            }

            if (user.ImagePath == null)
            {
                claims.Add(new Claim("ImagePath", "Default"));
            }
            else
            {
                claims.Add(new Claim("ImagePath", user.ImagePath));
            }

            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
