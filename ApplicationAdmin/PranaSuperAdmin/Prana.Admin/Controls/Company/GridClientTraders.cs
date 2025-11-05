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
	/// Summary description for GridClientTraders.
	/// </summary>
	public class GridClientTraders : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "GridClientTraders : ";
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdClientTraders;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridClientTraders()
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
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.grdClientTraders = new System.Windows.Forms.DataGrid();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdClientTraders)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(426, 98);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 33;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(590, 98);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 32;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(508, 98);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 31;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.grdClientTraders);
			this.groupBox1.Location = new System.Drawing.Point(-7, -2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(678, 98);
			this.groupBox1.TabIndex = 30;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Traders";
			// 
			// grdClientTraders
			// 
			this.grdClientTraders.CaptionVisible = false;
			this.grdClientTraders.DataMember = "";
			this.grdClientTraders.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdClientTraders.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdClientTraders.Location = new System.Drawing.Point(8, 14);
			this.grdClientTraders.Name = "grdClientTraders";
			this.grdClientTraders.ReadOnly = true;
			this.grdClientTraders.Size = new System.Drawing.Size(662, 78);
			this.grdClientTraders.TabIndex = 16;
			// 
			// GridClientTraders
			// 
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.groupBox1);
			this.Name = "GridClientTraders";
			this.Size = new System.Drawing.Size(664, 122);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdClientTraders)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private CreateClientTrader createClientTrader = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(createClientTrader == null)
			{
				createClientTrader = new CreateClientTrader();				
			}
			createClientTrader.ShowDialog(this.Parent);
			grdClientTraders.DataSource = null;
			grdClientTraders.Refresh();
			grdClientTraders.DataSource = createClientTrader.CurrentTraders; 

			if(createClientTrader.CurrentTraders.Count > 0 )
			{
				grdClientTraders.Select(0);
			}
		}

		public  Traders CurrentTraders
		{
			get 
			{
				return (Traders) grdClientTraders.DataSource; 
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

		private int _clientTraderID = int.MinValue;
		public int ClientTraderID
		{
			get{return _clientTraderID;}
			set
			{
				_clientTraderID = value;
				BindDataGrid();
			}
		}

		public Trader TraderProperty
		{
			get 
			{
				Trader trader = new Trader();
				GetTrader(trader);
				return trader; 
			}
			set 
			{
				SetTrader(value);
					
			}
		}

		public void GetTrader(Trader trader)
		{
			trader = (Trader)grdClientTraders.DataSource;						
		}
		
		public void SetTrader(Trader trader)
		{
			grdClientTraders.DataSource = trader;
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.Traders traders = TraderManager.GetTraders(_companyID);
				grdClientTraders.DataSource = traders;
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
			if(MessageBox.Show(this, "Do you want to delete this Trader?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				int traderID = int.Parse(grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 8].ToString());
				if(traderID != int.MinValue)
				{
					//CompanyManager.de .DeleteSymbolMapping(symbolMappingID);	
					//BindDataGrid();
				}
				else
				{
					Nirvana.Admin.BLL.Traders traders = (Traders)grdClientTraders.DataSource;
					Nirvana.Admin.BLL.Trader trader = new Nirvana.Admin.BLL.Trader();
					
					trader.TraderID = int.Parse(grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 8].ToString());
					trader.FirstName = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 2].ToString();
					trader.LastName = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 0].ToString();
					trader.ShortName = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 6].ToString();
					trader.Title = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 7].ToString();
					trader.EMail = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 1].ToString();
					trader.TelephoneWork = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 5].ToString();
					trader.TelephoneCell = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 3].ToString();
					trader.Pager = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 10].ToString();
					trader.TelephoneHome = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 9].ToString();
					trader.Fax = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 4].ToString();
					trader.CompanyID = int.Parse(grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 11].ToString());
					
					traders.RemoveAt(traders.IndexOf(trader));
					grdClientTraders.DataSource = null;
					grdClientTraders.DataSource = traders;
					grdClientTraders.Refresh();
				}
		
			}
		}

		private CreateClientTrader createClientTraderEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(createClientTraderEdit == null)
			{
				if(grdClientTraders.VisibleRowCount > 0)
				{
					createClientTraderEdit = new CreateClientTrader();
					createClientTraderEdit.TraderEdit = (Trader)((Nirvana.Admin.BLL.Traders) grdClientTraders.DataSource)[grdClientTraders.CurrentCell.RowNumber];
					
					createClientTraderEdit.CurrentTraders = (Nirvana.Admin.BLL.Traders) grdClientTraders.DataSource;	
					createClientTraderEdit.ShowDialog(this.Parent);
					createClientTraderEdit = null;
				}
			}
		}
	}
}
