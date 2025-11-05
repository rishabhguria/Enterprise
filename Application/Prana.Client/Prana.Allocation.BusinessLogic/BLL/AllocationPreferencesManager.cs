using System;
using System.Xml.Serialization ;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.Collections.Generic;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for AllocationPreferencesManager.
	/// </summary>
	public class AllocationPreferencesManager
    {
        #region Private Variables
        static string _startPath = string.Empty;
		static string _allocationPreferencesPath =string.Empty;
		static string _allocationPreferencesFilePath=string.Empty;
		static string _allocationPreferencesDirectoryPath=string.Empty;
        static AllocationPreferences _oldAllocationPreferences = null;
        static int _userID = int.MinValue;
        #endregion
        public static void SetUp(int userId)
        {
            _userID = userId;

        }
        public AllocationPreferencesManager()
		{
			
		}

        /// <summary>
        /// Reads preferences from File and returns it
        /// </summary>
        /// <returns></returns>

        public static AllocationPreferences GetPreferences()
		{
			TextReader r=null;
			AllocationPreferences _allocationPreferences= new AllocationPreferences();
			_startPath = System.Windows.Forms.Application.StartupPath ;
            _allocationPreferencesDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + _userID.ToString();
			_allocationPreferencesFilePath = _allocationPreferencesDirectoryPath+ @"\AlloationPreference.xml" ;
			
		
			try
			{
				XmlSerializer s = new XmlSerializer( typeof( AllocationPreferences ) );

				if(!Directory.Exists(_allocationPreferencesDirectoryPath))
				{
					Directory.CreateDirectory(_allocationPreferencesDirectoryPath);
				
				}
				
				if(File.Exists(_allocationPreferencesFilePath))
				{
					r = new StreamReader(_allocationPreferencesFilePath);
					_allocationPreferences = (AllocationPreferences)s.Deserialize( r );
					r.Close();
				}
					//if No Preferences File Exist Take Default Preferences
				else 
				{
					_allocationPreferences=GetDefualtPreferences();
				}
                _oldAllocationPreferences = _allocationPreferences;
				return _allocationPreferences;
			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				if(r!=null)
				r.Close();
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				
				if (rethrow)
				{
					throw;			
				}
				return null;
			}				
			#endregion
		
		}

		public static void SavePreferences(AllocationPreferences allocationPreferences)
		{
			TextWriter w=null;
		
			try
			{
                _oldAllocationPreferences = allocationPreferences;
				XmlSerializer s = new XmlSerializer( typeof(AllocationPreferences) );
				w = new StreamWriter(_allocationPreferencesFilePath);
				s.Serialize( w, allocationPreferences );
				w.Close();
				
			}
				#region catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
				bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

				if(w!=null)
					w.Close();
				if (rethrow)
				{
					throw;			
				}
			}				
				#endregion
			
		}

		public static bool CompareDisplayColumns(object[] list1,object[] list2 )
		{
			
			int count1=list1.Length;
			int count2=list2.Length;
			if(count1!=count2)
				return false;
			
			for(int i=0;i<count1;i++)
			{
				
				DisplayColumn displayColumn1=(DisplayColumn) list1[i];
				DisplayColumn displayColumn2=(DisplayColumn) list2[i];
				if(displayColumn1.DisplayName !=displayColumn2.DisplayName)
					return false;
					

			}
			return true;
		}

		public  static AllocationPreferences GetDefualtPreferences()
		{
			AllocationPreferences _allocationPreferences= new AllocationPreferences();
			_allocationPreferences.RowProperties.UnAllocatedBackColor=System.Drawing.Color.FromArgb(64,64,64).ToArgb();
			
			List<string> UnAllocatedColumnNames=OrderFields.UnAllocaedDisplayColumns;
            foreach (string str in UnAllocatedColumnNames)
            {
                _allocationPreferences.FundType.UnAllocatedGridColumns.AddColumn(str);
                _allocationPreferences.StrategyType.UnAllocatedGridColumns.AddColumn(str);
            }

            List<string> GroupedColumnNames = OrderFields.GroupedDisplayColumns;
            foreach (string str in GroupedColumnNames)
            {
                _allocationPreferences.FundType.GroupedGridColumns.AddColumn(str);
                _allocationPreferences.StrategyType.GroupedGridColumns.AddColumn(str);
            }

            List<string> AllocatedColumnNames = OrderFields.AllocatedDisplayColumns;
            foreach (string str in AllocatedColumnNames)
            {
                _allocationPreferences.FundType.AllocatedGridColumns.AddColumn(str);
                _allocationPreferences.StrategyType.AllocatedGridColumns.AddColumn(str);
            }

			return _allocationPreferences;
			

		}

        public static AllocationPreferences AllocationPreferences
        {
            get {
                if (_oldAllocationPreferences == null)
                {
                    _oldAllocationPreferences = GetPreferences();
                }
                return _oldAllocationPreferences; }
        }

        
	}
}
