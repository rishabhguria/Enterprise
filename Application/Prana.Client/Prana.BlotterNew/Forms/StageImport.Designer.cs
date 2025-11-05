using System.Drawing;
using System.Windows.Forms;

namespace Prana.Blotter.Forms
{
    partial class StageImport
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
            this.StageImport_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel = new System.Windows.Forms.Panel();
            this.label = new System.Windows.Forms.Label();
            this.progressCircle = new Prana.Utilities.UI.UIUtilities.ProgressCircle();
            this.cmbThirdParty = new System.Windows.Forms.ComboBox();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.upload = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.browse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._StageImport_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._StageImport_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._StageImport_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._StageImport_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.StageImport_Fill_Panel.ClientArea.SuspendLayout();
            this.StageImport_Fill_Panel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // StageImport_Fill_Panel
            // 
            // 
            // StageImport_Fill_Panel.ClientArea
            // 
            this.StageImport_Fill_Panel.ClientArea.Controls.Add(this.groupBox1);
            this.StageImport_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.StageImport_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StageImport_Fill_Panel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StageImport_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.StageImport_Fill_Panel.Name = "StageImport_Fill_Panel";
            this.StageImport_Fill_Panel.Size = new System.Drawing.Size(424, 138);
            this.StageImport_Fill_Panel.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel);
            this.groupBox1.Controls.Add(this.cmbThirdParty);
            this.groupBox1.Controls.Add(this.filePathTextBox);
            this.groupBox1.Controls.Add(this.upload);
            this.groupBox1.Controls.Add(this.cancel);
            this.groupBox1.Controls.Add(this.browse);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, -7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(432, 148);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.AutoSize = true;
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.Controls.Add(this.label);
            this.panel.Controls.Add(this.progressCircle);
            this.panel.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel.Location = new System.Drawing.Point(114, 12);
            this.panel.Margin = new System.Windows.Forms.Padding(2);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(222, 66);
            this.panel.TabIndex = 17;
            this.panel.Visible = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.Color.Black;
            this.label.Location = new System.Drawing.Point(10, 49);
            this.label.Margin = new System.Windows.Forms.Padding(2, 3, 2, 4);
            this.label.MinimumSize = new System.Drawing.Size(183, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(210, 13);
            this.label.TabIndex = 6;
            this.label.Text = CAPTION_UPLOADING_DATA;
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressCircle
            // 
            this.progressCircle.AutoSize = true;
            this.progressCircle.BackColor = System.Drawing.Color.Transparent;
            this.progressCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(5)))), ((int)(((byte)(90)))));
            this.progressCircle.Location = new System.Drawing.Point(84, 3);
            this.progressCircle.Margin = new System.Windows.Forms.Padding(2);
            this.progressCircle.Name = "progressCircle";
            this.progressCircle.NumberOfTail = 7;
            this.progressCircle.RingColor = System.Drawing.Color.White;
            this.progressCircle.RingThickness = 10;
            this.progressCircle.Size = new System.Drawing.Size(38, 41);
            this.progressCircle.TabIndex = 5;
            // 
            // cmbThirdParty
            // 
            this.cmbThirdParty.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbThirdParty.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbThirdParty.FormattingEnabled = true;
            this.cmbThirdParty.Location = new System.Drawing.Point(151, 15);
            this.cmbThirdParty.Name = "cmbThirdParty";
            this.cmbThirdParty.Size = new System.Drawing.Size(154, 21);
            this.cmbThirdParty.TabIndex = 16;
            this.cmbThirdParty.Text = "-Select-";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(151, 47);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(154, 20);
            this.filePathTextBox.TabIndex = 15;
            // 
            // upload
            // 
            this.upload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(156)))), ((int)(((byte)(46)))));
            this.upload.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upload.ForeColor = System.Drawing.Color.White;
            this.upload.Location = new System.Drawing.Point(114, 98);
            this.upload.Name = "upload";
            this.upload.Size = new System.Drawing.Size(85, 32);
            this.upload.TabIndex = 14;
            this.upload.Text = "Upload";
            this.upload.UseVisualStyleBackColor = false;
            this.upload.Click += new System.EventHandler(this.Upload_Click);
            // 
            // cancel
            // 
            this.cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(67)))), ((int)(((byte)(85)))));
            this.cancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.ForeColor = System.Drawing.Color.White;
            this.cancel.Location = new System.Drawing.Point(237, 98);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(85, 32);
            this.cancel.TabIndex = 13;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = false;
            this.cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // browse
            // 
            this.browse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(67)))), ((int)(((byte)(85)))));
            this.browse.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browse.ForeColor = System.Drawing.Color.White;
            this.browse.Location = new System.Drawing.Point(336, 40);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(85, 32);
            this.browse.TabIndex = 12;
            this.browse.Text = "Browse";
            this.browse.UseVisualStyleBackColor = false;
            this.browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 14);
            this.label2.TabIndex = 11;
            this.label2.Text = "File Path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 14);
            this.label1.TabIndex = 10;
            this.label1.Text = "Choose Third Party";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _StageImport_UltraFormManager_Dock_Area_Right
            // 
            this._StageImport_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StageImport_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StageImport_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._StageImport_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StageImport_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._StageImport_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._StageImport_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(432, 32);
            this._StageImport_UltraFormManager_Dock_Area_Right.Name = "_StageImport_UltraFormManager_Dock_Area_Right";
            this._StageImport_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 138);
            // 
            // _StageImport_UltraFormManager_Dock_Area_Left
            // 
            this._StageImport_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StageImport_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StageImport_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._StageImport_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StageImport_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._StageImport_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._StageImport_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._StageImport_UltraFormManager_Dock_Area_Left.Name = "_StageImport_UltraFormManager_Dock_Area_Left";
            this._StageImport_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 138);
            // 
            // _StageImport_UltraFormManager_Dock_Area_Bottom
            // 
            this._StageImport_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StageImport_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StageImport_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._StageImport_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StageImport_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._StageImport_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._StageImport_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 170);
            this._StageImport_UltraFormManager_Dock_Area_Bottom.Name = "_StageImport_UltraFormManager_Dock_Area_Bottom";
            this._StageImport_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(440, 8);
            // 
            // _StageImport_UltraFormManager_Dock_Area_Top
            // 
            this._StageImport_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StageImport_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._StageImport_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._StageImport_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StageImport_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._StageImport_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._StageImport_UltraFormManager_Dock_Area_Top.Name = "_StageImport_UltraFormManager_Dock_Area_Top";
            this._StageImport_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(440, 32);
            // 
            // StageImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(440, 178);
            this.Controls.Add(this.StageImport_Fill_Panel);
            this.Controls.Add(this._StageImport_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._StageImport_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._StageImport_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._StageImport_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(440, 178);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 178);
            this.Name = "StageImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stage Import File";
            this.Load += new System.EventHandler(this.StageImport_Load);
            this.StageImport_Fill_Panel.ClientArea.ResumeLayout(false);
            this.StageImport_Fill_Panel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StageImport_FormClosing);
        }

        #endregion
        private Infragistics.Win.Misc.UltraPanel StageImport_Fill_Panel;
        private GroupBox groupBox1;
        public Panel panel;
        private Label label;
        private Utilities.UI.UIUtilities.ProgressCircle progressCircle;
        private ComboBox cmbThirdParty;
        private TextBox filePathTextBox;
        private Button upload;
        private Button cancel;
        private Button browse;
        private Label label2;
        private Label label1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StageImport_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StageImport_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StageImport_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _StageImport_UltraFormManager_Dock_Area_Bottom;
    }
}
