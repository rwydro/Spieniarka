using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TOReportApplication.Model
{
    public class FormReportFileModel
    {
        [XmlElement("Zmiana")]
        public string Shift { get; set; }
        [XmlElement("Czas rozpoczecia")]
        public DateTime TimeFrom { get; set; }
        [XmlElement("Czas zakończenia")]
        public DateTime TimeTo { get; set; }
        [XmlElement("Liczba bloków")]
        public int NumberOfBlocks { get; set; }
        [XmlElement("Silos")]
        public int Silos { get; set; }
        [XmlElement("Komora")]
        public int Chamber { get; set; }
        [XmlElement("Operator")]
        public string Operator { get; set; }
        [XmlElement("Gatunek")]
        public string Type { get; set; }
        [XmlElement("Suma wag bloków")]
        public double SumBlockWeight { get; set; }
        [XmlElement("Numer PZ")]
        public int PzNumber { get; set; }
        [XmlElement("Średnia gęstość perełek")]
        public string AvgDensityOfPearls { get; set; }
    }
}
