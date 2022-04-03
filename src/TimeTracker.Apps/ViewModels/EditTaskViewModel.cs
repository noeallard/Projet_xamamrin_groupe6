using Newtonsoft.Json;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Projects;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using TimeTracker.Apps.Pages;
using TimeTracker.Apps.Models;

namespace TimeTracker.Apps.ViewModels
{
    public class EditTaskViewModel : ViewModelBase
    {
        private string _name;
        private Project _project;
        private TaskItem _task;

        HttpClient client;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Project Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }

        public TaskItem Task
        {
            get => _task;
            set => SetProperty(ref _task, value);
        }

        public async void onClickEditButton()
        {
            AddTaskRequest addTaskRequest = new AddTaskRequest();
            addTaskRequest.Name = Name;

            string json = JsonConvert.SerializeObject(addTaskRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri((Urls.HOST + "/" + Urls.UPDATE_TASK).Replace("{projectId}", _project.Id.ToString()).Replace("{taskId}", _task.Id.ToString()));
                if (!client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                }
                HttpResponseMessage response = await client.PutAsync(uri, content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("DANS LE SUCCSEIJSUDSD ");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    Debug.WriteLine(response);
                    _task.Name = Name;
                    await NavigationService.PushAsync<MainPage>();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("dans le catch HHHHHHHHHH");
                Debug.WriteLine(ex.Message);
            }

        }
        public Command OnClickEditButton
        {
            get;
        }
        public EditTaskViewModel(TaskItem task, Project project)
        {
            _project = project;
            _task = task;
            client = new HttpClient();
            OnClickEditButton = new Command(onClickEditButton);
        }
    }
}
