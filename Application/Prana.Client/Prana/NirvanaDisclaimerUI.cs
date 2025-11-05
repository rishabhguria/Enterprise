using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Prana
{
    /// <summary>
    /// Summary description for PranaHelp.
    /// </summary>
    public class NirvanaDisclaimerUI : System.Windows.Forms.Form
    {
        private Infragistics.Win.Misc.UltraButton button1;
        private Label label5;
        private IContainer components;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Panel AboutPrana_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Bottom;


        public NirvanaDisclaimerUI()
        {

            InitializeComponent();
            GetDisclaimerFromDB();
        }

        /// <summary>
        /// Get Disclaimer From DB
        /// </summary>
        private void GetDisclaimerFromDB()
        {
            try
            {
                DataSet pranaPref = GetPranaPreference();
                label5.Text = string.Empty;
                foreach (DataRow row in pranaPref.Tables[0].Rows)
                {
                    string PreferenceKey = row["PreferenceKey"].ToString();
                    string PreferenceValue = row["PreferenceValue"].ToString();
                    if (PreferenceKey.Equals("Disclaimer", StringComparison.OrdinalIgnoreCase))
                    {
                        label5.Text = PreferenceValue;
                        break;
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
        /// 
        /// </summary>
        /// <returns></returns>
        private DataSet GetPranaPreference()
        {
            DataSet ds = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPranaPreferences";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return ds;
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
                if (_AboutPrana_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _AboutPrana_UltraFormManager_Dock_Area_Bottom.Dispose();
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
                if (button1 != null)
                {
                    button1.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NirvanaDisclaimerUI));
            this.button1 = new Infragistics.Win.Misc.UltraButton();
            this.label5 = new System.Windows.Forms.Label();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AboutPrana_Fill_Panel = new System.Windows.Forms.Panel();
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
            this.button1.Location = new System.Drawing.Point(456, 341);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 21);
            this.button1.TabIndex = 11;
            this.button1.Text = "EXIT";
            this.button1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(519, 229);
            this.label5.TabIndex = 30;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AboutPrana_Fill_Panel
            // 
            this.AboutPrana_Fill_Panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AboutPrana_Fill_Panel.Controls.Add(this.button1);
            this.AboutPrana_Fill_Panel.Controls.Add(this.label5);
            this.AboutPrana_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AboutPrana_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutPrana_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.AboutPrana_Fill_Panel.Name = "AboutPrana_Fill_Panel";
            this.AboutPrana_Fill_Panel.Size = new System.Drawing.Size(536, 372);
            this.AboutPrana_Fill_Panel.TabIndex = 0;
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
            // NirvanaDisclaimerUI
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
            this.Name = "NirvanaDisclaimerUI";
            this.ShowInTaskbar = false;
            this.Text = "Disclaimer";
            this.Load += new System.EventHandler(this.PranaHelp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AboutPrana_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Prana Help Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PranaHelp_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.AboutPrana_Fill_Panel.BackgroundImage = WhiteLabelTheme.AboutBackGroundImage;
                this.Text = WhiteLabelTheme.AppTitle;
                this.Icon = WhiteLabelTheme.AppIcon;

                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_MISSING_TRADES);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Disclaimer", CustomThemeHelper.UsedFont);
                }
                this.label5.ForeColor = System.Drawing.Color.White;
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
