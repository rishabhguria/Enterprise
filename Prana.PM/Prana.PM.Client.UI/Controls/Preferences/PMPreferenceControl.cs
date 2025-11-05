using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls.Preferences
{
    public partial class PMPreferenceControl : UserControl, IPreferences
    {
        PMPreferenceData PMPrefdata_default = new PMPreferenceData();
        PMPreferenceData PMPrefdata = new PMPreferenceData();

        CompanyUser UserID = new CompanyUser();

        public PMPreferenceControl()
        {
            InitializeComponent();
        }

        #region IPreferences Members

        public void SetUp(CompanyUser user)
        {
            try
            {
                UserID = user;
                PMPrefdata = GetFromDB(UserID.CompanyUserID);
                this.spnPercentAvgVol.Value = PMPrefdata.XPercentofAvgVolume;
                this.chbIsShowPMToolbar.Checked = PMPrefdata.IsShowPMToolbar;
                //this.chkClosingMark.Checked = PMPrefdata.UseClosingMark;
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
            //    restore default values from PMPrefdata_default
            PMPrefdata.UseClosingMark = PMPrefdata_default.UseClosingMark;
            PMPrefdata.XPercentofAvgVolume = PMPrefdata_default.XPercentofAvgVolume;
            PMPrefdata.IsShowPMToolbar = PMPrefdata_default.IsShowPMToolbar;
        }

        public IPreferenceData GetPrefs()
        {

            return PMPrefdata;


        }


        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;

            }
        }

        #endregion

        //private void chkClosingMark_click(object sender, EventArgs e)
        //{
        //    PMPrefdata.UseClosingMark = chkClosingMark.Checked;

        //}



        private PMPreferenceData GetFromDB(int companyuserID)
        {

            //PMDatabaseManager d = new PMDatabaseManager();
            return PMDatabaseManager.GetPMPrefDataFromDB(companyuserID);
            //Set PMPrefdata values from DB
        }
        private void SaveInDB(int companyUserID)
        {

            //PMDatabaseManager d= new PMDatabaseManager();
            PMDatabaseManager.SavePMPrefDatainDB(companyUserID, PMPrefdata.UseClosingMark, PMPrefdata.XPercentofAvgVolume, PMPrefdata.IsShowPMToolbar);
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

        private void spnPercentAvgVol_ValueChanged(object sender, EventArgs e)
        {
            PMPrefdata.XPercentofAvgVolume = spnPercentAvgVol.Value;

        }


        /// <summary>
        /// Handles the CheckedChanged event of the checkBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void chbIsShowPMToolbar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                PMPrefdata.IsShowPMToolbar = chbIsShowPMToolbar.Checked;
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