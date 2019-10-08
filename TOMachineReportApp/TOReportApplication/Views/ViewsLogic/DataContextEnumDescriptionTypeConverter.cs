using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using TOReportApplication.Logic.Enums;

namespace TOReportApplication.Views.ViewsLogic
{
    public class DataContextEnumDescriptionTypeConverter: IValueConverter
    {
        private static IEnumerable<string> GetDescriptions(DataContextEnum type)
        {
            var descs = new List<string>();
            var names = Enum.GetNames(typeof(DataContextEnum));
            foreach (var name in names)
            {
                var field = type.GetType().GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    descs.Add(fd.Description);
                }
            }
            return descs;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var myEnum = new DataContextEnum();
            var arrayDescription = GetDescriptions(myEnum);
            return arrayDescription;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
