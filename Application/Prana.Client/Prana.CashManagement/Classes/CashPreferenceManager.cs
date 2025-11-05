using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.CashManagement.Classes
{
    public class CashPreferenceManager : IDisposable
    {

        #region Private fields

        /// <summary>
        /// The start path
        /// </summary>
        string _startPath = string.Empty;

        /// <summary>
        /// The preferences path
        /// </summary>
        string _preferencesPath = string.Empty;

        /// <summary>
        /// The preferences file
        /// </summary>
        string _preferencesFile = string.Empty;

        /// <summary>
        /// The login user
        /// </summary>
        private static CompanyUser _loginUser = null;

        #endregion

        #region Singleton Implementation of class

        /// <summary>
        /// The cash preferences manager
        /// </summary>
        private static CashPreferenceManager _cashPreferencesManager = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static CashPreferenceManager GetInstance()
        {
            if (_cashPreferencesManager == null)
            {
                _cashPreferencesManager = new CashPreferenceManager();
            }
            return _cashPreferencesManager;
        }

        #endregion

        #region Get Cash Layout Details

        /// <summary>
        /// Sets the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public static void SetUser(CompanyUser user)
        {
            _loginUser = user;
        }

        /// <summary>
        /// This is to get the latest path each time. If we had pasted it in the contructor, it would have
        /// worked for the first time only when the instance of CashPreferenceManager was created but 
        /// we want to call each time as xml file has been every time whenever we saved any layout.
        /// </summary>
        /// <returns></returns>
        private void GetGridLayoutFilePath()
        {
            _startPath = Application.StartupPath;
            _preferencesPath = _startPath + "\\" + Prana.Global.ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID.ToString();

            try
            {
                if (!Directory.Exists(_preferencesPath))
                {
                    Directory.CreateDirectory(_preferencesPath);
                }

                _preferencesFile = _preferencesPath + @"\CashManagementGridsLayout.xml";

                if (!File.Exists(_preferencesFile))
                {
                    SetDefaultPreferences();
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
        /// Getting the grid layout
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="preference"></param>
        public void GetLayoutDetails<T>(ref T preference)
        {
            FileStream fs = null;

            try
            {
                GetGridLayoutFilePath();

                fs = File.OpenRead(_preferencesFile);
                XmlSerializer serializer;
                serializer = new XmlSerializer(typeof(T));
                preference = (T)serializer.Deserialize(fs);
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
            finally
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                }
            }

        }

        /// <summary>
        /// Called at time of loading of grid over CM
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public CashPreferencesList GetLayoutDetails()
        {
            CashPreferencesList list = null;
            try
            {
                GetLayoutDetails(ref list);
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
            return list;
        }

        #endregion

        #region Set Layout of CM Grids

        /// <summary>
        /// Used to save current grid layout
        /// </summary>
        /// <param name="SetCashGridLayout"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SetCashGridLayout(CashManagementLayout cashManagementLayout, string key)
        {
            CashPreferencesList list = null;
            bool result = false;

            try
            {
                GetLayoutDetails(ref list);

                if (list != null)
                {
                    if (!list.ContainsKey(key))
                    {
                        list.Add(key, cashManagementLayout);
                    }
                    else
                    {
                        list[key].GroupByColumnsCollection = cashManagementLayout.GroupByColumnsCollection;
                        list[key].SelectedColumnsCollection = cashManagementLayout.SelectedColumnsCollection;
                    }

                    result = SetGridLayout(list);
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
                return result;
            }

            return result;
        }

        /// <summary>
        /// In case to save all the layout of grids at once
        /// </summary>
        /// <param name="SetCashGridList"></param>
        /// <returns></returns>
        public bool SetCashGridList(Dictionary<string, CashManagementLayout> cashManagementLayout)
        {
            CashPreferencesList list = null;
            bool result = false;

            try
            {
                GetLayoutDetails(ref list);

                if (list != null)
                {
                    foreach (string key in cashManagementLayout.Keys)
                    {
                        if (!list.ContainsKey(key))
                        {
                            list.Add(key, cashManagementLayout[key]);
                        }
                        else
                        {
                            list[key].GroupByColumnsCollection = cashManagementLayout[key].GroupByColumnsCollection;
                            list[key].SelectedColumnsCollection = cashManagementLayout[key].SelectedColumnsCollection;
                        }
                    }

                    result = SetGridLayout(list);
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
                return result;
            }

            return result;
        }

        /// <summary>
        /// Set Preferences if not set before
        /// </summary>
        /// <param name="_preferences"></param>
        /// <returns></returns>
        public void SetDefaultPreferences()
        {
            try
            {
                CashPreferencesList cashPreferences = new CashPreferencesList();
                SetGridLayout(cashPreferences);
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
        ///  Set the new layout for CM grid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_preferences"></param>
        /// <returns></returns>
        public bool SetGridLayout<T>(T _preferences)
        {
            XmlTextWriter writer = new XmlTextWriter(_preferencesFile, Encoding.UTF8);
            try
            {
                writer.Formatting = Formatting.Indented;

                XmlSerializer serializer;
                serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, _preferences);
                serializer = null;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
            return true;

        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _startPath = null;
                _preferencesPath = null;
                _preferencesFile = null;
                //_userName = null;
                _loginUser = null;
                _cashPreferencesManager = null;
            }
        }
        #endregion

    }
}
