using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using TimeTracker.Apps.Pages;
using TimeTracker.Dtos;
using Xamarin.Forms;
using TimeTracker.Dtos.Projects;
using Storm.Mvvm;
using Xamarin.Essentials;

namespace TimeTracker.Apps.ViewModels
{
    public class AddProjectViewModel : ViewModelBase
    {
        HttpClient client;
        private String _name;
        private String _description;

        public String Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public String Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
            }
        }

        public Command OnClickAddProjectButton
        {
            get;
        }

        public async void onClickAddProjectButton()
        {
            AddProjectRequest addProjectRequest = new AddProjectRequest();
            addProjectRequest.Name = _name;
            addProjectRequest.Description = _description;

            string json = JsonConvert.SerializeObject(addProjectRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri(Urls.HOST + "/" + Urls.ADD_PROJECT);
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
                    Debug.WriteLine(response);
                    await NavigationService.PushAsync<MainPage>();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public Command OnClickConnectionButton
        {
            get;
        }

        public AddProjectViewModel()
        {
            OnClickAddProjectButton = new Command(onClickAddProjectButton);
            client = new HttpClient();
        }
    }
}
