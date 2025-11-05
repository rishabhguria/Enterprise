using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Prana.TradeManager.Extension
{
    public class BlotterPreferenceManager
    {
        private static object _lockerObj = new object();

        private static BlotterPreferenceManager _blotterPreferencesManager;
        public static BlotterPreferenceManager GetInstance()
        {
            if (_blotterPreferencesManager == null)
            {
                lock (_lockerObj)
                {
                    if (_blotterPreferencesManager == null)
                    {
                        _blotterPreferencesManager = new BlotterPreferenceManager();
                    }
                }
            }
            return _blotterPreferencesManager;
        }

        private BlotterPreferenceData _blotterPreferences;
        public BlotterPreferenceData BlotterPreferences
        {
            get
            {
                if (_blotterPreferences == null)
                {
                    _blotterPreferences = (BlotterPreferenceData)GetPreferencesBinary();
                }
                return _blotterPreferences;
            }
            set { _blotterPreferences = value; }
        }

        private string _blotterPreferencesPath = string.Empty;
        public string BlotterPreferencesPath
        {
            get { return _blotterPreferencesPath; }
        }

        public string _preferencesBinaryFilePath = string.Empty;
        public string BinaryPreferenceFileName = "Blotter Preferences.dat";
        public string GridPreferenceFileName = "BlotterGridLayout.xml";
        private CompanyUser _loggedInUser = null;

        public void SetUser(CompanyUser user)
        {
            if (user != null)
            {
                _loggedInUser = user;
                _blotterPreferencesPath = TradeManagerExtension.GetInstance().ApplicationPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loggedInUser.CompanyUserID.ToString();
                _preferencesBinaryFilePath = _blotterPreferencesPath + "\\" + BinaryPreferenceFileName;
            }
        }

        public BlotterPreferenceData SetDefaultPreferences()
        {
            BlotterPreferenceData blotterPreferenceData = new BlotterPreferenceData();
            blotterPreferenceData.BuyOrder = System.Drawing.Color.FromArgb(177, 216, 64);
            blotterPreferenceData.CoverOrder = System.Drawing.Color.SkyBlue;
            blotterPreferenceData.SellOrder = System.Drawing.Color.FromArgb(255, 91, 71);
            blotterPreferenceData.ShortOrder = System.Drawing.Color.FromArgb(239, 175, 85);
            blotterPreferenceData.ApplyColorPreferencesInTheme = true;
            blotterPreferenceData.WrapHeader = false;
            blotterPreferenceData.RejectionPopup = true;
            return blotterPreferenceData;
        }

        public object GetPreferencesBinary()
        {
            FileStream fileStream = null;
            BlotterPreferenceData blotterPrefrencesData = new BlotterPreferenceData();
            try
            {
                if (File.Exists(_preferencesBinaryFilePath))
                {
                    fileStream = new FileStream(_preferencesBinaryFilePath, FileMode.Open, FileAccess.Read);

                    BinaryFormatter serializer = new BinaryFormatter();
                    object obj = null;
                    try
                    {
                        obj = serializer.Deserialize(fileStream);
                    }
                    catch (System.Runtime.Serialization.SerializationException)
                    {
                    }
                    string str = obj as string;
                    blotterPrefrencesData.DeSerialize(str);
                }

                if (blotterPrefrencesData == null)
                {
                    blotterPrefrencesData = SetDefaultPreferences();
                    SetPreferencesBinary(blotterPrefrencesData);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            return blotterPrefrencesData;
        }

        public bool SetPreferencesBinary(BlotterPreferenceData blotterPreferenceData)
        {
            if (File.Exists(_preferencesBinaryFilePath))
            {
                File.Delete(_preferencesBinaryFilePath);
            }

            FileStream fs = new FileStream(_preferencesBinaryFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            try
            {
                binaryFormatter.Serialize(fs, blotterPreferenceData.Serialize());
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                return false;
            }
            finally
            {
                _blotterPreferences = blotterPreferenceData;
                fs.Close();
            }
            return true;
        }

        public string GetOrdersPreferencePath(CompanyUser user, string fileName)
        {
            return TradeManagerExtension.GetInstance().ApplicationPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + user.CompanyUserID.ToString() + "\\" + fileName;
        }
    }
}