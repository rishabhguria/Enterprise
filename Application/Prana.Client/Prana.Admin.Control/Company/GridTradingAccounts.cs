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
	/// Summary description for GridTradingAccounts.
	/// </summary>
	public class GridTradingAccounts : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "GridTradingAccounts : ";
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridTradingAccounts()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
			this.grdTradingAccounts = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnCreate = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdTradingAccounts)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.grdTradingAccounts);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(0, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(568, 92);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trading Accounts";
			// 
			// grdTradingAccounts
			// 
			this.grdTradingAccounts.CaptionVisible = false;
			this.grdTradingAccounts.DataMember = "";
			this.grdTradingAccounts.FlatMode = true;
			this.grdTradingAccounts.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdTradingAccounts.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdTradingAccounts.Location = new System.Drawing.Point(6, 14);
			this.grdTradingAccounts.Name = "grdTradingAccounts";
			this.grdTradingAccounts.ReadOnly = true;
			this.grdTradingAccounts.Size = new System.Drawing.Size(558, 74);
			this.grdTradingAccounts.TabIndex = 18;
			this.grdTradingAccounts.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										   this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.grdTradingAccounts;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.dataGridTextBoxColumn1,
																												  this.dataGridTextBoxColumn2,
																												  this.dataGridTextBoxColumn3,
																												  this.dataGridTextBoxColumn4});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "tradingAccounts";
			// 
			// dataGridTextBoxColumn1
			// 
			this.dataGridTextBoxColumn1.Format = "";
			this.dataGridTextBoxColumn1.FormatInfo = null;
			this.dataGridTextBoxColumn1.HeaderText = "Trading AccountsID";
			this.dataGridTextBoxColumn1.MappingName = "TradingAccountsID";
			this.dataGridTextBoxColumn1.Width = 0;
			// 
			// dataGridTextBoxColumn2
			// 
			this.dataGridTextBoxColumn2.Format = "";
			this.dataGridTextBoxColumn2.FormatInfo = null;
			this.dataGridTextBoxColumn2.HeaderText = "Name";
			this.dataGridTextBoxColumn2.MappingName = "TradingAccountName";
			this.dataGridTextBoxColumn2.Width = 150;
			// 
			// dataGridTextBoxColumn3
			// 
			this.dataGridTextBoxColumn3.Format = "";
			this.dataGridTextBoxColumn3.FormatInfo = null;
			this.dataGridTextBoxColumn3.HeaderText = "Short Name";
			this.dataGridTextBoxColumn3.MappingName = "TradingShortName";
			this.dataGridTextBoxColumn3.Width = 150;
			// 
			// dataGridTextBoxColumn4
			// 
			this.dataGridTextBoxColumn4.Format = "";
			this.dataGridTextBoxColumn4.FormatInfo = null;
			this.dataGridTextBoxColumn4.HeaderText = "Company ID";
			this.dataGridTextBoxColumn4.MappingName = "CompanyID";
			this.dataGridTextBoxColumn4.Width = 0;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.Enabled = false;
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(490, 94);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 24;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.Enabled = false;
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(408, 94);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 23;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnCreate
			// 
			this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnCreate.Enabled = false;
			this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnCreate.Location = new System.Drawing.Point(326, 94);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 26;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// GridTradingAccounts
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "GridTradingAccounts";
			this.Size = new System.Drawing.Size(572, 118);
			this.Load += new System.EventHandler(this.GridTradingAccounts_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdTradingAccounts)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdTradingAccounts;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnCreate;

		
		public Nirvana.Admin.BLL.TradingAccounts CurrentTradingAccounts
		{
			get 
			{
				return (Nirvana.Admin.BLL.TradingAccounts) grdTradingAccounts.DataSource; 
			}						
		}
		
		private CreateCompanyTradingAccounts createCompanyTradingAccounts = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(createCompanyTradingAccounts == null)
			{
				createCompanyTradingAccounts = new CreateCompanyTradingAccounts();				
			}
			TradingAccounts tradingAccounts = new TradingAccounts();
			createCompanyTradingAccounts.CurrentCompanyTradingAccounts = (TradingAccounts)grdTradingAccounts.DataSource;
			createCompanyTradingAccounts.ShowDialog(this.Parent);
			grdTradingAccounts.DataSource = null;
			grdTradingAccounts.Refresh();
			tradingAccounts = createCompanyTradingAccounts.CurrentCompanyTradingAccounts;
			//grdTradingAccounts.DataSource = createCompanyTradingAccounts.CurrentCompanyTradingAccounts;
			grdTradingAccounts.DataSource = tradingAccounts;

			if(createCompanyTradingAccounts.CurrentCompanyTradingAccounts.Count > 0 )
			{
				grdTradingAccounts.Select(0);
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

		private int _tradingAccountID = int.MinValue;
		public int TradingAccountID
		{
			get{return _tradingAccountID;}
			set
			{
				_tradingAccountID = value;
				BindDataGrid();
			}
		}

		public TradingAccount TradingAccountProperty
		{
			get 
			{
				TradingAccount tradingAccount = new TradingAccount();
				GetTradingAccount(tradingAccount);
				return tradingAccount; 
			}
			set 
			{
				SetTradingAccount(value);
					
			}
		}

		public void GetTradingAccount(TradingAccount tradingAccount)
		{
			tradingAccount = (TradingAccount)grdTradingAccounts.DataSource;						
		}
		
		public void SetTradingAccount(TradingAccount tradingAccount)
		{
			grdTradingAccounts.DataSource = tradingAccount;
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.TradingAccounts tradingAccounts = CompanyManager.GetTradingAccount(_companyID);
				grdTradingAccounts.DataSource = tradingAccounts;
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
			if (grdTradingAccounts.VisibleRowCount >0)
			{
				if(MessageBox.Show(this, "Do you want to delete this TradingAccount?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					int tradingAccountID = int.Parse(grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 0].ToString());
				
					Nirvana.Admin.BLL.TradingAccounts tradingAccounts = (TradingAccounts)grdTradingAccounts.DataSource;
					Nirvana.Admin.BLL.TradingAccount tradingAccount = new Nirvana.Admin.BLL.TradingAccount();

					tradingAccount.TradingAccountsID = int.Parse(grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 0].ToString());
					tradingAccount.TradingAccountName = grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 1].ToString();
					tradingAccount.TradingShortName = grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 2].ToString();
					tradingAccount.CompanyID = int.Parse(grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 3].ToString());
				
					tradingAccounts.RemoveAt(tradingAccounts.IndexOf(tradingAccount));
					if(tradingAccountID != int.MinValue)
					{
						CompanyManager.DeleteTradingAccount(tradingAccountID);	
					}
					grdTradingAccounts.DataSource = null;
					grdTradingAccounts.DataSource = tradingAccounts;
					grdTradingAccounts.Refresh();				
				}
			}
		
		}

		private CreateCompanyTradingAccounts createCompanyTradingAccountsEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(createCompanyTradingAccountsEdit == null)
			{
				if(grdTradingAccounts.VisibleRowCount > 0)
				{
					createCompanyTradingAccountsEdit = new CreateCompanyTradingAccounts();
//					Nirvana.Admin.BLL.TradingAccount tradingAccountEdit = new Nirvana.Admin.BLL.TradingAccount();
//					tradingAccountEdit.TradingAccountsID = int.Parse(grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 1].ToString());
//					tradingAccountEdit.TradingAccountName = grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 0].ToString();
//					tradingAccountEdit.TradingShortName = grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 3].ToString();
//					tradingAccountEdit.CompanyID = int.Parse(grdTradingAccounts[grdTradingAccounts.CurrentCell.RowNumber, 2].ToString());
//					createCompanyTradingAccountsEdit.TradingAccountEdit =  tradingAccountEdit;
					
					createCompanyTradingAccountsEdit.TradingAccountEdit = (TradingAccount)((TradingAccounts)grdTradingAccounts.DataSource)[grdTradingAccounts.CurrentCell.RowNumber];

					createCompanyTradingAccountsEdit.CurrentCompanyTradingAccounts = (Nirvana.Admin.BLL.TradingAccounts) grdTradingAccounts.DataSource;	
					createCompanyTradingAccountsEdit.ShowDialog(this.Parent);
					createCompanyTradingAccountsEdit = null;
				}
			}		
		}

		private void GridTradingAccounts_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
