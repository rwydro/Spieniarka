using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{

    public class EventBaseArgs<T> : EventArgs where T: ReportModelBase
    {
        public ReportModel<T> ReportModel { get; set; }
    }

    [Serializable]
    public abstract class ReportModelBase
    {
    }

    public class ReportModel<T> where T : ReportModelBase
    {
        public List<T> Model { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}


