using System;
using System.Collections.Generic;

namespace DeepWork.MVVM.Models
{
    [Serializable]
    public class Account
    {
        public string AccountName { get; set; }
        public List<LongTask> LongTask { get; set; }
    }
}
