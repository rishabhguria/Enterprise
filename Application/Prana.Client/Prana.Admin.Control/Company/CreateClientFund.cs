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
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;

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
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtFundName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(9, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(319, 72);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Funds";
			// 
			// txtFundName
			// 
			this.txtFundName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFundName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFundName.Location = new System.Drawing.Point(142, 20);
			this.txtFundName.MaxLength = 50;
			this.txtFundName.Name = "txtFundName";
			this.txtFundName.Size = new System.Drawing.Size(148, 21);
			this.txtFundName.TabIndex = 1;
			this.txtFundName.Text = "";
			this.txtFundName.LostFocus += new System.EventHandler(this.txtFundName_LostFocus);
			this.txtFundName.GotFocus += new System.EventHandler(this.txtFundName_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(142, 48);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 21);
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(80, 80);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 23);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(166, 80);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 23);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// stbClientFund
			// 
			this.stbClientFund.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.stbClientFund.Location = new System.Drawing.Point(0, 113);
			this.stbClientFund.Name = "stbClientFund";
			this.stbClientFund.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							 this.statusBarPanel1});
			this.stbClientFund.ShowPanels = true;
			this.stbClientFund.Size = new System.Drawing.Size(330, 22);
			this.stbClientFund.TabIndex = 2;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 314;
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
			this.label4.Location = new System.Drawing.Point(138, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 33;
			this.label4.Text = "*";
			// 
			// CreateClientFund
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(330, 135);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.stbClientFund);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
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
				if(_fundEdit != null)
				{
					errorProvider1.SetError(txtFundName, "");
					errorProvider1.SetError(txtShortName, "");
					if(txtFundName.Text.Trim() == "")
					{
						errorProvider1.SetError(txtFundName, "Please enter Fund Name!");
						txtFundName.Focus();
					}
					else if(txtShortName.Text.Trim() == "")
					{
						errorProvider1.SetError(txtShortName, "Please enter Short Name!");
						txtShortName.Focus();
					}
					{
						_fundEdit.FundName = txtFundName.Text.ToString();
						_fundEdit.FundShortName = txtShortName.Text.ToString();
						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientFund);
						Nirvana.Admin.Utility.Common.SetStatusPanel(stbClientFund, "Stored!");
					}
				}
				else
				{
					errorProvider1.SetError(txtFundName, "");
					errorProvider1.SetError(txtShortName, "");
					if(txtFundName.Text.Trim() == "")
					{
						errorProvider1.SetError(txtFundName, "Please enter Fund Name!");
						txtFundName.Focus();
					}
					else if(txtShortName.Text.Trim() == "")
					{
						errorProvider1.SetError(txtShortName, "Please enter Short Name!");
						txtShortName.Focus();
					}
					else
					{
						Nirvana.Admin.BLL.Fund fund = new Nirvana.Admin.BLL.Fund();
						
						fund.FundName = txtFundName.Text.ToString();
						fund.FundShortName = txtShortName.Text.ToString();
						_funds.Add(fund);		
						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientFund);
						Nirvana.Admin.Utility.Common.SetStatusPanel(stbClientFund, "Stored!");
						this.Hide();
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
				Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientFund);
				Nirvana.Admin.Utility.Common.SetStatusPanel(stbClientFund, "");
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
		#region
		private void txtFundName_GotFocus(object sender, System.EventArgs e)
		{
			txtFundName.BackColor = Color.LemonChiffon;
		}
		private void txtFundName_LostFocus(object sender, System.EventArgs e)
		{
			txtFundName.BackColor = Color.White;
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
	
	}
}
