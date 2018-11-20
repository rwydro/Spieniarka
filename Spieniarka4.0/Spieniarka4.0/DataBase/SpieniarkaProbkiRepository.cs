using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SpieniarkaApplication.DataBase
{
    public interface ISpieniarkaProbkiRepository
    {
       void InsertMissingData(string[] lines, DateTime date);
        DateTime GetDateLastRecord();
    }

    public class SpieniarkaProbkiRepository: ISpieniarkaProbkiRepository
    {
        private SpieniarkaDBConnection dbConnection;
        private ISpieniarkaLogger logger;
        public SpieniarkaProbkiRepository(ISpieniarkaLogger logger)
        {
            this.logger = logger;
            dbConnection = new SpieniarkaDBConnection(this.logger);
        }

        public DateTime GetDateLastRecord()
        {
            dbConnection.OpenSession();
            var query = String.Format("SELECT * FROM spieniarka_probki order by data_czas desc limit 1");
            var cmd = new NpgsqlCommand(query, dbConnection.session);
            cmd.ExecuteNonQuery();
            var ds = new DataSet();
            var dt = new DataTable();
            var date = new DateTime();
            try
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, dbConnection.session);
                da.Fill(ds);
                dt = ds.Tables[0];
                date = (DateTime)dt.Rows[0].ItemArray[2];
            }
            catch (Exception e)
            {
                
            }

            dbConnection.session.Close();
            return date;
        }

        public void InsertMissingData(string[] lines, DateTime date)
        {
            dbConnection.OpenSession();

            var stringDate = date.ToString("dd.MM.yyyy HH:mm:ss");

            for (int i = lines.Length-1; i >= 0; i--)
            {

                var line = $"\'{lines[i].Replace("\t", "','")}\'";
                if (line.Contains(stringDate))
                {
                    return;
                }

                var query = String.Format("INSERT INTO public.spieniarka_probki( symbol, wersja, data_czas, nr_zlecenia, nr_wsadu, nawazka_zadana, nawazka_rzecz, licznik_regulacji, gestosc_zadana, gestosc_rzecz, czas_pracowania, cisn_pary, temp_pary, temp_kotla, czas_cyklu, j_wagi, j_gest, j_cisn, j_temp) VALUES({0})",
                    line);
                var cmd = new NpgsqlCommand(query, dbConnection.session);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
