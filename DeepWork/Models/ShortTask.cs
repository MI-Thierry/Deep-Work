using System;
using System.ComponentModel.DataAnnotations;

namespace DeepWork.Models
{
	public class ShortTask
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public TimeSpan Duration { get; set; }

		public DateTimeOffset FinishDate { get; set; }
	}
}
