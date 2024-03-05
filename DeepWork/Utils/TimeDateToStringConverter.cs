using Microsoft.UI.Xaml.Data;
using System;

namespace DeepWork.Utils
{
	public class TimeDateToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var dateTime = (DateTime)value;
			string day = dateTime.ToString("ddd");
			string month = dateTime.ToString("MMM");
			string date = dateTime.ToString("dd");
			string year = dateTime.ToString("yyyy");
			return $"{day}, {month} {date}, {year}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return DateTime.Now;
		}
	}
}
