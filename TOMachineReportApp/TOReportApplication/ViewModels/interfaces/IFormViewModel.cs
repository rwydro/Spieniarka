using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.ViewModels.interfaces
{
    public enum FormDetailedReportTypeEnum
    {
        ShortVersionDetailedReport = 0,
        FullVersionDetailedReport = 1,
    }

    public interface IFormViewModel
    {
        void Dispose();
        void OnCommandCellEnded();
    }
}
