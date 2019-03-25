using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SpieniarkaCiagla4._0
{
    public static class FilesLogic
    {

        public static string[] GetDataFromFile(string filePath)
        {
            try
            {
                var data = File.ReadAllLines(filePath);

                return ToDataTableObject(data);

                Console.WriteLine($"Read file {filePath} was ended");
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                throw;
            }

        }

        private static string[] ConvertDataTableToStringArray(DataTable table)
        {
            var result
                = new string[table.Rows.Count];
            int i = 0;


            foreach (DataRow dr in table.Rows)
            {
                var qwe = String.Join(",", dr.ItemArray.Cast<string>());
                result[i++] = qwe;
            }

            return result;
        }

        private static string[] ToDataTableObject(string[] data)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("de-DE");
            var table = new DataTable();
            var summaryProperties = typeof(SpieniarkaCiaglaColumnName).GetFields();
            foreach (var col in summaryProperties)
                table.Columns.Add(new DataColumn(col.Name));



            foreach (string line in data)
            {
                if (data[0] == line) continue; // na poczatku w pliku sa jakies dziwne informacje
                var rowIndex = 0;
                var cols = line.Split(';');
                var row = table.NewRow();
                var date = DateTime.Parse(cols[0]);
                var time = DateTime.Parse(cols[1]);
                date = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                for (var cIndex = 0; cIndex < cols.Length; cIndex++)
                {

                    if (cIndex == 1 || cIndex == (cols.Length - 1))
                        continue; //2 warunek po to ze na koncu pliku jest tez srednik i rozpoznaje ze tam jeszcze jedna kolumna jest
                    if (cIndex == 0)
                    {
                        row[cIndex] = date.ToString("dd.MM.yyyy HH:mm:ss");
                        rowIndex++;
                        continue;
                    }

                    row[rowIndex] = cols[cIndex];
                    rowIndex++;
                }

                table.Rows.Add(row);
            }


            return ConvertDataTableToStringArray(table);
        }
    }
}
