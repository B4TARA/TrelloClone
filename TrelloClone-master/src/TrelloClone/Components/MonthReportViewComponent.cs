using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrelloClone.ViewModels.Report;

namespace TrelloClone.Components
{
    public class MonthReportViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<ReportCardModel> model)
        {
            return View(model);
        }
    }
}
