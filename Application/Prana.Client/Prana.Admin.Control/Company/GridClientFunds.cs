#region Using

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

#endregion

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for GridClientFunds.
	/// </summary>
	public class GridClientFunds : System.Windows.Forms.UserControl
	{
		#region Private members

		private const string FORM_NAME = "GridClientFunds : ";
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdClientFunds;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridCompanyClientFundID;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridCompanyClientFundName;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridCompanyClientFundShortName;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridCompanyClientID;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

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
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridCompanyClientFundID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridCompanyClientFundName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridCompanyClientFundShortName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridCompanyClientID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdClientFunds)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCreate
			// 
			this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnCreate.Location = new System.Drawing.Point(428, 98);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 29;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(592, 98);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 27;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(510, 98);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 26;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.grdClientFunds);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(0, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(670, 92);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Funds";
			// 
			// grdClientFunds
			// 
			this.grdClientFunds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdClientFunds.CaptionVisible = false;
			this.grdClientFunds.DataMember = "";
			this.grdClientFunds.FlatMode = true;
			this.grdClientFunds.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdClientFunds.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdClientFunds.Location = new System.Drawing.Point(4, 12);
			this.grdClientFunds.Name = "grdClientFunds";
			this.grdClientFunds.ReadOnly = true;
			this.grdClientFunds.Size = new System.Drawing.Size(665, 80);
			this.grdClientFunds.TabIndex = 16;
			this.grdClientFunds.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																									   this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.grdClientFunds;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.dataGridCompanyClientFundID,
																												  this.dataGridCompanyClientFundName,
																												  this.dataGridCompanyClientFundShortName,
																												  this.dataGridCompanyClientID});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "clientFunds";
			// 
			// dataGridCompanyClientFundID
			// 
			this.dataGridCompanyClientFundID.Format = "";
			this.dataGridCompanyClientFundID.FormatInfo = null;
			this.dataGridCompanyClientFundID.MappingName = "CompanyClientFundID";
			this.dataGridCompanyClientFundID.Width = 0;
			// 
			// dataGridCompanyClientFundName
			// 
			this.dataGridCompanyClientFundName.Format = "";
			this.dataGridCompanyClientFundName.FormatInfo = null;
			this.dataGridCompanyClientFundName.HeaderText = "Name";
			this.dataGridCompanyClientFundName.MappingName = "CompanyClientFundName";
			this.dataGridCompanyClientFundName.Width = 150;
			// 
			// dataGridCompanyClientFundShortName
			// 
			this.dataGridCompanyClientFundShortName.Format = "";
			this.dataGridCompanyClientFundShortName.FormatInfo = null;
			this.dataGridCompanyClientFundShortName.HeaderText = "Short Name";
			this.dataGridCompanyClientFundShortName.MappingName = "CompanyClientFundShortName";
			this.dataGridCompanyClientFundShortName.Width = 150;
			// 
			// dataGridCompanyClientID
			// 
			this.dataGridCompanyClientID.Format = "";
			this.dataGridCompanyClientID.FormatInfo = null;
			this.dataGridCompanyClientID.MappingName = "CompanyClientID";
			this.dataGridCompanyClientID.Width = 0;
			// 
			// GridClientFunds
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "GridClientFunds";
			this.Size = new System.Drawing.Size(676, 122);
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
			Nirvana.Admin.BLL.ClientFunds clientFunds = new Nirvana.Admin.BLL.ClientFunds();
			createCompanyClientFund.CurrentClientFunds = (Nirvana.Admin.BLL.ClientFunds)grdClientFunds.DataSource;
			createCompanyClientFund.ShowDialog(this.Parent);
			grdClientFunds.DataSource = null;
			grdClientFunds.Refresh();
			clientFunds = createCompanyClientFund.CurrentClientFunds;
			//grdClientFunds.DataSource = createCompanyClientFund.CurrentClientFunds; 
			grdClientFunds.DataSource = clientFunds; 

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

		private int _companyClientID = int.MinValue;
		public int CompanyClientID
		{
			get{return _companyClientID;}
			set
			{
				_companyClientID = value;
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
				Nirvana.Admin.BLL.ClientFunds clientFunds = ClientFundManager.GetCompanyClientFunds(_companyClientID);
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
			if (grdClientFunds.VisibleRowCount >0)
			{
				if(MessageBox.Show(this, "Do you want to delete this Client Fund?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					int clientFundID = int.Parse(grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 0].ToString());
			
					Nirvana.Admin.BLL.ClientFunds clientFunds = (Nirvana.Admin.BLL.ClientFunds)grdClientFunds.DataSource;
					Nirvana.Admin.BLL.ClientFund clientFund = new Nirvana.Admin.BLL.ClientFund();					
				
					clientFund.CompanyClientFundID = int.Parse(grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 0].ToString());
					clientFund.CompanyClientFundName = grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 1].ToString();
					clientFund.CompanyClientFundShortName = grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 2].ToString();
					clientFund.CompanyClientID = int.Parse(grdClientFunds[grdClientFunds.CurrentCell.RowNumber, 3].ToString());
				
					clientFunds.RemoveAt(clientFunds.IndexOf(clientFund));
					if(clientFundID != int.MinValue)
					{
						ClientFundManager.DeleteClientFund(clientFundID);
					}
					
					Nirvana.Admin.BLL.ClientFunds newClientFunds = new Nirvana.Admin.BLL.ClientFunds();
					foreach(Nirvana.Admin.BLL.ClientFund tempClientFund in clientFunds)
					{
						newClientFunds.Add(tempClientFund);
					}

					grdClientFunds.DataSource = newClientFunds;
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
					createCompanyClientFundEdit.ClientFundEdit = (Nirvana.Admin.BLL.ClientFund)((Nirvana.Admin.BLL.ClientFunds) grdClientFunds.DataSource)[grdClientFunds.CurrentCell.RowNumber];
					
					createCompanyClientFundEdit.CurrentClientFunds = (Nirvana.Admin.BLL.ClientFunds) grdClientFunds.DataSource;	
					createCompanyClientFundEdit.ShowDialog(this.Parent);
					createCompanyClientFundEdit = null;
				}
			}
		}
	}
}
