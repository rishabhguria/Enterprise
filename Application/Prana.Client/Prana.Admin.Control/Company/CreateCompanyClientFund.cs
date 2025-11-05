using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CreateCompanyClientFund.
	/// </summary>
	public class CreateCompanyClientFund : System.Windows.Forms.Form
	{
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.StatusBar stbClientFund;
		private System.Windows.Forms.TextBox txtClientFundName;
		private System.Windows.Forms.TextBox txtClientShortName;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCompanyClientFund()
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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtClientFundName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtClientShortName = new System.Windows.Forms.TextBox();
			this.stbClientFund = new System.Windows.Forms.StatusBar();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 320;
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(87, 78);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 23);
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(165, 78);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 23);
			this.btnClose.TabIndex = 9;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtClientFundName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtClientShortName);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(13, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(321, 74);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Company Client Funds";
			// 
			// txtClientFundName
			// 
			this.txtClientFundName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtClientFundName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtClientFundName.Location = new System.Drawing.Point(142, 20);
			this.txtClientFundName.MaxLength = 50;
			this.txtClientFundName.Name = "txtClientFundName";
			this.txtClientFundName.Size = new System.Drawing.Size(148, 21);
			this.txtClientFundName.TabIndex = 1;
			this.txtClientFundName.Text = "";
			this.txtClientFundName.LostFocus += new System.EventHandler(this.txtClientFundName_LostFocus);
			this.txtClientFundName.GotFocus += new System.EventHandler(this.txtClientFundName_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// txtClientShortName
			// 
			this.txtClientShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtClientShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtClientShortName.Location = new System.Drawing.Point(142, 48);
			this.txtClientShortName.MaxLength = 50;
			this.txtClientShortName.Name = "txtClientShortName";
			this.txtClientShortName.Size = new System.Drawing.Size(148, 21);
			this.txtClientShortName.TabIndex = 1;
			this.txtClientShortName.Text = "";
			this.txtClientShortName.LostFocus += new System.EventHandler(this.txtClientShortName_LostFocus);
			this.txtClientShortName.GotFocus += new System.EventHandler(this.txtClientShortName_GotFocus);
			// 
			// stbClientFund
			// 
			this.stbClientFund.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.stbClientFund.Location = new System.Drawing.Point(0, 107);
			this.stbClientFund.Name = "stbClientFund";
			this.stbClientFund.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							 this.statusBarPanel1});
			this.stbClientFund.ShowPanels = true;
			this.stbClientFund.Size = new System.Drawing.Size(336, 22);
			this.stbClientFund.TabIndex = 10;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(130, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 14);
			this.label3.TabIndex = 33;
			this.label3.Text = "*";
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(142, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 33;
			this.label4.Text = "*";
			// 
			// CreateCompanyClientFund
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(336, 129);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.stbClientFund);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CreateCompanyClientFund";
			this.Text = "CreateCompanyClientFund";
			this.Load += new System.EventHandler(this.CreateCompanyClientFund_Load);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	
		Nirvana.Admin.BLL.ClientFund _clientFundEdit = null;
		public Nirvana.Admin.BLL.ClientFund ClientFundEdit
		{
			set{_clientFundEdit = value;}
		}

		public void BindForEdit()
		{
			if(_clientFundEdit != null)
			{
				txtClientFundName.Text = _clientFundEdit.CompanyClientFundName;
				txtClientShortName.Text = _clientFundEdit.CompanyClientFundShortName;
			}
		}
	
		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set{_companyID = value;}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(_clientFundEdit != null)
			{
				errorProvider1.SetError(txtClientFundName, "");
				errorProvider1.SetError(txtClientShortName, "");
				if(txtClientFundName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtClientFundName, "Please enter Client Fund Name!");
					txtClientFundName.Focus();
				}
				else if(txtClientShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtClientShortName, "Please enter Short Name!");
					txtClientShortName.Focus();
				}
				else
				{
					_clientFundEdit.CompanyClientFundName = txtClientFundName.Text.ToString();
					_clientFundEdit.CompanyClientFundShortName = txtClientShortName.Text.ToString();
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientFund);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbClientFund, "Stored!");	
				}
			}
			else
			{
				errorProvider1.SetError(txtClientFundName, "");
				errorProvider1.SetError(txtClientShortName, "");
				if(txtClientFundName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtClientFundName, "Please enter Client Fund Name!");
					txtClientFundName.Focus();
				}
				else if(txtClientShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtClientShortName, "Please enter Short Name!");
					txtClientShortName.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.ClientFund clientFund = new Nirvana.Admin.BLL.ClientFund();
						
					clientFund.CompanyClientFundName = txtClientFundName.Text.ToString();
					clientFund.CompanyClientFundShortName = txtClientShortName.Text.ToString();
					_clientFunds.Add(clientFund);		
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientFund);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbClientFund, "Stored!");
						this.Hide();
				}
			}
		
		}

		public void Refresh(object sender, System.EventArgs e)
		{
			txtClientFundName.Text = "";
			txtClientShortName.Text = "";
		}

		private void CreateCompanyClientFund_Load(object sender, System.EventArgs e)
		{
			if(_clientFundEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientFund);
			}
		}
	
		private Nirvana.Admin.BLL.ClientFunds _clientFunds = new Nirvana.Admin.BLL.ClientFunds();
		public Nirvana.Admin.BLL.ClientFunds CurrentClientFunds
		{
			get 
			{
				return _clientFunds; 
			}
			set
			{
				if(value != null)
				{
					_clientFunds = value;
				}
			}			
		}
		#region Focus Colors
		private void txtClientFundName_GotFocus(object sender, System.EventArgs e)
		{
			txtClientFundName.BackColor = Color.LemonChiffon;
		}
		private void txtClientFundName_LostFocus(object sender, System.EventArgs e)
		{
			txtClientFundName.BackColor = Color.White;
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
