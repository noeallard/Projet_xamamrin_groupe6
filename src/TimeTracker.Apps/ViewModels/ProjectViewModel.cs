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
using Microcharts;
using Entry = Microcharts.ChartEntry;
using SkiaSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TimeTracker.Apps.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        private ObservableCollection<TaskItem> _tasks;
        private Project _project;
        HttpClient client;
        private PieChart _chart;
        private List<Entry> _entries;
        public ProjectViewModel(ObservableCollection<TaskItem> tasks, Project project)
        {
            _tasks = tasks;
            _project = project;
            OnClickAddButton = new Command(onClickAddButton);
            OnClickSetProjectButton = new Command(onClickSetProjectButton);
            _entries = new List<Entry>();
            CommandTask();
            loadChart();
        }

        public PieChart Chart
        {
            get => _chart;
            set => SetProperty(ref _chart, value);
        }

        public List<Entry> Entries
        {
            get => _entries;
            set => SetProperty(ref _entries, value);
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

        public void loadChart()
        {
            refreshTasks();
            foreach (var task in _tasks)
            {
                int second = 0;
                foreach (var time in task.Times)
                {
                    TimeSpan diff = time.EndTime.Subtract(time.StartTime);
                    time.Difference = new TimeSpan(diff.Hours, diff.Minutes, diff.Seconds);
                    second += (int)time.Difference.TotalSeconds;
                }
                Random r = new Random();
                _entries.Add(new Entry(second)
                {
                    Color = new SKColor(((byte)r.Next(0, 256)), (byte)r.Next(0, 256), (byte)r.Next(0, 256)),
                    Label = task.Name,
                    ValueLabel = second.ToString()
                }); ; ; ;
            }
            PieChart donut = new PieChart();
            donut.Entries = _entries;
            Chart = donut;

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
            var taskPage = new TaskPage(task,_project);
            await NavigationService.PushAsync(taskPage);
        }

        public async void refreshTasks()
        {
            try
            {
                Uri uri = new Uri((Urls.HOST + "/" + Urls.LIST_TASKS).Replace("{projectId}", _project.Id.ToString()));
                if (!client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                }
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("AAAAAGJHGHJBHKDSHDSDJS");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    ObservableCollection<TaskItem> tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(parsedObject["data"].ToString());
                    _tasks = tasks;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public async void DeleteTask(TaskItem task)
        {
            try
            {
                client = new HttpClient();
                Uri uri = new Uri((Urls.HOST + "/" + Urls.DELETE_TASK).Replace("{projectId}", _project.Id.ToString()).Replace("{taskId}", task.Id.ToString()));
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
            var addTaskPage = new AddTaskPage(_tasks, _project);
            await NavigationService.PushAsync(addTaskPage);
        }


        public async void onClickSetProjectButton()
        {
            var editProjectPage = new EditProjectPage(_project);
            await NavigationService.PushAsync(editProjectPage);
        }

        public Command OnClickSetProjectButton 
        {
            get;
        }
    }
}