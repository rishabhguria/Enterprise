using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.ThirdPartyReport
{
    /// <summary>
    /// Summary description for ThirdPartyReportMain.
    /// </summary>
    public class ThirdPartyReportMain : System.Windows.Forms.Form, IThirdPartyReport
    {
        private ThirdPartyReportControl thirdPartyReportControl;
        private IContainer components;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyReportMain_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyReportMain_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyReportMain_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private CompanyUser _loginUser;
        public ThirdPartyReportMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();


            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (thirdPartyReportControl != null)
                {
                    thirdPartyReportControl.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (_ThirdPartyReportMain_UltraFormManager_Dock_Area_Left != null)
                {
                    _ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_ThirdPartyReportMain_UltraFormManager_Dock_Area_Right != null)
                {
                    _ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_ThirdPartyReportMain_UltraFormManager_Dock_Area_Top != null)
                {
                    _ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (_ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThirdPartyReportMain));
            this.thirdPartyReportControl = new Prana.ThirdPartyReport.ThirdPartyReportControl();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // thirdPartyReportControl
            // 
            this.thirdPartyReportControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.thirdPartyReportControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thirdPartyReportControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.thirdPartyReportControl.Location = new System.Drawing.Point(4, 27);
            this.thirdPartyReportControl.Name = "thirdPartyReportControl";
            this.thirdPartyReportControl.Size = new System.Drawing.Size(992, 514);
            this.thirdPartyReportControl.TabIndex = 0;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ThirdPartyReportMain_UltraFormManager_Dock_Area_Left
            // 
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.Name = "_ThirdPartyReportMain_UltraFormManager_Dock_Area_Left";
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 514);
            // 
            // _ThirdPartyReportMain_UltraFormManager_Dock_Area_Right
            // 
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(996, 27);
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.Name = "_ThirdPartyReportMain_UltraFormManager_Dock_Area_Right";
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 514);
            // 
            // _ThirdPartyReportMain_UltraFormManager_Dock_Area_Top
            // 
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.Name = "_ThirdPartyReportMain_UltraFormManager_Dock_Area_Top";
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1000, 27);
            // 
            // _ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom
            // 
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 541);
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.Name = "_ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom";
            this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1000, 4);
            // 
            // ThirdPartyReportMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(1000, 545);
            this.Controls.Add(this.thirdPartyReportControl);
            this.Controls.Add(this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ThirdPartyReportMain_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThirdPartyReportMain";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Third Party Report ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThirdPartyReportMain_FormClosing);
            this.Load += new System.EventHandler(this.ThirdPartyReportMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region IThirdPartyReport Members

        public System.Windows.Forms.Form Reference()
        {
            return this;
        }

        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                thirdPartyReportControl.CompanyID = LoginUser.CompanyID;
                thirdPartyReportControl.CompanyUserID = LoginUser.CompanyUserID;
            }
        }
        public event EventHandler ThirdPartyFlatFileClosed;
        #pragma warning disable CS0067
        //Suppressing this warning as this event is not used in the current implementation but can't remove this event as it is part of the interface contract.
        public event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked;
        #pragma warning restore CS0067

        #endregion


        /// <summary>
        /// ON FORM CLOSING CHECK WHETHER WANTS TO SAVE THE CHANGES MADE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThirdPartyReportMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            int messageme = thirdPartyReportControl.OnClosingChangesSaved();
            // POPUP COMES ON FORM CLOSING IF SOME CHANGES MADE IN THE GRID DATA
            // YES , NO AND CANCEL OPTIONS COME,IF USER SAYS NO, METHOD RETURNS VALUE 1,SAYS YES RETURNS 2 AND 3 IN CASE OF CANCEL
            // messageme==3 means do not close else close the UI
            if (messageme == 3)
            {
                e.Cancel = true;
            }
            else
            {
                if (ThirdPartyFlatFileClosed != null)
                    ThirdPartyFlatFileClosed(this, EventArgs.Empty);

            }
        }

        private void ThirdPartyReportMain_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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
    }
}
