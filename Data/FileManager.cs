using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TastyBot.Data
{
	/// <summary>
	/// Manages saving random data into .json files. The files will have the name of the class of the objects being saved.
	/// </summary>
	public class FileManager
	{
		/// <summary>
		/// Path to the hidden "ProgramData" folder next to the "Program Files" and "Program Files (x86)" folders.
		/// </summary>
		private readonly DirectoryInfo saveDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TastyBot"));

		/// <summary>
		/// Contains all the lock objects for each type of data. This way we have a per file lock system.
		/// </summary>
		private readonly Dictionary<string, object> dicWriteLock = new Dictionary<string, object>();


		public void Init()
		{
			if (!saveDirectory.Exists)
			{
				saveDirectory.Create();
			}
		}

		private object GetLock(string type)
		{
			if (!dicWriteLock.TryGetValue(type, out object writelock))
			{
				dicWriteLock[type] = new object();
				writelock = dicWriteLock[type];
			}
			return writelock;
		}

		/// <summary>
		/// Saves a List of data as json files. 
		/// </summary>
		/// <typeparam name="T">The type of data to save. Also the name of the resulting file.</typeparam>
		/// <param name="data">The data to serialize into json.</param>
		/// <param name="id">A uinque id that identifies the server the data files belong to.</param>
		/// <returns></returns>
		public Task SaveData<T>(List<T> data, string id)
		{
			string type = typeof(T).Name;
			object writeLock = GetLock(type);
			return Task.Run(() =>
			{
				string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects });
				lock (writeLock)
				{
					File.WriteAllText(Path.Combine(saveDirectory.FullName, id, $"{type}.json"), json);
				}
			});
		}

		/// <summary>
		/// Loads a List of data from a json file
		/// </summary>
		/// <typeparam name="T">The type of data to save. Also the name of the file to load data from</typeparam>
		/// <param name="id">A uinque id that identifies the server the data files belong to.</param>
		/// <returns></returns>
		public Task<List<T>> LoadData<T>(string id)
		{
			string type = typeof(T).Name;
			object writeLock = GetLock(type);
			return Task.Run(() =>
			{
				FileInfo fi = new FileInfo(Path.Combine(saveDirectory.FullName, id, $"{type}.json"));
				if (!fi.Exists)
				{
					return new List<T>();
				}

				string json = string.Empty;
				lock (writeLock)
				{
					json = File.ReadAllText(fi.FullName);
				}
				List<T> lstData = new List<T>();
				if (string.IsNullOrWhiteSpace(json))
				{
					return new List<T>();
				}
				try
				{
					lstData = JsonConvert.DeserializeObject<List<T>>(json);
				}
				catch (Exception)
				{ }
				return lstData;
			});
		}
	}
}
