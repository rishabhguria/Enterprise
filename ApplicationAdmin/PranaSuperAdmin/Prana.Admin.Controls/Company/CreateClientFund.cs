using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateClientAccount.
    /// </summary>
    public class CreateClientAccount : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "CreateClientAccount : ";
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.TextBox txtShortName;
        private IContainer components;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpAccount;

        Prana.Admin.BLL.Account _accountEdit = null;

        //Form where do we set this AccountEdit property ?
        public Prana.Admin.BLL.Account AccountEdit
        {
            set { _accountEdit = value; }
        }

        public CreateClientAccount()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            BindForEdit();
            //TODO: Create Refresh procedure to remove any previous values from textboxes
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
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (txtAccountName != null)
                {
                    txtAccountName.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (grpAccount != null)
                {
                    grpAccount.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateClientAccount));
            this.grpAccount = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpAccount
            // 
            this.grpAccount.Controls.Add(this.label4);
            this.grpAccount.Controls.Add(this.label3);
            this.grpAccount.Controls.Add(this.txtAccountName);
            this.grpAccount.Controls.Add(this.label1);
            this.grpAccount.Controls.Add(this.label2);
            this.grpAccount.Controls.Add(this.txtShortName);
            this.grpAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpAccount.Location = new System.Drawing.Point(0, 2);
            this.grpAccount.Name = "grpAccount";
            this.grpAccount.Size = new System.Drawing.Size(274, 66);
            this.grpAccount.TabIndex = 0;
            this.grpAccount.TabStop = false;
            this.grpAccount.Text = "Account";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(70, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 4;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(40, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 8);
            this.label3.TabIndex = 1;
            this.label3.Text = "*";
            // 
            // txtAccountName
            // 
            this.txtAccountName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAccountName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAccountName.Location = new System.Drawing.Point(100, 19);
            this.txtAccountName.MaxLength = 50;
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(148, 21);
            this.txtAccountName.TabIndex = 0;
            this.txtAccountName.GotFocus += new System.EventHandler(this.txtAccountName_GotFocus);
            this.txtAccountName.LostFocus += new System.EventHandler(this.txtAccountName_LostFocus);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(100, 42);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(78, 74);
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
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(154, 74);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CreateClientAccount
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(276, 132);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpAccount);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(268, 134);
            this.Name = "CreateClientAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Account";
            this.Load += new System.EventHandler(this.CreateClientAccount_Load);
            this.grpAccount.ResumeLayout(false);
            this.grpAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus
        private void txtAccountName_GotFocus(object sender, System.EventArgs e)
        {
            txtAccountName.BackColor = Color.LemonChiffon;
        }
        private void txtAccountName_LostFocus(object sender, System.EventArgs e)
        {
            txtAccountName.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        #endregion
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public void BindForEdit()
        {
            if (_accountEdit != null)
            {
                txtAccountName.Text = _accountEdit.AccountName;
                txtShortName.Text = _accountEdit.AccountShortName;
            }

        }
        private int _companyID = int.MinValue;

        public int CompanyID
        {
            set { _companyID = value; }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                bool validAccount = true;
                if (_noData == 1)
                {
                    _accounts.Clear();
                }
                if (_accountEdit != null)
                {
                    errorProvider1.SetError(txtAccountName, "");
                    errorProvider1.SetError(txtShortName, "");
                    if (txtAccountName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtAccountName, "Please enter Account Name!");
                        txtAccountName.Focus();
                    }
                    else if (txtShortName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                        txtShortName.Focus();
                    }
                    else
                    {
                        foreach (Account checkAccount in _accounts)
                        {
                            if (checkAccount.AccountName.Trim().ToUpper() == txtAccountName.Text.Trim().ToUpper() && checkAccount.AccountName != _accountEdit.AccountName)
                            {
                                validAccount = false;
                                MessageBox.Show(this, "The edited account name with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
                                break;
                            }
                            if (checkAccount.AccountShortName.Trim().ToUpper() == txtShortName.Text.Trim().ToUpper() && checkAccount.AccountShortName != _accountEdit.AccountShortName)
                            {
                                validAccount = false;
                                MessageBox.Show(this, "The edited account name with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                                break;
                            }
                        }
                        if (validAccount == true)
                        {
                            _accountEdit.AccountName = txtAccountName.Text.ToString().Trim();
                            _accountEdit.AccountShortName = txtShortName.Text.ToString().Trim();
                            //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientAccount);
                            //Prana.Admin.Utility.Common.SetStatusPanel(stbClientAccount, "Stored!");
                            this.Hide();
                        }
                    }
                }
                else
                {
                    errorProvider1.SetError(txtAccountName, "");
                    errorProvider1.SetError(txtShortName, "");
                    if (txtAccountName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtAccountName, "Please enter Account Name!");
                        txtAccountName.Focus();
                    }
                    else if (txtShortName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                        txtShortName.Focus();
                    }
                    else
                    {
                        foreach (Account checkAccount in _accounts)
                        {
                            if (checkAccount.AccountName.Trim().ToUpper() == txtAccountName.Text.Trim().ToUpper())
                            {
                                validAccount = false;
                                MessageBox.Show(this, "The account name with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
                                break;
                            }
                            if (checkAccount.AccountShortName.Trim().ToUpper() == txtShortName.Text.Trim().ToUpper())
                            {
                                validAccount = false;
                                MessageBox.Show(this, "The account name with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                                break;
                            }
                        }
                        if (validAccount == true)
                        {
                            Prana.Admin.BLL.Account account = new Prana.Admin.BLL.Account();

                            account.AccountName = txtAccountName.Text.ToString().Trim();
                            account.AccountShortName = txtShortName.Text.ToString().Trim();
                            _accounts.Add(account);
                            //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientAccount);
                            //Prana.Admin.Utility.Common.SetStatusPanel(stbClientAccount, "Stored!");
                            this.Hide();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnLogin_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnLogin_Click", null);
                #endregion
            }
        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtAccountName.Text = "";
            txtShortName.Text = "";
            txtAccountName.Focus();
        }

        private void CreateClientAccount_Load(object sender, System.EventArgs e)
        {
            if (_accountEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientAccount);
                //Prana.Admin.Utility.Common.SetStatusPanel(stbClientAccount, "");
            }
        }

        //Shoudn't we use same Prana.Admin.BLL on the right hand side

        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }

        private Prana.Admin.BLL.Accounts _accounts = new Accounts();
        public Accounts CurrentAccounts
        {
            get
            {
                return _accounts;
            }
            set
            {
                if (value != null)
                {
                    _accounts = value;
                }
            }
        }
        //		#region
        //		private void txtAccountName_GotFocus(object sender, System.EventArgs e)
        //		{
        //			txtAccountName.BackColor = Color.LemonChiffon;
        //		}
        //		private void txtAccountName_LostFocus(object sender, System.EventArgs e)
        //		{
        //			txtAccountName.BackColor = Color.White;
        //		}
        //		private void txtShortName_GotFocus(object sender, System.EventArgs e)
        //		{
        //			txtShortName.BackColor = Color.LemonChiffon;
        //		}
        //		private void txtShortName_LostFocus(object sender, System.EventArgs e)
        //		{
        //			txtShortName.BackColor = Color.White;
        //		}
        //		#endregion

    }
}
