using System;
using System.Collections.Generic;

namespace Prana.Admin.BLL
{
    public class ReleaseDetails
    {
        public List<object> clientID;
        public List<object> accountID;
        string releaseName;
        string IPAddress;
        string releasePath;
        string clientDB_Name;
        string SMDBName;
        int inUse;

        public ReleaseDetails()
        {
            clientID = new List<object>();
            accountID = new List<object>();
            releaseName = String.Empty;
            IP = String.Empty;
            releasePath = String.Empty;
            clientDB_Name = String.Empty;
            SMDBName = String.Empty;
        }

        public string ReleaseName
        {
            get { return releaseName; }
            set { releaseName = value; }
        }

        public string IP
        {
            get { return IPAddress; }
            set { IPAddress = value; }
        }

        public string ReleasePath
        {
            get { return releasePath; }
            set { releasePath = value; }
        }

        public string ClientDB_Name
        {
            get { return clientDB_Name; }
            set { clientDB_Name = value; }
        }

        public string SMDB_Name
        {
            get { return SMDBName; }
            set { SMDBName = value; }
        }
        public int InUse
        {
            get { return inUse; }
            set { inUse = value; }
        }
    }
}
