using DeepWork.Data;
using DeepWork.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DeepWork.Services
{
	public class AccountManagementService
	{
		private readonly AccountContext _accountContext;

		public List<Account> AvailableAccounts { get; private set; }
		public Account ActiveAccount { get; private set; }
		public bool IsAccountAvailable { get; private set; }

		public event Action<Account> ActiveAccountChanged;

		public AccountManagementService(AccountContext context)
		{
			_accountContext = context;
			context.Database.EnsureCreated();

			// Todo: Remove this in production
			// _accountContext.DbInitialize();
			AvailableAccounts = [.. _accountContext.Accounts];

			if (AvailableAccounts.Count != 0
				&& AvailableAccounts.SingleOrDefault(account => account.IsActive) != null)
			{
				AvailableAccounts = [.. _accountContext.Accounts];
				ActiveAccount = context.Accounts
					.Include(p => p.RunningLongTasks).ThenInclude(p => p.RunningTasks)
					.Include(p => p.RunningLongTasks).ThenInclude(p => p.FinishedTasks)
					.Single(account => account.IsActive);

				if (ActiveAccount.LastUpdated != DateTimeOffset.Now)
				{
					ActiveAccount.LastUpdated = DateTimeOffset.Now;
					ActiveAccount.CompletedDailyTarget = TimeSpan.Zero;
					_accountContext.SaveChanges();
				}

				IsAccountAvailable = true;
				ActiveAccountChanged?.Invoke(ActiveAccount);
			}
			else
			{
				ActiveAccount = null;
				IsAccountAvailable = false;
			}
		}

		public Account CreateAccount(string username, string password)
		{
			byte[] computedPassword = SHA1.HashData(Encoding.Default.GetBytes(password));

			Account account = new()
			{
				Username = username,
				Password = Convert.ToBase64String(computedPassword),
				Theme = ElementTheme.Default
			};
			_accountContext.Accounts.Add(account);
			IsAccountAvailable = true;
			_accountContext.SaveChanges();
			AvailableAccounts = [.. _accountContext.Accounts];

			return account;
		}

		public Account SignInAccount(int accountId, string password)
		{
			byte[] computedPassword = SHA1.HashData(Encoding.Default.GetBytes(password));
			Account account = AvailableAccounts.First(acc => acc.Id == accountId);

			if (account.Password != Convert.ToBase64String(computedPassword))
				return null;

			if (ActiveAccount != null)
				ActiveAccount.IsActive = false;

			ActiveAccount = _accountContext.Accounts
				.Include(p => p.RunningLongTasks).ThenInclude(p => p.RunningTasks)
				.Include(p => p.RunningLongTasks).ThenInclude(p => p.FinishedTasks)
				.Single(acc => acc.Username == account.Username);
			ActiveAccount.IsActive = true;

			if (ActiveAccount.LastUpdated != DateTimeOffset.Now)
			{
				ActiveAccount.LastUpdated = DateTimeOffset.Now;
				ActiveAccount.CompletedDailyTarget = TimeSpan.Zero;
			}
			ActiveAccountChanged?.Invoke(ActiveAccount);

			_accountContext.SaveChanges();
			return ActiveAccount;
		}

		public void SignOutAccount()
		{
			ActiveAccount.IsActive = false;
			ActiveAccount = null;
			_accountContext.SaveChanges();
		}

		public void ModifyAccount(string username, string password)
		{
			byte[] computedPassword = SHA1.HashData(Encoding.Default.GetBytes(password));
			ActiveAccount.Username = username;
			ActiveAccount.Password = Convert.ToBase64String(computedPassword);
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

			ActiveAccount.RunningLongTasks.Add(task);
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
			LongTask taskToRemove = ActiveAccount.RunningLongTasks.FirstOrDefault(item => item.Id == id);

			foreach (var task in taskToRemove.RunningTasks)
				taskToRemove.FinishedTasks.Add(task);
			taskToRemove.RunningTasks.Clear();

			ActiveAccount.RunningLongTasks.Remove(taskToRemove);
			ActiveAccount.FinishedLongTasks.Add(taskToRemove);

			_accountContext.SaveChanges();
		}

		public void DeleteLongTask(int id)
		{
			if (!IsAccountAvailable)
				return;

			LongTask taskToRemove = ActiveAccount.RunningLongTasks.FirstOrDefault(item => item.Id == id);
			foreach (var shortTask in taskToRemove.RunningTasks)
				_accountContext.RunningTasks.Remove(shortTask);
			foreach (var shortTask in taskToRemove.FinishedTasks)
				_accountContext.FinishedTasks.Remove(shortTask);
			ActiveAccount.RunningLongTasks.Remove(taskToRemove);
			_accountContext.RunningLongTasks.Remove(taskToRemove);
			_accountContext.SaveChanges();
		}
		
		public ShortTask AddShortTask(int parentId, ShortTask task)
		{
			if (!IsAccountAvailable)
				return task;

			LongTask parentTask = ActiveAccount.RunningLongTasks.First(item => item.Id == parentId);
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

			LongTask parentTask = ActiveAccount.RunningLongTasks.First(item => item.Id == parentId);
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

			LongTask parentTask = ActiveAccount.RunningLongTasks.First(item => item.Id == parentId);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Id == taskId);

			parentTask.RunningTasks.Remove(task);

			_accountContext.RunningTasks.Remove(task);
			_accountContext.SaveChanges();
		}

		public LongTask GetLongTaskById(int id) =>
			ActiveAccount.RunningLongTasks.First(task => task.Id == id);

		public ShortTask GetShortTaskById(int parentId, int childId)
		{
			LongTask parentTask = ActiveAccount.RunningLongTasks.First(item => item.Id == parentId);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Id == childId);
			return task;
		}

		public void SetAccountDailyTarget(TimeSpan target)
		{
			ActiveAccount.DailyTarget = target;
			_accountContext.SaveChanges();
		}

		public void SetAccountCompletedDailyTarget(TimeSpan span)
		{
			ActiveAccount.CompletedDailyTarget = span;
			_accountContext.SaveChanges();
		}
	}
}
