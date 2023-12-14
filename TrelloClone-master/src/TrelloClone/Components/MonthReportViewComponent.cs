using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloClone.Data.Repositories;
using TrelloClone.Services;
using TrelloClone.ViewModels;
using TrelloClone.ViewModels.Report;

namespace TrelloClone.Components
{
    public class MonthReportViewComponent : ViewComponent
    {
        private readonly UserBoardService _userBoardService;

        public MonthReportViewComponent(UserBoardService boardService)
        {
            _userBoardService = boardService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string supervisorName, DateTime startDate, DateTime endDate)
        {
            var model = await GetModelAsync(supervisorName, startDate, endDate);

            return View("MonthReport", model);
        }

        private async Task<List<ReportMonthModel>> GetModelAsync(string supervisorName, DateTime startDate, DateTime endDate)
        {
            var model = await _userBoardService.GetReportView(supervisorName, startDate, endDate);         

            return model.Data;
        }
    }
}
