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
	/// Summary description for GridClearingFirmsPrimeBrokers.
	/// </summary>
	public class GridClearingFirmsPrimeBrokers : System.Windows.Forms.UserControl
	{	
		private const string FORM_NAME = "GridClearingFirmsPrimeBrokers : ";
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridClearingFirmsPrimeBrokers()
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
			this.grdClearingFirmsPrimeBrokers = new System.Windows.Forms.DataGrid();
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdClearingFirmsPrimeBrokers)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.grdClearingFirmsPrimeBrokers);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(644, 90);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Clearing Firms/Prime Brokers";
			// 
			// grdClearingFirmsPrimeBrokers
			// 
			this.grdClearingFirmsPrimeBrokers.DataMember = "";
			this.grdClearingFirmsPrimeBrokers.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdClearingFirmsPrimeBrokers.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(1, 12);
			this.grdClearingFirmsPrimeBrokers.Name = "grdClearingFirmsPrimeBrokers";
			this.grdClearingFirmsPrimeBrokers.ReadOnly = true;
			this.grdClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(640, 78);
			this.grdClearingFirmsPrimeBrokers.TabIndex = 19;
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(410, 96);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 24;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(574, 96);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 26;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(492, 96);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 25;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// GridClearingFirmsPrimeBrokers
			// 
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.Name = "GridClearingFirmsPrimeBrokers";
			this.Size = new System.Drawing.Size(654, 116);
			this.Load += new System.EventHandler(this.GridClearingFirmsPrimeBrokers_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdClearingFirmsPrimeBrokers)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdClearingFirmsPrimeBrokers;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;

		private CreateCompanyClearingFirmsPrimeBrokers createCompanyClearingFirmsPrimeBrokers = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(createCompanyClearingFirmsPrimeBrokers == null)
			{
				createCompanyClearingFirmsPrimeBrokers = new CreateCompanyClearingFirmsPrimeBrokers();				
			}
			createCompanyClearingFirmsPrimeBrokers.ShowDialog(this.ParentForm);
			grdClearingFirmsPrimeBrokers.DataSource = null;
			grdClearingFirmsPrimeBrokers.Refresh();
			grdClearingFirmsPrimeBrokers.DataSource = createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers;

			if(createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers.Count > 0 )
			{
				grdClearingFirmsPrimeBrokers.Select(0);
			}
		}

		public Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers CurrentClearingFirmsPrimeBrokers
		{
			get 
			{
				return (Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers) grdClearingFirmsPrimeBrokers.DataSource; 
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

		private int _clearingFirmsPrimeBrokersID = int.MinValue;
		public int ClearingFirmsPrimeBrokersID
		{
			get{return _clearingFirmsPrimeBrokersID;}
			set
			{
				_clearingFirmsPrimeBrokersID = value;
				BindDataGrid();
			}
		}

		public ClearingFirmPrimeBroker ClearingFirmPrimeBrokerProperty
		{
			get 
			{
				ClearingFirmPrimeBroker clearingFirmPrimeBroker = new ClearingFirmPrimeBroker();
				GetClearingFirmPrimeBroker(clearingFirmPrimeBroker);
				return clearingFirmPrimeBroker; 
			}
			set 
			{
				SetClearingFirmPrimeBroker(value);
					
			}
		}

		public void GetClearingFirmPrimeBroker(ClearingFirmPrimeBroker clearingFirmPrimeBroker)
		{
			clearingFirmPrimeBroker = (ClearingFirmPrimeBroker)grdClearingFirmsPrimeBrokers.DataSource;						
		}
		
		public void SetClearingFirmPrimeBroker(ClearingFirmPrimeBroker clearingFirmPrimeBroker)
		{
			grdClearingFirmsPrimeBrokers.DataSource = clearingFirmPrimeBroker;
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = CompanyManager.GetClearingFirmPrimeBroker(_companyID);
				grdClearingFirmsPrimeBrokers.DataSource = clearingFirmsPrimeBrokers;
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
			if(MessageBox.Show(this, "Do you want to delete this ClearingFirmPrimeBroker?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				int clearingFirmPrimeBrokerID = int.Parse(grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 0].ToString());
				if(clearingFirmPrimeBrokerID != int.MinValue)
				{
					
				}
				else
				{
					Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = (ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource;
					Nirvana.Admin.BLL.ClearingFirmPrimeBroker clearingFirmPrimeBroker = new Nirvana.Admin.BLL.ClearingFirmPrimeBroker();
					
					
					clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID = int.Parse(grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 0].ToString());
					clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName = grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 1].ToString();
					clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName = grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 2].ToString();
					clearingFirmPrimeBroker.CompanyID = int.Parse(grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 3].ToString());
					
					clearingFirmsPrimeBrokers.RemoveAt(clearingFirmsPrimeBrokers.IndexOf(clearingFirmPrimeBroker));
					grdClearingFirmsPrimeBrokers.DataSource = null;
					grdClearingFirmsPrimeBrokers.DataSource = clearingFirmsPrimeBrokers;
					grdClearingFirmsPrimeBrokers.Refresh();
				}
		
			}
		}

		private CreateCompanyClearingFirmsPrimeBrokers createCompanyClearingFirmsPrimeBrokersEdit = null;

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(createCompanyClearingFirmsPrimeBrokersEdit == null)
			{
				if(grdClearingFirmsPrimeBrokers.VisibleRowCount > 0)
				{
					createCompanyClearingFirmsPrimeBrokersEdit = new CreateCompanyClearingFirmsPrimeBrokers();
//					Nirvana.Admin.BLL.ClearingFirmPrimeBroker clearingFirmPrimeBrokerEdit = new Nirvana.Admin.BLL.ClearingFirmPrimeBroker();
//					clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersID = int.Parse(grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 0].ToString());
//					clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersName = grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 1].ToString();
//					clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersShortName = grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 3].ToString();
//					clearingFirmPrimeBrokerEdit.CompanyID = int.Parse(grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 2].ToString());
//					createCompanyClearingFirmsPrimeBrokersEdit.ClearingFirmPrimeBrokerEdit =  clearingFirmPrimeBrokerEdit;

					createCompanyClearingFirmsPrimeBrokersEdit.ClearingFirmPrimeBrokerEdit = (ClearingFirmPrimeBroker)((Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource)[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber]; 

					createCompanyClearingFirmsPrimeBrokersEdit.CurrentClearingFirmsPrimeBrokers = (Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers) grdClearingFirmsPrimeBrokers.DataSource;	
					createCompanyClearingFirmsPrimeBrokersEdit.ShowDialog(this.Parent);
					createCompanyClearingFirmsPrimeBrokersEdit = null;
				}
			}		
		}

		
		private void GridClearingFirmsPrimeBrokers_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
