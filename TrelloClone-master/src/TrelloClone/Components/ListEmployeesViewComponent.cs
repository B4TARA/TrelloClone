using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.ViewModels;

namespace TrelloClone.Components
{
    public class ListEmployeesViewComponent : ViewComponent
    {
        private readonly RepositoryManager _repository;
        public ListEmployeesViewComponent(RepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = await GetModelAsync(id);

            return View("ListEmployees", model);
        }

        private async Task<UserBoardList> GetModelAsync(int id)
        {
            var model = new UserBoardList();

            var supervisor = await _repository.UserRepository.GetUserById(false, id);      
            var subordinateUsers = await _repository.UserRepository.GetByCondition(x => x.SupervisorName == supervisor!.Name, false);

            foreach (var user in subordinateUsers)
            {
                model.Users.Add(new UserBoardList.UserBoard
                {
                    Id = user.Id,
                    Name = user.Name
                });
            }

            return model;
        }
    }
}
