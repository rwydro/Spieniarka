using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class FormDetailedReportDBModel: FormDateReportDBModel
    {
        public DateTime OrganicDate { get; set; }
        public double Weight { get; set; }
        public int CycleTimeInSecond { get; set; }
        public double Blow { get; set; }
        public string Type { get; set; }
        public string Comments { get; set; }
        public double? AvgDensityOfPearls { get; set; }
    }
}
