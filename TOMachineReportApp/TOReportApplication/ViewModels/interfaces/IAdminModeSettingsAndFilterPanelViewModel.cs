using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.ViewModels.interfaces
{
    public interface IAdminModeSettingsAndFilterPanelViewModel
    {
        DateTime SelectedFromDate { get; set; }
        DateTime SelectedToDate { get; set; }
        object SelectedMachine { get; set; }
        Action SearchButtonClickAction { get; set; }
    }
}
