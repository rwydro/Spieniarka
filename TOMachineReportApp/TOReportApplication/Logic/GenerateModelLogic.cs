using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace TOReportApplication.Logic
{
    public static class GenerateModelLogic<T> where T : new()
    {
        public static List<T> GenerateReportModel(DataTable obj, Dictionary<string, string> dict)
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

                        switch (Type.GetTypeCode(p.PropertyType))
                        {

                            case TypeCode.Int32:
                                var intVal = Convert<int>(row[c].ToString());
                                p.SetValue(item, intVal, null);
                                break;
                            case TypeCode.DateTime:
                                var dateTimeVal = Convert<DateTime>(row[c].ToString());
                                p.SetValue(item, dateTimeVal, null);
                                break;
                            case TypeCode.Double:
                                var doubleVal = Convert<double>(row[c].ToString());
                                p.SetValue(item, doubleVal, null);
                                break;
                            case TypeCode.String:
                            case TypeCode.Char:
                                var strVal = Convert<string>(row[c].ToString());
                                p.SetValue(item, strVal, null);
                                break;
                            default:
                                if (row[0] != null)
                                {
                                    var nullableDouble = Convert<double>(row[c].ToString());
                                    p.SetValue(item, nullableDouble, null);
                                }
                                break;
                        }
                    }
                }
            }
        }

        public static T Convert<T>(string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T)); 
                if (converter != null)
                {
                    if (converter.GetType() ==typeof(DoubleConverter))
                        input = input.Replace(".", ",");
                    
                    return (T) converter.ConvertFromString(input);
                }

                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }
    }
}
