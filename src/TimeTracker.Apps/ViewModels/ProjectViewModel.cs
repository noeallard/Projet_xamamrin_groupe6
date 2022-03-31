using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Storm.Mvvm;
using System.Linq;
using System.Text;
using TimeTracker.Dtos.Projects;
using Xamarin.Forms;
using TimeTracker.Apps.Pages;

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
            CommandTask();
        }

        public ObservableCollection<TaskItem> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }
        public void CommandTask()
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                _tasks[i].OnClickAffTask = new Command<TaskItem>(GoToTaskPage);
            }
        }

        public async void GoToTaskPage(TaskItem task)
        {
            var taskPage = new TaskPage(task,_projectId);
            await NavigationService.PushAsync(taskPage);
        }
    }
}