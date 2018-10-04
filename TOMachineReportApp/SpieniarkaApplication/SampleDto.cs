using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpieniarkaApplication
{
    class SampleDto
    {
        public string Id { get; set; }
        public string Wersja { get; set; }
        public DateTime DateTime { get; set; }
        public string NrZlecenia { get; set; }
        public int NrWsadu { get; set; }
        public double NadwZadana { get; set; }
        public double NadwRzecz { get; set; }
        public int LicznikRegulacji { get; set; }
        public double GestZadana { get; set; }
        public double GestRzecz { get; set; }
        public double CzasParowania { get; set; }
        public double CisnPary { get; set; }
        public double TPary { get; set; }
        public double TKotla { get; set; }
        public double CzasCyklu { get; set; }
        public double JWagi { get; set; }
        public double JGest { get; set; }
        public double JCisn { get; set; }
        public double JTemp { get; set; }
    }
}
