using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using TOReportApplication.Logic;
using TOReportApplication.Model;
using TOReportApplication.ViewModels;

namespace TOReportApplication.DataBase
{
    public interface IForm2Repository
    {
        DataTable GetFormDateReportItems(string query) ;
    }

    public class Form2Repository: DBConnection,IForm2Repository
    {

        public Form2Repository(IMyLogger myLogger) : base(myLogger)
        {

        }

        public DataTable GetFormDateReportItems(string query) 
        {

            OpenSession();
            var ds = new DataSet();
            var dt = new DataTable();
            try
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, session);
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception e)
            {
                myLogger.logger.ErrorFormat("Error during execute query: {0}   message: {1}",query,e.Message);
            }

            session.Close();
            return dt;
        }
    }
}
