using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ReconciliationNew;
using System;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ReconPrefForm : Form
    {
        private const string TAB_MATCHING_RULES = "tabMatchingRules";
        private const string TAB_XSLT_MAPPING = "tabXSLT";
        private const string TAB_RECON_TEMPLATES = "ReconTemplates";

        /// <summary>
        /// TODO: Remove code from constructor and move in method shown or load
        /// </summary>
        public ReconPrefForm()
        {
            try
            {
                InitializeComponent();
                ReconPrefManager.SetUp(Application.StartupPath);
                ctrlReconTemplate1.InitializeReconTemplates();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        #region commented
        //private void tabCrtrlReconPrefs_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        //{
        //    string selectedTabKey = e.Tab.Key;
        //    switch (selectedTabKey)
        //    {
        //        case TAB_MATCHING_RULES:
        //            btnSaveTemplates.Visible = false;
        //            btnSave.Visible = true;
        //            btnCancel.Visible = true;
        //            //ctrlMatchingRules1.InitializeMatchingRulesTab();
        //            break;
        //        case TAB_XSLT_MAPPING:
        //             btnSaveTemplates.Visible = false;
        //            btnSave.Visible = true;
        //            btnCancel.Visible = true;
        //           // ctrlReconXSLTMapping1.InitializeXSLTMappingTab();
        //            break;
        //        case TAB_RECON_TEMPLATES:
        //            btnSaveTemplates.Visible = true;
        //            btnSave.Visible = false;
        //            btnCancel.Visible = false;
        //            Dictionary<string, ReconTemplates> dict = ReconPrefManager.ReconPreferences.DictReconTemplates;
        //            //GetReconTemplates();
        //            //BindTree(dict);

        //            break;
        //    }
        //}

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    // ctrlMatchingRules1.SaveMatchingRuleXML();
        //    //ctrlReconXSLTMapping1.SaveXSLTMappingXML();
        //    string XmlMatchingRulePath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString();
        //    //ReconPrefManager.ClearRuleCache();
        //    //  ReconPrefManager.SetUp(XmlMatchingRulePath);
        //    MessageBox.Show("Sucessfully Saved and Applied !   ", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    this.Close();

        //}
        #endregion

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                //modified by Pranay Deep 20 Oct 2015
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-11685
                ReconPreferences reconPref = ReconPrefManager.ReconPreferences;
                ReconUtilities reconUtilities = new ReconUtilities();
                reconUtilities.SaveReconPreferencesInDB(reconPref);

                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5460
                ctrlReconTemplate1.UpdateDataForSelectedTab();
                ctrlReconTemplate1.SaveTemplate();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //load last Saved Preferences...
                ReconPrefManager.GetPreferences();
                this.Close();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReconPrefForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ctrlReconTemplate1.UpdateDataForSelectedTab();

                if (ctrlReconTemplate1.IsUnsavedChanges())
                {

                    DialogResult result = MessageBox.Show("There are some unsaved changes, Do you want to save?", "Reconciliation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (DialogResult.Yes.Equals(result))
                    {
                        // ctrlReconTemplate1.UpdateDataForSelectedTab();
                        btnSaveTemplate_Click(null, null);
                    }
                    else
                    {
                        //load last Saved Preferences...
                        ReconPrefManager.GetPreferences();
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Load the form and make "Run Recon" button invisible if a user is a prana user.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-471
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ReconPrefForm_Load(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (CachedDataManager.GetPranaReleaseType() != PranaReleaseViewType.CHMiddleWare)
        //        {
        //            SetButtonsColor();
        //            btnRunRecon.Visible = false;
        //            CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RECONCILATION);
        //            if (CustomThemeHelper.ApplyTheme)
        //            {
        //                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
        //                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void SetButtonsColor()
        //{
        //    try
        //    {
        //        btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
        //        btnSave.ForeColor = System.Drawing.Color.White;
        //        btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
        //        btnSave.UseAppStyling = false;
        //        btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

        //        btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
        //        btnCancel.ForeColor = System.Drawing.Color.White;
        //        btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
        //        btnCancel.UseAppStyling = false;
        //        btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

        //        btnSaveTemplate.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
        //        btnSaveTemplate.ForeColor = System.Drawing.Color.White;
        //        btnSaveTemplate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        btnSaveTemplate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
        //        btnSaveTemplate.UseAppStyling = false;
        //        btnSaveTemplate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

        //        btnSaveTemplates.BackColor = System.Drawing.Color.FromArgb(104,156,46);
        //        btnSaveTemplates.ForeColor = System.Drawing.Color.White;
        //        btnSaveTemplates.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        btnSaveTemplates.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
        //        btnSaveTemplates.UseAppStyling = false;
        //        btnSaveTemplates.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

        //        btnCancelUpdate.BackColor = System.Drawing.Color.FromArgb(104,5,5);
        //        btnCancelUpdate.ForeColor = System.Drawing.Color.White;
        //        btnCancelUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        btnCancelUpdate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
        //        btnCancelUpdate.UseAppStyling = false;
        //        btnCancelUpdate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

        //        btnRunRecon.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
        //        btnRunRecon.ForeColor = System.Drawing.Color.White;
        //        btnRunRecon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        btnRunRecon.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
        //        btnRunRecon.UseAppStyling = false;
        //        btnRunRecon.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void btnRunRecon_Click(object sender, EventArgs e)
        {
            try
            {
                ReconParameters reconParameters = new ReconParameters();
                reconParameters.DTFromDate = DateTime.Now;
                reconParameters.DTToDate = DateTime.Now;
                //here date is hardcoded for today's date
                //List<string> reconDateRange = new List<string>(new string[] { DateTime.Now.Date.ToString(ApplicationConstants.DateFormat), DateTime.Now.Date.ToString(ApplicationConstants.DateFormat) });

                ctrlReconTemplate1.RunRecon(reconParameters);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


    }
}