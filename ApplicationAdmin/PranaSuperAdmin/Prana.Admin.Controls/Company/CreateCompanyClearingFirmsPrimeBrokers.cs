using Prana.Admin.BLL;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateCompanyClearingFirmsPrimeBrokers.
    /// </summary>
    public class CreateCompanyClearingFirmsPrimeBrokers : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtClearingFirmsPrimeBrokers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpPrimeBroker;
        private IContainer components;

        public CreateCompanyClearingFirmsPrimeBrokers()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

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
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (txtClearingFirmsPrimeBrokers != null)
                {
                    txtClearingFirmsPrimeBrokers.Dispose();
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
                if (btnSave != null)
                {
                    btnSave.Dispose();
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
                if (grpPrimeBroker != null)
                {
                    grpPrimeBroker.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCompanyClearingFirmsPrimeBrokers));
            this.btnClose = new System.Windows.Forms.Button();
            this.grpPrimeBroker = new System.Windows.Forms.GroupBox();
            this.txtClearingFirmsPrimeBrokers = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.grpPrimeBroker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Location = new System.Drawing.Point(133, 76);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpPrimeBroker
            // 
            this.grpPrimeBroker.Controls.Add(this.txtClearingFirmsPrimeBrokers);
            this.grpPrimeBroker.Controls.Add(this.label1);
            this.grpPrimeBroker.Controls.Add(this.label2);
            this.grpPrimeBroker.Controls.Add(this.txtShortName);
            this.grpPrimeBroker.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpPrimeBroker.Location = new System.Drawing.Point(2, 4);
            this.grpPrimeBroker.Name = "grpPrimeBroker";
            this.grpPrimeBroker.Size = new System.Drawing.Size(256, 66);
            this.grpPrimeBroker.TabIndex = 0;
            this.grpPrimeBroker.TabStop = false;
            this.grpPrimeBroker.Text = "Clearing Firms/Prime Brokers";
            // 
            // txtClearingFirmsPrimeBrokers
            // 
            this.txtClearingFirmsPrimeBrokers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClearingFirmsPrimeBrokers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(86, 20);
            this.txtClearingFirmsPrimeBrokers.MaxLength = 50;
            this.txtClearingFirmsPrimeBrokers.Name = "txtClearingFirmsPrimeBrokers";
            this.txtClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(148, 21);
            this.txtClearingFirmsPrimeBrokers.TabIndex = 1;
            this.txtClearingFirmsPrimeBrokers.GotFocus += new System.EventHandler(this.txtClearingFirmsPrimeBrokers_GotFocus);
            this.txtClearingFirmsPrimeBrokers.LostFocus += new System.EventHandler(this.txtClearingFirmsPrimeBrokers_LostFocus);
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(86, 42);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 3;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(57, 76);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(48, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 10);
            this.label3.TabIndex = 1;
            this.label3.Text = "*";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(70, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 2;
            this.label4.Text = "*";
            // 
            // CreateCompanyClearingFirmsPrimeBrokers
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(265, 106);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grpPrimeBroker);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(248, 142);
            this.Name = "CreateCompanyClearingFirmsPrimeBrokers";
            this.Text = "Client Clearing Firm Prime Brokers";
            this.Load += new System.EventHandler(this.CreateCompanyClearingFirmsPrimeBrokers_Load);
            this.grpPrimeBroker.ResumeLayout(false);
            this.grpPrimeBroker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        #region Focus
        private void txtClearingFirmsPrimeBrokers_GotFocus(object sender, System.EventArgs e)
        {
            txtClearingFirmsPrimeBrokers.BackColor = Color.LemonChiffon;
        }
        private void txtClearingFirmsPrimeBrokers_LostFocus(object sender, System.EventArgs e)
        {
            txtClearingFirmsPrimeBrokers.BackColor = Color.White;
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


        Prana.Admin.BLL.ClearingFirmPrimeBroker _clearingFirmPrimeBrokerEdit = null;

        public Prana.Admin.BLL.ClearingFirmPrimeBroker ClearingFirmPrimeBrokerEdit
        {
            set { _clearingFirmPrimeBrokerEdit = value; }
        }

        public void BindForEdit()
        {
            if (_clearingFirmPrimeBrokerEdit != null)
            {
                txtClearingFirmsPrimeBrokers.Text = _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersName;
                txtShortName.Text = _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersShortName;
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            set { _companyID = value; }
        }

        private Prana.Admin.BLL.ClearingFirmsPrimeBrokers _clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            bool validCFPB = true;
            if (_noData == 1)
            {
                _clearingFirmsPrimeBrokers.Clear();
            }
            if (_clearingFirmPrimeBrokerEdit != null)
            {
                errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "");
                errorProvider1.SetError(txtShortName, "");
                if (txtClearingFirmsPrimeBrokers.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "Please enter Clearing Firm's Prime Broker's Name!");
                    txtClearingFirmsPrimeBrokers.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                else
                {
                    foreach (ClearingFirmPrimeBroker checkCFPB in _clearingFirmsPrimeBrokers)
                    {
                        if (checkCFPB.ClearingFirmsPrimeBrokersName.ToUpper() == txtClearingFirmsPrimeBrokers.Text.Trim().ToUpper() && checkCFPB.ClearingFirmsPrimeBrokersName != _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersName)
                        {
                            validCFPB = false;
                            MessageBox.Show(this, "The edited clearing firm prime broker with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                        if (checkCFPB.ClearingFirmsPrimeBrokersShortName.ToUpper() == txtShortName.Text.Trim().ToUpper() && checkCFPB.ClearingFirmsPrimeBrokersShortName != _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersShortName)
                        {
                            validCFPB = false;
                            MessageBox.Show(this, "The edited clearing firm prime broker with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validCFPB == true)
                    {
                        _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersName = txtClearingFirmsPrimeBrokers.Text.ToString();
                        _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersShortName = txtShortName.Text.ToString();

                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers, "Stored!");
                        this.Hide();
                    }
                }

            }
            else
            {
                errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "");
                errorProvider1.SetError(txtShortName, "");
                if (txtClearingFirmsPrimeBrokers.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "Please enter ClearingFirmsPrimeBrokers Name!");
                    txtClearingFirmsPrimeBrokers.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                else
                {
                    foreach (ClearingFirmPrimeBroker checkCFPB in _clearingFirmsPrimeBrokers)
                    {
                        if (checkCFPB.ClearingFirmsPrimeBrokersName.ToUpper() == txtClearingFirmsPrimeBrokers.Text.Trim().ToUpper())
                        {
                            validCFPB = false;
                            MessageBox.Show(this, "The clearing firm prime broker with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                        if (checkCFPB.ClearingFirmsPrimeBrokersShortName.ToUpper() == txtShortName.Text.Trim().ToUpper())
                        {
                            validCFPB = false;
                            MessageBox.Show(this, "The clearing firm prime broker with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validCFPB == true)
                    {
                        Prana.Admin.BLL.ClearingFirmPrimeBroker clearingFirmPrimeBroker = new Prana.Admin.BLL.ClearingFirmPrimeBroker();

                        clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName = txtClearingFirmsPrimeBrokers.Text.ToString();
                        clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName = txtShortName.Text.ToString();
                        _clearingFirmsPrimeBrokers.Add(clearingFirmPrimeBroker);
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers, "Stored!");
                        this.Hide();
                    }
                }
            }

        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "");
            errorProvider1.SetError(txtShortName, "");
            this.Close();
        }

        private void CreateCompanyClearingFirmsPrimeBrokers_Load(object sender, System.EventArgs e)
        {
            if (_clearingFirmPrimeBrokerEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers);
            }
        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtClearingFirmsPrimeBrokers.Text = "";
            txtShortName.Text = "";
        }

        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }

        //private Prana.Admin.BLL.ClearingFirmsPrimeBrokers _clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
        public ClearingFirmsPrimeBrokers CurrentClearingFirmsPrimeBrokers
        {
            get
            {
                return _clearingFirmsPrimeBrokers;
            }
            set
            {
                if (value != null)
                {
                    _clearingFirmsPrimeBrokers = value;
                }
            }
        }

        private void label3_Click(object sender, System.EventArgs e)
        {

        }

    }
}
