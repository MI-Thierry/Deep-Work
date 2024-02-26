using DeepWork.MVVM.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
			SHA1 sha1 = SHA1.Create();
			byte[] psw = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
			UserAccount = new Account
			{
				Username = username,
				Password = Encoding.UTF8.GetString(psw)
			};
			m_AccountFileStream = File.Create(AccountFilePath);
			UpdateAccountDataFile();
		}

		public void AddLongTask(LongTask task)
		{
			UserAccount.LongTasks.Add(task);
			UpdateAccountDataFile();
		}

		public void AddShortTask(LongTask parentTask, ShortTask task)
		{
			parentTask.Tasks.Add(task);
			UpdateAccountDataFile();
		}

		private void UpdateAccountDataFile()
		{
			m_AccountSerializer.Serialize(m_AccountFileStream, UserAccount);
			m_AccountFileStream.Flush();
		}

		public void Dispose()
		{
			UpdateAccountDataFile();
			m_AccountFileStream.Dispose();
		}
	}
}
