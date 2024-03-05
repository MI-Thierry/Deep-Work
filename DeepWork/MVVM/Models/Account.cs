using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace DeepWork.MVVM.Models
{
    [Serializable]
    public class Account
    {
        [XmlAttribute]
        public string Username { get; set; }

        [XmlAttribute]
        public string Password { get; set; }

        [XmlArray]
        public ObservableCollection<LongTask> LongTasks { get; set; } = new();
    }
}
