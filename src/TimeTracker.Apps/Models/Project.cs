using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TimeTracker.Apps.ViewModels;
using Xamarin.Forms;

namespace TimeTracker.Apps.Models
{
    public class Project : NotifierBase
    {
        private int _id;
        private string _name;
        private string _description;
        private int _totalSecond;
        public Project(int id, string name, string description, int totalSecond)
        {
            _id = id;
            _name = name;
            _description = description;
            _totalSecond = totalSecond;
        }
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public int TotalSecond
        {
            get { return _totalSecond; }
            set { _totalSecond = value; }
        }

        public Command OnClickDelete
        {
            get;
            set;
        }

    }
}
