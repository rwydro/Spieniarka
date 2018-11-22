using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    

    public static class GenerateModelLogic
    {
        public static Dictionary<string, string> BlowingMachineDbColumnNameToModelPropertyNameDictionary =
            new Dictionary<string, string>()
            {
                {"maszyna","Machine"},
                {"data_poczatek","DateTimeStart"},
                {"data_koniec","DateTimeStop"},
                {"nr_zlecenia","OrderNumber"},
                {"nr_recepty","RecipeNumber"},
                {"producent","Manufacturer"},
                {"typ","Type"},
                {"gestosc_zadana","DensitySet"},
                {"gestosc_min","DensityMin"},
                {"gestosc_srednia","DensityMean"},
                {"gestosc_max","DensityMax"},
                {"ilosc_zad_surowca","WeightSet"},
                {"ilosc_rzecz_surowca","WeightActual"},
                {"ilosc_rzecz_partii","BatchesActual"},
                {"operator","Operator"},
                {"komora","Chamber"},
                {"nr_lot","LotNumber"},
                {"material","Material"},
                {"czas_cyklu","CycleTime"},
                {"silos_0","Silos0"},
                {"czas_pary","TimeSteam"},
                {"wyl_poziomu","LevelSwitch"},
                {"bajpas","Bypass"},
                {"wartosc_nawazki","InputWeight"},
                {"cisn_pary","SteamPressure"},
                {"predkosc_mieszadla","SpeedAgitator"},
                {"predkosc_sluzy_lopatk","SpeedRotaryValve"},
                {"klapa_suszarki","FlapFluidizedBed"},
                {"klapa_transportu","FlapTransportUnit"}
            };

        public static List<BlowingMachineReportModel> GenerateBlowingMachineReportModel(DataTable obj)
        {
            var  list = new List<BlowingMachineReportModel>();


            foreach (DataRow row in obj.Rows)
            {
               var item = CreateItemFromRow<BlowingMachineReportModel>(row);
               list.Add(item);
            }

            return list;
        }

        private static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new T();
            
            SetItemFromRow(item, row);
            return item;
        }

        private static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                string colName = item is BlowingMachineReportModel ? BlowingMachineDbColumnNameToModelPropertyNameDictionary.First(s => s.Key == c.ColumnName).Value : c.ColumnName;
                PropertyInfo p = item.GetType().GetProperty(colName);

                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
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
                    Operator = (string)item.ItemArray[47],

                });
            }
            return list;
        }

        public static List<FormDetailedReportDBModel> GeneratFormDetailedReportModel(DataTable obj)
        {

            var list = new List<FormDetailedReportDBModel>();
            foreach (DataRow item in obj.Rows)
            {
                list.Add(new FormDetailedReportDBModel()
                {
                    OrganicDate = (DateTime)item.ItemArray[44],
                    ProductionDate = (DateTime)item.ItemArray[1],
                    Weight = Double.Parse(item.ItemArray[6].ToString()),
                    CycleTimeInSecond = Int32.Parse(item.ItemArray[32].ToString()),
                    Chamber = Int32.Parse(item.ItemArray[35].ToString()),
                    Silos = Int32.Parse(item.ItemArray[34].ToString()),
                    Operator = (string)item.ItemArray[47],
                    Blow = Double.Parse(item.ItemArray[10].ToString()),
                    Type = (string)item.ItemArray[38],
                    Comments = (string)item.ItemArray[39],
                    AvgDensityOfPearls = Double.Parse(item.ItemArray[46].ToString())
                });
            }
            return list;
        }
    }
}
