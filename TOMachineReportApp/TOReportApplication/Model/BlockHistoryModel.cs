using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class BlockHistoryModel : ReportModelBase
    {
        public DateTime Date { get; set; }
        public int BlockNumber { get; set; }
        public int PzNumber { get; set; }
    }
}
