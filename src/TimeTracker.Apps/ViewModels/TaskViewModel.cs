using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Diagnostics;
using System.Net.Http;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Projects;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Text;

namespace TimeTracker.Apps.ViewModels
{

    public class TaskViewModel : ViewModelBase
    {
        private TaskItem _task;
        private String _name;
        private string _nameButton;
        private long _Id;
        private int _projectId;
        private ObservableCollection<TimeItem> _times;
        HttpClient client;

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

        public ObservableCollection<TimeItem> Times
        {
            get => _times;
            set => SetProperty(ref _times, value);
        }

        public string NameButton
        {
            get => _nameButton;
            set => SetProperty(ref _nameButton, value);
        }
        public TaskViewModel(TaskItem task,int projectId)
        {
            _task = task;
            _name = task.Name;
            _Id = task.Id;
            _projectId = projectId;
            _times = convertList(_task, task.Times);
            client = new HttpClient();
            OnClickAddTimeButton = new Command(onClickAddTimeButton);
            _nameButton = "Démarrer";
        }

        public ObservableCollection<TimeItem> convertList(TaskItem task, List<TimeItem> times)
        {
            ObservableCollection<TimeItem> finalList = new ObservableCollection<TimeItem>();
            foreach(TimeItem time in task.Times)
            {
                TimeSpan diff = time.EndTime.Subtract(time.StartTime);
                time.Difference = diff;
                Debug.WriteLine(time.Difference);
                Debug.WriteLine(time.Id);
                finalList.Add(time);
            }
            return finalList;
        }

        public Command OnClickAddTimeButton
        {
            get;
        }

        public async void onClickAddTimeButton()
        {
            AddTimeRequest addTimeRequest = new AddTimeRequest();
            addTimeRequest.StartTime = DateTime.Now;
            addTimeRequest.EndTime = DateTime.Now;

            string json = JsonConvert.SerializeObject(addTimeRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri((Urls.HOST + "/" + Urls.ADD_TIME).Replace("{projectId}",_projectId.ToString()).Replace("{taskId}", _Id.ToString()));
                if (!client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                }
                HttpResponseMessage response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    TimeItem item = JsonConvert.DeserializeObject<TimeItem>(responseBody);
                    _task.Times.Add(item);
                    Times = convertList(_task, _task.Times);
                    Debug.WriteLine(response);
                }
                

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}