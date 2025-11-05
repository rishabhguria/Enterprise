using Prana.Admin.BLL;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridClientClearer.
    /// </summary>
    public class ClientClearer : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.TextBox txtClearerName;
        private System.Windows.Forms.Label lblClearerName;
        private System.Windows.Forms.GroupBox grpClearer;
        private System.Windows.Forms.Label lblClearingFirmBrokerID;
        private System.Windows.Forms.TextBox txtClearingFirmBrokerID;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtClientClearerShortName;
        private System.Windows.Forms.Label lblClientClearerShortName;
        private IContainer components;
        #region Constructor
        public ClientClearer()
        {

            InitializeComponent();



        }

        #endregion


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
                if (txtClearerName != null)
                {
                    txtClearerName.Dispose();
                }
                if (lblClearerName != null)
                {
                    lblClearerName.Dispose();
                }
                if (grpClearer != null)
                {
                    grpClearer.Dispose();
                }
                if (lblClearingFirmBrokerID != null)
                {
                    lblClearingFirmBrokerID.Dispose();
                }
                if (txtClearingFirmBrokerID != null)
                {
                    txtClearingFirmBrokerID.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (txtClientClearerShortName != null)
                {
                    txtClientClearerShortName.Dispose();
                }
                if (lblClientClearerShortName != null)
                {
                    lblClientClearerShortName.Dispose();
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
            this.txtClearerName = new System.Windows.Forms.TextBox();
            this.txtClientClearerShortName = new System.Windows.Forms.TextBox();
            this.lblClearerName = new System.Windows.Forms.Label();
            this.lblClientClearerShortName = new System.Windows.Forms.Label();
            this.grpClearer = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClearingFirmBrokerID = new System.Windows.Forms.TextBox();
            this.lblClearingFirmBrokerID = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpClearer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtClearerName
            // 
            this.txtClearerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClearerName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClearerName.Location = new System.Drawing.Point(127, 22);
            this.txtClearerName.MaxLength = 50;
            this.txtClearerName.Name = "txtClearerName";
            this.txtClearerName.Size = new System.Drawing.Size(100, 21);
            this.txtClearerName.TabIndex = 0;
            this.txtClearerName.GotFocus += new System.EventHandler(this.txtClearerName_GotFocus);
            this.txtClearerName.LostFocus += new System.EventHandler(this.txtClearerName_LostFocus);
            // 
            // txtClientClearerShortName
            // 
            this.txtClientClearerShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientClearerShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClientClearerShortName.Location = new System.Drawing.Point(127, 44);
            this.txtClientClearerShortName.MaxLength = 50;
            this.txtClientClearerShortName.Name = "txtClientClearerShortName";
            this.txtClientClearerShortName.Size = new System.Drawing.Size(100, 21);
            this.txtClientClearerShortName.TabIndex = 1;
            this.txtClientClearerShortName.GotFocus += new System.EventHandler(this.txtClientClearerShortName_GotFocus);
            this.txtClientClearerShortName.LostFocus += new System.EventHandler(this.txtClientClearerShortName_LostFocus);
            // 
            // lblClearerName
            // 
            this.lblClearerName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClearerName.Location = new System.Drawing.Point(8, 23);
            this.lblClearerName.Name = "lblClearerName";
            this.lblClearerName.Size = new System.Drawing.Size(47, 18);
            this.lblClearerName.TabIndex = 2;
            this.lblClearerName.Text = "Name";
            // 
            // lblClientClearerShortName
            // 
            this.lblClientClearerShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClientClearerShortName.Location = new System.Drawing.Point(8, 45);
            this.lblClientClearerShortName.Name = "lblClientClearerShortName";
            this.lblClientClearerShortName.Size = new System.Drawing.Size(71, 18);
            this.lblClientClearerShortName.TabIndex = 3;
            this.lblClientClearerShortName.Text = "Short Name";
            // 
            // grpClearer
            // 
            this.grpClearer.Controls.Add(this.label5);
            this.grpClearer.Controls.Add(this.label1);
            this.grpClearer.Controls.Add(this.label3);
            this.grpClearer.Controls.Add(this.txtClearingFirmBrokerID);
            this.grpClearer.Controls.Add(this.lblClearingFirmBrokerID);
            this.grpClearer.Controls.Add(this.lblClearerName);
            this.grpClearer.Controls.Add(this.lblClientClearerShortName);
            this.grpClearer.Controls.Add(this.txtClearerName);
            this.grpClearer.Controls.Add(this.txtClientClearerShortName);
            this.grpClearer.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpClearer.Location = new System.Drawing.Point(2, 6);
            this.grpClearer.Name = "grpClearer";
            this.grpClearer.Size = new System.Drawing.Size(249, 90);
            this.grpClearer.TabIndex = 0;
            this.grpClearer.TabStop = false;
            this.grpClearer.Text = "Details";
            this.grpClearer.Enter += new System.EventHandler(this.grpClearer_Enter);
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(81, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(8, 9);
            this.label5.TabIndex = 37;
            this.label5.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(80, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 9);
            this.label1.TabIndex = 35;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(56, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 9);
            this.label3.TabIndex = 34;
            this.label3.Text = "*";
            // 
            // txtClearingFirmBrokerID
            // 
            this.txtClearingFirmBrokerID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClearingFirmBrokerID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClearingFirmBrokerID.Location = new System.Drawing.Point(127, 66);
            this.txtClearingFirmBrokerID.MaxLength = 50;
            this.txtClearingFirmBrokerID.Name = "txtClearingFirmBrokerID";
            this.txtClearingFirmBrokerID.Size = new System.Drawing.Size(100, 21);
            this.txtClearingFirmBrokerID.TabIndex = 2;
            this.txtClearingFirmBrokerID.GotFocus += new System.EventHandler(this.txtClearingFirmBrokerID_GotFocus);
            this.txtClearingFirmBrokerID.LostFocus += new System.EventHandler(this.txtClearingFirmBrokerID_LostFocus);
            // 
            // lblClearingFirmBrokerID
            // 
            this.lblClearingFirmBrokerID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblClearingFirmBrokerID.Location = new System.Drawing.Point(8, 67);
            this.lblClearingFirmBrokerID.Name = "lblClearingFirmBrokerID";
            this.lblClearingFirmBrokerID.Size = new System.Drawing.Size(71, 18);
            this.lblClearingFirmBrokerID.TabIndex = 4;
            this.lblClearingFirmBrokerID.Text = "Broker ID";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ClientClearer
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpClearer);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "ClientClearer";
            this.Size = new System.Drawing.Size(255, 102);
            this.grpClearer.ResumeLayout(false);
            this.grpClearer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        #region  Private Methods

        private void SetClientClearerDetails(CompanyClientClearer companyClientClearer)
        {
            if (companyClientClearer.CompanyClientClearerName != String.Empty)
                txtClearerName.Text = companyClientClearer.CompanyClientClearerName;
            else
                txtClearerName.Text = "";
            //if(companyClientClearer.ClearingFirmBrokerID != int.MinValue)
            txtClearingFirmBrokerID.Text = companyClientClearer.ClearingFirmBrokerID.ToString();
            //else
            //txtClearingFirmBrokerID.Text="";
            if (companyClientClearer.CompanyClientClearerShortName != String.Empty)
                txtClientClearerShortName.Text = companyClientClearer.CompanyClientClearerShortName;
            else
                txtClientClearerShortName.Text = "";

        }
        private CompanyClientClearer GetClientClearerDetails()
        {
            //bool bISValid=true;
            errorProvider1.SetError(txtClearerName, "");
            errorProvider1.SetError(txtClearingFirmBrokerID, "");
            errorProvider1.SetError(txtClientClearerShortName, "");
            System.Text.RegularExpressions.Regex rgnumber = new System.Text.RegularExpressions.Regex(@"^\d+$");
            if (txtClearerName.Text == String.Empty)
            {
                errorProvider1.SetError(txtClearerName, "Please Enter a value!");
                txtClearerName.Focus();
                //bISValid=false;			

            }
            else if (txtClientClearerShortName.Text == String.Empty)
            {
                errorProvider1.SetError(txtClientClearerShortName, "Please Enter a value!");
                txtClientClearerShortName.Focus();
                //bISValid=false;		
            }
            else if (txtClearingFirmBrokerID.Text == string.Empty)
            {
                errorProvider1.SetError(txtClearingFirmBrokerID, "Please Enter a value!");
                txtClearingFirmBrokerID.Focus();
                //bISValid=false;		
            }
            else
            {
                CompanyClientClearer ClientClearerDetails = new CompanyClientClearer();
                ClientClearerDetails.CompanyClientClearerName = txtClearerName.Text;
                ClientClearerDetails.ClearingFirmBrokerID = txtClearingFirmBrokerID.Text;
                ClientClearerDetails.CompanyClientClearerShortName = txtClientClearerShortName.Text;
                return ClientClearerDetails;
            }
            return null;

        }

        #endregion

        #region Focus Colors
        private void txtClearerName_GotFocus(object sender, System.EventArgs e)
        {
            txtClearerName.BackColor = Color.LemonChiffon;
        }
        private void txtClearerName_LostFocus(object sender, System.EventArgs e)
        {
            txtClearerName.BackColor = Color.White;
        }
        private void txtClearingFirmBrokerID_GotFocus(object sender, System.EventArgs e)
        {
            txtClearingFirmBrokerID.BackColor = Color.LemonChiffon;
        }
        private void txtClearingFirmBrokerID_LostFocus(object sender, System.EventArgs e)
        {
            txtClearingFirmBrokerID.BackColor = Color.White;
        }
        private void txtClientClearerShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtClientClearerShortName.BackColor = Color.LemonChiffon;
        }
        private void txtClientClearerShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtClientClearerShortName.BackColor = Color.White;
        }
        #endregion
        private void grpClearer_Enter(object sender, System.EventArgs e)
        {

        }
        #region  Get Set Properties
        public CompanyClientClearer CompanyClientClearer
        {
            get { return GetClientClearerDetails(); }
            //set{SetClientClearerDetails(value);}
        }
        public void SetupControl(int clientID)
        {
            CompanyClientClearer companyClientClearer = CompanyClientClearerManager.GetCompanyClientClearer(clientID);
            SetClientClearerDetails(companyClientClearer);
        }


        #endregion









    }
}
