using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
   
    public static class ContinuousBlowingMachineReportLogic
    {
        public static List<ContinuousBlowingMachineShiftReportModel> GenerateContinuousBlowingMachineFileReportModel(ContinuousBlowingMachineReportModel[] data, ShifTimeDefinition shiftInfo, DateTime selectedDate)
        { 
            var model = new List<ContinuousBlowingMachineShiftReportModel>();
            var dataForShift = (from item in data
                where
                    item.Date >= new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day,
                        shiftInfo.BeginningShift.Hours, shiftInfo.BeginningShift.Minutes,
                        shiftInfo.BeginningShift.Seconds) &&
                    item.Date <= new DateTime(selectedDate.Year, selectedDate.Month,
                        shiftInfo.NumberOfShift == ShiftInfoEnum.ThirdShift
                            ? selectedDate.AddDays(1).Day
                            : selectedDate.Day,
                        shiftInfo.EndShift.Hours, shiftInfo.EndShift.Minutes, shiftInfo.EndShift.Seconds)
                select item).ToArray();
            double sumDensityMeassurements = 0;
            var counter = 1;
            var startDate = new DateTime();

            for (int i = 0; i < dataForShift.Length; i++)
            {
                if (counter == 1)
                {
                    startDate = dataForShift[i].Date;
                }

                if ((i + 1) == dataForShift.Length || dataForShift[i].Chamber != dataForShift[i + 1].Chamber )
                {
                    counter++;
                    sumDensityMeassurements = sumDensityMeassurements + dataForShift[i].MeasurementDensity;
                    
                    if ((i + 1) == dataForShift.Length)
                    {
                        counter++;
                        sumDensityMeassurements = sumDensityMeassurements + dataForShift[i].MeasurementDensity;
                    }
                    model.Add(new ContinuousBlowingMachineShiftReportModel()
                    {
                        Chamber = dataForShift[i].Chamber,
                        AvgDensityOfMeassurement = Math.Round(sumDensityMeassurements / counter,2).ToString(),
                        DensitySet = dataForShift[i].DensitySet,
                        Material = dataForShift[i].Material,
                        Type = dataForShift[i].Type,
                        StartDate = startDate,
                        EndDate = dataForShift[i].Date,
                        Operator = dataForShift[i].Operator,
                        PzNumber = dataForShift[i].PzNumber
                    });
                    counter = 1;
                    sumDensityMeassurements = 0;
                }
                else
                {
                    counter++;
                    sumDensityMeassurements = sumDensityMeassurements + dataForShift[i].MeasurementDensity;
                }
            }

            return model;
        }
    }
}
