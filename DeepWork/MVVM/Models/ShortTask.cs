using System;

namespace DeepWork.MVVM.Models
{
    [Serializable]
	public class ShortTask
	{
        public int Duration { get; set; }
        public string Name { get; set; }
    }
}
