using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using TOReportApplication.Model;
using static System.Double;

namespace TOReportApplication.Logic
{  
    public static class GenerateModelLogic<T> where T : new()
    {
        public static List<T> GenerateBlowingMachineReportModel(DataTable obj, Dictionary<string, string> dict)
        {
            var newTable = obj;
            var list = new List<T>();

            foreach (DataRow row in newTable.Rows)
            {
               var item = CreateItemFromRow(row, dict);
               list.Add(item);
            }

            return list;
        }

        private static T CreateItemFromRow(DataRow row, Dictionary<string, string> dict)
        {
            T item = new T();
            
            SetItemFromRow(item, row, dict);
            return item;
        }

        private static void SetItemFromRow(T item, DataRow row, Dictionary<string, string> dict) 
        {
          

            foreach (DataColumn c in row.Table.Columns)
            {
                if (dict.ContainsKey(c.ColumnName))
                {
                    string colName = dict.First(s => s.Key == c.ColumnName).Value;
                    PropertyInfo p = item.GetType().GetProperty(colName);

                    if (p != null && row[c] != DBNull.Value)
                    {

                        p.SetValue(item, row[c], null);
                    }
                }
            }
        }
        public static T Convert<T>(string input) where T :  class 
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static List<FormDateReportDBModel> GeneratFormDateReportModel(DataTable obj)
        {
            var list = new List<FormDateReportDBModel>();
            foreach (DataRow item in obj.Rows)
            {

                list.Add(new FormDateReportDBModel()
                {
                    ProductionDate = (DateTime)item.ItemArray[1],
                    Chamber = Int32.Parse(item.ItemArray[35].ToString()),
                    Silos = Int32.Parse(item.ItemArray[34].ToString()),
                    Operator = !String.IsNullOrEmpty(item.ItemArray[47].ToString()) ? (string)item.ItemArray[47] : "",
                });
            }
            return list;
        }

        public static List<FormDetailedReportDBModel> GeneratFormDetailedReportModel(DataTable obj)
        {

            var list = new List<FormDetailedReportDBModel>();

            DateTime organicDate;
            double weight;
            int cycleTimeInSecond;
            double blow;
            int silos;
            int chamber;
            double avgDensityOfPearls;
            DateTime productionDate;
            int assignedNumber;
            int id;
          
            foreach (DataRow item in obj.Rows)
            {
                DateTime.TryParse(item.ItemArray[44].ToString(), out organicDate);
                DateTime.TryParse(item.ItemArray[1].ToString(), out productionDate);
                TryParse(item.ItemArray[5].ToString(),out weight);
                int.TryParse(item.ItemArray[32].ToString(), out cycleTimeInSecond);
                int.Parse(item.ItemArray[35].ToString());
                TryParse(item.ItemArray[9].ToString(), out blow);
                TryParse(item.ItemArray[46].ToString(),out avgDensityOfPearls);
                int.TryParse(item.ItemArray[35].ToString(), out chamber);
                int.TryParse(item.ItemArray[34].ToString(), out silos);
                int.TryParse(item.ItemArray[43].ToString(), out assignedNumber);
                int.TryParse(item.ItemArray[0].ToString(), out id);

                try
                {
                    list.Add(new FormDetailedReportDBModel()
                    {
                        Id = id,
                        OrganicDate =  organicDate,
                        ProductionDate = (DateTime)item.ItemArray[1],
                        Weight = weight,
                        CycleTimeInSecond = cycleTimeInSecond,
                        Chamber = chamber,
                        Silos = silos,
                        Operator = item.ItemArray[47].ToString(),
                        Blow = blow,
                        Type = !String.IsNullOrEmpty(item.ItemArray[38].ToString()) ? item.ItemArray[38].ToString() : "",
                        Comments = !String.IsNullOrEmpty(item.ItemArray[39].ToString()) ? item.ItemArray[39].ToString() : "",
                        AvgDensityOfPearls = avgDensityOfPearls,
                        AssignedNumber = assignedNumber
                    });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }               
            }
            return list;
        }
    }
}
