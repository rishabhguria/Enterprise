#region Using
using Prana.Admin.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
//
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyVenue.
    /// </summary>
    public class CompanyVenue : System.Windows.Forms.UserControl
    {
        const string C_COMBO_SELECT = "- Select -";
        private const string NULLYEAR = "10/10/2000";

        private System.Windows.Forms.GroupBox grpCompanyVenue;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label lblVenueName;
        private System.Windows.Forms.TextBox txtVenueShortName;
        private System.Windows.Forms.TextBox txtVenueName;
        private System.Windows.Forms.Label lblVenueType;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbVenueType;
        private System.Windows.Forms.Label lblVenueShortName;
        private System.Windows.Forms.GroupBox grpVenueDetails;
        private Prana.Admin.Controls.TimeControl txtPreMarketTradingStartTime;
        private Prana.Admin.Controls.TimeControl txtPreMarketTradingEndTime;
        private Prana.Admin.Controls.TimeControl txtRegularTradingEndTime;
        private Prana.Admin.Controls.TimeControl txtRegularTradingStartTime;
        private Prana.Admin.Controls.TimeControl txtLunchTimeStartTime;
        private Prana.Admin.Controls.TimeControl txtLunchTimeEndTime;
        private Prana.Admin.Controls.TimeControl txtPostMarketTradingStartTime;
        private Prana.Admin.Controls.TimeControl txtPostMarketTradingEndTime;
        private System.Windows.Forms.Label lblPostMarket;
        private System.Windows.Forms.Label lblLunchTime;
        private System.Windows.Forms.Label lblRegularTime;
        private System.Windows.Forms.Label lblPreMarket;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkRegularMarket;
        private System.Windows.Forms.CheckBox chkPostMarket;
        private System.Windows.Forms.CheckBox chkLunchTime;
        private System.Windows.Forms.CheckBox chkPreMarket;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor cmbTimeZone;
        private IContainer components;

        public CompanyVenue()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();


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
                if (grpCompanyVenue != null)
                {
                    grpCompanyVenue.Dispose();
                }
                if (label41 != null)
                {
                    label41.Dispose();
                }
                if (label42 != null)
                {
                    label42.Dispose();
                }
                if (label45 != null)
                {
                    label45.Dispose();
                }
                if (lblVenueName != null)
                {
                    lblVenueName.Dispose();
                }
                if (txtVenueShortName != null)
                {
                    txtVenueShortName.Dispose();
                }
                if (txtVenueName != null)
                {
                    txtVenueName.Dispose();
                }
                if (lblVenueType != null)
                {
                    lblVenueType.Dispose();
                }
                if (cmbVenueType != null)
                {
                    cmbVenueType.Dispose();
                }
                if (lblVenueShortName != null)
                {
                    lblVenueShortName.Dispose();
                }
                if (grpVenueDetails != null)
                {
                    grpVenueDetails.Dispose();
                }
                if (txtPreMarketTradingEndTime != null)
                {
                    txtPreMarketTradingEndTime.Dispose();
                }
                if (txtPreMarketTradingEndTime != null)
                {
                    txtPreMarketTradingEndTime.Dispose();
                }
                if (txtRegularTradingEndTime != null)
                {
                    txtRegularTradingEndTime.Dispose();
                }
                if (txtRegularTradingStartTime != null)
                {
                    txtRegularTradingStartTime.Dispose();
                }
                if (txtLunchTimeEndTime != null)
                {
                    txtLunchTimeEndTime.Dispose();
                }
                if (txtLunchTimeStartTime != null)
                {
                    txtLunchTimeStartTime.Dispose();
                }
                if (txtPostMarketTradingEndTime != null)
                {
                    txtPostMarketTradingEndTime.Dispose();
                }
                if (txtPostMarketTradingStartTime != null)
                {
                    txtPostMarketTradingStartTime.Dispose();
                }
                if (lblPostMarket != null)
                {
                    lblPostMarket.Dispose();
                }
                if (lblLunchTime != null)
                {
                    lblLunchTime.Dispose();
                }
                if (lblRegularTime != null)
                {
                    lblRegularTime.Dispose();
                }
                if (lblPreMarket != null)
                {
                    lblPreMarket.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (chkRegularMarket != null)
                {
                    chkRegularMarket.Dispose();
                }
                if (chkPostMarket != null)
                {
                    chkPostMarket.Dispose();
                }
                if (chkLunchTime != null)
                {
                    chkLunchTime.Dispose();
                }
                if (chkPreMarket != null)
                {
                    chkPreMarket.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (cmbTimeZone != null)
                {
                    cmbTimeZone.Dispose();
                }
                if (txtPreMarketTradingStartTime != null)
                {
                    txtPreMarketTradingStartTime.Dispose();
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueTypeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.grpCompanyVenue = new System.Windows.Forms.GroupBox();
            this.cmbVenueType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.txtVenueShortName = new System.Windows.Forms.TextBox();
            this.txtVenueName = new System.Windows.Forms.TextBox();
            this.lblVenueType = new System.Windows.Forms.Label();
            this.lblVenueShortName = new System.Windows.Forms.Label();
            this.lblVenueName = new System.Windows.Forms.Label();
            this.grpVenueDetails = new System.Windows.Forms.GroupBox();
            this.cmbTimeZone = new Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor();
            this.lblPostMarket = new System.Windows.Forms.Label();
            this.lblLunchTime = new System.Windows.Forms.Label();
            this.lblRegularTime = new System.Windows.Forms.Label();
            this.lblPreMarket = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkRegularMarket = new System.Windows.Forms.CheckBox();
            this.chkPostMarket = new System.Windows.Forms.CheckBox();
            this.chkLunchTime = new System.Windows.Forms.CheckBox();
            this.chkPreMarket = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtPreMarketTradingStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtPreMarketTradingEndTime = new Prana.Admin.Controls.TimeControl();
            this.txtRegularTradingEndTime = new Prana.Admin.Controls.TimeControl();
            this.txtRegularTradingStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtLunchTimeStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtLunchTimeEndTime = new Prana.Admin.Controls.TimeControl();
            this.txtPostMarketTradingStartTime = new Prana.Admin.Controls.TimeControl();
            this.txtPostMarketTradingEndTime = new Prana.Admin.Controls.TimeControl();
            this.grpCompanyVenue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenueType)).BeginInit();
            this.grpVenueDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCompanyVenue
            // 
            this.grpCompanyVenue.Controls.Add(this.cmbVenueType);
            this.grpCompanyVenue.Controls.Add(this.label41);
            this.grpCompanyVenue.Controls.Add(this.label42);
            this.grpCompanyVenue.Controls.Add(this.label45);
            this.grpCompanyVenue.Controls.Add(this.txtVenueShortName);
            this.grpCompanyVenue.Controls.Add(this.txtVenueName);
            this.grpCompanyVenue.Controls.Add(this.lblVenueType);
            this.grpCompanyVenue.Controls.Add(this.lblVenueShortName);
            this.grpCompanyVenue.Controls.Add(this.lblVenueName);
            this.grpCompanyVenue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCompanyVenue.Location = new System.Drawing.Point(2, 6);
            this.grpCompanyVenue.Name = "grpCompanyVenue";
            this.grpCompanyVenue.Size = new System.Drawing.Size(372, 88);
            this.grpCompanyVenue.TabIndex = 0;
            this.grpCompanyVenue.TabStop = false;
            this.grpCompanyVenue.Text = "Venue";
            // 
            // cmbVenueType
            // 
            this.cmbVenueType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
                                                             ultraGridColumn1,
                                                             ultraGridColumn2});
            this.cmbVenueType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbVenueType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenueType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance1.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenueType.DisplayLayout.Override.RowAppearance = appearance1;
            this.cmbVenueType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenueType.DropDownWidth = 0;
            this.cmbVenueType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbVenueType.LimitToList = true;
            this.cmbVenueType.Location = new System.Drawing.Point(152, 62);
            this.cmbVenueType.Name = "cmbVenueType";
            this.cmbVenueType.Size = new System.Drawing.Size(150, 21);
            this.cmbVenueType.TabIndex = 4;
            this.cmbVenueType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbVenueType.GotFocus += new System.EventHandler(this.cmbVenueType_GotFocus);
            this.cmbVenueType.LostFocus += new System.EventHandler(this.cmbVenueType_LostFocus);
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label41.ForeColor = System.Drawing.Color.Red;
            this.label41.Location = new System.Drawing.Point(134, 42);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(12, 10);
            this.label41.TabIndex = 1;
            this.label41.Text = "*";
            // 
            // label42
            // 
            this.label42.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label42.ForeColor = System.Drawing.Color.Red;
            this.label42.Location = new System.Drawing.Point(104, 64);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(12, 8);
            this.label42.TabIndex = 15;
            this.label42.Text = "*";
            // 
            // label45
            // 
            this.label45.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label45.ForeColor = System.Drawing.Color.Red;
            this.label45.Location = new System.Drawing.Point(106, 21);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(12, 9);
            this.label45.TabIndex = 12;
            this.label45.Text = "*";
            // 
            // txtVenueShortName
            // 
            this.txtVenueShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVenueShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtVenueShortName.Location = new System.Drawing.Point(152, 40);
            this.txtVenueShortName.MaxLength = 50;
            this.txtVenueShortName.Name = "txtVenueShortName";
            this.txtVenueShortName.Size = new System.Drawing.Size(150, 21);
            this.txtVenueShortName.TabIndex = 3;
            this.txtVenueShortName.GotFocus += new System.EventHandler(this.txtVenueShortName_GotFocus);
            this.txtVenueShortName.LostFocus += new System.EventHandler(this.txtVenueShortName_LostFocus);
            // 
            // txtVenueName
            // 
            this.txtVenueName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVenueName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtVenueName.Location = new System.Drawing.Point(152, 18);
            this.txtVenueName.MaxLength = 50;
            this.txtVenueName.Name = "txtVenueName";
            this.txtVenueName.Size = new System.Drawing.Size(150, 21);
            this.txtVenueName.TabIndex = 0;
            this.txtVenueName.GotFocus += new System.EventHandler(this.txtVenueName_GotFocus);
            this.txtVenueName.LostFocus += new System.EventHandler(this.txtVenueName_LostFocus);
            // 
            // lblVenueType
            // 
            this.lblVenueType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVenueType.Location = new System.Drawing.Point(70, 64);
            this.lblVenueType.Name = "lblVenueType";
            this.lblVenueType.Size = new System.Drawing.Size(34, 16);
            this.lblVenueType.TabIndex = 3;
            this.lblVenueType.Text = "Type";
            // 
            // lblVenueShortName
            // 
            this.lblVenueShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVenueShortName.Location = new System.Drawing.Point(70, 42);
            this.lblVenueShortName.Name = "lblVenueShortName";
            this.lblVenueShortName.Size = new System.Drawing.Size(64, 14);
            this.lblVenueShortName.TabIndex = 0;
            this.lblVenueShortName.Text = "Short Name";
            // 
            // lblVenueName
            // 
            this.lblVenueName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVenueName.Location = new System.Drawing.Point(70, 21);
            this.lblVenueName.Name = "lblVenueName";
            this.lblVenueName.Size = new System.Drawing.Size(36, 14);
            this.lblVenueName.TabIndex = 0;
            this.lblVenueName.Text = "Name";
            // 
            // grpVenueDetails
            // 
            this.grpVenueDetails.Controls.Add(this.cmbTimeZone);
            this.grpVenueDetails.Controls.Add(this.txtPreMarketTradingStartTime);
            this.grpVenueDetails.Controls.Add(this.txtPreMarketTradingEndTime);
            this.grpVenueDetails.Controls.Add(this.txtRegularTradingEndTime);
            this.grpVenueDetails.Controls.Add(this.txtRegularTradingStartTime);
            this.grpVenueDetails.Controls.Add(this.txtLunchTimeStartTime);
            this.grpVenueDetails.Controls.Add(this.txtLunchTimeEndTime);
            this.grpVenueDetails.Controls.Add(this.txtPostMarketTradingStartTime);
            this.grpVenueDetails.Controls.Add(this.txtPostMarketTradingEndTime);
            this.grpVenueDetails.Controls.Add(this.lblPostMarket);
            this.grpVenueDetails.Controls.Add(this.lblLunchTime);
            this.grpVenueDetails.Controls.Add(this.lblRegularTime);
            this.grpVenueDetails.Controls.Add(this.lblPreMarket);
            this.grpVenueDetails.Controls.Add(this.label10);
            this.grpVenueDetails.Controls.Add(this.chkRegularMarket);
            this.grpVenueDetails.Controls.Add(this.chkPostMarket);
            this.grpVenueDetails.Controls.Add(this.chkLunchTime);
            this.grpVenueDetails.Controls.Add(this.chkPreMarket);
            this.grpVenueDetails.Controls.Add(this.label9);
            this.grpVenueDetails.Controls.Add(this.label1);
            this.grpVenueDetails.Controls.Add(this.label4);
            this.grpVenueDetails.Controls.Add(this.label5);
            this.grpVenueDetails.Controls.Add(this.label6);
            this.grpVenueDetails.Controls.Add(this.label7);
            this.grpVenueDetails.Controls.Add(this.label8);
            this.grpVenueDetails.Controls.Add(this.label11);
            this.grpVenueDetails.Controls.Add(this.label14);
            this.grpVenueDetails.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpVenueDetails.Location = new System.Drawing.Point(2, 100);
            this.grpVenueDetails.Name = "grpVenueDetails";
            this.grpVenueDetails.Size = new System.Drawing.Size(372, 306);
            this.grpVenueDetails.TabIndex = 1;
            this.grpVenueDetails.TabStop = false;
            this.grpVenueDetails.Text = "Details";
            // 
            // cmbTimeZone
            // 
            this.cmbTimeZone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbTimeZone.Location = new System.Drawing.Point(188, 16);
            this.cmbTimeZone.Name = "cmbTimeZone";
            this.cmbTimeZone.Size = new System.Drawing.Size(166, 20);
            this.cmbTimeZone.TabIndex = 1;
            this.cmbTimeZone.Text = "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi";
            this.cmbTimeZone.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTimeZone.GotFocus += new System.EventHandler(this.cmbTimeZone_GotFocus);
            this.cmbTimeZone.LostFocus += new System.EventHandler(this.cmbTimeZone_LostFocus);
            // 
            // lblPostMarket
            // 
            this.lblPostMarket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPostMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPostMarket.Location = new System.Drawing.Point(8, 238);
            this.lblPostMarket.Name = "lblPostMarket";
            this.lblPostMarket.Size = new System.Drawing.Size(84, 12);
            this.lblPostMarket.TabIndex = 67;
            this.lblPostMarket.Text = "Post Market";
            // 
            // lblLunchTime
            // 
            this.lblLunchTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLunchTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLunchTime.Location = new System.Drawing.Point(8, 168);
            this.lblLunchTime.Name = "lblLunchTime";
            this.lblLunchTime.Size = new System.Drawing.Size(74, 12);
            this.lblLunchTime.TabIndex = 66;
            this.lblLunchTime.Text = "Lunch Time";
            // 
            // lblRegularTime
            // 
            this.lblRegularTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRegularTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRegularTime.Location = new System.Drawing.Point(8, 104);
            this.lblRegularTime.Name = "lblRegularTime";
            this.lblRegularTime.Size = new System.Drawing.Size(80, 12);
            this.lblRegularTime.TabIndex = 65;
            this.lblRegularTime.Text = "Regular Time";
            // 
            // lblPreMarket
            // 
            this.lblPreMarket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPreMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPreMarket.Location = new System.Drawing.Point(8, 42);
            this.lblPreMarket.Name = "lblPreMarket";
            this.lblPreMarket.Size = new System.Drawing.Size(64, 12);
            this.lblPreMarket.TabIndex = 64;
            this.lblPreMarket.Text = "Pre Market";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(8, 146);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 12);
            this.label10.TabIndex = 62;
            this.label10.Text = "Regular Market End Time(Local)";
            // 
            // chkRegularMarket
            // 
            this.chkRegularMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkRegularMarket.Location = new System.Drawing.Point(186, 98);
            this.chkRegularMarket.Name = "chkRegularMarket";
            this.chkRegularMarket.Size = new System.Drawing.Size(16, 24);
            this.chkRegularMarket.TabIndex = 5;
            this.chkRegularMarket.Text = "checkBox1";
            this.chkRegularMarket.CheckStateChanged += new System.EventHandler(this.chkRegularMarket_CheckStateChanged);
            // 
            // chkPostMarket
            // 
            this.chkPostMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkPostMarket.Location = new System.Drawing.Point(186, 232);
            this.chkPostMarket.Name = "chkPostMarket";
            this.chkPostMarket.Size = new System.Drawing.Size(16, 24);
            this.chkPostMarket.TabIndex = 12;
            this.chkPostMarket.Text = "checkBox4";
            this.chkPostMarket.CheckStateChanged += new System.EventHandler(this.chkPostMarket_CheckStateChanged);
            // 
            // chkLunchTime
            // 
            this.chkLunchTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkLunchTime.Location = new System.Drawing.Point(186, 162);
            this.chkLunchTime.Name = "chkLunchTime";
            this.chkLunchTime.Size = new System.Drawing.Size(16, 24);
            this.chkLunchTime.TabIndex = 8;
            this.chkLunchTime.Text = "checkBox3";
            this.chkLunchTime.CheckStateChanged += new System.EventHandler(this.chkLunchTime_CheckStateChanged);
            // 
            // chkPreMarket
            // 
            this.chkPreMarket.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkPreMarket.Location = new System.Drawing.Point(186, 36);
            this.chkPreMarket.Name = "chkPreMarket";
            this.chkPreMarket.Size = new System.Drawing.Size(16, 24);
            this.chkPreMarket.TabIndex = 2;
            this.chkPreMarket.Text = "checkBox1";
            this.chkPreMarket.CheckStateChanged += new System.EventHandler(this.chkPreMarket_CheckStateChanged);
            // 
            // label9
            // 
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(8, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(170, 12);
            this.label9.TabIndex = 57;
            this.label9.Text = "Regular Market Start Time(Local)";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(144, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 56;
            this.label1.Text = "*";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(8, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 12);
            this.label4.TabIndex = 47;
            this.label4.Text = "Pre-Market End Time(Local)";
            // 
            // label5
            // 
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(8, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 12);
            this.label5.TabIndex = 45;
            this.label5.Text = "Pre-Market Start Time(Local)";
            // 
            // label6
            // 
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(8, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Relationship to GMT (+/-)";
            // 
            // label7
            // 
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(8, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 12);
            this.label7.TabIndex = 42;
            this.label7.Text = "Lunch Start Time(Local)";
            // 
            // label8
            // 
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(8, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 12);
            this.label8.TabIndex = 46;
            this.label8.Text = "Lunch End Time(Local)";
            // 
            // label11
            // 
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(8, 282);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 12);
            this.label11.TabIndex = 43;
            this.label11.Text = "Post-Market End Time(Local)";
            // 
            // label14
            // 
            this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label14.Location = new System.Drawing.Point(8, 258);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(158, 12);
            this.label14.TabIndex = 55;
            this.label14.Text = "Post-Market Start Time(Local)";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // txtPreMarketTradingStartTime
            // 
            this.txtPreMarketTradingStartTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPreMarketTradingStartTime.Location = new System.Drawing.Point(186, 58);
            this.txtPreMarketTradingStartTime.Name = "txtPreMarketTradingStartTime";
            this.txtPreMarketTradingStartTime.Size = new System.Drawing.Size(165, 21);
            this.txtPreMarketTradingStartTime.TabIndex = 3;
            this.txtPreMarketTradingStartTime.Time = "0:0";
            // 
            // txtPreMarketTradingEndTime
            // 
            this.txtPreMarketTradingEndTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPreMarketTradingEndTime.Location = new System.Drawing.Point(186, 80);
            this.txtPreMarketTradingEndTime.Name = "txtPreMarketTradingEndTime";
            this.txtPreMarketTradingEndTime.Size = new System.Drawing.Size(165, 21);
            this.txtPreMarketTradingEndTime.TabIndex = 4;
            this.txtPreMarketTradingEndTime.Time = "0:0";
            // 
            // txtRegularTradingEndTime
            // 
            this.txtRegularTradingEndTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtRegularTradingEndTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtRegularTradingEndTime.ForeColor = System.Drawing.Color.Black;
            this.txtRegularTradingEndTime.Location = new System.Drawing.Point(186, 142);
            this.txtRegularTradingEndTime.Name = "txtRegularTradingEndTime";
            this.txtRegularTradingEndTime.Size = new System.Drawing.Size(165, 21);
            this.txtRegularTradingEndTime.TabIndex = 7;
            this.txtRegularTradingEndTime.Time = "0:0";
            // 
            // txtRegularTradingStartTime
            // 
            this.txtRegularTradingStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtRegularTradingStartTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtRegularTradingStartTime.ForeColor = System.Drawing.Color.Black;
            this.txtRegularTradingStartTime.Location = new System.Drawing.Point(186, 120);
            this.txtRegularTradingStartTime.Name = "txtRegularTradingStartTime";
            this.txtRegularTradingStartTime.Size = new System.Drawing.Size(165, 21);
            this.txtRegularTradingStartTime.TabIndex = 6;
            this.txtRegularTradingStartTime.Time = "0:0";
            // 
            // txtLunchTimeStartTime
            // 
            this.txtLunchTimeStartTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLunchTimeStartTime.Location = new System.Drawing.Point(186, 184);
            this.txtLunchTimeStartTime.Name = "txtLunchTimeStartTime";
            this.txtLunchTimeStartTime.Size = new System.Drawing.Size(165, 21);
            this.txtLunchTimeStartTime.TabIndex = 10;
            this.txtLunchTimeStartTime.Time = "0:1";
            // 
            // txtLunchTimeEndTime
            // 
            this.txtLunchTimeEndTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLunchTimeEndTime.Location = new System.Drawing.Point(186, 210);
            this.txtLunchTimeEndTime.Name = "txtLunchTimeEndTime";
            this.txtLunchTimeEndTime.Size = new System.Drawing.Size(165, 21);
            this.txtLunchTimeEndTime.TabIndex = 11;
            this.txtLunchTimeEndTime.Time = "1:0";
            // 
            // txtPostMarketTradingStartTime
            // 
            this.txtPostMarketTradingStartTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPostMarketTradingStartTime.Location = new System.Drawing.Point(186, 254);
            this.txtPostMarketTradingStartTime.Name = "txtPostMarketTradingStartTime";
            this.txtPostMarketTradingStartTime.Size = new System.Drawing.Size(165, 21);
            this.txtPostMarketTradingStartTime.TabIndex = 13;
            this.txtPostMarketTradingStartTime.Time = "0:0";
            // 
            // txtPostMarketTradingEndTime
            // 
            this.txtPostMarketTradingEndTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPostMarketTradingEndTime.Location = new System.Drawing.Point(186, 278);
            this.txtPostMarketTradingEndTime.Name = "txtPostMarketTradingEndTime";
            this.txtPostMarketTradingEndTime.Size = new System.Drawing.Size(165, 21);
            this.txtPostMarketTradingEndTime.TabIndex = 14;
            this.txtPostMarketTradingEndTime.Time = "0:0";
            // 
            // CompanyVenue
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpVenueDetails);
            this.Controls.Add(this.grpCompanyVenue);
            this.Name = "CompanyVenue";
            this.Size = new System.Drawing.Size(378, 412);
            this.grpCompanyVenue.ResumeLayout(false);
            this.grpCompanyVenue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenueType)).EndInit();
            this.grpVenueDetails.ResumeLayout(false);
            this.grpVenueDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        #region Focus
        private void txtVenueName_GotFocus(object sender, System.EventArgs e)
        {
            txtVenueName.BackColor = Color.LemonChiffon;
        }
        private void txtVenueName_LostFocus(object sender, System.EventArgs e)
        {
            txtVenueName.BackColor = Color.White;
        }
        private void txtVenueShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtVenueShortName.BackColor = Color.LemonChiffon;
        }
        private void txtVenueShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtVenueShortName.BackColor = Color.White;
        }
        private void cmbVenueType_GotFocus(object sender, System.EventArgs e)
        {
            cmbVenueType.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbVenueType_LostFocus(object sender, System.EventArgs e)
        {
            cmbVenueType.Appearance.BackColor = Color.White;
        }
        private void cmbTimeZone_GotFocus(object sender, System.EventArgs e)
        {
            cmbTimeZone.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbTimeZone_LostFocus(object sender, System.EventArgs e)
        {
            cmbTimeZone.Appearance.BackColor = Color.White;
        }
        #endregion


        private void BindVenueTypes()
        {
            VenueTypes venueTypes = VenueManager.GetVenueTypes();
            if (venueTypes.Count > 0)
            {
                venueTypes.Insert(0, new VenueType(int.MinValue, C_COMBO_SELECT));
                cmbVenueType.DataSource = null;
                cmbVenueType.DataSource = venueTypes;

                cmbVenueType.DisplayMember = "Type";
                cmbVenueType.ValueMember = "VenueTypeID";
                cmbVenueType.Text = C_COMBO_SELECT;
            }
        }

        public Prana.Admin.BLL.Company CompanyVenueProperty
        {
            get
            {
                Prana.Admin.BLL.Company companyVenue = new Prana.Admin.BLL.Company();
                //GetCompanyVenueDetails(companyVenue);
                return companyVenue;
            }
            //			set 
            //			{
            //				SetCompanyVenueDetails(value);
            //			}
        }

        int _companyID = int.MinValue;
        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            BindVenueTypes();
            SetCompanyVenueDetails();
        }

        public Prana.Admin.BLL.Company GetCompanyVenueDetails()
        {
            Prana.Admin.BLL.Company companyVenueDetails = null;

            errorProvider1.SetError(txtVenueName, "");
            errorProvider1.SetError(txtVenueShortName, "");
            errorProvider1.SetError(cmbVenueType, "");
            errorProvider1.SetError(cmbTimeZone, "");

            #region Check if the fields have atleast one item selected
            if (txtVenueName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtVenueName, "Please enter Venue Name!");
                txtVenueName.Focus();
            }
            else if (txtVenueShortName.Text.ToString() == "")
            {
                errorProvider1.SetError(txtVenueShortName, "Please select Venue Short Name!");
                txtVenueShortName.Focus();
            }
            else if (int.Parse(cmbVenueType.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbVenueType, "Please Select Venue Type!");
                cmbVenueType.Focus();
            }
            else if (cmbTimeZone.Text.Trim() == "")
            {
                errorProvider1.SetError(cmbTimeZone, "Please Select Time Zone!");
                cmbTimeZone.Focus();
            }
            #endregion
            else
            {
                companyVenueDetails = new Prana.Admin.BLL.Company();
                companyVenueDetails.VenueName = txtVenueName.Text.Trim().ToString();
                companyVenueDetails.VenueShortName = txtVenueShortName.Text.Trim().ToString();
                companyVenueDetails.VenueType = int.Parse(cmbVenueType.Value.ToString());


                companyVenueDetails.TimeZone = cmbTimeZone.SelectedIndex;

                //If this checkbox is checked then only the PreMarketTradingStartTime's value is stored
                //else for the time being some temporary value equivalent to null is stored in the database, 
                //which in our case is year 2000.
                if (chkPreMarket.Checked == true)
                {
                    companyVenueDetails.PreMarketTradingStartTime = DateTime.Parse(txtPreMarketTradingStartTime.Time);
                    companyVenueDetails.PreMarketTradingEndTime = DateTime.Parse(txtPreMarketTradingEndTime.Time);
                }
                else
                {
                    companyVenueDetails.PreMarketTradingStartTime = DateTime.Parse(NULLYEAR);
                    companyVenueDetails.PreMarketTradingEndTime = DateTime.Parse(NULLYEAR);
                }

                if (chkLunchTime.Checked == true)
                {
                    companyVenueDetails.LunchTimeStartTime = DateTime.Parse(txtLunchTimeStartTime.Time);
                    companyVenueDetails.LunchTimeEndTime = DateTime.Parse(txtLunchTimeEndTime.Time);
                }
                else
                {
                    companyVenueDetails.LunchTimeStartTime = DateTime.Parse(NULLYEAR);
                    companyVenueDetails.LunchTimeEndTime = DateTime.Parse(NULLYEAR);
                }

                if (chkRegularMarket.Checked == true)
                {
                    companyVenueDetails.RegularTradingStartTime = DateTime.Parse(txtRegularTradingStartTime.Time);
                    companyVenueDetails.RegularTradingEndTime = DateTime.Parse(txtRegularTradingEndTime.Time);
                }
                else
                {
                    companyVenueDetails.RegularTradingStartTime = DateTime.Parse(NULLYEAR);
                    companyVenueDetails.RegularTradingEndTime = DateTime.Parse(NULLYEAR);
                }

                //If this checkbox is checked then only the chkPostMarketStartTime's value is stored
                //else for the time being some temporary value equivalent to null is stored in the database, 
                //which in our case is year 2000.
                if (chkPostMarket.Checked == true)
                {
                    companyVenueDetails.PostMarketTradingStartTime = DateTime.Parse(txtPostMarketTradingStartTime.Time);
                    companyVenueDetails.PostMarketTradingEndTime = DateTime.Parse(txtPostMarketTradingEndTime.Time);
                }
                else
                {
                    companyVenueDetails.PostMarketTradingStartTime = DateTime.Parse(NULLYEAR);
                    companyVenueDetails.PostMarketTradingEndTime = DateTime.Parse(NULLYEAR);
                }

                companyVenueDetails.PreMarketCheck = (chkPreMarket.Checked == true ? 1 : 0);
                companyVenueDetails.PostMarketCheck = (chkPostMarket.Checked == true ? 1 : 0);

                companyVenueDetails.RegularTimeCheck = (chkRegularMarket.Checked == true ? 1 : 0);
                companyVenueDetails.LunchTimeCheck = (chkLunchTime.Checked == true ? 1 : 0);

            }
            return companyVenueDetails;
        }

        private void SetCompanyVenueDetails()
        {
            Prana.Admin.BLL.Company companyVenueDetail = CompanyManager.GetCompanyVenueDetails(_companyID);

            txtVenueName.Text = companyVenueDetail.VenueName.ToString();
            txtVenueShortName.Text = companyVenueDetail.VenueShortName.ToString();
            cmbVenueType.Value = int.Parse(companyVenueDetail.VenueType.ToString());

            chkPreMarket.Checked = (int.Parse(companyVenueDetail.PreMarketCheck.ToString()) == 1 ? true : false);
            chkPostMarket.Checked = (int.Parse(companyVenueDetail.PostMarketCheck.ToString()) == 1 ? true : false);
            chkRegularMarket.Checked = (int.Parse(companyVenueDetail.RegularTimeCheck.ToString()) == 1 ? true : false);
            chkLunchTime.Checked = (int.Parse(companyVenueDetail.LunchTimeCheck.ToString()) == 1 ? true : false);

            cmbTimeZone.SelectedIndex = int.Parse(companyVenueDetail.TimeZone.ToString());

            if (chkPreMarket.Checked == true)
            {
                txtPreMarketTradingStartTime.Time = companyVenueDetail.PreMarketTradingStartTime.Hour + ":" + companyVenueDetail.PreMarketTradingStartTime.Minute;
                txtPreMarketTradingEndTime.Time = companyVenueDetail.PreMarketTradingEndTime.Hour + ":" + companyVenueDetail.PreMarketTradingEndTime.Minute;
            }
            else
            {
                txtPreMarketTradingStartTime.Time = companyVenueDetail.PreMarketTradingStartTime.Hour + ":" + companyVenueDetail.PreMarketTradingStartTime.Minute;
                txtPreMarketTradingEndTime.Time = companyVenueDetail.PreMarketTradingEndTime.Hour + ":" + companyVenueDetail.PreMarketTradingEndTime.Minute;
                txtPreMarketTradingStartTime.Enabled = false;
                txtPreMarketTradingEndTime.Enabled = false;
            }

            if (chkLunchTime.Checked == true)
            {
                txtLunchTimeStartTime.Time = companyVenueDetail.LunchTimeStartTime.Hour + ":" + companyVenueDetail.LunchTimeStartTime.Minute;
                txtLunchTimeEndTime.Time = companyVenueDetail.LunchTimeEndTime.Hour + ":" + companyVenueDetail.LunchTimeEndTime.Minute;
            }
            else
            {
                txtLunchTimeStartTime.Time = companyVenueDetail.LunchTimeStartTime.Hour + ":" + companyVenueDetail.LunchTimeStartTime.Minute;
                txtLunchTimeEndTime.Time = companyVenueDetail.LunchTimeEndTime.Hour + ":" + companyVenueDetail.LunchTimeEndTime.Minute;
                txtLunchTimeStartTime.Enabled = false;
                txtLunchTimeEndTime.Enabled = false;
            }

            if (chkRegularMarket.Checked == true)
            {
                txtRegularTradingStartTime.Time = companyVenueDetail.RegularTradingStartTime.Hour + ":" + companyVenueDetail.RegularTradingStartTime.Minute;
                txtRegularTradingEndTime.Time = companyVenueDetail.RegularTradingEndTime.Hour + ":" + companyVenueDetail.RegularTradingEndTime.Minute;
            }
            else
            {
                txtRegularTradingStartTime.Time = companyVenueDetail.RegularTradingStartTime.Hour + ":" + companyVenueDetail.RegularTradingStartTime.Minute;
                txtRegularTradingEndTime.Time = companyVenueDetail.RegularTradingEndTime.Hour + ":" + companyVenueDetail.RegularTradingEndTime.Minute;
                txtRegularTradingStartTime.Enabled = false;
                txtRegularTradingEndTime.Enabled = false;
            }
            if (chkPostMarket.Checked == true)
            {
                txtPostMarketTradingStartTime.Time = companyVenueDetail.PostMarketTradingStartTime.Hour + ":" + companyVenueDetail.PostMarketTradingStartTime.Minute;
                txtPostMarketTradingEndTime.Time = companyVenueDetail.PostMarketTradingEndTime.Hour + ":" + companyVenueDetail.PostMarketTradingEndTime.Minute;
            }
            else
            {
                txtPostMarketTradingStartTime.Time = companyVenueDetail.PostMarketTradingStartTime.Hour + ":" + companyVenueDetail.PostMarketTradingStartTime.Minute;
                txtPostMarketTradingEndTime.Time = companyVenueDetail.PostMarketTradingEndTime.Hour + ":" + companyVenueDetail.PostMarketTradingEndTime.Minute;
                txtPostMarketTradingStartTime.Enabled = false;
                txtPostMarketTradingEndTime.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the TextBox-txtPreMarketTradingStartTime when CheckBox-chkPreMarketStartTime's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPreMarket_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkPreMarket.Checked == true)
            {
                txtPreMarketTradingStartTime.Enabled = true;
                txtPreMarketTradingEndTime.Enabled = true;
            }
            else
            {
                txtPreMarketTradingStartTime.Enabled = false;
                txtPreMarketTradingEndTime.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the TextBox-txtRegularTradingStartTime when CheckBox-chkRegularMarket's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRegularMarket_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkRegularMarket.Checked == true)
            {
                txtRegularTradingStartTime.Enabled = true;
                txtRegularTradingEndTime.Enabled = true;
            }
            else
            {
                txtRegularTradingStartTime.Enabled = false;
                txtRegularTradingEndTime.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the TextBox-txtLunchTimeStartTime when CheckBox-chkLunchTime's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLunchTime_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkLunchTime.Checked == true)
            {
                txtLunchTimeStartTime.Enabled = true;
                txtLunchTimeEndTime.Enabled = true;
            }
            else
            {
                txtLunchTimeStartTime.Enabled = false;
                txtLunchTimeEndTime.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the TextBox-txtPostMarketTradingEndTime when CheckBox-chkPostMarketEndTime's state is 
        /// turned on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPostMarket_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (chkPostMarket.Checked == true)
            {
                txtPostMarketTradingStartTime.Enabled = true;
                txtPostMarketTradingEndTime.Enabled = true;
            }
            else
            {
                txtPostMarketTradingStartTime.Enabled = false;
                txtPostMarketTradingEndTime.Enabled = false;
            }
        }
    }
}
