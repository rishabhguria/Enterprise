using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Nirvana.PNL1
{
	/// <summary>
	/// Singleton class which serialize or deserialize the LiveFeedPreferences. We need to provide 
	/// the instance of respective module Preferences while serializing and it returns the object 
	/// which can be further cast in the respective module to their respective module preferences.
	/// </summary>
	public class PNLPreferencesManager
	{

		#region Private fields

		string _startPath = string.Empty;
		string _pnlPreferencesPath = string.Empty;
		string _preferencesFile = string.Empty;

		#endregion

		#region Singleton Implementation of class

		private PNLPreferencesManager()
		{
			_startPath = System.Windows.Forms.Application.StartupPath;
			_pnlPreferencesPath = _startPath + @"\Nirvana Preferences\PNLPreferences" ;

			_preferencesFile = _pnlPreferencesPath + @"\PNLPreferences.xml" ;

			if(!Directory.Exists(_pnlPreferencesPath))
			{
				Directory.CreateDirectory(_pnlPreferencesPath);
				SetDefaultPreferences();
			}
		}

		private static PNLPreferencesManager _pnlPreferencesManager;
		public static PNLPreferencesManager GetInstance()
		{
			if(_pnlPreferencesManager == null)
			{
				_pnlPreferencesManager = new PNLPreferencesManager();
			}
			return _pnlPreferencesManager;
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
			XmlSerializer serializer = null;
			FileStream fs = null;

			if(!File.Exists(_preferencesFile))
				SetDefaultPreferences();

			fs = File.OpenRead(_preferencesFile);
			
			PNLPrefrencesData pnlPrefrencesData = null;
			serializer = new XmlSerializer(typeof(PNLPrefrencesData));
			pnlPrefrencesData = (PNLPrefrencesData)serializer.Deserialize(fs);

			fs.Flush();
			fs.Close();
			return pnlPrefrencesData ;
			
		}

		#endregion Get Preferences
		
		#region Set Preferences
		
		/// <summary>
		/// Set the respective submodule Preferences at the specified _preferencesFile path
		/// </summary>
		/// <param name="_newsPreferences"></param>
		/// <returns></returns>
		public bool SetPreferences(object _preferences)
		{
			
			try
			{
				XmlSerializer serializer = null;
				XmlTextWriter writer = new XmlTextWriter(_preferencesFile, System.Text.Encoding.ASCII);
				//writer.Formatting = Formatting.Indented;

				serializer = new XmlSerializer(typeof(PNLPrefrencesData));
				serializer.Serialize(writer, _preferences);

				writer.Flush();
				writer.Close();
				
			}
			catch(Exception ex)
			{
				return false;
			}
			return true;
		}

		#endregion Set Default Preferences

		#region Set Default Preferences

		/// <summary>
		/// Set the default Preferences
		/// </summary>
		/// <returns></returns>
		private void SetDefaultPreferences()
		{
			PNLPrefrencesData pnlPreferences = new PNLPrefrencesData();
			pnlPreferences.SetDefaultPreferences();
	
			SetPreferences(pnlPreferences);
		}

		#endregion Get Default Preferences



	}
}
