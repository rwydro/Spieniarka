using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TOReportApplication.Model
{
    public class ContinuousBlowingMachineReportModel: BlowingMachineReportModelBase
    {
        [DisplayName("Data")]
        [XmlElement("Data")]
        public DateTime Date { get; set; }
        [DisplayName("Gestosc z pomiaru")]
        [XmlElement("Gestosc z pomiaru")]
        public double MeasurementDensity { get; set; }
        [DisplayName("Otwarcie pary")]
        [XmlElement("Otwarcie pary")]
        public string SteamOpening { get; set; }
        [DisplayName("Obroty dozownika")]
        [XmlElement("Obroty dozownika")]
        public string DispenserRotation { get; set; }
        [DisplayName("Numer PZ")]
        [XmlElement("Numer PZ")]
        public string PzNumber { get; set; }
    }
}
