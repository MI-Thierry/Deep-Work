using DeepWork.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Reflection.Metadata.Ecma335;

namespace DeepWork.UserControls
{
	public enum DateTimeSliderDisplayType
	{
		Day,
		Week,
		Month,
		Year,
	}
	public sealed partial class DateTimeSlider : UserControl
	{
		public DateTimeSliderDisplayType DisplayType
		{
			get { return (DateTimeSliderDisplayType)GetValue(DisplayTypeProperty); }
			set { SetValue(DisplayTypeProperty, value); }
		}
		public static readonly DependencyProperty DisplayTypeProperty =
			DependencyProperty.Register(nameof(DisplayType), typeof(DateTimeSliderDisplayType), typeof(DateTimeSlider), new PropertyMetadata(DateTimeSliderDisplayType.Day));

		public DateTimeOffset DateTime
		{
			get { return (DateTimeOffset)GetValue(DateTimeProperty); }
			set { SetValue(DateTimeProperty, value); }
		}
		public static readonly DependencyProperty DateTimeProperty =
			DependencyProperty.Register(nameof(DateTime), typeof(DateTimeOffset), typeof(DateTimeSlider), new PropertyMetadata(DateTimeOffset.Now, DateTimeChanged));

		private static void DateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is DateTimeSlider dateSlider)
				dateSlider.DateTime = ((DateTimeOffset)e.NewValue > dateSlider.MaxDateTime) ? dateSlider.MaxDateTime
					: ((DateTimeOffset)e.NewValue < dateSlider.MinDateTime) ? dateSlider.MinDateTime
					: (DateTimeOffset)e.NewValue;
		}

		public TimeSpan StepSpan
		{
			get { return (TimeSpan)GetValue(StepSpanProperty); }
			set { SetValue(StepSpanProperty, value); }
		}
		public static readonly DependencyProperty StepSpanProperty =
			DependencyProperty.Register(nameof(StepSpan), typeof(TimeSpan), typeof(DateTimeSlider), new PropertyMetadata(TimeSpan.Zero));

		public DateTimeOffset MaxDateTime
		{
			get { return (DateTimeOffset)GetValue(MaxDateTimeProperty); }
			set { SetValue(MaxDateTimeProperty, value); }
		}
		public static readonly DependencyProperty MaxDateTimeProperty =
			DependencyProperty.Register(nameof(MaxDateTime), typeof(DateTimeOffset), typeof(DateTimeSlider), new PropertyMetadata(DateTimeOffset.MaxValue));

		public DateTimeOffset MinDateTime
		{
			get { return (DateTimeOffset)GetValue(MinDateTimeProperty); }
			set { SetValue(MinDateTimeProperty, value); }
		}
		public static readonly DependencyProperty MinDateTimeProperty =
			DependencyProperty.Register(nameof(MinDateTime), typeof(DateTimeOffset), typeof(DateTimeSlider), new PropertyMetadata(DateTimeOffset.MinValue));

		public DateTimeSlider()
		{
			this.InitializeComponent();
		}

		public string ConvertDateTimeToString(DateTimeOffset dateTime)
		{
			return DisplayType switch
			{
				DateTimeSliderDisplayType.Day => $"{DateTime:ddd}",
				DateTimeSliderDisplayType.Week => $"{DateTime.FirstDayOfWeek():dd/MMM}-{DateTime.LastDayOfWeek():dd/MMM}",
				DateTimeSliderDisplayType.Month => $"{DateTime:MMM}",
				DateTimeSliderDisplayType.Year => $"{DateTime:yyyy}",
				_ => $"{DateTime}",
			};
		}

		private void LeftButton_Click(object sender, RoutedEventArgs e) =>
			DateTime -= StepSpan;

		private void RightButton_Click(object sender, RoutedEventArgs e) =>
			DateTime += StepSpan;
	}
}
