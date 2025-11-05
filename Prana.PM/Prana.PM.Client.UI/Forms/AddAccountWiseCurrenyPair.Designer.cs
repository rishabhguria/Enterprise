namespace Prana.PM.Client.UI.Forms
{
    partial class AddAccountWiseCurrenyPair
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.pnlBody = new Infragistics.Win.Misc.UltraPanel();
            this.upnlButtons = new Infragistics.Win.Misc.UltraPanel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.upnlFill = new Infragistics.Win.Misc.UltraPanel();
            this.txtFxSymbol = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cmbToCurrency = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbFromCurrency = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblToCurrency = new Infragistics.Win.Misc.UltraLabel();
            this.lblFromCurrency = new Infragistics.Win.Misc.UltraLabel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.pnlBody.ClientArea.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.upnlButtons.ClientArea.SuspendLayout();
            this.upnlButtons.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.upnlFill.ClientArea.SuspendLayout();
            this.upnlFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFxSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbToCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFromCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            // 
            // pnlBody.ClientArea
            // 
            this.pnlBody.ClientArea.Controls.Add(this.upnlButtons);
            this.pnlBody.ClientArea.Controls.Add(this.statusStrip1);
            this.pnlBody.ClientArea.Controls.Add(this.upnlFill);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(8, 31);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(576, 142);
            this.pnlBody.TabIndex = 0;
            // 
            // upnlButtons
            // 
            // 
            // upnlButtons.ClientArea
            // 
            this.upnlButtons.ClientArea.Controls.Add(this.btnCancel);
            this.upnlButtons.ClientArea.Controls.Add(this.btnAdd);
            this.upnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.upnlButtons.Location = new System.Drawing.Point(0, 82);
            this.upnlButtons.Name = "upnlButtons";
            this.upnlButtons.Size = new System.Drawing.Size(576, 38);
            this.upnlButtons.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(282, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(158, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(107, 25);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Gray;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 120);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(576, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusBar
            // 
            this.lblStatusBar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblStatusBar.Name = "lblStatusBar";
            this.lblStatusBar.Size = new System.Drawing.Size(0, 17);
            // 
            // upnlFill
            // 
            // 
            // upnlFill.ClientArea
            // 
            this.upnlFill.ClientArea.Controls.Add(this.txtFxSymbol);
            this.upnlFill.ClientArea.Controls.Add(this.cmbToCurrency);
            this.upnlFill.ClientArea.Controls.Add(this.cmbFromCurrency);
            this.upnlFill.ClientArea.Controls.Add(this.lblSymbol);
            this.upnlFill.ClientArea.Controls.Add(this.lblToCurrency);
            this.upnlFill.ClientArea.Controls.Add(this.lblFromCurrency);
            this.upnlFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upnlFill.Location = new System.Drawing.Point(0, 0);
            this.upnlFill.Name = "upnlFill";
            this.upnlFill.Size = new System.Drawing.Size(576, 142);
            this.upnlFill.TabIndex = 1;
            // 
            // txtFxSymbol
            // 
            this.txtFxSymbol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFxSymbol.Location = new System.Drawing.Point(384, 48);
            this.txtFxSymbol.Name = "txtFxSymbol";
            this.txtFxSymbol.Size = new System.Drawing.Size(144, 21);
            this.txtFxSymbol.TabIndex = 5;
            this.txtFxSymbol.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // cmbToCurrency
            // 
            this.cmbToCurrency.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbToCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.cmbToCurrency.Location = new System.Drawing.Point(216, 49);
            this.cmbToCurrency.Name = "cmbToCurrency";
            this.cmbToCurrency.NullText = "-SELECT-";
            this.cmbToCurrency.Size = new System.Drawing.Size(144, 21);
            this.cmbToCurrency.TabIndex = 4;
            this.cmbToCurrency.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // cmbFromCurrency
            // 
            this.cmbFromCurrency.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbFromCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.cmbFromCurrency.Location = new System.Drawing.Point(55, 49);
            this.cmbFromCurrency.Name = "cmbFromCurrency";
            this.cmbFromCurrency.NullText = "-SELECT-";
            this.cmbFromCurrency.Size = new System.Drawing.Size(144, 21);
            this.cmbFromCurrency.TabIndex = 3;
            this.cmbFromCurrency.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lblSymbol
            // 
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblSymbol.Appearance = appearance1;
            this.lblSymbol.AutoSize = true;
            this.lblSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSymbol.Location = new System.Drawing.Point(384, 26);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(46, 16);
            this.lblSymbol.TabIndex = 2;
            this.lblSymbol.Text = "Symbol";
            this.lblSymbol.UseAppStyling = false;
            // 
            // lblToCurrency
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblToCurrency.Appearance = appearance2;
            this.lblToCurrency.AutoSize = true;
            this.lblToCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblToCurrency.Location = new System.Drawing.Point(216, 26);
            this.lblToCurrency.Name = "lblToCurrency";
            this.lblToCurrency.Size = new System.Drawing.Size(72, 16);
            this.lblToCurrency.TabIndex = 1;
            this.lblToCurrency.Text = "To Currency";
            this.lblToCurrency.UseAppStyling = false;
            // 
            // lblFromCurrency
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.lblFromCurrency.Appearance = appearance3;
            this.lblFromCurrency.AutoSize = true;
            this.lblFromCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblFromCurrency.Location = new System.Drawing.Point(55, 26);
            this.lblFromCurrency.Name = "lblFromCurrency";
            this.lblFromCurrency.Size = new System.Drawing.Size(87, 16);
            this.lblFromCurrency.TabIndex = 0;
            this.lblFromCurrency.Text = "From Currency";
            this.lblFromCurrency.UseAppStyling = false;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left
            // 
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.Name = "_AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left";
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 142);
            // 
            // _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right
            // 
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(584, 31);
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.Name = "_AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right";
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 142);
            // 
            // _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top
            // 
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.Name = "_AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top";
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(592, 31);
            // 
            // _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom
            // 
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 173);
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.Name = "_AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom";
            this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(592, 8);
            // 
            // AddAccountWiseCurrenyPair
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 181);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(592, 181);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(592, 181);
            this.Name = "AddAccountWiseCurrenyPair";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Currency Pair";
            this.Load += new System.EventHandler(this.AddAccountWiseCurrenyPair_Load);
            this.pnlBody.ClientArea.ResumeLayout(false);
            this.pnlBody.ClientArea.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.upnlButtons.ClientArea.ResumeLayout(false);
            this.upnlButtons.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.upnlFill.ClientArea.ResumeLayout(false);
            this.upnlFill.ClientArea.PerformLayout();
            this.upnlFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFxSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbToCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFromCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel pnlBody;
        private Infragistics.Win.Misc.UltraPanel upnlFill;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFxSymbol;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbToCurrency;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbFromCurrency;
        private Infragistics.Win.Misc.UltraLabel lblSymbol;
        private Infragistics.Win.Misc.UltraLabel lblToCurrency;
        private Infragistics.Win.Misc.UltraLabel lblFromCurrency;
        private Infragistics.Win.Misc.UltraPanel upnlButtons;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnAdd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusBar;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddAccountWiseCurrenyPair_UltraFormManager_Dock_Area_Bottom;
    }
}