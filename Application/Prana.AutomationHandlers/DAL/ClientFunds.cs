using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data;

namespace Prana.AutomationHandlers
{
    public class ClientFunds
    {        

        static ClientFunds()
        {
            try
            {
                if (_lsClientFunds == null)
                    getAllFunds();
                if (DicFundIDByClientIDFundName == null || DicFundNameByClientIDFundID == null)
                    InitializeDictionaries();
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Global Beacuse It Will Be The Same For each Client key:--ClientID
        //This Is Seted By PranaReportingServices.ImportFundsFromDifferentClientsDB()
        private static Dictionary<string, ClientSettings> _dicClientDetails;
        public static Dictionary<string, ClientSettings> DicClientDetails
        {
            get { return _dicClientDetails; }
            set { _dicClientDetails = value; }
        }       
       

        #region Private Section

        private static List<ClientFundsStruct> _lsClientFunds;

        private static List<ClientFundsStruct> ClientFundsList
        {
            get { return _lsClientFunds; }
            set { _lsClientFunds = value; }
        }

        private static Dictionary<string,string> _dicFundNameByClientIDFundID;

        private static Dictionary<string, string> DicFundNameByClientIDFundID
        {
            get { return _dicFundNameByClientIDFundID; }
            set { _dicFundNameByClientIDFundID = value; }
        }

        private static Dictionary<string, int> _dicFundIDByClientIDFundName;

        private static Dictionary<string, int> DicFundIDByClientIDFundName
        {
            get { return _dicFundIDByClientIDFundName; }
            set { _dicFundIDByClientIDFundName = value; }
        }

        private static void InitializeDictionaries()
        {
            try
            {
                if (ClientFundsList != null)
                {
                    string KeyClientIDFundName, KeyClientIDFundID;
                    DicFundIDByClientIDFundName = new Dictionary<string, int>();
                    DicFundNameByClientIDFundID = new Dictionary<string, string>();
                    foreach (ClientFundsStruct _ClientFundsStruct in ClientFundsList)
                    {
                        KeyClientIDFundID = _ClientFundsStruct.ClientID.ToString() + _ClientFundsStruct.FundID.ToString();
                        KeyClientIDFundName = _ClientFundsStruct.ClientID.ToString() + _ClientFundsStruct.FundName;

                        if (!DicFundNameByClientIDFundID.ContainsKey(KeyClientIDFundID))
                            DicFundNameByClientIDFundID.Add(KeyClientIDFundID, _ClientFundsStruct.FundName);

                        if (!DicFundIDByClientIDFundName.ContainsKey(KeyClientIDFundName))
                            DicFundIDByClientIDFundName.Add(KeyClientIDFundName, _ClientFundsStruct.FundID);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void SetClientId(DataTable dataTable, int clientID)
        {
            dataTable.Columns.Add("ClientID", typeof(int));
            foreach (DataRow dr in dataTable.Rows)
                dr["ClientID"] = clientID;
        }

        private static DataTable MergDataTables(DataTable dtToMerg, DataTable dtMain)
        {
            if (dtMain == null && dtToMerg != null)
                dtMain = dtToMerg;
            else if (dtToMerg != null && dtMain != null)
                dtMain.Merge(dtToMerg);
            return dtMain;
        }

        #endregion


        #region Public Section

        public static string GetFundName(int clientID, int FundID)
        {
            string KeyClientIDFundID = clientID.ToString() + FundID.ToString();
            if (DicFundNameByClientIDFundID.ContainsKey(KeyClientIDFundID))
                return DicFundNameByClientIDFundID[KeyClientIDFundID];
            return null;
        }
        public static int GetFundID(int clientID, string FundName)
        {
            int FundID = int.MinValue;
            string KeyClientIDFundName = clientID.ToString() + FundName;
            if (DicFundIDByClientIDFundName.ContainsKey(KeyClientIDFundName))
                FundID = DicFundIDByClientIDFundName[KeyClientIDFundName];
            return FundID;
        }
        public static void Refresh()
        {
            try
            {
                _lsClientFunds = AutomationHandlerDataManager.getClientFundsFromDataBase();

                InitializeDictionaries();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public static bool ImportClientFunds()
        {
            try
            {
                if (DicClientDetails != null)
                {
                    DataTable dtAllClientsFunds = null;
                    foreach (string clientName in DicClientDetails.Keys)
                    {
                        int clientID = DicClientDetails[clientName].ClientID;
                        string clientConnectionString = DicClientDetails[clientName].DataBaseSettings.ClientDB;
                        DataTable dtclientSpecificFunds = AutomationHandlerDataManager.getFundsFromClientDataBase(clientConnectionString, DicClientDetails[clientName].ClientName);
                        if (dtclientSpecificFunds != null)
                            SetClientId(dtclientSpecificFunds, clientID);

                        dtAllClientsFunds = MergDataTables(dtclientSpecificFunds, dtAllClientsFunds);
                    }
                    //To Save AllClients Funds in Our DataBase 
                    AutomationHandlerDataManager.UpdateClientFundsInMainDB(dtAllClientsFunds);
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        public static List<ClientFundsStruct> getAllFunds()
        {
            try
            {              

                _lsClientFunds = AutomationHandlerDataManager.getClientFundsFromDataBase();
                
                return _lsClientFunds;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        #endregion
       

        

    }
}
