using DeepWork.MVVM.Models;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DeepWork.Services
{
	public class AccountServices : IDisposable
	{
		private readonly XmlSerializer m_AccountSerializer;
		private readonly Stream m_AccountFileStream;
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
				m_AccountSerializer = new(typeof(Account));
				UserAccount = (Account)m_AccountSerializer.Deserialize(m_AccountFileStream);
			}
			else
			{
				IsAccountAvailable = false;

				// Creating Account data xml file
				File.Create(AccountFilePath);

				// Opening account data xml file stream
				m_AccountFileStream = File.OpenWrite(AccountFilePath);

			}
		}

		public void CreateAccount(Account usrAccount)
		{
			UserAccount = usrAccount;
			m_AccountSerializer.Serialize(m_AccountFileStream, UserAccount);
		}

		public void Dispose()
		{
			m_AccountFileStream.Dispose();
		}
	}
}
