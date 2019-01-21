using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;

namespace TOReportApplication.Views.ViewsLogic
{
    public class NumericValidationRule: ValidationRule
    {
        public Type ValidationType { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = Convert.ToString(value);
            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");

            bool canConvert = false;
            switch (ValidationType.Name)
            {
                case "Boolean":
                    bool boolVal = false;
                    canConvert = bool.TryParse(strValue, out boolVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of boolean");
                case "Double":
                    double doubleVal = 0;
                    canConvert = Double.TryParse(strValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,out doubleVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");
                case "Int32":
                    int intValue = 0;
                    canConvert = Int32.TryParse(strValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out intValue);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");
                default:
                    throw new InvalidCastException($"{ValidationType.Name} is not supported");
            }
        }
    }
}

