using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using System.Drawing;

namespace Prana.ThirdPartyUI.Forms
{
    partial class ToleranceProfile
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
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThirdParty));
            this.ToleranceProfile_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastModifiedPanel = new Infragistics.Win.Misc.UltraPanel();
            this.lastModifiedTextBox = new System.Windows.Forms.TextBox();
            this.lastModifiedLabel = new Infragistics.Win.Misc.UltraLabel();
            this.saveButton = new Infragistics.Win.Misc.UltraButton();
            this.cancelButton = new Infragistics.Win.Misc.UltraButton();
            this.jobNameLabel = new Infragistics.Win.Misc.UltraLabel();
            this.executingBrokerLabel = new Infragistics.Win.Misc.UltraLabel();
            this.toleranceProfileAttributePanel = new Infragistics.Win.Misc.UltraPanel();
            this.miscFeesTextEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.miscFeesLabel = new Infragistics.Win.Misc.UltraLabel();
            this.commissionTextEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.commissionLabel = new Infragistics.Win.Misc.UltraLabel();
            this.netMoneyLabel = new Infragistics.Win.Misc.UltraLabel();
            this.avgPriceLabel = new Infragistics.Win.Misc.UltraLabel();
            this.netMoneyTextEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.avgPriceTextEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.matchingFieldGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.tolerancePercentageRadioButton = new Infragistics.Win.UltraWinEditors.UltraRadioButton();
            this.toleranceValueRadioButton = new Infragistics.Win.UltraWinEditors.UltraRadioButton();
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.executingBrokerUltraCombo = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.jobNameUltraCombo = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.ToleranceProfile_Fill_Panel.ClientArea.SuspendLayout();
            this.ToleranceProfile_Fill_Panel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.lastModifiedPanel.ClientArea.SuspendLayout();
            this.lastModifiedPanel.SuspendLayout();
            this.toleranceProfileAttributePanel.ClientArea.SuspendLayout();
            this.toleranceProfileAttributePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.miscFeesTextEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionTextEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.netMoneyTextEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.avgPriceTextEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchingFieldGroupBox)).BeginInit();
            this.matchingFieldGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tolerancePercentageRadioButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toleranceValueRadioButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.executingBrokerUltraCombo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jobNameUltraCombo)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // ToleranceProfile_Fill_Panel
            // 
            // 
            // ToleranceProfile_Fill_Panel.ClientArea
            // 
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.jobNameUltraCombo);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.executingBrokerUltraCombo);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.statusStrip);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.lastModifiedPanel);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.saveButton);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.cancelButton);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.jobNameLabel);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.executingBrokerLabel);
            this.ToleranceProfile_Fill_Panel.ClientArea.Controls.Add(this.toleranceProfileAttributePanel);
            this.ToleranceProfile_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ToleranceProfile_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToleranceProfile_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.ToleranceProfile_Fill_Panel.Name = "ToleranceProfile_Fill_Panel";
            this.ToleranceProfile_Fill_Panel.Size = new System.Drawing.Size(292, 402);
            this.ToleranceProfile_Fill_Panel.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.statusStrip.ForeColor = System.Drawing.Color.Black;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 380);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(292, 22);
            this.statusStrip.TabIndex = 22;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripStatusLabel1.Image = ((System.Drawing.Image)(resources.GetObject("tsStatus.Image")));
            this.toolStripStatusLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusLabel1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // lastModifiedPanel
            // 
            // 
            // lastModifiedPanel.ClientArea
            // 
            this.lastModifiedPanel.ClientArea.Controls.Add(this.lastModifiedTextBox);
            this.lastModifiedPanel.ClientArea.Controls.Add(this.lastModifiedLabel);
            this.lastModifiedPanel.Location = new System.Drawing.Point(6, 80);
            this.lastModifiedPanel.Name = "lastModifiedPanel";
            this.lastModifiedPanel.Size = new System.Drawing.Size(280, 20);
            this.lastModifiedPanel.TabIndex = 21;
            // 
            // lastModifiedTextBox
            // 
            this.lastModifiedTextBox.Location = new System.Drawing.Point(148, 0);
            this.lastModifiedTextBox.Name = "lastModifiedTextBox";
            this.lastModifiedTextBox.ReadOnly = true;
            this.lastModifiedTextBox.Size = new System.Drawing.Size(105, 20);
            this.lastModifiedTextBox.TabIndex = 5;
            // 
            // lastModifiedLabel
            // 
            this.lastModifiedLabel.Location = new System.Drawing.Point(14, 0);
            this.lastModifiedLabel.Name = "lastModifiedLabel";
            this.lastModifiedLabel.Size = new System.Drawing.Size(100, 20);
            this.lastModifiedLabel.TabIndex = 4;
            this.lastModifiedLabel.Text = "Last Modified";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(154, 337);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 22);
            this.saveButton.TabIndex = 18;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(45, 337);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 22);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // jobNameLabel
            // 
            this.jobNameLabel.Location = new System.Drawing.Point(20, 50);
            this.jobNameLabel.Name = "jobNameLabel";
            this.jobNameLabel.Size = new System.Drawing.Size(100, 20);
            this.jobNameLabel.TabIndex = 3;
            this.jobNameLabel.Text = "Job Name";
            // 
            // executingBrokerLabel
            // 
            this.executingBrokerLabel.Location = new System.Drawing.Point(20, 20);
            this.executingBrokerLabel.Name = "executingBrokerLabel";
            this.executingBrokerLabel.Size = new System.Drawing.Size(100, 20);
            this.executingBrokerLabel.TabIndex = 0;
            this.executingBrokerLabel.Text = "Executing Broker";
            // 
            // toleranceProfileAttributePanel
            // 
            // 
            // toleranceProfileAttributePanel.ClientArea
            // 
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.miscFeesTextEditor);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.miscFeesLabel);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.commissionTextEditor);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.commissionLabel);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.netMoneyLabel);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.avgPriceLabel);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.netMoneyTextEditor);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.avgPriceTextEditor);
            this.toleranceProfileAttributePanel.ClientArea.Controls.Add(this.matchingFieldGroupBox);
            this.toleranceProfileAttributePanel.Location = new System.Drawing.Point(6, 106);
            this.toleranceProfileAttributePanel.Name = "toleranceProfileAttributePanel";
            this.toleranceProfileAttributePanel.Size = new System.Drawing.Size(280, 215);
            this.toleranceProfileAttributePanel.TabIndex = 6;
            // 
            // miscFeesTextEditor
            // 
            this.miscFeesTextEditor.Location = new System.Drawing.Point(148, 185);
            this.miscFeesTextEditor.Name = "miscFeesTextEditor";
            this.miscFeesTextEditor.Size = new System.Drawing.Size(100, 21);
            this.miscFeesTextEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.decimalTextBox_KeyPress);
            this.miscFeesTextEditor.TextChanged += new System.EventHandler(this.decimalTextBox_TextChanged);
            this.miscFeesTextEditor.TabIndex = 17;
            // 
            // miscFeesLabel
            // 
            this.miscFeesLabel.Location = new System.Drawing.Point(20, 185);
            this.miscFeesLabel.Name = "miscFeesLabel";
            this.miscFeesLabel.Size = new System.Drawing.Size(100, 20);
            this.miscFeesLabel.TabIndex = 13;
            this.miscFeesLabel.Text = "Misc. Fees";
            // 
            // commissionTextEditor
            // 
            this.commissionTextEditor.Location = new System.Drawing.Point(148, 155);
            this.commissionTextEditor.Name = "commissionTextEditor";
            this.commissionTextEditor.Size = new System.Drawing.Size(100, 21);
            this.commissionTextEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.decimalTextBox_KeyPress);
            this.commissionTextEditor.TextChanged += new System.EventHandler(this.decimalTextBox_TextChanged);
            this.commissionTextEditor.TabIndex = 16;
            // 
            // commissionLabel
            // 
            this.commissionLabel.Location = new System.Drawing.Point(20, 155);
            this.commissionLabel.Name = "commissionLabel";
            this.commissionLabel.Size = new System.Drawing.Size(100, 20);
            this.commissionLabel.TabIndex = 12;
            this.commissionLabel.Text = "Commission";
            // 
            // netMoneyLabel
            // 
            this.netMoneyLabel.Location = new System.Drawing.Point(20, 125);
            this.netMoneyLabel.Name = "netMoneyLabel";
            this.netMoneyLabel.Size = new System.Drawing.Size(100, 20);
            this.netMoneyLabel.TabIndex = 11;
            this.netMoneyLabel.Text = "Net Money";
            // 
            // avgPriceLabel
            // 
            this.avgPriceLabel.Location = new System.Drawing.Point(20, 95);
            this.avgPriceLabel.Name = "avgPriceLabel";
            this.avgPriceLabel.Size = new System.Drawing.Size(100, 20);
            this.avgPriceLabel.TabIndex = 10;
            this.avgPriceLabel.Text = "Avg. Price";
            // 
            // netMoneyTextEditor
            // 
            this.netMoneyTextEditor.Location = new System.Drawing.Point(148, 125);
            this.netMoneyTextEditor.Name = "netMoneyTextEditor";
            this.netMoneyTextEditor.Size = new System.Drawing.Size(100, 21);
            this.netMoneyTextEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.decimalTextBox_KeyPress);
            this.netMoneyTextEditor.TextChanged += new System.EventHandler(this.decimalTextBox_TextChanged);
            this.netMoneyTextEditor.TabIndex = 15;
            // 
            // avgPriceTextEditor
            // 
            this.avgPriceTextEditor.Location = new System.Drawing.Point(148, 95);
            this.avgPriceTextEditor.Name = "avgPriceTextEditor";
            this.avgPriceTextEditor.Size = new System.Drawing.Size(100, 21);
            this.avgPriceTextEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.decimalTextBox_KeyPress);
            this.avgPriceTextEditor.TextChanged += new System.EventHandler(this.decimalTextBox_TextChanged);
            this.avgPriceTextEditor.TabIndex = 14;
            // 
            // matchingFieldGroupBox
            // 
            this.matchingFieldGroupBox.Controls.Add(this.tolerancePercentageRadioButton);
            this.matchingFieldGroupBox.Controls.Add(this.toleranceValueRadioButton);
            this.matchingFieldGroupBox.Location = new System.Drawing.Point(3, 5);
            this.matchingFieldGroupBox.Name = "matchingFieldGroupBox";
            this.matchingFieldGroupBox.Size = new System.Drawing.Size(270, 80);
            this.matchingFieldGroupBox.TabIndex = 20;
            this.matchingFieldGroupBox.Text = "Matching Field";
            // 
            // tolerancePercentageRadioButton
            // 
            this.tolerancePercentageRadioButton.Location = new System.Drawing.Point(19, 46);
            this.tolerancePercentageRadioButton.Name = "tolerancePercentageRadioButton";
            this.tolerancePercentageRadioButton.Size = new System.Drawing.Size(200, 20);
            this.tolerancePercentageRadioButton.TabIndex = 9;
            this.tolerancePercentageRadioButton.Text = "Tolerance in Percentage";
            this.tolerancePercentageRadioButton.CheckedChanged += new System.EventHandler(this.tolerancePercentageRadioButton_CheckedChanged);
            // 
            // toleranceValueRadioButton
            // 
            this.toleranceValueRadioButton.Location = new System.Drawing.Point(19, 20);
            this.toleranceValueRadioButton.Name = "toleranceValueRadioButton";
            this.toleranceValueRadioButton.Size = new System.Drawing.Size(200, 20);
            this.toleranceValueRadioButton.TabIndex = 8;
            this.toleranceValueRadioButton.Text = "Tolerance in Value";
            this.toleranceValueRadioButton.CheckedChanged += new System.EventHandler(this.toleranceValueRadioButton_CheckedChanged);
            // 
            // _ToleranceProfile_UltraFormManager_Dock_Area_Left
            // 
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.Name = "_ToleranceProfile_UltraFormManager_Dock_Area_Left";
            this._ToleranceProfile_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 402);
            // 
            // _ToleranceProfile_UltraFormManager_Dock_Area_Right
            // 
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(300, 32);
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.Name = "_ToleranceProfile_UltraFormManager_Dock_Area_Right";
            this._ToleranceProfile_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 402);
            // 
            // _ToleranceProfile_UltraFormManager_Dock_Area_Top
            // 
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.Name = "_ToleranceProfile_UltraFormManager_Dock_Area_Top";
            this._ToleranceProfile_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(308, 32);
            // 
            // _ToleranceProfile_UltraFormManager_Dock_Area_Bottom
            // 
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 434);
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.Name = "_ToleranceProfile_UltraFormManager_Dock_Area_Bottom";
            this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(308, 8);
            // 
            // executingBrokerUltraCombo
            // 
            this.executingBrokerUltraCombo.Location = new System.Drawing.Point(154, 18);
            this.executingBrokerUltraCombo.Name = "executingBrokerUltraCombo";
            this.executingBrokerUltraCombo.Size = new System.Drawing.Size(120, 21);
            this.executingBrokerUltraCombo.TabIndex = 23;
            this.executingBrokerUltraCombo.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest;
            this.executingBrokerUltraCombo.ValueChanged += new System.EventHandler(this.executingBrokerUltraCombo_ValueChanged);
            // 
            // jobNameUltraCombo
            // 
            this.jobNameUltraCombo.Location = new System.Drawing.Point(154, 50);
            this.jobNameUltraCombo.Name = "jobNameUltraCombo";
            this.jobNameUltraCombo.Size = new System.Drawing.Size(120, 21);
            this.jobNameUltraCombo.TabIndex = 24;
            this.jobNameUltraCombo.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest;
            this.jobNameUltraCombo.ValueChanged += new System.EventHandler(this.jobNameUltraCombo_ValueChanged);
            // 
            // ToleranceProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 442);
            this.Controls.Add(this.ToleranceProfile_Fill_Panel);
            this.Controls.Add(this._ToleranceProfile_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ToleranceProfile_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ToleranceProfile_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ToleranceProfile_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ToleranceProfile";
            this.Text = "ToleranceProfile";
            this.Load += new System.EventHandler(this.ToleranceProfile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ToleranceProfile_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ToleranceProfile_Fill_Panel.ClientArea.PerformLayout();
            this.ToleranceProfile_Fill_Panel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.lastModifiedPanel.ClientArea.ResumeLayout(false);
            this.lastModifiedPanel.ClientArea.PerformLayout();
            this.lastModifiedPanel.ResumeLayout(false);
            this.toleranceProfileAttributePanel.ClientArea.ResumeLayout(false);
            this.toleranceProfileAttributePanel.ClientArea.PerformLayout();
            this.toleranceProfileAttributePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.miscFeesTextEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commissionTextEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.netMoneyTextEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.avgPriceTextEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchingFieldGroupBox)).EndInit();
            this.matchingFieldGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tolerancePercentageRadioButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toleranceValueRadioButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.executingBrokerUltraCombo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jobNameUltraCombo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel ToleranceProfile_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ToleranceProfile_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ToleranceProfile_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ToleranceProfile_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ToleranceProfile_UltraFormManager_Dock_Area_Bottom;
        private UltraLabel jobNameLabel;
        private UltraLabel executingBrokerLabel;
        private UltraLabel miscFeesLabel;
        private UltraLabel commissionLabel;
        private UltraLabel netMoneyLabel;
        private UltraLabel avgPriceLabel;
        private Infragistics.Win.UltraWinEditors.UltraRadioButton tolerancePercentageRadioButton;
        private Infragistics.Win.UltraWinEditors.UltraRadioButton toleranceValueRadioButton;
        private System.Windows.Forms.TextBox lastModifiedTextBox;
        private UltraLabel lastModifiedLabel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor miscFeesTextEditor;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor commissionTextEditor;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor netMoneyTextEditor;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor avgPriceTextEditor;
        private UltraButton cancelButton;
        private UltraButton saveButton;
        private UltraGroupBox matchingFieldGroupBox;
        private UltraPanel toleranceProfileAttributePanel;
        private UltraPanel lastModifiedPanel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor jobNameUltraCombo;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor executingBrokerUltraCombo;
    }
}