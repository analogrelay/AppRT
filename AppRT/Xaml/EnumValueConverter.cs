using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AppRT.Xaml
{
    public class EnumValueConverter : IValueConverter
    {
        private Dictionary<object, string> _nameCache = new Dictionary<object, string>();
        private Dictionary<Type, FieldInfo[]> _typeCache = new Dictionary<Type, FieldInfo[]>();
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(string) && value.GetType().GetTypeInfo().IsEnum)
            {
                string name;
                if (!_nameCache.TryGetValue(value, out name))
                {
                    name = GetDisplayName(value, value.GetType());
                    _nameCache[value] = name;
                }
                return name;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }

        private string GetDisplayName(object value, Type enumType)
        {
            FieldInfo[] fields;
            if (!_typeCache.TryGetValue(enumType, out fields))
            {
                fields = enumType.GetRuntimeFields()
                                   .Where(f => f.IsPublic && f.IsStatic)
                                   .ToArray();
                _typeCache[enumType] = fields;
            }

            var matchingField = fields.FirstOrDefault(
                f => Equals(f.GetValue(null), value));
            if (matchingField == null)
            {
                throw new MissingMemberException(String.Format("No matching field in the Enum '{0}' for value '{1}'", enumType.FullName, value));
            }
            string displayName = matchingField.Name;
            DisplayNameAttribute attr = matchingField.GetCustomAttribute<DisplayNameAttribute>();
            if (attr != null)
            {
                displayName = attr.Name;
            }
            return displayName;
        }
    }
}
