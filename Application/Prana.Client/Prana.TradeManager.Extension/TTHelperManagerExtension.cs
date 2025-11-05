using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.TradeManager.Extension
{
    public class TTHelperManagerExtension
    {
        static private TTHelperManagerExtension _ttHelperManagerExtension = null;
        TTHelperCollection _ttCollection = new TTHelperCollection();
        List<TTHotKeyPreferences> _companyUserHotKeyPreferences = new List<TTHotKeyPreferences>();
        List<TTHotKeyPreferencesDetails> _companyUserHotKeyPreferencesDetails = new List<TTHotKeyPreferencesDetails>();
        Dictionary<string, Dictionary<int, string>> venueCollection = new Dictionary<string, Dictionary<int, string>>();
        Dictionary<string, Dictionary<int, string>> counterPartyCollection = new Dictionary<string, Dictionary<int, string>>();

        Dictionary<string, Dictionary<int, string>> counterPartyCollectionQTT = new Dictionary<string, Dictionary<int, string>>();
        Dictionary<string, Dictionary<int, string>> venueCollectionQTT = new Dictionary<string, Dictionary<int, string>>();

        int _userID = int.MinValue;
        private TTHelperManagerExtension()
        {
        }
        public static TTHelperManagerExtension GetInstance()
        {
            if (_ttHelperManagerExtension == null)
            {
                _ttHelperManagerExtension = new TTHelperManagerExtension();
            }
            return _ttHelperManagerExtension;
        }

        private Dictionary<int, int> _fundWiseExecutingBrokerMapping = new Dictionary<int, int>();
        /// <summary>
        /// Retrieves a dictionary representing the mapping of fund ID to executing broker ID
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> FundWiseExecutingBrokerMapping
        {
            get { return _fundWiseExecutingBrokerMapping; }
        }

        private bool _isFundWiseExecutingBrokerMappingAvailable;
        public bool IsFundWiseExecutingBrokerMappingAvailable
        {
            get { return _isFundWiseExecutingBrokerMappingAvailable; }
            set { _isFundWiseExecutingBrokerMappingAvailable = value; }
        }

        /// <summary>
        /// Retrieves fund-wise executing broker mapping from the db
        /// </summary>
        /// <param name="companyID"></param>
        public void GetFundWiseExecutingBrokerMappingFromDB(int companyID)
        {
            Dictionary<int, int> fundWiseExecutingBrokerMapping = new Dictionary<int, int>();
            bool isFundWiseExecutingBrokerMapping = false;
            try
            {
                object[] param = { companyID };
                string sProc = "P_GetFundWiseExecutingBroker";
                using (IDataReader dr = DatabaseManager.DatabaseManager.ExecuteReader(sProc, param))
                {
                    while (dr.Read())
                    {
                        int fundId = Convert.ToInt32(dr[0]);
                        int brokerId = Convert.ToInt32(dr[1]);
                        fundWiseExecutingBrokerMapping.Add(fundId, brokerId);
                    }
                    isFundWiseExecutingBrokerMapping = true;
                }
                _fundWiseExecutingBrokerMapping = fundWiseExecutingBrokerMapping;
                _isFundWiseExecutingBrokerMappingAvailable = isFundWiseExecutingBrokerMapping;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        Dictionary<int, List<int>> counterPartyAccountCollection = new Dictionary<int, List<int>>();

        public Dictionary<int, List<int>> CounterPartyAccountCollection
        {
            get { return counterPartyAccountCollection; }
        }

        /// <summary>
        /// Account wise CounterParty-Venue pair(s) collection
        /// </summary>
        Dictionary<int, Dictionary<int,List<int>>> accountWiseCounterPartyVenueCollection = new Dictionary<int, Dictionary<int, List<int>>>();
        public Dictionary<int, Dictionary<int, List<int>>> AccountWiseCounterPartyVenueCollection
        {
            get { return accountWiseCounterPartyVenueCollection; }
        }

        public Dictionary<string, Dictionary<int, string>> VenueCollection
        {
            get { return venueCollection; }
        }

        public Dictionary<string, Dictionary<int, string>> VenueCollectionQTT
        {
            get { return venueCollectionQTT; }
        }

        public Dictionary<string, Dictionary<int, string>> CounterPartyCollection
        {
            get { return counterPartyCollection; }
        }

        public Dictionary<string, Dictionary<int, string>> CounterPartyCollectionQTT
        {
            get { return counterPartyCollectionQTT; }
        }

        public TTHelperCollection TTCollection
        {
            get { return _ttCollection; }
        }

        public List<TTHotKeyPreferences> CompanyUserHotKeyPreferences
        {
            get { return _companyUserHotKeyPreferences; }
        }

        public List<TTHotKeyPreferencesDetails> CompanyUserHotKeyPreferencesDetails
        {
            get { return _companyUserHotKeyPreferencesDetails; }
        }

        public int UserID
        {
            get { return this._userID; }
            set { this._userID = value; }
        }

        /// <summary>
        /// Get CV Account Mappings
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public void GetCVAccountMappings(int CompanyID)
        {
            DataSet clientAccounts = new DataSet();
            counterPartyAccountCollection.Clear();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCounterPartyAccountMappingData";
                queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@companyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = CompanyID
                });
                queryData.CommandTimeout = 200;

                clientAccounts = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (clientAccounts != null && clientAccounts.Tables.Count > 0)
                {
                    foreach (DataRow row in clientAccounts.Tables[0].Rows)
                    {
                        //ValueList vl = new ValueList();
                        int counterPartyId = row["CounterPartyID"] != System.DBNull.Value ? Convert.ToInt32(row["CounterPartyID"].ToString()) : 0;
                        int accountId = row["CompanyFundID"] != System.DBNull.Value ? Convert.ToInt32(row["CompanyFundID"].ToString()) : 0;
                        string counterPartyName = row["CounterPartyName"] != System.DBNull.Value ? row["CounterPartyName"].ToString() : string.Empty;
                        if (counterPartyId > 0)
                        {
                            //vl.ValueListItems.Add(counterPartyId, counterPartyName);

                            if (!counterPartyAccountCollection.ContainsKey(accountId))
                            {
                                counterPartyAccountCollection.Add(accountId, new List<int> { counterPartyId });
                            }
                            else
                            {
                                var existingList = counterPartyAccountCollection[accountId];
                                if(!existingList.Contains(counterPartyId))
                                    existingList.Add(counterPartyId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Gets the Account wise Counter Party- Venue pair(s) mappings.
        /// </summary>
        /// <param name="CompanyID"></param>
        public void GetAccountCounterPartyVenueMappings(int CompanyID)
        {
            accountWiseCounterPartyVenueCollection.Clear();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCounterPartyAccountMappingData";
                queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@companyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = CompanyID
                });
                queryData.CommandTimeout = 200;
                DataSet clientAccountsDetails = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (clientAccountsDetails != null && clientAccountsDetails.Tables.Count > 0)
                {
                    foreach (DataRow row in clientAccountsDetails.Tables[0].Rows)
                    {
                        int counterPartyId = row["CounterPartyID"] != DBNull.Value ? Convert.ToInt32(row["CounterPartyID"].ToString()) : 0;
                        int accountId = row["CompanyFundID"] != DBNull.Value ? Convert.ToInt32(row["CompanyFundID"].ToString()) : 0;
                        int venueId = row["VenueID"] != DBNull.Value ? Convert.ToInt32(row["VenueID"].ToString()) : 0;
                        string venueName = row["VenueName"] != DBNull.Value ? row["VenueName"].ToString() : string.Empty;
                        if (counterPartyId > 0 && !string.Equals(venueName, "Ex&Assign", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!accountWiseCounterPartyVenueCollection.ContainsKey(accountId))
                            {
                                Dictionary<int, List<int>> counterPartyVenueMap = new Dictionary<int, List<int>>
                                {
                                    { counterPartyId, new List<int> { venueId } }
                                };
                                accountWiseCounterPartyVenueCollection.Add(accountId, counterPartyVenueMap);
                            }
                            else
                            {
                                Dictionary<int, List<int>> existingBrokers = accountWiseCounterPartyVenueCollection[accountId];
                                if (existingBrokers.ContainsKey(counterPartyId))
                                {
                                    existingBrokers[counterPartyId].Add(venueId);
                                }
                                else
                                {
                                    existingBrokers[counterPartyId] = new List<int> { venueId };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Get CV AUEC Mappings
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public void GetCVAUECMappings(int CompanyID, int UserID)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[2];
                parameter[0] = CompanyID;
                parameter[1] = UserID;

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTTicketBuildData", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            _ttCollection.Add(FillTTHelper(row, 0));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Get Company User Hot Key Preferences
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public void GetCompanyUserHotKeyPreferences(int UserID)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[1];
                parameter[0] = UserID;
                _companyUserHotKeyPreferences.Clear();

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUserHotKeyPreferences", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            TTHotKeyPreferences ttHotKey = new TTHotKeyPreferences();
                            ttHotKey.CompanyUserHotKeyPreferenceID = Convert.ToInt32(row[0]);
                            ttHotKey.CompanyUserID = Convert.ToInt32(row[1]);
                            ttHotKey.HotKeyPreferenceElements = row[2].ToString();
                            ttHotKey.EnableBookMarkIcon = row[3] == DBNull.Value ? false : Convert.ToBoolean(row[3]);
                            ttHotKey.HotKeyOrderChanged = row[4] == DBNull.Value ? false : Convert.ToBoolean(row[4]);
                            ttHotKey.TTTogglePreferenceForWeb = row[5] == DBNull.Value ? false : Convert.ToBoolean(row[5]);

                            _companyUserHotKeyPreferences.Add(ttHotKey);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Update Company User Hot Key Preferences
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public int UpdateCompanyUserHotKeyPreferences(int UserID, bool EnableBookMarkIcon, bool HotKeyOrderChanged, bool tTTogglePreferenceForWeb, string HotKeyPreferenceElements)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[5];
                parameter[0] = HotKeyPreferenceElements;
                parameter[1] = EnableBookMarkIcon;
                parameter[2] = HotKeyOrderChanged;
                parameter[3] = tTTogglePreferenceForWeb;
                parameter[4] = UserID;

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_UpdateCompanyUserHotKeyPreferences", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            return Convert.ToInt32(row[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// Get Company User Hot Key Preferences Details
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public void GetCompanyUserHotKeyPreferencesDetails(int UserID)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[1];
                parameter[0] = UserID;
                _companyUserHotKeyPreferencesDetails.Clear();
                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyUserHotKeyPreferencesDetails", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            TTHotKeyPreferencesDetails ttHotKeyDetails = new TTHotKeyPreferencesDetails();
                            ttHotKeyDetails.CompanyUserHotKeyID = Convert.ToInt32(row[0]);
                            ttHotKeyDetails.CompanyUserID = Convert.ToInt32(row[1]);
                            ttHotKeyDetails.CompanyUserHotKeyName = row[2].ToString();
                            ttHotKeyDetails.HotKeyPreferenceNameValue = row[3].ToString();
                            ttHotKeyDetails.IsFavourites = Convert.ToBoolean(row[4]);
                            ttHotKeyDetails.HotKeySequence = Convert.ToInt32(row[5]);
                            ttHotKeyDetails.Module = row[6].ToString();
                            ttHotKeyDetails.HotButtontype = row[7].ToString();

                            _companyUserHotKeyPreferencesDetails.Add(ttHotKeyDetails);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Update Company User Hot Key Preferences Details
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public int UpdateCompanyUserHotKeyPreferencesDetails(int CompanyUserHotKeyID, int UserID, string CompanyUserHotKeyName, string HotKeyPreferenceNameValue, bool IsFavourites, int HotKeySequence)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[6];
                parameter[0] = CompanyUserHotKeyName;
                parameter[1] = HotKeyPreferenceNameValue;
                parameter[2] = IsFavourites;
                parameter[3] = HotKeySequence;
                parameter[4] = UserID;
                parameter[5] = CompanyUserHotKeyID;

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_UpdateCompanyUserHotKeyPreferencesDetails", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            return Convert.ToInt32(row[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// Update Company User Hot Key Sequence Order
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public int UpdateCompanyUserHotKeySequenceOrder(int CompanyUserHotKeyID, int UserID, int HotKeySequence)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[3];
                parameter[0] = HotKeySequence;
                parameter[1] = UserID;
                parameter[2] = CompanyUserHotKeyID;

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_UpdateCompanyUserHotKeySequenceOrder", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            return Convert.ToInt32(row[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// Update Company User Hot Key Preferences Details
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public int SaveCompanyUserHotKeyPreferencesDetails(int UserID, string CompanyUserHotKeyName, string HotKeyPreferenceNameValue, bool IsFavourites, int HotKeySequence, string module, string hotButtonType)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[7];
                parameter[0] = CompanyUserHotKeyName;
                parameter[1] = HotKeyPreferenceNameValue;
                parameter[2] = IsFavourites;
                parameter[3] = HotKeySequence;
                parameter[4] = UserID;
                parameter[5] = module;
                parameter[6] = hotButtonType;

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_SaveCompanyUserHotKeyPreferencesDetails", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            return Convert.ToInt32(row[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
            return -1;
        }
        
        /// <summary>
        /// Delete Company User Hot Key Preferences Details
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public int DeleteCompanyUserHotKeyPreferencesDetails(int UserID, string CompanyUserHotKeyName)
        {
            if (UserID != _userID)
            {
                object[] parameter = new object[2];
                parameter[0] = CompanyUserHotKeyName;
                parameter[1] = UserID;

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_DeleteCompanyUserHotKeyPreferencesDetails", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            return Convert.ToInt32(row[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

            }
            return -1;
        }

        private TTHelper FillTTHelper(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            TTHelper _ttHelper = null;
            try
            {
                if (row != null)
                {
                    _ttHelper = new TTHelper();
                    int AUECID = offset + 0;
                    int ASSET_ID = offset + 1;
                    int UNDERLYING_ID = offset + 2;
                    int COUNTERPARTY_ID = offset + 3;
                    int VENUE_ID = offset + 4;
                    int UNDERLYING_NAME = offset + 5;

                    //					int QUANTITY	= offset + 5;
                    //					int ISSTAGED	= offset + 6;


                    _ttHelper.AuecID = int.Parse(row[AUECID].ToString());
                    _ttHelper.AssetID = int.Parse(row[ASSET_ID].ToString());
                    _ttHelper.UnderlyingID = int.Parse(row[UNDERLYING_ID].ToString());
                    _ttHelper.CounterpartyID = int.Parse(row[COUNTERPARTY_ID].ToString());
                    _ttHelper.VenueID = int.Parse(row[VENUE_ID].ToString());
                    _ttHelper.UnderlyingName = row[UNDERLYING_NAME].ToString();
                    //					_ttHelper.Quantity				= row[QUANTITY].ToString();
                    //					_ttHelper.IsStaged				= row[ISSTAGED].ToString();


                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return _ttHelper;

        }

        public string AUECCouterPartyKey(int auecId, int counterPartyID)
        {
            return AUECKey(auecId) + "C" + counterPartyID;
        }

        public string AUECKey(int auecId)
        {
            return "AUEC" + auecId;
        }

        public string AUCKey(int assetID, int underLyingID, int counterPartyID)
        {
            return AUKey(assetID, underLyingID) + "C" + counterPartyID;
        }

        public string AUKey(int assetID, int underLyingID)
        {
            return "A" + assetID.ToString() + "U" + underLyingID.ToString();
        }

        public void AddCounterparties(string key, TTHelper tt)
        {
            try
            {
                string cpName = CachedDataManager.GetInstance.GetCounterPartyText(tt.CounterpartyID);
                string cpKey = key + "C" + tt.CounterpartyID;
                if (!counterPartyCollection.ContainsKey(key))
                {
                    Dictionary<int, string> dictCp = new Dictionary<int, string>();
                    dictCp.Add(tt.CounterpartyID, cpName);
                    counterPartyCollection.Add(key, dictCp);
                }
                else
                {
                    if (!counterPartyCollection[key].ContainsKey(tt.CounterpartyID))
                    {
                        counterPartyCollection[key].Add(tt.CounterpartyID, cpName);
                    }
                }
                AddVenues(cpKey, tt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddCounterpartiesQTT(string key, TTHelper tt)
        {
            try
            {
                string cpName = CachedDataManager.GetInstance.GetCounterPartyText(tt.CounterpartyID);
                string cpKey = key + "C" + tt.CounterpartyID;
                if (!counterPartyCollectionQTT.ContainsKey(key))
                {
                    Dictionary<int, string> dictCp = new Dictionary<int, string>();
                    dictCp.Add(tt.CounterpartyID, cpName);
                    counterPartyCollectionQTT.Add(key, dictCp);
                }
                else
                {
                    if (!counterPartyCollectionQTT[key].ContainsKey(tt.CounterpartyID))
                    {
                        counterPartyCollectionQTT[key].Add(tt.CounterpartyID, cpName);
                    }
                }
                AddVenuesQTT(cpKey, tt);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void AddVenues(string key, TTHelper tt)
        {
            try
            {
                string venueName = CachedDataManager.GetInstance.GetVenueText(tt.VenueID);

                if (!venueCollection.ContainsKey(key))
                {
                    Dictionary<int, string> dictVenue = new Dictionary<int, string>();
                    dictVenue.Add(tt.VenueID, venueName);
                    venueCollection.Add(key, dictVenue);
                }
                else
                {
                    if (!venueCollection[key].ContainsKey(tt.VenueID))
                    {
                        venueCollection[key].Add(tt.VenueID, venueName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddVenuesQTT(string key, TTHelper tt)
        {
            try
            {
                string venueName = CachedDataManager.GetInstance.GetVenueText(tt.VenueID);
                if (!venueCollectionQTT.ContainsKey(key))
                {
                    Dictionary<int, string> dictVenue = new Dictionary<int, string>();
                    dictVenue.Add(tt.VenueID, venueName);
                    venueCollectionQTT.Add(key, dictVenue);
                }
                else
                {
                    if (!venueCollectionQTT[key].ContainsKey(tt.VenueID))
                    {
                        venueCollectionQTT[key].Add(tt.VenueID, venueName);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This Method is to get mapped broker id with account id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public int GetCounterPartyByAccountId(int accountId)
        {
            try
            {
                if (FundWiseExecutingBrokerMapping.ContainsKey(accountId))
                {
                    return FundWiseExecutingBrokerMapping[accountId];
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }


        /// <summary>
        /// This Method is to get mapped broker id with account id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public int? GetCounterPartyByAccountId(string accountId)
        {
            try
            {
                if (string.IsNullOrEmpty(accountId))
                {
                    return null;
                }
                else
                {
                    return GetCounterPartyByAccountId(int.Parse(accountId));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;    
        }
    }
}
