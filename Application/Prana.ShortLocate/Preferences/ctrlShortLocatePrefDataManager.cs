using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.ShortLocate.Preferences
{
    public class ctrlShortLocatePrefDataManager
    {

        #region singleton instance
        /// <summary>
        /// The PTT preference data manager object
        /// </summary>
        private static ctrlShortLocatePrefDataManager _shortLocatePrefDataManager = null;
        static object _locker = new object();
        public CompanyUser _loggedInUser = null;
        /// <summary>
        /// Gets the Preference instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        public static ctrlShortLocatePrefDataManager GetInstance
        {
            get
            {
                if (_shortLocatePrefDataManager == null)
                {
                    lock (_locker)
                    {
                        if (_shortLocatePrefDataManager == null)
                        {
                            _shortLocatePrefDataManager = new ctrlShortLocatePrefDataManager();
                        }
                    }
                }
                return _shortLocatePrefDataManager;
            }
        }

        public void SetUser(CompanyUser user)
        {
            _loggedInUser = user;
        }

        public ctrlShortLocatePrefDataManager()
        {
            try
            {
                _shortLocateUIGridBindingList = new BindingList<ShortLocateUIGridDetails>();
                _shortLocatePreferences = GetShortLocatePreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                ShortLocateUIGridDetails decimalPlaces = new ShortLocateUIGridDetails();
                decimalPlaces.ShortLocateDecimalPreference("Last Px", _shortLocatePreferences.LastPxDecimal);
                _shortLocateUIGridBindingList.Add(decimalPlaces);
                decimalPlaces = new ShortLocateUIGridDetails();
                decimalPlaces.ShortLocateDecimalPreference("Rebate Fees", _shortLocatePreferences.RebatefeesDecimal);
                _shortLocateUIGridBindingList.Add(decimalPlaces);
                decimalPlaces = new ShortLocateUIGridDetails();
                decimalPlaces.ShortLocateDecimalPreference("Total Amount", _shortLocatePreferences.TotalAmountDecimal);
                _shortLocateUIGridBindingList.Add(decimalPlaces);
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

        #endregion



        private static BindingList<ShortLocateUIGridDetails> _shortLocateUIGridBindingList;
        public static BindingList<ShortLocateUIGridDetails> ShortLocateUIGridBindingList
        {
            get { return _shortLocateUIGridBindingList; }
            set { _shortLocateUIGridBindingList = value; }
        }

        private ShortLocateUIPreferences _shortLocatePreferences;

        public ShortLocateUIPreferences ShortLocatePreferences
        {
            get { return _shortLocatePreferences; }
            set { _shortLocatePreferences = value; }
        }

        public ShortLocateUIPreferences GetShortLocatePreferences(int userId)
        {
            try
            {
                if (ShortLocatePreferences == null)
                    ShortLocatePreferences = GetShortLocatePrefs(userId);
                return ShortLocatePreferences;
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

        public static void SaveShortLocatePrefs(ShortLocateUIPreferences prefs, int userID)
        {
            try
            {
                string startPath = Application.StartupPath;
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID;
                string preferencesFilePath = preferencesDirectoryPath + @"\ShortLocatePreferences.xml";

                using (XmlTextWriter writer = new XmlTextWriter(preferencesFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(ShortLocateUIPreferences));
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

        internal static ShortLocateUIPreferences GetShortLocatePrefs(int userID)
        {
            ShortLocateUIPreferences preferences = null;
            string startPath = Application.StartupPath;
            string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID;
            string preferencesFilePath = preferencesDirectoryPath + @"\ShortLocatePreferences.xml";

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
                        XmlSerializer serializer = new XmlSerializer(typeof(ShortLocateUIPreferences));
                        preferences = (ShortLocateUIPreferences)serializer.Deserialize(fs);
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    preferences = new ShortLocateUIPreferences();
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

    }
}
