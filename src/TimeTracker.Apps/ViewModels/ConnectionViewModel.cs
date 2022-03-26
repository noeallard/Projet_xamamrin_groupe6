using Storm.Mvvm;
using TimeTracker.Apps.Pages;
using Xamarin.Forms;

namespace TimeTracker.Apps.ViewModels
{
    public class ConnectionViewModel : ViewModelBase
    {
        public async void onClickRegisterButton()
        {
            await NavigationService.PushAsync<RegisterPage>();
        }

        public Command OnClickRegisterButton
        {
            get;
        }

        public ConnectionViewModel()
        {
            OnClickRegisterButton = new Command(onClickRegisterButton);
        }
    }
}