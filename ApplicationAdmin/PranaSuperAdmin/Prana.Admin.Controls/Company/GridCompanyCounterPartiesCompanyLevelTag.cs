using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Interface;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for cc.
    /// </summary>
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.CounterPartyVenueDeleted, ShowAuditUI = true)]
    public class GridCompanyCounterPartiesCompanyLevelTag : System.Windows.Forms.UserControl, IAuditSource
    {
        private const string FORM_NAME = "GridCompanyCounterPartiesAccountLevelTags : ";
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label label1;
        private Prana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags uctCreateCompanyCounterPartiesCompanyLevelTags;
        private System.Windows.Forms.DataGrid grdCompanyLevelTags;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueDetailsID;
        private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueID;
        private System.Windows.Forms.DataGridTextBoxColumn colDeliverToCompanyID;
        private System.Windows.Forms.DataGridTextBoxColumn colSenderCompanyID;
        private System.Windows.Forms.DataGridTextBoxColumn colOnBehalfOfSubID;
        private System.Windows.Forms.DataGridTextBoxColumn colClearingFirm;
        private System.Windows.Forms.DataGridTextBoxColumn colIdentifierName;
        private System.Windows.Forms.DataGridTextBoxColumn colCompanyCounterPartyVenueName;
        private System.Windows.Forms.DataGridTextBoxColumn colClearingFirmPrimeBroker;
        private System.Windows.Forms.DataGridTextBoxColumn colAccountName;
        private System.Windows.Forms.DataGridTextBoxColumn colStrategyName;
        private System.Windows.Forms.DataGridTextBoxColumn colCMTAGiveUPName;
        private System.Windows.Forms.DataGridTextBoxColumn colMPIDName;
        private System.Windows.Forms.DataGridTextBoxColumn colTargetCompID;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        [AuditManager.Attributes.AuditSourceConstAttri]
        public GridCompanyCounterPartiesCompanyLevelTag()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.uctCreateCompanyCounterPartiesCompanyLevelTags.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uctCreateCompanyCounterPartiesCompanyLevelTags.ForeColor = System.Drawing.Color.White;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
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
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (uctCreateCompanyCounterPartiesCompanyLevelTags != null)
                {
                    uctCreateCompanyCounterPartiesCompanyLevelTags.Dispose();
                }
                if (grdCompanyLevelTags != null)
                {
                    grdCompanyLevelTags.Dispose();
                }
                if (dataGridTableStyle1 != null)
                {
                    dataGridTableStyle1.Dispose();
                }
                if (colCompanyCounterPartyVenueDetailsID != null)
                {
                    colCompanyCounterPartyVenueDetailsID.Dispose();
                }
                if (colCompanyCounterPartyVenueID != null)
                {
                    colCompanyCounterPartyVenueID.Dispose();
                }
                if (colDeliverToCompanyID != null)
                {
                    colDeliverToCompanyID.Dispose();
                }
                if (colSenderCompanyID != null)
                {
                    colSenderCompanyID.Dispose();
                }
                if (colOnBehalfOfSubID != null)
                {
                    colOnBehalfOfSubID.Dispose();
                }
                if (colClearingFirm != null)
                {
                    colClearingFirm.Dispose();
                }
                if (colIdentifierName != null)
                {
                    colIdentifierName.Dispose();
                }
                if (colCompanyCounterPartyVenueName != null)
                {
                    colCompanyCounterPartyVenueName.Dispose();
                }
                if (colClearingFirmPrimeBroker != null)
                {
                    colClearingFirmPrimeBroker.Dispose();
                }
                if (colAccountName != null)
                {
                    colAccountName.Dispose();
                }
                if (colStrategyName != null)
                {
                    colStrategyName.Dispose();
                }
                if (colCMTAGiveUPName != null)
                {
                    colCMTAGiveUPName.Dispose();
                }
                if (colMPIDName != null)
                {
                    colMPIDName.Dispose();
                }
                if (colTargetCompID != null)
                {
                    colTargetCompID.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridCompanyCounterPartiesCompanyLevelTag));
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grdCompanyLevelTags = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.colCompanyCounterPartyVenueDetailsID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colCompanyCounterPartyVenueID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colDeliverToCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colTargetCompID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colSenderCompanyID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colClearingFirm = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colIdentifierName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colCompanyCounterPartyVenueName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colClearingFirmPrimeBroker = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colAccountName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colStrategyName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colOnBehalfOfSubID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colCMTAGiveUPName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colMPIDName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.uctCreateCompanyCounterPartiesCompanyLevelTags = new Prana.Admin.Controls.CreateCompanyCounterPartiesCompanyLevelTags();
            ((System.ComponentModel.ISupportInitialize)(this.grdCompanyLevelTags)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(496, 448);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(415, 448);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(3, 453);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "- Please Click the Save button below to save the data";
            // 
            // grdCompanyLevelTags
            // 
            this.grdCompanyLevelTags.CaptionVisible = false;
            this.grdCompanyLevelTags.DataMember = "";
            // this.grdCompanyLevelTags.UseFlatMode = DefaultableBoolean.True;
            this.grdCompanyLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdCompanyLevelTags.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.grdCompanyLevelTags.Location = new System.Drawing.Point(499, 209);
            this.grdCompanyLevelTags.Name = "grdCompanyLevelTags";
            this.grdCompanyLevelTags.ReadOnly = true;
            this.grdCompanyLevelTags.Size = new System.Drawing.Size(61, 28);
            this.grdCompanyLevelTags.TabIndex = 43;
            this.grdCompanyLevelTags.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.grdCompanyLevelTags.Visible = false;
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.grdCompanyLevelTags;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.colCompanyCounterPartyVenueDetailsID,
            this.colCompanyCounterPartyVenueID,
            this.colDeliverToCompanyID,
            this.colTargetCompID,
            this.colSenderCompanyID,
            this.colClearingFirm,
            this.colIdentifierName,
            this.colCompanyCounterPartyVenueName,
            this.colClearingFirmPrimeBroker,
            this.colAccountName,
            this.colStrategyName,
            this.colOnBehalfOfSubID,
            this.colCMTAGiveUPName,
            this.colMPIDName});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "companyCounterPartyVenueDetails";
            // 
            // colCompanyCounterPartyVenueDetailsID
            // 
            this.colCompanyCounterPartyVenueDetailsID.Format = "";
            this.colCompanyCounterPartyVenueDetailsID.FormatInfo = null;
            this.colCompanyCounterPartyVenueDetailsID.MappingName = "CompanyCounterPartyVenueDetailsID";
            this.colCompanyCounterPartyVenueDetailsID.Width = 0;
            // 
            // colCompanyCounterPartyVenueID
            // 
            this.colCompanyCounterPartyVenueID.Format = "";
            this.colCompanyCounterPartyVenueID.FormatInfo = null;
            this.colCompanyCounterPartyVenueID.MappingName = "CompanyCounterPartyVenueID";
            this.colCompanyCounterPartyVenueID.Width = 0;
            // 
            // colDeliverToCompanyID
            // 
            this.colDeliverToCompanyID.Format = "";
            this.colDeliverToCompanyID.FormatInfo = null;
            this.colDeliverToCompanyID.HeaderText = "DeliverToCompID";
            this.colDeliverToCompanyID.MappingName = "DeliverToCompanyID";
            this.colDeliverToCompanyID.Width = 90;
            // 
            // colTargetCompID
            // 
            this.colTargetCompID.Format = "";
            this.colTargetCompID.FormatInfo = null;
            this.colTargetCompID.HeaderText = "TargetCompID";
            this.colTargetCompID.MappingName = "TargetCompID";
            this.colTargetCompID.Width = 90;
            // 
            // colSenderCompanyID
            // 
            this.colSenderCompanyID.Format = "";
            this.colSenderCompanyID.FormatInfo = null;
            this.colSenderCompanyID.HeaderText = "SenderCompanyID";
            this.colSenderCompanyID.MappingName = "SenderCompanyID";
            this.colSenderCompanyID.Width = 90;
            // 
            // colClearingFirm
            // 
            this.colClearingFirm.Format = "";
            this.colClearingFirm.FormatInfo = null;
            this.colClearingFirm.HeaderText = "ClearingFirm";
            this.colClearingFirm.MappingName = "ClearingFirm";
            this.colClearingFirm.Width = 90;
            // 
            // colIdentifierName
            // 
            this.colIdentifierName.Format = "";
            this.colIdentifierName.FormatInfo = null;
            this.colIdentifierName.HeaderText = "Identifier Name";
            this.colIdentifierName.MappingName = "IdentifierName";
            this.colIdentifierName.Width = 90;
            // 
            // colCompanyCounterPartyVenueName
            // 
            this.colCompanyCounterPartyVenueName.Format = "";
            this.colCompanyCounterPartyVenueName.FormatInfo = null;
            this.colCompanyCounterPartyVenueName.HeaderText = "CompanyCounterPartyVenue";
            this.colCompanyCounterPartyVenueName.MappingName = "CompanyCounterPartyVenueName";
            this.colCompanyCounterPartyVenueName.Width = 90;
            // 
            // colClearingFirmPrimeBroker
            // 
            this.colClearingFirmPrimeBroker.Format = "";
            this.colClearingFirmPrimeBroker.FormatInfo = null;
            this.colClearingFirmPrimeBroker.HeaderText = "ClearingFirm/PrimeBroker";
            this.colClearingFirmPrimeBroker.MappingName = "ClearingFirmPrimeBroker";
            this.colClearingFirmPrimeBroker.Width = 90;
            // 
            // colAccountName
            // 
            this.colAccountName.Format = "";
            this.colAccountName.FormatInfo = null;
            this.colAccountName.HeaderText = "AccountName";
            this.colAccountName.MappingName = "AccountName";
            this.colAccountName.Width = 90;
            // 
            // colStrategyName
            // 
            this.colStrategyName.Format = "";
            this.colStrategyName.FormatInfo = null;
            this.colStrategyName.HeaderText = "Strategy";
            this.colStrategyName.MappingName = "StrategyName";
            this.colStrategyName.Width = 90;
            // 
            // colOnBehalfOfSubID
            // 
            this.colOnBehalfOfSubID.Format = "";
            this.colOnBehalfOfSubID.FormatInfo = null;
            this.colOnBehalfOfSubID.HeaderText = "OnBehalfOfSubID";
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
            // colMPIDName
            // 
            this.colMPIDName.Format = "";
            this.colMPIDName.FormatInfo = null;
            this.colMPIDName.HeaderText = "MPID";
            this.colMPIDName.MappingName = "MPIDName";
            this.colMPIDName.Width = 90;
            // 
            // uctCreateCompanyCounterPartiesCompanyLevelTags
            // 
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.AutoSize = true;
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueID = -2147483648;
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Location = new System.Drawing.Point(48, 2);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.MinimumSize = new System.Drawing.Size(515, 437);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Name = "uctCreateCompanyCounterPartiesCompanyLevelTags";
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Size = new System.Drawing.Size(515, 438);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.TabIndex = 0;
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.SaveData += new System.EventHandler(this.uctCreateCompanyCounterPartiesCompanyLevelTags_SaveData);
            this.uctCreateCompanyCounterPartiesCompanyLevelTags.Load += new System.EventHandler(this.uctCreateCompanyCounterPartiesCompanyLevelTags_Load);
            // 
            // GridCompanyCounterPartiesCompanyLevelTag
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grdCompanyLevelTags);
            this.Controls.Add(this.uctCreateCompanyCounterPartiesCompanyLevelTags);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.MinimumSize = new System.Drawing.Size(580, 480);
            this.Name = "GridCompanyCounterPartiesCompanyLevelTag";
            this.Size = new System.Drawing.Size(580, 480);
            this.Load += new System.EventHandler(this.GridCompanyCounterPartiesCompanyLevelTag_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCompanyLevelTags)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public void InitializeCompanyControl()
        {
            uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyID = _companyID;
            uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
            uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
        }

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
            uctCreateCompanyCounterPartiesCompanyLevelTags.SetData();
            CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = new CompanyCounterPartyVenueDetails();
            uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;

        }

        private void uctCreateCompanyCounterPartiesCompanyLevelTags_SaveData(object companyCounterPartyVenueDetails, EventArgs e)
        {
            Prana.Admin.BLL.CompanyCounterPartyVenueDetails nullCompanyCounterPartyVenueDetails = CompanyManager.GetCompanyCounterPartyVenueDetails(_companyCounterPartyID);
            CompanyCounterPartyVenueDetail nullCompanyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail(int.MinValue, "", "", "", "", "", "", "", "", "", "", "", "");

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
                nullCompanyCounterPartyVenueDetails.Add(nullCompanyCounterPartyVenueDetail);
                grdCompanyLevelTags.DataSource = nullCompanyCounterPartyVenueDetails;
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

        //New property created as an ad-hoc for demo for now.
        public CompanyCounterPartyVenueDetail CompanyCounterPartyVenueDetailSave
        {
            get
            {
                CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = uctCreateCompanyCounterPartiesCompanyLevelTags.SaveCounterPartyVenueDetail();
                return companyCounterPartyVenueDetail;
            }


        }

        public void SetGiveUpGrid()
        {
            uctCreateCompanyCounterPartiesCompanyLevelTags.BindGiveUpGrid();

        }

        public void SetCMTAGrid()
        {
            uctCreateCompanyCounterPartiesCompanyLevelTags.BindCMTAGrid();
        }


        public CompanyCVGiveUpIdentifiers CompanyCVGiveUpIdentifiersSave
        {
            get
            {
                CompanyCVGiveUpIdentifiers companyCVGiveUpIdentifiers = uctCreateCompanyCounterPartiesCompanyLevelTags.companyCVGiveUpproperties;
                return companyCVGiveUpIdentifiers;
            }
            set
            {
                uctCreateCompanyCounterPartiesCompanyLevelTags.companyCVGiveUpproperties = value;
            }
        }

        public CompanyCVCMTAIdentifiers CompanyCVCMTAIdentifiersSave
        {
            get
            {
                CompanyCVCMTAIdentifiers companyCVCMTAIdentifiers = uctCreateCompanyCounterPartiesCompanyLevelTags.companyCVCMTAproperties;
                return companyCVCMTAIdentifiers;
            }
            set
            {
                uctCreateCompanyCounterPartiesCompanyLevelTags.companyCVCMTAproperties = value;
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            get { return _companyID; }
            //			set
            //			{
            //				_companyID = value;	
            //				uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyID = _companyID;
            //			}
        }

        private int _companyCounterPartyID = int.MinValue;
        public int CompanyCounterPartyID
        {
            get { return _companyCounterPartyID; }
            //			set
            //			{
            //				_companyCounterPartyID = value;				
            //				
            //				InitializeCompanyControl();
            //				BindDataGrid();
            //
            //				uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails) grdCompanyLevelTags.DataSource;
            //			}
        }

        private int _companyCounterPartyVenueID = int.MinValue;
        //		public int CompanyCounterPartyVenueID
        //		{
        //			set
        //			{
        //				_companyCounterPartyVenueID = value;
        //				uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueID = _companyCounterPartyVenueID;
        //			}
        //		}

        private int _companyCounterPartyVenueDetailID = int.MinValue;
        public int CompanyCounterPartyVenueDetailID
        {
            get { return _companyCounterPartyVenueDetailID; }
            //			set
            //			{
            //				_companyCounterPartyVenueDetailID = value;
            //				BindDataGrid();
            //			}
        }

        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void SetupControl(int companyCounterPartyVenueID, int companyCounterPartyID, int companyID)
        {
            _companyID = companyID;
            _companyCounterPartyID = companyCounterPartyID;
            _companyCounterPartyVenueID = companyCounterPartyVenueID;
            //_companyCounterPartyVenueDetailID = companyCounterPartyVenueDetailID;
            _companyCounterPartyVenueDetailID = companyCounterPartyVenueID;
            InitializeCompanyControl();
            uctCreateCompanyCounterPartiesCompanyLevelTags.SetupControl(companyCounterPartyVenueID);


            BindDataGrid();

            uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueID = _companyCounterPartyVenueID;
            uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
        }

        private void BindDataGrid()
        {
            try
            {
                Prana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = CompanyManager.GetCompanyCounterPartyVenueDetails(_companyCounterPartyID);

                CompanyCounterPartyVenueDetail nullCompanyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail(int.MinValue, "", "", "", "", "", "", "", "", "", "", "", "");
                if (companyCounterPartyVenueDetails.Count <= 0)
                {
                    companyCounterPartyVenueDetails.Add(nullCompanyCounterPartyVenueDetail);
                }
                grdCompanyLevelTags.DataSource = companyCounterPartyVenueDetails;
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnLogin_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnLogin_Click", null);
                #endregion
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
            //			set 
            //			{
            //				SetCompanyCounterPartyVenueDetail(value);
            //					
            //			}
        }


        public void GetCompanyCounterPartyVenueDetail(CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail)
        {
            companyCounterPartyVenueDetail = (CompanyCounterPartyVenueDetail)grdCompanyLevelTags.DataSource;
        }

        //		public void SetCompanyCounterPartyVenueDetail(CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail)
        //		{
        //			grdCompanyLevelTags.DataSource = companyCounterPartyVenueDetail;
        //		}


        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (grdCompanyLevelTags.VisibleRowCount > 0)
            {
                if (int.Parse(grdCompanyLevelTags.CurrentCell.RowNumber.ToString()) > int.MinValue)
                {
                    uctCreateCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueDetailEdit = (Prana.Admin.BLL.CompanyCounterPartyVenueDetail)((Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource)[grdCompanyLevelTags.CurrentCell.RowNumber];

                    uctCreateCompanyCounterPartiesCompanyLevelTags.CurrentCompanyCounterPartyVenueDetails = (Prana.Admin.BLL.CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
                }
            }
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            if (grdCompanyLevelTags.VisibleRowCount > 0)
            {
                if (MessageBox.Show(this, "Do you want to delete this Client Level Tag?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int companyCounterPartyVenueDetailsID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 0].ToString());
                    int companyCounterPartyVenueID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 1].ToString());

                    Prana.Admin.BLL.CompanyCounterPartyVenueDetails companyCounterPartyVenueDetails = (CompanyCounterPartyVenueDetails)grdCompanyLevelTags.DataSource;
                    Prana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Prana.Admin.BLL.CompanyCounterPartyVenueDetail();
                    if (companyCounterPartyVenueID > 0)
                    {
                        companyCounterPartyVenueDetail.CompanyCounterPartyVenueDetailsID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 0].ToString());
                        companyCounterPartyVenueDetail.TargetCompID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 3].ToString();
                        companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 1].ToString());
                        companyCounterPartyVenueDetail.ClearingFirmPrimeBroker = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 8].ToString();
                        companyCounterPartyVenueDetail.DeliverToCompanyID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 2].ToString();
                        companyCounterPartyVenueDetail.SenderCompanyID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 4].ToString();
                        companyCounterPartyVenueDetail.AccountName = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 9].ToString();
                        companyCounterPartyVenueDetail.OnBehalfOfSubID = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 11].ToString();
                        companyCounterPartyVenueDetail.ClearingFirm = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 5].ToString();

                        //Commented as per Shams comments
                        //companyCounterPartyVenueDetail.IdentifierName = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 6].ToString();

                        companyCounterPartyVenueDetail.CompanyCounterPartyVenueName = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 7].ToString();
                        companyCounterPartyVenueDetail.StrategyName = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 10].ToString();
                        companyCounterPartyVenueDetail.CMTAGiveUPName = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 12].ToString();
                        companyCounterPartyVenueDetail.MPIDName = grdCompanyLevelTags[grdCompanyLevelTags.CurrentCell.RowNumber, 13].ToString();

                        companyCounterPartyVenueDetails.RemoveAt(companyCounterPartyVenueDetails.IndexOf(companyCounterPartyVenueDetail));
                    }

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
                BindDataGrid();
            }
        }

        private void uctCreateCompanyCounterPartiesCompanyLevelTags_Load(object sender, System.EventArgs e)
        {

        }

        private void GridCompanyCounterPartiesCompanyLevelTag_Load(object sender, System.EventArgs e)
        {

        }
    }
}
