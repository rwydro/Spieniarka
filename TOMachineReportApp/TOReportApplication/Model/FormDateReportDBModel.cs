using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class FormDateReportDBModel
    {
        public DateTime ProductionDate{ get; set; }
        public int Silos { get; set; }
        public int Chamber { get; set; }
        public string Operator { get; set; }
    }
}
