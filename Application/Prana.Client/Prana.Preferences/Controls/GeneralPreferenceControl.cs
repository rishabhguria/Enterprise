using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Preferences
{
    public partial class GeneralPreferenceControl : UserControl, IPreferences
    {
        GeneralPreferenceData prefdata_default = new GeneralPreferenceData();
        GeneralPreferenceData prefdata = new GeneralPreferenceData();

        CompanyUser UserID = new CompanyUser();

        public GeneralPreferenceControl()
        {
            InitializeComponent();
        }

        #region IPreferences Members

        public void SetUp(CompanyUser user)
        {
            try
            {
                UserID = user;
                prefdata = GetFromDB(UserID.CompanyUserID);
                this.chbIsShowServiceIcons.Checked = prefdata.IsShowServiceIcons;
            }

            catch (Exception ex)
            {
                bool rethorw = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethorw)
                    throw;
            }
        }

        public UserControl Reference()
        {
            return this;
        }


        public void RestoreDefault()
        {
            prefdata.IsShowServiceIcons = prefdata_default.IsShowServiceIcons;
        }

        public IPreferenceData GetPrefs()
        {
            return prefdata;
        }



        public string SetModuleActive
        {
            get;
            set;
        }

        #endregion


        private GeneralPreferenceData GetFromDB(int companyuserID)
        {
            return GeneralDatabaseManager.GetPMPrefDataFromDB(companyuserID);
        }
        private void SaveInDB(int companyUserID)
        {
            GeneralDatabaseManager.SavePMPrefDatainDB(companyUserID, prefdata.IsShowServiceIcons);
        }



        #region IPreferences Members


        public bool Save()
        {
            try
            {
                SaveInDB(UserID.CompanyUserID);
            }
            catch (Exception ex)
            {
                bool rethow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethow)
                    throw;
            }
            //finally
            //{

            //}
            //TODO: check if saved in DB then return true
            return true;
        }

        #endregion


        /// <summary>
        /// Handles the CheckedChanged event of the checkBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void chbIsShowPMToolbar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                prefdata.IsShowServiceIcons = chbIsShowServiceIcons.Checked;
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
    }
}