using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeepWork.Models
{
	public class LongTask
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public TimeSpan MaxDuration { get; set; }

		public uint MaxShortTaskCount { get; set; }

		public DateTimeOffset StartDate { get; set; }

		public DateTimeOffset EndDate { get; set; }

		public List<ShortTask> RunningTasks { get; set; } = [];

		public List<ShortTask> FinishedTasks { get; set; } = [];
	}
}
