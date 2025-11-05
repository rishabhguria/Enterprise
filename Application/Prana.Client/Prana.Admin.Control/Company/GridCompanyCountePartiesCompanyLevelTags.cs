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
	/// Summary description for CompanyCountePartiesCompanyLevelTags.
	/// </summary>
	public class CompanyCountePartiesCompanyLevelTags : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "CompanyCountePartiesCompanyLevelTags : ";
		private System.Windows.Forms.DataGrid grdCompanyLevelTags;
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
		private Nirvana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags uctCreateCompanyCounterPartiesCompanyLevelTags;
		private System.Windows.Forms.DataGridTextBoxColumn colCounterPartyFullName;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueName;
		private System.Windows.Forms.DataGridTextBoxColumn colClearingFirmPrimeBroker;
		private System.Windows.Forms.DataGridTextBoxColumn colFundName;
		private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUPName;
		private System.Windows.Forms.DataGridTextBoxColumn colCompanyUserName;
		//private Nirvana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags createCompanyCounterPartiesCompanyLevelTags1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CompanyCountePartiesCompanyLevelTags()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			InitializeCompanyControl();
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
			this.grdCompanyLevelTags = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.colCounterPartyFullName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueDetailsID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colClearingFirmPrimeBroker = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colUserID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colClearingFirmPrimeBrokerID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colDeliverToCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colDeiverToSubID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colSenderCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colFundID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colOnBehalfOfCompID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCMTAGiveUp = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colOnBehalfOfSubID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyCounterPartyVenueTagID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colFundName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCMTAGiveUPName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colCompanyUserName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.uctCreateCompanyCounterPartiesCompanyLevelTags = new Nirvana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags();
			((System.ComponentModel.ISupportInitialize)(this.grdCompanyLevelTags)).BeginInit();
			this.SuspendLayout();
			// 
			// grdCompanyLevelTags
			// 
			this.grdCompanyLevelTags.CaptionVisible = false;
			this.grdCompanyLevelTags.DataMember = "";
			this.grdCompanyLevelTags.FlatMode = true;
			this.grdCompanyLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdCompanyLevelTags.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdCompanyLevelTags.Location = new System.Drawing.Point(10, 208);
			this.grdCompanyLevelTags.Name = "grdCompanyLevelTags";
			this.grdCompanyLevelTags.ReadOnly = true;
			this.grdCompanyLevelTags.Size = new System.Drawing.Size(640, 78);
			this.grdCompanyLevelTags.TabIndex = 32;
			this.grdCompanyLevelTags.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																											this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.grdCompanyLevelTags;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.colCounterPartyFullName,
																												  this.colCompanyCounterPartyVenueName,
																												  this.colCompanyCounterPartyVenueDetailsID,
																												  this.colCompanyCounterPartyID,
																												  this.colCompanyCounterPartyVenueID,
																												  this.colClearingFirmPrimeBroker,
																												  this.colUserID,
																												  this.colClearingFirmPrimeBrokerID,
																												  this.colDeliverToCompanyID,
																												  this.colDeiverToSubID,
																												  this.colSenderCompanyID,
																												  this.colFundID,
																												  this.colOnBehalfOfCompID,
																												  this.colCMTAGiveUp,
																												  this.colOnBehalfOfSubID,
																												  this.colCompanyCounterPartyVenueTagID,
																												  this.colFundName,
																												  this.colCMTAGiveUPName,
																												  this.colCompanyUserName});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "companyCounterPartyVenueDetails";
			// 
			// colCounterPartyFullName
			// 
			this.colCounterPartyFullName.Format = "";
			this.colCounterPartyFullName.FormatInfo = null;
			this.colCounterPartyFullName.HeaderText = "Full Name";
			this.colCounterPartyFullName.MappingName = "CounterPartyFullName";
			this.colCounterPartyFullName.Width = 75;
			// 
			// colCompanyCounterPartyVenueName
			// 
			this.colCompanyCounterPartyVenueName.Format = "";
			this.colCompanyCounterPartyVenueName.FormatInfo = null;
			this.colCompanyCounterPartyVenueName.HeaderText = "Venue Name";
			this.colCompanyCounterPartyVenueName.MappingName = "CompanyCounterPartyVenueName";
			this.colCompanyCounterPartyVenueName.Width = 75;
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
			// colClearingFirmPrimeBroker
			// 
			this.colClearingFirmPrimeBroker.Format = "";
			this.colClearingFirmPrimeBroker.FormatInfo = null;
			this.colClearingFirmPrimeBroker.HeaderText = "Clearing Firm Prime Broker";
			this.colClearingFirmPrimeBroker.MappingName = "ClearingFirmPrimeBroker";
			this.colClearingFirmPrimeBroker.Width = 75;
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
			this.colClearingFirmPrimeBrokerID.HeaderText = "Clearing Firm Prime Broker ID";
			this.colClearingFirmPrimeBrokerID.MappingName = "ClearingFirmPrimeBrokerID";
			this.colClearingFirmPrimeBrokerID.Width = 0;
			// 
			// colDeliverToCompanyID
			// 
			this.colDeliverToCompanyID.Format = "";
			this.colDeliverToCompanyID.FormatInfo = null;
			this.colDeliverToCompanyID.HeaderText = "Deliver To Company ID";
			this.colDeliverToCompanyID.MappingName = "DeliverToCompanyID";
			this.colDeliverToCompanyID.Width = 75;
			// 
			// colDeiverToSubID
			// 
			this.colDeiverToSubID.Format = "";
			this.colDeiverToSubID.FormatInfo = null;
			this.colDeiverToSubID.HeaderText = "Deiver To Sub ID";
			this.colDeiverToSubID.MappingName = "DeiverToSubID";
			this.colDeiverToSubID.Width = 75;
			// 
			// colSenderCompanyID
			// 
			this.colSenderCompanyID.Format = "";
			this.colSenderCompanyID.FormatInfo = null;
			this.colSenderCompanyID.HeaderText = "Sender Company ID";
			this.colSenderCompanyID.MappingName = "SenderCompanyID";
			this.colSenderCompanyID.Width = 75;
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
			this.colCMTAGiveUp.MappingName = "CMTAGiveUp";
			this.colCMTAGiveUp.Width = 0;
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
			// colFundName
			// 
			this.colFundName.Format = "";
			this.colFundName.FormatInfo = null;
			this.colFundName.HeaderText = "Fund Name";
			this.colFundName.MappingName = "FundName";
			this.colFundName.Width = 0;
			// 
			// colCMTAGiveUPName
			// 
			this.colCMTAGiveUPName.Format = "";
			this.colCMTAGiveUPName.FormatInfo = null;
			this.colCMTAGiveUPName.HeaderText = "CMTA/GiveUP Name";
			this.colCMTAGiveUPName.MappingName = "CMTAGiveUPName";
			this.colCMTAGiveUPName.Width = 0;
			// 
			// colCompanyUserName
			// 
			this.colCompanyUserName.Format = "";
			this.colCompanyUserName.FormatInfo = null;
			this.colCompanyUserName.HeaderText = "Company User Name";
			this.colCompanyUserName.MappingName = "CompanyUserName";
			this.colCompanyUserName.Width = 0;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.Enabled = false;
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(576, 290);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 34;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.Enabled = false;
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(494, 290);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 33;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// uctCreateCompanyCounterPartiesCompanyLevelTags
			// 
			this.uctCreateCompanyCounterPartiesCompanyLevelTags.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.uctCreateCompanyCounterPartiesCompanyLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.uctCreateCompanyCounterPartiesCompanyLevelTags.Location = new System.Drawing.Point(126, 6);
			this.uctCreateCompanyCounterPartiesCompanyLevelTags.Name = "uctCreateCompanyCounterPartiesCompanyLevelTags";
			this.uctCreateCompanyCounterPartiesCompanyLevelTags.Size = new System.Drawing.Size(414, 196);
			this.uctCreateCompanyCounterPartiesCompanyLevelTags.TabIndex = 36;
			this.uctCreateCompanyCounterPartiesCompanyLevelTags.SaveData += new System.EventHandler(this.uctCreateCompanyCounterPartiesCompanyLevelTags_SaveData);
			// 
			// CompanyCountePartiesCompanyLevelTags
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.uctCreateCompanyCounterPartiesCompanyLevelTags);
			this.Controls.Add(this.grdCompanyLevelTags);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CompanyCountePartiesCompanyLevelTags";
			this.Size = new System.Drawing.Size(658, 318);
			((System.ComponentModel.ISupportInitialize)(this.grdCompanyLevelTags)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public void InitializeCompanyControl()
		{
			uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyID = _companyID;
			uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
			//uctCreateCompanyCounterPartiesCompanyLevelTags.SetData();
		}
		
//		private CreateCompanyCounterPartiesCompanyLevelTags createCompanyCounterPartiesCompanyLevelTags = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
			uctCreateCompanyCounterPartiesCompanyLevelTags.SetData();
			CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = new  CompanyCounterPartyVenueDetails();
			uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdCompanyLevelTags.DataSource;

		}

		private void uctCreateCompanyCounterPartiesCompanyLevelTags_SaveData(object companyCounterPartyVenueDetails, EventArgs e) 
		{
			grdCompanyLevelTags.DataSource = null;
			grdCompanyLevelTags.Refresh();
			companyCounterPartyVenueDetails = uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails;

			grdCompanyLevelTags.DataSource = companyCounterPartyVenueDetails; 
			
			if(uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails.Count > 0 )
			{
				grdCompanyLevelTags.Select(0);
			}
			
			uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = null;
			uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueDetailEdit = null;	
		}
		public Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails CurrentCompanyCounterPartyVenueDetails
		{
			get 
			{
				return (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdCompanyLevelTags.DataSource; 
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
				uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyID = _companyID;
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
			}
		}

		private int _companyCounterPartyVenueDetailID = int.MinValue;
		public int CompanyCounterPartyVenueDetailID
		{
			get{return _companyCounterPartyVenueDetailID;}
			set
			{
				_companyCounterPartyVenueDetailID = value;
				//BindDataGrid();
			}
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = CompanyManager.GetCompanyCounterPartyVenueDetails(_companyCounterPartyID);
				grdCompanyLevelTags.DataSource = companyCounterPartyVenueDetails;
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
			companyCounterPartyVenueDetail = (CompanyCounterPartyVenueDetail)grdCompanyLevelTags.DataSource;						
		}
		
		public void SetCompanyCounterPartyVenueDetail(CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail)
		{
			grdCompanyLevelTags.DataSource = companyCounterPartyVenueDetail;
		}

//		private CreateCompanyCounterPartiesCompanyLevelTags createCompanyCounterPartiesCompanyLevelTagsEdit = null;
        		
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
//			if(createCompanyCounterPartiesCompanyLevelTagsEdit == null)
//			{
				if(grdCompanyLevelTags.VisibleRowCount > 0)
				{
					//createCompanyCounterPartiesCompanyLevelTagsEdit = new CreateCompanyCounterPartiesCompanyLevelTags();
					uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueDetailEdit = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail)((Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdCompanyLevelTags.DataSource)[grdCompanyLevelTags.CurrentCell.RowNumber];
					//uctCreateCompanyCounterPartiesCompanyLevelTags.
					
					uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails) grdCompanyLevelTags.DataSource;	
//					createCompanyCounterPartiesCompanyLevelTagsEdit.ShowDialog(this.Parent);					
				}
//			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(grdCompanyLevelTags.VisibleRowCount > 0)
			{
				if(MessageBox.Show(this, "Do you want to delete this Company Level Tag?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					int companyCounterPartyVenueDetailsID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 2].ToString());
				
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = (CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail();					
				
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 2].ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 3].ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 4].ToString());
					companyCounterPartyVenueDetail.UserID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 6].ToString());
					companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 7].ToString());
					companyCounterPartyVenueDetail.DeliverToCompanyID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 8].ToString();
					companyCounterPartyVenueDetail.DeiverToSubID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 9].ToString();
					companyCounterPartyVenueDetail.SenderCompanyID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 10].ToString();
					companyCounterPartyVenueDetail.FundID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 11].ToString());
					companyCounterPartyVenueDetail.OnBehalfOfCompID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 12].ToString();
					companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 13].ToString());
					companyCounterPartyVenueDetail.OnBehalfOfSubID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 14].ToString();
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueTagID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 15].ToString());

					companyCounterPartyVenueDetails.RemoveAt(companyCounterPartyVenueDetails.IndexOf(companyCounterPartyVenueDetail));
				
					if(companyCounterPartyVenueDetailsID != int.MinValue)
					{
						CompanyManager.DeleteCompanyCounterPartyVenueDetail(companyCounterPartyVenueDetailsID);	
					}				
					grdCompanyLevelTags.DataSource = null;
					grdCompanyLevelTags.DataSource = companyCounterPartyVenueDetails;
					grdCompanyLevelTags.Refresh();
				}
			}
		}
	}
}
