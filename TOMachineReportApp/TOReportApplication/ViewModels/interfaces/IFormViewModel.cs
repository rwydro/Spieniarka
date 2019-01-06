using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.ViewModels.interfaces
{
    public interface IFormViewModel
    {
        void Dispose();
        void OnCommandCellEnded();
    }
}
