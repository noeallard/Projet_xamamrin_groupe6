using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using TimeTracker.Apps.Pages;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Accounts;
using TimeTracker.Dtos.Authentications.Credentials;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeTracker.Apps.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        HttpClient client;
        private String _email;
        private String _firstname;
        private String _lastname;
        private String _oldpassword;
        private String _newpassword;

        public String Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
            }
        }

        public String Firstname
        {
            get { return _firstname; }
            set
            {
                SetProperty(ref _firstname, value);
            }
        }
        public String Lastname
        {
            get { return _lastname; }
            set
            {
                SetProperty(ref _lastname, value);
            }
        }
        public String OldPassword
        {
            get { return _oldpassword; }
            set
            {
                SetProperty(ref _oldpassword, value);
            }
        }

        public String NewPassword
        {
            get { return _newpassword; }
            set
            {
                SetProperty(ref _newpassword, value);
            }
        }
        public async void onClickModifUserPasswordButton()
        {
            SetPasswordRequest setPasswordRequest = new SetPasswordRequest();
            setPasswordRequest.OldPassword = _oldpassword;
            setPasswordRequest.NewPassword = _newpassword;
            string json = JsonConvert.SerializeObject(setPasswordRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri(Urls.HOST + "/" + Urls.SET_PASSWORD);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                HttpResponseMessage response = await client.PatchAsync(uri, content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    Debug.WriteLine(response);
                    //await NavigationService.PushAsync<MainPage>();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public async void onClickModifUserButton()
        {
            SetUserProfileRequest setUserProfileRequest = new SetUserProfileRequest();
            setUserProfileRequest.Email = _email;
            setUserProfileRequest.FirstName = _firstname;
            setUserProfileRequest.LastName = _lastname;

            string json = JsonConvert.SerializeObject(setUserProfileRequest, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Uri uri = new Uri(Urls.HOST + "/" + Urls.SET_USER_PROFILE);
                if (!client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                }
                HttpResponseMessage response = await client.PatchAsync(uri, content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    Debug.WriteLine(response);
                    //await NavigationService.PushAsync<MainPage>();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public Command OnClickModifUserButton
        {
            get;
        }

        public Command OnClickModifUserPasswordButton
        {
            get;
        }
        public ProfileViewModel()
        {
            OnClickModifUserButton = new Command(onClickModifUserButton);
            OnClickModifUserPasswordButton = new Command(onClickModifUserPasswordButton);
            client = new HttpClient();
        }
    }
}