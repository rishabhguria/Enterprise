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
	/// Summary description for GridFunds.
	/// </summary>
	public class GridFunds : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "GridFunds : ";
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridFunds()
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.grdFunds = new System.Windows.Forms.DataGrid();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnCreate = new System.Windows.Forms.Button();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.colCompanyClientFundShortName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyClientID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyClientFundID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyClientFundName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdFunds)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.grdFunds);
			this.groupBox1.Location = new System.Drawing.Point(-2, -2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(650, 96);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Funds";
			// 
			// grdFunds
			// 
			this.grdFunds.CaptionVisible = false;
			this.grdFunds.DataMember = "";
			this.grdFunds.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdFunds.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdFunds.Location = new System.Drawing.Point(8, 14);
			this.grdFunds.Name = "grdFunds";
			this.grdFunds.ReadOnly = true;
			this.grdFunds.Size = new System.Drawing.Size(640, 78);
			this.grdFunds.TabIndex = 16;
			this.grdFunds.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								 this.dataGridTableStyle1});
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(574, 98);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 22;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(492, 98);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 21;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(410, 98);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 24;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.grdFunds;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.colCompanyClientFundName,
																												  this.colCompanyClientFundShortName,
																												  this.colCompanyClientID,
																												  this.colCompanyClientFundID});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "funds";
			// 
			// colCompanyClientFundShortName
			// 
			this.colCompanyClientFundShortName.Format = "";
			this.colCompanyClientFundShortName.FormatInfo = null;
			this.colCompanyClientFundShortName.HeaderText = "Fund Short Name";
			this.colCompanyClientFundShortName.MappingName = "CompanyClientFundShortName";
			// 
			// colCompanyClientID
			// 
			this.colCompanyClientID.Format = "";
			this.colCompanyClientID.FormatInfo = null;
			this.colCompanyClientID.MappingName = "CompanyClientID";
			this.colCompanyClientID.Width = 0;
			// 
			// colCompanyClientFundID
			// 
			this.colCompanyClientFundID.Format = "";
			this.colCompanyClientFundID.FormatInfo = null;
			this.colCompanyClientFundID.MappingName = "CompanyClientFundID";
			this.colCompanyClientFundID.Width = 0;
			// 
			// colCompanyClientFundName
			// 
			this.colCompanyClientFundName.Format = "";
			this.colCompanyClientFundName.FormatInfo = null;
			this.colCompanyClientFundName.HeaderText = "Fund Name";
			this.colCompanyClientFundName.MappingName = "CompanyClientFundName";
			this.colCompanyClientFundName.Width = 175;
			// 
			// GridFunds
			// 
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.groupBox1);
			this.Name = "GridFunds";
			this.Size = new System.Drawing.Size(650, 120);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdFunds)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdFunds;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyClientFundShortName;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyClientID;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyClientFundID;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyClientFundName;

		
		//private Nirvana.Admin.BLL.Funds _funds = new Funds();
		public Nirvana.Admin.BLL.Funds CurrentFunds
		{
			get 
			{
				return (Nirvana.Admin.BLL.Funds) grdFunds.DataSource; 
			}						
		}
		
		private CreateClientFund createClientFund = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(createClientFund == null)
			{
				createClientFund = new CreateClientFund();				
			}
			createClientFund.ShowDialog(this.Parent);
			grdFunds.DataSource = null;
			grdFunds.Refresh();
			grdFunds.DataSource = createClientFund.CurrentFunds; 

			if(createClientFund.CurrentFunds.Count > 0 )
			{
				grdFunds.Select(0);
			}
		}

		//Company id is used to pass itself as an argument to fetch the details 
		//corresponding to it from the database and bind the datagrid.
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
		
		//Why we are creating fundID property ?
		private int _fundID = int.MinValue;
		public int FundID
		{
			get{return _fundID;}
			set
			{
				_fundID = value;
				BindDataGrid();
			}
		}
		
		//Why we have created the FundPrpoerty ?
		public Fund FundProperty
		{
			get 
			{
				Fund fund = new Fund();
				GetFund(fund);
				return fund; 
			}
			set 
			{
				SetFund(value);
					
			}
		}
		
		public void GetFund(Fund fund)
		{
			fund = (Fund)grdFunds.DataSource;						
		}
		
		public void SetFund(Fund fund)
		{
			grdFunds.DataSource = fund;
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.Funds funds = CompanyManager.GetFund(_companyID);
				grdFunds.DataSource = funds;
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
			if(MessageBox.Show(this, "Do you want to delete this Fund?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				int fundID = int.Parse(grdFunds[grdFunds.CurrentCell.RowNumber, 2].ToString());
				if(fundID != int.MinValue)
				{
					//CompanyManager.de .DeleteSymbolMapping(symbolMappingID);	
					//BindDataGrid();
				}
				else
				{
					Nirvana.Admin.BLL.Funds funds = (Funds)grdFunds.DataSource;
					Nirvana.Admin.BLL.Fund fund = new Nirvana.Admin.BLL.Fund();
					
					
					fund.FundID = int.Parse(grdFunds[grdFunds.CurrentCell.RowNumber, 2].ToString());
					fund.FundName = grdFunds[grdFunds.CurrentCell.RowNumber, 1].ToString();
					fund.FundShortName = grdFunds[grdFunds.CurrentCell.RowNumber, 0].ToString();
					fund.CompanyID = int.Parse(grdFunds[grdFunds.CurrentCell.RowNumber, 3].ToString());
					
					funds.RemoveAt(funds.IndexOf(fund));
					grdFunds.DataSource = null;
					grdFunds.DataSource = funds;
					grdFunds.Refresh();
				}		
			}
		}

		private CreateClientFund createClientFundEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(createClientFundEdit == null)
			{
				if(grdFunds.VisibleRowCount > 0)
				{
					createClientFundEdit = new CreateClientFund();
//					Fund fundEdit = new Fund();
//					fundEdit.FundID = int.Parse(grdFunds[grdFunds.CurrentCell.RowNumber, 2].ToString());
//					fundEdit.FundName = grdFunds[grdFunds.CurrentCell.RowNumber, 1].ToString();
//					fundEdit.FundShortName = grdFunds[grdFunds.CurrentCell.RowNumber, 0].ToString();
//					fundEdit.CompanyID = int.Parse(grdFunds[grdFunds.CurrentCell.RowNumber, 3].ToString());

//					fundEdit =  (Fund)((Funds) grdFunds.DataSource)[grdFunds.CurrentCell.RowNumber];
					createClientFundEdit.FundEdit = (Fund)((Funds) grdFunds.DataSource)[grdFunds.CurrentCell.RowNumber];
					
					createClientFundEdit.CurrentFunds = (Nirvana.Admin.BLL.Funds) grdFunds.DataSource;	
					createClientFundEdit.ShowDialog(this.Parent);
					createClientFundEdit = null;
				}
			}
		}
	}
}
