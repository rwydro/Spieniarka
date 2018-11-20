using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using SpieniarkaApplication.Logic;

namespace SpieniarkaApplication.DataBase
{
    public interface ISpieniarkaProbkiRepository
    {
       void InsertMissingData(string[] lines, DateTime date, FileType file);
        DateTime GetDateLastRecord(FileType file);
    }

    public class SpieniarkaProbkiRepository: ISpieniarkaProbkiRepository
    {
        private Regex dateRegEx = new Regex(
            @"(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2}) (0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]:[0-5][0-9]+)");
        private SpieniarkaDBConnection dbConnection;
        private ISpieniarkaLogger logger;
        public SpieniarkaProbkiRepository(ISpieniarkaLogger logger)
        {
            this.logger = logger;
            dbConnection = new SpieniarkaDBConnection(this.logger);
        }

        public DateTime GetDateLastRecord(FileType file)
        {
            dbConnection.OpenSession();

            var query = file == FileType.Batch ? String.Format("SELECT * FROM spieniarka_probki order by data_czas desc limit 1") :
                                                 String.Format("SELECT * FROM spieniarka_probki_summary order by data_koniec desc limit 1");
            
            var cmd = new NpgsqlCommand(query, dbConnection.session);
            cmd.ExecuteNonQuery();
            var ds = new DataSet();
            var dt = new DataTable();
            var date = new DateTime(2000,01,01);
            try
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, dbConnection.session);
                Console.WriteLine($"Executed  the query: {query} on connection {dbConnection.session}");
                
                da.Fill(ds);

                dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    Console.WriteLine("The table is empty");
                    return date;
                }


                date = file == FileType.Batch ? (DateTime)dt.Rows[0].ItemArray[2] : (DateTime)dt.Rows[0].ItemArray[1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            dbConnection.session.Close();
            return date;
        }

        public void InsertMissingData(string[] lines, DateTime date, FileType file)
        {   
            dbConnection.OpenSession();
            Console.WriteLine($"lines count: {lines.Length}, date: {date}");
            var stringDate = date.ToString("yyyy-MM-dd HH:mm:ss");

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                var line = $"\'{lines[i].Replace("\t", "','")}\'";
                var matchDateAsString = dateRegEx.Match(line);
                try
                {
                    Console.WriteLine($"matchDateAsString: {matchDateAsString}");
                    CultureInfo provider = CultureInfo.CreateSpecificCulture("de-DE");


                    var matchDate = DateTime.ParseExact(matchDateAsString.Value,"G", provider).ToString("yyyy-MM-dd HH:mm:ss");
                    Console.WriteLine("{0} converts to {1}.", matchDateAsString.Value, matchDate);
                    var correctLine = line.Replace(matchDateAsString.Value, matchDate);
                  
                   
                    if (matchDate.Contains(stringDate))
                    {
                        Console.WriteLine($"No more the new lines");
                        return;
                    }

                    var query = file == FileType.Batch ? "INSERT INTO public.spieniarka_probki(symbol, wersja, data_czas, nr_zlecenia, nr_wsadu, nawazka_zadana, nawazka_rzecz, licznik_regulacji, gestosc_zadana, gestosc_rzecz," +
                                                         $" czas_pracowania, cisn_pary, temp_pary, temp_kotla, czas_cyklu, j_wagi, j_gest, j_cisn, j_temp) VALUES({correctLine})": 
                        "INSERT INTO public.spieniarka_probki_summary(maszyna, data_poczatek, data_koniec, nr_zlecenia, nr_recepty, producent, typ, gestosc_zadana, gestosc_min," +
                          " gestosc_srednia, gestosc_max, ilosc_zad_surowca, ilosc_rzecz_surowca, ilosc_rzecz_partii, operator, komora, nr_lot, material, czas_cyklu, silos_0, czas_pary, wyl_poziomu," +
                          $" bajpas, wartosc_nawazki, cisn_pary, predkosc_mieszadla, predkosc_sluzy_lopatk, klapa_suszarki, klapa_transportu) VALUES({correctLine})";

                    var cmd = new NpgsqlCommand(query, dbConnection.session);
                    Console.WriteLine($"Executed the query {query}");

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
            Console.WriteLine($"End of for loop");
        }
        //public void InsertMissingData(string[] lines, DateTime date)
        //{
        //    dbConnection.OpenSession();
        //    Console.WriteLine($"lines count: {lines.Length}, date: {date}");
        //    var stringDate = date.ToString("dd-MM-yyyy HH:mm:ss");

        //    for (int i = lines.Length-1; i >= 0; i--)
        //    {
        //        try
        //        {
        //            var line = $"\'{lines[i].Replace("\t", "','")}\'";
        //            var matchDateAsString = dateRegEx.Match(line);
        //            var matchDate = Convert.ToDateTime(dateRegEx.Match(line).Value).ToString("yyyy-MM-dd HH:mm:ss");
        //            var correctLine = line.Replace(matchDateAsString.Value, matchDate);
        //            if (correctLine.Contains(stringDate))
        //            {
        //                Console.WriteLine($"line:{correctLine}, dateAsString: {stringDate}");
        //                return;
        //            }

        //            var query = String.Format(
        //                "INSERT INTO public.spieniarka_probki(symbol, wersja, data_czas, nr_zlecenia, nr_wsadu, nawazka_zadana, nawazka_rzecz, licznik_regulacji, gestosc_zadana, gestosc_rzecz, czas_pracowania, cisn_pary, temp_pary, temp_kotla, czas_cyklu, j_wagi, j_gest, j_cisn, j_temp) VALUES({0})",
        //                correctLine);
        //            var cmd = new NpgsqlCommand(query, dbConnection.session);
        //            Console.WriteLine($"Executed the query {query}");

        //            cmd.ExecuteNonQuery();
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            Console.ReadKey();
        //        }

        //        Console.WriteLine("Above query was executed successful");
        //    }
        //    Console.WriteLine($"End of for loop");
        //}
        
    }
}
