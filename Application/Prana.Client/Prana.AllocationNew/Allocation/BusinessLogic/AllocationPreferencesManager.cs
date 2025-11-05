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
using Prana.Utilities.XMLUtilities;
//using Prana.PostTrade;
using System.Data;
using Prana.CommonDataCache;
using System.Windows.Forms;

namespace Prana.AllocationNew
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
        static CustomXmlSerializer _Xml = new CustomXmlSerializer();
        #endregion

        ///// <summary>
        ///// Added to store no of master funds, PRANA-10389
        ///// </summary>
        //static int _noOfMasterFund;

        ///// <summary>
        ///// Event added to get no of master funds, PRANA-10389
        ///// </summary>
        //public static event EventHandler NoOfMasterFund;
        
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
			_allocationPreferencesFilePath = _allocationPreferencesDirectoryPath+ @"\AllocationPreference.xml" ;


            try
            {

                if (!Directory.Exists(_allocationPreferencesDirectoryPath))
                {
                    Directory.CreateDirectory(_allocationPreferencesDirectoryPath);
                }

                _allocationPreferences = GetAllocationPreferences(_allocationPreferences);

                _oldAllocationPreferences = _allocationPreferences;
                return _allocationPreferences;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                if (r != null)
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

        /// <summary>
        /// Added to get allocation preferences, PRANA-12209
        /// </summary>
        /// <param name="_allocationPreferences"></param>
        /// <returns></returns>
        private static AllocationPreferences GetAllocationPreferences(AllocationPreferences _allocationPreferences)
        {
            try
            {
                if (File.Exists(_allocationPreferencesFilePath))
                {
                    //r = new StreamReader(_allocationPreferencesFilePath);
                    _allocationPreferences = (AllocationPreferences)_Xml.ReadXml(_allocationPreferencesFilePath, new AllocationPreferences());
                    _allocationPreferences.AllocationDefaultList = CachedDataManager.GetInstance.GetAllocationDefaults();
                    //   _allocationPreferences.AccountingMethods.AccountingMethodsTable = CachedDataManager.GetInstance.GetAccountingMethodsTable();

                    //r.Close();
                }
                //if No Preferences File Exist Take Default Preferences
                else
                {
                    _allocationPreferences = GetDefualtPreferences();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _allocationPreferences;
        }
        
        /// <summary>
        /// Save the preferences 
        /// </summary>
        /// <param name="allocationPreferences">allocation perferences</param>
        /// <returns>true if preferences are saved else false</returns>
        public static bool SavePreferences(AllocationPreferences allocationPreferences)
	    {
		    try
		    {
                ////Modified to save preferences only if there is MasterFund when checkbox is checked. PRANA-10389
                //if (NoOfMasterFund != null)
                //    NoOfMasterFund(null,EventArgs.Empty);
                //if (_noOfMasterFund > 0 || !allocationPreferences.GeneralRules.isMasterFundRatioAllocation)
                //{
                    //Added to save splitPanelSize and Form Height and Width, PRANA-5836
                //Condition added to remove null exception, PRANA-12209
                if (_oldAllocationPreferences == null)
                {
                    _oldAllocationPreferences = GetAllocationPreferences(_oldAllocationPreferences);
                }
                    allocationPreferences.SplitPanelSize = _oldAllocationPreferences.SplitPanelSize;
                    allocationPreferences.AllocationFormHeight = _oldAllocationPreferences.AllocationFormHeight;
                    allocationPreferences.AllocationFormWidth = _oldAllocationPreferences.AllocationFormWidth;
                    _oldAllocationPreferences = allocationPreferences;
                    _Xml.WriteFile(allocationPreferences, _allocationPreferencesFilePath, true);
                    return true;
                //}
                //else
                //{
                //    MessageBox.Show("Cannot enable MasterFund Ratio Allocation as there is no MasterFund.", "Nirvana Allocation Preferences", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}
		    }
		    #region catch
		    catch(Exception ex)
		    {
			    // Invoke our policy that is responsible for making sure no secure information
			    // gets out of our layer.
			    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                //if(fs!=null)
                //    fs.Close();
				if (rethrow)
				{
					throw;			
				}
                return false;
			}				
			#endregion
		}
        ///// <summary>
        ///// Added to set the value of global variable, PRANA-10389
        ///// </summary>
        ///// <param name="noOfMasterFunds"></param>
        //public static void SetMasterFundCount(int noOfMasterFunds)
        //{
        //    try
        //    {
        //        _noOfMasterFund = noOfMasterFunds;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

		

		public  static AllocationPreferences GetDefualtPreferences()
		{
			AllocationPreferences _allocationPreferences= new AllocationPreferences();
            try
            {
			_allocationPreferences.RowProperties.UnAllocatedBackColor=System.Drawing.Color.FromArgb(64,64,64).ToArgb();

            string unAllocatedColumns = String.Empty;
            string allocatedColumns = String.Empty;

            List<string> UnAllocatedColumnNames = AllocationConstants.UnAllocatedDisplayColumns;
            foreach (string str in UnAllocatedColumnNames)
            {
                unAllocatedColumns += str + ",";
            }

            List<string> AllocatedColumnNames = AllocationConstants.AllocatedDisplayColumns;
            foreach (string str in AllocatedColumnNames)
            {
                allocatedColumns += str + ",";
            }

            _allocationPreferences.UnAllocatedColumns = unAllocatedColumns;
            _allocationPreferences.AllocatedColumns = allocatedColumns;
            _allocationPreferences.AllocationDefaultList = CachedDataManager.GetInstance.GetAllocationDefaults();
            _allocationPreferences.GeneralRules.AllocateBasedonOpenPositions = true;

            //_allocationPreferences.AccountingMethods.AccountingMethodsTable = CachedDataManager.GetInstance.GetAccountingMethodsTable();

			return _allocationPreferences;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
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

      
        public static void Dispose()
        {
            try
            {
                _oldAllocationPreferences = null;          
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
	}
}
