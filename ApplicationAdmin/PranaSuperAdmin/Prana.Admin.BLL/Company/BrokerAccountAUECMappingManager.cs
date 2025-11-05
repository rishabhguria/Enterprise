using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Prana.Admin.BLL
{
    /// <summary>
    /// Class to provide business logic for the master fund-account mapping
    /// </summary>
    public static class BrokerAccountAUECMappingManager
    {
        /// <summary>
        /// check backup of Account Counterpary Mapping is initialized or not
        /// </summary>
        public static bool isbackInitialied = false;

        /// <summary>
        /// check backup of AUEC Counterpary Mapping is initialized or not
        /// </summary>
        public static bool isbackInitialiedAUEC = false;

        /// <summary>
        /// check backup of MasterFund is initialized or not      
        /// </summary>
        public static bool isbackUpInitialiedMasterFund = false;

        /// <summary>
        ///  Dictionary for  collection of accounts
        /// </summary>
        static Dictionary<int, string> _accountCollection = new Dictionary<int, string>();

        /// <summary>
        ///  collection of AUEC
        /// </summary>
        static AUECs _auecCollection = new AUECs();

        /// <summary>
        ///  collection of CounterParty
        /// </summary>
        static Dictionary<int, string> _CounterPartyCollection = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for  collection of accounts CounterParty Mapping
        /// </summary>
        static Dictionary<int, List<int>> _accountCounterPartyMapping = new Dictionary<int, List<int>>();

        /// <summary>
        /// Dictionary for  collection of AUEC Counterparty Mapping
        /// </summary>
        static Dictionary<int, List<int>> _auecCounterPartyMapping = new Dictionary<int, List<int>>();

		/// <summary>
        /// Dictionary for _counterParties
        /// </summary>
        static Dictionary<int, string> _counterParties  = new Dictionary<int, string>();

        static Dictionary<int, int> _fundWiseExecutingBrokerMapping = new Dictionary<int, int>();

        /// <summary>
        /// Get Company Counter Parties
        /// </summary>
		public static Dictionary<int, string> GetCompanyCounterParty()
        {
            return _counterParties;
        }
		/// <summary>
        /// Get Company Accounts
        /// </summary>
        public static Dictionary<int, string> GetAccounts()
        {
            return _accountCollection;
        }

        /// <summary>
        /// Get Fund Wise Executing Broker Mapping
        /// </summary>
        public static Dictionary<int, int> GetFundWiseExecutingBrokerMapping()
        {
            return _fundWiseExecutingBrokerMapping;
        }

        /// <summary>
        /// Get all Account names for a particular masterFund name
        /// </summary>
        /// <param name="masterFundName"> Selected masterFundName</param>
        /// <returns>list of AccountNames for selected master Fund Name</returns>
        public static List<String> GetAccountNamesForCounterParty(int counterPartyId)
        {
            List<String> accountNameList = new List<string>();
            try
            {
                if (counterPartyId == -1)
                    return null;

                if (_accountCounterPartyMapping.ContainsKey(counterPartyId))
                {
                    foreach (int accountId in _accountCounterPartyMapping[counterPartyId])
                    {
                        if (_accountCollection.ContainsKey(accountId))
                        {
                            accountNameList.Add(_accountCollection[accountId]);
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
            return accountNameList;
        }

        /// <summary>
        /// Get all Account names for a particular masterFund name
        /// </summary>
        /// <param name="masterFundName"> Selected masterFundName</param>
        /// <returns>list of AccountNames for selected master Fund Name</returns>
        public static List<String> GetAUECNamesForCounterParty(int counterPartyId)
        {
            List<String> auecNameList = new List<string>();
            try
            {
                if (counterPartyId == -1)
                    return null;

                if (_auecCounterPartyMapping.ContainsKey(counterPartyId))
                {
                    foreach (int auecId in _auecCounterPartyMapping[counterPartyId])
                    {
                        if (_auecCollection.ContainsAUEC(auecId))
                        {
                            auecNameList.Add(_auecCollection.GetAUECDetail(auecId));
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
            return auecNameList;
        }

        /// <summary>
        /// Get all Unassigned Account names from Mapping Dictionary _accountMasterFundMapping
        /// </summary>
        /// <param name="counterParty">The name of the master fund for which the unmapped accounts are to be bound</param>
        /// <returns>List of Unmapped Account name</returns>
        public static List<String> GetUnmappedAccounts(int counterPartyId)
        {
            List<String> unmappedAccounts = new List<string>();
            try
            {
                foreach (string accountName in _accountCollection.Values)
                {
                    if (counterPartyId > 0)
                    {
                        if (!GetAccountNamesForCounterParty(counterPartyId).Contains(accountName))
                            unmappedAccounts.Add(accountName);
                    }
                    else
                    {
                        unmappedAccounts.Add(accountName);
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
            unmappedAccounts.Sort();
            return unmappedAccounts;
        }

        /// <summary>
        /// Get all Unassigned AUEC names from Mapping Dictionary _auecMasterFundMapping
        /// </summary>
        /// <param name="counterParty">The name of the counterPartyId for which the unmapped AUEC are to be bound</param>
        /// <returns>List of Unmapped AUEC name</returns>
        public static List<String> GetUnmappedAUECs(int counterPartyId)
        {
            List<String> unmappedAUECs = new List<string>();
            try
            {
                foreach (AUEC auec in _auecCollection)
                {
                    if (counterPartyId > 0)
                    {
                        if (!GetAUECNamesForCounterParty(counterPartyId).Contains(auec.DisplayName))
                            unmappedAUECs.Add(auec.DisplayName);
                    }
                    else
                    {
                        unmappedAUECs.Add(auec.DisplayName);
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
            unmappedAUECs.Sort();
            return unmappedAUECs;
        }

        /// <summary>
        /// Get all Unassigned Account names from Mapping Dictionary _accountMasterFundMapping
        /// </summary>
        /// <returns>List of Unmapped Account name</returns>
        public static List<String> GetUnmappedAccountsForOnetoMany()
        {
            List<String> unmappedAccounts = new List<string>();
            try
            {
                foreach (int accountId in _accountCollection.Keys)
                {
                    // Check a account id is associated with a particular masterFund or not 
                    bool isFound = false;
                    foreach (List<int> accountList in _accountCounterPartyMapping.Values)
                    {
                        if (accountList.Contains(accountId))
                        {
                            isFound = true;
                        }
                    }
                    //if not true or not mapped then add this account id to unmappedAccount collection
                    if (!isFound)
                    {
                        unmappedAccounts.Add(_accountCollection[accountId]);
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
            unmappedAccounts.Sort();
            return unmappedAccounts;
        }

        /// <summary>
        /// Search the list for matching account names
        /// </summary>
        /// <param name="searchKeyword">The keyword entered in the textbox</param>
        /// <param name="list">List of items in the listbox</param>
        /// <returns>The list of matching items</returns>
        public static List<String> SerachForKeyword(String searchKeyword, List<String> list)
        {
            List<String> foundStrings = new List<string>();
            try
            {

                if (list != null)
                {
                    foreach (String key in list)
                    {
                        if (key.Trim().ToLower().Contains(searchKeyword.Trim().ToLower()))
                            foundStrings.Add(key);
                    }
                }
                else
                {
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
            return foundStrings;
        }

        #region MayRequiredinFuture
        ///// <summary>
        ///// Take Back Up of Account masterFund mapping Relation ship to reuse it 
        ///// </summary>
        //public static bool IsBackInitialied()
        //{

        //    if (isbackInitialied == false)
        //    {
        //        _backUpMappingCollection = new Dictionary<int, List<int>>(_accountCounterPartyMapping.Count, _accountCounterPartyMapping.Comparer);

        //        //making a deep copy of mapping object
        //        foreach (int masterFundId in _accountCounterPartyMapping.Keys)
        //        {
        //            _backUpMappingCollection.Add(masterFundId, new List<int>());
        //            foreach (int accountId in _accountCounterPartyMapping[masterFundId])
        //            {
        //                _backUpMappingCollection[masterFundId].Add(accountId);
        //            }
        //        }

        //        isbackInitialied = true;
        //        // return true;
        //    }
        //    //else
        //    //    return false;
        //    return isbackInitialied;

        //}

        ///// <summary>
        ///// Take Back Up of AUEC CounterParty mapping Relation ship to reuse it 
        ///// </summary>
        //public static bool IsBackInitialiedAUEC()
        //{

        //    if (isbackInitialiedAUEC == false)
        //    {
        //        _backUpMappingCollectionAUEC = new Dictionary<int, List<int>>(_auecCounterPartyMapping.Count, _auecCounterPartyMapping.Comparer);

        //        //making a deep copy of mapping object
        //        foreach (int counterPartyId in _auecCounterPartyMapping.Keys)
        //        {
        //            _backUpMappingCollectionAUEC.Add(counterPartyId, new List<int>());
        //            foreach (int auecId in _auecCounterPartyMapping[counterPartyId])
        //            {
        //                _backUpMappingCollectionAUEC[counterPartyId].Add(auecId);
        //            }
        //        }

        //        isbackInitialiedAUEC = true;
        //        // return true;
        //    }
        //    //else
        //    //    return false;
        //    return isbackInitialiedAUEC;

        //}

        #endregion

        /// <summary>
        /// Remove  account names  for Selected  masterFundName from mapping dictionary named as  _accountMasterFundMapping
        /// </summary>
        /// <param name="masterFundName">Selected MasterFund name</param>
        /// <param name="accountNames">list of assigned Account names to be unassigned</param>
        public static void UnassignAccounts(int counterPartyId, List<String> accountNames)
        {
            try
            {
                // initialied Back up 
                //  IsBackInitialied();
                //get masterFund Id from given masterFund name

                if (!_accountCounterPartyMapping.ContainsKey(counterPartyId))
                {
                    _accountCounterPartyMapping.Add(counterPartyId, new List<int>());
                }
                foreach (string fname in accountNames)
                {
                    // RemoveFromAccountMasterFundMapping()in which return true or false check back up() is exist then updatebackup() else  call backup() method as copy of dictionary of _accountMasterFundMapping
                    _accountCounterPartyMapping[counterPartyId].Remove(GetAccountIdByName(fname));
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
        /// Remove  AUEC names  for Selected  counterParty from mapping dictionary named as  _AUECCounterPartyMapping
        /// </summary>
        /// <param name="masterFundName">Selected counterParty name</param>
        /// <param name="auecNames">list of assigned AUEC names to be unassigned</param>
        public static void UnassignAUEC(int counterPartyId, List<String> auecNames)
        {
            try
            {
                // initialied Back up 
                // IsBackInitialied();
                //get masterFund Id from given masterFund name

                if (!_auecCounterPartyMapping.ContainsKey(counterPartyId))
                {
                    _auecCounterPartyMapping.Add(counterPartyId, new List<int>());
                }
                foreach (string name in auecNames)
                {
                    _auecCounterPartyMapping[counterPartyId].Remove(GetAUECIdByName(name));
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
        ///Add account names for Selected  masterFundName to mapping dictionary named as  _accountMasterFundMapping
        /// </summary>
        /// <param name="masterFundName">Slected masterFundName</param>
        /// <param name="accountNames">List of Accountnames to be assigned in Slected materAccount name</param>
        public static void AssignAccounts(int counterPartyId, List<String> accountNames)
        {
            try
            {
                //   IsBackInitialied();

                if (!_accountCounterPartyMapping.ContainsKey(counterPartyId))
                {
                    _accountCounterPartyMapping.Add(counterPartyId, new List<int>());
                }
                foreach (string fName in accountNames)
                {
                    //addIntoAccountMasterFundMApping() in which check copy() of dictionary is exist then update else take backup as copy of dictionay _accountMasterFundMapping
                    _accountCounterPartyMapping[counterPartyId].Add(GetAccountIdByName(fName));
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
        ///Add AUEC names for Selected  counterParty to mapping dictionary
        /// </summary>
        /// <param name="masterFundName">Slected counterParty</param>
        /// <param name="auecNames">List of AUEC to be assigned in Slected counterParty name</param>
        public static void AssignAUEC(int counterPartyId, List<String> auecNames)
        {
            try
            {
                //IsBackInitialied();

                if (!_auecCounterPartyMapping.ContainsKey(counterPartyId))
                {
                    _auecCounterPartyMapping.Add(counterPartyId, new List<int>());
                }
                foreach (string fName in auecNames)
                {
                    _auecCounterPartyMapping[counterPartyId].Add(GetAUECIdByName(fName));
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
        /// Get Account Id From name from _accountCollection dictionary
        /// </summary>
        /// <param name="accountName">Given Account name to get Account id </param>
        /// <returns>Account Id of Given Account Name</returns>
        private static int GetAccountIdByName(String accountName)
        {
            int accountId = -1;
            try
            {
                foreach (int key in _accountCollection.Keys)
                {
                    if (_accountCollection[key] == accountName)
                    {
                        accountId = key;
                        break;
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
            return accountId;
        }

        /// <summary>
        /// Get Account Id From name from _accountCollection dictionary
        /// </summary>
        /// <param name="accountName">Given Account name to get Account id </param>
        /// <returns>Account Id of Given Account Name</returns>
        private static int GetAUECIdByName(String auecName)
        {
            int auecId = -1;
            try
            {
                auecId = _auecCollection.GetAUECId(auecName);

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
            return auecId;
        }

		/// <summary>
        /// Save Fund wise executing broker mapping in DB
        /// </summary>
        public static void SaveExecutingBrokerMappping(int companyID, string executingBrokerMapping)
        {
            try
            {
                CounterPartyManager.SaveFundWiseExecutingBroker(executingBrokerMapping, companyID);
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

        public static void SaveBreakOrderPrefernce(int companyID, bool BreakOrderPrefernce)
        {
            try
            {
                CounterPartyManager.SaveBreakOrderPreference(companyID, BreakOrderPrefernce);
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
        /// Save the mapping and the master funds
        /// </summary>
        /// <param name="companyID">ID of the company</param>
        /// <returns>positive integer if the records are saved</returns>
        public static int SaveMapping(int companyID)
        {
            int result = -1;
            try
            {

                //saving Counterparty and Account mapping to database
                DataSet dsCVAccountMapping = GetDataSetFromDictionaryCVAccountMapping();
                String xmlDocCVAccountMapping = ConvertDataSetToXml(dsCVAccountMapping);
                result = CounterPartyManager.SaveCounterPartyAccountMapping(xmlDocCVAccountMapping, companyID);

                //saving Counterparty and AUECs mapping to database
                DataSet dsCVAUECMapping = GetDataSetFromDictionaryCVAAUEC();
                String xmlDocCVAUECMapping = ConvertDataSetToXml(dsCVAUECMapping);
                result = CounterPartyManager.SaveCounterPartyAUECMapping(xmlDocCVAUECMapping, companyID);


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
            return result;
        }

        /// <summary>
        /// Convert _accountCounterPartyMapping Dictionary Data to Data Set 
        /// </summary>
        /// <param name="_accountmasterFundMapping">Collection of mapping of Account masterFund name</param>
        /// <returns>Data Set which is collection of counterpartyIDs corresponding to Account Id</returns>
        private static DataSet GetDataSetFromDictionaryCVAccountMapping()
        {
            DataTable dt = new DataTable("TABCounterpartyAccountMapping");
            dt.Columns.Add("CounterPartyVenueID", typeof(int));
            dt.Columns.Add("AccountID", typeof(int));
            DataSet ds = new DataSet("DSCounterpartyAccountMapping");
            try
            {
                foreach (int cId in _accountCounterPartyMapping.Keys)
                {
                    foreach (int fId in _accountCounterPartyMapping[cId])
                    {
                        dt.Rows.Add(cId, fId);
                    }
                }
                ds.Tables.Add(dt);

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
            return ds;
        }

        /// <summary>
        /// Convert _auecCounterPartyMapping Dictionary Data to Data Set 
        /// </summary>
        /// <param name="_accountmasterFundMapping">Collection of mapping of Account auec name</param>
        /// <returns>Data Set which is collection of counterpartyId corresponding to Auec Id</returns>
        private static DataSet GetDataSetFromDictionaryCVAAUEC()
        {
            DataTable dt = new DataTable("TABCounterpartyAUECMapping");
            dt.Columns.Add("CounterPartyVenueID", typeof(int));
            dt.Columns.Add("AUECID", typeof(int));
            DataSet ds = new DataSet("DSCounterpartyAUECMapping");
            try
            {
                foreach (int cId in _auecCounterPartyMapping.Keys)
                {
                    foreach (int fId in _auecCounterPartyMapping[cId])
                    {
                        dt.Rows.Add(cId, fId);
                    }
                }
                ds.Tables.Add(dt);

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
            return ds;
        }

        /// <summary>
        /// Convert data set to Xml Document
        /// </summary>
        /// <param name="ds">Data set collection of AccountIDs and MasterFund Ids</param>
        /// <returns>xml documents</returns>
        private static string ConvertDataSetToXml(DataSet ds)
        {
            string xml = null;
            try
            {
                xml = ds.GetXml();
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
            return xml;
        }
        public static bool IntialiseBreakOrderPrefernce(int companyID)
        {
            bool BreakPrefernceFromDB = false;
            try
            {
                BreakPrefernceFromDB = CounterPartyManager.GetBreakOrderPrefernceFromDB(companyID);
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
            return BreakPrefernceFromDB;
        }
        /// <summary>
        /// Load all dta from data base 
        /// </summary>
        public static void InitialiseData(int companyID)
        {
            try
            {
                var counterPartyVeneus = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
                var counterParties = CounterPartyManager.GetCompanyCounterParties(companyID);

                IEnumerable<CounterPartyVenue> _counterPartyVeneusList = counterPartyVeneus.Cast<CounterPartyVenue>();
                IEnumerable<CounterParty> _counterPartyList = counterParties.Cast<CounterParty>();

                _CounterPartyCollection.Clear();
                _counterParties.Clear();
                //joining the counterparty venue full name and counterparty name
                var counterPartyCollection = _counterPartyVeneusList.Join(_counterPartyList, x => x.CounterPartyID, y => y.CounterPartyID,
                      (x, y) => new { X = x, Y = y }).ToDictionary(x => x.X.CounterPartyVenueID, y => y.Y.CounterPartyFullName + "/" + y.X.DisplayName);

                var sortedDict = counterPartyCollection.OrderBy(x => x.Value).ToDictionary(x => x.Key, y => y.Value);

                _CounterPartyCollection.Add(0, "-Select-");
                foreach (var item in sortedDict)
                {
                    _CounterPartyCollection.Add(item.Key, item.Value);
                }

                foreach (var item in _counterPartyList) {
                    _counterParties.Add(item.CounterPartyID, item.ShortName);
                 }
                _accountCollection = MasterFundMappingDAL.LoadAccountsFromDb(companyID);
                //Load all Account Counterparty  mapping from data base
                _accountCounterPartyMapping = CounterPartyManager.GetCounterPartyAccountMappingFromDb(companyID);

                _fundWiseExecutingBrokerMapping = CounterPartyManager.GetFundWiseExecutingBrokerMappingFromDB(companyID);

                //Load all AUEC From Data base
                _auecCollection = AUECManager.GetCompanyAUEC(companyID);
                //Load all AUEC and Counterparty  mapping from data base
                _auecCounterPartyMapping = CounterPartyManager.GetCounterPartyAuecMappingFromDb(companyID);

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
        /// Get Company Counter Parties
        /// </summary>
        public static Dictionary<int, string> GetCompanyCounterParties()
        {
            return _CounterPartyCollection;
        }
    }
}
