namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlAddUploadClient
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlAddUploadClient));
            this.grpUploadClient = new System.Windows.Forms.GroupBox();
            this.lblIsFullNameEmpty = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cmbUploadClientsList = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.bindingSourceCompanyList = new System.Windows.Forms.BindingSource(this.components);
            this.grpUploadClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCompanyList)).BeginInit();
            this.SuspendLayout();
            // 
            // grpUploadClient
            // 
            this.grpUploadClient.Controls.Add(this.cmbUploadClientsList);
            this.grpUploadClient.Controls.Add(this.lblIsFullNameEmpty);
            this.grpUploadClient.Controls.Add(this.lblName);
            this.grpUploadClient.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpUploadClient.Location = new System.Drawing.Point(2, 6);
            this.grpUploadClient.Name = "grpUploadClient";
            this.grpUploadClient.Size = new System.Drawing.Size(266, 47);
            this.grpUploadClient.TabIndex = 8;
            this.grpUploadClient.TabStop = false;
            this.grpUploadClient.Text = "New Upload Client";
            // 
            // lblIsFullNameEmpty
            // 
            this.lblIsFullNameEmpty.ForeColor = System.Drawing.Color.Red;
            this.lblIsFullNameEmpty.Location = new System.Drawing.Point(63, 23);
            this.lblIsFullNameEmpty.Name = "lblIsFullNameEmpty";
            this.lblIsFullNameEmpty.Size = new System.Drawing.Size(12, 8);
            this.lblIsFullNameEmpty.TabIndex = 35;
            this.lblIsFullNameEmpty.Text = "*";
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblName.Location = new System.Drawing.Point(8, 23);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(56, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Company Name";
            // 
            // cmbUploadClientsList
            // 
            this.cmbUploadClientsList.DataSource = this.bindingSourceCompanyList;
            this.cmbUploadClientsList.DisplayMember = "FullName";
            this.cmbUploadClientsList.FormattingEnabled = true;
            this.cmbUploadClientsList.Location = new System.Drawing.Point(96, 16);
            this.cmbUploadClientsList.Name = "cmbUploadClientsList";
            this.cmbUploadClientsList.Size = new System.Drawing.Size(148, 21);
            this.cmbUploadClientsList.TabIndex = 36;
            this.cmbUploadClientsList.ValueMember = "ID";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.Location = new System.Drawing.Point(58, 62);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(138, 62);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // bindingSourceCompanyList
            // 
            this.bindingSourceCompanyList.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.CompanyNameIDList);
            // 
            // CtrlAddUploadClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpUploadClient);
            this.Controls.Add(this.btnClose);
            this.Name = "CtrlAddUploadClient";
            this.Size = new System.Drawing.Size(271, 91);            
            this.grpUploadClient.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCompanyList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox grpUploadClient;
        private System.Windows.Forms.Label lblIsFullNameEmpty;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cmbUploadClientsList;
        private System.Windows.Forms.BindingSource bindingSourceCompanyList;
    }
}
