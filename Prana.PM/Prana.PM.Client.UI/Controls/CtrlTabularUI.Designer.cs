using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlTabularUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.grpBoxGrid = new Infragistics.Win.Misc.UltraGroupBox();
            this.grdPivotDisplay = new PranaUltraGrid();
            this.dtDateMonth = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.grpSelectDateMethodology = new Infragistics.Win.Misc.UltraGroupBox();
            this.optMonthly = new System.Windows.Forms.RadioButton();
            this.optWeekly = new System.Windows.Forms.RadioButton();
            this.optDaily = new System.Windows.Forms.RadioButton();
            this.ultrapanelTop = new Infragistics.Win.Misc.UltraPanel();
            this.grpBoxLiveFeedHandling = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxGrid)).BeginInit();
            this.grpBoxGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPivotDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSelectDateMethodology)).BeginInit();
            this.grpSelectDateMethodology.SuspendLayout();
            this.ultrapanelTop.ClientArea.SuspendLayout();
            this.ultrapanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxLiveFeedHandling)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBoxGrid
            // 
            this.grpBoxGrid.Controls.Add(this.grdPivotDisplay);
            this.grpBoxGrid.Location = new System.Drawing.Point(3, 115);
            this.grpBoxGrid.Name = "grpBoxGrid";
            this.grpBoxGrid.Size = new System.Drawing.Size(771, 331);
            this.grpBoxGrid.TabIndex = 2;
            // 
            // grdPivotDisplay
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdPivotDisplay.DisplayLayout.Appearance = appearance1;
            this.grdPivotDisplay.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPivotDisplay.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdPivotDisplay.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdPivotDisplay.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdPivotDisplay.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdPivotDisplay.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdPivotDisplay.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPivotDisplay.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdPivotDisplay.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdPivotDisplay.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdPivotDisplay.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdPivotDisplay.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdPivotDisplay.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdPivotDisplay.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdPivotDisplay.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdPivotDisplay.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdPivotDisplay.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdPivotDisplay.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdPivotDisplay.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdPivotDisplay.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdPivotDisplay.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdPivotDisplay.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdPivotDisplay.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdPivotDisplay.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdPivotDisplay.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdPivotDisplay.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
            this.grdPivotDisplay.Location = new System.Drawing.Point(6, 5);
            this.grdPivotDisplay.Name = "grdPivotDisplay";
            this.grdPivotDisplay.Size = new System.Drawing.Size(759, 320);
            this.grdPivotDisplay.TabIndex = 0;
            this.grdPivotDisplay.TabStop = false;
            this.grdPivotDisplay.Text = "ultraGrid1";
            this.grdPivotDisplay.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdPivotDisplay_CellDataError);
            // 
            // dtDateMonth
            // 
            this.dtDateMonth.Location = new System.Drawing.Point(7, 70);
            this.dtDateMonth.Name = "dtDateMonth";
            this.dtDateMonth.Size = new System.Drawing.Size(144, 21);
            this.dtDateMonth.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.dtDateMonth.TabIndex = 1;
            this.dtDateMonth.TabStop = false;
            // 
            // grpSelectDateMethodology
            // 
            this.grpSelectDateMethodology.Controls.Add(this.optMonthly);
            this.grpSelectDateMethodology.Controls.Add(this.dtDateMonth);
            this.grpSelectDateMethodology.Controls.Add(this.optWeekly);
            this.grpSelectDateMethodology.Controls.Add(this.optDaily);
            this.grpSelectDateMethodology.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpSelectDateMethodology.Location = new System.Drawing.Point(0, 0);
            this.grpSelectDateMethodology.Name = "grpSelectDateMethodology";
            this.grpSelectDateMethodology.Size = new System.Drawing.Size(175, 114);
            this.grpSelectDateMethodology.TabIndex = 0;
            this.grpSelectDateMethodology.Text = "Select Date Type";
            // 
            // optMonthly
            // 
            this.optMonthly.AutoSize = true;
            this.optMonthly.Location = new System.Drawing.Point(7, 47);
            this.optMonthly.Name = "optMonthly";
            this.optMonthly.Size = new System.Drawing.Size(62, 17);
            this.optMonthly.TabIndex = 2;
            this.optMonthly.Text = "Monthly";
            this.optMonthly.UseVisualStyleBackColor = true;
            // 
            // optWeekly
            // 
            this.optWeekly.AutoSize = true;
            this.optWeekly.Location = new System.Drawing.Point(7, 30);
            this.optWeekly.Name = "optWeekly";
            this.optWeekly.Size = new System.Drawing.Size(61, 17);
            this.optWeekly.TabIndex = 1;
            this.optWeekly.Text = "Weekly";
            this.optWeekly.UseVisualStyleBackColor = true;
            // 
            // optDaily
            // 
            this.optDaily.AutoSize = true;
            this.optDaily.Location = new System.Drawing.Point(7, 13);
            this.optDaily.Name = "optDaily";
            this.optDaily.Size = new System.Drawing.Size(48, 17);
            this.optDaily.TabIndex = 0;
            this.optDaily.Text = "Daily";
            this.optDaily.UseVisualStyleBackColor = true;
            // 
            // ultrapanelTop
            // 
            // 
            // ultrapanelTop.ClientArea
            // 
            this.ultrapanelTop.ClientArea.Controls.Add(this.grpBoxLiveFeedHandling);
            this.ultrapanelTop.ClientArea.Controls.Add(this.grpSelectDateMethodology);
            this.ultrapanelTop.Location = new System.Drawing.Point(9, 0);
            this.ultrapanelTop.Name = "ultrapanelTop";
            this.ultrapanelTop.Size = new System.Drawing.Size(618, 114);
            this.ultrapanelTop.TabIndex = 3;
            // 
            // grpBoxLiveFeedHandling
            // 
            this.grpBoxLiveFeedHandling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxLiveFeedHandling.Location = new System.Drawing.Point(175, 0);
            this.grpBoxLiveFeedHandling.Name = "grpBoxLiveFeedHandling";
            this.grpBoxLiveFeedHandling.Size = new System.Drawing.Size(443, 114);
            this.grpBoxLiveFeedHandling.TabIndex = 1;
            this.grpBoxLiveFeedHandling.Text = "Update Prices";
            // 
            // CtrlTabularUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBoxGrid);
            this.Controls.Add(this.ultrapanelTop);
            this.Name = "CtrlTabularUI";
            this.Size = new System.Drawing.Size(777, 480);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxGrid)).EndInit();
            this.grpBoxGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPivotDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpSelectDateMethodology)).EndInit();
            this.grpSelectDateMethodology.ResumeLayout(false);
            this.grpSelectDateMethodology.PerformLayout();
            this.ultrapanelTop.ClientArea.ResumeLayout(false);
            this.ultrapanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxLiveFeedHandling)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.RadioButton optDaily;
        protected PranaUltraGrid grdPivotDisplay;
        protected Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtDateMonth;
        protected System.Windows.Forms.RadioButton optMonthly;
        protected System.Windows.Forms.RadioButton optWeekly;
        protected Infragistics.Win.Misc.UltraGroupBox grpBoxGrid;
        protected Infragistics.Win.Misc.UltraGroupBox grpSelectDateMethodology;
        protected Infragistics.Win.Misc.UltraPanel ultrapanelTop;
        protected Infragistics.Win.Misc.UltraGroupBox grpBoxLiveFeedHandling;
    }
}
