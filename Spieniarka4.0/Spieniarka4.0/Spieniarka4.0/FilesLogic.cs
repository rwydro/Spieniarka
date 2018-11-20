using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spieniarka4._0;

namespace SpieniarkaApplication.Logic
{
    public enum FileType
    {
        Batch,
        Summary
    }

    public static class FilesLogic
    { 

        public static string [] GetDataFromFile(string filePath, FileType fileType)
        {
            try
            {
                var data = File.ReadAllLines(filePath);

                if (fileType == FileType.Summary)
                {
                    return ToDataTableObject(data);
                }
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
            var result = new string[table.Rows.Count];
            int i = 0;

            
            foreach (DataRow dr in table.Rows)
            {
               var qwe = String.Join("\t",dr.ItemArray.Cast<string>());
                result[i++] = qwe; 
            }

            return result;
        }

        private static string[] ToDataTableObject(string[] data)
        {

            var table = new DataTable();
            var summaryProperties = typeof(SummaryFileColumnNames).GetFields();
            foreach (var col in summaryProperties)
                table.Columns.Add(new DataColumn(col.Name));
            foreach (string line in data)
            {
                var cols = line.Split('\t');

                var row = table.NewRow();
                for (var cIndex = 0; cIndex < cols.Length; cIndex++)
                {
                    if (cIndex == 4)
                    {
                        CultureInfo provider = CultureInfo.CreateSpecificCulture("de-DE");
                        row[cIndex] = DateTime.ParseExact(cols[cIndex], "G", provider).ToString("yyyy-MM-dd HH:mm:ss"); 
                        continue;
                    }
                    row[cIndex] = cols[cIndex];
                }

                table.Rows.Add(row);
            }

            return MapFileDataToDbData(table);
        }

        private static string [] MapFileDataToDbData(DataTable tableFile)
        {
            var summaryProperties = typeof(SummaryDbColumnNames).GetFields();
            var nastedColumnListName = new List<string>();
            foreach (DataColumn column in tableFile.Columns)
            {

                if (summaryProperties.All(s => s.Name != column.ColumnName))
                {
                  nastedColumnListName.Add(column.ColumnName);
                };
            }

            foreach (var col in nastedColumnListName)
            {               
                    tableFile.Columns.Remove(col); 
            }

            return ConvertDataTableToStringArray(tableFile); ;
        }
    }
}
