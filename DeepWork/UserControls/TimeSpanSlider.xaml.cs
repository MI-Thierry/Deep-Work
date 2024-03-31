using DeepWork.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Linq;
using System.Text;
using Windows.System;

namespace DeepWork.UserControls;

public sealed partial class TimeSpanSlider : UserControl
{
	public TimeSpan MaxTimeSpan
	{
		get { return (TimeSpan)GetValue(MaxTimeSpanProperty); }
		set { SetValue(MaxTimeSpanProperty, value); }
	}
	public static readonly DependencyProperty MaxTimeSpanProperty =
		DependencyProperty.Register(nameof(MaxTimeSpan), typeof(TimeSpan), typeof(TimeSpanSlider), new PropertyMetadata(TimeSpan.MaxValue));

	public TimeSpan MinTimeSpan
	{
		get { return (TimeSpan)GetValue(MinTimeSpanProperty); }
		set { SetValue(MinTimeSpanProperty, value); }
	}
	public static readonly DependencyProperty MinTimeSpanProperty =
		DependencyProperty.Register(nameof(MinTimeSpan), typeof(TimeSpan), typeof(TimeSpanSlider), new PropertyMetadata(TimeSpan.MinValue));

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

	public void ConvertHoursBackToTimeSpan(object value) =>
		TimeSpan = ConvertBackToTimeSpan(value as string, TimeSpanParts.Hours);
	public void ConvertMinutesBackToTimeSpan(object value) =>
		TimeSpan = ConvertBackToTimeSpan(value as string, TimeSpanParts.Minutes);
	public void ConvertSecondsBackToTimeSpan(object value) =>
		TimeSpan = ConvertBackToTimeSpan(value as string, TimeSpanParts.Seconds);


	private TimeSpan ConvertBackToTimeSpan(string value, TimeSpanParts conversionType)
	{
		TimeSpan span = TimeSpan;
		if (int.TryParse(value, out int result))
		{
			switch (conversionType)
			{
				case TimeSpanParts.Hours:
					span -= TimeSpan.FromHours(span.Hours);
					span += TimeSpan.FromHours(result);
					return (span > MaxTimeSpan && TimeSpan == MaxTimeSpan) ? MaxTimeSpan + TimeSpan.FromTicks(1)
						: (span > MaxTimeSpan && TimeSpan > MaxTimeSpan) ? MaxTimeSpan
						: (span > MaxTimeSpan) ? MaxTimeSpan
						: (span < MinTimeSpan && TimeSpan == MinTimeSpan) ? MinTimeSpan + TimeSpan.FromTicks(1)
						: (span < MinTimeSpan && TimeSpan > MinTimeSpan)? MinTimeSpan
						: (span < MinTimeSpan) ? MinTimeSpan
						: span;

				case TimeSpanParts.Minutes:
					span -= TimeSpan.FromMinutes(span.Minutes);
					span += TimeSpan.FromMinutes(result);
					return (span > MaxTimeSpan && TimeSpan == MaxTimeSpan) ? MaxTimeSpan + TimeSpan.FromTicks(1)
						: (span > MaxTimeSpan && TimeSpan > MaxTimeSpan) ? MaxTimeSpan
						: (span > MaxTimeSpan) ? MaxTimeSpan
						: (span < MinTimeSpan && TimeSpan == MinTimeSpan) ? MinTimeSpan + TimeSpan.FromTicks(1)
						: (span < MinTimeSpan && TimeSpan > MinTimeSpan) ? MinTimeSpan
						: (span < MinTimeSpan) ? MinTimeSpan
						: span;

				case TimeSpanParts.Seconds:
					span -= TimeSpan.FromSeconds(span.Seconds);
					span += TimeSpan.FromSeconds(result);
					return (span > MaxTimeSpan && TimeSpan == MaxTimeSpan) ? MaxTimeSpan + TimeSpan.FromTicks(1)
						: (span > MaxTimeSpan && TimeSpan > MaxTimeSpan) ? MaxTimeSpan
						: (span > MaxTimeSpan) ? MaxTimeSpan
						: (span < MinTimeSpan && TimeSpan == MinTimeSpan) ? MinTimeSpan + TimeSpan.FromTicks(1)
						: (span < MinTimeSpan && TimeSpan > MinTimeSpan) ? MinTimeSpan
						: (span < MinTimeSpan) ? MinTimeSpan
						: span;

				default:
					throw new InvalidOperationException("The conversion type is unknown.");
			}
		}
		return span;
	}

	private void UpButton_Click(object sender, RoutedEventArgs e)
	{
		string Tag = (sender as Button).Tag as string;
		TimeSpanParts part = (TimeSpanParts)Enum.Parse(typeof(TimeSpanParts), Tag);
		if (TimeSpan <= MaxTimeSpan)
		{
			TimeSpan += part switch
			{
				TimeSpanParts.Hours => TimeSpan.FromHours(1),
				TimeSpanParts.Minutes => TimeSpan.FromMinutes(1),
				TimeSpanParts.Seconds => TimeSpan.FromSeconds(1),
				_ => throw new InvalidOperationException("Unknown button tag."),
			};
		}
	}

	private void DownButton_Click(object sender, RoutedEventArgs e)
	{
		string Tag = (sender as Button).Tag as string;
		TimeSpanParts part = (TimeSpanParts)Enum.Parse(typeof(TimeSpanParts), Tag);
		if (TimeSpan >= MinTimeSpan)
		{
			TimeSpan -= part switch
			{
				TimeSpanParts.Hours => TimeSpan.FromHours(1),
				TimeSpanParts.Minutes => TimeSpan.FromMinutes(1),
				TimeSpanParts.Seconds => TimeSpan.FromSeconds(1),
				_ => throw new InvalidOperationException("Unknown button tag."),
			};
		}
	}

	private enum TimeSpanParts
	{
		None,
		Hours,
		Minutes,
		Seconds
	}

	private void UserControl_GotFocus(object sender, RoutedEventArgs e)
	{
		VisualStateManager.GoToState(this, nameof(Focused), false);
	}

	private void UserControl_LostFocus(object sender, RoutedEventArgs e)
	{
		VisualStateManager.GoToState(this, nameof(Unfocused), false);
		HoursDisplay.IsChecked = false;
		MinutesDisplay.IsChecked = false;
		SecondDisplay.IsChecked = false;
	}

	private string GetCharsFromKeys(VirtualKey key, uint scanCode)
	{
		StringBuilder buffer = new(256);
		byte[] keyboardState = new byte[256];
		KeyboardHelpers.GetKeyboardState(keyboardState);

		_ = KeyboardHelpers.ToUnicode((uint)key, scanCode, keyboardState, buffer, 256, 0);
		return buffer.ToString();
	}

	private void HoursDisplay_KeyDown(object sender, KeyRoutedEventArgs e)
	{
		string input = GetCharsFromKeys(e.Key, e.KeyStatus.ScanCode);

		if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int _))
		{
			TimeSpan span = TimeSpan;
			char[] chars = TimeSpan.Hours.ToString("00").ToCharArray();
			string hrs = chars.Last() + input;
			span -= TimeSpan.FromHours(span.Hours);
			span += TimeSpan.FromHours(int.Parse(hrs));
			TimeSpan = span;
		}
	}


	private void MinutesDisplay_KeyDown(object sender, KeyRoutedEventArgs e)
	{
		string input = GetCharsFromKeys(e.Key, e.KeyStatus.ScanCode);

		if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int _))
		{
			TimeSpan span = TimeSpan;
			char[] chars = TimeSpan.Minutes.ToString("00").ToCharArray();
			string hrs = chars.Last() + input;
			span -= TimeSpan.FromMinutes(span.Minutes);
			span += TimeSpan.FromMinutes(int.Parse(hrs));
			TimeSpan = span;
		}
	}

	private void SecondDisplay_KeyDown(object sender, KeyRoutedEventArgs e)
	{
		string input = GetCharsFromKeys(e.Key, e.KeyStatus.ScanCode);

		if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int _))
		{
			TimeSpan span = TimeSpan;
			char[] chars = TimeSpan.Seconds.ToString("00").ToCharArray();
			string hrs = chars.Last() + input;
			span -= TimeSpan.FromSeconds(span.Seconds);
			span += TimeSpan.FromSeconds(int.Parse(hrs));
			TimeSpan = span;
		}
	}
}
