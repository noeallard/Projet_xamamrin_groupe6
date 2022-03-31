using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Diagnostics;
using System.Net.Http;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Projects;
using Xamarin.Essentials;

namespace TimeTracker.Apps.ViewModels
{

    public class TaskViewModel : ViewModelBase
    {
        private TaskItem _task;
        private String _name;
        private long _Id;
        private int _projectId;

       public String Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public long Id
        {
            get => _Id;
            set => SetProperty(ref _Id, value);
        }

        public TaskViewModel(TaskItem task,int projectId)
        {
            _task = task;
            _name = task.Name;
            _Id = task.Id;
            _projectId = projectId;


            

        }

    }
}