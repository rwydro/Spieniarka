using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;

namespace TOReportApplication.ViewModels.interfaces
{
    public interface ISettingsAndFilterPanelViewModel<T> where T: ReportModelBase
    {
        DataContextEnum DataContextEnum { get; set; }
       // Action<FormReportsDBModel> FormReportsModelItemsAction { get; set; }
        event EventHandler<EventBaseArgs<T>> GeneratedModelItemsAction;
        void SetTimer(TimerActionEnum action);
        //Action IsReportGenerate { get; set; }
        event EventHandler IsReportGenerate;
    }
}
