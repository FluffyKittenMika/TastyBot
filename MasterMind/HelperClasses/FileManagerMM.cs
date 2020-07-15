using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MasterMind.Contracts;
using MasterMind.Entities;
using Newtonsoft.Json;

namespace MasterMind.HelperClasses
{
    public class FileManagerMM : IFileManagerMM
    {
        /// <summary>
        /// Path to the hidden "ProgramData" folder next to the "Program Files" and "Program Files (x86)" folders.
        /// </summary>
        private static readonly DirectoryInfo saveDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TastyBot"));
        //Thanks Earan for this code

        public List<MasterMindUser> LoadMasterMindUserData()
        {
            FileInfo fi = new FileInfo(Path.Combine(saveDirectory.FullName, $"MasterMindUsers.json"));
            if (!fi.Exists)
            {
                return new List<MasterMindUser>();
            }
            string json = File.ReadAllText(fi.FullName);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<MasterMindUser>();
            }
            try
            {
                List<MasterMindUser> obj = JsonConvert.DeserializeObject<List<MasterMindUser>>(json);
                return obj;
            }
            catch (Exception)
            {
                return new List<MasterMindUser>();
            }
        }

        public void SaveMasterMindUserData(List<MasterMindUser> users)
        {
            string json = JsonConvert.SerializeObject(users, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            DirectoryInfo StoragePath = new DirectoryInfo(saveDirectory.FullName);
            if (!StoragePath.Exists)
                StoragePath.Create();
            File.WriteAllText(Path.Combine(StoragePath.FullName, $"MasterMindUsers.json"), json);
        }
    }
}
