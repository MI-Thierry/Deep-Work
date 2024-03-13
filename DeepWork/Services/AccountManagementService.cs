using DeepWork.Data;
using DeepWork.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepWork.Services
{
	public class AccountManagementService
	{
		private readonly AccountContext _accountContext;

		public List<Account> AvailableAccounts { get; private set; }
		public Account ActiveAccount { get; private set; }
		public bool IsAccountAvailable { get; private set; }

		public AccountManagementService(AccountContext context)
		{
			_accountContext = context;
			context.Database.EnsureCreated();

			if (context.Accounts.Any())
			{
				AvailableAccounts = [.. _accountContext.Accounts];
				ActiveAccount = context.Accounts
					.Include(p => p.LongTasks).ThenInclude(p => p.RunningTasks)
					.Include(p => p.LongTasks).ThenInclude(p => p.FinishedTasks)
					.Single(account => account.IsActive);

				IsAccountAvailable = true;
			}
			else
			{
				ActiveAccount = null;
				IsAccountAvailable = false;
			}
		}

		public Account CreateAccount(string username, string password)
		{
			Account account = new()
			{
				Username = username,
				Password = password,
				Theme = ElementTheme.Default
			};
			_accountContext.Accounts.Add(account);
			IsAccountAvailable = true;
			_accountContext.SaveChanges();

			return account;
		}

		public void ActivateAccount(Account account)
		{
			if (ActiveAccount != null)
				ActiveAccount.IsActive = false;

			ActiveAccount = _accountContext.Accounts
				.Include(p => p.LongTasks).ThenInclude(p => p.RunningTasks)
				.Include(p => p.LongTasks).ThenInclude(p => p.FinishedTasks)
				.Single(acc => acc.Username == account.Username);
			ActiveAccount.IsActive = true;
			_accountContext.SaveChanges();
		}

		public void AddLongTask(LongTask task)
		{
			if (!IsAccountAvailable)
				return;

			ActiveAccount.LongTasks.Add(task);
			_accountContext.SaveChanges();
		}

		public void AddShortTask(string parentName, ShortTask task)
		{
			if (!IsAccountAvailable)
				return;
			LongTask parentTask = ActiveAccount.LongTasks.First(item => item.Name == parentName);
			parentTask.RunningTasks.Add(task);
			_accountContext.SaveChanges();
		}
		public void FinishLongTask(string name)
		{
			if (!IsAccountAvailable)
				return;

			ActiveAccount.LongTasks.Remove(ActiveAccount.LongTasks.FirstOrDefault(item => item.Name == name));
			_accountContext.SaveChanges();
		}

		public void FinishShortTask(string parentName, string taskName)
		{
			if (!IsAccountAvailable)
				return;

			LongTask parentTask = ActiveAccount.LongTasks.First(item => item.Name == parentName);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Name == taskName);
			if (parentTask.MaxDuration < task.Duration)
				parentTask.MaxDuration = task.Duration;
			task.FinishDate = DateTime.Now;

			parentTask.RunningTasks.Remove(task);
			parentTask.FinishedTasks.Add(task);

			_accountContext.SaveChanges();
		}

		public void EditLongTask(string taskName, LongTask editedTask)
		{
			LongTask task = GetLongTaskByName(taskName);
			task.Name = editedTask.Name;
			task.StartDate = editedTask.StartDate;
			task.EndDate = editedTask.EndDate;
			_accountContext.SaveChanges();
		}

		public void EditShortTask(string parentTask, string taskToEdit, ShortTask editedTask)
		{
			ShortTask task = GetShortTaskByName(parentTask, taskToEdit);
			task.Name = editedTask.Name;
			task.Duration = editedTask.Duration;
			_accountContext.SaveChanges();
		}

		public LongTask GetLongTaskByName(string name) =>
			ActiveAccount.LongTasks.First(task => task.Name == name);

		public ShortTask GetShortTaskByName(string parentName, string name)
		{
			LongTask parentTask = ActiveAccount.LongTasks.First(item => item.Name == parentName);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Name == name);
			return task;
		}

		public List<TaskHistory> GetAccountHistory()
		{
			List<TaskHistory> tasksHistoryTree = [];
			foreach (var longTask in ActiveAccount.LongTasks)
			{
				TaskHistory task = new()
				{
					Type = TaskType.LongTask,
					Name = longTask.Name,
				};
				foreach (var shortTask in longTask.FinishedTasks)
				{
					task.Childrens.Add(new TaskHistory
					{
						Name = shortTask.Name,
						FinishDate = shortTask.FinishDate,
						Type = TaskType.ShortTask
					});
				}
				tasksHistoryTree.Add(task);
			}

			return tasksHistoryTree;
		}
	}
}
