using Infragistics.Win.Misc;
using Prana.LogManager;
using Prana.TradeManager;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Prana.Blotter.Prefs
{
    /// <summary>
    /// Summary description for BlotterColorControl.
    /// </summary>
    public class BlotterColorControl : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public BlotterColorControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.Misc.UltraLabel label4;
        private Infragistics.Win.Misc.UltraLabel label5;
        private Infragistics.Win.Misc.UltraLabel label6;
        private Infragistics.Win.Misc.UltraLabel label7;
        private Infragistics.Win.Misc.UltraGroupBox groupBox2;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ddlCoverOrder;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ddlBuyOrder;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ddlShortOrder;
        private UltraGroupBox GroupBoxPreferences;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxApplyPreferencesOnTheme;
        private UltraGroupBox defaultTabGroupBox;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTabName;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxWrapHeader;
        private UltraLabel defaultExportPathLbl;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor defaultExportPath;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor Rejectionchkbx;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ddlSellOrder;

        public void SetUIBlotterPreferenceData(BlotterPreferenceData blotterPreferenceData)
        {
            try
            {
                ddlBuyOrder.Color = blotterPreferenceData.BuyOrder;
                ddlCoverOrder.Color = blotterPreferenceData.CoverOrder;
                ddlSellOrder.Color = blotterPreferenceData.SellOrder;
                ddlShortOrder.Color = blotterPreferenceData.ShortOrder;
                checkBoxApplyPreferencesOnTheme.Checked = blotterPreferenceData.ApplyColorPreferencesInTheme;
                checkBoxWrapHeader.Checked = blotterPreferenceData.WrapHeader;
                Rejectionchkbx.Checked = blotterPreferenceData.RejectionPopup;
                string _blotterPreferencePath = BlotterPreferenceManager.GetInstance().BlotterPreferencesPath;
                string[] files = System.IO.Directory.GetFiles(_blotterPreferencePath, "*BlotterGridLayout.xml");
                List<string> file = new List<string>();
                file.Add(BlotterConstants.TAB_NAME_ORDERS);
                file.Add(BlotterConstants.TAB_NAME_SUMMARY);
                file.Add(BlotterConstants.TAB_NAME_WORKINGSUBS);
                for (int i = 0; i < files.Length; i++)
                {
                    string tabKey = Path.GetFileName(files[i]).Replace("BlotterGridLayout.xml", "");
                    if (tabKey.StartsWith("Dynamic_") && !tabKey.StartsWith("Dynamic_SubOrder_"))
                    {
                        file.Add(tabKey.Replace("Dynamic_", String.Empty));
                    }
                }
                if (file.Contains(BlotterConstants.TAB_NAME_SUBORDERS))
                {
                    file.Remove(BlotterConstants.TAB_NAME_SUBORDERS);
                }

                cmbTabName.DataSource = file.ToArray();
                cmbTabName.Value = blotterPreferenceData.DefaultTabName;
                if (cmbTabName.Value == null)
                    cmbTabName.Value = BlotterConstants.TAB_NAME_ORDERS;

                defaultExportPath.Text = blotterPreferenceData.DefaultExportPath;

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

        public BlotterPreferenceData GetUIBlotterPreferenceData()
        {
            try
            {
                BlotterPreferenceData blotterPreferenceData = new BlotterPreferenceData();
                if (ddlBuyOrder.Color != null)
                {
                    blotterPreferenceData.BuyOrder = ddlBuyOrder.Color;
                }
                if (ddlCoverOrder.Color != null)
                {
                    blotterPreferenceData.CoverOrder = ddlCoverOrder.Color;
                }
                if (ddlSellOrder.Color != null)
                {
                    blotterPreferenceData.SellOrder = ddlSellOrder.Color;
                }
                if (ddlShortOrder.Color != null)
                {
                    blotterPreferenceData.ShortOrder = ddlShortOrder.Color;
                }
                if (cmbTabName.Value != null && !String.IsNullOrEmpty(cmbTabName.Value.ToString()))
                {
                    blotterPreferenceData.DefaultTabName = cmbTabName.Value.ToString();
                }

                blotterPreferenceData.ApplyColorPreferencesInTheme = checkBoxApplyPreferencesOnTheme.Checked;

                blotterPreferenceData.RejectionPopup = Rejectionchkbx.Checked;

                blotterPreferenceData.WrapHeader = checkBoxWrapHeader.Checked;

                blotterPreferenceData.DefaultExportPath = defaultExportPath.Text;

                return blotterPreferenceData;
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
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (label1 != null)
                {
                    label1.Dispose();
                }

                if (label4 != null)
                {
                    label4.Dispose();
                }

                if (label5 != null)
                {
                    label5.Dispose();
                }

                if (label6 != null)
                {
                    label6.Dispose();
                }

                if (label7 != null)
                {
                    label7.Dispose();
                }

                if (groupBox2 != null)
                {
                    groupBox2.Dispose();
                }

                if (ddlCoverOrder != null)
                {
                    ddlCoverOrder.Dispose();
                }

                if (ddlBuyOrder != null)
                {
                    ddlBuyOrder.Dispose();
                }

                if (ddlShortOrder != null)
                {
                    ddlShortOrder.Dispose();
                }

                if (GroupBoxPreferences != null)
                {
                    GroupBoxPreferences.Dispose();
                }

                if (checkBoxApplyPreferencesOnTheme != null)
                {
                    checkBoxApplyPreferencesOnTheme.Dispose();
                }

                if (defaultTabGroupBox != null)
                {
                    defaultTabGroupBox.Dispose();
                }

                if (cmbTabName != null)
                {
                    cmbTabName.Dispose();
                }

                if (checkBoxWrapHeader != null)
                {
                    checkBoxWrapHeader.Dispose();
                }

                if (defaultExportPathLbl != null)
                {
                    defaultExportPathLbl.Dispose();
                }

                if (defaultExportPath != null)
                {
                    defaultExportPath.Dispose();
                }

                if (Rejectionchkbx != null)
                {
                    Rejectionchkbx.Dispose();
                }

                if (ddlSellOrder != null)
                {
                    ddlSellOrder.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.ddlCoverOrder = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.ddlBuyOrder = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label5 = new Infragistics.Win.Misc.UltraLabel();
            this.ddlShortOrder = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label6 = new Infragistics.Win.Misc.UltraLabel();
            this.ddlSellOrder = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label7 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.GroupBoxPreferences = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkBoxApplyPreferencesOnTheme = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.defaultTabGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbTabName = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.checkBoxWrapHeader = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.defaultExportPathLbl = new Infragistics.Win.Misc.UltraLabel();
            this.defaultExportPath = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.Rejectionchkbx = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCoverOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBuyOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlShortOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlSellOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GroupBoxPreferences)).BeginInit();
            this.GroupBoxPreferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxApplyPreferencesOnTheme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultTabGroupBox)).BeginInit();
            this.defaultTabGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTabName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxWrapHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultExportPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rejectionchkbx)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dead Order";
            // 
            // ddlCoverOrder
            // 
            this.ddlCoverOrder.AllowEmpty = false;
            this.ddlCoverOrder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ddlCoverOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ddlCoverOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlCoverOrder.Location = new System.Drawing.Point(130, 46);
            this.ddlCoverOrder.Name = "ddlCoverOrder";
            this.ddlCoverOrder.Size = new System.Drawing.Size(115, 23);
            this.ddlCoverOrder.TabIndex = 7;
            this.ddlCoverOrder.Text = "Control";
            this.ddlCoverOrder.UseAppStyling = false;
            this.ddlCoverOrder.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Cover/ Buy To Close";
            // 
            // ddlBuyOrder
            // 
            this.ddlBuyOrder.AllowEmpty = false;
            this.ddlBuyOrder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ddlBuyOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ddlBuyOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlBuyOrder.Location = new System.Drawing.Point(130, 19);
            this.ddlBuyOrder.Name = "ddlBuyOrder";
            this.ddlBuyOrder.Size = new System.Drawing.Size(115, 23);
            this.ddlBuyOrder.TabIndex = 5;
            this.ddlBuyOrder.Text = "Control";
            this.ddlBuyOrder.UseAppStyling = false;
            this.ddlBuyOrder.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Buy/Buy To Open";
            // 
            // ddlShortOrder
            // 
            this.ddlShortOrder.AllowEmpty = false;
            this.ddlShortOrder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ddlShortOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ddlShortOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlShortOrder.Location = new System.Drawing.Point(130, 95);
            this.ddlShortOrder.Name = "ddlShortOrder";
            this.ddlShortOrder.Size = new System.Drawing.Size(115, 23);
            this.ddlShortOrder.TabIndex = 11;
            this.ddlShortOrder.Text = "Control";
            this.ddlShortOrder.UseAppStyling = false;
            this.ddlShortOrder.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Short/ Sell To Open";
            // 
            // ddlSellOrder
            // 
            this.ddlSellOrder.AllowEmpty = false;
            this.ddlSellOrder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ddlSellOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ddlSellOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlSellOrder.Location = new System.Drawing.Point(130, 70);
            this.ddlSellOrder.Name = "ddlSellOrder";
            this.ddlSellOrder.Size = new System.Drawing.Size(115, 23);
            this.ddlSellOrder.TabIndex = 9;
            this.ddlSellOrder.Text = "Control";
            this.ddlSellOrder.UseAppStyling = false;
            this.ddlSellOrder.ColorChanged += new System.EventHandler(this.clrPck_ColorChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "Sell/ Sell To Close";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.ddlSellOrder);
            this.groupBox2.Controls.Add(this.ddlCoverOrder);
            this.groupBox2.Controls.Add(this.ddlShortOrder);
            this.groupBox2.Controls.Add(this.ddlBuyOrder);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox2.Location = new System.Drawing.Point(8, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(254, 133);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.Text = "Open Blotter Text Color";
            // 
            // GroupBoxPreferences
            // 
            this.GroupBoxPreferences.Controls.Add(this.checkBoxWrapHeader);
            this.GroupBoxPreferences.Controls.Add(this.checkBoxApplyPreferencesOnTheme);
            this.GroupBoxPreferences.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.GroupBoxPreferences.Location = new System.Drawing.Point(273, 4);
            this.GroupBoxPreferences.Name = "GroupBoxPreferences";
            this.GroupBoxPreferences.Size = new System.Drawing.Size(254, 134);
            this.GroupBoxPreferences.TabIndex = 16;
            this.GroupBoxPreferences.Text = "Apply Preferences";
            // 
            // checkBoxApplyPreferencesOnTheme
            // 
            this.checkBoxApplyPreferencesOnTheme.Location = new System.Drawing.Point(12, 60);
            this.checkBoxApplyPreferencesOnTheme.Name = "checkBoxApplyPreferencesOnTheme";
            this.checkBoxApplyPreferencesOnTheme.Size = new System.Drawing.Size(220, 37);
            this.checkBoxApplyPreferencesOnTheme.TabIndex = 0;
            this.checkBoxApplyPreferencesOnTheme.Checked = true;
            this.checkBoxApplyPreferencesOnTheme.Visible = false;
            this.checkBoxApplyPreferencesOnTheme.Text = "Apply Color Preferences when theme applied";
            // 
            // defaultTabGroupBox
            // 
            this.defaultTabGroupBox.Controls.Add(this.cmbTabName);
            this.defaultTabGroupBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.defaultTabGroupBox.Location = new System.Drawing.Point(561, 11);
            this.defaultTabGroupBox.Name = "defaultTabGroupBox";
            this.defaultTabGroupBox.Size = new System.Drawing.Size(254, 134);
            this.defaultTabGroupBox.TabIndex = 17;
            this.defaultTabGroupBox.Text = "Default Open Tab";
            // 
            // cmbTabName
            // 
            this.cmbTabName.DropDownListWidth = -1;
            this.cmbTabName.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbTabName.Location = new System.Drawing.Point(73, 23);
            this.cmbTabName.Name = "cmbTabName";
            this.cmbTabName.Size = new System.Drawing.Size(107, 22);
            this.cmbTabName.TabIndex = 0;
            // 
            // checkBoxWrapHeader
            // 
            this.checkBoxWrapHeader.Location = new System.Drawing.Point(12, 23);
            this.checkBoxWrapHeader.Name = "checkBoxWrapHeader";
            this.checkBoxWrapHeader.Size = new System.Drawing.Size(220, 37);
            this.checkBoxWrapHeader.TabIndex = 1;
            this.checkBoxWrapHeader.Text = "Wrap Header";
            // 
            // defaultExportPath
            // 
            this.defaultExportPath.Location = new System.Drawing.Point(176, 144);
            this.defaultExportPath.Name = "defaultExportPath";
            this.defaultExportPath.Size = new System.Drawing.Size(351, 25);
            this.defaultExportPath.TabIndex = 19;
            // 
            // defaultExportPathLbl
            // 
            this.defaultExportPathLbl.Location = new System.Drawing.Point(12, 148);
            this.defaultExportPathLbl.Name = "defaultExportPathLbl";
            this.defaultExportPathLbl.Size = new System.Drawing.Size(178, 20);
            this.defaultExportPathLbl.TabIndex = 18;
            this.defaultExportPathLbl.Text = "Default Exporting Folder Path: ";
            // 
            // Rejectionchkbx
            // 
            this.Rejectionchkbx.Location = new System.Drawing.Point(12, 175);
            this.Rejectionchkbx.Name = "Rejectionchkbx";
            this.Rejectionchkbx.Size = new System.Drawing.Size(206, 25);
            this.Rejectionchkbx.TabIndex = 20;
            this.Rejectionchkbx.Text = "Show Rejection Pop Up Message";
            // 
            // BlotterColorControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.Rejectionchkbx);
            this.Controls.Add(this.defaultExportPath);
            this.Controls.Add(this.defaultExportPathLbl);
            this.Controls.Add(this.defaultTabGroupBox);
            this.Controls.Add(this.GroupBoxPreferences);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BlotterColorControl";
            this.Size = new System.Drawing.Size(847, 234);
            this.Load += new System.EventHandler(this.BlotterColorControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ddlCoverOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlBuyOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlShortOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlSellOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GroupBoxPreferences)).EndInit();
            this.GroupBoxPreferences.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxApplyPreferencesOnTheme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultTabGroupBox)).EndInit();
            this.defaultTabGroupBox.ResumeLayout(false);
            this.defaultTabGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTabName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxWrapHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultExportPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rejectionchkbx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void clrPck_ColorChanged(object sender, System.EventArgs e)
        {
            Infragistics.Win.UltraWinEditors.UltraColorPicker objUltraColorPicker = (Infragistics.Win.UltraWinEditors.UltraColorPicker)sender;

            objUltraColorPicker.Appearance.BackColor = objUltraColorPicker.Color;
            objUltraColorPicker.Appearance.BorderColor = objUltraColorPicker.Color;
            objUltraColorPicker.Appearance.ForeColor = objUltraColorPicker.Color;
        }

        private void BlotterColorControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
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