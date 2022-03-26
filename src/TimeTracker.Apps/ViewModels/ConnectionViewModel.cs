using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using TimeTracker.Apps.Pages;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Authentications;
using TimeTracker.Dtos.Authentications.Credentials;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeTracker.Apps.ViewModels
{
    public class ConnectionViewModel : ViewModelBase
    {

        HttpClient client;
        private String _login;
        private String _password;

        public String Login
        {
            get { return _login; }
            set
            {
                SetProperty(ref _login, value);
            }
        }

        public String Password 
        { 
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
            }
        }




        public async void onClickRegisterButton()
        {
            await NavigationService.PushAsync<RegisterPage>();
        }

        public Command OnClickRegisterButton
        {
            get;
        }

        public async void onClickConnectionButton()
        {
            LoginWithCredentialsRequest loginRequest = new LoginWithCredentialsRequest();
            loginRequest.ClientId = "MOBILE";
            loginRequest.ClientSecret = "COURS";
            loginRequest.Login = _login;
            loginRequest.Password = _password;

            string json = JsonConvert.SerializeObject(loginRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri(Urls.HOST + "/" + Urls.LOGIN);
                HttpResponseMessage response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(parsedObject["data"].ToString());

                    Preferences.Set("access_token", loginResponse.AccessToken);
                    Preferences.Set("refresh_token", loginResponse.RefreshToken);
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

        public ConnectionViewModel()
        {
            OnClickRegisterButton = new Command(onClickRegisterButton);
            OnClickConnectionButton = new Command(onClickConnectionButton);
            client = new HttpClient();
        }
    }
}