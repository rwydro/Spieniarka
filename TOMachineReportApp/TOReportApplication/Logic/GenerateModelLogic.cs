using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    public static class GenerateModelLogic
    {
        public static List<FormDateReportDBModel> GeneratFormDateReportModel(DataTable obj)
        {
            var list = new List<FormDateReportDBModel>();
            foreach (DataRow item in obj.Rows)
            {

                list.Add(new FormDateReportDBModel()
                {
                    ProductionDate = (DateTime) item.ItemArray[1],
                    Chamber = Int32.Parse(item.ItemArray[35].ToString()),
                    Silos = Int32.Parse(item.ItemArray[34].ToString()),
                    Operator = (string) item.ItemArray[47],

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
