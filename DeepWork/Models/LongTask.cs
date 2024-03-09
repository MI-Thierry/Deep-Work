using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeepWork.Models
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
        public DateTimeOffset StartDate { get; set; }

        [XmlAttribute]
        public DateTimeOffset EndDate { get; set; }

        [XmlArray]
        public List<ShortTask> RunningTasks { get; set; } = new();

        [XmlArray]
        public List<ShortTask> FinishedTasks { get; set; } = new();
    }
}
