using Newtonsoft.Json;
using Storm.Mvvm;
using Storm.Mvvm.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using TimeTracker.Apps.Pages;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Accounts;
using Xamarin.Forms;

namespace TimeTracker.Apps.ViewModels
{
    internal class RegisterViewModel : ViewModelBase
    {

        HttpClient client;

        public async void onClick()
        {
            CreateUserRequest registerRequest = new CreateUserRequest();
            registerRequest.ClientId = "MOBILE";
            registerRequest.ClientSecret = "COURS";

            registerRequest.Email = "test1@gmail.com";
            registerRequest.Password = "password";
            registerRequest.FirstName = "test1";
            registerRequest.LastName = "test1";

            string json = JsonConvert.SerializeObject(registerRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            

            try
            {
                Debug.WriteLine(Urls.HOST+"/"+Urls.CREATE_USER);
                Uri uri = new Uri(Urls.HOST + "/" + Urls.CREATE_USER);
                Debug.WriteLine(uri.ToString());
                HttpResponseMessage response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            


            /*await NavigationService.PushAsync<MainPage>();*/
        }
        public Command OnClick
        {
            get;
        }
        public RegisterViewModel()
        {
            OnClick = new Command(onClick);
            client = new HttpClient();
        }
    }
}