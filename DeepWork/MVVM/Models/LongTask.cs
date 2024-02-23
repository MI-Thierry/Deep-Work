using System;
using System.Collections.Generic;

namespace DeepWork.MVVM.Models
{
    [Serializable]
    public class LongTask
    {
        public string Name { get; set; }
        public uint MaxDuration { get; set; }
        public uint MaxShortTaskCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<ShortTask> Tasks { get; set; }

        public void AddShortTask(ShortTask task)
        {
            Tasks.Add(task);
        }
    }
}
