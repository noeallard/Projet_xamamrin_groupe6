using System;
using Newtonsoft.Json;

namespace TimeTracker.Dtos.Projects
{
    public class TimeItem
    {
        public TimeItem(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }
        
        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }

        public TimeSpan Difference { get; set; }
        
    }
}