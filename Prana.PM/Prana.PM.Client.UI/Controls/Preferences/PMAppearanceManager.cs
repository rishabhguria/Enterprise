using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.PM.Client.UI.Classes
{
    class PMAppearanceManager
    {
        public PMAppearanceManager()
        {
        }

        private static object locker = new object();
        static int _userID = int.MinValue;
        static string _appearancePreferencesDirectoryPath = string.Empty;
        static string _appearancePreferencesFilePath = string.Empty;
        static string _startPath = Application.StartupPath;
        static PMAppearances _oldPMAppearances = null;

        public static PMAppearances PMAppearance
        {
            get
            {
                lock (locker)
                {
                    if (_oldPMAppearances == null)
                    {
                        _oldPMAppearances = GetPreferences();
                    }
                    return _oldPMAppearances;
                }
            }
        }

        public static PMAppearances GetPreferences()
        {
            _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            PMAppearances pMAppearances = new PMAppearances();
            _appearancePreferencesDirectoryPath = _startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID.ToString();
            _appearancePreferencesFilePath = _appearancePreferencesDirectoryPath + @"\PMAppearance.xml";

            try
            {
                if (!Directory.Exists(_appearancePreferencesDirectoryPath))
                {
                    Directory.CreateDirectory(_appearancePreferencesDirectoryPath);
                }
                if (File.Exists(_appearancePreferencesFilePath))
                {
                    using (FileStream fs = File.OpenRead(_appearancePreferencesFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(PMAppearances));
                        pMAppearances = (PMAppearances)serializer.Deserialize(fs);

                        SerializableDictionary<string, int> tempDecimalPlaceLimitsForColumnsOld = (SerializableDictionary<string, int>)pMAppearances.DecimalPlaceLimitsForColumns.Clone();
                        GetDictionaryForDecimalPlaces(pMAppearances);
                        SerializableDictionary<string, int> tempDecimalPlaceLimitsForColumnsNew = (SerializableDictionary<string, int>)pMAppearances.DecimalPlaceLimitsForColumns.Clone();

                        foreach (string key in tempDecimalPlaceLimitsForColumnsNew.Keys)
                        {
                            if (tempDecimalPlaceLimitsForColumnsOld.ContainsKey(key))
                            {
                                pMAppearances.DecimalPlaceLimitsForColumns[key] = tempDecimalPlaceLimitsForColumnsOld[key];
                            }
                        }
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    pMAppearances = new PMAppearances();
                    GetDictionaryForDecimalPlaces(pMAppearances);
                }

                _oldPMAppearances = pMAppearances;
                return pMAppearances;
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

        public static void GetDictionaryForDecimalPlaces(PMAppearances pMAppearances)
        {
            try
            {
                Type type = typeof(ExposurePnlCacheItem);
                PropertyInfo[] propertyList = type.GetProperties();
                pMAppearances.DecimalPlaceLimitsForColumns = new SerializableDictionary<string, int>();
                foreach (PropertyInfo property in propertyList)
                {
                    if (property.PropertyType == typeof(System.Double) || (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        bool isBrowsableFalse = false;
                        object[] att = property.GetCustomAttributes(typeof(BrowsableAttribute), false);
                        if (att != null && att.Length > 0)
                        {
                            BrowsableAttribute b = att[0] as BrowsableAttribute;
                            if (!b.Browsable)
                            {
                                isBrowsableFalse = true;
                            }
                        }

                        if (!isBrowsableFalse)
                        {
                            string caption = PMConstantsHelper.GetCaptionByColumnName(property.Name);
                            int decimalPlaceLimit = PMConstantsHelper.GetDefaultCloumnWiseDecimalDigits(caption);

                            if (!pMAppearances.DecimalPlaceLimitsForColumns.ContainsKey(caption))
                            {
                                pMAppearances.DecimalPlaceLimitsForColumns.Add(caption, decimalPlaceLimit);
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

        public static void SavePreferences(PMAppearances pMAppearances)
        {
            try
            {
                lock (locker)
                {
                    if (pMAppearances != null)
                    {
                        _oldPMAppearances = pMAppearances;
                    }
                    using (XmlTextWriter writer = new XmlTextWriter(_appearancePreferencesFilePath, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(PMAppearances));
                        serializer.Serialize(writer, _oldPMAppearances);
                        writer.Flush();
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
        }

        public static void Dispose()
        {
            lock (locker)
            {
                if (_oldPMAppearances != null)
                {
                    _oldPMAppearances.Dispose();
                    _oldPMAppearances = null;
                }
            }
        }

        public static void RestoreDefaults()
        {
            lock (locker)
            {
                _oldPMAppearances = new PMAppearances();
            }
        }
    }
}