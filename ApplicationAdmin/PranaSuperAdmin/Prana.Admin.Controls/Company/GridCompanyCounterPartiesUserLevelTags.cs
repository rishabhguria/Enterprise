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
	/// Summary description for GridCompanyCounterPartiesUserLevelTags.
	/// </summary>
	public class GridCompanyCounterPartiesUserLevelTags : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "GridCompanyCounterPartiesUserLevelTags : ";
		private System.Windows.Forms.DataGrid grdUserLevelTags;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueDetailsID;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyID;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueID;
		private System.Windows.Forms.DataGridTextBoxColumn colUserID;
		private System.Windows.Forms.DataGridTextBoxColumn colClearingFirmPrimeBrokerID;
		private System.Windows.Forms.DataGridTextBoxColumn colDeliverToCompanyID;
		private System.Windows.Forms.DataGridTextBoxColumn colDeiverToSubID;
		private System.Windows.Forms.DataGridTextBoxColumn colSenderCompanyID;
		private System.Windows.Forms.DataGridTextBoxColumn colFundID;
		private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUp;
		private System.Windows.Forms.DataGridTextBoxColumn colOnBehalfOfSubID;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueTagID;
		private System.Windows.Forms.DataGridTextBoxColumn colOnBehalfOfCompID;
		//private Nirvana.Admin.Controls.CreateCompanyCounterPartiesUserLevelTags uctCreateCompanyCounterPartiesUserLevelTags;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueName;
		private System.Windows.Forms.DataGridTextBoxColumn colClearingFirmPrimeBroker;
		private System.Windows.Forms.DataGridTextBoxColumn colFundName;
		private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUPName;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyUserName;
		private System.Windows.Forms.DataGridTextBoxColumn FullName;
		private System.Windows.Forms.DataGridTextBoxColumn colStrategyID;
		private System.Windows.Forms.DataGridTextBoxColumn colStrategyName;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridCompanyCounterPartiesUserLevelTags()
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
			this.grdUserLevelTags = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.FullName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueDetailsID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyUserName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colUserID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colClearingFirmPrimeBrokerID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colDeliverToCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colDeiverToSubID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colSenderCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colFundID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colOnBehalfOfCompID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCMTAGiveUp = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colOnBehalfOfSubID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCMTAGiveUPName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueTagID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colClearingFirmPrimeBroker = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colFundName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colStrategyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colStrategyName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			//this.uctCreateCompanyCounterPartiesUserLevelTags = new Nirvana.Admin.Controls.CreateCompanyCounterPartiesUserLevelTags();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.grdUserLevelTags)).BeginInit();
			this.SuspendLayout();
			// 
			// grdUserLevelTags
			// 
			this.grdUserLevelTags.CaptionVisible = false;
			this.grdUserLevelTags.DataMember = "";
			this.grdUserLevelTags.FlatMode = true;
			this.grdUserLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdUserLevelTags.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdUserLevelTags.Location = new System.Drawing.Point(4, 194);
			this.grdUserLevelTags.Name = "grdUserLevelTags";
			this.grdUserLevelTags.ReadOnly = true;
			this.grdUserLevelTags.Size = new System.Drawing.Size(566, 78);
			this.grdUserLevelTags.TabIndex = 40;
			this.grdUserLevelTags.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										 this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.grdUserLevelTags;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.FullName,
																												  this.colCompanyCounterPartyVenueName,
																												  this.colCompanyCounterPartyVenueDetailsID,
																												  this.colCompanyCounterPartyID,
																												  this.colCompanyCounterPartyVenueID,
																												  this.colCompanyUserName,
																												  this.colUserID,
																												  this.colClearingFirmPrimeBrokerID,
																												  this.colDeliverToCompanyID,
																												  this.colDeiverToSubID,
																												  this.colSenderCompanyID,
																												  this.colFundID,
																												  this.colOnBehalfOfCompID,
																												  this.colCMTAGiveUp,
																												  this.colOnBehalfOfSubID,
																												  this.colCMTAGiveUPName,
																												  this.colCompanyCounterPartyVenueTagID,
																												  this.colClearingFirmPrimeBroker,
																												  this.colFundName,
																												  this.colStrategyID,
																												  this.colStrategyName});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "companyCounterPartyVenueDetails";
			// 
			// FullName
			// 
			this.FullName.Format = "";
			this.FullName.FormatInfo = null;
			this.FullName.HeaderText = "CounterParty FullName";
			this.FullName.MappingName = "CounterPartyFullName";
			this.FullName.Width = 90;
			// 
			// colCompanyCounterPartyVenueName
			// 
			this.colCompanyCounterPartyVenueName.Format = "";
			this.colCompanyCounterPartyVenueName.FormatInfo = null;
			this.colCompanyCounterPartyVenueName.HeaderText = "Venue Name";
			this.colCompanyCounterPartyVenueName.MappingName = "CompanyCounterPartyVenueName";
			this.colCompanyCounterPartyVenueName.Width = 90;
			// 
			// colCompanyCounterPartyVenueDetailsID
			// 
			this.colCompanyCounterPartyVenueDetailsID.Format = "";
			this.colCompanyCounterPartyVenueDetailsID.FormatInfo = null;
			this.colCompanyCounterPartyVenueDetailsID.MappingName = "CompanyCounterPartyVenueDetailsID";
			this.colCompanyCounterPartyVenueDetailsID.Width = 0;
			// 
			// colCompanyCounterPartyID
			// 
			this.colCompanyCounterPartyID.Format = "";
			this.colCompanyCounterPartyID.FormatInfo = null;
			this.colCompanyCounterPartyID.HeaderText = "Company Counter Party ID";
			this.colCompanyCounterPartyID.MappingName = "CompanyCounterPartyID";
			this.colCompanyCounterPartyID.Width = 0;
			// 
			// colCompanyCounterPartyVenueID
			// 
			this.colCompanyCounterPartyVenueID.Format = "";
			this.colCompanyCounterPartyVenueID.FormatInfo = null;
			this.colCompanyCounterPartyVenueID.HeaderText = "Company Counter Party Venue ID";
			this.colCompanyCounterPartyVenueID.MappingName = "CompanyCounterPartyVenueID";
			this.colCompanyCounterPartyVenueID.Width = 0;
			// 
			// colCompanyUserName
			// 
			this.colCompanyUserName.Format = "";
			this.colCompanyUserName.FormatInfo = null;
			this.colCompanyUserName.HeaderText = "UserName";
			this.colCompanyUserName.MappingName = "CompanyUserName";
			this.colCompanyUserName.Width = 90;
			// 
			// colUserID
			// 
			this.colUserID.Format = "";
			this.colUserID.FormatInfo = null;
			this.colUserID.HeaderText = "User ID";
			this.colUserID.MappingName = "UserID";
			this.colUserID.Width = 0;
			// 
			// colClearingFirmPrimeBrokerID
			// 
			this.colClearingFirmPrimeBrokerID.Format = "";
			this.colClearingFirmPrimeBrokerID.FormatInfo = null;
			this.colClearingFirmPrimeBrokerID.MappingName = "ClearingFirmPrimeBrokerID";
			this.colClearingFirmPrimeBrokerID.Width = 0;
			// 
			// colDeliverToCompanyID
			// 
			this.colDeliverToCompanyID.Format = "";
			this.colDeliverToCompanyID.FormatInfo = null;
			this.colDeliverToCompanyID.MappingName = "DeliverToCompanyID";
			this.colDeliverToCompanyID.Width = 0;
			// 
			// colDeiverToSubID
			// 
			this.colDeiverToSubID.Format = "";
			this.colDeiverToSubID.FormatInfo = null;
			this.colDeiverToSubID.MappingName = "DeiverToSubID";
			this.colDeiverToSubID.Width = 0;
			// 
			// colSenderCompanyID
			// 
			this.colSenderCompanyID.Format = "";
			this.colSenderCompanyID.FormatInfo = null;
			this.colSenderCompanyID.MappingName = "SenderCompanyID";
			this.colSenderCompanyID.Width = 0;
			// 
			// colFundID
			// 
			this.colFundID.Format = "";
			this.colFundID.FormatInfo = null;
			this.colFundID.MappingName = "FundID";
			this.colFundID.Width = 0;
			// 
			// colOnBehalfOfCompID
			// 
			this.colOnBehalfOfCompID.Format = "";
			this.colOnBehalfOfCompID.FormatInfo = null;
			this.colOnBehalfOfCompID.MappingName = "OnBehalfOfCompID";
			this.colOnBehalfOfCompID.Width = 0;
			// 
			// colCMTAGiveUp
			// 
			this.colCMTAGiveUp.Format = "";
			this.colCMTAGiveUp.FormatInfo = null;
			this.colCMTAGiveUp.HeaderText = "CMTA/GiveUp";
			this.colCMTAGiveUp.MappingName = "CMTAGiveUp";
			this.colCMTAGiveUp.Width = 0;
			// 
			// colOnBehalfOfSubID
			// 
			this.colOnBehalfOfSubID.Format = "";
			this.colOnBehalfOfSubID.FormatInfo = null;
			this.colOnBehalfOfSubID.HeaderText = "On Behalf Of SubID";
			this.colOnBehalfOfSubID.MappingName = "OnBehalfOfSubID";
			this.colOnBehalfOfSubID.Width = 90;
			// 
			// colCMTAGiveUPName
			// 
			this.colCMTAGiveUPName.Format = "";
			this.colCMTAGiveUPName.FormatInfo = null;
			this.colCMTAGiveUPName.HeaderText = "CMTA/GiveUP";
			this.colCMTAGiveUPName.MappingName = "CMTAGiveUPName";
			this.colCMTAGiveUPName.Width = 90;
			// 
			// colCompanyCounterPartyVenueTagID
			// 
			this.colCompanyCounterPartyVenueTagID.Format = "";
			this.colCompanyCounterPartyVenueTagID.FormatInfo = null;
			this.colCompanyCounterPartyVenueTagID.MappingName = "CompanyCounterPartyVenueTagID";
			this.colCompanyCounterPartyVenueTagID.Width = 0;
			// 
			// colClearingFirmPrimeBroker
			// 
			this.colClearingFirmPrimeBroker.Format = "";
			this.colClearingFirmPrimeBroker.FormatInfo = null;
			this.colClearingFirmPrimeBroker.HeaderText = "Clearing Firm Prime Broker";
			this.colClearingFirmPrimeBroker.MappingName = "ClearingFirmPrimeBroker";
			this.colClearingFirmPrimeBroker.Width = 0;
			// 
			// colFundName
			// 
			this.colFundName.Format = "";
			this.colFundName.FormatInfo = null;
			this.colFundName.HeaderText = "Fund Name";
			this.colFundName.MappingName = "FundName";
			this.colFundName.Width = 0;
			// 
			// colStrategyID
			// 
			this.colStrategyID.Format = "";
			this.colStrategyID.FormatInfo = null;
			this.colStrategyID.MappingName = "StrategyID";
			this.colStrategyID.Width = 0;
			// 
			// colStrategyName
			// 
			this.colStrategyName.Format = "";
			this.colStrategyName.FormatInfo = null;
			this.colStrategyName.MappingName = "StrategyName";
			this.colStrategyName.Width = 0;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(496, 276);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 42;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(414, 276);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 41;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// uctCreateCompanyCounterPartiesUserLevelTags
			// 
			this.uctCreateCompanyCounterPartiesUserLevelTags.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCreateCompanyCounterPartiesUserLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCreateCompanyCounterPartiesUserLevelTags.Location = new System.Drawing.Point(100, 4);
			this.uctCreateCompanyCounterPartiesUserLevelTags.Name = "uctCreateCompanyCounterPartiesUserLevelTags";
			this.uctCreateCompanyCounterPartiesUserLevelTags.Size = new System.Drawing.Size(396, 188);
			this.uctCreateCompanyCounterPartiesUserLevelTags.TabIndex = 44;
			this.uctCreateCompanyCounterPartiesUserLevelTags.SaveData += new System.EventHandler(this.uctCreateCompanyCounterPartiesUserLevelTags_SaveData);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(4, 280);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(310, 23);
			this.label1.TabIndex = 45;
			this.label1.Text = "- Please Click the Save button below to save the data";
			// 
			// GridCompanyCounterPartiesUserLevelTags
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.label1);
			this.Controls.Add(this.uctCreateCompanyCounterPartiesUserLevelTags);
			this.Controls.Add(this.grdUserLevelTags);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "GridCompanyCounterPartiesUserLevelTags";
			this.Size = new System.Drawing.Size(574, 306);
			((System.ComponentModel.ISupportInitialize)(this.grdUserLevelTags)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public void InitializeCompanyControl()
		{
			uctCreateCompanyCounterPartiesUserLevelTags.CompanyID = _companyID;
			uctCreateCompanyCounterPartiesUserLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
			//uctCreateCompanyCounterPartiesUserLevelTags.SetData();
		}
		

		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			uctCreateCompanyCounterPartiesUserLevelTags.CompanyID = _companyID;
			uctCreateCompanyCounterPartiesUserLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
			uctCreateCompanyCounterPartiesUserLevelTags.SetData();
			CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = new  CompanyCounterPartyVenueDetails();
			uctCreateCompanyCounterPartiesUserLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdUserLevelTags.DataSource;
		}

		private void uctCreateCompanyCounterPartiesUserLevelTags_SaveData(object companyCounterPartyVenueDetails, EventArgs e) 
		{
			grdUserLevelTags.DataSource = null;
			grdUserLevelTags.Refresh();
			companyCounterPartyVenueDetails = uctCreateCompanyCounterPartiesUserLevelTags.CurrentCompanyCounterPartyVenueDetails;

			grdUserLevelTags.DataSource = companyCounterPartyVenueDetails;

			if(uctCreateCompanyCounterPartiesUserLevelTags.CurrentCompanyCounterPartyVenueDetails.Count > 0 )
			{
				grdUserLevelTags.Select(0);
			}
			
			if(uctCreateCompanyCounterPartiesUserLevelTags.CurrentCompanyCounterPartyVenueDetails.Count > 0 )
			{
				grdUserLevelTags.Select(0);
				uctCreateCompanyCounterPartiesUserLevelTags.NoData = 0;
			}
			else
			{
				uctCreateCompanyCounterPartiesUserLevelTags.NoData = 1;
			}
			
			uctCreateCompanyCounterPartiesUserLevelTags.CurrentCompanyCounterPartyVenueDetails = null;
			uctCreateCompanyCounterPartiesUserLevelTags.CompanyCounterPartyVenueDetailEdit = null;	
		}
		
		public Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails CurrentCompanyCounterPartyVenueDetails
		{
			get 
			{
				return (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdUserLevelTags.DataSource; 
			}						
		}

		private int _companyID = int.MinValue;
		public int CompanyID
		{
			get{return _companyID;}
			set
			{
				_companyID = value;
				
			}
		}

		private int _companyCounterPartyID = int.MinValue;
		public int CompanyCounterPartyID
		{
			get{return _companyCounterPartyID;}
			set
			{
				_companyCounterPartyID = value;
				InitializeCompanyControl();
				BindDataGrid();
				uctCreateCompanyCounterPartiesUserLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdUserLevelTags.DataSource;
			}
		}

		private int _companyCounterPartyVenueDetailID = int.MinValue;
		public int CompanyCounterPartyVenueDetailID
		{
			get{return _companyCounterPartyVenueDetailID;}
			set
			{
				_companyCounterPartyVenueDetailID = value;
				BindDataGrid();
			}
		}

		private void BindDataGrid()
		{
			try
			{
				//Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = CompanyManager.GetCompanyCounterPartyVenueDetails(_companyCounterPartyID);
				Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = CompanyManager.GetUserLevelDetails(_companyCounterPartyID, 2);
				grdUserLevelTags.DataSource = companyCounterPartyVenueDetails;
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
		
		public CompanyCounterPartyVenueDetail CompanyCounterPartyVenueDetailProperty
		{
			get 
			{
				CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new  CompanyCounterPartyVenueDetail();
				GetCompanyCounterPartyVenueDetail(companyCounterPartyVenueDetail);
				return companyCounterPartyVenueDetail; 
			}
			set 
			{
				SetCompanyCounterPartyVenueDetail(value);
					
			}
		}

		public void GetCompanyCounterPartyVenueDetail(CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail)
		{
			companyCounterPartyVenueDetail = (CompanyCounterPartyVenueDetail)grdUserLevelTags.DataSource;						
		}
		
		public void SetCompanyCounterPartyVenueDetail(CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail)
		{
			grdUserLevelTags.DataSource = companyCounterPartyVenueDetail;
		}
		
		//private CreateCompanyCounterPartiesUserLevelTags createCompanyCounterPartiesUserLevelTagsEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			//if(createCompanyCounterPartiesUserLevelTagsEdit == null)
			//{
				if(grdUserLevelTags.VisibleRowCount > 0)
				{
					//					createCompanyCounterPartiesUserLevelTagsEdit = new  CreateCompanyCounterPartiesUserLevelTags();
					uctCreateCompanyCounterPartiesUserLevelTags.CompanyCounterPartyVenueDetailEdit = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail)((Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdUserLevelTags.DataSource)[grdUserLevelTags.CurrentCell.RowNumber];

					uctCreateCompanyCounterPartiesUserLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdUserLevelTags.DataSource;	
					//					createCompanyCounterPartiesUserLevelTagsEdit.ShowDialog(this.Parent);
					//					uctCreateCompanyCounterPartiesUserLevelTags = null;
				}
			//}		
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(grdUserLevelTags.VisibleRowCount > 0)
			{
				if(MessageBox.Show(this, "Do you want to delete this User Level Tag?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					int companyCounterPartyVenueDetailsID = int.Parse(grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 2].ToString());
				
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = (CompanyCounterPartyVenueDetails)grdUserLevelTags.DataSource;
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail();					
				
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID = int.Parse(grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 2].ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 3].ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 4].ToString());
					companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 7].ToString());
					companyCounterPartyVenueDetail.DeliverToCompanyID = grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 8].ToString();
					companyCounterPartyVenueDetail.SenderCompanyID = grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 10].ToString();
					companyCounterPartyVenueDetail.FundID = int.Parse(grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 11].ToString());
					companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 13].ToString());
					companyCounterPartyVenueDetail.OnBehalfOfSubID = grdUserLevelTags[grdUserLevelTags.CurrentCell.RowNumber, 14].ToString();

					companyCounterPartyVenueDetails.RemoveAt(companyCounterPartyVenueDetails.IndexOf(companyCounterPartyVenueDetail));
				
					if(companyCounterPartyVenueDetailsID != int.MinValue)
					{
						CompanyManager.DeleteCompanyCounterPartyVenueDetail(companyCounterPartyVenueDetailsID);	
					}				
					
					CompanyCounterPartyVenueDetails newCompanyCounterPartyVenueDetails = new CompanyCounterPartyVenueDetails();
					foreach(CompanyCounterPartyVenueDetail tempCompanyCounterPartyVenueDetail in companyCounterPartyVenueDetails)
					{
						newCompanyCounterPartyVenueDetails.Add(tempCompanyCounterPartyVenueDetail);
					}					
					grdUserLevelTags.DataSource = newCompanyCounterPartyVenueDetails;
					
					if (companyCounterPartyVenueDetails.Count <= 0)
					{
						uctCreateCompanyCounterPartiesUserLevelTags.NoData = 1;
					}
				}
			}
		}
	}
}
