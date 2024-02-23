using DeepWork.MVVM.Models;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DeepWork.Services
{
	public class AccountServices : IDisposable
	{
		private readonly XmlSerializer m_AccountSerializer;
		private Stream m_AccountFileStream;
		public Account UserAccount { get; private set; }
		public string AccountFilePath { get; private set; }
		public string AppDataPath { get; private set; }
		public bool IsAccountAvailable { get; private set; }

		public AccountServices()
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
				IsAccountAvailable = true;

				// Opening account data xml file
				m_AccountFileStream = File.OpenWrite(AccountFilePath);

				// Deserializing account data xml file
				try
				{
					UserAccount = (Account)m_AccountSerializer.Deserialize(m_AccountFileStream);
				}
				catch (InvalidOperationException)
				{
					m_AccountFileStream.Dispose();
					File.Delete(AccountFilePath);
				}
			}
			else
			{
				m_AccountFileStream = Stream.Null;
				IsAccountAvailable = false;
			}
		}

		public void CreateAccount(Account usrAccount)
		{
			UserAccount = usrAccount;
			m_AccountFileStream = File.Create(AccountFilePath);
			m_AccountSerializer.Serialize(m_AccountFileStream, UserAccount);
		}

		public void Dispose()
		{
			m_AccountFileStream.Dispose();
		}
	}
}
