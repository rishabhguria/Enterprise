namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlAddTransactionType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlAddTransactionType));
            this.grpAddTransactionType = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblMandatory = new Infragistics.Win.Misc.UltraLabel();
            this.lblName = new Infragistics.Win.Misc.UltraLabel();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.grpAddTransactionType)).BeginInit();
            this.grpAddTransactionType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            this.SuspendLayout();
            // 
            // grpAddTransactionType
            // 
            this.grpAddTransactionType.Controls.Add(this.txtName);
            this.grpAddTransactionType.Controls.Add(this.lblMandatory);
            this.grpAddTransactionType.Controls.Add(this.lblName);
            this.grpAddTransactionType.Location = new System.Drawing.Point(4, 4);
            this.grpAddTransactionType.Name = "grpAddTransactionType";
            this.grpAddTransactionType.Size = new System.Drawing.Size(255, 49);
            this.grpAddTransactionType.SupportThemes = false;
            this.grpAddTransactionType.TabIndex = 8;
            this.grpAddTransactionType.Text = "Add Transaction Type";
            // 
            // txtName
            // 
            this.txtName.AutoSize = false;
            this.txtName.Location = new System.Drawing.Point(95, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 20);
            this.txtName.TabIndex = 41;
            // 
            // lblMandatory
            // 
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.lblMandatory.Appearance = appearance1;
            this.lblMandatory.Location = new System.Drawing.Point(78, 16);
            this.lblMandatory.Name = "lblMandatory";
            this.lblMandatory.Size = new System.Drawing.Size(11, 15);
            this.lblMandatory.TabIndex = 40;
            this.lblMandatory.Text = "*";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(6, 21);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(37, 15);
            this.lblName.TabIndex = 39;
            this.lblName.Text = "Name";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance2.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_add;
            this.btnAdd.Appearance = appearance2;
            this.btnAdd.ImageSize = new System.Drawing.Size(75, 23);
            this.btnAdd.Location = new System.Drawing.Point(47, 58);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ShowFocusRect = false;
            this.btnAdd.ShowOutline = false;
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Add";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnClose.Appearance = appearance3;
            this.btnClose.ImageSize = new System.Drawing.Size(75, 23);
            this.btnClose.Location = new System.Drawing.Point(140, 58);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            // 
            // CtrlAddTransactionType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpAddTransactionType);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlAddTransactionType";
            this.Size = new System.Drawing.Size(262, 86);
            ((System.ComponentModel.ISupportInitialize)(this.grpAddTransactionType)).EndInit();
            this.grpAddTransactionType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox grpAddTransactionType;
        private Infragistics.Win.Misc.UltraLabel lblMandatory;
        private Infragistics.Win.Misc.UltraLabel lblName;
        private Infragistics.Win.Misc.UltraButton btnAdd;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;


    }
}
