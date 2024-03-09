using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace DeepWork.UserControls;

public sealed partial class TimeSpanSlider : UserControl
{
	public TimeSpan TimeSpan
	{
		get { return (TimeSpan)GetValue(TimeSpanProperty); }
		set { SetValue(TimeSpanProperty, value); }
	}
	public static readonly DependencyProperty TimeSpanProperty =
		DependencyProperty.Register(nameof(TimeSpan), typeof(TimeSpan), typeof(TimeSpanSlider), new PropertyMetadata(TimeSpan.Zero));

	public TimeSpanSlider()
	{
		this.InitializeComponent();
	}

	public string ConvertTimeSpanToString(TimeSpan value, string conversionPart)
	{
		TimeSpanParts part = (TimeSpanParts)Enum.Parse(typeof(TimeSpanParts), conversionPart);
		return part switch
		{
			TimeSpanParts.Hours => value.Hours.ToString("00"),
			TimeSpanParts.Minutes => value.Minutes.ToString("00"),
			TimeSpanParts.Seconds => value.Seconds.ToString("00"),
			_ => throw new InvalidOperationException("Unknown conversion type.")
		};
	}

	public void ConvertHoursBackToTimeSpan(string value) =>
		TimeSpan = ConvertBackToTimeSpan(value, TimeSpanParts.Hours);
	public void ConvertMinutesBackToTimeSpan(string value) =>
		TimeSpan = ConvertBackToTimeSpan(value, TimeSpanParts.Minutes);
	public void ConvertSecondsBackToTimeSpan(string value) =>
		TimeSpan = ConvertBackToTimeSpan(value, TimeSpanParts.Seconds);


	private TimeSpan ConvertBackToTimeSpan(string value, TimeSpanParts conversionType)
	{
		TimeSpan span = TimeSpan;
		switch (conversionType)
		{
			case TimeSpanParts.Hours:
				span -= TimeSpan.FromHours(span.Hours);
				span += TimeSpan.FromHours(int.Parse(value));
				return span;

			case TimeSpanParts.Minutes:
				span -= TimeSpan.FromMinutes(span.Minutes);
				span += TimeSpan.FromMinutes(int.Parse(value));
				return span;

			case TimeSpanParts.Seconds:
				span -= TimeSpan.FromSeconds(span.Seconds);
				span += TimeSpan.FromSeconds(int.Parse(value));
				return span;

			default:
				throw new InvalidOperationException("The conversion type is unknown.");
		}
	}

	private void UpButton_Click(object sender, RoutedEventArgs e)
	{
		string Tag = (sender as Button).Tag as string;
		TimeSpanParts part = (TimeSpanParts)Enum.Parse(typeof(TimeSpanParts), Tag);
		switch (part)
		{
			case TimeSpanParts.Hours:
				HoursDisplay.Focus(FocusState.Keyboard);
				TimeSpan += TimeSpan.FromHours(1);
				HoursDisplay.IsSelected = true;
				break;

			case TimeSpanParts.Minutes:
				MinutesDisplay.Focus(FocusState.Keyboard);
				TimeSpan += TimeSpan.FromMinutes(1);
				MinutesDisplay.IsSelected = true;
				break;

			case TimeSpanParts.Seconds:
				SecondDisplay.Focus(FocusState.Keyboard);
				TimeSpan += TimeSpan.FromSeconds(1);
				SecondDisplay.IsSelected = true;
				break;

			default:
				throw new InvalidOperationException("Unknown button tag.");
		}
	}

	private void DownButton_Click(object sender, RoutedEventArgs e)
	{
		string Tag = (sender as Button).Tag as string;
		TimeSpanParts part = (TimeSpanParts)Enum.Parse(typeof(TimeSpanParts), Tag);
		switch (part)
		{
			case TimeSpanParts.Hours:
				HoursDisplay.Focus(FocusState.Keyboard);
				TimeSpan -= TimeSpan.FromHours(1);
				HoursDisplay.IsSelected = true;
				break;

			case TimeSpanParts.Minutes:
				MinutesDisplay.Focus(FocusState.Keyboard);
				TimeSpan -= TimeSpan.FromMinutes(1);
				MinutesDisplay.IsSelected = true;
				break;

			case TimeSpanParts.Seconds:
				SecondDisplay.Focus(FocusState.Keyboard);
				TimeSpan -= TimeSpan.FromSeconds(1);
				SecondDisplay.IsSelected = true;
				break;
			default:
				throw new InvalidOperationException("Unknown button tag.");
		}
	}

	private void DisplayBorder_GotFocus(object sender, RoutedEventArgs e)
	{
		VisualStateManager.GoToState(this, nameof(Focused), false);
	}

	private void DisplayBorder_LostFocus(object sender, RoutedEventArgs e)
	{
		VisualStateManager.GoToState(this, nameof(Unfocused), false);
		HoursDisplay.IsSelected = false;
		MinutesDisplay.IsSelected = false;
		SecondDisplay.IsSelected = false;
	}

	private enum TimeSpanParts
	{
		None,
		Hours,
		Minutes,
		Seconds
	}
}
