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
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 288;
			// 
			// btnSave
			// 
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(68, 80);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 26);
			this.btnSave.TabIndex = 9;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(154, 80);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 26);
			this.btnClose.TabIndex = 8;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtClientFundName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtClientShortName);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.groupBox1.Location = new System.Drawing.Point(6, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(300, 70);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Company Client Funds";
			// 
			// txtClientFundName
			// 
			this.txtClientFundName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtClientFundName.Location = new System.Drawing.Point(148, 16);
			this.txtClientFundName.MaxLength = 50;
			this.txtClientFundName.Name = "txtClientFundName";
			this.txtClientFundName.Size = new System.Drawing.Size(148, 21);
			this.txtClientFundName.TabIndex = 1;
			this.txtClientFundName.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(14, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Fund Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(14, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// txtClientShortName
			// 
			this.txtClientShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtClientShortName.Location = new System.Drawing.Point(148, 44);
			this.txtClientShortName.MaxLength = 50;
			this.txtClientShortName.Name = "txtClientShortName";
			this.txtClientShortName.Size = new System.Drawing.Size(148, 21);
			this.txtClientShortName.TabIndex = 1;
			this.txtClientShortName.Text = "";
			// 
			// stbClientFund
			// 
			this.stbClientFund.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.stbClientFund.Location = new System.Drawing.Point(0, 109);
			this.stbClientFund.Name = "stbClientFund";
			this.stbClientFund.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							 this.statusBarPanel1});
			this.stbClientFund.ShowPanels = true;
			this.stbClientFund.Size = new System.Drawing.Size(304, 22);
			this.stbClientFund.TabIndex = 10;
			// 
			// CreateCompanyClientFund
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 131);
			this.ControlBox = false;
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.stbClientFund);
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
			//			ClientFund clientFund = new ClientFund();
			//			clientFund.CompanyClientFundName = txtFundName.Text;
			//			clientFund.CompanyClientFundShortName = txtShortName.Text;
			//			clientFund.CompanyClientID = _companyID;
			//
			//			ClientFundManager.SaveClientFund(clientFund);

			if(_clientFundEdit != null)
			{
				//				int index = _funds.IndexOf(_fundEdit);						
				//				((Nirvana.Admin.BLL.Fund)_funds[index]).FundName = txtFundName.Text.ToString();
				//				((Nirvana.Admin.BLL.Fund)_funds[index]).FundShortName = txtShortName.Text.ToString();				
				_clientFundEdit.CompanyClientFundName = txtClientFundName.Text.ToString();
				_clientFundEdit.CompanyClientFundShortName = txtClientShortName.Text.ToString();
				SetStatusPanel(stbClientFund, "Stored!");				
			}
			else
			{
				if(txtClientFundName.Text.Trim() == "")
				{
					SetStatusPanel(stbClientFund, "Please enter Client Fund Name!");
					txtClientFundName.Focus();
				}
				if(txtClientShortName.Text.Trim() == "")
				{
					SetStatusPanel(stbClientFund, "Please enter Short Name!");
					txtClientShortName.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.ClientFund clientFund = new Nirvana.Admin.BLL.ClientFund();
						
					clientFund.CompanyClientFundName = txtClientFundName.Text.ToString();
					clientFund.CompanyClientFundShortName = txtClientShortName.Text.ToString();
					_clientFunds.Add(clientFund);		
					SetStatusPanel(stbClientFund, "Stored!");
				}
			}
		}
		
		public static void SetStatusPanel(StatusBar statusBar, string displayText)
		{
			StatusBarPanel pnlStatus = new StatusBarPanel();
			pnlStatus.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			pnlStatus.Text = displayText;					
			pnlStatus.Alignment = HorizontalAlignment.Left;
			statusBar.Panels.Add(pnlStatus);	
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
				SetStatusPanel(stbClientFund, "");
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
	}
}
