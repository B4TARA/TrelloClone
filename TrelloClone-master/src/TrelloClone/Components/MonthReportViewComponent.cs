using Microsoft.AspNetCore.Mvc;
using TrelloClone.ViewModels.Report;

namespace TrelloClone.Components
{
    public class MonthReportViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ReportMonthModel reportMonthModel)
        {
            return View("MonthReport", reportMonthModel);
        }
    }
}