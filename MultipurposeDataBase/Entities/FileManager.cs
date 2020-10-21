using MultipurposeDataBase.Contracts;
using MultipurposeDataBase.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;


namespace MultipurposeDataBase.Entities
{
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Path to the hidden "ProgramData" folder next to the "Program Files" and "Program Files (x86)" folders.
        /// </summary>
        private static readonly DirectoryInfo saveDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TastyBot"));


        //Thanks Earan for this code

        public List<MDBUser> LoadMDBData()
        {
            FileInfo fi = new FileInfo(Path.Combine(saveDirectory.FullName, $"MultiPurposeDataBase.json"));
            if (!fi.Exists)
            {
                return new List<MDBUser>();
            }
            string json = File.ReadAllText(fi.FullName);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<MDBUser>();
            }
            try
            {
                List<MDBUser> obj = JsonConvert.DeserializeObject<List<MDBUser>>(json);
                return obj;
            }
            catch (Exception)
            {
                return new List<MDBUser>();
            }
        }

        public void SaveMDBData(List<MDBUser> users)
        {
            string json = JsonConvert.SerializeObject(users, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            DirectoryInfo StoragePath = new DirectoryInfo(saveDirectory.FullName);
            if (!StoragePath.Exists)
                StoragePath.Create();
            File.WriteAllText(Path.Combine(StoragePath.FullName, $"MultiPurposeDataBase.json"), json);
        }
    }
}
