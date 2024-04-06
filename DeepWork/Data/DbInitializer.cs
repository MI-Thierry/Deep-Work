using DeepWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepWork.Data;

public static class DbInitializer
{
	public static void DbInitialize(this AccountContext context)
	{
		if (context.Accounts.Any())
			return;

		List<ShortTask> shortTasks =
		[
			new(){Name = "First Short Task", Duration = TimeSpan.FromHours(2.5), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
			new(){Name = "Second Short Task", Duration = TimeSpan.FromHours(1.2), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(6)},
			new(){Name = "Third Short Task", Duration = TimeSpan.FromHours(0.7), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(5)},
			new(){Name = "Forth Short Task", Duration = TimeSpan.FromHours(2.3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(4)},
			new(){Name = "Fifth Short Task", Duration = TimeSpan.FromHours(1.1), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(3)},
			new(){Name = "Sixth Short Task", Duration = TimeSpan.FromHours(1.3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(2)},
			new(){Name = "Seventh Short Task", Duration = TimeSpan.FromHours(1.5), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(1)},
			new(){Name = "Eighth Short Task", Duration = TimeSpan.FromHours(0.4), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
		];

		List<ShortTask> shortTasks1 =
		[
			new(){Name = "First Short Task", Duration = TimeSpan.FromHours(2), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
			new(){Name = "Second Short Task", Duration = TimeSpan.FromHours(1.3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(6)},
			new(){Name = "Third Short Task", Duration = TimeSpan.FromHours(1.7), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(5)},
			new(){Name = "Forth Short Task", Duration = TimeSpan.FromHours(3.2), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(4)},
			new(){Name = "Fifth Short Task", Duration = TimeSpan.FromHours(0.7), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(3)},
			new(){Name = "Sixth Short Task", Duration = TimeSpan.FromHours(0.5), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(2)},
			new(){Name = "Seventh Short Task", Duration = TimeSpan.FromHours(1.7), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(1)},
			new(){Name = "Eighth Short Task", Duration = TimeSpan.FromHours(1.23), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
		];

		List<ShortTask> shortTasks2 =
		[
			new(){Name = "First Short Task", Duration = TimeSpan.FromHours(3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
			new(){Name = "Second Short Task", Duration = TimeSpan.FromHours(2.2), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(6)},
			new(){Name = "Third Short Task", Duration = TimeSpan.FromHours(0.5), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(5)},
			new(){Name = "Forth Short Task", Duration = TimeSpan.FromHours(3.3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(4)},
			new(){Name = "Fifth Short Task", Duration = TimeSpan.FromHours(1.3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(3)},
			new(){Name = "Sixth Short Task", Duration = TimeSpan.FromHours(2.3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(2)},
			new(){Name = "Seventh Short Task", Duration = TimeSpan.FromHours(1.7), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(1)},
			new(){Name = "Eighth Short Task", Duration = TimeSpan.FromHours(1.23), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
		];

		List<ShortTask> shortTasks3 =
		[
			new(){Name = "First Short Task", Duration = TimeSpan.FromHours(1), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
			new(){Name = "Second Short Task", Duration = TimeSpan.FromHours(0.2), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(6)},
			new(){Name = "Third Short Task", Duration = TimeSpan.FromHours(3.7), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(5)},
			new(){Name = "Forth Short Task", Duration = TimeSpan.FromHours(2), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(4)},
			new(){Name = "Fifth Short Task", Duration = TimeSpan.FromHours(2.5), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(3)},
			new(){Name = "Sixth Short Task", Duration = TimeSpan.FromHours(1.3), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(2)},
			new(){Name = "Seventh Short Task", Duration = TimeSpan.FromHours(1.7), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(1)},
			new(){Name = "Eighth Short Task", Duration = TimeSpan.FromHours(1.23), FinishDate = DateTimeOffset.Now - TimeSpan.FromDays(7)},
		];

		List<LongTask> longTasks =
		[
			new LongTask
			{
				Name = "First Long Task",
				StartDate = DateTimeOffset.Now - TimeSpan.FromDays(20),
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(20),
				MaxDuration = TimeSpan.FromHours(3),
				FinishedTasks = shortTasks,
			},
			new LongTask
			{
				Name = "Second Long Task",
				StartDate = DateTimeOffset.Now - TimeSpan.FromDays(15),
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(15),
				MaxDuration = TimeSpan.FromHours(2),
				FinishedTasks = shortTasks1,
			},
			new LongTask
			{
				Name = "Third Long Task",
				StartDate = DateTimeOffset.Now - TimeSpan.FromDays(20),
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(20),
				MaxDuration = TimeSpan.FromHours(1),
				FinishedTasks = shortTasks2,
			},
			new LongTask
			{
				Name = "Fourth Long Task",
				StartDate = DateTimeOffset.Now - TimeSpan.FromDays(15),
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(15),
				MaxDuration = TimeSpan.FromHours(4),
				FinishedTasks = shortTasks3,
			},
		];

		List<LongTask> longTasks1 =
		[
			new LongTask
			{
				Name = "First Long Task",
				StartDate = DateTimeOffset.Now - TimeSpan.FromDays(20),
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(20),
				MaxDuration = TimeSpan.FromHours(1),
				FinishedTasks = shortTasks2,
			},
			new LongTask
			{
				Name = "Second Long Task",
				StartDate = DateTimeOffset.Now - TimeSpan.FromDays(15),
				EndDate = DateTimeOffset.Now + TimeSpan.FromDays(15),
				MaxDuration = TimeSpan.FromHours(4),
				FinishedTasks = shortTasks3,
			},
		];
		Account account = new()
		{
			Username = "Muhirwa I. Thierry",
			Password = "P9/UR5dSnMQRpqBTqnGpc2Ca8fI=",
			IsActive = true,
			Theme = Microsoft.UI.Xaml.ElementTheme.Default,
			DailyTarget = TimeSpan.FromHours(2),
			RunningLongTasks = longTasks,
			//FinishedLongTasks = longTasks1,
		};

		context.Accounts.Add(account);
		context.SaveChanges();
	}
}
