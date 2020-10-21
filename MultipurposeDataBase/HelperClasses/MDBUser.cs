using MasterMind.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipurposeDataBase.HelperClasses
{
    public class MDBUser
    {
        public ulong UserId { get; set; }
        public string UserName { get; set; }
        public MasterMindDBUser MMGame { get; set; }
    }
}
