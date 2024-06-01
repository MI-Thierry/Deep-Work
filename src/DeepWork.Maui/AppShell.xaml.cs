using DeepWork.Maui.Views;

namespace DeepWork.Maui;

public partial class AppShell : Shell
{
	private static Dictionary<Type, string> _pagesRoutes = [];
	public AppShell()
	{
		InitializeComponent();
		RegisterPages();
	} 

	/// <summary>
	/// It registers the page in the shell
	/// </summary>
	/// <param name="shellPage">Parent page in that have a ShellItem</param>
	/// <param name="pages">The pages that makes up the route and last one as a target page</param>
	/// <returns>Route to the page</returns>
	public static string RegisterPage(Type shellPage, params Type[] pages)
	{
		string route = $"//{shellPage.Name}/{string.Join('/', pages.Select(page => page.Name))}";
		_pagesRoutes.Add(pages.Last(), route);
		Routing.RegisterRoute(route, pages.Last());
		return route;
	}

	public static void NavigateTo(Type pageType, bool animate = false, IDictionary<string, object>? parameters = null)
	{
		if (!_pagesRoutes.TryGetValue(pageType, out string? value))
			throw new InvalidOperationException("Specified page type is not registered in shell. Consider Calling AppShell.RegisterPage() to register the page");
		if (parameters != null)
			Current.GoToAsync(value, animate, parameters);
		else
			Current.GoToAsync(value, animate);
	}

	private void RegisterPages()
	{
		RegisterPage(typeof(SettingsPage), typeof(ThemeSettingsPage));
	}
}