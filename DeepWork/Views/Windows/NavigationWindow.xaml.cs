using DeepWork.ViewModels.Windows;
using DeepWork.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;
using System;
using Microsoft.UI.Windowing;
using Windows.Foundation;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using Microsoft.UI.Input;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI;
using Windows.UI;

namespace DeepWork.Views.Windows
{
	public sealed partial class NavigationWindow : Window
	{
		public NavigationWindowViewModel ViewModel { get; set; }
		public NavigationWindow(NavigationWindowViewModel viewModel)
		{
			ViewModel = viewModel;
			this.InitializeComponent();

			if (MicaController.IsSupported())
			{
				SystemBackdrop = new MicaBackdrop();
			}
			AppWindow.Changed += AppWindow_Changed;
			AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
			AppTitleBar.Loaded += AppTitleBar_Loaded;
			(Content as FrameworkElement).ActualThemeChanged += NavigationWindow_ActualThemeChanged;

			ExtendsContentIntoTitleBar = true;
			if (ExtendsContentIntoTitleBar == true)
			{
				AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
			}

			BackRequestButton.PointerEntered += BackRequestButton_PointerEntered;
			BackRequestButton.PointerExited += BackRequestButton_PointerExited;
			BackRequestButton.PointerPressed += BackRequestButton_PointerPressed;
		}

		private void NavigationWindow_ActualThemeChanged(FrameworkElement sender, object args)
		{
			if (ExtendsContentIntoTitleBar)
				SetTheme();
		}

		private void SetTheme()
		{
			switch ((Content as FrameworkElement).ActualTheme)
			{
				case ElementTheme.Light:
					AppWindow.TitleBar.ButtonHoverBackgroundColor = new Color { A = 10, B = 0, G = 0, R = 0 };
					AppWindow.TitleBar.ButtonPressedBackgroundColor = new Color { A = 20, B = 0, G = 0, R = 0 };
					AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
					AppWindow.TitleBar.ButtonHoverForegroundColor = Colors.Black;
					break;

				case ElementTheme.Dark:
					AppWindow.TitleBar.ButtonHoverBackgroundColor = new Color { A = 10, B = 255, G = 255, R = 255 };
					AppWindow.TitleBar.ButtonPressedBackgroundColor = new Color { A = 20, B = 255, G = 255, R = 255 };
					AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
					AppWindow.TitleBar.ButtonHoverForegroundColor = Colors.White;
					break;
			}
		}

		private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
		{
			if (ExtendsContentIntoTitleBar)
			{
				// Set initial interactive regions
				SetRegionsForCustomTitleBar();

				SetTheme();
			}
		}

		private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (ExtendsContentIntoTitleBar)
			{
				// Update interactive regions if the sizes of the window changes.
				SetRegionsForCustomTitleBar();
			}
		}
		private void SetRegionsForCustomTitleBar()
		{
			// Specify the interactive regions of the title bar.
			double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

			RightPaddingColumn.Width = new GridLength(AppWindow.TitleBar.RightInset / scaleAdjustment);
			LeftPaddingColumn.Width = new GridLength(AppWindow.TitleBar.LeftInset / scaleAdjustment);

			// Get rectangle around BackRequestButton control.
			GeneralTransform transform = BackRequestButton.TransformToVisual(null);
			Rect bounds = transform.TransformBounds(new Rect(0, 0, BackRequestButton.ActualWidth, BackRequestButton.ActualHeight));
			RectInt32 backRequestButtonRect = GetRect(bounds, scaleAdjustment);

			RectInt32[] rectArray = [backRequestButtonRect];

			InputNonClientPointerSource nonClientPointerSource = InputNonClientPointerSource.GetForWindowId(AppWindow.Id);
			nonClientPointerSource.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
		}

		private RectInt32 GetRect(Rect bounds, double scale)
		{
			return new RectInt32(
				_X: (int)Math.Round(bounds.X * scale),
				_Y: (int)Math.Round(bounds.Y * scale),
				_Width: (int)Math.Round(bounds.Width * scale),
				_Height: (int)Math.Round(bounds.Height * scale)
				);
		}

		private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
		{
			if (args.DidPresenterChange)
			{
				switch (sender.Presenter.Kind)
				{
					case AppWindowPresenterKind.CompactOverlay:
						// Compact overlay - hide custom title bar
						// and use the default system title bar instead.
						AppTitleBar.Visibility = Visibility.Collapsed;
						sender.TitleBar.ResetToDefault();
						break;

					case AppWindowPresenterKind.FullScreen:
						// Full screen - hide the custom title bar
						// and the default system title bar.
						AppTitleBar.Visibility = Visibility.Collapsed;
						sender.TitleBar.ExtendsContentIntoTitleBar = true;
						break;

					case AppWindowPresenterKind.Overlapped:
						// Normal - hide the system title bar
						// and use the custom title bar instead.
						AppTitleBar.Visibility = Visibility.Visible;
						sender.TitleBar.ExtendsContentIntoTitleBar = true;
						break;

					default:
						// Use the default system title bar.
						sender.TitleBar.ResetToDefault();
						break;
				}
			}
		}

		private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new InvalidOperationException("Failed to navigate to specified page.");
		}

		private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
		{
			if (args.IsSettingsInvoked)
				ContentFrame.Navigate(typeof(SettingsPage), null, args.RecommendedNavigationTransitionInfo);
			else if (args.InvokedItemContainer != null)
			{
				Type pageType = args.InvokedItemContainer.Tag as Type;
				ContentFrame.Navigate(pageType, null, args.RecommendedNavigationTransitionInfo);
			}
		}

		private void NavigationView_Loaded(object sender, RoutedEventArgs e)
		{
			Type pageType = ViewModel.MenuItems.First().Tag as Type;
			ContentFrame.Navigate(pageType, null, new EntranceNavigationTransitionInfo());
		}

		private void BackRequestButton_Click(object sender, RoutedEventArgs e)
		{
			ContentFrame.GoBack();
			NavView.SelectedItem = ViewModel.MenuItems.First(item => item.Tag as Type == ContentFrame.CurrentSourcePageType);
		}

		private void BackRequestButton_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			AnimatedIcon.SetState(BackRequestButtonIcon, "PointerOver");
		}

		private void BackRequestButton_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			AnimatedIcon.SetState(BackRequestButtonIcon, "Normal");
		}

		private void BackRequestButton_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			AnimatedIcon.SetState(BackRequestButton, "Pressed");
		}
	}
}
