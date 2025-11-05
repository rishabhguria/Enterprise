using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.PNL.UI.Preferences;

namespace Prana.PNL.UI.Preferences
{
    /// <summary>
	/// Singleton class which serialize or deserialize the PNLPreferences. We need to provide 
	/// the instance of respective module Preferences while serializing and it returns the object 
	/// which can be further cast in the respective module to their respective module preferences.
	/// </summary>
	public class PNLPrefrenceManager :IDisposable
	{
	
		#region Private fields

		string _startPath = string.Empty;
		string _preferencesPath = string.Empty;
		string _preferencesFile = string.Empty;
		string _subModuleName = string.Empty;
        private string _userName = string.Empty;
        private const string CUSTOMVIEW = "PNL";

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
			get{return _subModuleName;}
			set
			{
				_subModuleName = value;
				///Call this function each time when the submodule name change 
				///as we need to change the filepath also 
				GetPreferenceFilePath();
				if(!Directory.Exists(_preferencesPath))
				{
					Directory.CreateDirectory(_preferencesPath);
					SetDefaultPreferences();
				}
			}
		}


		#endregion 

		#region Singleton Implementation of class

		public PNLPrefrenceManager(string subModuleName) //string subModuleName)
		{
		    _subModuleName = subModuleName;
		}

		private static PNLPrefrenceManager _pmPreferencesManager = null;
		public static PNLPrefrenceManager GetInstance(string subModuleName) 
		{
			if(_pmPreferencesManager == null)
			{
				_pmPreferencesManager = new PNLPrefrenceManager(subModuleName); 
			}
			return _pmPreferencesManager;
		}

		/// <summary>
		/// This is to get the latest path each time according to the submodule. If we had pasted it
		/// in the contructor, it would have worked for the first time only when the instance of 
		/// PMPrefrenceManager was created but we want to call each time the subModuleName property is changed.
		/// </summary>
		/// <returns></returns>
		private void GetPreferenceFilePath()
		{
			_startPath = Application.StartupPath ;
			_preferencesPath = _startPath + @"\Prana Preferences\PNL UI Preferences\";

            try
            {
                switch (_subModuleName)
                {
                    case CUSTOMVIEW:
                        _preferencesPath += _userName;
                        if (!Directory.Exists(_preferencesPath))
                        {
                            Directory.CreateDirectory(_preferencesPath);
                            
                        }
                        _preferencesFile = _preferencesPath + @"\Preferences.xml";

                        if(!File.Exists(_preferencesFile))
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
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

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

				switch(_subModuleName)
				{
					case CUSTOMVIEW :
						PNLPreferenceList customViewPreferences;
				        XmlSerializer serializer;
				        serializer = new XmlSerializer(typeof(PNLPreferenceList));
                        customViewPreferences = (PNLPreferenceList)serializer.Deserialize(fs);
						
						return customViewPreferences;

					default :
						return null;
				}
			}
			catch(Exception)
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
                        //PNLPreferencesList customViewPreferences;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(T));
                        preference = (T)serializer.Deserialize(fs);
                        break;
                        
                    //default:
                        //return null;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

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

        public void GetPreferences<T>(string userName, ref T preference)
        {
            _userName = userName;
            GetPreferences(ref preference);
        }

        public PNLPreferenceList GetCustomViewPreferences(string userName)
        {
            PNLPreferenceList list = null;
            try
            {
                GetPreferences(userName,ref list);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return list;
        }

        public bool SetCustomViewPreference(PNLPreference preference)
        {
            PNLPreferenceList list = null;
            bool result = false;

            try
            {
                GetPreferences(ref list);
                string tabName = preference.TabName;

                if(list != null)
                {
                    if (!list.ContainsKey(tabName))
                    {
                        //if not found in existing collection then Add
                        list.Add(tabName, preference);

                    }
                    else
                    {
                        //else update respective GroupBy and Cols collections
                        list[tabName].GroupByColumnsCollection = preference.GroupByColumnsCollection;
                        list[tabName].DeselectedColumnsCollection = preference.DeselectedColumnsCollection;
                    }

                    result = SetPreferences(list);
                }                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

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
			try
			{
			    using (XmlTextWriter writer = new XmlTextWriter(_preferencesFile, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;

                    switch (_subModuleName)
                    {
                        case CUSTOMVIEW:
                            XmlSerializer serializer;
                            serializer = new XmlSerializer(typeof (T));
                            serializer.Serialize(writer, _preferences);
                            break;
                    }

                    writer.Flush();
                    writer.Close();
                }
			}
			catch(Exception)
			{
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

			    return false;
			}

                return true;
           
		}

		#endregion Get Preferences

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

                        PNLPreferenceList customViewPreferences = new PNLPreferenceList();
                        SetPreferences(customViewPreferences);
                        break;

                }
            }

		}

		#endregion Get Default Preferences

		#region Convert Color object from 32 bit ARGB values and vice versa

        ///// <summary>
        ///// It receives the 32 bit ARGB value and returns the Color object corresponding to that
        ///// </summary>
        ///// <param name="ARGBValue"></param>
        ///// <returns></returns>
        //public Color GetColorFromARGB(string ARGBValue)
        //{
        //    Color computedColor = Color.Transparent;
        //    try
        //    {
        //        computedColor = Color.FromArgb(Convert.ToInt32(ARGBValue));

             
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return computedColor;

        //}

		#endregion  Convert Color object from 32 bit ARGB values and vice versa

		#region IDisposable Members

		public void Dispose()
		{
			_pmPreferencesManager = null;
			this.Dispose();
		}

		#endregion
	}
}


