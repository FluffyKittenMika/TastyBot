using MultipurposeDataBase.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipurposeDataBase.Contracts
{
    public interface IFileManager
    {
        List<MDBUser> LoadMDBData();
        void SaveMDBData(List<MDBUser> users);
    }
}
