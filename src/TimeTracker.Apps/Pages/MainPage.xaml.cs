using Storm.Mvvm.Forms;
using TimeTracker.Apps.ViewModels;
using Xamarin.Forms;

namespace TimeTracker.Apps.Pages
{
    public partial class MainPage : BaseContentPage
    {
        MainViewModel viewModel = new MainViewModel();
        public MainPage()
        {
            viewModel.loadListProject();
            InitializeComponent();
            BindingContext = new MainViewModel();
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}