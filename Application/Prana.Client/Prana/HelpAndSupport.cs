using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana
{
    /// <summary>
    /// Summary description for PranaHelp.
    /// </summary>
    public class HelpAndSupport : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraButton button1;
        private Label lblEmailAddress;
        private Label lblWarning;
        private Label lblEmail;
        private Label lbllSupportContactDetails;

        private Prana.Utilities.XMLUtilities.Config _config = new Prana.Utilities.XMLUtilities.Config();
        private Label lblContactNumber;
        private IContainer components;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Panel AboutPrana_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Bottom;
        private Label lblContactNum;

        public HelpAndSupport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            LoadSupportDetails();
        }

        /// <summary>
        /// Loads the about box information.
        /// </summary>
        private void LoadSupportDetails()
        {
            try
            {
                _config.cfgFile = Application.StartupPath + "\\Prana.exe.config";

                lblContactNumber.Text = _config.GetValue("//appSettings//add[@key='SupportContact']");
                lblEmailAddress.Text = _config.GetValue("//appSettings//add[@key='SupportEmail']");
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
                if (button1 != null)
                {
                    button1.Dispose();
                }
                if (lblEmailAddress != null)
                {
                    lblEmailAddress.Dispose();
                }
                if (lblWarning != null)
                {
                    lblWarning.Dispose();
                }
                if (lblEmail != null)
                {
                    lblEmail.Dispose();
                }
                if (lblContactNum != null)
                {
                    lblContactNum.Dispose();
                }
                if (lbllSupportContactDetails != null)
                {
                    lbllSupportContactDetails.Dispose();
                }
                if (lblContactNumber != null)
                {
                    lblContactNumber.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Left != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Right != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Top != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (_AboutPrana_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (AboutPrana_Fill_Panel != null)
                {
                    AboutPrana_Fill_Panel.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpAndSupport));
            this.button1 = new Infragistics.Win.Misc.UltraButton();
            this.lblEmailAddress = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lbllSupportContactDetails = new System.Windows.Forms.Label();
            this.lblContactNumber = new System.Windows.Forms.Label();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AboutPrana_Fill_Panel = new System.Windows.Forms.Panel();
            this.lblContactNum = new System.Windows.Forms.Label();
            this._AboutPrana_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.AboutPrana_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(456, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 21);
            this.button1.TabIndex = 11;
            this.button1.Text = "EXIT";
            this.button1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.BackColor = System.Drawing.Color.Transparent;
            this.lblEmailAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailAddress.Location = new System.Drawing.Point(218, 162);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(340, 30);
            this.lblEmailAddress.TabIndex = 31;
            this.lblEmailAddress.Text = "mailID";
            // 
            // label5
            // 
            this.lblWarning.BackColor = System.Drawing.Color.Transparent;
            this.lblWarning.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.Location = new System.Drawing.Point(11, 259);
            this.lblWarning.Name = "label5";
            this.lblWarning.Size = new System.Drawing.Size(519, 69);
            this.lblWarning.TabIndex = 30;
            this.lblWarning.Text = resources.GetString("label5.Text");
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(60, 162);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(154, 29);
            this.lblEmail.TabIndex = 29;
            this.lblEmail.Text = "Email :";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbllSupportContactDetails
            // 
            this.lbllSupportContactDetails.BackColor = System.Drawing.Color.Transparent;
            this.lbllSupportContactDetails.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllSupportContactDetails.Location = new System.Drawing.Point(175, 113);
            this.lbllSupportContactDetails.Name = "lbllSupportContactDetails";
            this.lbllSupportContactDetails.Size = new System.Drawing.Size(242, 32);
            this.lbllSupportContactDetails.TabIndex = 28;
            this.lbllSupportContactDetails.Text = "Support Contact Details";
            // 
            // lblContactNumber
            // 
            this.lblContactNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblContactNumber.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContactNumber.Location = new System.Drawing.Point(218, 192);
            this.lblContactNumber.Name = "lblContactNumber";
            this.lblContactNumber.Size = new System.Drawing.Size(340, 27);
            this.lblContactNumber.TabIndex = 37;
            this.lblContactNumber.Text = "cntct";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AboutPrana_Fill_Panel
            // 
            this.AboutPrana_Fill_Panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AboutPrana_Fill_Panel.Controls.Add(this.button1);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblContactNumber);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblContactNum);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblEmailAddress);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblWarning);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lblEmail);
            this.AboutPrana_Fill_Panel.Controls.Add(this.lbllSupportContactDetails);
            this.AboutPrana_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AboutPrana_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutPrana_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.AboutPrana_Fill_Panel.Name = "AboutPrana_Fill_Panel";
            this.AboutPrana_Fill_Panel.Size = new System.Drawing.Size(536, 372);
            this.AboutPrana_Fill_Panel.TabIndex = 0;
            // 
            // lblContactNum
            // 
            this.lblContactNum.BackColor = System.Drawing.Color.Transparent;
            this.lblContactNum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContactNum.Location = new System.Drawing.Point(60, 192);
            this.lblContactNum.Name = "lblContactNum";
            this.lblContactNum.Size = new System.Drawing.Size(154, 26);
            this.lblContactNum.TabIndex = 34;
            this.lblContactNum.Text = "Contact Number :";
            this.lblContactNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Left
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Name = "_AboutPrana_UltraFormManager_Dock_Area_Left";
            this._AboutPrana_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 372);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Right
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(544, 32);
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Name = "_AboutPrana_UltraFormManager_Dock_Area_Right";
            this._AboutPrana_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 372);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Top
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Name = "_AboutPrana_UltraFormManager_Dock_Area_Top";
            this._AboutPrana_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(552, 32);
            // 
            // _AboutPrana_UltraFormManager_Dock_Area_Bottom
            // 
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 404);
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Name = "_AboutPrana_UltraFormManager_Dock_Area_Bottom";
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(552, 8);
            // 
            // HelpAndSupport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(552, 412);
            this.Controls.Add(this.AboutPrana_Fill_Panel);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AboutPrana_UltraFormManager_Dock_Area_Bottom);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(552, 412);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(552, 412);
            this.Name = "HelpAndSupport";
            this.ShowInTaskbar = false;
            this.Text = "Help And Support";
            this.Load += new System.EventHandler(this.HelpAndSupport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AboutPrana_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Handles the Load event of the HelpAndSupport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HelpAndSupport_Load(object sender, System.EventArgs e)
        {
            this.AboutPrana_Fill_Panel.BackgroundImage = WhiteLabelTheme.AboutBackGroundImage;
            this.Text = WhiteLabelTheme.AppTitle;
            this.Icon = WhiteLabelTheme.AppIcon;

            SetButtonsColor();
            CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_MISSING_TRADES);
            if (CustomThemeHelper.ApplyTheme)
            {
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Help And Support", CustomThemeHelper.UsedFont);
            }
            this.lblEmailAddress.ForeColor = System.Drawing.Color.White;
            this.lblWarning.ForeColor = System.Drawing.Color.White;
            this.lblEmail.ForeColor = System.Drawing.Color.White;
            this.lbllSupportContactDetails.ForeColor = System.Drawing.Color.White;
            this.lblContactNum.ForeColor = System.Drawing.Color.White;
            this.lblContactNumber.ForeColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:       
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                this.button1.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                this.button1.ForeColor = System.Drawing.Color.White;
                this.button1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                this.button1.UseAppStyling = false;
                this.button1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }


    }
}
