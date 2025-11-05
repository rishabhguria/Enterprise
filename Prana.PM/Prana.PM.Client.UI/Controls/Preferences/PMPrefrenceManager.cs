using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.PM.BLL
{
    /// <summary>
    /// Singleton class which serialize or deserialize the PMPreferences. We need to provide 
    /// the instance of respective module Preferences while serializing and it retu
    /// rns the object 
    /// which can be further cast in the respective module to their respective module preferences.
    /// </summary>
    public class PMPrefrenceManager : IDisposable
    {

        #region Private fields

        string _startPath = string.Empty;
        string _preferencesPath = string.Empty;
        string _preferencesFile = string.Empty;
        string _subModuleName = string.Empty;
        private const string CUSTOMVIEW = "CustomView";
        CompanyUser _loginUser;
        #endregion

        #region Public Property

        /// <summary>
        /// Crucial property to set before  PMPrefrenceManager.GetInstace() call
        /// We need to set this property exclusively because just passing the subModuleName in the PMPrefrenceManager 
        /// is not going to set submodule. It is because we are loading the preference control for all modules each time
        /// it is opened from any of the place.
        /// </summary>
        public string SubModuleName
        {
            get { return _subModuleName; }
            set
            {
                _subModuleName = value;
                ///Call this function each time when the submodule name change 
                ///as we need to change the filepath also 
                GetPreferenceFilePath();
                if (!Directory.Exists(_preferencesPath))
                {
                    Directory.CreateDirectory(_preferencesPath);
                    SetDefaultPreferences();
                }
            }
        }


        #endregion

        #region Singleton Implementation of class

        public PMPrefrenceManager(string subModuleName) //string subModuleName)
        {
            _subModuleName = subModuleName;
        }

        private static PMPrefrenceManager _pmPreferencesManager = null;
        public static PMPrefrenceManager GetInstance(string subModuleName)
        {
            if (_pmPreferencesManager == null)
            {
                _pmPreferencesManager = new PMPrefrenceManager(subModuleName);
            }
            return _pmPreferencesManager;
        }
        public void SetUser(CompanyUser user)
        {
            _loginUser = user;
        }

        public string GetPreferenceDirectory()
        {
            return Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + _loginUser.CompanyUserID.ToString();
        }

        /// <summary>
        /// This is to get the latest path each time according to the submodule. If we had pasted it
        /// in the contructor, it would have worked for the first time only when the instance of 
        /// PMPrefrenceManager was created but we want to call each time the subModuleName property is changed.
        /// </summary>
        /// <returns></returns>
        private void GetPreferenceFilePath()
        {
            _startPath = Application.StartupPath;
            _preferencesPath = GetPreferenceDirectory();

            try
            {
                switch (_subModuleName)
                {
                    case CUSTOMVIEW:
                        if (!Directory.Exists(_preferencesPath))
                        {
                            Directory.CreateDirectory(_preferencesPath);

                        }
                        _preferencesFile = _preferencesPath + @"\CustomViewPreferences.xml";

                        if (!File.Exists(_preferencesFile))
                        {
                            SetDefaultPreferences();
                        }

                        break;

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

        #endregion

        #region Get Preferences

        /// <summary>
        /// Get the Preferences from either the preexisting xml file or get default preferences.
        /// Need to cast back to appropriate preferences class for taking the corresponding object.
        /// </summary>
        /// <returns></returns>
        public object GetPreferences()
        {
            FileStream fs = null;

            try
            {
                GetPreferenceFilePath();

                fs = File.OpenRead(_preferencesFile);

                switch (_subModuleName)
                {
                    case CUSTOMVIEW:
                        CustomViewPreferencesList customViewPreferences;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(CustomViewPreferencesList));
                        customViewPreferences = (CustomViewPreferencesList)serializer.Deserialize(fs);

                        return customViewPreferences;

                    default:
                        return null;
                }
            }
            catch (Exception)
            {

                return null;

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

        public void GetPreferences<T>(ref T preference)
        {
            FileStream fs = null;

            try
            {
                GetPreferenceFilePath();

                fs = File.OpenRead(_preferencesFile);

                switch (_subModuleName)
                {
                    case CUSTOMVIEW:
                        //CustomViewPreferencesList customViewPreferences;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(T));
                        preference = (T)serializer.Deserialize(fs);
                        break;
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
                //return null;

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

        public CustomViewPreferencesList GetCustomViewPreferences()
        {
            CustomViewPreferencesList list = null;
            try
            {
                GetPreferences(ref list);
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

        public bool SetCustomViewPreference(CustomViewPreferences preference, string key)
        {
            CustomViewPreferencesList list = null;
            bool result = false;

            try
            {
                GetPreferences(ref list);

                if (list != null) // && list.Count > 0
                {
                    if (!list.ContainsKey(key))
                    {
                        //if not found in existing collection then Add
                        list.Add(key, preference);

                    }
                    else
                    {
                        //else update respective GroupBy and Cols collections
                        list[key].GroupByColumnsCollection = preference.GroupByColumnsCollection;
                        list[key].SelectedColumnsCollection = preference.SelectedColumnsCollection;
                        list[key].SplitterPosition = preference.SplitterPosition;
                        list[key].FilterDetails = preference.FilterDetails;
                        list[key].IsDashboardVisible = preference.IsDashboardVisible;
                    }

                    result = SetPreferences(list);
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

        public bool SetCustomViewPreferenceList(Dictionary<string, CustomViewPreferences> dicCustomViewPreferences)
        {
            CustomViewPreferencesList list = null;
            bool result = false;

            try
            {
                GetPreferences(ref list);
                //string tabName = preference.TabName;

                if (list != null) // && list.Count > 0
                {
                    foreach (string key in dicCustomViewPreferences.Keys)
                    {
                        if (!list.ContainsKey(key))
                        {
                            //if not found in existing collection then Add
                            list.Add(key, dicCustomViewPreferences[key]);

                        }
                        else
                        {
                            //else update respective GroupBy and Cols collections
                            list[key].GroupByColumnsCollection = dicCustomViewPreferences[key].GroupByColumnsCollection;
                            list[key].SelectedColumnsCollection = dicCustomViewPreferences[key].SelectedColumnsCollection;
                            list[key].SplitterPosition = dicCustomViewPreferences[key].SplitterPosition;
                            list[key].FilterDetails = dicCustomViewPreferences[key].FilterDetails;
                            list[key].IsDashboardVisible = dicCustomViewPreferences[key].IsDashboardVisible;
                        }
                    }

                    result = SetPreferences(list);
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

        #endregion Get Preferences

        #region Set Preferences

        /// <summary>
        /// Set the respective submodule Preferences at the specified _preferencesFile path
        /// </summary>
        /// <param name="_preferences"></param>
        /// <returns></returns>
        public bool SetPreferences<T>(T _preferences)
        {
            XmlTextWriter writer = new XmlTextWriter(_preferencesFile, Encoding.UTF8);
            try
            {
                //using ()
                //{
                writer.Formatting = Formatting.Indented;

                switch (_subModuleName)
                {
                    case CUSTOMVIEW:
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(writer, _preferences);
                        serializer = null;
                        break;
                }
                //}
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

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

        #region Set Default Preferences

        /// <summary>
        /// Set the default sub module Preferences on the basis of _subModuleName set.
        /// </summary>
        /// <returns></returns>
        public void SetDefaultPreferences()
        {
            if (SubModuleName != null)
            {
                switch (SubModuleName)
                {
                    case CUSTOMVIEW:

                        CustomViewPreferencesList customViewPreferences = new CustomViewPreferencesList();
                        SetPreferences(customViewPreferences);
                        break;

                }
            }

        }

        public void GetPMDefaultPreferences(ref CustomViewPreferences customViewPreferences)
        {
            try
            {
                string xmlpath = GetPreferenceDirectory() + "\\" + @"PMDefaultPreferences.xml";
                //if (!File.Exists(xmlpath))
                //{
                xmlpath = _startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + @"PMDefaultPreferences.xml";
                //}
                PreferenceGridColumn currentColumn = null;
                XmlTextReader myxmlreader = new XmlTextReader(xmlpath);
                GridColumnFilterDetails dynamicFilterDetails = null;
                //_dynamicFileds = new List<string>();
                while (myxmlreader.Read())
                {
                    myxmlreader.MoveToContent();

                    if (myxmlreader.NodeType == XmlNodeType.Element)
                    {
                        switch (myxmlreader.Name)
                        {
                            case "SplitterPosition":
                                string SplitterPosition = myxmlreader.ReadString();
                                customViewPreferences.SplitterPosition = int.Parse(SplitterPosition);
                                break;
                            case "SelectedColumnsCollection":
                                break;
                            case "Column":
                                break;
                            case "Name":
                                currentColumn = new PreferenceGridColumn();
                                currentColumn.Name = myxmlreader.ReadString();

                                break;
                            case "Position":
                                currentColumn.Position = int.Parse(myxmlreader.ReadString());

                                break;
                            case "Width":
                                currentColumn.Width = int.Parse(myxmlreader.ReadString());

                                break;
                            case "IsHeaderFixed":
                                currentColumn.IsHeaderFixed = bool.Parse(myxmlreader.ReadString());

                                break;
                            case "FilterConditionList":
                                //currentColumn.FilterConditionList = bool.Parse(myxmlreader.ReadString());

                                break;
                            case "FilterLogicalOperator":
                                //currentColumn.FilterConditionList = bool.Parse(myxmlreader.ReadString());

                                break;
                            case "ColumnType":
                                currentColumn.ColumnType = myxmlreader.ReadString();
                                customViewPreferences.SelectedColumnsCollection.Add(currentColumn);
                                break;

                            case "GroupByColumnsCollection":
                                myxmlreader.ReadString();
                                string GroupByColumn = myxmlreader.ReadString();
                                customViewPreferences.GroupByColumnsCollection.Add(GroupByColumn);// = int.Parse(SplitterPosition);
                                break;
                            case "SortIndicator":

                                currentColumn.SortIndicator = (SortIndicator)Enum.Parse(typeof(SortIndicator), myxmlreader.ReadString(), true);
                                break;
                            case "FilterDetails":
                                dynamicFilterDetails = new GridColumnFilterDetails();
                                break;
                            case "FilterColumnKey":
                                dynamicFilterDetails.FilterColumnKey = myxmlreader.ReadString();
                                break;
                            case "DynamicFilterConditionList":
                                break;
                        }
                    }
                }


                myxmlreader.Close();
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


        #endregion Get Default Preferences

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _startPath = null;
                _preferencesPath = null;
                _preferencesFile = null;
                _subModuleName = null;
                _loginUser = null;
                _pmPreferencesManager = null;
            }
        }
        #endregion
    }
}


