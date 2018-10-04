using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class FormReportsDBModel
    {
        public List<FormDetailedReportDBModel> DetailedReportDb { get; set; }
        public List<FormDateReportDBModel> DateReportDb { get; set; }
    }
}
