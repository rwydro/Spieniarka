using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TOReportApplication.Model
{

    public class FormDateReportModel
    {
        public string Shift { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int NumberOfBlocks { get; set; }
        public int Silos { get; set; }
        public int Chamber { get; set; }
        public string Operator { get; set; }
        public List<FormDateReportDBModel> DetailedReportForChamber { get; set; }
    }
}
