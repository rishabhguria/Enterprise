using Prana.Global;
using Prana.WashSale.Controls;
using System;

namespace Prana.WashSale
{
    partial class WashSale
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Action<object, EventArgs<string>> UpdateStatusBar { get; private set; }

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.washSaleUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.washSaleUltraPanel_Top = new Infragistics.Win.Misc.UltraPanel();
            this.washSaleGridPanel = new Infragistics.Win.Misc.UltraPanel();
            this.washSaleTradesGridUC = new Prana.WashSale.Controls.WashSaleTradesGridUC();
            this.washSaleTradesFiltersUC = new Prana.WashSale.Controls.WashSaleTradesFiltersUC();
            this.washSaleTradesButtonUC = new Prana.WashSale.Controls.WashSaleTradesButtonUC();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.washSaleUltraFormManager)).BeginInit();
            this.washSaleUltraPanel_Top.ClientArea.SuspendLayout();
            this.washSaleUltraPanel_Top.SuspendLayout();
            this.washSaleGridPanel.ClientArea.SuspendLayout();
            this.washSaleGridPanel.SuspendLayout();
            statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // washSaleUltraFormManager
            // 
            this.washSaleUltraFormManager.Form = this;
            // 
            // _WashSaleTrades_UltraFormManager_Dock_Area_Left
            // 
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.FormManager = this.washSaleUltraFormManager;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.Name = "_WashSaleTrades_UltraFormManager_Dock_Area_Left";
            this._WashSaleTrades_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 450);
            // 
            // _WashSaleTrades_UltraFormManager_Dock_Area_Right
            // 
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.FormManager = this.washSaleUltraFormManager;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1124, 32);
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.Name = "_WashSaleTrades_UltraFormManager_Dock_Area_Right";
            this._WashSaleTrades_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 450);
            // 
            // _WashSaleTrades_UltraFormManager_Dock_Area_Top
            // 
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.FormManager = this.washSaleUltraFormManager;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.Name = "_WashSaleTrades_UltraFormManager_Dock_Area_Top";
            this._WashSaleTrades_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1132, 32);
            // 
            // _WashSaleTrades_UltraFormManager_Dock_Area_Bottom
            // 
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.FormManager = this.washSaleUltraFormManager;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 482);
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.Name = "_WashSaleTrades_UltraFormManager_Dock_Area_Bottom";
            this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1132, 8);
            // 
            // washSaleUltraPanel_Top
            // 
            this.washSaleUltraPanel_Top.ClientArea.Controls.Add(this.washSaleTradesFiltersUC);
            this.washSaleUltraPanel_Top.ClientArea.Controls.Add(this.washSaleTradesButtonUC);
            this.washSaleUltraPanel_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.washSaleUltraPanel_Top.Location = new System.Drawing.Point(8, 32);
            this.washSaleUltraPanel_Top.Name = "washSaleUltraPanel_Top";
            this.washSaleUltraPanel_Top.Size = new System.Drawing.Size(1116, 106);
            this.washSaleUltraPanel_Top.TabIndex = 0;
            // 
            // washSaleTradesFiltersUC
            // 
            this.washSaleTradesFiltersUC.Dock = System.Windows.Forms.DockStyle.Left;
            this.washSaleTradesFiltersUC.Location = new System.Drawing.Point(0, 0);
            this.washSaleTradesFiltersUC.Name = "washSaleTradesFiltersUC";
            this.washSaleTradesFiltersUC.Size = new System.Drawing.Size(708, 106);
            this.washSaleTradesFiltersUC.TabIndex = 0;
            // 
            // washSaleTradesButtonUC
            // 
            this.washSaleTradesButtonUC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.washSaleTradesButtonUC.Location = new System.Drawing.Point(708, 0);
            this.washSaleTradesButtonUC.Name = "washSaleTradesButtonUC";
            this.washSaleTradesButtonUC.Size = new System.Drawing.Size(408, 106);
            this.washSaleTradesButtonUC.TabIndex = 0;
            // 
            // washSaleGridPanel
            // 
            this.washSaleGridPanel.ClientArea.Controls.Add(this.washSaleTradesGridUC);
            this.washSaleGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.washSaleGridPanel.Location = new System.Drawing.Point(8, 138);
            this.washSaleGridPanel.Name = "washSaleGridPanel";
            this.washSaleGridPanel.Size = new System.Drawing.Size(1116, 322);
            this.washSaleGridPanel.TabIndex = 1;
            // 
            // washSaleTradesGridUC
            // 
            this.washSaleTradesGridUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.washSaleTradesGridUC.Location = new System.Drawing.Point(0, 0);
            this.washSaleTradesGridUC.Name = "washSaleTradesGridUC";
            this.washSaleTradesGridUC.Size = new System.Drawing.Size(1116, 322);
            this.washSaleTradesGridUC.TabIndex = 0;
            // 
            // statusStrip
            // 
            statusStrip.BackColor = System.Drawing.Color.DimGray;
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel});
            statusStrip.Location = new System.Drawing.Point(8, 460);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1116, 22);
            statusStrip.TabIndex = 6;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.BackColor = System.Drawing.Color.DimGray;
            toolStripStatusLabel.ForeColor = System.Drawing.Color.White;
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            toolStripStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // 
            // WashSale
            // 
            this.ShowIcon = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 490);
            this.Controls.Add(this.washSaleGridPanel);
            this.Controls.Add(this.washSaleUltraPanel_Top);
            this.Controls.Add(this._WashSaleTrades_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._WashSaleTrades_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._WashSaleTrades_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._WashSaleTrades_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(1066, 490);
            this.Controls.Add(statusStrip);
            this.UpdateStatusBar += this.WashSale_UpdateStatusBar;
            this.Name = "WashSale";
            this.Text = "Wash Sale Trades";
            this.Load += new System.EventHandler(this.WashSale_Load);
            this.Disposed += new System.EventHandler(this.WashSale_Disposed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WashSale_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.washSaleUltraFormManager)).EndInit();
            this.washSaleUltraPanel_Top.ClientArea.ResumeLayout(false);
            this.washSaleUltraPanel_Top.ResumeLayout(false);
            this.washSaleGridPanel.ClientArea.ResumeLayout(false);
            this.washSaleGridPanel.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager washSaleUltraFormManager;
        private Infragistics.Win.Misc.UltraPanel washSaleGridPanel;
        private Infragistics.Win.Misc.UltraPanel washSaleUltraPanel_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleTrades_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleTrades_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleTrades_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WashSaleTrades_UltraFormManager_Dock_Area_Bottom;
        private WashSaleTradesFiltersUC washSaleTradesFiltersUC;
        private WashSaleTradesGridUC washSaleTradesGridUC;
        private WashSaleTradesButtonUC washSaleTradesButtonUC;
        public static System.Windows.Forms.StatusStrip statusStrip;
        public static System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
      
        
    }
}

