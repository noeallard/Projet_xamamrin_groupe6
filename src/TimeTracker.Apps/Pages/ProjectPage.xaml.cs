using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Apps.Models;
using TimeTracker.Apps.ViewModels;
using TimeTracker.Dtos.Projects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Apps.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectPage : ContentPage
    {
        public ProjectPage(ObservableCollection<TaskItem> tasks, Project project)
        {
            InitializeComponent();
            BindingContext = new ProjectViewModel(tasks, project);
            taskList.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;
                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
            };

        }
    }
}