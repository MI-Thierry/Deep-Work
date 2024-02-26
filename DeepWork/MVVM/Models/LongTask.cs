using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace DeepWork.MVVM.Models
{
    [Serializable]
    public class LongTask
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public TimeSpan MaxDuration { get; set; }

        [XmlAttribute]
        public uint MaxShortTaskCount { get; set; }

        [XmlAttribute]
        public DateTime StartDate { get; set; }

        [XmlAttribute]
        public DateTime EndDate { get; set; }

        [XmlArray]
        public ObservableCollection<ShortTask> RunningTasks { get; set; } = new();

        [XmlArray]
        public ObservableCollection<ShortTask> FinishedTasks { get; set; } = new();
    }
}
