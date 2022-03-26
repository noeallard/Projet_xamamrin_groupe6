using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Apps.Models
{
    public class Project
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
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int TotalSecond
        {
            get { return _totalSecond; }
            set { _totalSecond = value; }
        }
    }
}
