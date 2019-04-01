using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Npgsql;
using SpieniarkaCiagla;

namespace SpieniarkaCiagla4._0
{
    public interface ISpieniarkaCiaglaRepository
    {
        void InsertMissingData(string[] lines, DateTime date);
        DateTime GetDateLastRecord();
    }

    public class SpieniarkaCiaglaRepository : ISpieniarkaCiaglaRepository
    {
        private readonly Regex dateRegEx = new Regex(
            @"(3[01]|[12][0-9]|0?[1-9])\-(1[012]|0?[1-9])\-((?:19|20)\d{2}) (0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]:[0-5][0-9]+)");

        private readonly SpieniarkaCiaglaDBConnection dbConnection;
        private readonly ISpieniarkaLogger logger;

        public SpieniarkaCiaglaRepository(ISpieniarkaLogger logger)
        {
            this.logger = logger;
            dbConnection = new SpieniarkaCiaglaDBConnection(this.logger);
        }

        public DateTime GetDateLastRecord()
        {
            dbConnection.OpenSession();

            var query = "SELECT * FROM spieniarka_ciagla_probki order by data_czas desc limit 1";

            var cmd = new NpgsqlCommand(query, dbConnection.session);
            cmd.ExecuteNonQuery();
            var ds = new DataSet();
            var dt = new DataTable();
            var date = new DateTime(2000, 01, 01);
            try
            {
                var da = new NpgsqlDataAdapter(query, dbConnection.session);
                Console.WriteLine("Executed  the query: {0} on connection {1}", query, dbConnection.session);

                da.Fill(ds);

                dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    Console.WriteLine("The table is empty");
                    return date;
                }

                date = (DateTime) dt.Rows[0].ItemArray[1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            dbConnection.session.Close();
            return date;
        }

        public void InsertMissingData(string[] lines, DateTime date)
        {
            dbConnection.OpenSession();
            var provider = CultureInfo.CreateSpecificCulture("de-DE");
            var stringDate = date.ToString("yyyy-MM-dd HH:mm:ss", provider);

            for (var i = lines.Length - 1; i >= 0; i--)
            {
                Console.WriteLine("line: {0}", lines[0]);
                var line = string.Format("\'{0}\'", lines[i].Replace(",", "','"));

                Console.WriteLine("line: {0}", line);
                var matchDateAsString = dateRegEx.Match(line);
                try
                {
                    var matchDate =
                        DateTime.ParseExact(matchDateAsString.Value, "dd-MM-yyyy HH:mm:ss", provider)
                            .ToString("yyyy-MM-dd HH:mm:ss", provider);
                    Console.WriteLine("{0} converts to {1}.", matchDateAsString.Value, matchDate);
                    var correctLine = line.Replace(matchDateAsString.Value, matchDate);

                    if (correctLine.Contains(stringDate))
                    {

                        Console.WriteLine("No more the new lines");
                        return;
                    }

                    var query =
                        string.Format(
                            "INSERT INTO public.spieniarka_ciagla_probki(data_czas, gestosc_z_pomiaru, gestosc_zadana, otwarcie_pary, obroty_dozownika, material, gatunek, silos, komora, operator)  VALUES({0})",
                            correctLine);

                    var cmd = new NpgsqlCommand(query, dbConnection.session);
                    Console.WriteLine("Executed the query {0}", query);

                    cmd.ExecuteNonQuery();
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e);

                    Console.WriteLine("{0} is not in the correct format.", matchDateAsString.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadKey();
                }

                Console.WriteLine("Above query was executed successful");
            }
            Console.WriteLine("End of for loop");
        }
    }
}