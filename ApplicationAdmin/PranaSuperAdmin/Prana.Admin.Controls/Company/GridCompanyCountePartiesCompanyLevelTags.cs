using Prana.Admin.BLL;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
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
        private System.Windows.Forms.DataGridTextBoxColumn colAccountID;
        private System.Windows.Forms.DataGridTextBoxColumn colOnBehalfOfCompID;
        private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUp;
        private System.Windows.Forms.DataGridTextBoxColumn colOnBehalfOfSubID;
        private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueTagID;
        private Prana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags uctCreateCompanyCounterPartiesCompanyLevelTags;
        private System.Windows.Forms.DataGridTextBoxColumn colCounterPartyFullName;
        private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueName;
        private System.Windows.Forms.DataGridTextBoxColumn colClearingFirmPrimeBroker;
        private System.Windows.Forms.DataGridTextBoxColumn colAccountName;
        private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUPName;
        private System.Windows.Forms.DataGridTextBoxColumn colCompanyUserName;
        private System.Windows.Forms.DataGridTextBoxColumn colStrategyID;
        private System.Windows.Forms.DataGridTextBoxColumn colStrategyName;
        private System.Windows.Forms.Label label1;
        //private Prana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags createCompanyCounterPartiesCompanyLevelTags1;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CompanyCountePartiesCompanyLevelTags()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            //			InitializeCompanyControl();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (grdCompanyLevelTags != null)
                {
                    grdCompanyLevelTags.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (dataGridTableStyle1 != null)
                {
                    dataGridTableStyle1.Dispose();
                }
                if (colCompanyCounterPartyVenueDetailsID != null)
                {
                    colCompanyCounterPartyVenueDetailsID.Dispose();
                }
                if (colCompanyCounterPartyID != null)
                {
                    colCompanyCounterPartyID.Dispose();
                }
                if (colCompanyCounterPartyVenueID != null)
                {
                    colCompanyCounterPartyVenueID.Dispose();
                }
                if (colUserID != null)
                {
                    colUserID.Dispose();
                }
                if (colClearingFirmPrimeBrokerID != null)
                {
                    colClearingFirmPrimeBrokerID.Dispose();
                }
                if (colClearingFirmPrimeBroker != null)
                {
                    colClearingFirmPrimeBroker.Dispose();
                }
                if (colDeliverToCompanyID != null)
                {
                    colDeliverToCompanyID.Dispose();
                }
                if (colDeiverToSubID != null)
                {
                    colDeiverToSubID.Dispose();
                }
                if (colSenderCompanyID != null)
                {
                    colSenderCompanyID.Dispose();
                }
                if (colAccountID != null)
                {
                    colAccountID.Dispose();
                }
                if (colOnBehalfOfCompID != null)
                {
                    colOnBehalfOfCompID.Dispose();
                }
                if (colOnBehalfOfCompID != null)
                {
                    colOnBehalfOfCompID.Dispose();
                }
                if (colCMTAGiveUp != null)
                {
                    colCMTAGiveUp.Dispose();
                }
                if (colOnBehalfOfSubID != null)
                {
                    colOnBehalfOfSubID.Dispose();
                }
                if (colCompanyCounterPartyVenueTagID != null)
                {
                    colCompanyCounterPartyVenueTagID.Dispose();
                }
                if (uctCreateCompanyCounterPartiesCompanyLevelTags != null)
                {
                    uctCreateCompanyCounterPartiesCompanyLevelTags.Dispose();
                }
                if (colCounterPartyFullName != null)
                {
                    colCounterPartyFullName.Dispose();
                }
                if (colCompanyCounterPartyVenueName != null)
                {
                    colCompanyCounterPartyVenueName.Dispose();
                }
                if (colClearingFirmPrimeBroker != null)
                {
                    colClearingFirmPrimeBroker.Dispose();
                }
                if (colAccountID != null)
                {
                    colAccountID.Dispose();
                }
                if (colCMTAGiveUp != null)
                {
                    colCMTAGiveUp.Dispose();
                }
                if (colAccountName != null)
                {
                    colAccountName.Dispose();
                }
                if (colCMTAGiveUPName != null)
                {
                    colCMTAGiveUPName.Dispose();
                }
                if (colCompanyUserName != null)
                {
                    colCompanyUserName.Dispose();
                }
                if (colStrategyID != null)
                {
                    colStrategyID.Dispose();
                }
                if (colStrategyName != null)
                {
                    colStrategyName.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
            }
            base.Dispose(disposing);
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
            this.colAccountID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colOnBehalfOfCompID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colCMTAGiveUp = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colOnBehalfOfSubID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colCompanyCounterPartyVenueTagID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colAccountName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colCMTAGiveUPName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colCompanyUserName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colStrategyID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colStrategyName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.uctCreateCompanyCounterPartiesCompanyLevelTags = new Prana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags();
            this.label1 = new System.Windows.Forms.Label();
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
            this.grdCompanyLevelTags.Location = new System.Drawing.Point(4, 356);
            this.grdCompanyLevelTags.Name = "grdCompanyLevelTags";
            this.grdCompanyLevelTags.ReadOnly = true;
            this.grdCompanyLevelTags.Size = new System.Drawing.Size(566, 78);
            this.grdCompanyLevelTags.TabIndex = 1;
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
            this.colAccountID,
                                                                                                                  this.colOnBehalfOfCompID,
                                                                                                                  this.colCMTAGiveUp,
                                                                                                                  this.colOnBehalfOfSubID,
                                                                                                                  this.colCompanyCounterPartyVenueTagID,
            this.colAccountName,
                                                                                                                  this.colCMTAGiveUPName,
                                                                                                                  this.colCompanyUserName,
                                                                                                                  this.colStrategyID,
                                                                                                                  this.colStrategyName});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "companyCounterPartyVenueDetails";
            // 
            // colCounterPartyFullName
            // 
            this.colCounterPartyFullName.Format = "";
            this.colCounterPartyFullName.FormatInfo = null;
            this.colCounterPartyFullName.HeaderText = "Full Name";
            this.colCounterPartyFullName.MappingName = "CounterPartyFullName";
            this.colCounterPartyFullName.Width = 95;
            // 
            // colCompanyCounterPartyVenueName
            // 
            this.colCompanyCounterPartyVenueName.Format = "";
            this.colCompanyCounterPartyVenueName.FormatInfo = null;
            this.colCompanyCounterPartyVenueName.HeaderText = "Venue Name";
            this.colCompanyCounterPartyVenueName.MappingName = "CompanyCounterPartyVenueName";
            this.colCompanyCounterPartyVenueName.Width = 95;
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
            this.colCompanyCounterPartyID.HeaderText = "Client Counter Party ID";
            this.colCompanyCounterPartyID.MappingName = "CompanyCounterPartyID";
            this.colCompanyCounterPartyID.Width = 0;
            // 
            // colCompanyCounterPartyVenueID
            // 
            this.colCompanyCounterPartyVenueID.Format = "";
            this.colCompanyCounterPartyVenueID.FormatInfo = null;
            this.colCompanyCounterPartyVenueID.HeaderText = "Client Counter Party Venue ID";
            this.colCompanyCounterPartyVenueID.MappingName = "CompanyCounterPartyVenueID";
            this.colCompanyCounterPartyVenueID.Width = 0;
            // 
            // colClearingFirmPrimeBroker
            // 
            this.colClearingFirmPrimeBroker.Format = "";
            this.colClearingFirmPrimeBroker.FormatInfo = null;
            this.colClearingFirmPrimeBroker.HeaderText = "Clearing Firm Prime Broker";
            this.colClearingFirmPrimeBroker.MappingName = "ClearingFirmPrimeBroker";
            this.colClearingFirmPrimeBroker.Width = 95;
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
            this.colDeliverToCompanyID.HeaderText = "Deliver To Client ID";
            this.colDeliverToCompanyID.MappingName = "DeliverToCompanyID";
            this.colDeliverToCompanyID.Width = 95;
            // 
            // colDeiverToSubID
            // 
            this.colDeiverToSubID.Format = "";
            this.colDeiverToSubID.FormatInfo = null;
            this.colDeiverToSubID.HeaderText = "Deiver To Sub ID";
            this.colDeiverToSubID.MappingName = "DeiverToSubID";
            this.colDeiverToSubID.Width = 95;
            // 
            // colSenderCompanyID
            // 
            this.colSenderCompanyID.Format = "";
            this.colSenderCompanyID.FormatInfo = null;
            this.colSenderCompanyID.HeaderText = "Sender Client ID";
            this.colSenderCompanyID.MappingName = "SenderCompanyID";
            this.colSenderCompanyID.Width = 95;
            // 
            // colAccountID
            // 
            this.colAccountID.Format = "";
            this.colAccountID.FormatInfo = null;
            this.colAccountID.MappingName = "AccountID";
            this.colAccountID.Width = 0;
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
            // colAccountName
            // 
            this.colAccountName.Format = "";
            this.colAccountName.FormatInfo = null;
            this.colAccountName.HeaderText = "Account Name";
            this.colAccountName.MappingName = "AccountName";
            this.colAccountName.Width = 0;
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
            this.colCompanyUserName.HeaderText = "Client User Name";
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
            // colStrategyName
            // 
            this.colStrategyName.Format = "";
            this.colStrategyName.FormatInfo = null;
            this.colStrategyName.MappingName = "StrategyName";
            this.colStrategyName.Width = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(494, 438);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(412, 438);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // uctCreateCompanyCounterPartiesCompanyLevelTags
            // 
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueID = -2147483648;
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Location = new System.Drawing.Point(56, 4);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Name = "uctCreateCompanyCounterPartiesCompanyLevelTags";
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Size = new System.Drawing.Size(446, 338);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.TabIndex = 0;
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.SaveData += new System.EventHandler(this.uctCreateCompanyCounterPartiesCompanyLevelTags_SaveData);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(4, 440);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "- Please Click the Save button below to save the data";
            // 
            // CompanyCountePartiesCompanyLevelTags
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uctCreateCompanyCounterPartiesCompanyLevelTags);
            this.Controls.Add(this.grdCompanyLevelTags);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CompanyCountePartiesCompanyLevelTags";
            this.Size = new System.Drawing.Size(578, 526);
            ((System.ComponentModel.ISupportInitialize)(this.grdCompanyLevelTags)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        //		public void InitializeCompanyControl()
        //		{
        //			uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyID = _companyID;
        //			uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
        //			uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails) grdCompanyLevelTags.DataSource;
        //			//uctCreateCompanyCounterPartiesCompanyLevelTags.SetData();
        //		}

        //		private CreateCompanyCounterPartiesCompanyLevelTags createCompanyCounterPartiesCompanyLevelTags = null;
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
            uctCreateCompanyCounterPartiesCompanyLevelTags.SetData();
            CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = new CompanyCounterPartyVenueDetails();
            uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;

        }

        private void uctCreateCompanyCounterPartiesCompanyLevelTags_SaveData(object companyCounterPartyVenueDetails, EventArgs e)
        {
            grdCompanyLevelTags.DataSource = null;
            grdCompanyLevelTags.Refresh();
            companyCounterPartyVenueDetails = uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails;

            grdCompanyLevelTags.DataSource = companyCounterPartyVenueDetails;

            if (uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails.Count > 0)
            {
                grdCompanyLevelTags.Select(0);
                uctCreateCompanyCounterPartiesCompanyLevelTags.NoData = 0;
            }
            else
            {
                uctCreateCompanyCounterPartiesCompanyLevelTags.NoData = 1;
            }

            uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = null;
            uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueDetailEdit = null;
        }
        public Prana.Admin.BLL.CompanyCounterPartyVenueDetails CurrentCompanyCounterPartyVenueDetails
        {
            get
            {
                return (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            get { return _companyID; }
            set
            {
                _companyID = value;
                //BindDataGrid();
                uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyID = _companyID;
            }
        }

        private int _companyCounterPartyID = int.MinValue;
        public int CompanyCounterPartyID
        {
            get { return _companyCounterPartyID; }
            set
            {
                _companyCounterPartyID = value;

                //				InitializeCompanyControl();
                //				BindDataGrid();

                uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
            }
        }

        private int _companyCounterPartyVenueDetailID = int.MinValue;
        public int CompanyCounterPartyVenueDetailID
        {
            get { return _companyCounterPartyVenueDetailID; }
            set
            {
                _companyCounterPartyVenueDetailID = value;
            }
        }

        public CompanyCounterPartyVenueDetail CompanyCounterPartyVenueDetailProperty
        {
            get
            {
                CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail();
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
            if (grdCompanyLevelTags.VisibleRowCount > 0)
            {
                //createCompanyCounterPartiesCompanyLevelTagsEdit = new CreateCompanyCounterPartiesCompanyLevelTags();
                uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueDetailEdit = (Prana.Admin.BLL.CompanyCounterPartyVenueDetail)((Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource)[grdCompanyLevelTags.CurrentCell.RowNumber];
                //uctCreateCompanyCounterPartiesCompanyLevelTags.

                uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
                //					createCompanyCounterPartiesCompanyLevelTagsEdit.ShowDialog(this.Parent);					
            }
            //			}
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            if (grdCompanyLevelTags.VisibleRowCount > 0)
            {
                if (MessageBox.Show(this, "Do you want to delete this Client Level Tag?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int companyCounterPartyVenueDetailsID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 2].ToString());

                    Prana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = (CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
                    Prana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Prana.Admin.BLL.CompanyCounterPartyVenueDetail();

                    companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 2].ToString());
                    companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 3].ToString());
                    companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 4].ToString());
                    companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 7].ToString());
                    companyCounterPartyVenueDetail.DeliverToCompanyID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 8].ToString();
                    companyCounterPartyVenueDetail.SenderCompanyID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 10].ToString();
                    companyCounterPartyVenueDetail.AccountID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 11].ToString());
                    companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 13].ToString());
                    companyCounterPartyVenueDetail.OnBehalfOfSubID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 14].ToString();

                    companyCounterPartyVenueDetails.RemoveAt(companyCounterPartyVenueDetails.IndexOf(companyCounterPartyVenueDetail));

                    if (companyCounterPartyVenueDetailsID != int.MinValue)
                    {
                        CompanyManager.DeleteCompanyCounterPartyVenueDetail(companyCounterPartyVenueDetailsID);
                    }

                    CompanyCounterPartyVenueDetails newCompanyCounterPartyVenueDetails = new CompanyCounterPartyVenueDetails();
                    foreach (CompanyCounterPartyVenueDetail tempCompanyCounterPartyVenueDetail in companyCounterPartyVenueDetails)
                    {
                        newCompanyCounterPartyVenueDetails.Add(tempCompanyCounterPartyVenueDetail);
                    }
                    grdCompanyLevelTags.DataSource = newCompanyCounterPartyVenueDetails;

                    if (companyCounterPartyVenueDetails.Count <= 0)
                    {
                        uctCreateCompanyCounterPartiesCompanyLevelTags.NoData = 1;
                    }

                }
            }
        }
    }
}
