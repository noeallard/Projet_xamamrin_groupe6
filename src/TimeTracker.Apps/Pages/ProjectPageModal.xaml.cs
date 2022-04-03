using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ProjectPageModal : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ProjectPageModal(ObservableCollection<TaskItem> tasks, Project project)
        {
            InitializeComponent();
            BindingContext = new ProjectViewModel(tasks,project);
        }
    }
}