using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using TimeTracker.Apps.Models;
using TimeTracker.Apps.Pages;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Projects;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeTracker.Apps.ViewModels
{
    public class EditProjectViewModel : ViewModelBase
    {
        private Project _project;
        HttpClient client;
        private String _name;
        private String _description;

        public String Name {
            get {return _name;}
            set { SetProperty(ref _name, value); }
        }

        public String Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public Command OnClickSetProjectButton { get;  }

        public async void onClickSetProject() 
        {
            AddProjectRequest addProjectRequest = new AddProjectRequest();
            addProjectRequest.Name = _name;
            addProjectRequest.Description = _description;

            string json = JsonConvert.SerializeObject(addProjectRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri((Urls.HOST + "/" + Urls.UPDATE_PROJECT).Replace("{projectId}", _project.Id.ToString()));
                if (!client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                }
                HttpResponseMessage response = await client.PutAsync(uri, content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    Debug.WriteLine(response);
                    await NavigationService.PushAsync<MainPage>();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public EditProjectViewModel(Project project)
        {
            _project = project;
            _name = project.Name;
            _description = project.Description;
            client = new HttpClient();
            OnClickSetProjectButton = new Command(onClickSetProject);
            
         
        }
    }
}