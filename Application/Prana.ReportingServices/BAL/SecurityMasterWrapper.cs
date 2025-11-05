using System;
using System.Collections.Generic;
using System.Text;
using Prana.Interfaces;

namespace Prana.ReportingServices
{
    public class SecurityMasterWrapper
    {
        static Dictionary<int, ISecMasterServices> _clientWiseSecMaster = new Dictionary<int, ISecMasterServices>();
        public static ISecMasterServices GetSecurityMaster(int clientID)
        {
            if (_clientWiseSecMaster.ContainsKey(clientID))
            {
                return _clientWiseSecMaster[clientID];
            }
            else
            {
                throw new Exception("No Security Master Set for this User");
            }
        }
        public static void AddSecMaster(int clientID, ISecMasterServices secMaster, string connString)
        {
            secMaster.ConnectionString = connString;
            if (!_clientWiseSecMaster.ContainsKey(clientID))
                _clientWiseSecMaster.Add(clientID, secMaster);
            else
                _clientWiseSecMaster[clientID] = secMaster;
        }
    }
}
