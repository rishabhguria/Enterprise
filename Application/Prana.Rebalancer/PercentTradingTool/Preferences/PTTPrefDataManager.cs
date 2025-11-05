using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Rebalancer.PercentTradingTool.Preferences
{
    internal class PTTPrefDataManager
    {
        #region singleton instance
        /// <summary>
        /// The PTT preference data manager object
        /// </summary>
        private static PTTPrefDataManager _pttPrefDataManager = null;
        static object _locker = new object();
        /// <summary>
        /// Gets the Preference instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        public static PTTPrefDataManager GetInstance
        {
            get
            {
                if (_pttPrefDataManager == null)
                {
                    lock (_locker)
                    {
                        if (_pttPrefDataManager == null)
                        {
                            _pttPrefDataManager = new PTTPrefDataManager();
                        }
                    }
                }
                return _pttPrefDataManager;
            }
        }

        private PTTPrefDataManager()
        {
            try
            {
                _pttAccountFactorBindingList = new BindingList<PTTAccountPreference>();
                _pttPreferences = GetPTTPreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                _pttMFAccountPrefBindingList = GetMasterFundAccountPreferences();
                _pttDollarAmountPermission = GetPttDollarAmountPermission();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion


        private static BindingList<PTTAccountPreference> _pttAccountFactorBindingList;

        public static BindingList<PTTAccountPreference> PttAccountFactorBindingList
        {
            get { return _pttAccountFactorBindingList; }
            set { _pttAccountFactorBindingList = value; }
        }

        /// <summary>
        /// The PTT mf account preference binding list
        /// </summary>
        private List<BindingList<PTTMFAccountPref>> _pttMFAccountPrefBindingList;

        /// <summary>
        /// The PTT preferences
        /// </summary>
        private PTTPreferences _pttPreferences;

        public PTTPreferences PTTPreferences
        {
            get { return _pttPreferences; }
            set { _pttPreferences = value; }
        }

        private static bool? _pttDollarAmountPermission = null;

        public static bool? PTTDollarAmountPermission
        {
            get { return _pttDollarAmountPermission; }
            set { _pttDollarAmountPermission = value; }
        }
        /// <summary>
        /// Gets or sets the PTT mf account preference binding list.
        /// </summary>
        /// <value>
        /// The PTT mf account preference binding list.
        /// </value>
        public List<BindingList<PTTMFAccountPref>> PTTMfAccountPrefBindingList
        {
            get { return _pttMFAccountPrefBindingList; }
            set { _pttMFAccountPrefBindingList = value; }
        }

        /// <summary>
        /// Gets the preferences.
        /// </summary>
        /// <returns></returns>
        public List<BindingList<PTTMFAccountPref>> GetMasterFundAccountPreferences()
        {
            try
            {
                PTTMfAccountPrefBindingList = null;
                PTTMfAccountPrefBindingList = GetPTTMFAccountFromDB();
                return PTTMfAccountPrefBindingList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public static bool? GetPttDollarAmountPermission()
        {
            try
            {
                if (_pttDollarAmountPermission == null)
                    _pttDollarAmountPermission = GetDollarAmountPermission();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _pttDollarAmountPermission;
        }

        /// <summary>
        /// Gets the PTT preferences.
        /// </summary>
        /// <returns></returns>
        public PTTPreferences GetPTTPreferences(int userId)
        {
            try
            {
                if (PTTPreferences == null)
                    PTTPreferences = GetPTTInputParametersPrefs(userId);
                return PTTPreferences;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// to get the value of dollar amount permission for PTT from the database
        /// </summary>
        /// <returns></returns>
        public static bool GetDollarAmountPermission()
        {
            try
            {
                DataSet result = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDollarAmountPermission";

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (result.Tables[0].Rows.Count != 0)
                    return Boolean.Parse(result.Tables[0].Rows[0]["PTT"].ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the PTTMF account from database.
        /// </summary>
        /// <returns></returns>
        public static List<BindingList<PTTMFAccountPref>> GetPTTMFAccountFromDB()
        {
            try
            {
                DataSet result = null;
                PttAccountFactorBindingList = new BindingList<PTTAccountPreference>();

                DataTable dtMasterFundPref = new DataTable();
                DataTable dtAccountPref = new DataTable();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_PTTGetMasterFundAccountPreference";

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                AccountCollection userAccounts = CachedDataManager.GetInstance.GetUserAccounts();
                if (result != null && result.Tables.Count > 1)
                {
                    dtMasterFundPref = result.Tables[0];
                    dtAccountPref = result.Tables[1];
                }

                List<BindingList<PTTMFAccountPref>> masterFundPttBindingList = new List<BindingList<PTTMFAccountPref>>();
                masterFundPttBindingList.Add(new BindingList<PTTMFAccountPref>());
                masterFundPttBindingList.Add(new BindingList<PTTMFAccountPref>());
                masterFundPttBindingList.Add(new BindingList<PTTMFAccountPref>());
                Dictionary<int, List<int>> masterFundAssociation = new Dictionary<int, List<int>>();
                masterFundAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                Dictionary<int, string> dictUserPermittedMasterFund = CachedDataManager.GetInstance.GetUserMasterFunds();
                foreach (DataRow masterfund in dtMasterFundPref.Rows)
                {
                    int mfID = Int32.Parse(masterfund[PTTConstants.COL_MASTERFUNDID].ToString());
                    if (!dictUserPermittedMasterFund.ContainsKey(mfID))
                        continue;
                    if (masterFundAssociation.ContainsKey(mfID))
                    {
                        PTTMFAccountPref masterFundAlloc = new PTTMFAccountPref();
                        masterFundAlloc.MasterFundId = mfID;
                        masterFundAlloc.MasterFundName = CachedDataManager.GetInstance.GetMasterFund(mfID);
                        masterFundAlloc.IsProrataPrefChecked = Boolean.Parse(masterfund[PTTConstants.COL_USE_PRORATA_PREF].ToString());
                        masterFundAlloc.PreferenceType = (PTTPreferenceType)masterfund[PTTConstants.COL_PREFERENCETYPE];
                        List<int> accounts = new List<int>(masterFundAssociation[Int32.Parse(masterfund[PTTConstants.COL_MASTERFUNDID].ToString())]);
                        if (accounts != null && accounts.Count > 0)
                        {
                            foreach (var acc in accounts)
                            {
                                if (userAccounts.Contains(acc))
                                {
                                    //  int prefType = 0;//Convert.ToInt32(MFAccBindingList.FirstOrDefault().PreferenceType.ToString());
                                    DataRow[] rowAcc = dtAccountPref.Select(String.Format(@"[" + PTTConstants.COL_ACCOUNTID + "]='{0}' AND [" + PTTConstants.COL_PREFERENCETYPE + "] ='{1}'", acc, (int)masterFundAlloc.PreferenceType));
                                    PTTAccountPreference acccount = new PTTAccountPreference(CachedDataManager.GetInstance.GetAccountText(
                                            Int32.Parse(rowAcc[0][PTTConstants.COL_ACCOUNTID].ToString())),
                                            Int32.Parse(rowAcc[0][PTTConstants.COL_ACCOUNTID].ToString()),
                                            Decimal.Parse(rowAcc[0][PTTConstants.COL_PERCENT_IN_MASTERFUND].ToString()));
                                    masterFundAlloc.AccountWisePercentage.Add(acccount);
                                    masterFundAlloc.TotalPercentage += Decimal.Parse(rowAcc[0][PTTConstants.COL_PERCENT_IN_MASTERFUND].ToString());
                                }
                            }
                        }
                        masterFundPttBindingList[(int)masterFundAlloc.PreferenceType].Add(masterFundAlloc);
                    }
                }
                Dictionary<int, bool> dict = new Dictionary<int, bool>();

                foreach (DataRow accRow in dtAccountPref.Rows)
                {
                    if (userAccounts.Contains(accRow[PTTConstants.COL_ACCOUNTID].ToString()))
                    {
                        PTTAccountPreference account = new PTTAccountPreference();
                        account.AccountName = CachedDataManager.GetInstance.GetAccountText(Int32.Parse(accRow[PTTConstants.COL_ACCOUNTID].ToString()));
                        account.AccountId = Int32.Parse(accRow[PTTConstants.COL_ACCOUNTID].ToString());
                        account.AccountFactor = float.Parse(accRow[PTTConstants.COL_ACCOUNTFACTOR].ToString());
                        if (!dict.ContainsKey(account.AccountId))
                        {
                            dict.Add(account.AccountId, true);
                            PttAccountFactorBindingList.Add(account);
                        }
                    }
                }

                dict.Clear();
                return masterFundPttBindingList;
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
                return null;
            }
        }

        /// <summary>
        /// Saves the PTTMF account preference database.
        /// </summary>
        /// <param name="saveMFAccBindingList">The save mf accID binding list.</param>
        /// <param name="saveAccountFactorList"></param>
        public static void SavePTTMFAccountPrefDB(List<BindingList<PTTMFAccountPref>> saveMFAccBindingList, Dictionary<int, float> saveAccountFactorList)
        {
            DataTable masterFundTable = null;
            DataTable accountTable = null;
            StringWriter writerMf = null;
            StringWriter writerAcc = null;
            try
            {
                object[] parameter = new object[2];
                masterFundTable = new DataTable();
                masterFundTable.TableName = PTTConstants.CAP_MASTERFUND_PREF_TABLENAME;
                masterFundTable.Columns.Add(new DataColumn(PTTConstants.COL_MASTERFUNDID, typeof(int)));
                masterFundTable.Columns.Add(new DataColumn(PTTConstants.COL_IS_PRORATA_PERCENTAGE, typeof(bool)));
                masterFundTable.Columns.Add(new DataColumn(PTTConstants.COL_PREFERENCETYPE, typeof(string)));
                accountTable = new DataTable();
                accountTable.TableName = PTTConstants.CAP_ACCOUNT_PREF_TABLE_NAME;
                accountTable.Columns.Add(new DataColumn(PTTConstants.COL_ACCOUNTID, typeof(string)));
                accountTable.Columns.Add(new DataColumn(PTTConstants.COL_PERCENTAGE, typeof(decimal)));
                accountTable.Columns.Add(new DataColumn(PTTConstants.COL_ACCOUNTFACTOR, typeof(float)));
                accountTable.Columns.Add(new DataColumn(PTTConstants.COL_PREFERENCETYPE, typeof(string)));
                DataTable dt = GeneralUtilities.CreateTableFromCollection(masterFundTable, saveMFAccBindingList.ToList());
                foreach (var account in saveAccountFactorList)
                {
                    int temp = 0;
                    while (temp < 3)
                    {
                        DataRow dr = accountTable.NewRow();
                        dr[PTTConstants.COL_ACCOUNTID] = account.Key;
                        dr[PTTConstants.COL_ACCOUNTFACTOR] = account.Value;
                        dr[PTTConstants.COL_PERCENTAGE] = 0;
                        dr[PTTConstants.COL_PREFERENCETYPE] = temp++;
                        accountTable.Rows.Add(dr);
                    }
                }
                foreach (BindingList<PTTMFAccountPref> MFAccBindingList in saveMFAccBindingList)
                {
                    var AccountList = MFAccBindingList.SelectMany(x => x.AccountWisePercentage);

                    if (AccountList != null)
                    {
                        foreach (var account in AccountList)
                        {
                            int prefType = (int)MFAccBindingList.FirstOrDefault().PreferenceType;
                            DataRow[] dr = accountTable.Select(String.Format(@"[" + PTTConstants.COL_ACCOUNTID + "]='{0}' AND [" + PTTConstants.COL_PREFERENCETYPE + "] ='{1}'", account.AccountId, prefType)); //PTTConstants.COL_ACCOUNTID + @"='{0}'" + PTTConstants.COL_PREFERENCETYPE + @"='{0}'", +account.AccountId ,+prefType));
                            dr[0][PTTConstants.COL_PERCENTAGE] = account.Percentage;
                        }
                    }
                    foreach (PTTMFAccountPref mf in MFAccBindingList)
                    {
                        DataRow dr = masterFundTable.NewRow();
                        dr[PTTConstants.COL_MASTERFUNDID] = mf.MasterFundId;
                        dr[PTTConstants.COL_IS_PRORATA_PERCENTAGE] = mf.IsProrataPrefChecked;
                        dr[PTTConstants.COL_PREFERENCETYPE] = (int)mf.PreferenceType;
                        masterFundTable.Rows.Add(dr);
                    }
                }
                writerMf = new StringWriter();
                writerAcc = new StringWriter();
                dt.WriteXml(writerMf, true);
                accountTable.WriteXml(writerAcc, true);
                parameter[0] = writerMf.ToString();
                parameter[1] = writerAcc.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveMasterFundAccountPreference", parameter);
                GetInstance.PTTMfAccountPrefBindingList = DeepCopyHelper.Clone(saveMFAccBindingList);
            }


            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal static PTTPreferences GetPTTInputParametersPrefs(int userID)
        {
            PTTPreferences preferences = null;
            string startPath = Application.StartupPath;
            string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID;
            string preferencesFilePath = preferencesDirectoryPath + @"\PTTPreferences.xml";

            try
            {
                if (!Directory.Exists(preferencesDirectoryPath))
                {
                    Directory.CreateDirectory(preferencesDirectoryPath);
                }
                if (File.Exists(preferencesFilePath))
                {
                    using (FileStream fs = File.OpenRead(preferencesFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(PTTPreferences));
                        preferences = (PTTPreferences)serializer.Deserialize(fs);
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    preferences = new PTTPreferences();
                }
                return preferences;
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
                return null;
            }
            #endregion
        }

        public static void SavePTTInputParametersPrefs(PTTPreferences prefs, int userID)
        {
            try
            {
                string startPath = Application.StartupPath;
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID;
                string preferencesFilePath = preferencesDirectoryPath + @"\PTTPreferences.xml";

                using (XmlTextWriter writer = new XmlTextWriter(preferencesFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(PTTPreferences));
                    serializer.Serialize(writer, prefs);
                    writer.Flush();

                }
            }
            #region catch
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
        }



        internal bool? GetPTTDollorAmountPermission()
        {
            try
            {

                return PTTDollarAmountPermission;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
    }
}

