using Prana.Utilities.UI.UIUtilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana
{
    /// <summary>
    /// Summary description for PranaHelp.
    /// </summary>
    public class PranaShortcuts : System.Windows.Forms.Form
    {

        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;

        private IContainer components;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Panel AboutPrana_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AboutPrana_UltraFormManager_Dock_Area_Bottom;

        public PranaShortcuts()
        {
            InitializeComponent();
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
                if (ultraGrid1 != null)
                {
                    ultraGrid1.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (AboutPrana_Fill_Panel != null)
                {
                    AboutPrana_Fill_Panel.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PranaShortcuts));
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AboutPrana_Fill_Panel = new System.Windows.Forms.Panel();
            this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._AboutPrana_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AboutPrana_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.AboutPrana_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AboutPrana_Fill_Panel
            // 
            this.AboutPrana_Fill_Panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AboutPrana_Fill_Panel.Controls.Add(this.ultraGrid1);
            this.AboutPrana_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AboutPrana_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutPrana_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.AboutPrana_Fill_Panel.Name = "AboutPrana_Fill_Panel";
            this.AboutPrana_Fill_Panel.Size = new System.Drawing.Size(536, 372);
            this.AboutPrana_Fill_Panel.TabIndex = 0;
            // 
            // ultraGrid1
            //             
            this.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
            this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.Override.CellPadding = 0;
            this.ultraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ultraGrid1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGrid1.Location = new System.Drawing.Point(0, 63);
            this.ultraGrid1.Name = "ultraGrid1";
            this.ultraGrid1.Size = new System.Drawing.Size(536, 309);
            this.ultraGrid1.TabIndex = 0;
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
            // PranaShortcuts
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
            this.Name = "PranaShortcuts";
            this.ShowInTaskbar = false;
            this.Text = "Available Shortcuts";
            this.Load += new System.EventHandler(this.PranaHelp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AboutPrana_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void PranaHelp_Load(object sender, System.EventArgs e)
        {
            this.AboutPrana_Fill_Panel.BackgroundImage = WhiteLabelTheme.AboutBackGroundImage;
            this.Text = WhiteLabelTheme.AppTitle;
            this.Icon = WhiteLabelTheme.AppIcon;

            Dictionary<string, string> dict = new Dictionary<string, string>()
             {
                 {"TT" ,"Ctrl + Shift + T"},
                 {"Blotter"," Ctrl + Shift + B"},
                 {"% TT", "Ctrl + Shift + E"},
                 {"Rebalancer", "Ctrl + Shift + R"},
                 {"Allocation", "Ctrl + Shift + A"},
                 {"Watchlist" , "Ctrl + Shift + W"},
                 {"Option Chain"  ,"Ctrl + Alt + O"},
                 {"Compliance Engine" ,"Ctrl + Shift + C"},
                 {"PM","Ctrl + Shift + P"},
                 {"Risk Analysis" ,"Ctrl + Alt + R"},
                 {"GL" ," Ctrl + Shift + G"},
                 {"SM" ,"Ctrl + Shift + S"},
                 {"Third Party Manager "," Ctrl + Alt + T"},
                 {"Blotter Execution Report ", "Ctrl + Alt + B"},
                 {"Audit Trail", "Ctrl + Alt + A"},
                 {"Daily Valuation"," Ctrl + Shift + D"},
                 {"Recon"," Ctrl + Alt + E"},
                 {"Closing"," Ctrl + Alt + C"},
                 {"Pricing Inputs","Ctrl + Alt + P"},
                 {"Data Mapping ","Ctrl + Alt + D"},
                 {"Import  "," Ctrl + Shift + I"},
                 {"Create Transaction "," Ctrl + Alt + S"},
                 {"Corporate Actions ","Ctrl + Alt + I"},
                 {"Preferences "," Ctrl + Alt + F"},
                 {"Short locate ","  Ctrl + Alt + L"},
                 {"Broker Connections"," Ctrl + Alt + K"}
             };
            ultraGrid1.DataSource = dict;
            ultraGrid1.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Module Name";
            ultraGrid1.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Shortcut";

            if (CustomThemeHelper.ApplyTheme)
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_SHORTCUTS);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Module Shortcuts", CustomThemeHelper.UsedFont);
            }

            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
