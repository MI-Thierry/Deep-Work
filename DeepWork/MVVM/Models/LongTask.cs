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
        public uint MaxDuration { get; set; }

        [XmlAttribute]
        public uint MaxShortTaskCount { get; set; }

        [XmlAttribute]
        public DateTime StartTime { get; set; }

        [XmlAttribute]
        public DateTime EndTime { get; set; }

        [XmlArray]
        public ObservableCollection<ShortTask> Tasks { get; set; }
    }
}
