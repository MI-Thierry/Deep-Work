using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeepWork.Models
{
    [Serializable]
    public class Account
    {
        [XmlAttribute]
        public string Username { get; set; }

        [XmlAttribute]
        public string Password { get; set; }

        [XmlElement]
        ApplicationTheme ApplicationTheme { get; set; }

        [XmlArray]
        public List<LongTask> LongTasks { get; set; } = new();
    }
}
