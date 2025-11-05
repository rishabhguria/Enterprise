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
	/// Summary description for cc.
	/// </summary>
	public class GridCompanyCounterPartiesFundLevelTags : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "GridCompanyCounterPartiesFundLevelTags : ";
		private System.Windows.Forms.DataGrid grdFundLevelTags;
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
		private System.Windows.Forms.DataGridTextBoxColumn colOnBehalfOfCompID;
		private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUp;
		private System.Windows.Forms.DataGridTextBoxColumn colOnBehalfOfSubID;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueTagID;
		private Nirvana.Admin.Controls.CreateCompanyCounterPartyFundLevelTags uctCreateCompanyCounterPartyFundLevelTags;
		private System.Windows.Forms.DataGridTextBoxColumn colCounterPartyFullName;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueName;
		private System.Windows.Forms.DataGridTextBoxColumn colClearingFirmPrimeBroker;
		private System.Windows.Forms.DataGridTextBoxColumn colFundName;
		private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUPName;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyUserName;
		private System.Windows.Forms.DataGridTextBoxColumn colStrategyID;
		private System.Windows.Forms.DataGridTextBoxColumn colStrategyName;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridCompanyCounterPartiesFundLevelTags()
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
			this.grdFundLevelTags = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.colCounterPartyFullName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueDetailsID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colFundName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colStrategyName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colUserID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colClearingFirmPrimeBrokerID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colDeliverToCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colDeiverToSubID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colSenderCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colFundID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colOnBehalfOfCompID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colOnBehalfOfSubID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueTagID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCMTAGiveUp = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colClearingFirmPrimeBroker = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCMTAGiveUPName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyUserName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colStrategyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.uctCreateCompanyCounterPartyFundLevelTags = new Nirvana.Admin.Controls.CreateCompanyCounterPartyFundLevelTags();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.grdFundLevelTags)).BeginInit();
			this.SuspendLayout();
			// 
			// grdFundLevelTags
			// 
			this.grdFundLevelTags.CaptionVisible = false;
			this.grdFundLevelTags.DataMember = "";
			this.grdFundLevelTags.FlatMode = true;
			this.grdFundLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdFundLevelTags.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdFundLevelTags.Location = new System.Drawing.Point(4, 220);
			this.grdFundLevelTags.Name = "grdFundLevelTags";
			this.grdFundLevelTags.ReadOnly = true;
			this.grdFundLevelTags.Size = new System.Drawing.Size(566, 78);
			this.grdFundLevelTags.TabIndex = 36;
			this.grdFundLevelTags.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										 this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.grdFundLevelTags;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.colCounterPartyFullName,
																												  this.colCompanyCounterPartyVenueName,
																												  this.colCompanyCounterPartyVenueDetailsID,
																												  this.colCompanyCounterPartyID,
																												  this.colCompanyCounterPartyVenueID,
																												  this.colFundName,
																												  this.colStrategyName,
																												  this.colUserID,
																												  this.colClearingFirmPrimeBrokerID,
																												  this.colDeliverToCompanyID,
																												  this.colDeiverToSubID,
																												  this.colSenderCompanyID,
																												  this.colFundID,
																												  this.colOnBehalfOfCompID,
																												  this.colOnBehalfOfSubID,
																												  this.colCompanyCounterPartyVenueTagID,
																												  this.colCMTAGiveUp,
																												  this.colClearingFirmPrimeBroker,
																												  this.colCMTAGiveUPName,
																												  this.colCompanyUserName,
																												  this.colStrategyID});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "companyCounterPartyVenueDetails";
			// 
			// colCounterPartyFullName
			// 
			this.colCounterPartyFullName.Format = "";
			this.colCounterPartyFullName.FormatInfo = null;
			this.colCounterPartyFullName.HeaderText = "Full Name";
			this.colCounterPartyFullName.MappingName = "CounterPartyFullName";
			this.colCounterPartyFullName.Width = 90;
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
			// colFundName
			// 
			this.colFundName.Format = "";
			this.colFundName.FormatInfo = null;
			this.colFundName.HeaderText = "Fund Name";
			this.colFundName.MappingName = "FundName";
			this.colFundName.Width = 90;
			// 
			// colStrategyName
			// 
			this.colStrategyName.Format = "";
			this.colStrategyName.FormatInfo = null;
			this.colStrategyName.HeaderText = "Strategy Name";
			this.colStrategyName.MappingName = "StrategyName";
			this.colStrategyName.Width = 90;
			// 
			// colUserID
			// 
			this.colUserID.Format = "";
			this.colUserID.FormatInfo = null;
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
			this.colFundID.HeaderText = "Fund ID";
			this.colFundID.MappingName = "FundID";
			this.colFundID.Width = 0;
			// 
			// colOnBehalfOfCompID
			// 
			this.colOnBehalfOfCompID.Format = "";
			this.colOnBehalfOfCompID.FormatInfo = null;
			this.colOnBehalfOfCompID.HeaderText = "On Behalf Of Comp ID";
			this.colOnBehalfOfCompID.MappingName = "OnBehalfOfCompID";
			this.colOnBehalfOfCompID.Width = 90;
			// 
			// colOnBehalfOfSubID
			// 
			this.colOnBehalfOfSubID.Format = "";
			this.colOnBehalfOfSubID.FormatInfo = null;
			this.colOnBehalfOfSubID.MappingName = "OnBehalfOfSubID";
			this.colOnBehalfOfSubID.Width = 0;
			// 
			// colCompanyCounterPartyVenueTagID
			// 
			this.colCompanyCounterPartyVenueTagID.Format = "";
			this.colCompanyCounterPartyVenueTagID.FormatInfo = null;
			this.colCompanyCounterPartyVenueTagID.MappingName = "CompanyCounterPartyVenueTagID";
			this.colCompanyCounterPartyVenueTagID.Width = 0;
			// 
			// colCMTAGiveUp
			// 
			this.colCMTAGiveUp.Format = "";
			this.colCMTAGiveUp.FormatInfo = null;
			this.colCMTAGiveUp.HeaderText = "CMTA/GiveUp";
			this.colCMTAGiveUp.MappingName = "CMTAGiveUp";
			this.colCMTAGiveUp.Width = 0;
			// 
			// colClearingFirmPrimeBroker
			// 
			this.colClearingFirmPrimeBroker.Format = "";
			this.colClearingFirmPrimeBroker.FormatInfo = null;
			this.colClearingFirmPrimeBroker.HeaderText = "Clearing Firm Prime Broker";
			this.colClearingFirmPrimeBroker.MappingName = "ClearingFirmPrimeBroker";
			this.colClearingFirmPrimeBroker.Width = 0;
			// 
			// colCMTAGiveUPName
			// 
			this.colCMTAGiveUPName.Format = "";
			this.colCMTAGiveUPName.FormatInfo = null;
			this.colCMTAGiveUPName.HeaderText = "CMTA/GiveUP";
			this.colCMTAGiveUPName.MappingName = "CMTAGiveUPName";
			this.colCMTAGiveUPName.Width = 90;
			// 
			// colCompanyUserName
			// 
			this.colCompanyUserName.Format = "";
			this.colCompanyUserName.FormatInfo = null;
			this.colCompanyUserName.HeaderText = "CompanyUser Name";
			this.colCompanyUserName.MappingName = "CompanyUserName";
			this.colCompanyUserName.Width = 0;
			// 
			// colStrategyID
			// 
			this.colStrategyID.Format = "";
			this.colStrategyID.FormatInfo = null;
			this.colStrategyID.MappingName = "StrategyID";
			this.colStrategyID.Width = 0;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(496, 302);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 38;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(416, 302);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 37;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// uctCreateCompanyCounterPartyFundLevelTags
			// 
			this.uctCreateCompanyCounterPartyFundLevelTags.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCreateCompanyCounterPartyFundLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCreateCompanyCounterPartyFundLevelTags.Location = new System.Drawing.Point(96, 6);
			this.uctCreateCompanyCounterPartyFundLevelTags.Name = "uctCreateCompanyCounterPartyFundLevelTags";
			this.uctCreateCompanyCounterPartyFundLevelTags.Size = new System.Drawing.Size(396, 212);
			this.uctCreateCompanyCounterPartyFundLevelTags.TabIndex = 40;
			this.uctCreateCompanyCounterPartyFundLevelTags.SaveData += new System.EventHandler(this.uctCreateCompanyCounterPartyFundLevelTags_SaveData);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(4, 306);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(310, 23);
			this.label1.TabIndex = 41;
			this.label1.Text = "- Please Click the Save button below to save the data";
			// 
			// GridCompanyCounterPartiesFundLevelTags
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.label1);
			this.Controls.Add(this.uctCreateCompanyCounterPartyFundLevelTags);
			this.Controls.Add(this.grdFundLevelTags);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "GridCompanyCounterPartiesFundLevelTags";
			this.Size = new System.Drawing.Size(574, 332);
			((System.ComponentModel.ISupportInitialize)(this.grdFundLevelTags)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public void InitializeCompanyControl()
		{
			uctCreateCompanyCounterPartyFundLevelTags.CompanyID = _companyID;
			uctCreateCompanyCounterPartyFundLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
			//uctCreateCompanyCounterPartyFundLevelTags.SetData();
		}
		

		private void btnCreate_Click(object sender, System.EventArgs e)
		{
		
			uctCreateCompanyCounterPartyFundLevelTags.CompanyID = _companyID;
			uctCreateCompanyCounterPartyFundLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
			uctCreateCompanyCounterPartyFundLevelTags.SetData();
			CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = new  CompanyCounterPartyVenueDetails();
			uctCreateCompanyCounterPartyFundLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdFundLevelTags.DataSource;

		}

		
		private void uctCreateCompanyCounterPartyFundLevelTags_SaveData(object companyCounterPartyVenueDetails, EventArgs e) 
		{
			grdFundLevelTags.DataSource = null;
			grdFundLevelTags.Refresh();
			companyCounterPartyVenueDetails = uctCreateCompanyCounterPartyFundLevelTags.CurrentCompanyCounterPartyVenueDetails;

			grdFundLevelTags.DataSource = companyCounterPartyVenueDetails;

			if(uctCreateCompanyCounterPartyFundLevelTags.CurrentCompanyCounterPartyVenueDetails.Count > 0 )
			{
				grdFundLevelTags.Select(0);
			}
			
			if(uctCreateCompanyCounterPartyFundLevelTags.CurrentCompanyCounterPartyVenueDetails.Count > 0 )
			{
				grdFundLevelTags.Select(0);
				uctCreateCompanyCounterPartyFundLevelTags.NoData = 0;
			}
			else
			{
				uctCreateCompanyCounterPartyFundLevelTags.NoData = 1;
			}

			uctCreateCompanyCounterPartyFundLevelTags.CurrentCompanyCounterPartyVenueDetails = null;
			uctCreateCompanyCounterPartyFundLevelTags.CompanyCounterPartyVenueDetailEdit = null;	
		}

		public Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails CurrentCompanyCounterPartyVenueDetails
		{
			get 
			{
				return (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdFundLevelTags.DataSource; 
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
				uctCreateCompanyCounterPartyFundLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdFundLevelTags.DataSource;
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
				Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = CompanyManager.GetFundLevelDetails(_companyCounterPartyID, 1);
				grdFundLevelTags.DataSource = companyCounterPartyVenueDetails;
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
			companyCounterPartyVenueDetail = (CompanyCounterPartyVenueDetail)grdFundLevelTags.DataSource;						
		}
		
		public void SetCompanyCounterPartyVenueDetail(CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail)
		{
			grdFundLevelTags.DataSource = companyCounterPartyVenueDetail;
		}

		private CreateCompanyCounterPartyFundLevelTags createCompanyCounterPartyFundLevelTagsEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(createCompanyCounterPartyFundLevelTagsEdit == null)
			{
				if(grdFundLevelTags.VisibleRowCount > 0)
				{
//					createCompanyCounterPartyFundLevelTagsEdit = new  CreateCompanyCounterPartyFundLevelTags();
//					createCompanyCounterPartyFundLevelTagsEdit.CompanyCounterPartyVenueDetailEdit = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail)((Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdFundLevelTags.DataSource)[grdFundLevelTags.CurrentCell.RowNumber];
//
//					createCompanyCounterPartyFundLevelTagsEdit.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdFundLevelTags.DataSource;	
//					//createCompanyCounterPartyFundLevelTagsEdit.ShowDialog(this.Parent);
//					createCompanyCounterPartyFundLevelTagsEdit = null;

					uctCreateCompanyCounterPartyFundLevelTags.CompanyCounterPartyVenueDetailEdit = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail)((Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdFundLevelTags.DataSource)[grdFundLevelTags.CurrentCell.RowNumber];
					uctCreateCompanyCounterPartyFundLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdFundLevelTags.DataSource;	
				}
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(grdFundLevelTags.VisibleRowCount > 0)
			{
				if(MessageBox.Show(this, "Do you want to delete this Fund Level Tag?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					int companyCounterPartyVenueDetailsID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 2].ToString());
				
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = (CompanyCounterPartyVenueDetails)grdFundLevelTags.DataSource;
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail();					
				
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 2].ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 3].ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 4].ToString());
					
					companyCounterPartyVenueDetail.UserID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 7].ToString());
					
					companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 8].ToString());
					companyCounterPartyVenueDetail.DeliverToCompanyID = grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 9].ToString();
					companyCounterPartyVenueDetail.DeiverToSubID = grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 10].ToString();
					companyCounterPartyVenueDetail.SenderCompanyID = grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 11].ToString();
					companyCounterPartyVenueDetail.FundID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 12].ToString());
					companyCounterPartyVenueDetail.OnBehalfOfCompID = grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 13].ToString();
					companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 16].ToString());
					companyCounterPartyVenueDetail.OnBehalfOfSubID = grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 14].ToString();
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueTagID = int.Parse(grdFundLevelTags[grdFundLevelTags.CurrentCell.RowNumber, 15].ToString());

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
					grdFundLevelTags.DataSource = newCompanyCounterPartyVenueDetails;

					if (companyCounterPartyVenueDetails.Count <= 0)
					{
						uctCreateCompanyCounterPartyFundLevelTags.NoData = 1;
					}
				}
			}
		}
	}
}
