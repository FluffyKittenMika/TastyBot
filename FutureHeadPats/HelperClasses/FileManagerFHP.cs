using FutureHeadPats.Entities;
using FutureHeadPats.Contracts;

using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace FutureHeadPats.HelperClasses
{
    /// <summary>
    /// Manages saving random data into .json files. The files will have the name of the class of the objects being saved.
    /// </summary>
    public class FileManagerFHP : IFileManagerFHP
    {
        /// <summary>
        /// Path to the hidden "ProgramData" folder next to the "Program Files" and "Program Files (x86)" folders.
        /// </summary>
        private static readonly DirectoryInfo saveDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TastyBot"));

        public List<FhpUser> LoadFhpUserData()
        {
            FileInfo fi = new FileInfo(Path.Combine(saveDirectory.FullName, $"FhpUsers.json"));
            if (!fi.Exists)
            {
                return new List<FhpUser>();
            }
            string json = File.ReadAllText(fi.FullName);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<FhpUser>();
            }
            try
            {
                List<FhpUser> obj = JsonConvert.DeserializeObject<List<FhpUser>>(json);
                return obj;
            }
            catch (Exception)
            {
                return new List<FhpUser>();
            }
        }

        public void SaveFhpUserData(List<FhpUser> users)
        {
            string json = JsonConvert.SerializeObject(users, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            DirectoryInfo StoragePath = new DirectoryInfo(saveDirectory.FullName);
            if (!StoragePath.Exists)
                StoragePath.Create();
            File.WriteAllText(Path.Combine(StoragePath.FullName, $"FhpUsers.json"), json);
        }
    }
}
