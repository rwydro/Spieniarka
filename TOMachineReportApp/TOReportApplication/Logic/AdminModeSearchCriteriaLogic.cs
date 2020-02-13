using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.DataBase;
using TOReportApplication.Logic.interfaces;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    public class AdminModeSearchCriteriaLogic: IAdminModeSearchCriteriaLogic
    {
        private readonly IApplicationRepository dbConnection;

        public AdminModeSearchCriteriaLogic(IApplicationRepository dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        private string GenerateBlowingMachineQuery(DateTime selectedFromDate, DateTime selectedToDate)
        {
            var qweqwe = string.Format(
                "SELECT * FROM public.spieniarka_probki_summary where data_koniec > '{0}' and data_koniec < '{1}' order by data_koniec",
                selectedFromDate, selectedToDate);
            return qweqwe;
        }

        public List<BlowingMachineReportModel> GenerateBlowingMachineReport(DateTime selectedFromDate, DateTime selectedToDate)
        {
            var data = dbConnection.GetDataFromDB(GenerateBlowingMachineQuery(selectedFromDate, selectedToDate));

            var model = GenerateModelLogic<BlowingMachineReportModel>.GenerateReportModel(data,
                ModelDictionaries.BlowingMachineDbColumnNameToModelPropertyNameDictionary);

            return model;
        }

        private string GenerateContinuousBlowingMachineQuery(DateTime selectedFromDate, DateTime selectedToDate)
        {
            var qweqw = string.Format(
                "SELECT * FROM public.spieniarka_ciagla_probki where data_czas > '{0}' and data_czas < '{1}' order by data_czas",
                selectedFromDate, selectedToDate);
            return qweqw;
        }

        public List<ContinuousBlowingMachineReportModel> GenerateContinuousBlowingMachineReport(DateTime selectedFromDate, DateTime selectedToDate)
        {
            var data = dbConnection.GetDataFromDB(GenerateContinuousBlowingMachineQuery(selectedFromDate, selectedToDate));

            var model = GenerateModelLogic<ContinuousBlowingMachineReportModel>.GenerateReportModel(data,
                ModelDictionaries.ContinuousBlowingMachineDbColumnNameToModelPropertyNameDictionary);

            return model;
        }

        public List<FormDateReportDBModel> GenerateFormReportModel(DateTime selectedFromDate, DateTime selectedToDate)
        {
            var query = string.Format(
                "SELECT * FROM public.forma_blok2 where data_czas > '{0}' and data_czas < '{1}'",
                selectedFromDate, selectedToDate);
            var data = dbConnection.GetDataFromDB(query);
            var dateReportDbModelList = GenerateModelLogic<FormDateReportDBModel>.GenerateReportModel(data, ModelDictionaries.FormDetailedReportDbModelPropertyNameDictionary);
            return dateReportDbModelList;
        }
    }
}
