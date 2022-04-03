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
using System.Net.Http;
using TimeTracker.Dtos;
using Xamarin.Essentials;
using TimeTracker.Apps.Models;

namespace TimeTracker.Apps.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        private ObservableCollection<TaskItem> _tasks;
        private int _projectId;
        private Project _project;
        HttpClient client;
        public ProjectViewModel(ObservableCollection<TaskItem> tasks,int projectId, Project project)
        {
            _tasks = tasks;
            _projectId = projectId;
            _project = project;
            OnClickAddButton = new Command(onClickAddButton);
            OnClickSetProjectButton = new Command(onClickSetProjectButton);
            CommandTask();
        }

        public ProjectViewModel(ObservableCollection<TaskItem> tasks, int projectId)
        {
            _tasks = tasks;
            _projectId = projectId;
            OnClickAddButton = new Command(onClickAddButton);
            OnClickSetProjectButton = new Command(onClickSetProjectButton);
            CommandTask();
        }

        public String Title
        {
            get => _project.Name;
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
                _tasks[i].OnClickDeleteTask =  new Command<TaskItem>(DeleteTask);
            }
        }

        public async void GoToTaskPage(TaskItem task)
        {
            var taskPage = new TaskPage(task,_projectId,_project);
            await NavigationService.PushAsync(taskPage);
        }
        public async void DeleteTask(TaskItem task)
        {
            try
            {
                client = new HttpClient();
                Uri uri = new Uri((Urls.HOST + "/" + Urls.DELETE_TASK).Replace("{projectId}", _projectId.ToString()).Replace("{taskId}", task.Id.ToString()));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                HttpResponseMessage response = await client.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Task deleted");
                    int index = _tasks.IndexOf(task);
                    _tasks.RemoveAt(index);


                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public Command OnClickAddButton
        {
            get;
        }

        public async void onClickAddButton()
        {
            var addTaskPage = new AddTaskPage(_tasks,_projectId, _project);
            await NavigationService.PushAsync(addTaskPage);
        }


        public async void onClickSetProjectButton()
        {
            var editProjectPage = new EditProjectPage(_projectId);
            await NavigationService.PushAsync(editProjectPage);
        }

        public Command OnClickSetProjectButton 
        {
            get;
        }
    }
}