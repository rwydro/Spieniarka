using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class BlowingMachineReportDto
    {
        public List<BlowingMachineReportModel> Model { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}
