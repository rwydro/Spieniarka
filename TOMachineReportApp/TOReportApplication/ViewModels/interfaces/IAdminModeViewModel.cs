using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Logic.Enums;

namespace TOReportApplication.ViewModels.interfaces
{
    public interface IAdminModeViewModel
    {
        void Dispose();
        void SaveInFile();
        Action<object> SearchButtonClickedAction { get; set; }
        ReadOnlyCollection<object> ReportModelToSaveInFile { get; set; }
    }
}
