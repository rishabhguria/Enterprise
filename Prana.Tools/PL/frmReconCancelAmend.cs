using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.Enums;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace Prana.Tools
{
    public partial class frmReconCancelAmend : Form, IPluggableTools, ILaunchForm, IPublishing
    {
        //TODO: Create regions for each section e.g. member variable, private methods, public methods

        Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate = null;
        Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate = null;
        Form _frmDateDetails = null;

        AuthAction _permissionLevel = AuthAction.None;

        public static event EventHandler DisableReconOutput = delegate { };
        public static event EventHandler DisableApproveChanges = delegate { };
        public static event EventHandler DisableMarkPriceAppend = delegate { };

        public frmReconCancelAmend()
        {
            try
            {
                InitializeComponent();
                DisableEnableForm();
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

        private void DisableEnableForm()
        {
            try
            {
                //Modified by : Pranay Deep 09 Oct 2015
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-11218
                //Hide manage marks, manage marks and approve changes tab.
                ultraTabControl3.Tabs["tbManageMarks"].Visible = false;
                ultraTabControl3.Tabs["tbManageForex"].Visible = false;
                ultraTabControl3.Tabs["tbApproveChanges"].Visible = false;
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

        #region ILaunchForm Members

        public event EventHandler LaunchForm;

        #endregion

        #region IPluggableTools Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        ISecurityMasterServices _secMaster = null;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _secMaster = value;
                NewUtilities.SecurityMaster = _secMaster;
                Prana.ReconciliationNew.SecMasterHelper.SecurityMaster = _secMaster;
                ctrlMarkPriceAppend1.SecurityMaster = _secMaster;

            }
        }

        public void SetUP()
        {
            CreateSubscriptionServicesProxy();
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion






        private void frmReconCancelAmend_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ctrlReconDashboard1.StopDashBoradDataRefreshing();
                //do not save changes if user dont have permission to save
                if (_permissionLevel == AuthAction.Write || _permissionLevel == AuthAction.Approve)
                {
                    ctrlReconTemplate1.UpdateDataForSelectedTab();

                    if (ctrlReconTemplate1.IsUnsavedChanges())
                    {

                        DialogResult result = MessageBox.Show("There are some unsaved changes, Do you want to save?", "Reconciliation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                        if (DialogResult.Yes.Equals(result))
                        {
                            // ctrlReconTemplate1.UpdateDataForSelectedTab();
                            btnSave_Click(null, null);
                            //CHMW-3160	[Recon] Amendments are not saving if a column is under different group
                            if (ctrlReconTemplate1.IsUnsavedChanges())
                            {
                                ctrlReconTemplate1._isUnSavedChanges = true;
                                e.Cancel = true;
                                ctrlReconDashboard1.ReStartTimer();
                            }
                        }
                        else
                        {
                            //load last Saved Preferences...
                            ReconPrefManager.GetPreferences();
                        }
                    }
                }
                //stop timer even if user has permission or not.

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

        private void frmReconCancelAmend_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
                }
                Dispose();
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


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Prana.Utilities.IO.File.IsFileOpen(ReconPrefManager.ReconPreferencesFilePath))
                {
                    //modified by amit on 16.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3396                 
                    ctrlReconTemplate1.UpdateDataForSelectedTab();
                    ctrlReconTemplate1.IsUnsavedChanges();
                    ctrlReconTemplate1.SaveTemplate();

                }
                else
                {
                    MessageBox.Show("Preference not Saved.\nFile already in use.", "Reconciliation", MessageBoxButtons.OK);

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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //load last Saved Preferences...
                //ReconPrefManager.GetPreferences();
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
        /// loads the date range form for input of reconciliation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunRecon_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ctrlReconTemplate1.isTemplateSelected())
                {
                    MessageBox.Show("Please select a template", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (_frmDateDetails == null)
                {
                    _frmDateDetails = new Form();
                    SetThemeAtDynamicForm(_frmDateDetails);
                }
                else
                {
                    _frmDateDetails.BringToFront();
                }
                // _frmDateDetails.Location = new System.Drawing.Point( (this.Width - _frmDateDetails.Width) / 2,(this.Height - _frmDateDetails.Height) / 2);
                _frmDateDetails.Show();

                //set the location  to the centre of recon form
                int x = this.Location.X + ((this.Width - _frmDateDetails.Width) / 2);
                int y = this.Location.Y + ((this.Height - _frmDateDetails.Height) / 2);
                _frmDateDetails.DesktopLocation = new Point(x, y);

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
        /// initialize the form designer with controls and Theme toolbars
        /// </summary>
        /// <param name="dynamicForm"></param>
        private void SetThemeAtDynamicForm(Form dynamicForm)
        {
            try
            {
                System.ComponentModel.IContainer dynamicComponents = new System.ComponentModel.Container();
                Infragistics.Win.Misc.UltraButton btnOK = new Infragistics.Win.Misc.UltraButton();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(dynamicComponents);
                Infragistics.Win.Misc.UltraPanel Form1_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
                dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
                Infragistics.Win.Misc.UltraLabel lblToDate = new Infragistics.Win.Misc.UltraLabel();
                dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
                Infragistics.Win.Misc.UltraLabel lblFromDate = new Infragistics.Win.Misc.UltraLabel();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                Infragistics.Win.Misc.UltraLabel lblHeader = new Infragistics.Win.Misc.UltraLabel();
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).BeginInit();
                Form1_Fill_Panel.ClientArea.SuspendLayout();
                Form1_Fill_Panel.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(dtToDate)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(dtFromDate)).BeginInit();
                dynamicForm.SuspendLayout();
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1.DesignerFlags = 1;
                ultraToolbarsManager1.DockWithinContainer = dynamicForm;
                ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
                ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
                ultraToolbarsManager1.IsGlassSupported = false;
                // 
                // Form1_Fill_Panel
                // 
                // 
                // Form1_Fill_Panel.ClientArea
                // 
                Form1_Fill_Panel.ClientArea.Controls.Add(btnOK);
                Form1_Fill_Panel.ClientArea.Controls.Add(lblHeader);
                Form1_Fill_Panel.ClientArea.Controls.Add(dtToDate);
                Form1_Fill_Panel.ClientArea.Controls.Add(lblToDate);
                Form1_Fill_Panel.ClientArea.Controls.Add(dtFromDate);
                Form1_Fill_Panel.ClientArea.Controls.Add(lblFromDate);
                Form1_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
                Form1_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                Form1_Fill_Panel.Location = new System.Drawing.Point(4, 27);
                Form1_Fill_Panel.Name = "Form1_Fill_Panel";
                Form1_Fill_Panel.Size = new System.Drawing.Size(536, 64);
                Form1_Fill_Panel.TabIndex = 0;
                // 
                // dtToDate
                // 
                dtToDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                dtToDate.Location = new System.Drawing.Point(346, 35);
                dtToDate.Name = "dtToDate";
                dtToDate.Size = new System.Drawing.Size(120, 22);
                dtToDate.TabIndex = 13;
                this.dtToDate.ValueChanged += new System.EventHandler(this.dtToDate_ValueChanged);
                // 
                // lblToDate
                // 
                lblToDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                lblToDate.Location = new System.Drawing.Point(269, 39);
                lblToDate.Name = "lblToDate";
                lblToDate.Size = new System.Drawing.Size(83, 18);
                lblToDate.TabIndex = 12;
                lblToDate.Text = "To Date";
                // 
                // dtFromDate
                // 
                dtFromDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                dtFromDate.Location = new System.Drawing.Point(114, 35);
                dtFromDate.Name = "dtFromDate";
                dtFromDate.Size = new System.Drawing.Size(120, 22);
                dtFromDate.TabIndex = 11;
                ReconType reconType = (ReconType)Enum.Parse(typeof(ReconType), ctrlReconTemplate1.GetReconType());
                if (reconType == ReconType.Position || reconType == ReconType.TaxLot)
                {
                    dtFromDate.Enabled = false;
                }
                // 
                // lblFromDate
                // 
                lblFromDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                lblFromDate.Location = new System.Drawing.Point(30, 39);
                lblFromDate.Name = "lblFromDate";
                lblFromDate.Size = new System.Drawing.Size(83, 18);
                lblFromDate.TabIndex = 10;
                lblFromDate.Text = "From Date";
                // 
                // _Form1_Toolbars_Dock_Area_Left
                // 
                _Form1_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                _Form1_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                _Form1_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
                _Form1_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
                _Form1_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
                _Form1_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
                _Form1_Toolbars_Dock_Area_Left.Name = "_Form1_Toolbars_Dock_Area_Left";
                _Form1_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 64);
                _Form1_Toolbars_Dock_Area_Left.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _Form1_Toolbars_Dock_Area_Right
                // 
                _Form1_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                _Form1_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                _Form1_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
                _Form1_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
                _Form1_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
                _Form1_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(540, 27);
                _Form1_Toolbars_Dock_Area_Right.Name = "_Form1_Toolbars_Dock_Area_Right";
                _Form1_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 64);
                _Form1_Toolbars_Dock_Area_Right.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _Form1_Toolbars_Dock_Area_Top
                // 
                _Form1_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                _Form1_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                _Form1_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
                _Form1_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
                _Form1_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
                _Form1_Toolbars_Dock_Area_Top.Name = "_Form1_Toolbars_Dock_Area_Top";
                _Form1_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(544, 27);
                _Form1_Toolbars_Dock_Area_Top.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _Form1_Toolbars_Dock_Area_Bottom
                // 
                _Form1_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                _Form1_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                _Form1_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
                _Form1_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
                _Form1_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
                _Form1_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 91);
                _Form1_Toolbars_Dock_Area_Bottom.Name = "_Form1_Toolbars_Dock_Area_Bottom";
                _Form1_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(544, 4);
                _Form1_Toolbars_Dock_Area_Bottom.ToolbarsManager = ultraToolbarsManager1;
                // 
                // lblHeader
                // 
                lblHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                lblHeader.Location = new System.Drawing.Point(30, 7);
                lblHeader.Name = "lblHeader";
                lblHeader.Size = new System.Drawing.Size(235, 23);
                lblHeader.TabIndex = 14;
                lblHeader.Text = "Reconciliation Date Range :";
                // 
                // btnOK
                // 
                btnOK.Location = new System.Drawing.Point(482, 35);
                btnOK.Name = "btnOK";
                btnOK.Size = new System.Drawing.Size(35, 23);
                btnOK.TabIndex = 15;
                btnOK.Text = "Ok";
                btnOK.Click += new System.EventHandler(btnOK_Click);
                // 
                // Form1
                //  
                dynamicForm.Owner = this.FindForm();
                dynamicForm.ShowInTaskbar = false;
                dynamicForm.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
                dynamicForm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                dynamicForm.ClientSize = new System.Drawing.Size(544, 95);
                dynamicForm.MaximumSize = new System.Drawing.Size(544, 95);
                dynamicForm.MinimumSize = new System.Drawing.Size(544, 95);
                // int x=this.Location.X+((this.Width - _frmDateDetails.Width) / 2);
                //int y=this.Location.Y+((this.Height - _frmDateDetails.Height) / 2);
                // Rectangle tempRect = new Rectangle(x, y, 544, 95);
                // //  Set the bounds of the form using the Rectangle object. 
                // frmDateDetails.DesktopBounds = tempRect;
                dynamicForm.Controls.Add(Form1_Fill_Panel);
                dynamicForm.Controls.Add(_Form1_Toolbars_Dock_Area_Left);
                dynamicForm.Controls.Add(_Form1_Toolbars_Dock_Area_Right);
                dynamicForm.Controls.Add(_Form1_Toolbars_Dock_Area_Bottom);
                dynamicForm.Controls.Add(_Form1_Toolbars_Dock_Area_Top);
                dynamicForm.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                dynamicForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDateDetails_FormClosing);
                dynamicForm.Load += new System.EventHandler(this.frmDateDetails_Load);
                dynamicForm.Name = "dynamicForm";
                dynamicForm.ShowIcon = false;
                dynamicForm.Text = "Reconciliation";
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).EndInit();
                Form1_Fill_Panel.ClientArea.ResumeLayout(false);
                Form1_Fill_Panel.ClientArea.PerformLayout();
                Form1_Fill_Panel.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(dtToDate)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(dtFromDate)).EndInit();
                dynamicForm.ResumeLayout(false);

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
        /// <summary>
        /// loadds the theme on the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDateDetails_Load(object sender, EventArgs e)
        {
            try
            {
                //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                //{
                CustomThemeHelper.SetThemeProperties(_frmDateDetails, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                //}
                //Publish(null, null);
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
        /// dateRange form closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDateDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _frmDateDetails = null;
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
        /// runs reconciliation on the dates entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //List<string> reconDateRange = new List<string>(new string[] { dtFromDate.DateTime.Date.ToString(ApplicationConstants.DateFormat), dtToDate.DateTime.Date.ToString(ApplicationConstants.DateFormat) });
                ReconParameters reconParameters = new ReconParameters();
                reconParameters.DTFromDate = dtFromDate.DateTime;
                reconParameters.DTToDate = dtToDate.DateTime;
                _frmDateDetails.Close();
                ctrlReconTemplate1.RunRecon(reconParameters);
                MessageBox.Show("Reconciliation process started. To see the progress, please go reconciliation dashboard UI.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //bgWorkerRecon.RunWorkerAsync(reconDateRange);
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
        /// Rename the Template chosen using InputBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRename_Click(object sender, EventArgs e)
        {
            try
            {
                //get template name
                //null or empty value if there is any issue
                string prevTemplateName = ctrlReconTemplate1.GetTemplateName();
                if (!string.IsNullOrEmpty(prevTemplateName))
                {
                    // Modified by Ankit Gupta on 1st October, 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1369
                    List<string> listOfTemplatesOnDashboard = ctrlReconDashboard1.GetListOfTemplatesOnDashboard();
                    if (listOfTemplatesOnDashboard != null && listOfTemplatesOnDashboard.Contains(prevTemplateName))
                    {
                        MessageBox.Show("Template in use cannot be modified.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }

                    //inputs new value
                    string newTemplateName = Microsoft.VisualBasic.Interaction.InputBox("Enter New Name:", "Reconciliation Template Rename", prevTemplateName).Trim();
                    //check if new name is Equal to previous name or user pressed cancel button
                    if (!string.IsNullOrEmpty(newTemplateName))
                    {
                        //changes the template name
                        ctrlReconTemplate1.SetTemplateName(prevTemplateName, newTemplateName);
                    }
                    else
                    {
                        MessageBox.Show("Template name cannot be empty or have blank spaces.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
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
        }

        private void frmReconCancelAmend_Load(object sender, System.EventArgs e)
        {
            try
            {
                SetEventinControls();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    ReconPrefManager.SetUp(Application.StartupPath);
                    ctrlReconTemplate1.InitializeReconTemplates();
                    //TODO: Move launch form in CachedDataManagerRecon from ReconPrefManager
                    ReconPrefManager.SetupLaunchForm(LaunchForm);
                    ReconManager.GetFileSettingDetails();
                    //Create the cache for all the recon 
                    CachedDataManagerRecon.CreateRunBatchDictionary(Application.StartupPath);
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        CustomThemeHelper.SetThemeProperties(ctrlAdHocRecon1, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RECON_NEW);
                        CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RECON_NEW);
                        ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);
                    }
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
                    }
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
        }

        private void SetButtonsColor()
        {
            try
            {
                btSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btSave.ForeColor = System.Drawing.Color.White;
                btSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btSave.UseAppStyling = false;
                btSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btCancel.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btCancel.ForeColor = System.Drawing.Color.White;
                btCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btCancel.UseAppStyling = false;
                btCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btRunRecon.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btRunRecon.ForeColor = System.Drawing.Color.White;
                btRunRecon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btRunRecon.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btRunRecon.UseAppStyling = false;
                btRunRecon.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btRename.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btRename.ForeColor = System.Drawing.Color.White;
                btRename.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btRename.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btRename.UseAppStyling = false;
                btRename.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void SetEventinControls()
        {
            try
            {
                DisableReconOutput += frmReconCancelAmend_DisableReconOutput;
                DisableApproveChanges += frmReconCancelAmend_DisableApproveChanges;
                //DisableMarkPriceAppend += frmReconCancelAmend_DisableMarkPriceAppend;


                ctrlAdHocRecon1.UpdateEvent(DisableApproveChanges);
                //ctrlMarkPriceAppend1.UpdateEvent(DisableApproveChanges);

                ctrlReconDashboard1.UpdateEvent(DisableReconOutput, DisableApproveChanges);
                ctrlApproveChanges1.UpdateEvent(DisableReconOutput, DisableMarkPriceAppend);
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

        //void frmReconCancelAmend_DisableMarkPriceAppend(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //ctrlMarkPriceAppend1.DisableControls(true);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmReconCancelAmend_DisableApproveChanges(object sender, EventArgs e)
        {
            try
            {
                ctrlApproveChanges1.DisableControls(true);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmReconCancelAmend_DisableReconOutput(object sender, EventArgs e)
        {
            try
            {
                ctrlAdHocRecon1.DisableControls();
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
        /// Intialize data on grid and set Grid Disabled or enabled
        /// After amendments and approval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (e.Tab.Key == "tbReports")
                {
                    ctrlReport1.InitializeDataSources();
                }
                //if (e.Tab.Key == "tbSetUpRunRecon")
                //{
                //    ultraTabControl2_SelectedTabChanged(null, null);
                //}
                //if (e.Tab.Key == "tbManageCancelAmends" && (ctrlAdHocRecon1.IsExceptionReportGenerated || ctrlReconTemplate1.IsExceptionReportGenerated))
                //{
                //ctrlApproveChanges1.DisableControls(true);
                // set the value to false after disabling the grid
                //ctrlReconTemplate1.IsExceptionReportGenerated = false;
                //ctrlAdHocRecon1.IsExceptionReportGenerated = false;
                //}
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
        /// Intialize data on grid and set Grid Disabled or enabled
        /// After amendments and approval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraTabControl2_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (e.Tab.Key == "tbRunAndManageRecon")
                {
                    ctrlAdHocRecon1.InitializeDataSourcesOfCombo();
                    ///if (ctrlApproveChanges1.IsChangesApproved || ctrlReconDashboard1.IsExceptionReportGenerated)
                    //{
                    //ctrlAdHocRecon1.InitializeDataSources();
                    //ctrlApproveChanges1.IsChangesApproved = false;
                    //ctrlReconDashboard1.IsExceptionReportGenerated = false;
                    //}
                    //else
                    //{
                    //ctrlAdHocRecon1.InitializeDataSources();
                    //}
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
        /// <summary>
        /// Intialize data on grid and set Grid Disabled or enabled
        /// After amendments and approval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraTabControl3_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                //if (e.Tab.Key == "tbApproveChanges" && ctrlReconDashboard1.IsExceptionReportGenerated)
                //{
                //ctrlApproveChanges1.DisableControls(true);
                //ctrlReconDashboard1.IsExceptionReportGenerated = false;
                //}
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
        /// Intialize data on grid and set Grid Disabled or enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraTabControl4_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                //if (e.Tab.Key == "tbSetForex")
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


        //private void bgWorkerRecon_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        //TODO: method of a control accessed, improve the code and make it thread safe.
        //        ctrlReconTemplate1.RunRecon(e.Argument as List<string>);
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

        public void CreateProxies()
        {
            CreateSubscriptionServicesProxy();
        }

        bool _isAlreadyAskedToUpdatePreference = false;
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-2079
        //  After changing recon preference by other user multiple times, pop should come only once to other users
        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void PromptToSavePreference()
        {
            try
            {
                _isAlreadyAskedToUpdatePreference = true;
                if (MessageBox.Show("Recon preference changed by another user. Do you want to update preference.", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    //modified by amit on 17.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3396
                    ReloadPreference();
                }
                else
                {
                    //TODO: _isUnSavedChanges property should not be used directly
                    //use setter and getter
                    ctrlReconTemplate1._isUnSavedChanges = true;
                }
                _isAlreadyAskedToUpdatePreference = false;
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
        /// Reload Preference
        /// </summary>
        //modified by amit on 17.04.2015
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3396
        private void ReloadPreference()
        {
            try
            {
                ReconPrefManager.GetPreferences();
                if (ReconPrefManager.ReconPreferences != null)
                {
                    if (ctrlReconTemplate1.isTemplateSelected())
                    {
                        ctrlReconTemplate1.LoadDataForSelectedTab();
                    }
                }
                else
                {
                    if (DialogResult.Retry == MessageBox.Show("Failed to update preference.", "Alert", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question))
                    {
                        ReloadPreference();
                    }
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



        #region IPublishing Members

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                object[] publishClientID = null;
                //List<TaxLot> taxlotsList = new List<TaxLot>();
                if (topicName == Topics.Topic_ReconPreferenceUpdated)
                {
                    if (e != null)
                    {
                        publishClientID = (object[])e.EventData;
                        if ((int)publishClientID[0] != CachedDataManager.GetInstance.LoggedInUser.CompanyUserID)
                        {
                            if (!_isAlreadyAskedToUpdatePreference)
                            {
                                this.BeginInvoke((Action)(() => PromptToSavePreference()));
                            }
                        }
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

        public string getReceiverUniqueName()
        {
            return "frmReconCancelAmend";
        }

        #endregion

        #region Subscribe Section
        DuplexProxyBase<ISubscription> _proxy;
        public void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_ReconPreferenceUpdated, null);
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

        #endregion
        #region IDisposable Members
        // <summary>
        /// Clean up any resources being used.
        ///TODO: Microsoft managed rule violated, dispose already implemented in designer to dispose component model
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_proxy != null)
                    {
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_ReconPreferenceUpdated);
                        _proxy.Dispose();
                    }
                    if (components != null)
                    {
                        components.Dispose();
                        base.Dispose(disposing);
                        DisableReconOutput -= frmReconCancelAmend_DisableReconOutput;
                        DisableApproveChanges -= frmReconCancelAmend_DisableApproveChanges;
                        //DisableMarkPriceAppend -= frmReconCancelAmend_DisableMarkPriceAppend;
                    }
                    if (dtToDate != null)
                    {
                        dtToDate.Dispose();
                    }
                    if (dtFromDate != null)
                    {
                        dtFromDate.Dispose();
                    }
                    if (_frmDateDetails != null)
                    {
                        _frmDateDetails.Dispose();
                    }
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
        }
        #endregion

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ReconType reconType = (ReconType)Enum.Parse(typeof(ReconType), ctrlReconTemplate1.GetReconType());
                if (reconType == ReconType.Position || reconType == ReconType.TaxLot)
                {
                    dtFromDate.Value = dtToDate.Value;
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
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
