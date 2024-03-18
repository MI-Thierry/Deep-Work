using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DeepWork.UserControls
{
	public sealed partial class PmdrDurationInputBox : UserControl
	{
		public int MaxValue
		{
			get { return (int)GetValue(MaxValueProperty); }
			set { SetValue(MaxValueProperty, value); }
		}
		public static readonly DependencyProperty MaxValueProperty =
			DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(PmdrDurationInputBox), new PropertyMetadata(int.MaxValue));

		public int MinValue
		{
			get { return (int)GetValue(MinValueProperty); }
			set { SetValue(MinValueProperty, value); }
		}
		public static readonly DependencyProperty MinValueProperty =
			DependencyProperty.Register(nameof(MinValue), typeof(int), typeof(PmdrDurationInputBox), new PropertyMetadata(int.MinValue));

		public int StepValue
		{
			get { return (int)GetValue(StepValueProperty); }
			set { SetValue(StepValueProperty, value); }
		}
		public static readonly DependencyProperty StepValueProperty =
			DependencyProperty.Register(nameof(StepValue), typeof(int), typeof(PmdrDurationInputBox), new PropertyMetadata(1));

		public string PlaceHolderText
		{
			get { return (string)GetValue(PlaceHolderTextProperty); }
			set { SetValue(PlaceHolderTextProperty, value); }
		}
		public static readonly DependencyProperty PlaceHolderTextProperty =
			DependencyProperty.Register(nameof(PlaceHolderText), typeof(string), typeof(PmdrDurationInputBox), new PropertyMetadata(string.Empty));

		public int Value
		{
			get { return (int)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(int), typeof(PmdrDurationInputBox), new PropertyMetadata(0, ValueChanged));

		private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is PmdrDurationInputBox inputBox)
				inputBox.Value = ((int)e.NewValue > inputBox.MaxValue) ? inputBox.MaxValue
					: ((int)e.NewValue < inputBox.MinValue) ? inputBox.MinValue
					: (int)e.NewValue;
		}

		public PmdrDurationInputBox()
		{
			this.InitializeComponent();
		}

		private void UpButton_Click(object sender, RoutedEventArgs e) =>
			Value += StepValue;

		private void DownButton_Click(object sender, RoutedEventArgs e) =>
			Value -= StepValue;

		private void Grid_GotFocus(object sender, RoutedEventArgs e) =>
			VisualStateManager.GoToState(this, nameof(Focused), false);

		private void Grid_LostFocus(object sender, RoutedEventArgs e) =>
			VisualStateManager.GoToState(this, nameof(Unfocused), false);

	}
}
