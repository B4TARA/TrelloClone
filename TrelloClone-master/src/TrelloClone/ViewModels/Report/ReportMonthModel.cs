using System.Collections.Generic;

namespace TrelloClone.ViewModels.Report
{
    public class ReportMonthModel
    {
        public string viewDate { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public List<ReportCardModel> ReportCardModelList { get; set; } = new List<ReportCardModel>();
    }
}
