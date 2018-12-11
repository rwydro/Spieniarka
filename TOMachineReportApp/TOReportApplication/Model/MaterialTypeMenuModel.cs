using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class MaterialTypeMenuModel
    {
        public string SelectedMaterialType { get; set; }
        public string NumberOfBlock { get; set; }
        public string Comments { get; set; }
        public double AvgDensityOfPearls { get; set; }
        public int AssignedNumber { get; set; }
    }
}
