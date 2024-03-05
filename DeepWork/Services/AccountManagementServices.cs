using DeepWork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DeepWork.Services
{
    public class AccountManagementServices : IDisposable
	{
		private readonly XmlSerializer m_AccountSerializer;
		private Stream m_AccountFileStream;
		public Account UserAccount { get; private set; }
		public string AccountFilePath { get; private set; }
		public string AppDataPath { get; private set; }
		public bool IsAccountAvailable { get; private set; }

		public AccountManagementServices()
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
			UserAccount.LongTasks.Add(task);
			SaveChanges();
		}

		public void AddShortTask(LongTask parentTask, ShortTask task)
		{
			parentTask.RunningTasks.Add(task);
			SaveChanges();
		}
        public void FinishLongTask(string name)
        {
			LongTask taskToFinish = UserAccount.LongTasks.FirstOrDefault(task => task.Name == name);
			UserAccount.LongTasks.Remove(taskToFinish);
			SaveChanges();
        }

		public void FinishShortTask(LongTask parentTask, string name)
		{
			ShortTask taskToFinish = parentTask.RunningTasks.FirstOrDefault(task => task.Name == name);
			
			if (parentTask.MaxDuration < taskToFinish.Duration)
				parentTask.MaxDuration = taskToFinish.Duration;
			taskToFinish.FinishDate = DateTime.Now;

			parentTask.RunningTasks.Remove(taskToFinish);
			parentTask.FinishedTasks.Add(taskToFinish);

			SaveChanges();
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

        public async void SaveChanges()
		{
			await Task.Run(() =>
			{
				m_AccountFileStream.SetLength(0);
				m_AccountSerializer.Serialize(m_AccountFileStream, UserAccount);
				m_AccountFileStream.Flush();
			});
		}

		public void Dispose()
		{
			SaveChanges();
			m_AccountFileStream.Dispose();
		}
    }
}
