using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.CommonDataCache
{
    public class RiskPreferenceManager
    {
        #region Private Variables
        private static float _interestRate = Convert.ToSingle(ConfigurationManager.AppSettings["InterestRate"]);  
        private static int _companyID = int.Parse(CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());
        static string _startPath = string.Empty;
        static string _riskPreferencesFilePath = string.Empty;
        static string _riskDefaultPreferencesFilePath = string.Empty;
        static string _riskPreferencesDirectoryPath = string.Empty;
        static RiskPrefernece _oldRiskPreferences = null;
        static SortedList<int, float> _interestRates = null;
        static Dictionary<string, string> _dictPSSymbolMapping = null;
        static int _userID = int.MinValue;
        static readonly object _locker = new object();
        private static IRiskPreferenceDataManager _riskPreferenceDataManager;
        #endregion

        public RiskPreferenceManager()
        {
        }

        static RiskPreferenceManager()
        {
            _riskPreferenceDataManager = WindsorContainerManager.Container.Resolve<IRiskPreferenceDataManager>();
        }

        private static RiskPrefernece GetPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            RiskPrefernece riskPreferences = new RiskPrefernece();
            _riskPreferencesDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _riskPreferencesFilePath = _riskPreferencesDirectoryPath + @"\RiskPreference.xml";

            try
            {
                if (!Directory.Exists(_riskPreferencesDirectoryPath))
                {
                    Directory.CreateDirectory(_riskPreferencesDirectoryPath);
                }
                if (File.Exists(_riskPreferencesFilePath))
                {
                    try
                    {
                        using (FileStream fs = File.OpenRead(_riskPreferencesFilePath))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(RiskPrefernece));
                            riskPreferences = (RiskPrefernece)serializer.Deserialize(fs);
                        }
                    }
                    catch (Exception ex)
                    {
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-3885
                        // If there is a problem in preferences xml then we will provide default.
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        riskPreferences.StepAnalPreferencesDict.Add("Main", GetDefaultPreferences());
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    riskPreferences.StepAnalPreferencesDict.Add("Main", GetDefaultPreferences());
                }
                if (riskPreferences == null)
                {
                    riskPreferences = new RiskPrefernece();
                    riskPreferences.StepAnalPreferencesDict.Add("Main", GetDefaultPreferences());
                }

                DataSet dsMaxViews = KeyValueDataManager.GetCompanyRiskPreferences(_companyID);
                if (dsMaxViews != null)
                {
                    if (dsMaxViews.Tables.Count > 0)
                    {
                        if (dsMaxViews.Tables[0].Rows.Count > 0)
                        {
                            riskPreferences.MaxStressTestViewsWithVolSkew = int.Parse(dsMaxViews.Tables[0].Rows[0]["MaxStressTestViewsWithVolSkew"].ToString());
                            riskPreferences.MaxStressTestViewsWithoutVolSkew = int.Parse(dsMaxViews.Tables[0].Rows[0]["MaxStressTestViewsWithoutVolSkew"].ToString());
                        }
                    }
                }
                lock (locker)
                {
                    _interestRates = _riskPreferenceDataManager.SetInerestRateFromDB(riskPreferences);
                    _oldRiskPreferences = riskPreferences;
                }
                _riskPreferenceDataManager.SetDefaultVolShockAdjFactorFromDB(riskPreferences);
                _dictPSSymbolMapping = _riskPreferenceDataManager.SetPSSymbolMappingFromDB(riskPreferences);
                return riskPreferences;
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

        public static StepAnalysisPref GetDefaultPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            StepAnalysisPref stepAnalPref = new StepAnalysisPref();
            _riskPreferencesDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _riskDefaultPreferencesFilePath = _riskPreferencesDirectoryPath + @"\RiskDefaultPreference.xml";

            try
            {
                if (!Directory.Exists(_riskPreferencesDirectoryPath))
                {
                    Directory.CreateDirectory(_riskPreferencesDirectoryPath);
                }
                if (File.Exists(_riskDefaultPreferencesFilePath))
                {
                    using (FileStream fs = File.OpenRead(_riskDefaultPreferencesFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(StepAnalysisPref));
                        stepAnalPref = (StepAnalysisPref)serializer.Deserialize(fs);
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    stepAnalPref = new StepAnalysisPref();
                }
                return stepAnalPref;
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

        public static void SavePreferences(RiskPrefernece riskPrefernece)
        {
            try
            {
                if (riskPrefernece != null)
                {
                    _oldRiskPreferences = riskPrefernece;
                }
                if (_oldRiskPreferences != null)
                {
                    using (XmlTextWriter writer = new XmlTextWriter(_riskPreferencesFilePath, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(RiskPrefernece));
                        serializer.Serialize(writer, _oldRiskPreferences);
                        writer.Flush();

                    }
                    if (_oldRiskPreferences.InterestRateTable.Tables.Count > 0)
                    {
                        DataTable dtIR = _oldRiskPreferences.InterestRateTable.Tables[0];
                        _riskPreferenceDataManager.SaveInterestRatesToDB(dtIR);
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

        public static void SaveDefaultPreferences(StepAnalysisPref stepAnalPref)
        {
            try
            {
                _riskPreferencesDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
                _riskDefaultPreferencesFilePath = _riskPreferencesDirectoryPath + @"\RiskDefaultPreference.xml";

                using (XmlTextWriter writer = new XmlTextWriter(_riskDefaultPreferencesFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(StepAnalysisPref));
                    serializer.Serialize(writer, stepAnalPref);
                    writer.Flush();
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

        public static RiskPrefernece RiskPrefernece
        {
            get
            {
                if (_oldRiskPreferences == null)
                {
                    _oldRiskPreferences = GetPreferences();
                }
                return _oldRiskPreferences;
            }
        }

        private static object locker = new object();
        public static double GetInterestRate(double numberOfDays)
        {
            float rate = float.MinValue; // default rate
            try
            {
                lock (locker)
                {
                    if (_oldRiskPreferences == null)
                    {
                        _oldRiskPreferences = new RiskPrefernece();
                        _interestRates = _riskPreferenceDataManager.SetInerestRateFromDB(_oldRiskPreferences);
                    }
                }
                // The value of 'numberofDays' connot be converted into decimal when string contain scientific number
                decimal numbOfDays = Convert.ToDecimal(numberOfDays) / 30;
                // numbOfDays is already a decimal value So removing decimal cast operator
                decimal keyDouble = Math.Ceiling(numbOfDays);
                int key = Convert.ToInt32(keyDouble);
                if (_interestRates.ContainsKey(key))
                {
                    rate = _interestRates[key];
                }
                else
                {
                    // take the mean of adjacent ones
                    foreach (KeyValuePair<int, float> interestRatePair in _interestRates)
                    {
                        if (key > interestRatePair.Key)
                        {
                            int currentIndex = _interestRates.IndexOfKey(interestRatePair.Key);
                            if (currentIndex < _interestRates.Count - 1)
                            {
                                rate = (_interestRates.Values[currentIndex + 1] + interestRatePair.Value) / 2;
                            }
                            else if (currentIndex == _interestRates.Count - 1)
                            {
                                rate = interestRatePair.Value;
                            }
                            //add this for future references to avoid future calculation
                            _interestRates.Add(key, rate);
                            break;
                        }
                    }
                }

                // if yet not set take the first one or the default value 
                if (rate == float.MinValue)
                {
                    if (_interestRates.Count > 0)
                    {
                        rate = _interestRates.Values[0];
                    }
                    else
                    {
                        rate = _interestRate;
                    }
                }
                rate = rate / 100;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rate;
        }

        public static SerializableDictionary<string, bool> GetExportColumns()
        {
            try
            {
                if (_oldRiskPreferences.ExportColumnList.Count > 0)
                {
                    return _oldRiskPreferences.ExportColumnList;
                }
                else
                {
                    _oldRiskPreferences.ExportColumnList = GetDefaultColumnsForExport();
                    return _oldRiskPreferences.ExportColumnList;
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
            return null;
        }

        private static SerializableDictionary<string, bool> GetDefaultColumnsForExport()
        {
            SerializableDictionary<string, bool> defaultExportList = new SerializableDictionary<string, bool>();
            try
            {
                List<string> visibleColumns = GetVisibleColumns();
                List<string> defaultColumns = GetDefaultColumns();
                foreach (string col in visibleColumns)
                {
                    if (defaultColumns.Contains(col))
                        defaultExportList.Add(col, true);
                    else
                        defaultExportList.Add(col, false);
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
            return defaultExportList;
        }

        public static List<string> GetVisibleColumns()
        {
            List<string> visibleColumns = new List<string>();
            try
            {
                visibleColumns.Add("Position");
                visibleColumns.Add("Asset");
                visibleColumns.Add("Trade Date");
                visibleColumns.Add("Cost Basis (Local)");
                visibleColumns.Add("User");
                visibleColumns.Add("Cost Basis P&L (Local)");
                visibleColumns.Add("Broker");
                visibleColumns.Add("Country");
                visibleColumns.Add("Currency");
                visibleColumns.Add("Simulated P&L (Local)");
                visibleColumns.Add("Days To Expiration");
                visibleColumns.Add("Exchange");
                visibleColumns.Add("Expiration Date");
                visibleColumns.Add("Volatility (%)");
                visibleColumns.Add("Interest Rate (%)");
                visibleColumns.Add("Px Selected Feed (Local)");
                visibleColumns.Add("Simulated Price (Local)");
                visibleColumns.Add("Position Type");
                visibleColumns.Add("Sector");
                visibleColumns.Add("Security Name");
                visibleColumns.Add("Security Type");
                visibleColumns.Add("Simulated Underlying Price (Local)");
                visibleColumns.Add("Strike Price");
                visibleColumns.Add("Sub Sector");
                visibleColumns.Add("Underlying");
                visibleColumns.Add("Underlying Symbol");
                visibleColumns.Add("Delta Exposure (Local)");
                visibleColumns.Add("Delta Position");
                visibleColumns.Add("Put/Call");
                visibleColumns.Add("Dollar Delta (Local)");
                visibleColumns.Add("Dollar Gamma (Local)");
                visibleColumns.Add("Dollar Theta (Local)");
                visibleColumns.Add("Dollar Vega (Local)");
                visibleColumns.Add("Dollar Rho (Local)");
                visibleColumns.Add("Account");
                visibleColumns.Add("Strategy");
                visibleColumns.Add("Proxy Symbol");
                visibleColumns.Add("Master Fund");
                visibleColumns.Add("Symbol");
                visibleColumns.Add("Bloomberg");
                visibleColumns.Add("Delta Position (LME)");
                visibleColumns.Add("Expiration Month");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return visibleColumns;
        }

        public static List<string> GetDefaultColumns()
        {
            List<string> colDefault = new List<string>();
            try
            {
                colDefault.Add("Symbol");
                colDefault.Add("Bloomberg");
                colDefault.Add("Security Name");
                colDefault.Add("Underlying Symbol");
                colDefault.Add("Account");
                colDefault.Add("Master Fund");
                colDefault.Add("Asset");
                colDefault.Add("Position");
                colDefault.Add("Px Selected Feed (Local)");
                colDefault.Add("Simulated Underlying Price (Local)");
                colDefault.Add("Simulated Price (Local)");
                colDefault.Add("Simulated P&L (Local)");
                colDefault.Add("Volatility (%)");
                colDefault.Add("Delta Exposure (Local)");
                colDefault.Add("Delta");
                colDefault.Add("Dollar Delta (Local)");
                colDefault.Add("Gamma");
                colDefault.Add("Dollar Gamma (Local)");
                colDefault.Add("Theta");
                colDefault.Add("Dollar Theta (Local)");
                colDefault.Add("Vega");
                colDefault.Add("Dollar Vega (Local)");
                colDefault.Add("Rho");
                colDefault.Add("Dollar Rho (Local)");
                colDefault.Add("Days To Expiration");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return colDefault;
        }

        public static string GetPSSymbol(string symbol)
        {
            try
            {
                lock (_locker)
                {
                    if (_oldRiskPreferences == null)
                    {
                        _oldRiskPreferences = new RiskPrefernece();
                        _dictPSSymbolMapping = _riskPreferenceDataManager.SetPSSymbolMappingFromDB(_oldRiskPreferences);
                    }

                    if (_dictPSSymbolMapping.ContainsKey(symbol))
                    {
                        return _dictPSSymbolMapping[symbol];
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
            return string.Empty;
        }

        public static void RefreshInterestRate()
        {
            try
            {
                lock (locker)
                {
                    if (_oldRiskPreferences == null)
                    {
                        _oldRiskPreferences = new RiskPrefernece();
                    }
                    _interestRates = _riskPreferenceDataManager.SetInerestRateFromDB(_oldRiskPreferences);
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
        }

        public static void RefreshPSSymbolMapping(DataSet dsSymbolMapping)
        {
            DataTable dt = null;
            try
            {
                lock (_locker)
                {
                    if (dsSymbolMapping != null)
                    {
                        if (_dictPSSymbolMapping == null)
                        {
                            _dictPSSymbolMapping = new Dictionary<string, string>();
                        }

                        if (dsSymbolMapping.Tables.Count > 0)
                        {
                            dt = dsSymbolMapping.Tables[0];
                        }

                        if (dt != null)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted)
                                {
                                    string symbolDeleted = row[0, DataRowVersion.Original].ToString();
                                    if (symbolDeleted != string.Empty)
                                    {
                                        if (_dictPSSymbolMapping.ContainsKey(symbolDeleted))
                                        {
                                            _dictPSSymbolMapping.Remove(symbolDeleted);
                                        }
                                    }
                                }
                                else
                                {
                                    string symbol = row[0].ToString();
                                    string PSSymbol = row[1].ToString();

                                    if (symbol != string.Empty && PSSymbol != string.Empty)
                                    {
                                        if (!_dictPSSymbolMapping.ContainsKey(symbol))
                                        {
                                            _dictPSSymbolMapping.Add(symbol, PSSymbol);
                                        }
                                        else
                                        {
                                            _dictPSSymbolMapping[symbol] = PSSymbol;
                                        }
                                    }
                                }
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

        public static void CleanUp()
        {
            try
            {
                lock (locker)
                {
                    _oldRiskPreferences = null;
                    _interestRates = null;
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
        }

        public static void SetUp(string startUpPath)
        {
            _startPath = startUpPath;
        }

        public static void RestoreDefaults()
        {
            try
            {
                RiskPrefernece currentPreferences;

                lock (locker)
                {
                    currentPreferences = _oldRiskPreferences;
                    _oldRiskPreferences = new RiskPrefernece();
                    _interestRates = _riskPreferenceDataManager.SetInerestRateFromDB(_oldRiskPreferences);
                }

                _oldRiskPreferences.DtVolShockFactorDefault = currentPreferences.DtVolShockFactorDefault;
                _oldRiskPreferences.SymbolMappingTable = currentPreferences.SymbolMappingTable;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}