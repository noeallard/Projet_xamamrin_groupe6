using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Diagnostics;
using System.Net.Http;
using TimeTracker.Dtos;
using Xamarin.Essentials;

namespace TimeTracker.Apps.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        HttpClient client;
        public async void loadTask()
        {
            try
            {
                Uri uri = new Uri(Urls.HOST + "/" + Urls.USER_PROFILE);
                if (!client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                }
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public TaskViewModel()
        {
            
            client = new HttpClient();
            loadTask();
        }
    }
}