using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class ContinuousBlowingMachineShiftReportModel: BlowingMachineReportModelBase
    {
        [DisplayName("Data rozpoczęcia")]
        public DateTime StartDate { get; set; }
        [DisplayName("Data zakończenia")]
        public DateTime EndDate { get; set; }
        [DisplayName("Komora")]
        public string Chamber { get; set; }
        [DisplayName("Średnia gęstość z pomiaru")]
        public string AvgDensityOfMeassurement{ get; set; }
        [DisplayName("Gatunek")]
        public string Type{ get; set; }
        [DisplayName("Materiał")]
        public string Material{ get; set; }
    }
}
