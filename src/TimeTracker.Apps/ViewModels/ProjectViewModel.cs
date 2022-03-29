using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Storm.Mvvm;
using System.Linq;
using System.Text;
using TimeTracker.Dtos.Projects;
using Xamarin.Forms;

namespace TimeTracker.Apps.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        private ObservableCollection<TaskItem> _tasks;
        private int _projectId;
        public ProjectViewModel(ObservableCollection<TaskItem> tasks,int projectId)
        {
            _tasks = tasks;
            _projectId = projectId;
        }

        public ObservableCollection<TaskItem> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }
    }
}