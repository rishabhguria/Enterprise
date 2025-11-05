using Prana.Global;
using Prana.LogManager;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Tools
{
    public class PricingPreferenceManager
    {
        #region Private Variables
        //private static int _companyID = int.Parse(CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());
        static string _startPath = string.Empty;
        //static string _pricingPreferencesPath = string.Empty;
        static string _pricingPreferencesFilePath = string.Empty;
        static string _pricingPreferencesDirectoryPath = string.Empty;
        static PricingPreference _oldPricingPreferences = null;
        static int _userID = int.MinValue;
        private static object locker = new object();
        #endregion

        public PricingPreferenceManager()
        {
        }

        public static void Dispose()
        {
            try
            {
                lock (locker)
                {
                    _oldPricingPreferences = null;
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

        private static PricingPreference GetPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            PricingPreference pricingPreferences = new PricingPreference();
            _pricingPreferencesDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _pricingPreferencesFilePath = _pricingPreferencesDirectoryPath + @"\PricingInputsPreference.xml";

            try
            {
                if (!Directory.Exists(_pricingPreferencesDirectoryPath))
                {
                    Directory.CreateDirectory(_pricingPreferencesDirectoryPath);
                }
                if (File.Exists(_pricingPreferencesFilePath))
                {
                    try
                    {
                        using (FileStream fs = File.OpenRead(_pricingPreferencesFilePath))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(PricingPreference));
                            pricingPreferences = (PricingPreference)serializer.Deserialize(fs);
                        }
                    }
                    catch (Exception ex)
                    {
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-6121
                        // If there is a problem in preferences xml then we will provide default.
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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
            return pricingPreferences;
        }

        public static void SavePreferences(PricingPreference pricingPreference)
        {
            try
            {
                if (pricingPreference != null)
                {
                    _oldPricingPreferences = pricingPreference;
                }
                if (_oldPricingPreferences != null)
                {
                    using (XmlTextWriter writer = new XmlTextWriter(_pricingPreferencesFilePath, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(PricingPreference));
                        serializer.Serialize(writer, _oldPricingPreferences);
                        writer.Flush();
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

        public static PricingPreference PricingPreference
        {
            get
            {
                if (_oldPricingPreferences == null)
                {
                    _oldPricingPreferences = GetPreferences();
                }
                return _oldPricingPreferences;
            }
        }

        public static void SetUp(string startUpPath)
        {
            _startPath = startUpPath;
        }
    }
}