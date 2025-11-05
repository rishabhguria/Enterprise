//Created by:Bharat Raturi, Date: 02/13/2014
//Purpose: Business Logic Layer to show the Strategy-master strategy mapping
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Admin.BLL
{
    public class StrategyMasterStrategyMappingManager
    {
        /// <summary>
        /// check whether the backup of strategy to master strategy mapping is initialized or not
        /// </summary>
        public static bool isBackUpOn = false;

        /// <summary>
        /// check whether the backup of Masterstrategy is initialized or not      
        /// </summary>
        public static bool isBackUpMasterStrategyOn = false;

        /// <summary>
        /// Set default master strategy name to NewM_Str
        /// </summary>
        static String defaultMasterStrategyName = "NewMasterStrategy";

        /// <summary>
        /// Dictionary to take back up or collection of master strategies
        /// </summary>
        static Dictionary<int, String> _backUpMasterStrategyCollection;

        /// <summary>
        ///Dictionary to take back up of the collection of strategy to master strategy mapping.
        ///</summary>
        static Dictionary<int, List<int>> _backUpMappingCollection;

        /// <summary>
        ///  Dictionary for collection of strategys
        /// </summary>
        public static Dictionary<int, string> _strategyCollection = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for  collection of Masterstrategys
        /// </summary>
        static Dictionary<int, string> _masterStrategyCollection = new Dictionary<int, string>();

        /// <summary>
        /// Dictionary for  collection of strategysMasterstrategyMapping
        /// </summary>
        static Dictionary<int, List<int>> _strategyMasterStrategyMapping = new Dictionary<int, List<int>>();

        /// <summary>
        ///  Dictionary for  collection of master strategy status
        /// </summary>
        static Dictionary<int, int> _masterStrategyStatus = new Dictionary<int, int>();

        /// <summary>
        ///  Dictionary for  collection of master strategy delete status
        /// </summary>
        static Dictionary<int, int> _masterStrategyDelStatus = new Dictionary<int, int>();

        /// <summary>
        /// Dictionary to take back up or collection of master strategy for Audit.
        /// 
        static Dictionary<int, String> _backUpMasterForAudit;

        //Added By Faisal Shah
        //Dated 02/07/14
        //Needed to get the maxID on Saving rather than on initialization
        static int _maxMasterStrategyID;

        static bool _isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();

        static int newId = -2;

        static Dictionary<int, List<int>> _masterStrategyAddUpdateStatus = new Dictionary<int, List<int>>();

        /// <summary>
        /// method for get all masterstrategyName List from __masterstrategyCollection dictionary and return list of master strategy Name
        /// </summary>
        /// <returns>list of masterstrategyName</returns>
        public static List<String> GetAllMasterStrategyName()
        {
            List<String> masterStrategyNames = new List<string>();
            try
            {
                foreach (String name in _masterStrategyCollection.Values)
                {
                    masterStrategyNames.Add(name);
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
            masterStrategyNames.Sort();
            return masterStrategyNames;
        }

        /// <summary>
        /// Get all strategy names for a particular masterstrategy name
        /// </summary>
        /// <param name="masterstrategyName"> Selected master strategy Name</param>
        /// <returns>list of strategyNames for selected master strategy Name</returns>
        public static List<String> GetStrategyNamesForMasterStrategy(String masterStrategyName)
        {
            List<String> strategyNameList = new List<string>();
            try
            {
                int masterStrategyId = GetMasterStrategyIdByName(masterStrategyName);

                if (masterStrategyId == -1)
                {
                    return null;
                }

                if (_strategyMasterStrategyMapping.ContainsKey(masterStrategyId))
                {
                    foreach (int strategyId in _strategyMasterStrategyMapping[masterStrategyId])
                    {
                        if (_strategyCollection.ContainsKey(strategyId))
                        {
                            strategyNameList.Add(_strategyCollection[strategyId]);
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
            strategyNameList.Sort();
            return strategyNameList;
        }

        /// <summary>
        /// Get all Unassigned strategy names from Mapping Dictionary _strategyMasterstrategyMapping
        /// </summary>
        /// <returns>List of Unmapped strategy names</returns>
        public static List<String> GetUnmappedStrategies(string masterStrategy)
        {
            List<String> unmappedStrategies = new List<string>();
            try
            {
                if (_strategyCollection.Keys.Count > 0)
                {
                    if (string.IsNullOrEmpty(masterStrategy))
                    {
                        foreach (string strategyName in _strategyCollection.Values)
                        {
                            unmappedStrategies.Add(strategyName);
                        }
                    }
                    else
                    {
                        //if a strategy is not assigned to the selected master fund
                        //show it in the list of strategies available for mapping
                        foreach (string strategyName in _strategyCollection.Values)
                        {
                            if (!GetStrategyNamesForMasterStrategy(masterStrategy).Contains(strategyName))
                                unmappedStrategies.Add(strategyName);
                        }
                    }
                    unmappedStrategies.Sort();
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
            return unmappedStrategies;
        }

        //added by: Bharat raturi, 22 apr 2014
        //purpose: Load the mapping if one to many mapping is allowed
        /// <summary>
        /// Get all Unassigned Account names from Mapping Dictionary _accountMasterFundMapping
        /// </summary>
        /// <returns>List of Unmapped Account name</returns>
        public static List<String> GetUnmappedStrategiesForOnetoMany()
        {
            List<String> unmappedStrategies = new List<string>();
            try
            {
                foreach (int strategyID in _strategyCollection.Keys)
                {
                    // Check a account id is associated with a particular masterFund or not 
                    bool isFound = false;
                    foreach (List<int> strategyList in _strategyMasterStrategyMapping.Values)
                    {
                        if (strategyList.Contains(strategyID))
                        {
                            isFound = true;
                        }
                    }
                    //if not true or not mapped then add this account id to unmappedAccount collection
                    if (!isFound)
                    {
                        unmappedStrategies.Add(_strategyCollection[strategyID]);
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
            unmappedStrategies.Sort();
            return unmappedStrategies;
        }


        /// <summary>
        /// Filter the strategies according to the keyword
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <param name="list"></param>
        /// <returns>the list of strategies mathcing the keyword</returns>
        public static List<String> SearchForKeyword(String searchKeyword, List<String> list)
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
            foundStrings.Sort();
            return foundStrings;
        }

        /// <summary>
        /// Take Back Up of strategy masterstrategy mapping Rleation ship to reuse it 
        /// </summary>
        public static bool IsBackUpInitialized()
        {

            if (isBackUpOn == false)
            {
                _backUpMappingCollection = new Dictionary<int, List<int>>(_strategyMasterStrategyMapping.Count, _strategyMasterStrategyMapping.Comparer);

                //making a deep copy of mapping object
                foreach (int masterStrategyId in _strategyMasterStrategyMapping.Keys)
                {
                    _backUpMappingCollection.Add(masterStrategyId, new List<int>());
                    foreach (int StrategyId in _strategyMasterStrategyMapping[masterStrategyId])
                    {
                        _backUpMappingCollection[masterStrategyId].Add(StrategyId);
                    }
                }
                isBackUpOn = true;
            }
            return isBackUpOn;
        }

        /// <summary>
        /// Restore strategyMaster strategy Mapping Dictionary from back up of _strategyMasterstrategyMapping named as _backUpMappingCollection 
        /// </summary>
        public static void RestoreBackUp()
        {
            if (isBackUpOn == true)
            {
                _strategyMasterStrategyMapping = _backUpMappingCollection;
                CleanBackUp();
            }
        }

        /// <summary>
        ///deep copy of _strategyMasterstrategyMapping is set null named as  _backUpMappingCollection and set  isbackInitialied false 
        /// </summary>
        public static void CleanBackUp()
        {
            _backUpMappingCollection = null;
            isBackUpOn = false;
        }

        /// <summary>
        /// return true or false given master strategy name is already exist or not
        /// </summary>
        /// <param name="list">collelction of masterstrategy name</param>
        /// <returns>true if exist else false</returns>
        public static bool IsMasterStrategyNameExist(params String[] list)
        {
            bool isexistNewname = false;
            List<String> tempList = new List<string>();
            foreach (String val in _masterStrategyCollection.Values)
                tempList.Add(val);
            tempList.Sort();
            int res = tempList.BinarySearch(list[0], StringComparer.OrdinalIgnoreCase);
            if (res >= 0)
            {
                isexistNewname = true;
            }
            return isexistNewname;
        }

        /// <summary>
        /// Save masterstrategy in dictionary collection wnem it delete rename or add
        /// </summary>
        /// <param name="id">0-add,1-rename,2-delete</param>
        /// <param name="newName"></param>
        /// <param name="list">1 name always new name and then all old name </param>
        public static void ManageMasterStrategy(int id, params String[] list)
        {
            try
            {
                if (!isBackUpMasterStrategyOn)
                {
                    isBackUpMasterStrategyOn = IsBackInitializedMasterStrategyCollection();
                }
                if (!isBackUpOn)
                {
                    isBackUpOn = IsBackUpInitialized();
                }
                if (id == 0)
                {
                    // newId value to be taken as Max strategy ID 
                    //Modified by Faisal Shah on 02/07/14
                    #region CommentedCode
                    //_maxMasterStrategyID += 1;
                    //List<int> idList = new List<int>(_masterStrategyCollection.Keys);
                    //if (idList.Count != 0)
                    //{
                    //    idList.Sort();
                    //    newId = idList[_masterStrategyCollection.Keys.Count - 1] + 1;
                    //}
                    #endregion
                    _masterStrategyCollection.Add(newId, list[0]);
                    newId--;
                }
                else
                {
                    if (id == 1)
                    {
                        //Modified By Faisal Shah
                        //Dated 03/07/14
                        //Adding key value pairs in Dictionary for Addition Updation and deletion
                        int mId = GetMasterStrategyIdByName(list[1]);
                        _masterStrategyCollection[mId] = list[0];

                        if (mId > 0)
                        {
                            if (!_masterStrategyAddUpdateStatus.ContainsKey(1))
                            {
                                _masterStrategyAddUpdateStatus.Add(1, new List<int>());
                                _masterStrategyAddUpdateStatus[1].Add(mId);
                            }
                            else
                                _masterStrategyAddUpdateStatus[1].Add(mId);
                        }

                    }
                    else
                        if (id == 2)
                    {
                        int mId = GetMasterStrategyIdByName(list[1]);
                        if (_strategyMasterStrategyMapping.ContainsKey(mId))
                        {
                            _strategyMasterStrategyMapping.Remove(mId);
                        }
                        if (!_masterStrategyAddUpdateStatus.ContainsKey(2))
                        {
                            _masterStrategyAddUpdateStatus.Add(2, new List<int>());
                            _masterStrategyAddUpdateStatus[2].Add(mId);
                        }
                        else
                            _masterStrategyAddUpdateStatus[2].Add(mId);
                        _masterStrategyCollection.Remove(mId);
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
        /// return true if back up of master strategy collection is intialized
        /// </summary>
        /// <returns>true  or false </returns>
        public static bool IsBackInitializedMasterStrategyCollection()
        {
            bool result = false;
            try
            {
                if (isBackUpMasterStrategyOn == false)
                {
                    _backUpMasterStrategyCollection = new Dictionary<int, String>(_masterStrategyCollection.Count, _masterStrategyCollection.Comparer);
                    //making a deep copy of mapping object
                    foreach (int key in _masterStrategyCollection.Keys)
                    {
                        _backUpMasterStrategyCollection.Add(key, _masterStrategyCollection[key]);
                    }
                    isBackUpMasterStrategyOn = true;
                    result = true;
                }
                else
                    result = false;
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
        /// Restore strategyMaster strategy Mapping Dictionary from back up of _strategyMasterstrategyMapping named as _backUpMappingCollection 
        /// </summary>
        public static void RestoreBackUpMasterStrategy()
        {
            try
            {
                if (isBackUpMasterStrategyOn == true)
                {
                    _masterStrategyCollection = _backUpMasterStrategyCollection;
                    CleanBackUpMasterStrategy();
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
        ///deep copy of _strategyMasterstrategyMapping is set null named as  _backUpMappingCollection and set  isbackInitialied false 
        /// </summary>
        public static void CleanBackUpMasterStrategy()
        {
            try
            {
                _backUpMasterStrategyCollection = null;
                isBackUpMasterStrategyOn = false;
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
        /// Remove  strategy names  for Selected  masterstrategyName from mapping dictionary named as  _strategyMasterstrategyMapping
        /// </summary>
        /// <param name="masterstrategyName">Slected Masterstrategy name</param>
        /// <param name="strategyNames">list of assigned strategy names to be unassigned</param>
        public static void UnassignStrategies(String masterStrategyName, List<String> strategyNames)
        {
            try
            {
                // initialied Back up 
                IsBackUpInitialized();
                //get masterstrategy Id from given masterstrategy name
                int masterStrategyId = GetMasterStrategyIdByName(masterStrategyName);
                if (!_strategyMasterStrategyMapping.ContainsKey(masterStrategyId))
                {
                    _strategyMasterStrategyMapping.Add(masterStrategyId, new List<int>());
                }
                foreach (string strName in strategyNames)
                {
                    // RemoveFromstrategyMasterstrategyMapping()in which return true or false check back up() is exist then updatebackup() else  call backup() method as copy of dictionary of _strategyMasterstrategyMapping
                    _strategyMasterStrategyMapping[masterStrategyId].Remove(GetStrategyIdByName(strName));
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
        ///Add strategy names  for Selected  masterstrategyName to mapping dictionary named as  _strategyMasterstrategyMapping
        /// </summary>
        /// <param name="masterstrategyName">Slected masterstrategyName</param>
        /// <param name="strategyNames">List of strategynames to be assigned in Slected materstrategy name</param>
        public static void AssignStrategies(String masterStrategyName, List<String> strategyNames)
        {
            try
            {
                IsBackUpInitialized();
                int masterStrategyId = GetMasterStrategyIdByName(masterStrategyName);
                if (!_strategyMasterStrategyMapping.ContainsKey(masterStrategyId))
                {
                    _strategyMasterStrategyMapping.Add(masterStrategyId, new List<int>());
                }
                foreach (string strName in strategyNames)
                {
                    //addIntostrategyMasterstrategyMApping() in which check copy() of dictiory is exist then update else take backup as copy of dictionay _strategyMasterstrategyMapping
                    _strategyMasterStrategyMapping[masterStrategyId].Add(GetStrategyIdByName(strName));
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
        /// Get strategy Id From name from _strategyCollection dictionary
        /// </summary>
        /// <param name="strategyName">Given strategy name to get strategy id </param>
        /// <returns>strategy Id of Given strategy Name</returns>
        private static int GetStrategyIdByName(String strategyName)
        {
            int strategyId = -1;
            try
            {
                foreach (int key in _strategyCollection.Keys)
                {
                    if (_strategyCollection[key] == strategyName)
                    {
                        strategyId = key;
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
            return strategyId;
        }

        /// <summary>
        /// Get masterstrategy Id From name from _masterstrategyCollection dictionary
        /// </summary>
        /// <param name="strategyName">Given Masterstrategy name to get Masterstrategy id </param>
        /// <returns>Masterstrategy Id of Given Masterstrategy Name</returns>
        public static int GetMasterStrategyIdByName(String masterStrategyName)
        {
            int masterStrategyId = -1;
            try
            {
                if (masterStrategyName == null)
                {
                    return masterStrategyId;
                }
                foreach (int key in _masterStrategyCollection.Keys)
                {
                    if (_masterStrategyCollection[key] == masterStrategyName)
                    {
                        masterStrategyId = key;
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
            return masterStrategyId;
        }

        /// <summary>
        /// Save the mapping in the database
        /// </summary>
        /// <returns>The number of affected records</returns>
        public static int SaveMapping(int companyID)
        {
            int r = -1;
            _masterStrategyStatus.Clear();
            _masterStrategyDelStatus.Clear();
            try
            {
                //Modified By faisal Gani Shah
                //Dated 02/07/2014
                //Assingning MaxID from DB here rather than on initialization.

                _maxMasterStrategyID = MasterStrategyMappingDAL.GetNewMasterStrategyID();
                for (int NewKey = newId + 1; NewKey <= -2; NewKey++)
                {

                    if (_masterStrategyCollection.Keys.Contains(NewKey))
                    {
                        _masterStrategyCollection.Add(_maxMasterStrategyID, _masterStrategyCollection[NewKey]);
                        _masterStrategyCollection.Remove(NewKey);
                        if (!_masterStrategyAddUpdateStatus.ContainsKey(0))
                        {
                            _masterStrategyAddUpdateStatus.Add(0, new List<int>());
                            _masterStrategyAddUpdateStatus[0].Add(_maxMasterStrategyID);
                        }
                        else
                            _masterStrategyAddUpdateStatus[0].Add(_maxMasterStrategyID);
                        //Added By Faisal Shah on 18/07/14
                        if (_strategyMasterStrategyMapping.Keys.Contains(NewKey))
                        {
                            if (!_strategyMasterStrategyMapping.ContainsKey(_maxMasterStrategyID))
                            {
                                _strategyMasterStrategyMapping.Add(_maxMasterStrategyID, _strategyMasterStrategyMapping[NewKey]);
                                _strategyMasterStrategyMapping.Remove(NewKey);
                            }
                        }
                        _maxMasterStrategyID++;
                    }
                    else
                    { }
                }
                DataSet dsMapping = GetDataSetFromDictionary(_strategyMasterStrategyMapping);
                String xmlDoc = ConvertDataSetToXml(dsMapping);
                //Modified By Faisal Shah 06/08/14
                //Added CH Release as Parameter to execute separate Code for two releases        
                DataSet dsMasterStrategy = GetDataSetFromMasterStrategyDictionary(_masterStrategyCollection, companyID);
                string xml = ConvertDataSetToXml(dsMasterStrategy);
                _masterStrategyAddUpdateStatus.Clear();

                r = MasterStrategyMappingDAL.SaveDataSetInDB(xmlDoc, xml, companyID, _isPermanentDeletion);
                // For audit trail

                if (_backUpMasterForAudit != null)
                {
                    foreach (int key in _backUpMasterForAudit.Keys)
                    {
                        if (!_masterStrategyDelStatus.ContainsKey(key))
                        {
                            if (!_masterStrategyCollection.ContainsKey(key))
                            {
                                // status is set to 2 for delete
                                _masterStrategyDelStatus.Add(key, 2);
                            }
                            // if deletion and insertion on same key
                            else if (_masterStrategyCollection.ContainsKey(key) && _masterStrategyCollection[key] != _backUpMasterForAudit[key])
                            {
                                _masterStrategyDelStatus.Add(key, 2);
                            }
                        }
                    }
                }
                if (_masterStrategyStatus != null)
                {
                    foreach (int key in _masterStrategyCollection.Keys)
                    {
                        if (!_masterStrategyStatus.ContainsKey(key))
                        {
                            if (_backUpMasterForAudit.ContainsKey(key) && _masterStrategyCollection[key] == _backUpMasterForAudit[key])
                            {
                                // status is set to 3 for update
                                _masterStrategyStatus.Add(key, 3);
                            }
                            else
                            {
                                // status is set to 1 for create
                                _masterStrategyStatus.Add(key, 1);
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
            return r;
        }

        /// <summary>
        /// dictionary data is converted into data set
        /// </summary>
        /// <param name="_masterstrategyCollection">dictionary of master strategy name and id</param>
        /// <returns>data set </returns>
        private static DataSet GetDataSetFromMasterStrategyDictionary(Dictionary<int, string> _masterStrategyCollection, int companyID)
        {
            DataTable dt = new DataTable("Table1");
            DataSet ds = new DataSet("NewDataSet");
            try
            {
                dt.Columns.Add("CompanyMasterStrategyID", typeof(int));
                dt.Columns.Add("MasterStrategyName", typeof(string));
                dt.Columns.Add("CompanyId", typeof(int));
                //Added By Faisal Shah
                //Dated 03/07/14
                //Purpose to identify the update delete or addition entries
                //Modified By Faisal Shah 06/08/14
                //CH logic and Prana Logic Separation due to additional Column IsActive in CH Release.

                if (!_isPermanentDeletion)
                {
                    dt.Columns.Add("QueryType", typeof(int));
                    //foreach (int key in _masterStrategyCollection.Keys)
                    //    dt.Rows.Add(key, _masterStrategyCollection[key], companyID);
                    //ds.Tables.Add(dt);
                    foreach (KeyValuePair<int, List<int>> kvp in _masterStrategyAddUpdateStatus)
                    {
                        if (kvp.Key == 0 || kvp.Key == 1)
                        {
                            foreach (int keyforMaster in kvp.Value)
                            {
                                dt.Rows.Add(keyforMaster, _masterStrategyCollection[keyforMaster], companyID, kvp.Key);

                            }
                        }
                        else if (kvp.Key == 2)
                        {
                            foreach (int keyforMaster in kvp.Value)
                            {
                                dt.Rows.Add(keyforMaster, "ToBeDeleted", companyID, kvp.Key);
                            }
                        }
                    }
                    ds.Tables.Add(dt);
                }
                else
                {
                    foreach (int key in _masterStrategyCollection.Keys)
                        dt.Rows.Add(key, _masterStrategyCollection[key], companyID);
                    ds.Tables.Add(dt);
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
            return ds;
        }

        /// <summary>
        /// Convert _strategymasterstrategyMapping Dictionary Data to Data Set 
        /// </summary>
        /// <param name="_strategymasterstrategyMapping">Collection of mapping of strategy masterstrategy name</param>
        /// <returns>Data Set which is collection of strategyIds corresponding to masterstrategy Id</returns>
        private static DataSet GetDataSetFromDictionary(Dictionary<int, List<int>> _strategyMasterStrategyMapping)
        {
            DataTable dt = new DataTable("TABStrategyMasterStrategyMapping");
            dt.Columns.Add("CompanyMasterStrategyId", typeof(int));
            dt.Columns.Add("CompanyStrategyId", typeof(int));
            DataSet ds = new DataSet("DSStrategyMasterStrategyMapping");
            try
            {
                foreach (int mId in _strategyMasterStrategyMapping.Keys)
                {
                    foreach (int fId in _strategyMasterStrategyMapping[mId])
                    {
                        dt.Rows.Add(mId, fId);
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
        /// <param name="ds">Data set collection of strategyIDs and Masterstrategy Ids</param>
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

        /// <summary>
        /// Load all data from data base 
        /// </summary>
        public static void InitializeData(int companyID)
        {
            try
            {
                if (_masterStrategyCollection.Keys.Count > 0)
                {
                    _masterStrategyCollection.Clear();
                }
                if (_strategyCollection.Keys.Count > 0)
                {
                    _strategyCollection.Clear();
                }
                if (_strategyMasterStrategyMapping.Keys.Count > 0)
                {
                    _strategyMasterStrategyMapping.Clear();
                }
                _masterStrategyCollection = MasterStrategyMappingDAL.LoadMasterStrategyFromDb(companyID);
                //Load all strategy From Database
                _strategyCollection = MasterStrategyMappingDAL.LoadStrategyFromDb(companyID);
                //Load all strategy Masterstrategy  mapping from data base
                _strategyMasterStrategyMapping = MasterStrategyMappingDAL.LoadStrategyMasterStrategyMappingFromDb(companyID);
                // For audit
                _backUpMasterForAudit = MasterStrategyMappingDAL.LoadMasterStrategyFromDb(companyID);

                //Modified By Faisal Shah on 01/07/14
                // _maxMasterStrategyID = MasterStrategyMappingDAL.GetNewMasterStrategyID();
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
        /// genetae new name for master strategy
        /// </summary>
        /// <returns>new name for master strategy</returns>
        public static String GetRuntimeMasterStrategyName()
        {
            String returnMasterStrategyNameValue = defaultMasterStrategyName;
            try
            {
                if (_masterStrategyCollection.ContainsValue(defaultMasterStrategyName))
                {
                    bool breakLoop = false;
                    int tempAppendKey = 1;
                    while (!breakLoop)
                    {
                        if (!_masterStrategyCollection.ContainsValue(defaultMasterStrategyName + tempAppendKey))
                        {
                            returnMasterStrategyNameValue = defaultMasterStrategyName + tempAppendKey;
                            breakLoop = true;
                        }
                        else
                        {
                            tempAppendKey++;
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
            return returnMasterStrategyNameValue;
        }

        /// <summary>
        /// Check if there is one to many mapping doen previously
        /// </summary>
        /// <returns>true if many to many mapping, false if one to many mapping</returns>
        public static bool IsManyToManyMapping()
        {
            List<int> tempStrategyList = new List<int>();
            foreach (int masterFundID in _strategyMasterStrategyMapping.Keys)
            {
                tempStrategyList.AddRange(_strategyMasterStrategyMapping[masterFundID]);
            }
            tempStrategyList.Sort();
            if (tempStrategyList.Distinct().Count() != tempStrategyList.Count)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get status dictionary for audit
        /// </summary>
        /// <returns>dictionary for auditStatus of master strategy</returns>
        public static Dictionary<int, int> GetStatusForAudit()
        {
            try
            {
                return _masterStrategyStatus;
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
            return _masterStrategyStatus;
        }

        /// <summary>
        /// Get status dictionary for audit delete
        /// </summary>
        /// <returns>dictionary for auditStatus for deletion in master strategy</returns>
        public static Dictionary<int, int> GetStatusForAuditDel()
        {
            try
            {
                return _masterStrategyDelStatus;
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
            return _masterStrategyDelStatus;
        }
    }
}
