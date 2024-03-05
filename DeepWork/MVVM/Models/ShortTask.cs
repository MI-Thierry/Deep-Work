using System;
using System.Xml.Serialization;

namespace DeepWork.MVVM.Models
{
    [Serializable]
	public class ShortTask
	{
        [XmlAttribute]
        public TimeSpan Duration { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public DateTime FinishDate { get; set; }
    }
}
