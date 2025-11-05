using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

	
namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CreateClientFund.
	/// </summary>
	public class CreateClientFund : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "CreateClientFund : ";
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox txtFundName;
		private System.Windows.Forms.TextBox txtShortName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBar stbClientFund;

		Nirvana.Admin.BLL.Fund _fundEdit = null;

		//Form where do we set this FundEdit property ?
		public Nirvana.Admin.BLL.Fund FundEdit
		{
			set{_fundEdit = value;}
		}
		
		public CreateClientFund()
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtFundName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.stbClientFund = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtFundName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.groupBox1.Location = new System.Drawing.Point(6, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(312, 70);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Funds";
			// 
			// txtFundName
			// 
			this.txtFundName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFundName.Location = new System.Drawing.Point(148, 16);
			this.txtFundName.MaxLength = 50;
			this.txtFundName.Name = "txtFundName";
			this.txtFundName.Size = new System.Drawing.Size(148, 21);
			this.txtFundName.TabIndex = 1;
			this.txtFundName.Text = "";
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
			// txtShortName
			// 
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(148, 44);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 21);
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			// 
			// btnSave
			// 
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(68, 84);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 26);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(154, 84);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 26);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// stbClientFund
			// 
			this.stbClientFund.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.stbClientFund.Location = new System.Drawing.Point(0, 127);
			this.stbClientFund.Name = "stbClientFund";
			this.stbClientFund.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							 this.statusBarPanel1});
			this.stbClientFund.ShowPanels = true;
			this.stbClientFund.Size = new System.Drawing.Size(326, 22);
			this.stbClientFund.TabIndex = 2;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 310;
			// 
			// CreateClientFund
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(326, 149);
			this.ControlBox = false;
			this.Controls.Add(this.stbClientFund);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CreateClientFund";
			this.Text = "New Client Fund";
			this.Load += new System.EventHandler(this.CreateClientFund_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		public void BindForEdit()
		{
			if(_fundEdit != null)
			{
				txtFundName.Text = _fundEdit.FundName;
				txtShortName.Text = _fundEdit.FundShortName;
			}
			
		}
		private int _companyID = int.MinValue;
		
		public int CompanyID
		{
			set{_companyID = value;}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//			ClientFund clientFund = new ClientFund();
				//			clientFund.CompanyClientFundName = txtFundName.Text;
				//			clientFund.CompanyClientFundShortName = txtShortName.Text;
				//			clientFund.CompanyClientID = _companyID;
				//
				//			ClientFundManager.SaveClientFund(clientFund);

				if(_fundEdit != null)
				{
					//				int index = _funds.IndexOf(_fundEdit);						
					//				((Nirvana.Admin.BLL.Fund)_funds[index]).FundName = txtFundName.Text.ToString();
					//				((Nirvana.Admin.BLL.Fund)_funds[index]).FundShortName = txtShortName.Text.ToString();				
					_fundEdit.FundName = txtFundName.Text.ToString();
					_fundEdit.FundShortName = txtShortName.Text.ToString();
					SetStatusPanel(stbClientFund, "Stored!");				
				}
				else
				{
					if(txtFundName.Text.Trim() == "")
					{
						SetStatusPanel(stbClientFund, "Please enter Fund Name!");
						txtFundName.Focus();
					}
					if(txtShortName.Text.Trim() == "")
					{
						SetStatusPanel(stbClientFund, "Please enter Short Name!");
						txtShortName.Focus();
					}
					else
					{
						Nirvana.Admin.BLL.Fund fund = new Nirvana.Admin.BLL.Fund();
						
						fund.FundName = txtFundName.Text.ToString();
						fund.FundShortName = txtShortName.Text.ToString();
						_funds.Add(fund);		
						SetStatusPanel(stbClientFund, "Stored!");
					}
				}
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnLogin_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnLogin_Click"); 
				Logger.Write(logEntry); 

				#endregion
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
			txtFundName.Text = "";
			txtShortName.Text = "";
		}
		
		private void CreateClientFund_Load(object sender, System.EventArgs e)
		{
			if(_fundEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				SetStatusPanel(stbClientFund, "");
			}
		}

		//Shoudn't we use same Nirvana.Admin.BLL on the right hand side
		private Nirvana.Admin.BLL.Funds _funds = new Funds();
		public Funds CurrentFunds
		{
			get 
			{
				return _funds; 
			}
			set
			{
				if(value != null)
				{
					_funds = value;
				}
			}			
		}


	}
}
