using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using TimeTracker.Apps.Models;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Authentications.Credentials;

namespace TimeTracker.Apps.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        HttpClient client;
        List<Project> projects;
        public MainViewModel()
        {
            client = new HttpClient();
        }

        public async void loadListProject()
        {
            try
            {
                Uri uri = new Uri(Urls.HOST + "/" + Urls.LIST_PROJECTS);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer e4948d5a65f94b46acfb3eafa311c8c6");
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("AAAAAGJHGHJBHKDSHDSDJS");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    List<Project> loginResponse = JsonConvert.DeserializeObject<List<Project>>(parsedObject["data"].ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}