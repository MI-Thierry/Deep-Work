using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepWork.Helpers
{
	public class ListToArrayConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ((List<Axis>)value).ToArray();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return ((Axis[])value).ToList();
		}
	}
}
