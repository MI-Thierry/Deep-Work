using Microsoft.UI.Xaml.Data;
using System;

namespace DeepWork.Helpers
{
	public class TimeSpanToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			TimeSpan timeSpan = (TimeSpan)value;
			if (timeSpan < TimeSpan.FromMinutes(1))
				return $"{timeSpan.Seconds} secs";
			else if (timeSpan < TimeSpan.FromHours(1))
				return $"{timeSpan.Minutes} mins and {timeSpan.Seconds} secs";
			else if (timeSpan < TimeSpan.FromDays(1))
				return $"{timeSpan.Hours} hrs and {timeSpan.Minutes} mins";
			else if (timeSpan > TimeSpan.FromDays(1))
				return $"{timeSpan.Days} days and {timeSpan.Hours} hrs";
			else
				return "0 Sec";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
