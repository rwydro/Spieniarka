using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.DataBase;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    public interface IBlockHistoryGetDataLogic
    {
        Task<List<BlowingMachineReportModel>> GetBlowingMachineData(DateTime endDate, int silos, int pz);
        void GetContinuousBlowingMachineData(string query);
    }

    public class BlockHistoryGetDataLogic: IBlockHistoryGetDataLogic
    {
        private readonly IApplicationRepository dbConnection;

        public BlockHistoryGetDataLogic(IApplicationRepository dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<List<BlowingMachineReportModel>> GetBlowingMachineData(DateTime endDate, int chamber, int pz)
        {
            var query = string.Format(
                "SELECT * FROM public.spieniarka_probki_summary where silos_0 = '{0}' and nr_lot = '{1}' and data_koniec < '{2}' order by data_koniec",
                chamber,
                pz,
                new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second));

            var data = dbConnection.GetDataFromDB(query);           
            return  GenerateModelLogic<BlowingMachineReportModel>.GenerateReportModel(data, ModelDictionaries.BlowingMachineDbColumnNameToModelPropertyNameDictionary);
        }

        public void GetContinuousBlowingMachineData(string query)
        {

        }
    }
}
