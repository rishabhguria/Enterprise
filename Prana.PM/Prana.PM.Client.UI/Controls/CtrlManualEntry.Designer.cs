//namespace Prana.PM.Client.UI.Controls
//{
//    partial class CtrlManualEntry
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            _isInitialized = false;
//            base.Dispose(disposing);
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlManualEntry));
//            this.grpDataEntry = new Infragistics.Win.Misc.UltraGroupBox();
//            this.txtManualEntry = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
//            this.txtSourceData = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
//            this.txtApplicationData = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
//            this.lblApplicationData = new Infragistics.Win.Misc.UltraLabel();
//            this.lblManualEntry = new Infragistics.Win.Misc.UltraLabel();
//            this.lblSourceData = new Infragistics.Win.Misc.UltraLabel();
//            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
//            this.btnClear = new Infragistics.Win.Misc.UltraButton();
//            this.btnSave = new Infragistics.Win.Misc.UltraButton();
//            this.txtComments = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
//            this.lblComments = new Infragistics.Win.Misc.UltraLabel();
//            this.txtUserName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
//            this.lblUserName = new Infragistics.Win.Misc.UltraLabel();
//            this.lblSymbol = new Infragistics.Win.Misc.UltraLabel();
//            this.txtSymbol = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
//            ((System.ComponentModel.ISupportInitialize)(this.grpDataEntry)).BeginInit();
//            this.grpDataEntry.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.txtManualEntry)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtSourceData)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtApplicationData)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtComments)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtSymbol)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // grpDataEntry
//            // 
//            this.grpDataEntry.Controls.Add(this.txtManualEntry);
//            this.grpDataEntry.Controls.Add(this.txtSourceData);
//            this.grpDataEntry.Controls.Add(this.txtApplicationData);
//            this.grpDataEntry.Controls.Add(this.lblApplicationData);
//            this.grpDataEntry.Controls.Add(this.lblManualEntry);
//            this.grpDataEntry.Controls.Add(this.lblSourceData);
//            this.grpDataEntry.Location = new System.Drawing.Point(5, 59);
//            this.grpDataEntry.Name = "grpDataEntry";
//            this.grpDataEntry.Size = new System.Drawing.Size(427, 72);
//            this.grpDataEntry.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
//            this.grpDataEntry.TabIndex = 69;
//            this.grpDataEntry.Text = "Data Details";
//            // 
//            // txtManualEntry
//            // 
//            this.txtManualEntry.Location = new System.Drawing.Point(304, 42);
//            this.txtManualEntry.Name = "txtManualEntry";
//            this.txtManualEntry.Size = new System.Drawing.Size(115, 22);
//            this.txtManualEntry.TabIndex = 63;
//            // 
//            // txtSourceData
//            // 
//            this.txtSourceData.Location = new System.Drawing.Point(156, 42);
//            this.txtSourceData.Name = "txtSourceData";
//            this.txtSourceData.ReadOnly = true;
//            this.txtSourceData.Size = new System.Drawing.Size(115, 22);
//            this.txtSourceData.TabIndex = 62;
//            // 
//            // txtApplicationData
//            // 
//            this.txtApplicationData.Location = new System.Drawing.Point(7, 42);
//            this.txtApplicationData.Name = "txtApplicationData";
//            this.txtApplicationData.ReadOnly = true;
//            this.txtApplicationData.Size = new System.Drawing.Size(115, 22);
//            this.txtApplicationData.TabIndex = 61;
//            // 
//            // lblApplicationData
//            // 
//            this.lblApplicationData.Location = new System.Drawing.Point(7, 21);
//            this.lblApplicationData.Name = "lblApplicationData";
//            this.lblApplicationData.Size = new System.Drawing.Size(86, 15);
//            this.lblApplicationData.TabIndex = 58;
//            this.lblApplicationData.Text = "Application Data";
//            // 
//            // lblManualEntry
//            // 
//            this.lblManualEntry.Location = new System.Drawing.Point(304, 21);
//            this.lblManualEntry.Name = "lblManualEntry";
//            this.lblManualEntry.Size = new System.Drawing.Size(70, 15);
//            this.lblManualEntry.TabIndex = 60;
//            this.lblManualEntry.Text = "Manual Entry";
//            // 
//            // lblSourceData
//            // 
//            this.lblSourceData.Location = new System.Drawing.Point(156, 21);
//            this.lblSourceData.Name = "lblSourceData";
//            this.lblSourceData.Size = new System.Drawing.Size(65, 15);
//            this.lblSourceData.TabIndex = 59;
//            this.lblSourceData.Text = "Source Data";
//            // 
//            // btnCancel
//            // 
//            appearance1.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_cancel;
//            this.btnCancel.Appearance = appearance1;
//            this.btnCancel.ImageSize = new System.Drawing.Size(75, 23);
//            this.btnCancel.Location = new System.Drawing.Point(287, 242);
//            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.ShowFocusRect = false;
//            this.btnCancel.ShowOutline = false;
//            this.btnCancel.Size = new System.Drawing.Size(75, 23);
//            this.btnCancel.TabIndex = 68;
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
//            // 
//            // btnClear
//            // 
//            appearance2.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_clear;
//            this.btnClear.Appearance = appearance2;
//            this.btnClear.ImageSize = new System.Drawing.Size(75, 23);
//            this.btnClear.Location = new System.Drawing.Point(180, 242);
//            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
//            this.btnClear.Name = "btnClear";
//            this.btnClear.ShowFocusRect = false;
//            this.btnClear.ShowOutline = false;
//            this.btnClear.Size = new System.Drawing.Size(75, 23);
//            this.btnClear.TabIndex = 67;
//            // 
//            // btnSave
//            // 
//            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
//            this.btnSave.Appearance = appearance3;
//            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
//            this.btnSave.Location = new System.Drawing.Point(75, 242);
//            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
//            this.btnSave.Name = "btnSave";
//            this.btnSave.ShowFocusRect = false;
//            this.btnSave.ShowOutline = false;
//            this.btnSave.Size = new System.Drawing.Size(75, 23);
//            this.btnSave.TabIndex = 66;
//            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//            // 
//            // txtComments
//            // 
//            this.txtComments.Location = new System.Drawing.Point(5, 158);
//            this.txtComments.Multiline = true;
//            this.txtComments.Name = "txtComments";
//            this.txtComments.Size = new System.Drawing.Size(427, 77);
//            this.txtComments.TabIndex = 65;
//            // 
//            // lblComments
//            // 
//            this.lblComments.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblComments.Location = new System.Drawing.Point(4, 137);
//            this.lblComments.Name = "lblComments";
//            this.lblComments.Size = new System.Drawing.Size(65, 15);
//            this.lblComments.TabIndex = 64;
//            this.lblComments.Text = "Comments";
//            // 
//            // txtUserName
//            // 
//            this.txtUserName.AutoSize = false;
//            this.txtUserName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.txtUserName.Location = new System.Drawing.Point(80, 7);
//            this.txtUserName.Name = "txtUserName";
//            this.txtUserName.ReadOnly = true;
//            this.txtUserName.Size = new System.Drawing.Size(150, 20);
//            this.txtUserName.TabIndex = 63;
//            // 
//            // lblUserName
//            // 
//            this.lblUserName.Location = new System.Drawing.Point(4, 9);
//            this.lblUserName.Name = "lblUserName";
//            this.lblUserName.Size = new System.Drawing.Size(59, 15);
//            this.lblUserName.TabIndex = 62;
//            this.lblUserName.Text = "User Name";
//            // 
//            // lblSymbol
//            // 
//            this.lblSymbol.Location = new System.Drawing.Point(4, 38);
//            this.lblSymbol.Name = "lblSymbol";
//            this.lblSymbol.Size = new System.Drawing.Size(41, 15);
//            this.lblSymbol.TabIndex = 70;
//            this.lblSymbol.Text = "Symbol";
//            // 
//            // txtSymbol
//            // 
//            this.txtSymbol.AutoSize = false;
//            this.txtSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.txtSymbol.Location = new System.Drawing.Point(80, 33);
//            this.txtSymbol.Name = "txtSymbol";
//            this.txtSymbol.ReadOnly = true;
//            this.txtSymbol.Size = new System.Drawing.Size(150, 20);
//            this.txtSymbol.TabIndex = 71;
//            // 
//            // CtrlManualEntry
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
//            this.Controls.Add(this.txtSymbol);
//            this.Controls.Add(this.lblSymbol);
//            this.Controls.Add(this.grpDataEntry);
//            this.Controls.Add(this.btnCancel);
//            this.Controls.Add(this.btnClear);
//            this.Controls.Add(this.btnSave);
//            this.Controls.Add(this.txtComments);
//            this.Controls.Add(this.lblComments);
//            this.Controls.Add(this.txtUserName);
//            this.Controls.Add(this.lblUserName);
//            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.Name = "CtrlManualEntry";
//            this.Size = new System.Drawing.Size(434, 273);
//            this.Load += new System.EventHandler(this.CtrlManualEntry_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.grpDataEntry)).EndInit();
//            this.grpDataEntry.ResumeLayout(false);
//            this.grpDataEntry.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.txtManualEntry)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtSourceData)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtApplicationData)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtComments)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.txtSymbol)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private Infragistics.Win.Misc.UltraGroupBox grpDataEntry;
//        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtManualEntry;
//        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSourceData;
//        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtApplicationData;
//        private Infragistics.Win.Misc.UltraLabel lblApplicationData;
//        private Infragistics.Win.Misc.UltraLabel lblManualEntry;
//        private Infragistics.Win.Misc.UltraLabel lblSourceData;
//        private Infragistics.Win.Misc.UltraButton btnCancel;
//        private Infragistics.Win.Misc.UltraButton btnClear;
//        private Infragistics.Win.Misc.UltraButton btnSave;
//        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtComments;
//        private Infragistics.Win.Misc.UltraLabel lblComments;
//        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserName;
//        private Infragistics.Win.Misc.UltraLabel lblUserName;
//        private Infragistics.Win.Misc.UltraLabel lblSymbol;
//        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSymbol;

//    }
//}
