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
        private string _email;
        private string _password;
        private string _lastname;
        private string _firstname;

        public async void onClick()
        {
            CreateUserRequest registerRequest = new CreateUserRequest();
            registerRequest.ClientId = "MOBILE";
            registerRequest.ClientSecret = "COURS";
            registerRequest.Email = Email;
            registerRequest.Password = Password;
            registerRequest.FirstName = FirstName;
            registerRequest.LastName = LastName;
            
            if(Password != null)
            {
                if (Password.Length > 5)
                {
                    if (Email != null && FirstName != null && LastName != null)
                    {
                        string json = JsonConvert.SerializeObject(registerRequest, Formatting.Indented);
                        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                        try
                        {
                            Debug.WriteLine(Urls.HOST + "/" + Urls.CREATE_USER);
                            Uri uri = new Uri(Urls.HOST + "/" + Urls.CREATE_USER);
                            Debug.WriteLine(uri.ToString());
                            HttpResponseMessage response = await client.PostAsync(uri, content);
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine(responseBody);
                            await NavigationService.PushAsync<ConnectionPage>();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            await Application.Current.MainPage.DisplayAlert("Erreur", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erreur", "Les champs ne doivent pas être vide", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Votre mot de passe doit comporter au moins 6 caractères", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Veuillez rentrer un mot de passe", "OK");
            }
            
            
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Password { 
            get { return _password; } 
            set { _password = value; }
        }

        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        public string LastName
        {
            get { return _lastname; }
            set { _lastname = value; }
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