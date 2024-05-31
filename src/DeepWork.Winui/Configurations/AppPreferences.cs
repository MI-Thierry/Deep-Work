using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace DeepWork.UI.Shared;
public class AppPreferences : IAppPreferences
{
	public static Type[] SupportedTypes { get; private set; } =
	[
		typeof(short),
		typeof(int),
		typeof(long),
		typeof(float),
		typeof(double),
		typeof(DateTime),
		typeof(string),
	];
	private readonly object _locker = new object();
	private readonly XmlDocument _preferenceDoc = new();
	private readonly Stream _preferenceStream;
	private XmlNode ParentNode => _preferenceDoc.LastChild ?? throw new InvalidOperationException("Missing root element");
	public AppPreferences(Stream stream)
	{
		_preferenceStream = stream;
		if (_preferenceStream.Length == 0)
		{
			using var writer = XmlWriter.Create(_preferenceStream);
			writer.WriteStartDocument();
			writer.WriteStartElement("Preferences");
			writer.WriteEndDocument();
			writer.Close();
			_preferenceStream.Position = 0;
		}
		_preferenceDoc.Load(_preferenceStream);
	}
	public void Clear(string? sharedName = null)
	{
		lock (_locker)
		{
			ParentNode.RemoveAll();
			_preferenceDoc.Save(_preferenceStream);
		}
	}

	public bool ContainsKey(string key, string? sharedName = null)
	{
		lock (_locker)
		{
			foreach (XmlNode child in ParentNode.ChildNodes)
				if (child.Name == key)
					return true;
			return false;
		}
	}

	public TResult? Get<TResult>(string key, string? sharedName = null) where TResult : notnull
	{
		lock (_locker)
		{
			object result = 0;
			if (!SupportedTypes.Contains(typeof(TResult)))
				throw new InvalidOperationException($"TResult Type is not supported. This is is the only supported types: {string.Join(',', SupportedTypes.AsEnumerable())}");

			foreach (XmlNode child in ParentNode.ChildNodes)
			{
				if (child.Name == key)
				{
					if (typeof(TResult) == typeof(int))
						result = int.Parse(child.InnerText);

					else if (typeof(TResult) == typeof(short))
						result = short.Parse(child.InnerText);

					else if (typeof(TResult) == typeof(bool))
						result = bool.Parse(child.InnerText);

					else if (typeof(TResult) == typeof(double))
						result = double.Parse(child.InnerText);

					else if (typeof(TResult) == typeof(float))
						result = float.Parse(child.InnerText);

					else if (typeof(TResult) == typeof(DateTime))
						result = DateTime.Parse(child.InnerText);

					else if (typeof(TResult) == typeof(long))
						result = long.Parse(child.InnerText);

					else if (typeof(TResult) == typeof(string))
						result = child.InnerText;

					else
						throw new InvalidOperationException($"{typeof(TResult).Name} is not supported. This is is the only supported types: {string.Join(',', SupportedTypes.AsEnumerable())}");
				}
				return (TResult)result;
			}
			return default;
		}
	}

	public void Remove(string key, string? sharedName = null)
	{
		lock (_locker)
		{
			foreach (XmlNode child in ParentNode.ChildNodes)
				if (child.Name == key)
					ParentNode.RemoveChild(child);
			Save();
		}
	}

	public void Set<TIn>(string key, TIn value, string? sharedName = null) where TIn : notnull
	{
		lock (_locker)
		{
			bool isAvailable = false;

			if (!SupportedTypes.Contains(typeof(TIn)))
				throw new InvalidOperationException($"TResult Type is not supported. This is is the only supported types: {string.Join(',', SupportedTypes.AsEnumerable())}");

			foreach (XmlNode child in ParentNode.ChildNodes)
			{
				if (child.Name == key)
				{
					child.InnerText = value.ToString() ?? string.Empty;
					isAvailable = true;
				}
			}
			if (!isAvailable)
				ParentNode.AppendChild(_preferenceDoc.CreateElement(key))!.InnerText = value.ToString()!;
			Save();
		}
	}

	private void Save()
	{
		lock (_locker)
		{
			_preferenceStream.SetLength(0);
			_preferenceDoc.Save(_preferenceStream);
			_preferenceStream.Flush();
		}
	}
}
