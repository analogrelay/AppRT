using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Globalization;

namespace AppRT.Xaml
{
    public class FloatToCurrencyStringConverter : IValueConverter
    {
        public bool IncludeCurrencySymbol { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(string))
            {
                float val;
                if (value is float?)
                {
                    float? nullableVal = (float?)value;
                    if (nullableVal == null)
                    {
                        return String.Empty;
                    }
                    val = nullableVal.Value;
                }
                else if (value is float)
                {
                    val = (float)value;
                }
                else
                {
                    return value;
                }

                if (IncludeCurrencySymbol)
                {
                    return String.Format("{0:c}", val);
                }
                else
                {
                    return String.Format("{0:F2}", val);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string s = value as string;
            float f;
            if (s != null && targetType.GetTypeInfo().IsAssignableFrom(typeof(float).GetTypeInfo()) && Single.TryParse(s, out f))
            {
                return f;
            }
            return value;
        }
    }
}
