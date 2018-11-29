using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Model;

namespace TOReportApplication.ViewModels.interfaces
{
    public interface ISettingsAndFilterPanelViewModel
    {
        DataContextEnum DataContextEnum { get; set; }
        Action<FormReportsDBModel> FormReportsModelItemsAction { get; set; }
        Action<BlowingMachineReportDto> BlowingMachineReportsModelItemsAction { get; set; }
        Action<string> SaveBlowingMachineReportsInFileAction { get; set; }
    }
}
