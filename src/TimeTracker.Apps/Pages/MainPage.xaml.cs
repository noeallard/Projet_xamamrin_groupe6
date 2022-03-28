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
            InitializeComponent();
            BindingContext = new MainViewModel();
            NavigationPage.SetHasBackButton(this, false);
            mainList.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;
                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
            };
        }
    }
}