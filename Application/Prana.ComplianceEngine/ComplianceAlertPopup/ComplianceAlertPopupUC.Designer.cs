using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using System.Windows.Forms;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    partial class ComplianceAlertPopupUC
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_dtListViewSource != null)
                {
                    _dtListViewSource.Dispose();
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultraPanelTop = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanelBottom = new FlowLayoutPanel();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.bottomMsgLabel = new Infragistics.Win.Misc.UltraLabel();
            this.exportButton = new Infragistics.Win.Misc.UltraButton();
            this.cancelButton = new Infragistics.Win.Misc.UltraButton();
            this.responseButton = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelGrid = new Infragistics.Win.Misc.UltraPanel();
            this.alertGrid = new PranaUltraGrid();
            this.titleUltraLabel = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.ultraPanelTop.ClientArea.SuspendLayout();
            this.ultraPanelTop.SuspendLayout();
            this.ultraPanelBottom.SuspendLayout();
            this.ultraPanelBottom.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.ultraPanelGrid.ClientArea.SuspendLayout();
            this.ultraPanelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alertGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanelTop.ClientArea
            // 
            this.ultraPanelTop.ClientArea.Controls.Add(this.titleUltraLabel);
            this.ultraPanelTop.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraPanelTop.Name = "ultraPanelTop";
            this.ultraPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanelTop.Size = new System.Drawing.Size(890, 25);
            this.ultraPanelTop.TabIndex = 0;

            //
            // titleUltraLabel
            // 
            this.titleUltraLabel.Padding = new System.Drawing.Size(3, 3);
            this.titleUltraLabel.AutoSize = true;
            this.titleUltraLabel.BackColorInternal = System.Drawing.Color.Transparent;
            this.titleUltraLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleUltraLabel.Name = "titleUltraLabel";
            this.titleUltraLabel.TabIndex = 0;
            //
            // tableLayoutPanel
            //
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.Controls.Add(this.bottomMsgLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.ultraPanelBottom, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            // 
            // ultraPanelBottom
            // 

            this.ultraPanelBottom.Controls.Add(this.responseButton);
            this.ultraPanelBottom.Controls.Add(this.cancelButton);
            this.ultraPanelBottom.Controls.Add(this.exportButton);
            this.ultraPanelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ultraPanelBottom.Name = "ultraPanelBottom";
            this.ultraPanelBottom.TabIndex = 2;
            this.ultraPanelBottom.AutoSize = true;
            this.ultraPanelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ultraPanelBottom.BackColor = System.Drawing.Color.Transparent;
            this.ultraPanelBottom.Size = new System.Drawing.Size(890, 27);
            // 
            // ultraLabel
            // 
            this.bottomMsgLabel.BackColorInternal = System.Drawing.Color.Transparent;
            this.bottomMsgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bottomMsgLabel.Name = "bottomMsgLabel";
            this.bottomMsgLabel.TabIndex = 3;
            this.bottomMsgLabel.AutoSize = true;
            this.bottomMsgLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.bottomMsgLabel.Padding = new System.Drawing.Size(3, 3);
            // 
            // responseButton
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(156)))), ((int)(((byte)(46)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            this.responseButton.Appearance = appearance4;
            this.responseButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.responseButton.Name = "responseButton";
            this.responseButton.Size = new System.Drawing.Size(75, 25);
            this.responseButton.TabIndex = 0;
            this.responseButton.Text = "OK";
            this.responseButton.UseAppStyling = false;
            this.responseButton.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.responseButton.Margin = new System.Windows.Forms.Padding(10, 0, 10, 7);
            this.responseButton.Click += new System.EventHandler(this.ResponseButton_Click);
            // 
            // cancelButton
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            appearance3.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Appearance = appearance3;
            this.cancelButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseAppStyling = false;
            this.cancelButton.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.cancelButton.Margin = new System.Windows.Forms.Padding(10, 0, 10, 7);
            this.cancelButton.Click += CancelButton_Click;
            // 
            // exportButton
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(99)))), ((int)(((byte)(160)))));
            appearance2.ForeColor = System.Drawing.Color.White;
            this.exportButton.Appearance = appearance2;
            this.exportButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 25);
            this.exportButton.TabIndex = 2;
            this.exportButton.Text = "Export";
            this.exportButton.UseAppStyling = false;
            this.exportButton.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.exportButton.Margin = new System.Windows.Forms.Padding(10, 0, 10, 7);
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ultraPanelGrid
            // 
            this.ultraPanelGrid.ClientArea.Controls.Add(this.alertGrid);
            this.ultraPanelGrid.Name = "ultraPanelGrid";
            this.ultraPanelGrid.Size = new System.Drawing.Size(890, 300);
            this.ultraPanelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelGrid.TabIndex = 1;

            // 
            // alertGrid
            // 
            this.alertGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.alertGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.alertGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.alertGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.alertGrid.DisplayLayout.MaxRowScrollRegions = 1;
            this.alertGrid.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
            this.alertGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.alertGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.alertGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.alertGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.alertGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.alertGrid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.alertGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.alertGrid.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.alertGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.alertGrid.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.alertGrid.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.alertGrid.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.alertGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.alertGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.alertGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            //this.alertGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
            this.alertGrid.Name = "alertPopupGridCompliance";
            this.alertGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;                
            this.alertGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.alertGrid.TabIndex = 1;
            this.alertGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.AlertGrid_InitializeLayout);
            this.alertGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.AlertGrid_IntializeRow);
            this.alertGrid.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.AlertGrid_ClickCell);
            this.alertGrid.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.AlertGrid_BeforeExitEditMode);
            // 
            // ComplianceAlertPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(890, 352);
            this.Controls.Add(this.ultraPanelGrid);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.ultraPanelTop);
            //this.MaximumSize = this.Size;
            //this.MinimumSize = this.Size;
            this.Name = "ComplianceAlertPopUp";
            this.ultraPanelTop.ClientArea.ResumeLayout(false);
            this.ultraPanelTop.ClientArea.PerformLayout();
            this.ultraPanelTop.ResumeLayout(false);
            this.ultraPanelBottom.ResumeLayout(false);
            this.ultraPanelBottom.PerformLayout();
            this.ultraPanelBottom.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ultraPanelGrid.ClientArea.ResumeLayout(false);
            this.ultraPanelGrid.ClientArea.PerformLayout();
            this.ultraPanelGrid.ResumeLayout(false);
            this.ultraPanelGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alertGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private UltraPanel ultraPanelTop;
        private FlowLayoutPanel ultraPanelBottom;
        private UltraPanel ultraPanelGrid;
        private TableLayoutPanel tableLayoutPanel;
        private UltraButton exportButton;
        private UltraButton cancelButton;
        private UltraButton responseButton;
        private UltraLabel titleUltraLabel;
        private UltraLabel bottomMsgLabel;
        private PranaUltraGrid alertGrid;
        private UltraGridExcelExporter ultraGridExcelExporter;

        #endregion
    }
}
