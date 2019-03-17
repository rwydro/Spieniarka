using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Logic.Enums;
using TOReportApplication.Model;

namespace TOReportApplication.Logic
{
    public class ShifTimeDefinition
    {
        public TimeSpan BeginningShift { get; set; }
        public TimeSpan EndShift { get; set; }

        public ShiftInfoEnum NumberOfShift { get; set; }
    }
    public class ShiftCalendarManager
    {
         private List<ShifTimeDefinition> shiftsDefinition =new List<ShifTimeDefinition>()
         {
             new ShifTimeDefinition(){BeginningShift = new TimeSpan(7,0,0),EndShift = new TimeSpan(15,00,00), NumberOfShift = ShiftInfoEnum.FirstShift},
             new ShifTimeDefinition(){BeginningShift = new TimeSpan(15,0,0),EndShift = new TimeSpan(23,00,00),NumberOfShift = ShiftInfoEnum.SecondShift},
             new ShifTimeDefinition(){BeginningShift = new TimeSpan(23,0,0),EndShift = new TimeSpan(7,0,0),NumberOfShift = ShiftInfoEnum.ThirdShift},
         };

        private readonly Dictionary<string, ShiftInfoEnum> shiftInfoDictionary = new Dictionary<string, ShiftInfoEnum>()
        {
            {"1", ShiftInfoEnum.FirstShift},
            {"2", ShiftInfoEnum.SecondShift},
            {"3", ShiftInfoEnum.ThirdShift},
            {@"1\2", ShiftInfoEnum.FirstToSecondShitf},
            {@"2\3", ShiftInfoEnum.SecondToThirdShift},
            {@"3\1", ShiftInfoEnum.ThirdToFirstShift}
        };

        public List<FormDateReportModel> RemoveNastedRows(List<FormDateReportModel> dateReportModel)
        {
            int index = 0;
            int counter = -1;
            string shift = shiftInfoDictionary.Last().Key;
            var newDateReportModel = new List<FormDateReportModel>();

            for (int i = 0; i < dateReportModel.Count; i++)
            {
               
                if(dateReportModel[i].TimeTo.Hour >= shiftsDefinition.First().BeginningShift.Hours && dateReportModel[i].TimeTo.Date.Day == dateReportModel.First().TimeTo.Day)
                    newDateReportModel.Add(dateReportModel[i]);
                if (dateReportModel[i].TimeTo.Hour < shiftsDefinition.Last().EndShift.Hours && dateReportModel[i].TimeTo.Date.Day > dateReportModel.First().TimeTo.Day)
                   newDateReportModel.Add(dateReportModel[i]);

               counter++;
            }
            return newDateReportModel;
        }

        public ShiftInfoEnum GetShift(TimeSpan fromDate, TimeSpan toDate)
        {
            foreach (var shift in shiftsDefinition)
            {                
                if (fromDate < shiftsDefinition.ElementAt(2).BeginningShift && toDate > shiftsDefinition.ElementAt(2).BeginningShift || 
                    fromDate > toDate && fromDate < shiftsDefinition.ElementAt(2).BeginningShift ||
                    fromDate > shift.BeginningShift && toDate >= shift.EndShift && shift.NumberOfShift == ShiftInfoEnum.SecondShift)
                    return ShiftInfoEnum.SecondToThirdShift;    
                if (fromDate > shift.BeginningShift && fromDate<shift.EndShift && toDate >= shift.EndShift && shift.NumberOfShift== ShiftInfoEnum.FirstShift )
                    return ShiftInfoEnum.FirstToSecondShitf;
                if(fromDate < shiftsDefinition.ElementAt(0).BeginningShift && toDate > shiftsDefinition.ElementAt(0).BeginningShift||
                   fromDate > shiftsDefinition.ElementAt(2).BeginningShift && toDate > shiftsDefinition.ElementAt(0).BeginningShift)
                    return ShiftInfoEnum.ThirdToFirstShift;
                if (fromDate > toDate || toDate < shiftsDefinition.ElementAt(0).BeginningShift)
                    return ShiftInfoEnum.ThirdShift;
                if (fromDate >= shift.BeginningShift && toDate < shift.EndShift)
                    return shift.NumberOfShift;
            }
            return ShiftInfoEnum.FirstShift;
        }

        public string GetShiftAsString(ShiftInfoEnum shift)
        {
            return (from sh in shiftInfoDictionary where sh.Value == shift select sh.Key).FirstOrDefault();
        }

        public ShifTimeDefinition GetShiftInfo(string selectedShift)
        {
            var enumShift = shiftInfoDictionary.FirstOrDefault(s => s.Key == selectedShift).Value;
            return shiftsDefinition.FirstOrDefault(s => s.NumberOfShift == enumShift);
        }
    }
}
