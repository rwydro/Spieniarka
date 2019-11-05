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
        Action<object> SearchButtonClickedAction { get; set; }
    }
}
