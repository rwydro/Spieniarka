using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TOReportApplication.Model
{

    [Serializable]
    public class FormDatailedReportModel : ReportModelBase
    {
        public int Id { get; set; }
        [DisplayName("Silos")]
        [XmlElement("Silos")]
        public int Silos { get; set; }
        [DisplayName("Komora")]
        [XmlElement("Komora")]
        public int Chamber { get; set; }
        [DisplayName("Data produkcji")]
        [XmlElement("Data produkcji")]
        public DateTime ProductionDate { get; set; }
        [DisplayName("Operator")]
        [XmlElement("Operator")]
        public string Operator { get; set; }
        [DisplayName("Data organiki")]
        [XmlElement("Data organiki")]
        public DateTime OrganicDate { get; set; }
        [DisplayName("Waga [kg]")]
        [XmlElement("Waga [kg]")]
        public double Weight { get; set; }
        [DisplayName("Czas cyklu [s]")]
        [XmlElement("Czas cyklu [s]")]
        public int CycleTimeInSecond { get; set; }
        [DisplayName("Regranulat")]
        [XmlElement("Regranulat")]
        public double Blow { get; set; }
        [DisplayName("Gatunek")]
        [XmlElement("Gatunek")]
        public string Type { get; set; }
        [DisplayName("Komentarz")]
        [XmlElement("Komentarz")]
        public string Comments { get; set; }
        [DisplayName("Sr.g. perełek")]
        [XmlElement("Sr.g. perełek")]
        public string AvgDensityOfPearls { get; set; }
        [DisplayName("Numer nadany")]
        [XmlElement("Numer nadany")]
        public int AssignedNumber { get; set; }
        [DisplayName("Numer PZ")]
        [XmlElement("Numer PZ")]
        public int PzNumber { get; set; }
    }

    public class FormDateReportDBModel : FormDatailedReportModel
    {
        [DisplayName("Numer bloku")]
        [XmlElement("Numer bloku")]
        public int BlockNumber { get; set; }
        [DisplayName("Czas cyklu [min]")]
        [XmlElement("Czas cyklu [min]")]
        public int CycleMin { get; set; }
        [DisplayName("Czas cyklu [sec]")]
        [XmlElement("Czas cyklu [sec]")]
        public int CycleSec { get; set; }
        [DisplayName("Ciśnienie eps 1")]
        [XmlElement("Ciśnienie eps 1")]
        public double PreassureEPS1 { get; set; }
        [DisplayName("Ciśnienie eps 2")]
        [XmlElement("Ciśnienie eps 2")]
        public double PreassureEPS2 { get; set; }
        [DisplayName("Ciśnienie eps 3")]
        [XmlElement("Ciśnienie eps 3")]
        public double PreassureEPS3 { get; set; }
        [DisplayName("Wstępna próżnia")]
        [XmlElement("Wstępna próżnia")]
        public double Vacuum { get; set; }
        [DisplayName("Czas próźni")]
        [XmlElement("Czas próźni")]
        public double VacuumTime { get; set; }
        [DisplayName("Czas płókania 1")]
        [XmlElement("Czas płókania 1")]
        public double RinsingTime1 { get; set; }
        [DisplayName("Czas płókania 2")]
        [XmlElement("Czas płókania 2")]
        public double RinsingTime2 { get; set; }
        [DisplayName("Czas płókania 3")]
        [XmlElement("Czas płókania 3")]
        public double RinsingTime3 { get; set; }
        [DisplayName("Czas płókania 4")]
        [XmlElement("Czas płókania 4")]
        public double RinsingTime4 { get; set; }
        [DisplayName("Otwarcie zaworu pary 1")]
        [XmlElement("Otwarcie zaworu pary 1")]
        public double Flap1 { get; set; }
        [DisplayName("Otwarcie zaworu pary 2")]
        [XmlElement("Otwarcie zaworu pary 2")]
        public double Flap2 { get; set; }
        [DisplayName("Otwarcie zaworu pary 3")]
        [XmlElement("Otwarcie zaworu pary 3")]
        public double Flap3 { get; set; }
        [DisplayName("Otwarcie zaworu pary 4")]
        [XmlElement("Otwarcie zaworu pary 4")]
        public double Flap4 { get; set; }
        [DisplayName("AutoKlwProcUs")]
        [XmlElement("AutoKlwProcUs")]
        public double AutoKlwProcUs { get; set; }
        [DisplayName("Ustawione autoklaw [%]")]
        [XmlElement("Ustawione autoklaw [%]")]
        public double AutoKlwCisUs { get; set; }
        [DisplayName("AutoKlwProcLs")]
        [XmlElement("AutoKlwProcLs")]
        public double AutoKlwProcLs { get; set; }
        [DisplayName("Ciśnienie ustawione autoklawu")]
        [XmlElement("Ciśnienie ustawione autoklawu")]
        public double AutoKlwCisLs { get; set; }
        [DisplayName("AutoKlwProcFs")]
        [XmlElement("AutoKlwProcFs")]
        public double AutoKlwProcFs { get; set; }
        [DisplayName("AutoKlwCisFs")]
        [XmlElement("AutoKlwCisFs")]
        public double AutoKlwCisFs { get; set; }
        [DisplayName("AutoKlwProcMs")]
        [XmlElement("AutoKlwProcMs")]
        public double AutoKlwProcMs { get; set; }
        [DisplayName("AutoKlwCisMs")]
        [XmlElement("AutoKlwCisMs")]
        public double AutoKlwCisMs { get; set; }
        [DisplayName("Czas autoklawu")]
        [XmlElement("Czas autoklawu")]
        public double AutoKlwCzas { get; set; }
        [DisplayName("Czas stabilizacji")]
        [XmlElement("Czas stabilizacji")]
        public double Stabilization { get; set; }
        [DisplayName("Czas chłodzenia")]
        [XmlElement("Czas chłodzenia")]
        public double CoolingTime { get; set; }
        [DisplayName("Gęstość bloku")]
        [XmlElement("Gęstość bloku")]
        public double DensityBlockNumber { get; set; }
        [DisplayName("Data modyfikacji")]
        [XmlElement("Data modyfikacji")]
        public DateTime ModificationDate { get; set; }
        [DisplayName("Użytkownik")]
        [XmlElement("Użytkownik")]
        public string User { get; set; }
        [DisplayName("Flaga zakwalifikowania do odpadu")]
        [XmlElement("Flaga zakwalifikowania do odpadu")]
        public int Waste { get; set; }
        [DisplayName("Flagausunięcia bloku z ewidencji")]
        [XmlElement("Flagausunięcia bloku z ewidencji")]
        public int Removed { get; set; }
        [DisplayName("Rok produkcji")]
        [XmlElement("Rok produkcji")]
        public int PzYear { get; set; }
        [DisplayName("Flaga pocięcia")]
        [XmlElement("Flaga pocięcia")]
        public int Pociety { get; set; }
    }
}
