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

		public void ChangeTheme(ElementTheme theme)
		{
			ActiveAccount.Theme = theme;
			_accountContext.SaveChanges();
		}

		public LongTask AddLongTask(LongTask task)
		{
			if (!IsAccountAvailable)
				return task;

			ActiveAccount.LongTasks.Add(task);
			_accountContext.SaveChanges();
			return task;
		}

		public void EditLongTask(int taskId, LongTask editedTask)
		{
			LongTask task = GetLongTaskById(taskId);
			task.Name = editedTask.Name;
			task.StartDate = editedTask.StartDate;
			task.EndDate = editedTask.EndDate;
			_accountContext.SaveChanges();
		}

		public void FinishLongTask(int id)
		{
			DeleteLongTask(id);
		}

		public void DeleteLongTask(int id)
		{
			if (!IsAccountAvailable)
				return;

			LongTask taskToRemove = ActiveAccount.LongTasks.FirstOrDefault(item => item.Id == id);
			foreach (var shortTask in taskToRemove.RunningTasks)
				_accountContext.RunningTasks.Remove(shortTask);
			foreach (var shortTask in taskToRemove.FinishedTasks)
				_accountContext.FinishedTasks.Remove(shortTask);
			ActiveAccount.LongTasks.Remove(taskToRemove);
			_accountContext.LongTasks.Remove(taskToRemove);
			_accountContext.SaveChanges();
		}
		
		public ShortTask AddShortTask(int parentId, ShortTask task)
		{
			if (!IsAccountAvailable)
				return task;

			LongTask parentTask = ActiveAccount.LongTasks.First(item => item.Id == parentId);
			parentTask.RunningTasks.Add(task);
			_accountContext.SaveChanges();

			return task;
		}

		public void EditShortTask(int parentId, int taskToEditId, ShortTask editedTask)
		{
			ShortTask task = GetShortTaskById(parentId, taskToEditId);
			task.Name = editedTask.Name;
			task.Duration = editedTask.Duration;
			_accountContext.SaveChanges();
		}

		public void FinishShortTask(int parentId, int taskId)
		{
			if (!IsAccountAvailable)
				return;

			LongTask parentTask = ActiveAccount.LongTasks.First(item => item.Id == parentId);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Id == taskId);
			if (parentTask.MaxDuration < task.Duration)
				parentTask.MaxDuration = task.Duration;
			task.FinishDate = DateTime.Now;

			parentTask.RunningTasks.Remove(task);
			parentTask.FinishedTasks.Add(task);

			_accountContext.SaveChanges();
		}

		public void DeleteShortTask(int parentId, int taskId)
		{
			if (!IsAccountAvailable)
				return;

			LongTask parentTask = ActiveAccount.LongTasks.First(item => item.Id == parentId);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Id == taskId);

			parentTask.RunningTasks.Remove(task);

			_accountContext.RunningTasks.Remove(task);
			_accountContext.SaveChanges();
		}

		public LongTask GetLongTaskById(int id) =>
			ActiveAccount.LongTasks.First(task => task.Id == id);

		public ShortTask GetShortTaskById(int parentId, int childId)
		{
			LongTask parentTask = ActiveAccount.LongTasks.First(item => item.Id == parentId);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Id == childId);
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
