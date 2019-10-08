using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Model;

namespace TOReportApplication.Logic.interfaces
{
    interface IAdminModeSearchCriteriaLogic
    {
        List<BlowingMachineReportModel> GenerateBlowingMachineReport(DateTime selectedFromDate, DateTime selectedToDate);
        List<ContinuousBlowingMachineReportModel> GenerateContinuousBlowingMachineReport(DateTime selectedFromDate, DateTime selectedToDate);
        List<FormDateReportDBModel> GenerateFormReportModel(DateTime selectedFromDate, DateTime selectedToDate);
    }
}
