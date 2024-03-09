using DeepWork.Models;
using DeepWork.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DeepWork.Services
{
    public class AccountManagementService : IDisposable
	{
		private readonly XmlSerializer m_AccountSerializer;
		private Stream m_AccountFileStream;
		public Account UserAccount { get; private set; }
		public string AccountFilePath { get; private set; }
		public string AppDataPath { get; private set; }
		public bool IsAccountAvailable { get; private set; }

		public AccountManagementService()
		{
			m_AccountSerializer = new XmlSerializer(typeof(Account));

			// Create a path to appdata directory
			string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			AppDataPath = $@"{localAppData}\Deep Work";
			AccountFilePath = $@"{AppDataPath}\Account.xml";

			// Checking appdata directory availability
			if (!Directory.Exists(AccountFilePath))
				Directory.CreateDirectory(AppDataPath);

			// Checking account data xml file availablability
			if (File.Exists(AccountFilePath))
			{
				// Opening account data xml file
				m_AccountFileStream = File.Open(AccountFilePath, FileMode.Open, FileAccess.ReadWrite);

				// Deserializing account data xml file
				try
				{
					UserAccount = (Account)m_AccountSerializer.Deserialize(m_AccountFileStream);
					IsAccountAvailable = true;
				}
				catch (InvalidOperationException)
				{
					m_AccountFileStream.Dispose();
					File.Delete(AccountFilePath);
					IsAccountAvailable = false;
				}
			}
			else
			{
				m_AccountFileStream = Stream.Null;
				IsAccountAvailable = false;
			}
		}

		public void CreateAccount(string username, string password)
		{
			UserAccount = new Account
			{
				Username = username,
				Password = password
			};
			IsAccountAvailable = true;
			m_AccountFileStream = File.Create(AccountFilePath);
			SaveChanges();
		}

		public void AddLongTask(LongTask task)
		{
			if (!IsAccountAvailable)
				return;
			UserAccount.LongTasks.Add(task);
			SaveChanges();
		}

		public void AddShortTask(string parentName, ShortTask task)
		{
			if (!IsAccountAvailable)
				return;
			LongTask parentTask = UserAccount.LongTasks.First(item => item.Name == parentName);
			parentTask.RunningTasks.Add(task);
			SaveChanges();
		}
        public void FinishLongTask(string name)
        {
			if (!IsAccountAvailable)
				return;

			UserAccount.LongTasks.Remove(UserAccount.LongTasks.FirstOrDefault(item => item.Name == name));
			SaveChanges();
        }

		public void FinishShortTask(string parentName, string taskName)
		{
			if (!IsAccountAvailable)
				return;

			LongTask parentTask = UserAccount.LongTasks.First(item => item.Name == parentName);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Name == taskName);
			if (parentTask.MaxDuration < task.Duration)
				parentTask.MaxDuration = task.Duration;
			task.FinishDate = DateTime.Now;

			parentTask.RunningTasks.Remove(task);
			parentTask.FinishedTasks.Add(task);

			SaveChanges();
		}

		public void EditLongTask(string taskName, LongTask editedTask)
		{
			LongTask task = GetLongTaskByName(taskName);
			task.Name = editedTask.Name;
			task.StartDate = editedTask.StartDate;
			task.EndDate = editedTask.EndDate;
			SaveChanges();
		}

		public void EditShortTask(string parentTask, string taskToEdit, ShortTask editedTask)
		{
			ShortTask task = GetShortTaskByName(parentTask, taskToEdit);
			task.Name = editedTask.Name;
			task.Duration = editedTask.Duration;
			SaveChanges();
		}

		public LongTask GetLongTaskByName(string name) =>
			UserAccount.LongTasks.First(task => task.Name == name);

		public ShortTask GetShortTaskByName(string parentName, string name)
		{
			LongTask parentTask = UserAccount.LongTasks.First(item => item.Name == parentName);
			ShortTask task = parentTask.RunningTasks.FirstOrDefault(item => item.Name == name);
			return task;
		}

		public List<TaskHistory> GetAccountHistory()
		{
			List<TaskHistory> tasksHistoryTree = new();
			foreach (var longTask in UserAccount.LongTasks)
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

		public void SaveChanges()
		{
			m_AccountFileStream.SetLength(0);
			m_AccountSerializer.Serialize(m_AccountFileStream, UserAccount);
			m_AccountFileStream.Flush();
		}

		public void Dispose()
		{
			SaveChanges();
			m_AccountFileStream.Dispose();
		}
	}
}
