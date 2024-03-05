using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepWork.Models
{
    public enum TaskType
    {
        None,
        ShortTask,
        LongTask
    }
    public class TaskHistory
    {
        public TaskType Type { get; set; }
        public DateTime FinishDate { get; set; }
        public string Name { get; set; }
        public List<TaskHistory> Childrens { get; set; } = new List<TaskHistory>();
    }
}
