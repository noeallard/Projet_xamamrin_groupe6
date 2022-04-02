using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using TimeTracker.Apps.Models;
using TimeTracker.Apps.Pages;
using TimeTracker.Dtos;
using TimeTracker.Dtos.Authentications.Credentials;
using TimeTracker.Dtos.Projects;
using Xamarin.Essentials;
using Xamarin.Forms;
using Microcharts;
using Entry = Microcharts.ChartEntry;
using SkiaSharp;

namespace TimeTracker.Apps.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        HttpClient client;
        private ObservableCollection<Project> _projects;
        private List<Entry> _entries;
        private PieChart _chart;

        public PieChart Chart
        {
            get => _chart;
            set => SetProperty(ref _chart, value);
        }
        public List<Entry> Entries
        {
            get => _entries;
            set => SetProperty(ref _entries, value);
        }
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }
        public MainViewModel()
        {
            OnClickProfileButton = new Command(onClickProfileButton);
            OnClickAddButton = new Command(onClickAddButton);
            client = new HttpClient();
            _projects = new ObservableCollection<Project>();
            _entries = new List<Entry>();
            loadListProject();
        }

        public async void loadListProject()
        {
            try
            {
                Uri uri = new Uri(Urls.HOST + "/" + Urls.LIST_PROJECTS);
                if (!client.DefaultRequestHeaders.Contains("Authorization"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                }
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("AAAAAGJHGHJBHKDSHDSDJS");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    ObservableCollection<Project> projets = JsonConvert.DeserializeObject<ObservableCollection<Project>>(parsedObject["data"].ToString());
                    for (int i = 0; i < projets.Count; i++)
                    {
                        projets[i].OnClickDelete = new Command<Project>(DeleteProject);
                        projets[i].OnClickTask = new Command<Project>(GoToProjectPage);
                        _projects.Add(projets[i]);
                        Random r = new Random();
                        Color color = Color.FromRgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
                        _entries.Add(new Entry(projets[i].TotalSecond + i * 2)
                        {
                            Color = new SKColor(((byte)r.Next(0, 256)), (byte)r.Next(0, 256), (byte)r.Next(0, 256)),
                            Label = projets[i].Name,
                            ValueLabel = projets[i].TotalSecond.ToString()
                        }); ; ; ; 
                    }
                    PieChart donut = new PieChart();
                    donut.Entries = _entries;
                    Chart = donut;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public Command OnClickAddButton
        {
            get;
        }

        public async void onClickAddButton()
        {
            await NavigationService.PushAsync<AddProject>();
        }

        public async void DeleteProject(Project project)
        {
            try
            {
                client = new HttpClient();
                Uri uri = new Uri((Urls.HOST + "/" + Urls.DELETE_PROJECT).Replace("{projectId}",project.Id.ToString()));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                HttpResponseMessage response = await client.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Project deleted");
                    int index = Projects.IndexOf(project);
                    Projects.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public async void GoToProjectPage(Project project)
        {
            try
            {
                client = new HttpClient();
                Uri uri = new Uri((Urls.HOST + "/" + Urls.LIST_TASKS).Replace("{projectId}", project.Id.ToString()));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("access_token", "undefiend"));
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var parsedObject = JObject.Parse(responseBody);
                    ObservableCollection<TaskItem> tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(parsedObject["data"].ToString());
                    var projectPage = new ProjectPage(tasks,project.Id);
               
                    await NavigationService.PushAsync(projectPage);


                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public Command OnClickProfileButton
        {
            get;
        }

        public async void onClickProfileButton()
        {
            await NavigationService.PushAsync<ProfilePage>();
        }
    }
}