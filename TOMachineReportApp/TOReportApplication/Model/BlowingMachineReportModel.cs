using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Model
{
    public class BlowingMachineReportModel : ReportModelBase
    {
        public string Machine { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeStop { get; set; }

        public string OrderNumber { get; set; }

        public double RecipeNumber { get; set; }

        public string Manufacturer { get; set; }

        public string Type { get; set; }

        public double DensitySet { get; set; }

        public double DensityMin { get; set; }

        public double DensityMean { get; set; }

        public double DensityMax { get; set; }

        public double WeightSet { get; set; }

        public double WeightActual { get; set; }

        public int BatchesActual { get; set; }

        public string Operator { get; set; }

        public string Chamber { get; set; }

        public string LotNumber { get; set; }

        public string Material { get; set; }

        public double CycleTime { get; set; }

        public string Silos0 { get; set; }

        public double TimeSteam { get; set; }

        public int LevelSwitch { get; set; }

        public double Bypass { get; set; }

        public double InputWeight { get; set; }

        public double SteamPressure { get; set; }

        public double SpeedAgitator { get; set; }

        public double SpeedRotaryValve { get; set; }

        public double FlapFluidizedBed { get; set; }

        public double FlapTransportUnit { get; set; }
    }
}
