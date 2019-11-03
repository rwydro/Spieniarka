using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace TOReportApplication.Model
{
    public class BlowingMachineReportModelBase : ReportModelBase
    {
        [DisplayName("Komora")]
        [XmlElement("Komora")]
        public string Chamber { get; set; }
        [DisplayName("Operator")]
        [XmlElement("Operator")]
        public string Operator { get; set; }     
        [DisplayName("Gatunek")]
        [XmlElement("Gatunek")]
        public string Type { get; set; }
        [DisplayName("Material")]
        [XmlElement("Material")]
        public string Material { get; set; }
        [DisplayName("Gestosc zadana")]
        [XmlElement("Gestosc zadana")]
        public double DensitySet { get; set; }
    }


    public class BlowingMachineReportModel: BlowingMachineReportModelBase
    {
        [DisplayName("Maszyna")]
        [XmlElement("Maszyna")]
        public string Machine { get; set; }
        [DisplayName("Poczatek")]
        [XmlElement("Poczatek")]
        public DateTime DateTimeStart { get; set; }
        [DisplayName("Koniec")]
        [XmlElement("Koniec")]
        public DateTime DateTimeStop { get; set; }
        [DisplayName("Nr zlecenia")]
        [XmlElement("Nr zlecenia")]
        public string OrderNumber { get; set; }
        [DisplayName("Nr recepty")]
        [XmlElement("Nr recepty")]
        public double RecipeNumber { get; set; }
        [DisplayName("Producent")]
        [XmlElement("Producent")]
        public string Manufacturer { get; set; }
        [DisplayName("Gestosc min")]
        [XmlElement("Gestosc min")]
        public double DensityMin { get; set; }
        [DisplayName("Gestosc srednia")]
        [XmlElement("Gestosc srednia")]
        public double DensityMean { get; set; }
        [DisplayName("Gestosc max")]
        [XmlElement("Gestosc max")]
        public double DensityMax { get; set; }
        [DisplayName("Ilosc zad Surowca")]
        [XmlElement("Ilosc zad Surowca")]
        public double WeightSet { get; set; }
        [DisplayName("losc rzecz Surowca")]
        [XmlElement("losc rzecz Surowca")]
        public double WeightActual { get; set; }
        [DisplayName("Ilosc rzecz partii")]
        [XmlElement("Ilosc rzecz partii")]
        public int BatchesActual { get; set; }
        [DisplayName("Nr LOT")]
        [XmlElement("Nr LOT")]
        public string LotNumber { get; set; }
        [DisplayName("Czas cyklu")]
        [XmlElement("Czas cyklu")]
        public double CycleTime { get; set; }
        [DisplayName("Silos 0")]
        [XmlElement("Silos 0")]
        public string Silos0 { get; set; }
        [DisplayName("Czas pary")]
        [XmlElement("Czas pary")]
        public double TimeSteam { get; set; }
        [DisplayName("Wyl poziomu")]
        [XmlElement("Wyl poziomu")]
        public int LevelSwitch { get; set; }
        [DisplayName("Bajpas")]
        [XmlElement("Bajpas")]
        public double Bypass { get; set; }
        [DisplayName("Wartosc nawazki")]
        [XmlElement("Wartosc nawazki")]
        public double InputWeight { get; set; }
        [DisplayName("Cisn pary")]
        [XmlElement("Cisn pary")]
        public double SteamPressure { get; set; }
        [DisplayName("Predkosc mieszadla")]
        [XmlElement("Predkosc mieszadla")]
        public double SpeedAgitator { get; set; }
        [DisplayName("Predkosc sluzy lopatk")]
        [XmlElement("Predkosc sluzy lopatk")]
        public double SpeedRotaryValve { get; set; }
        [DisplayName("Klapa suszarki")]
        [XmlElement("Klapa suszarki")]
        public double FlapFluidizedBed { get; set; }
        [DisplayName("Klapa transportu")]
        [XmlElement("Klapa transportu")]
        public double FlapTransportUnit { get; set; }
    }
}
