using DeepWork.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.System;

namespace DeepWork.UserControls;

[ContentProperty(Name = "Content")]
public sealed class SelectableDisplay : Control
{
	private Button Display;
	private static readonly Dictionary<string, List<SelectableDisplay>> _groups = [];

	public event TypedEventHandler<SelectableDisplay, SelectableDisplayBeforeTextChangingEventArgs> BeforeTextChanging;
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
			if (_groups.TryGetValue(value, out List<SelectableDisplay> group))
				group.Add(this);
			else
				_groups.Add(value, [this]);
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

	private string GetCharsFromKeys(VirtualKey key)
	{
		StringBuilder buffer = new(256);
		byte[] keyboardState = new byte[256];
		KeyboardHelpers.GetKeyboardState(keyboardState);

		_ = KeyboardHelpers.ToUnicode((uint)key, 0, keyboardState, buffer, 256, 0);
		return buffer.ToString();
	}

	private void Display_KeyDown(object sender, KeyRoutedEventArgs e)
	{
		string input = GetCharsFromKeys(e.Key);
		if (!string.IsNullOrEmpty(input))
		{
			SelectableDisplayBeforeTextChangingEventArgs args = new(input);
			BeforeTextChanging?.Invoke(this, args);
			if (!args.Cancel)
			{
				char[] chars = Content.ToCharArray();
				Content = chars.Last() + input;
			}
		}
	}

	private void Display_Click(object sender, RoutedEventArgs e)
	{
		IsSelected = true;
	}

	private void UpdateVisualState()
	{
		bool useTransitions = false;
		if (IsSelected)
			VisualStateManager.GoToState(this, "Selected", useTransitions);
		else
			VisualStateManager.GoToState(this, "Unselected", useTransitions);
	}
}

public class SelectableDisplayBeforeTextChangingEventArgs(string newText)
{
	public bool Cancel { get; set; } = false;
	public string NewText { get; set; } = newText;
}