using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using Prana.Utilities.XMLUtilities;
using Prana.BusinessObjects;
using System.Data;

namespace Prana.AutomationHandlers
{
    public class AutomationHandlerDataManager
    {
        private static Dictionary<string, string> lsAllClients;

        public static Dictionary<string, string> AllClients
        {
            get
            {
                if (lsAllClients == null)
                    getAllClients(); 
                return lsAllClients;
            }
            set { lsAllClients = value; }
        }
	
        
        static Dictionary<string, string> lsAllThirdParties;

        private static Dictionary<string, List<string>> dicClientThirdParties;

        public static Dictionary<string, List<string>> DicClientThirdParties
        {
            get
            {
                if (dicClientThirdParties == null)
                    dicClientThirdParties = getClientThirdPartyDictionary();
                return dicClientThirdParties; }
            set { dicClientThirdParties = value; }  
        }
	
        

        const string ReportsConnectionString = "RiskReportsConnectionString";

        static AutomationHandlerDataManager()
        {
            if (lsAllClients == null)
                getAllClients();
            if (lsAllThirdParties == null)
                getAllThirdParties();
        }

        private static void getAllClients()
        {
            Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ClientID,ClientName FROM T_Client");
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(cmd))
                {
                    lsAllClients = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        lsAllClients.Add(row[0].ToString(), row[1].ToString());
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
        private static void getAllThirdParties()
        {

            try
            {
                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);
                SqlCommand cmd = new SqlCommand("SELECT ThirdPartyID,ThirdPartyName FROM T_ThirdParty");
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(cmd))
                {
                    lsAllThirdParties = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        lsAllThirdParties.Add(row[0].ToString(), row[1].ToString());
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

        internal static Dictionary<string, List<string>> getClientThirdPartyDictionary()
        {
            dicClientThirdParties = new Dictionary<string, List<string>>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);
                SqlCommand cmd = new SqlCommand("SELECT ClientID,ThirdPartyID FROM T_ClientThirdPartyMapping");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(cmd))
                {

                    string clientName, ThirdPartyName;
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[1] != DBNull.Value)
                        {
                            clientName = lsAllClients[row[0].ToString()];
                            ThirdPartyName = lsAllThirdParties[row[1].ToString()];
                            if (!dicClientThirdParties.ContainsKey(clientName))
                                dicClientThirdParties.Add(clientName, new List<string>(new string[] { ThirdPartyName }));
                            else
                                dicClientThirdParties[clientName].Add(ThirdPartyName);
                        }
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

            return dicClientThirdParties;
        }

        internal static int SaveRunUploadFileDataForMarkPrice(List<MarkPriceImport> markPriceImportCollection, String connStr)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(markPriceImportCollection);

                return XMLSaveManager.SaveThroughXML("[PM_SaveMarkPrices]", xml, connStr);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0;

        }

        internal static List<ClientFundsStruct> getClientFundsFromDataBase()
        {
            try
            {
                List<ClientFundsStruct> _lsClientFunds = new List<ClientFundsStruct>();
                ClientFundsStruct _clientFundStruct;
                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("GetClientFunds"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        _clientFundStruct = new ClientFundsStruct();

                        _clientFundStruct.ClientID = Convert.ToInt32(row[0]);
                        _clientFundStruct.ClientName = row[1].ToString();                        
                        _clientFundStruct.FundID = Convert.ToInt32(row[2]);
                        _clientFundStruct.FundName = row[3].ToString();
                       

                        _lsClientFunds.Add(_clientFundStruct);
                    }
                }
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

        internal static GenericBindingList<InternalExternalColumnsStruct> getInternalExternalColumnsFromDataBase()
        {
            try
            {
                GenericBindingList<InternalExternalColumnsStruct> _lsIEColumns = new GenericBindingList<InternalExternalColumnsStruct>();
                InternalExternalColumnsStruct _internalExternalColumnsStruct;
                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("getInternalExternalColumns"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        _internalExternalColumnsStruct = new InternalExternalColumnsStruct();


                        _internalExternalColumnsStruct.ColumnID = Convert.ToInt32(row[0]);
                        _internalExternalColumnsStruct.InternalColumnName = row[1].ToString();
                        _internalExternalColumnsStruct.ExternalColumnName = row[2].ToString();


                        _lsIEColumns.Add(_internalExternalColumnsStruct);
                    }
                }
                return _lsIEColumns;
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

        internal static Dictionary<string, List<int>> getColumnRTypeMappingDictionary()
        {
            Dictionary<string, List<int>> dicColumnRTypeMapping = new Dictionary<string, List<int>>();
            try
            {
                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("RiskReportTypeColumnMapping"))
                {

                    string RiskReportType;
                    int columnID;
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[1] != DBNull.Value)
                        {
                            RiskReportType = row[0].ToString();
                            columnID = Convert.ToInt32(row[1]);
                            if (!dicColumnRTypeMapping.ContainsKey(RiskReportType))
                                dicColumnRTypeMapping.Add(RiskReportType, new List<int>(new int[] { columnID }));
                            else
                                dicColumnRTypeMapping[RiskReportType].Add(columnID);

                        }
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

            return dicColumnRTypeMapping;
        }

        public static void Refresh()
        {        
            getAllClients();
            
            getAllThirdParties();
        }
        public static List<string> GetCurrencyStandardPairs()
        {
            List<string> currencystandardPairs = new List<string>();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("GetCurrencyStandardPairs"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != System.DBNull.Value && (row[1] != System.DBNull.Value))
                        {
                            int fromCurrencyID = int.Parse(row[0].ToString());
                            int toCurrencyID = int.Parse(row[1].ToString());
                            string pairID = fromCurrencyID + Seperators.SEPERATOR_7 + toCurrencyID;
                            if (!currencystandardPairs.Contains(pairID))
                            {
                                currencystandardPairs.Add(pairID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return currencystandardPairs;
        }

        public static int SaveRunUploadFileDataForForexPrice(List<ForexPriceImport> forexPriceImportCollection, String connStr)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(forexPriceImportCollection);

                return XMLSaveManager.SaveThroughXML("[PMSaveForexRate_Import]", xml, connStr);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        public static int SaveRunUploadFileDataForCash(List<CashCurrencyValue> cashCurrencyCollection, String connStr)
        {
            try
            {
                string xml = XMLUtilities.SerializeToXML(cashCurrencyCollection);

                return XMLSaveManager.SaveThroughXML("[PM_SaveCompanyFundCashCurrencyValue]", xml, connStr);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0;

        }

        internal static DataTable getFundsFromClientDataBase(string clientConnectionString,string tableName)
        {
            try
            {
                SqlConnection myConn = new SqlConnection(clientConnectionString); 
                DataSet ds = new DataSet();
                
                myConn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CompanyFundID as FundID,FundName FROM T_CompanyFunds", myConn);
                cmd.CommandTimeout = 200;                
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(ds);                                
                if(ds.Tables.Count>0)
                    return ds.Tables[0];
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

        internal static int UpdateClientFundsInMainDB(DataTable dtClientDbFunds)
        {
            Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);
            int rowsAffected = 0;           
            try
            {
                using (SqlConnection conn = (SqlConnection)db.CreateConnection())
                using (SqlDataAdapter da = new SqlDataAdapter("select * from T_ClientFunds ORDER BY ClientID", conn))
                using (new SqlCommandBuilder(da))
                {
                    DataSet MainDBFundsDataSet = new DataSet();
                    da.Fill(MainDBFundsDataSet, "Funds");

                    #region Code to Udate Main DataSet
                                      

                    //If Here Are More Records in Main Database Then All ClientRecords :--Excessed Records Has to be deleted 
                    foreach (DataRow drMain in MainDBFundsDataSet.Tables["Funds"].Rows)
                    {
                        bool rowFound = false;
                        foreach (DataRow drClientDB in dtClientDbFunds.Rows)
                        {
                            if (drMain["FundID"].ToString() == drClientDB["FundID"].ToString() && drMain["ClientID"].ToString() == drClientDB["ClientID"].ToString())
                            {
                                drMain["FundName"] = drClientDB["FundName"];
                                rowFound = true;
                                break;
                            }
                        }
                        if (!rowFound)
                            drMain.Delete();
                    }

                    //If Here Are Less Records in Main Database Then All ClientRecords :--Records Has to be Added In MainDataBAse 
                    foreach (DataRow drClientDB in dtClientDbFunds.Rows)
                    {
                        bool rowFound = false;
                        foreach (DataRow drMain in MainDBFundsDataSet.Tables["Funds"].Rows)
                        {
                            if (drMain["FundID"].ToString() == drClientDB["FundID"].ToString() && drMain["ClientID"].ToString() == drClientDB["ClientID"].ToString())
                            {
                                drMain["FundName"] = drClientDB["FundName"];
                                rowFound = true;
                                break;
                            }
                        }
                        if (!rowFound)
                        {
                            DataRow newRow = MainDBFundsDataSet.Tables["Funds"].NewRow();
                            newRow["FundID"] = drClientDB["FundID"];
                            newRow["ClientID"] = drClientDB["ClientID"];
                            newRow["FundName"] = drClientDB["FundName"];
                            MainDBFundsDataSet.Tables["Funds"].Rows.Add(newRow);
                        }
                    }


                    #endregion

                    //Assign the SqlCommand to the UpdateCommand property of the SqlDataAdapter.
                    da.UpdateCommand = getUpdateCommand();
                    da.Update(MainDBFundsDataSet, "Funds");
                }
                
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

            return rowsAffected;
        }

        private static SqlCommand getUpdateCommand()
        {            
            SqlCommand DAUpdateCmd=null;
            try
            {
                //Initialize the SqlCommand object that will be used as the UpdateCommand for the DataAdapter.
                DAUpdateCmd = new SqlCommand("Update T_ClientFunds set FundName = @pFundName where FundID = @pFundID AND ClientID=@pClientID");

                //Create and append the parameters for the Update command.
                DAUpdateCmd.Parameters.Add(new SqlParameter("@pFundName", SqlDbType.VarChar));
                DAUpdateCmd.Parameters["@pFundName"].SourceVersion = DataRowVersion.Current;
                DAUpdateCmd.Parameters["@pFundName"].SourceColumn = "FundName";

                DAUpdateCmd.Parameters.Add(new SqlParameter("@pClientID", SqlDbType.Int));
                DAUpdateCmd.Parameters["@pClientID"].SourceVersion = DataRowVersion.Original;
                DAUpdateCmd.Parameters["@pClientID"].SourceColumn = "ClientID";

                DAUpdateCmd.Parameters.Add(new SqlParameter("@pFundID", SqlDbType.Int));
                DAUpdateCmd.Parameters["@pFundID"].SourceVersion = DataRowVersion.Original;
                DAUpdateCmd.Parameters["@pFundID"].SourceColumn = "FundID";
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

            return DAUpdateCmd;
        }        

    }
}
