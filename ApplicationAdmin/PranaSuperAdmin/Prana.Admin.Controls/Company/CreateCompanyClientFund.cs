using Prana.Admin.BLL;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateCompanyClientAccount.
    /// </summary>
    public class CreateCompanyClientAccount : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtClientAccountName;
        private System.Windows.Forms.TextBox txtClientShortName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpCompanyClientAccounts;
        private IContainer components;

        public CreateCompanyClientAccount()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            BindForEdit();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtClientAccountName != null)
                {
                    txtClientAccountName.Dispose();
                }
                if (txtClientShortName != null)
                {
                    txtClientShortName.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (grpCompanyClientAccounts != null)
                {
                    grpCompanyClientAccounts.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCompanyClientAccount));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpCompanyClientAccounts = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClientAccountName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClientShortName = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.grpCompanyClientAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(53, 70);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(131, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpCompanyClientAccounts
            // 
            this.grpCompanyClientAccounts.Controls.Add(this.label3);
            this.grpCompanyClientAccounts.Controls.Add(this.txtClientAccountName);
            this.grpCompanyClientAccounts.Controls.Add(this.label1);
            this.grpCompanyClientAccounts.Controls.Add(this.label2);
            this.grpCompanyClientAccounts.Controls.Add(this.txtClientShortName);
            this.grpCompanyClientAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCompanyClientAccounts.Location = new System.Drawing.Point(2, 0);
            this.grpCompanyClientAccounts.Name = "grpCompanyClientAccounts";
            this.grpCompanyClientAccounts.Size = new System.Drawing.Size(254, 68);
            this.grpCompanyClientAccounts.TabIndex = 0;
            this.grpCompanyClientAccounts.TabStop = false;
            this.grpCompanyClientAccounts.Text = "Client Accounts";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(46, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 8);
            this.label3.TabIndex = 1;
            this.label3.Text = "*";
            // 
            // txtClientAccountName
            // 
            this.txtClientAccountName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientAccountName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClientAccountName.Location = new System.Drawing.Point(86, 22);
            this.txtClientAccountName.MaxLength = 50;
            this.txtClientAccountName.Name = "txtClientAccountName";
            this.txtClientAccountName.Size = new System.Drawing.Size(148, 21);
            this.txtClientAccountName.TabIndex = 0;
            this.txtClientAccountName.GotFocus += new System.EventHandler(this.txtClientAccountName_GotFocus);
            this.txtClientAccountName.LostFocus += new System.EventHandler(this.txtClientAccountName_LostFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Short Name";
            // 
            // txtClientShortName
            // 
            this.txtClientShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClientShortName.Location = new System.Drawing.Point(86, 44);
            this.txtClientShortName.MaxLength = 50;
            this.txtClientShortName.Name = "txtClientShortName";
            this.txtClientShortName.Size = new System.Drawing.Size(148, 21);
            this.txtClientShortName.TabIndex = 1;
            this.txtClientShortName.GotFocus += new System.EventHandler(this.txtClientShortName_GotFocus);
            this.txtClientShortName.LostFocus += new System.EventHandler(this.txtClientShortName_LostFocus);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(70, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 1;
            this.label4.Text = "*";
            // 
            // CreateCompanyClientAccount
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(262, 97);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grpCompanyClientAccounts);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CreateCompanyClientAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Client Account";
            this.Load += new System.EventHandler(this.CreateCompanyClientAccount_Load);
            this.grpCompanyClientAccounts.ResumeLayout(false);
            this.grpCompanyClientAccounts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion



        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        Prana.Admin.BLL.ClientAccount _clientAccountEdit = null;
        public Prana.Admin.BLL.ClientAccount ClientAccountEdit
        {
            set { _clientAccountEdit = value; }
        }

        public void BindForEdit()
        {
            if (_clientAccountEdit != null)
            {
                txtClientAccountName.Text = _clientAccountEdit.CompanyClientAccountName;
                txtClientShortName.Text = _clientAccountEdit.CompanyClientAccountShortName;
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            set { _companyID = value; }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            bool validClientAccount = true;
            if (_noData == 1)
            {
                _clientAccounts.Clear();
            }
            if (_clientAccountEdit != null)
            {
                errorProvider1.SetError(txtClientAccountName, "");
                errorProvider1.SetError(txtClientShortName, "");
                if (txtClientAccountName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtClientAccountName, "Please enter Client Account Name!");
                    txtClientAccountName.Focus();
                }
                else if (txtClientShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtClientShortName, "Please enter Short Name!");
                    txtClientShortName.Focus();
                }
                else
                {
                    foreach (ClientAccount checkClientAccount in _clientAccounts)
                    {
                        if (checkClientAccount.CompanyClientAccountName.ToUpper() == txtClientAccountName.Text.Trim().ToUpper() && checkClientAccount.CompanyClientAccountName != _clientAccountEdit.CompanyClientAccountName)
                        {
                            validClientAccount = false;
                            MessageBox.Show(this, "The edited account with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                        if (checkClientAccount.CompanyClientAccountShortName.ToUpper() == txtClientShortName.Text.Trim().ToUpper() && checkClientAccount.CompanyClientAccountShortName != _clientAccountEdit.CompanyClientAccountShortName)
                        {
                            validClientAccount = false;
                            MessageBox.Show(this, "The edited account with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validClientAccount == true)
                    {
                        _clientAccountEdit.CompanyClientAccountName = txtClientAccountName.Text.ToString();
                        _clientAccountEdit.CompanyClientAccountShortName = txtClientShortName.Text.ToString();
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientAccount);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbClientAccount, "Stored!");	
                        this.Hide();
                    }
                }
            }
            else
            {
                errorProvider1.SetError(txtClientAccountName, "");
                errorProvider1.SetError(txtClientShortName, "");
                if (txtClientAccountName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtClientAccountName, "Please enter Client Account Name!");
                    txtClientAccountName.Focus();
                }
                else if (txtClientShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtClientShortName, "Please enter Short Name!");
                    txtClientShortName.Focus();
                }
                else
                {
                    foreach (ClientAccount checkClientAccount in _clientAccounts)
                    {
                        if (checkClientAccount.CompanyClientAccountName.ToUpper() == txtClientAccountName.Text.Trim().ToUpper())
                        {
                            validClientAccount = false;
                            MessageBox.Show(this, "The account with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                        if (checkClientAccount.CompanyClientAccountShortName.ToUpper() == txtClientShortName.Text.Trim().ToUpper())
                        {
                            validClientAccount = false;
                            MessageBox.Show(this, "The account with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validClientAccount == true)
                    {
                        Prana.Admin.BLL.ClientAccount clientAccount = new Prana.Admin.BLL.ClientAccount();

                        clientAccount.CompanyClientAccountName = txtClientAccountName.Text.ToString();
                        clientAccount.CompanyClientAccountShortName = txtClientShortName.Text.ToString();
                        _clientAccounts.Add(clientAccount);
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientAccount);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbClientAccount, "Stored!");
                        this.Hide();
                    }
                }
            }

        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtClientAccountName.Text = "";
            txtClientShortName.Text = "";
        }

        private void CreateCompanyClientAccount_Load(object sender, System.EventArgs e)
        {
            if (_clientAccountEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientAccount);
            }
        }

        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }

        private Prana.Admin.BLL.ClientAccounts _clientAccounts = new Prana.Admin.BLL.ClientAccounts();
        public Prana.Admin.BLL.ClientAccounts CurrentClientAccounts
        {
            get
            {
                return _clientAccounts;
            }
            set
            {
                if (value != null)
                {
                    _clientAccounts = value;
                }
            }
        }
        #region Focus Colors
        private void txtClientAccountName_GotFocus(object sender, System.EventArgs e)
        {
            txtClientAccountName.BackColor = Color.LemonChiffon;
        }
        private void txtClientAccountName_LostFocus(object sender, System.EventArgs e)
        {
            txtClientAccountName.BackColor = Color.White;
        }
        private void txtClientShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtClientShortName.BackColor = Color.LemonChiffon;
        }
        private void txtClientShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtClientShortName.BackColor = Color.White;
        }
        #endregion
    }
}
