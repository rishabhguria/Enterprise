using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for GridClientFunds.
	/// </summary>
	public class GridClientFunds : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "GridClientFunds : ";
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdClientFunds;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridClientFunds()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			
			

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.grdClientFunds = new System.Windows.Forms.DataGrid();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdClientFunds)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(436, 102);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 29;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(600, 102);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 27;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(518, 102);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 26;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.grdClientFunds);
			this.groupBox1.Location = new System.Drawing.Point(0, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(678, 98);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Funds";
			// 
			// grdClientFunds
			// 
			this.grdClientFunds.CaptionVisible = false;
			this.grdClientFunds.DataMember = "";
			this.grdClientFunds.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdClientFunds.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdClientFunds.Location = new System.Drawing.Point(8, 14);
			this.grdClientFunds.Name = "grdClientFunds";
			this.grdClientFunds.ReadOnly = true;
			this.grdClientFunds.Size = new System.Drawing.Size(662, 78);
			this.grdClientFunds.TabIndex = 16;
			// 
			// GridClientFunds
			// 
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.groupBox1);
			this.Name = "GridClientFunds";
			this.Size = new System.Drawing.Size(680, 124);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdClientFunds)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private CreateCompanyClientFund createCompanyClientFund = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(createCompanyClientFund == null)
			{
				createCompanyClientFund = new CreateCompanyClientFund();				
			}
			createCompanyClientFund.ShowDialog(this.Parent);
//			grdClientFunds.DataSource = null;
//			grdClientFunds.Refresh();
			grdClientFunds.DataSource = createCompanyClientFund.CurrentClientFunds; 

			if(createCompanyClientFund.CurrentClientFunds.Count > 0 )
			{
				grdClientFunds.Select(0);
			}
		
		}

		public Nirvana.Admin.BLL.ClientFunds CurrentCompanyClientFunds
		{
			get 
			{
				return (Nirvana.Admin.BLL.ClientFunds) grdClientFunds.DataSource; 
			}						
		}

		private int _companyID = int.MinValue;
		public int CompanyID
		{
			get{return _companyID;}
			set
			{
				_companyID = value;
				BindDataGrid();
			}
		}

		private int _currentCompanyClientFundID = int.MinValue;
		public int CurrentCompanyClientFundID
		{
			get{return _currentCompanyClientFundID;}
			set
			{
				_currentCompanyClientFundID = value;
				BindDataGrid();
			}
		}

		public ClientFund CompanyClientFundProperty
		{
			get 
			{
				ClientFund clientFund = new ClientFund();
				GetClientFund(clientFund);
				return clientFund; 
			}
			set 
			{
				SetClientFund(value);
					
			}
		}

		public void GetClientFund(ClientFund clientFund)
		{
			clientFund = (ClientFund)grdClientFunds.DataSource;						
		}
		
		public void SetClientFund(ClientFund clientFund)
		{
			grdClientFunds.DataSource = clientFund;
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.ClientFunds clientFunds = ClientFundManager.GetClientFunds(_companyID);
				grdClientFunds.DataSource = clientFunds;
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

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show(this, "Do you want to delete this Client Fund?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				int clientFundID = int.Parse(grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 2].ToString());
				if(clientFundID != int.MinValue)
				{
					//CompanyManager.de .DeleteSymbolMapping(symbolMappingID);	
					//BindDataGrid();
				}
				else
				{
					Nirvana.Admin.BLL.ClientFunds clientFunds = (Nirvana.Admin.BLL.ClientFunds)grdClientFunds.DataSource;
					Nirvana.Admin.BLL.ClientFund clientFund = new Nirvana.Admin.BLL.ClientFund();					
					
					clientFund.CompanyClientFundID = int.Parse(grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 2].ToString());
					clientFund.CompanyClientFundName = grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 3].ToString();
					clientFund.CompanyClientFundShortName = grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 0].ToString();
					clientFund.CompanyClientID = int.Parse(grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 1].ToString());
					
					clientFunds.RemoveAt(clientFunds.IndexOf(clientFund));
					grdClientFunds.DataSource = null;
					grdClientFunds.DataSource = clientFunds;
					grdClientFunds.Refresh();
				}		
			}
		}

		private CreateCompanyClientFund createCompanyClientFundEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(createCompanyClientFundEdit == null)
			{
				if(grdClientFunds.VisibleRowCount > 0)
				{
					createCompanyClientFundEdit = new CreateCompanyClientFund();
					createCompanyClientFundEdit.ClientFundEdit = (ClientFund)((Nirvana.Admin.BLL.ClientFunds) grdClientFunds.DataSource)[grdClientFunds.CurrentCell.RowNumber];
					
					createCompanyClientFundEdit.CurrentClientFunds = (Nirvana.Admin.BLL.ClientFunds) grdClientFunds.DataSource;	
					createCompanyClientFundEdit.ShowDialog(this.Parent);
					createCompanyClientFundEdit = null;
				}
			}
		}
	}
}
