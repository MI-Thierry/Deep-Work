using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace DeepWork.Helpers
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is not string enumString)
            {
                throw new ArgumentException("EnumToBooleanConverter parameter must be an enum name");
            }

            if (!Enum.IsDefined(typeof(ElementTheme), value))
            {
                throw new ArgumentException("EnumToBooleanConverter value must be an enum");
            }

            var enumValue = Enum.Parse(typeof(ElementTheme), enumString);

            return enumValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (parameter is not string enumString)
            {
                throw new ArgumentException("EnumToBooleanConverter parameter must be an enum name");
            }

            return Enum.Parse(typeof(ElementTheme), enumString);
        }
    }
}
