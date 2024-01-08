using EmailService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.Models;
using TrelloClone.Models.Enum;
using TrelloClone.Services;
using TrelloClone.ViewModels;

namespace TrelloClone.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly RepositoryManager _repository;
        private readonly EmailSender _emailSender;

        public AccountController(AccountService accountService, RepositoryManager repository, EmailSender emailSender)
        {
            _accountService = accountService;
            _repository = repository;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var changePasswordLogin = TempData["changePasswordLogin"];

                if (changePasswordLogin != null)
                {
                    ViewBag.changePasswordLogin = TempData["changePasswordLogin"].ToString();
                }

                var changePasswordMessage = TempData["changePasswordMessage"];

                if (changePasswordMessage != null)
                {
                    ViewBag.changePasswordMessage = TempData["changePasswordMessage"].ToString();
                    ModelState.AddModelError("", changePasswordMessage.ToString());
                }

                return View();
            }

            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(model);

                if (response.StatusCode == StatusCodes.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data),
                        new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", response.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemindPassword(ChangePasswordViewModel model)
        {
            TempData["changePasswordLogin"] = model.Email;

            if (ModelState.IsValid)
            {
                var userToRemindPassword = await _repository.UserRepository.GetUserByEmail(false, model.Email);

                if (userToRemindPassword == null)
                {
                    TempData["changePasswordMessage"] = "Пользователя с таким email-ом не существует";
                    return RedirectToAction("Login", "Account");
                }

                var content = "Ваш логин - " + userToRemindPassword.Login + ". Ваш пароль - " + userToRemindPassword.Password;
                var message = new Message(new string[] { userToRemindPassword.Email }, "Напоминание", content, userToRemindPassword.Name);
                await _emailSender.SendEmailAsync(message);

                TempData["changePasswordMessage"] = "Ваши учетные данные успешно высланы на почту";

                return RedirectToAction("Login", "Account");
            }

            TempData["changePasswordMessage"] = "Invalid ModelState";

            return RedirectToAction("Login", "Account");
        }
    }
}
