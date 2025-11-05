using Infragistics.Win;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.ClientCommon
{
    /// <summary>
    /// Summary description for MTTHelperManager.
    /// </summary>
    public class MTTHelperManager
    {
        Dictionary<string, Dictionary<int, string>> venueCollection = new Dictionary<string, Dictionary<int, string>>();

        Dictionary<int, List<int>> counterPartyAccountCollection = new Dictionary<int, List<int>>();
        Dictionary<string, ValueList> counterPartyValueListCollection = new Dictionary<string, ValueList>();
        Dictionary<string, ValueList> venueValueListCollection = new Dictionary<string, ValueList>();
        Dictionary<int, string> dictCounterPartyCollection = new Dictionary<int, string>();
        Dictionary<int, string> dictVenueCollection = new Dictionary<int, string>();

        TTHelperCollection _ttCollection = new TTHelperCollection();
        int _userID = int.MinValue;
        private MTTHelperManager()
        {
        }

        private static readonly MTTHelperManager _ttManager = new MTTHelperManager();
        public static MTTHelperManager GetInstance()
        {
            return _ttManager;
        }

        /// <summary>
        /// Set Helper Collection
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public void SetHelperCollection(int CompanyID, int UserID)
        {
            if (UserID != _userID)
            {
                GetCVAUECMappings(CompanyID, UserID);
                GetCVAccountMappings(CompanyID);
                CreateDictionaries();
                _userID = UserID;
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
                                if (!existingList.Contains(counterPartyId))
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

        public TTHelper FillTTHelper(object[] row, int offset)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="underLyingID"></param>
        /// <returns></returns>
        public ValueList GetCounterparties(int auecID)
        {
            if (counterPartyValueListCollection.ContainsKey(AUECKey(auecID)))
            {
                return counterPartyValueListCollection[AUECKey(auecID)];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method is used in the work area preferences
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetCounterparties()
        {
            return dictCounterPartyCollection;
        }

        private Dictionary<int, ValueList> cpWiseVenues = new Dictionary<int, ValueList>();
        /// <summary>
        /// Get Venues By Counter Party ID 
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <param name="auecId"></param>
        /// <returns></returns>
        public ValueList GetVenuesByCounterPartyID(int counterPartyID)
        {
            try
            {
                if (!cpWiseVenues.ContainsKey(counterPartyID))
                {
                    var key = AUECCounterPartyKey(counterPartyID);
                    ValueList vl = new ValueList();

                    //This hashset is created so that it contains unique values as venueCollection contains duplicate values
                    HashSet<int> set = new HashSet<int>();
                    foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in venueCollection)
                    {
                        if (keyvaluepair.Key.EndsWith(key))
                        {
                            foreach (KeyValuePair<int, string> venue in keyvaluepair.Value)
                            {
                                if (set.Add(venue.Key))
                                    vl.ValueListItems.Add(venue.Key, venue.Value);

                            }
                        }
                    }
                    cpWiseVenues.Add(counterPartyID, vl);
                }
                return cpWiseVenues[counterPartyID];
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Get Venues By Counter Party ID and AUEC
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <param name="auecId"></param>
        /// <returns></returns>
        public ValueList GetVenuesByCounterPartyID(int counterPartyID, int auecId)
        {
            try
            {
                var key = AUECCouterPartyKey(auecId, counterPartyID);
                if (venueValueListCollection.ContainsKey(key))
                    return venueValueListCollection[key];
                else
                    return new ValueList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used in the work area preferences
        /// </summary>
        /// <returns></returns>
        public ValueList GetVenues()
        {
            ValueList vl = new ValueList();
            foreach (KeyValuePair<int, string> venue in dictVenueCollection)
            {
                vl.ValueListItems.Add(venue.Key, venue.Value);
            }
            return vl;
        }

        private void CreateDictionaries()
        {
            try
            {
                Dictionary<string, Dictionary<int, string>> counterPartyCollection = new Dictionary<string, Dictionary<int, string>>();
                counterPartyValueListCollection.Clear();
                venueValueListCollection.Clear();
                dictVenueCollection.Clear();
                dictCounterPartyCollection.Clear();

                #region CounterParties based on AUEC
                foreach (TTHelper tt in _ttCollection)
                {
                    string key = AUECKey(tt.AuecID);
                    AddCounterparties(key, tt, counterPartyCollection);
                }
                foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in counterPartyCollection)
                {
                    ValueList vl = new ValueList();
                    foreach (KeyValuePair<int, string> cp in keyvaluepair.Value)
                    {
                        vl.ValueListItems.Add(cp.Key, cp.Value);

                        //Fill all the counter parties in the dictionary
                        if (!dictCounterPartyCollection.ContainsKey(cp.Key))
                        {
                            dictCounterPartyCollection.Add(cp.Key, cp.Value);
                        }
                    }
                    counterPartyValueListCollection.Add(keyvaluepair.Key, vl);
                }
                foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in venueCollection)
                {
                    ValueList vl = new ValueList();
                    foreach (KeyValuePair<int, string> venue in keyvaluepair.Value)
                    {
                        vl.ValueListItems.Add(venue.Key, venue.Value);
                        //Fill all the venues in the dictionary
                        if (!dictVenueCollection.ContainsKey(venue.Key))
                        {
                            dictVenueCollection.Add(venue.Key, venue.Value);
                        }
                    }
                    venueValueListCollection.Add(keyvaluepair.Key, vl);
                }
                #endregion
                _ttCollection.Clear();

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
        private void AddCounterparties(string key, TTHelper tt, Dictionary<string, Dictionary<int, string>> counterPartyCollection)
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
        private void AddVenues(string key, TTHelper tt)
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

        private string AUECKey(int auecId)
        {
            return "AUEC" + auecId;
        }

        private string AUECCouterPartyKey(int auecId, int counterPartyID)
        {
            return "AUEC" + auecId + "C" + counterPartyID;
        }

        private string AUECCounterPartyKey(int counterPartyID)
        {
            return "C" + counterPartyID;
        }

        /// <summary>
        /// Get Counter parties Filter By Account and AUEC
        /// </summary>
        /// <param name="accountIds"></param>
        /// <param name="cpList"></param>
        /// <returns></returns>
        public ValueList GetCounterpartiesFilterByAccount(int accountId, ValueList cpList)
        {
            ValueList cpListFinal = new ValueList();
            try
            {
                var accountsCPs = counterPartyAccountCollection.FirstOrDefault(x => accountId == x.Key).Value;
                if (accountsCPs != null)
                {
                    foreach (var item in cpList.ValueListItems)
                    {
                        if (accountsCPs.Contains((int)item.DataValue))
                        {
                            cpListFinal.ValueListItems.Add((int)item.DataValue, item.DisplayText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return cpListFinal;
        }
    }

}

