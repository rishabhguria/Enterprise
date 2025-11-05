using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.Blotter.Controls
{
    partial class ViewAllocationDetails
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
                if (allocationPrefCmb != null)
                {
                    allocationPrefCmb.Dispose();
                    allocationPrefCmb = null;
                }
                if (vlAccount != null)
                {
                    vlAccount.Dispose();
                }
                if(_gridData != null)
                {
                    _gridData.Dispose();
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ultraLabelQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelStartOfDayQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelAllocationPref = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraLabelOrderQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelStartOfDayOrderQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelSymbolName = new Infragistics.Win.Misc.UltraLabel();
            this.gridViewAllocation = new PranaUltraGrid();
            this.mnuAllocationGrid = new System.Windows.Forms.ContextMenuStrip();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new UltraButton();
            this.ultraLabelRemainingQty = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelRemainingQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelRemainingPerc = new Infragistics.Win.Misc.UltraLabel();
            this.btnAllocate = new UltraButton();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraLabelRemainingPercentage = new Infragistics.Win.Misc.UltraLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grdGroupDetails = new PranaUltraGrid();
            this.lblGrouped = new System.Windows.Forms.Label();
            this.allocationPrefCmb = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAllocation)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGroupDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.flowLayoutPanel1);
            this.ultraPanel1.ClientArea.Controls.Add(this.panel1);
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(736, 434);
            this.ultraPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(-3, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(733, 356);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ultraLabelQuantity);
            this.panel2.Controls.Add(this.ultraLabelStartOfDayQuantity);
            this.panel2.Controls.Add(this.ultraLabelSymbol);
            this.panel2.Controls.Add(this.ultraLabelOrderQuantity);
            this.panel2.Controls.Add(this.ultraLabelStartOfDayOrderQuantity);
            this.panel2.Controls.Add(this.ultraLabelSymbolName);
            this.panel2.Controls.Add(this.ultraLabelAllocationPref);
            this.panel2.Controls.Add(this.allocationPrefCmb);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(727, 50);
            this.panel2.TabIndex = 14;
            // 
            // ultraLabelQuantity
            // 
            appearance9.TextVAlignAsString = "Middle";
            this.ultraLabelQuantity.Appearance = appearance9;
            this.ultraLabelQuantity.AutoSize = true;
            this.ultraLabelQuantity.Location = new System.Drawing.Point(657, 3);
            this.ultraLabelQuantity.Name = "ultraLabelQuantity";
            this.ultraLabelQuantity.Size = new System.Drawing.Size(20, 14);
            this.ultraLabelQuantity.TabIndex = 8;
            this.ultraLabelQuantity.Text = "0.0";
            //
            // ultraLabelQuantity
            // 
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabelStartOfDayQuantity.Appearance = appearance17;
            this.ultraLabelStartOfDayQuantity.AutoSize = true;
            this.ultraLabelStartOfDayQuantity.Location = new System.Drawing.Point(490, 3);
            this.ultraLabelStartOfDayQuantity.Name = "ultraLabelStartOfDayQuantity";
            this.ultraLabelStartOfDayQuantity.Size = new System.Drawing.Size(20, 14);
            this.ultraLabelStartOfDayQuantity.TabIndex = 8;
            this.ultraLabelStartOfDayQuantity.Text = "0.0";
            // 
            // ultraLabelSymbol
            // 
            appearance10.TextVAlignAsString = "Middle";
            this.ultraLabelSymbol.Appearance = appearance10;
            this.ultraLabelSymbol.AutoSize = true;
            this.ultraLabelSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelSymbol.Location = new System.Drawing.Point(20, 3);
            this.ultraLabelSymbol.Name = "ultraLabelSymbol";
            this.ultraLabelSymbol.Size = new System.Drawing.Size(52, 14);
            this.ultraLabelSymbol.TabIndex = 1;
            this.ultraLabelSymbol.Text = "Symbol:";
            // 
            // ultraLabelOrderQuantity
            // 
            appearance11.TextVAlignAsString = "Middle";
            this.ultraLabelOrderQuantity.Appearance = appearance11;
            this.ultraLabelOrderQuantity.AutoSize = true;
            this.ultraLabelOrderQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelOrderQuantity.Location = new System.Drawing.Point(600, 3);
            this.ultraLabelOrderQuantity.Name = "ultraLabelOrderQuantity";
            this.ultraLabelOrderQuantity.Size = new System.Drawing.Size(52, 14);
            this.ultraLabelOrderQuantity.TabIndex = 3;
            this.ultraLabelOrderQuantity.Text = "Quantity: ";
            // 
            // ultraLabelOrderQuantity
            // 
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabelStartOfDayOrderQuantity.Appearance = appearance18;
            this.ultraLabelStartOfDayOrderQuantity.AutoSize = true;
            this.ultraLabelStartOfDayOrderQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelStartOfDayOrderQuantity.Location = new System.Drawing.Point(400, 3);
            this.ultraLabelStartOfDayOrderQuantity.Name = "ultraLabelOrderQuantity";
            this.ultraLabelStartOfDayOrderQuantity.Size = new System.Drawing.Size(52, 14);
            this.ultraLabelStartOfDayOrderQuantity.TabIndex = 3;
            this.ultraLabelStartOfDayOrderQuantity.Text = "SOD Quantity: ";
            // 
            // ultraLabelSymbolName
            // 
            appearance12.TextVAlignAsString = "Middle";
            this.ultraLabelSymbolName.Appearance = appearance12;
            this.ultraLabelSymbolName.AutoSize = true;
            this.ultraLabelSymbolName.Location = new System.Drawing.Point(73, 3);
            this.ultraLabelSymbolName.Name = "ultraLabelSymbolName";
            this.ultraLabelSymbolName.Text = "symbol";
            this.ultraLabelSymbolName.TabIndex = 2;
            // 
            // ultraLabelAllocationPref
            // 
            this.ultraLabelAllocationPref.Location = new System.Drawing.Point(470, 30);
            this.ultraLabelAllocationPref.Name = "ultraLabelAllocationPref";
            this.ultraLabelAllocationPref.AutoSize = true;
            this.ultraLabelAllocationPref.TabIndex = 8;
            this.ultraLabelAllocationPref.Text = "Allocation Preference: ";
            // 
            // allocationPrefCmb
            // 
            this.allocationPrefCmb.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.allocationPrefCmb.Name = "allocationPrefCmb";
            this.allocationPrefCmb.Size = new System.Drawing.Size(200, 40);
            this.allocationPrefCmb.Location = new System.Drawing.Point(480, 30);
            this.allocationPrefCmb.ValueChanged += allocationPrefCmb_ValueChanged;
            this.allocationPrefCmb.Leave += allocationPrefCmb_Leave;
            this.allocationPrefCmb.TabIndex = 0;
            // 
            // mnuAllocationGrid
            // 
            this.mnuAllocationGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.mnuAllocationGrid.Name = "mnuAllocationGrid";
            this.mnuAllocationGrid.Size = new System.Drawing.Size(151, 23);
            this.mnuAllocationGrid.Opening += mnuAllocationGrid_Opening;
            // 
            // buyToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // gridViewAllocation
            // 
            this.gridViewAllocation.ContextMenuStrip = mnuAllocationGrid;
            this.gridViewAllocation.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.gridViewAllocation.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.gridViewAllocation.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridViewAllocation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.gridViewAllocation.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.gridViewAllocation.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            this.gridViewAllocation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridViewAllocation.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridViewAllocation.Location = new System.Drawing.Point(-3, 223);
            this.gridViewAllocation.Name = "gridViewAllocation";
            this.gridViewAllocation.Size = new System.Drawing.Size(727, 184);
            this.gridViewAllocation.TabIndex = 5;
            this.gridViewAllocation.Text = "Allocation Details";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.gridViewAllocation.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ExtendFirstColumn;
            this.gridViewAllocation.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridViewAllocation_InitializeLayout);
            this.gridViewAllocation.InitializeTemplateAddRow += gridViewAllocation_InitializeTemplateAddRow;
            this.gridViewAllocation.AfterExitEditMode += gridViewAllocation_AfterExitEditMode;
            this.gridViewAllocation.BeforeRowUpdate += gridViewAllocation_BeforeRowUpdate;
            this.gridViewAllocation.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gridViewAllocation_InitializeRow);
            this.gridViewAllocation.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridViewAllocation_AfterCellUpdate);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            //
            //Statusstrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1284, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.BackColor = System.Drawing.Color.DimGray;
            this.statusStrip1.TabIndex = 5;
            //
            //Toolstrip
            //
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.ultraLabelRemainingQty);
            this.panel1.Controls.Add(this.ultraLabelRemainingQuantity);
            this.panel1.Controls.Add(this.ultraLabelRemainingPerc);
            this.panel1.Controls.Add(this.btnAllocate);
            this.panel1.Controls.Add(this.ultraLabelRemainingPercentage);
            this.panel1.Location = new System.Drawing.Point(0, 363);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(727, 65);
            this.panel1.TabIndex = 13;
            // 
            // button2
            // 
            this.btnClear.Location = new System.Drawing.Point(590, 2);
            this.btnClear.Name = "button2";
            this.btnClear.Size = new System.Drawing.Size(95, 34);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += btnClear_Click;
            // 
            // ultraLabelRemainingQty
            // 
            appearance13.TextVAlignAsString = "Middle";
            this.ultraLabelRemainingQty.Appearance = appearance13;
            this.ultraLabelRemainingQty.AutoSize = true;
            this.ultraLabelRemainingQty.Location = new System.Drawing.Point(176, 23);
            this.ultraLabelRemainingQty.Name = "ultraLabelRemainingQty";
            this.ultraLabelRemainingQty.Size = new System.Drawing.Size(20, 14);
            this.ultraLabelRemainingQty.TabIndex = 9;
            this.ultraLabelRemainingQty.Text = "0.0";
            // 
            // ultraLabelRemainingQuantity
            // 
            appearance14.TextVAlignAsString = "Middle";
            this.ultraLabelRemainingQuantity.Appearance = appearance14;
            this.ultraLabelRemainingQuantity.AutoSize = true;
            this.ultraLabelRemainingQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelRemainingQuantity.Location = new System.Drawing.Point(43, 23);
            this.ultraLabelRemainingQuantity.Name = "ultraLabelRemainingQuantity";
            this.ultraLabelRemainingQuantity.Size = new System.Drawing.Size(110, 14);
            this.ultraLabelRemainingQuantity.TabIndex = 7;
            this.ultraLabelRemainingQuantity.Text = "Unexecuted Quantity:";
            // 
            // ultraLabelRemainingPerc
            // 
            appearance15.TextVAlignAsString = "Middle";
            this.ultraLabelRemainingPerc.Appearance = appearance15;
            this.ultraLabelRemainingPerc.AutoSize = true;
            this.ultraLabelRemainingPerc.Location = new System.Drawing.Point(176, 3);
            this.ultraLabelRemainingPerc.Name = "ultraLabelRemainingPerc";
            this.ultraLabelRemainingPerc.Size = new System.Drawing.Size(43, 14);
            this.ultraLabelRemainingPerc.TabIndex = 10;
            this.ultraLabelRemainingPerc.Text = "100.0%";
            // 
            // button1
            // 
            this.btnAllocate.Location = new System.Drawing.Point(443, 2);
            this.btnAllocate.Name = "button1";
            this.btnAllocate.Size = new System.Drawing.Size(101, 34);
            this.btnAllocate.TabIndex = 11;
            this.btnAllocate.Text = "Allocate";
            this.btnAllocate.Click += btnAllocate_Click;
            // 
            // ultraLabelRemainingPercentage
            // 
            appearance16.TextVAlignAsString = "Middle";
            this.ultraLabelRemainingPercentage.Appearance = appearance16;
            this.ultraLabelRemainingPercentage.AutoSize = true;
            this.ultraLabelRemainingPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelRemainingPercentage.Location = new System.Drawing.Point(30, 0);
            this.ultraLabelRemainingPercentage.Name = "ultraLabelRemainingPercentage";
            this.ultraLabelRemainingPercentage.Size = new System.Drawing.Size(110, 14);
            this.ultraLabelRemainingPercentage.TabIndex = 6;
            this.ultraLabelRemainingPercentage.Text = "% Unexecuted Quantity: ";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblGrouped);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(727, 24);
            this.panel3.TabIndex = 15;
            // 
            // grdGroupDetails
            // 
            this.grdGroupDetails.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdGroupDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdGroupDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdGroupDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdGroupDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdGroupDetails.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            this.grdGroupDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdGroupDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdGroupDetails.Location = new System.Drawing.Point(3, 65);
            this.grdGroupDetails.Name = "grdGroupDetails";
            this.grdGroupDetails.Size = new System.Drawing.Size(727, 102);
            this.grdGroupDetails.TabIndex = 16;
            this.grdGroupDetails.Text = "Group Details";
            // 
            // label1
            // 
            this.lblGrouped.AutoSize = true;
            this.lblGrouped.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrouped.ForeColor = System.Drawing.Color.Red;
            this.lblGrouped.Location = new System.Drawing.Point(54, 4);
            this.lblGrouped.Name = "label1";
            this.lblGrouped.Size = new System.Drawing.Size(613, 20);
            this.lblGrouped.TabIndex = 0;
            this.lblGrouped.Text = "This order is grouped and thus allocation can be changed from Allocation Module only.";
            this.lblGrouped.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // allocationPrefCmb
            // 
            this.allocationPrefCmb.Name = "ultraComboEditorSymbology";
            this.allocationPrefCmb.Size = new System.Drawing.Size(100, 20);
            this.allocationPrefCmb.Location = new System.Drawing.Point(600, 30);
            this.allocationPrefCmb.TabIndex = 0;
            // 
            // ViewAllocationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ViewAllocationDetails";
            this.Size = new System.Drawing.Size(742, 472);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAllocation)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGroupDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuAllocationGrid;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabelOrderQuantity;
        private Infragistics.Win.Misc.UltraLabel ultraLabelStartOfDayOrderQuantity;
        private Infragistics.Win.Misc.UltraLabel ultraLabelSymbolName;
        private Infragistics.Win.Misc.UltraLabel ultraLabelSymbol;
        private Infragistics.Win.Misc.UltraLabel ultraLabelRemainingQuantity;
        private Infragistics.Win.Misc.UltraLabel ultraLabelRemainingPercentage;
        private PranaUltraGrid gridViewAllocation;
        private Infragistics.Win.Misc.UltraLabel ultraLabelRemainingPerc;
        private Infragistics.Win.Misc.UltraLabel ultraLabelRemainingQty;
        private Infragistics.Win.Misc.UltraLabel ultraLabelQuantity;
        private Infragistics.Win.Misc.UltraLabel ultraLabelStartOfDayQuantity;
        private Infragistics.Win.Misc.UltraLabel ultraLabelAllocationPref;
        private UltraButton btnClear;
        private UltraButton btnAllocate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private PranaUltraGrid grdGroupDetails;
        private System.Windows.Forms.Label lblGrouped;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor allocationPrefCmb;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
