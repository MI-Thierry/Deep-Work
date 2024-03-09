﻿using System;
using System.Xml.Serialization;

namespace DeepWork.Models
{
    [Serializable]
    public class ShortTask
    {
        [XmlAttribute]
        public TimeSpan Duration { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public DateTimeOffset FinishDate { get; set; }
    }
}
