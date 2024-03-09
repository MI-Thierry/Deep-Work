using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using System.Collections.Generic;
using System.Linq;

namespace DeepWork.UserControls
{
	[ContentProperty(Name ="Content")]
	public sealed class SelectableDisplay : Control
	{
		private Button Display;
		private static readonly Dictionary<string, List<SelectableDisplay>> _groups = new();
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set {
				SetValue(IsSelectedProperty, value);
				UpdateVisualState();
				if (!string.IsNullOrEmpty(GroupName) && value == true)
				{
					foreach (var item in _groups[GroupName])
					{
						if (item == this) continue;
						item.IsSelected = false;
					}
				}
			}
		}
		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(SelectableDisplay), new PropertyMetadata(false));

		public string GroupName
		{
			get { return (string)GetValue(GroupNameProperty); }
			set {
				if (_groups.ContainsKey(value))
					_groups[value].Add(this);
				else
					_groups.Add(value, new List<SelectableDisplay> { this });
				SetValue(GroupNameProperty, value);
			}
		}
		public static readonly DependencyProperty GroupNameProperty =
			DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(SelectableDisplay), new PropertyMetadata(string.Empty));

		public string Content
		{
			get { return (string)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register(nameof(Content), typeof(string), typeof(SelectableDisplay), new PropertyMetadata(string.Empty));

		public SelectableDisplay()
		{
			this.DefaultStyleKey = typeof(SelectableDisplay);
			IsTabStop = true;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (GetTemplateChild(nameof(Display)) is Button button)
			{
				Display = button;
				Display.Click += Display_Click;
				Display.KeyDown += Display_KeyDown;
			}
		}

		private void Display_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			// Todo: Add validation codes here.
			string input = Utils.KeyboardHelpers.GetCharsFromKeys(e.Key, false, false);
			char[] chars = Content.ToCharArray();
			Content = chars.Last() + input;
		}

		private void Display_Click(object sender, RoutedEventArgs e)
		{
			IsSelected = true;
		}

		private void UpdateVisualState()
		{
			bool useTransitions = false;
			if (IsSelected)
				VisualStateManager.GoToState(this,"Selected" ,useTransitions);
			else
				VisualStateManager.GoToState(this, "Unselected", useTransitions);
		}
	}
}
