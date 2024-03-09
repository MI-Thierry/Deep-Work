using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml.Controls;

namespace DeepWork.Views.Pages;
public sealed partial class AddLongTaskDialogPage : Page
{
    public LongTaskViewModel ViewModel { get; set; }
    public AddLongTaskDialogPage()
	{
		ViewModel = new LongTaskViewModel();
		this.InitializeComponent();
	}
}
