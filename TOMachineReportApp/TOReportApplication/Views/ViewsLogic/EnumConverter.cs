using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TOReportApplication.Logic.Enums;

namespace TOReportApplication.Views.ViewsLogic
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if ((FormDetailedReportTypeEnum) value == FormDetailedReportTypeEnum.FullVersionDetailedReport)
            {
                return FormDetailedReportTypeEnum.FullVersionDetailedReport;
            }
            if ((FormDetailedReportTypeEnum)value == FormDetailedReportTypeEnum.Any)
            {
                return FormDetailedReportTypeEnum.Any;
            }
            if ((FormDetailedReportTypeEnum)value == FormDetailedReportTypeEnum.ShortVersionDetailedReport)
            {
                return FormDetailedReportTypeEnum.ShortVersionDetailedReport;
            }
            var enumValue = default(Enum);
            if (parameter is Type)
            {
                enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());
            }
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                returnValue = (int)Enum.Parse((Type)parameter, value.ToString());
            }
            return returnValue;
        }
    }
}
