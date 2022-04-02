using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using TimeTracker.Apps.Pages;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Projects;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Net.WebRequestMethods;

namespace TimeTracker.Apps.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        HttpClient client;
        private String _name;
        private int _ProjectId;
        private ObservableCollection<TaskItem> _tasks;


        public String Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }
        public int ProjectId{
            get { return _ProjectId; }
            set {SetProperty(ref _ProjectId, value); }
        }
        public ObservableCollection<TaskItem> Tasks
        {
            get { return _tasks; }
            set {SetProperty(ref _tasks, value); }
        }


        public Command OnClickAddTaskButton
        {
            get;
        }

        public async void onClickAddTaskButton()
        {
            AddTaskRequest addTaskRequest = new AddTaskRequest();
            addTaskRequest.Name = _name;

            string json = JsonConvert.SerializeObject(addTaskRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri((Urls.HOST + "/" + Urls.CREATE_TASK).Replace("{projectId}", ProjectId.ToString()));
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
                    Debug.WriteLine(responseBody);
                    TaskItem taskItem = JsonConvert.DeserializeObject<TaskItem>(parsedObject["data"].ToString());
                    Debug.WriteLine(taskItem.Name);
                    Tasks.Add(taskItem);
                    var projectPage = new ProjectPage(_tasks, _ProjectId);
                    await NavigationService.PushAsync(projectPage);
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public AddTaskViewModel( ObservableCollection<TaskItem> tasks, int projectId)
        {
            client = new HttpClient();
            OnClickAddTaskButton = new Command(onClickAddTaskButton);
            ProjectId = projectId;
            Tasks = tasks;
            
        }
    }
}