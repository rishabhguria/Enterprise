using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.PositionManagement
{
    public partial class PositionMgmtPreferenceControl : UserControl, IPreferencesSavedClicked
    {
        DateTime snapShotDate;
        public PositionMgmtPreferenceControl()
        {
            InitializeComponent();
            snapShotDate = PositionDataManager.GetInstance().GetSnapShotDate();
            dtSnapShotDate.DateTime = snapShotDate;
        }

        #region IPreferences Members

        public void SetUp(CompanyUser user)
        {
            // throw new Exception("The method or operation is not implemented.");
        }

        public UserControl Reference()
        {
            return this;
        }

        public bool Save()
        {
            bool isSuccess = false;
            try
            {
                isSuccess = PositionDataManager.GetInstance().SaveSnapShotDate(dtSnapShotDate.DateTime.Date);
                if (isSuccess)
                {
                    snapShotDate = dtSnapShotDate.DateTime.Date;
                    MessageBox.Show("SnapShot Position Date Saved!");
                }
                if (SaveClicked != null)
                {

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isSuccess;
        }

        public void RestoreDefault()
        {
            dtSnapShotDate.DateTime = snapShotDate;
        }

        public IPreferenceData GetPrefs()
        {
            SnapShotPositionPreferences preferences = new SnapShotPositionPreferences();
            return preferences;
            //throw new Exception("The method or operation is not implemented.");
        }

        public event EventHandler SaveClicked;
        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;

            }
        }

        /// <summary>
        /// remove invalid preferences
        /// </summary>
        /// <returns>true to cancel closing event, false otherwise</returns>
        public bool RemoveInvalidNewPreferences()
        {
            // This method is required in allocation preferences so added it to IPreferencesSavedClicked interface
            // Defining this method here as it implements IPreferencesSavedClicked interface
            // returned false so that prefernces are closed in case of position management prerfernces
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7332
            return false;
        }

        #endregion
    }
}
