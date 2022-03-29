using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Apps.ViewModels;
using TimeTracker.Dtos.Projects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Apps.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectPage : ContentPage
    {
        public ProjectPage(ObservableCollection<TaskItem> tasks,int projectId)
        {
            InitializeComponent();
            BindingContext = new ProjectViewModel(tasks,projectId);

            
        }
    }
}