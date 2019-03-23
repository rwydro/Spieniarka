using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class BlowingMachineReportModelBase : ReportModelBase
    {
        [DisplayName("Operator")]
        public string Operator { get; set; }
        [DisplayName("Komora")]
        public string Chamber { get; set; }
        [DisplayName("Gatunek")]
        public string Type { get; set; }
        [DisplayName("Material")]
        public string Material { get; set; }
        [DisplayName("Gestosc zadana")]
        public double DensitySet { get; set; }
    }


    public class BlowingMachineReportModel: BlowingMachineReportModelBase
    {
        [DisplayName("Maszyna")]
        public string Machine { get; set; }
        [DisplayName("Poczatek")]
        public DateTime DateTimeStart { get; set; }
        [DisplayName("Koniec")]
        public DateTime DateTimeStop { get; set; }
        [DisplayName("Nr zlecenia")]
        public string OrderNumber { get; set; }
        [DisplayName("Nr recepty")]
        public double RecipeNumber { get; set; }
        [DisplayName("Producent")]
        public string Manufacturer { get; set; }
        [DisplayName("Gestosc min")]
        public double DensityMin { get; set; }
        [DisplayName("Gestosc srednia")]
        public double DensityMean { get; set; }
        [DisplayName("Gestosc max")]
        public double DensityMax { get; set; }
        [DisplayName("Ilosc zad Surowca")]
        public double WeightSet { get; set; }
        [DisplayName("losc rzecz Surowca")]
        public double WeightActual { get; set; }
        [DisplayName("Ilosc rzecz partii")]
        public int BatchesActual { get; set; }
        [DisplayName("Nr LOT")]
        public string LotNumber { get; set; }
        [DisplayName("Czas cyklu")]
        public double CycleTime { get; set; }
        [DisplayName("Silos 0")]
        public string Silos0 { get; set; }
        [DisplayName("Czas pary")]
        public double TimeSteam { get; set; }
        [DisplayName("Wyl poziomu")]
        public int LevelSwitch { get; set; }
        [DisplayName("Bajpas")]
        public double Bypass { get; set; }
        [DisplayName("Wartosc nawazki")]
        public double InputWeight { get; set; }
        [DisplayName("Cisn pary")]
        public double SteamPressure { get; set; }
        [DisplayName("Predkosc mieszadla")]
        public double SpeedAgitator { get; set; }
        [DisplayName("Predkosc sluzy lopatk")]
        public double SpeedRotaryValve { get; set; }
        [DisplayName("Klapa suszarki")]
        public double FlapFluidizedBed { get; set; }
        [DisplayName("Klapa transportu")]
        public double FlapTransportUnit { get; set; }
    }
}
