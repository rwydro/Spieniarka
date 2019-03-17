using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{

    [Serializable]
    public class FormDatailedReportModel
    {
        public int Id { get; set; }
        [DisplayName("Silos")]
        public int Silos { get; set; }
        [DisplayName("Komora")]
        public int Chamber { get; set; }
        [DisplayName("Data produkcji")]
        public DateTime ProductionDate { get; set; }
        [DisplayName("Operator")]
        public string Operator { get; set; }
        [DisplayName("Data organiki")]
        public DateTime OrganicDate { get; set; }
        [DisplayName("Waga [kg]")]
        public double Weight { get; set; }
        [DisplayName("Czas cyklu [s]")]
        public int CycleTimeInSecond { get; set; }
        [DisplayName("Regranulat")]
        public double Blow { get; set; }
        [DisplayName("Gatunek")]
        public string Type { get; set; }
        [DisplayName("Komentarz")]
        public string Comments { get; set; }
        [DisplayName("Sr.g. perełek")]
        public string AvgDensityOfPearls { get; set; }
        [DisplayName("Numer nadany")]
        public int AssignedNumber { get; set; }
        [DisplayName("Numer PZ")]
        public int PzNumber { get; set; }
    }

    public class FormDateReportDBModel : FormDatailedReportModel
    {
        [DisplayName("Numer bloku")]
        public int BlockNumber { get; set; }
        [DisplayName("Czas cyklu [min]")]
        public int CycleMin { get; set; }
        [DisplayName("Czas cyklu [sec]")]
        public int CycleSec { get; set; }
        [DisplayName("Ciśnienie eps 1")]
        public double PreassureEPS1 { get; set; }
        [DisplayName("Ciśnienie eps 2")]
        public double PreassureEPS2 { get; set; }
        [DisplayName("Ciśnienie eps 3")]
        public double PreassureEPS3 { get; set; }
        [DisplayName("Wstępna próżnia")]
        public double Vacuum { get; set; }
        [DisplayName("Czas próźni")]
        public double VacuumTime { get; set; }
        [DisplayName("Czas płókania 1")]
        public double RinsingTime1 { get; set; }
        [DisplayName("Czas płókania 2")]
        public double RinsingTime2 { get; set; }
        [DisplayName("Czas płókania 3")]
        public double RinsingTime3 { get; set; }
        [DisplayName("Czas płókania 4")]
        public double RinsingTime4 { get; set; }
        [DisplayName("Otwarcie zaworu pary 1")]
        public double Flap1 { get; set; }
        [DisplayName("Otwarcie zaworu pary 2")]
        public double Flap2 { get; set; }
        [DisplayName("Otwarcie zaworu pary 3")]
        public double Flap3 { get; set; }
        [DisplayName("Otwarcie zaworu pary 4")]
        public double Flap4 { get; set; }
        public double AutoKlwProcUs { get; set; }
        [DisplayName("Ustawione autoklaw [%]")]
        public double AutoKlwCisUs { get; set; }
        [DisplayName("Otwarcie zaworu pary 1")]
        public double AutoKlwProcLs { get; set; }
        [DisplayName("Ciśnienie ustawione autoklawu")]
        public double AutoKlwCisLs { get; set; }
        public double AutoKlwProcFs { get; set; }
        public double AutoKlwCisFs { get; set; }
        public double AutoKlwProcMs { get; set; }
        public double AutoKlwCisMs { get; set; }
        [DisplayName("Czas autoklawu")]
        public double AutoKlwCzas { get; set; }
        [DisplayName("Czas stabilizacji")]
        public double Stabilization { get; set; }
        [DisplayName("Czas chłodzenia")]
        public double CoolingTime { get; set; }
        [DisplayName("Gęstość bloku")]
        public double DensityBlockNumber { get; set; }
        [DisplayName("Data modyfikacji")]
        public DateTime ModificationDate { get; set; }
        [DisplayName("Użytkownik")]
        public string User { get; set; }
        [DisplayName("Flaga zakwalifikowania do odpadu")]
        public int Waste { get; set; }
        [DisplayName("Flagausunięcia bloku z ewidencji")]
        public int Removed { get; set; }
        [DisplayName("Rok produkcji")]
        public int PzYear { get; set; }
        [DisplayName("Flaga pocięcia")]
        public int Pociety { get; set; }
    }
}
