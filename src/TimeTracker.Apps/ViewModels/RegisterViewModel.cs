using Storm.Mvvm;
using Storm.Mvvm.Services;
using TimeTracker.Apps.Pages;
using Xamarin.Forms;

namespace TimeTracker.Apps.ViewModels
{
    internal class RegisterViewModel : ViewModelBase

    {
        public async void onClick()
        {
            await NavigationService.PushAsync<MainPage>();
        }
        public Command OnClick
        {
            get;
        }
        public RegisterViewModel()
        {
            OnClick = new Command(onClick);
        }
    }
}