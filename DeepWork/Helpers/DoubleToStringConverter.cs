using Microsoft.UI.Xaml.Data;
using System;

namespace DeepWork.Helpers
{
	public class DoubleToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language) =>
			$"{(double)value:0.00}";

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (double.TryParse(value as string, out double d))
				return d ;
			throw new InvalidOperationException("Value have to be a floating point number");
		}
	}
}
