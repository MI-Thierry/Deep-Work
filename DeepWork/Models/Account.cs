using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeepWork.Models
{
	public class Account
	{
		public int Id { get; set; }

		[Required]
		public string Username { get; set; }

		public string Password { get; set; }

		public bool IsActive { get; set; }

		public ElementTheme Theme { get; set; }

		public TimeSpan DailyTarget { get; set; }

		public DateTimeOffset LastUpdated { get; set; }

		public TimeSpan CompletedDailyTarget { get; set; }

		public List<LongTask> RunningLongTasks { get; set; } = [];

		public List<LongTask> FinishedLongTasks { get; set; } = [];
	}
}
