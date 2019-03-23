using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class ContinuousBlowingMachineReportModel: BlowingMachineReportModelBase
    {
        [DisplayName("Data")]
        public DateTime Date { get; set; }
        [DisplayName("Gestosc z pomiaru")]
        public string MeasurementDensity { get; set; }
        [DisplayName("Otwarcie pary")]
        public string SteamOpening { get; set; }
        [DisplayName("Obroty dozownika")]
        public string DispenserRotation { get; set; }
        [DisplayName("Silos")]
        public string Silos { get; set; }
    }
}
