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
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTraderID;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridFirstName;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridLastName;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridShortName;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTitle;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridEMail;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTelephoneWork;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTelephoneCell;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridPager;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTelephoneHome;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridFax;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridCompanyID;
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
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTraderID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridFirstName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridLastName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridShortName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTitle = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridEMail = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTelephoneWork = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTelephoneCell = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridPager = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTelephoneHome = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridFax = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdClientTraders)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCreate
			// 
			this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnCreate.Location = new System.Drawing.Point(430, 98);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 33;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(594, 98);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 32;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(512, 98);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 31;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.grdClientTraders);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(2, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(672, 98);
			this.groupBox1.TabIndex = 30;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Traders";
			// 
			// grdClientTraders
			// 
			this.grdClientTraders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdClientTraders.CaptionVisible = false;
			this.grdClientTraders.DataMember = "";
			this.grdClientTraders.FlatMode = true;
			this.grdClientTraders.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdClientTraders.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdClientTraders.Location = new System.Drawing.Point(3, 14);
			this.grdClientTraders.Name = "grdClientTraders";
			this.grdClientTraders.ReadOnly = true;
			this.grdClientTraders.Size = new System.Drawing.Size(665, 80);
			this.grdClientTraders.TabIndex = 16;
			this.grdClientTraders.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										 this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.grdClientTraders;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.dataGridTraderID,
																												  this.dataGridFirstName,
																												  this.dataGridLastName,
																												  this.dataGridShortName,
																												  this.dataGridTitle,
																												  this.dataGridEMail,
																												  this.dataGridTelephoneWork,
																												  this.dataGridTelephoneCell,
																												  this.dataGridPager,
																												  this.dataGridTelephoneHome,
																												  this.dataGridFax,
																												  this.dataGridCompanyID});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "traders";
			// 
			// dataGridTraderID
			// 
			this.dataGridTraderID.Format = "";
			this.dataGridTraderID.FormatInfo = null;
			this.dataGridTraderID.MappingName = "TraderID";
			this.dataGridTraderID.Width = 0;
			// 
			// dataGridFirstName
			// 
			this.dataGridFirstName.Format = "";
			this.dataGridFirstName.FormatInfo = null;
			this.dataGridFirstName.HeaderText = "First Name";
			this.dataGridFirstName.MappingName = "FirstName";
			this.dataGridFirstName.Width = 60;
			// 
			// dataGridLastName
			// 
			this.dataGridLastName.Format = "";
			this.dataGridLastName.FormatInfo = null;
			this.dataGridLastName.HeaderText = "Last Name";
			this.dataGridLastName.MappingName = "LastName";
			this.dataGridLastName.Width = 60;
			// 
			// dataGridShortName
			// 
			this.dataGridShortName.Format = "";
			this.dataGridShortName.FormatInfo = null;
			this.dataGridShortName.HeaderText = "Short Name";
			this.dataGridShortName.MappingName = "ShortName";
			this.dataGridShortName.Width = 60;
			// 
			// dataGridTitle
			// 
			this.dataGridTitle.Format = "";
			this.dataGridTitle.FormatInfo = null;
			this.dataGridTitle.HeaderText = "Title";
			this.dataGridTitle.MappingName = "Title";
			this.dataGridTitle.Width = 60;
			// 
			// dataGridEMail
			// 
			this.dataGridEMail.Format = "";
			this.dataGridEMail.FormatInfo = null;
			this.dataGridEMail.HeaderText = "EMail";
			this.dataGridEMail.MappingName = "EMail";
			this.dataGridEMail.Width = 60;
			// 
			// dataGridTelephoneWork
			// 
			this.dataGridTelephoneWork.Format = "";
			this.dataGridTelephoneWork.FormatInfo = null;
			this.dataGridTelephoneWork.HeaderText = "Telephone Work";
			this.dataGridTelephoneWork.MappingName = "TelephoneWork";
			this.dataGridTelephoneWork.Width = 60;
			// 
			// dataGridTelephoneCell
			// 
			this.dataGridTelephoneCell.Format = "";
			this.dataGridTelephoneCell.FormatInfo = null;
			this.dataGridTelephoneCell.HeaderText = "Telephone Cell";
			this.dataGridTelephoneCell.MappingName = "TelephoneCell";
			this.dataGridTelephoneCell.Width = 60;
			// 
			// dataGridPager
			// 
			this.dataGridPager.Format = "";
			this.dataGridPager.FormatInfo = null;
			this.dataGridPager.HeaderText = "Pager";
			this.dataGridPager.MappingName = "Pager";
			this.dataGridPager.Width = 60;
			// 
			// dataGridTelephoneHome
			// 
			this.dataGridTelephoneHome.Format = "";
			this.dataGridTelephoneHome.FormatInfo = null;
			this.dataGridTelephoneHome.HeaderText = "Telephone Home";
			this.dataGridTelephoneHome.MappingName = "TelephoneHome";
			this.dataGridTelephoneHome.Width = 60;
			// 
			// dataGridFax
			// 
			this.dataGridFax.Format = "";
			this.dataGridFax.FormatInfo = null;
			this.dataGridFax.HeaderText = "Fax";
			this.dataGridFax.MappingName = "Fax";
			this.dataGridFax.Width = 60;
			// 
			// dataGridCompanyID
			// 
			this.dataGridCompanyID.Format = "";
			this.dataGridCompanyID.FormatInfo = null;
			this.dataGridCompanyID.MappingName = "CompanyID";
			this.dataGridCompanyID.Width = 0;
			// 
			// GridClientTraders
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "GridClientTraders";
			this.Size = new System.Drawing.Size(676, 122);
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
			Nirvana.Admin.BLL.Traders traders  = new Nirvana.Admin.BLL.Traders();
			createClientTrader.CurrentTraders = (Nirvana.Admin.BLL.Traders)grdClientTraders.DataSource;
			createClientTrader.ShowDialog(this.Parent);
			grdClientTraders.DataSource = null;
			grdClientTraders.Refresh();
			traders = createClientTrader.CurrentTraders;
			//grdClientTraders.DataSource = createClientTrader.CurrentTraders; 
			grdClientTraders.DataSource = traders;

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
				Nirvana.Admin.BLL.Traders traders = TraderManager.GetTraders(_companyClientID);
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
			if (grdClientTraders.VisibleRowCount >0)
			{
				if(MessageBox.Show(this, "Do you want to delete this Trader?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					int traderID = int.Parse(grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 0].ToString());
					Nirvana.Admin.BLL.Traders traders = (Traders)grdClientTraders.DataSource;
					Nirvana.Admin.BLL.Trader trader = new Nirvana.Admin.BLL.Trader();
				
					trader.TraderID = int.Parse(grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 0].ToString());
					trader.FirstName = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 1].ToString();
					trader.LastName = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 2].ToString();
					trader.ShortName = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 3].ToString();
					trader.Title = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 4].ToString();
					trader.EMail = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 5].ToString();
					trader.TelephoneWork = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 6].ToString();
					trader.TelephoneCell = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 7].ToString();
					trader.Pager = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 8].ToString();
					trader.TelephoneHome = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 9].ToString();
					trader.Fax = grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 10].ToString();
					trader.CompanyID = int.Parse(grdClientTraders[grdClientTraders.CurrentCell.RowNumber, 11].ToString());
				
					traders.RemoveAt(traders.IndexOf(trader));
					if(traderID != int.MinValue)
					{
						TraderManager.DeleteTrader(traderID);
					}
					Traders newTraders = new Traders();
					
					foreach(Trader tempTrader in traders)
					{
						newTraders.Add(tempTrader);
					}
					
					grdClientTraders.DataSource = newTraders;
				
		
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
