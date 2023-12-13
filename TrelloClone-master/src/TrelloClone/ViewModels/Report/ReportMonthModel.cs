using System.Collections.Generic;

namespace TrelloClone.ViewModels.Report
{
    public class ReportMonthModel
    {
        public string MonthName { get; set; } = string.Empty;
        public List<ReportCardModel> ReportCardModelList { get; set; }
    }
}
