using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Logic
{
    public static class ModelDictionaries
    {
        public static Dictionary<string, string> FormDetailedReportDbModelPropertyNameDictionary =
            new Dictionary<string, string>()
            {
                {"data_organiki", "OrganicDate"},
                {"waga", "Weight"},
                {"cykl_sec", "CycleTimeInSecond"},
                {"wsk_proznia", "Blow"},
                {"gatunek", "Type"},
                {"uwaga", "Comments"},
                {"getosc_perelek", "AvgDensityOfPearls"},
                {"data_czas"," ProductionDate"},
                {"silos","Silos"},  
                {"komora","Chamber"} ,
                {"operator","Operator"}
    };

        public static Dictionary<string, string> BlowingMachineDbColumnNameToModelPropertyNameDictionary =
            new Dictionary<string, string>()
            {
                {"maszyna","Machine"},
                {"data_poczatek","DateTimeStart"},
                {"data_koniec","DateTimeStop"},
                {"nr_zlecenia","OrderNumber"},
                {"nr_recepty","RecipeNumber"},
                {"producent","Manufacturer"},
                {"typ","Type"},
                {"gestosc_zadana","DensitySet"},
                {"gestosc_min","DensityMin"},
                {"gestosc_srednia","DensityMean"},
                {"gestosc_max","DensityMax"},
                {"ilosc_zad_surowca","WeightSet"},
                {"ilosc_rzecz_surowca","WeightActual"},
                {"ilosc_rzecz_partii","BatchesActual"},
                {"operator","Operator"},
                {"komora","Chamber"},
                {"nr_lot","LotNumber"},
                {"material","Material"},
                {"czas_cyklu","CycleTime"},
                {"silos_0","Silos0"},
                {"czas_pary","TimeSteam"},
                {"wyl_poziomu","LevelSwitch"},
                {"bajpas","Bypass"},
                {"wartosc_nawazki","InputWeight"},
                {"cisn_pary","SteamPressure"},
                {"predkosc_mieszadla","SpeedAgitator"},
                {"predkosc_sluzy_lopatk","SpeedRotaryValve"},
                {"klapa_suszarki","FlapFluidizedBed"},
                {"klapa_transportu","FlapTransportUnit"}
            };
    }
}
