using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DeepWork.ViewModels.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _counter;

        [RelayCommand]
        void OnIncrementCounter()
        {
            Counter++;
        }
    }
}
