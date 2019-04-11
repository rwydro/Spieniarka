using System;
using System.Collections.Generic;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;

namespace TOReportApplication.Logic
{
    public static class ModelDictionaries
    {
        public static List<string> FormShortDetailedReportVersionListField =
            new List<string>()
            {
                "Nr nadany" ,
                "Data organiki" ,
                "Data produkcji",
                "Waga [kg]" ,
                "Czas cyklu [s]",
                "Operator" ,
                "Silos" ,
                "Komora" ,
                "Ragranulat" ,
                "Gatunek",
                "Uwagi",
                "Sr.g. perełek"
            }; 

        public static Dictionary<string, string> FormDetailedReportDbModelPropertyNameDictionary =
            new Dictionary<string, string>()
            {
                {"id_blok", "Id"},
                {"data_czas", "ProductionDate"},
                {"nr_bloku" , "BlockNumber"},
                {"cykl_min", "CycleMin"},
                {"cykl_sek" ,"CycleSec"},
                {"masa_blok",  "Weight"},
                {"cisnienie_eps1", "PreassureEPS1"},
                {"cisnienie_eps2" ,"PreassureEPS2"},
                {"cisnienie_eps3" ,"PreassureEPS3"},
                {"regranulat" , "Blow"},
                {"wst_proznia",  "Vacuum"},
                {"czas_proznia", "VacuumTime"},
                {"czas_plukania1", "RinsingTime1"},
                {"czas_plukania2" ,"RinsingTime2"},
                {"czas_plukania3" ,"RinsingTime3"},
                {"czas_plukania4" ,"RinsingTime4"},
                {"klapa1" , "Flap1"},
                {"klapa2" , "Flap2"},
                {"klapa3" , "Flap3"},
                {"klapa4" , "Flap4"},
                {"autoklw_proc_us", "AutoKlwProcUs"},
                {"autoklw_cis_us" , "AutoKlwCisUs"},
                {"autoklw_proc_ls", "AutoKlwProcLs"},
                {"autoklw_cis_ls" , "AutoKlwCisLs"},
                {"autoklw_proc_fs", "AutoKlwProcFs"},
                {"autoklw_cis_fs" , "AutoKlwCisFs"},
                {"autoklw_proc_ms", "AutoKlwProcMs"},
                {"autoklw_cis_ms" , "AutoKlwCisMs"},
                {"autoklw_czas" , "AutoKlwCzas"},
                {"stabilizacja" , "Stabilization"},
                {"chlodzenie_czas", "CoolingTime"},
                {"gestosc_bloku" , "DensityBlockNumber"},
                {"czas_cyklu", "CycleTimeInSecond"},
                {"pz" , "PzNumber"},
                {"silos", "Silos"},
                {"komora", "Chamber"},
                {"data_mod", "ModificationDate"},
                {"uzytkownik", "User"},
                {"gatunek", "Type"},
                {"uwaga" , "Comments"},
                {"odpad" , "Waste"},
                {"usuniety", "Removed"},
                {"pz_rok" , "PzYear"},
                {"nrnadany", "AssignedNumber"},
                {"data_organiki", "OrganicDate"},
                {"pociety", "Pociety"},
                {"getosc_perelek", "AvgDensityOfPearls"},
                {"operator" , "Operator"},
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

        public static Dictionary<string, string> ContinuousBlowingMachineDbColumnNameToModelPropertyNameDictionary =
            new Dictionary<string, string>()
            {
                {"maszyna","Machine"},
                {"data_czas","Date"},
                {"gatunek","Type"},
                {"gestosc_zadana","DensitySet"},
                {"operator","Operator"},
                {"komora","Chamber"},
                {"numer_pz","PzNumber"},
                {"gestosc_z_pomiaru","MeasurementDensity"},
                {"otwarcie_pary","SteamOpening"},
                {"material","Material"},
                {"obroty_dozownika","DispenserRotation"},
            };
    }
}
