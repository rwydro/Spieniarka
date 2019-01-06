using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using TOReportApplication.Logic;

namespace TOReportApplication.DataBase
{
    public interface IApplicationRepository
    {
        DataTable GetFormDateReportItems(string query);
        void UpdateData(string query);
    }

    public class ApplicationRepository : DBConnection, IApplicationRepository
    {
        public ApplicationRepository(IMyLogger myLogger)
            : base(myLogger)
        {
            this.OpenSession();
        }

        public DataTable GetFormDateReportItems(string query)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            try
            {
                this.myLogger.logger.Debug((object)("query " + query));
                new NpgsqlDataAdapter(query, this.session).Fill(dataSet);
                dataTable = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                this.myLogger.logger.ErrorFormat("Error during execute query: {0}   message: {1}", (object)query, (object)ex.Message);
            }
            return dataTable;
        }

        public void UpdateData(string query)
        {
            try
            {
                this.myLogger.logger.Debug((object)("query " + query));
                var command = new NpgsqlCommand(query, this.session);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.myLogger.logger.ErrorFormat("Error during execute query: {0}   message: {1}", (object)query, (object)ex.Message);
            }
        }
    }
}
