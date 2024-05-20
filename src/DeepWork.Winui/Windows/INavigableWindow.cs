using Microsoft.UI.Xaml.Media.Animation;
using System;
using Windows.Graphics;

namespace DeepWork.Winui.Windows;
public interface INavigableWindow
{
	void NavigateTo(Type pageType, NavigationTransitionInfo navigationTransitionInfo);
	void SetDragRegion(RectInt32[] rectArray);
}
