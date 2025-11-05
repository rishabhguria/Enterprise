using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;
using Infragistics.Win.UltraWinGrid;
using System.Data;
using System.IO;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for Exchange.
	/// </summary>
	public class ExchangeForm : System.Windows.Forms.Form
	{
		#region Constant Declarations
		const string C_COMBO_SELECT = "- Select -";
		private const string FORM_NAME = "Exchange Form : ";

		private const string NULLYEAR = "10/10/2000";
		#endregion

		#region declaration

		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
		private System.Windows.Forms.StatusBar stbExchange;
		private System.Windows.Forms.TreeView trvExchange;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.ErrorProvider errorProvider2;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource3;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource4;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Button btnUploadHolidays;
		private System.Windows.Forms.Label label59;
		private System.Windows.Forms.Label label58;
		private System.Windows.Forms.Label label57;
		private System.Windows.Forms.DateTimePicker dttHolidayDate;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.TextBox txtHolidayDescription;
		private System.Windows.Forms.Button btnAddHoliday;
		private System.Windows.Forms.Button btnDeleteHoliday;
		private System.Windows.Forms.Label label15;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMasterExchangeHolidays;
		private System.Windows.Forms.OpenFileDialog openFileDialogFlag;
		private System.Windows.Forms.Panel panel1;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdHoliday;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.Misc.UltraLabel ultraLabel4;
		private Infragistics.Win.Misc.UltraLabel ultraLabel5;
		private Infragistics.Win.Misc.UltraLabel ultraLabel6;
		private Infragistics.Win.Misc.UltraLabel ultraLabel7;
		private Infragistics.Win.Misc.UltraLabel ultraLabel8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private Infragistics.Win.Misc.UltraLabel ultraLabel9;
		private Infragistics.Win.Misc.UltraLabel ultraLabel10;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private Infragistics.Win.Misc.UltraLabel ultraLabel15;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCountry;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbState;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbFlag;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbExchangeLogo;
		private System.Windows.Forms.CheckBox chkLunchTime;
		private System.Windows.Forms.CheckBox chkRegularTime;
		private System.Windows.Forms.DateTimePicker timeRMET;
		private System.Windows.Forms.DateTimePicker timeRMST;
		private System.Windows.Forms.DateTimePicker timeLunchET;
		private System.Windows.Forms.DateTimePicker timeLunchST;
		private Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor cmbTimeZone;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtShortName;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtExchangeNameFull;
		private System.Windows.Forms.Button btnUpoadExchangeLogo;
		private System.Windows.Forms.Button btnUploadFlag;
		private System.Windows.Forms.Label label8;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion


		public ExchangeForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExchangeForm));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryFlagID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryFlagName", 1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryFlagImage", 2);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LogoID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LogoName", 1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LogoImage", 2);
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 0);
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 1);
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 0);
			Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 1);
			Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateName", 2);
			Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Date", 0);
			Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
			Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnUpoadExchangeLogo = new System.Windows.Forms.Button();
			this.btnUploadFlag = new System.Windows.Forms.Button();
			this.cmbFlag = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbExchangeLogo = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.label4 = new System.Windows.Forms.Label();
			this.chkLunchTime = new System.Windows.Forms.CheckBox();
			this.chkRegularTime = new System.Windows.Forms.CheckBox();
			this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
			this.timeRMET = new System.Windows.Forms.DateTimePicker();
			this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
			this.timeRMST = new System.Windows.Forms.DateTimePicker();
			this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.timeLunchET = new System.Windows.Forms.DateTimePicker();
			this.timeLunchST = new System.Windows.Forms.DateTimePicker();
			this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
			this.cmbTimeZone = new Infragistics.Win.UltraWinEditors.UltraTimeZoneEditor();
			this.cmbCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel15 = new Infragistics.Win.Misc.UltraLabel();
			this.cmbState = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtShortName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.txtExchangeNameFull = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.grdHoliday = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.btnUploadHolidays = new System.Windows.Forms.Button();
			this.label59 = new System.Windows.Forms.Label();
			this.label58 = new System.Windows.Forms.Label();
			this.dttHolidayDate = new System.Windows.Forms.DateTimePicker();
			this.label40 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.txtHolidayDescription = new System.Windows.Forms.TextBox();
			this.btnAddHoliday = new System.Windows.Forms.Button();
			this.btnDeleteHoliday = new System.Windows.Forms.Button();
			this.label57 = new System.Windows.Forms.Label();
			this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource();
			this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource();
			this.stbExchange = new System.Windows.Forms.StatusBar();
			this.trvExchange = new System.Windows.Forms.TreeView();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.errorProvider2 = new System.Windows.Forms.ErrorProvider();
			this.ultraDataSource3 = new Infragistics.Win.UltraWinDataSource.UltraDataSource();
			this.ultraDataSource4 = new Infragistics.Win.UltraWinDataSource.UltraDataSource();
			this.tabMasterExchangeHolidays = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.openFileDialogFlag = new System.Windows.Forms.OpenFileDialog();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.ultraTabPageControl1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbFlag)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbExchangeLogo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtShortName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtExchangeNameFull)).BeginInit();
			this.ultraTabPageControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).BeginInit();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tabMasterExchangeHolidays)).BeginInit();
			this.tabMasterExchangeHolidays.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Controls.Add(this.groupBox2);
			this.ultraTabPageControl1.Controls.Add(this.groupBox1);
			this.ultraTabPageControl1.Controls.Add(this.label15);
			this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			this.ultraTabPageControl1.Size = new System.Drawing.Size(544, 377);
			this.ultraTabPageControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.ultraTabPageControl1_Paint);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.groupBox2.Controls.Add(this.btnUpoadExchangeLogo);
			this.groupBox2.Controls.Add(this.btnUploadFlag);
			this.groupBox2.Controls.Add(this.cmbFlag);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.cmbExchangeLogo);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.chkLunchTime);
			this.groupBox2.Controls.Add(this.chkRegularTime);
			this.groupBox2.Controls.Add(this.ultraLabel3);
			this.groupBox2.Controls.Add(this.timeRMET);
			this.groupBox2.Controls.Add(this.ultraLabel4);
			this.groupBox2.Controls.Add(this.ultraLabel5);
			this.groupBox2.Controls.Add(this.timeRMST);
			this.groupBox2.Controls.Add(this.ultraLabel6);
			this.groupBox2.Controls.Add(this.ultraLabel7);
			this.groupBox2.Controls.Add(this.ultraLabel8);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.timeLunchET);
			this.groupBox2.Controls.Add(this.timeLunchST);
			this.groupBox2.Controls.Add(this.ultraLabel9);
			this.groupBox2.Controls.Add(this.ultraLabel10);
			this.groupBox2.Controls.Add(this.cmbTimeZone);
			this.groupBox2.Controls.Add(this.cmbCountry);
			this.groupBox2.Controls.Add(this.ultraLabel11);
			this.groupBox2.Controls.Add(this.ultraLabel12);
			this.groupBox2.Controls.Add(this.ultraLabel13);
			this.groupBox2.Controls.Add(this.ultraLabel14);
			this.groupBox2.Controls.Add(this.ultraLabel15);
			this.groupBox2.Controls.Add(this.cmbState);
			this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox2.Location = new System.Drawing.Point(38, 78);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(466, 278);
			this.groupBox2.TabIndex = 48;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Trading Hours";
			this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
			// 
			// btnUpoadExchangeLogo
			// 
			this.btnUpoadExchangeLogo.BackColor = System.Drawing.Color.Thistle;
			this.btnUpoadExchangeLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpoadExchangeLogo.BackgroundImage")));
			this.btnUpoadExchangeLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnUpoadExchangeLogo.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnUpoadExchangeLogo.Location = new System.Drawing.Point(370, 244);
			this.btnUpoadExchangeLogo.Name = "btnUpoadExchangeLogo";
			this.btnUpoadExchangeLogo.Size = new System.Drawing.Size(74, 23);
			this.btnUpoadExchangeLogo.TabIndex = 143;
			this.btnUpoadExchangeLogo.Click += new System.EventHandler(this.btnUpoadExchangeLogo_Click);
			// 
			// btnUploadFlag
			// 
			this.btnUploadFlag.BackColor = System.Drawing.Color.Thistle;
			this.btnUploadFlag.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUploadFlag.BackgroundImage")));
			this.btnUploadFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnUploadFlag.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnUploadFlag.Location = new System.Drawing.Point(370, 220);
			this.btnUploadFlag.Name = "btnUploadFlag";
			this.btnUploadFlag.Size = new System.Drawing.Size(74, 23);
			this.btnUploadFlag.TabIndex = 142;
			this.btnUploadFlag.Click += new System.EventHandler(this.btnUploadFlag_Click);
			// 
			// cmbFlag
			// 
			this.cmbFlag.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			this.cmbFlag.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand1.ColHeadersVisible = false;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3});
			this.cmbFlag.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.cmbFlag.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbFlag.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance1.BorderColor = System.Drawing.Color.Silver;
			this.cmbFlag.DisplayLayout.Override.RowAppearance = appearance1;
			this.cmbFlag.DisplayMember = "";
			this.cmbFlag.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbFlag.DropDownWidth = 0;
			this.cmbFlag.FlatMode = true;
			this.cmbFlag.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbFlag.LimitToList = true;
			this.cmbFlag.Location = new System.Drawing.Point(186, 226);
			this.cmbFlag.Name = "cmbFlag";
			this.cmbFlag.Size = new System.Drawing.Size(170, 20);
			this.cmbFlag.TabIndex = 141;
			this.cmbFlag.ValueMember = "";
			this.cmbFlag.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbFlag_InitializeLayout);
			this.cmbFlag.LostFocus += new System.EventHandler(this.cmbFlag_LostFocus);
			this.cmbFlag.GotFocus += new System.EventHandler(this.cmbFlag_GotFocus);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.Location = new System.Drawing.Point(10, 251);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 14);
			this.label3.TabIndex = 140;
			this.label3.Text = "ExchangeLogo";
			// 
			// cmbExchangeLogo
			// 
			this.cmbExchangeLogo.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			this.cmbExchangeLogo.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand2.ColHeadersVisible = false;
			ultraGridColumn4.Header.VisiblePosition = 0;
			ultraGridColumn4.Hidden = true;
			ultraGridColumn5.Header.VisiblePosition = 1;
			ultraGridColumn6.Header.VisiblePosition = 2;
			ultraGridBand2.Columns.AddRange(new object[] {
															 ultraGridColumn4,
															 ultraGridColumn5,
															 ultraGridColumn6});
			this.cmbExchangeLogo.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
			this.cmbExchangeLogo.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbExchangeLogo.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance2.BorderColor = System.Drawing.Color.Silver;
			this.cmbExchangeLogo.DisplayLayout.Override.RowAppearance = appearance2;
			this.cmbExchangeLogo.DisplayMember = "";
			this.cmbExchangeLogo.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbExchangeLogo.DropDownWidth = 0;
			this.cmbExchangeLogo.FlatMode = true;
			this.cmbExchangeLogo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbExchangeLogo.LimitToList = true;
			this.cmbExchangeLogo.Location = new System.Drawing.Point(186, 248);
			this.cmbExchangeLogo.Name = "cmbExchangeLogo";
			this.cmbExchangeLogo.Size = new System.Drawing.Size(170, 20);
			this.cmbExchangeLogo.TabIndex = 139;
			this.cmbExchangeLogo.ValueMember = "";
			this.cmbExchangeLogo.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbExchangeLogo_InitializeLayout);
			this.cmbExchangeLogo.LostFocus += new System.EventHandler(this.cmbExchangeLogo_LostFocus);
			this.cmbExchangeLogo.GotFocus += new System.EventHandler(this.cmbExchangeLogo_GotFocus);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label4.Location = new System.Drawing.Point(10, 230);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(26, 13);
			this.label4.TabIndex = 138;
			this.label4.Text = "Flag";
			// 
			// chkLunchTime
			// 
			this.chkLunchTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.chkLunchTime.Location = new System.Drawing.Point(186, 114);
			this.chkLunchTime.Name = "chkLunchTime";
			this.chkLunchTime.Size = new System.Drawing.Size(18, 16);
			this.chkLunchTime.TabIndex = 137;
			this.chkLunchTime.CheckStateChanged += new System.EventHandler(this.chkLunchTime_CheckStateChanged);
			// 
			// chkRegularTime
			// 
			this.chkRegularTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.chkRegularTime.Location = new System.Drawing.Point(186, 44);
			this.chkRegularTime.Name = "chkRegularTime";
			this.chkRegularTime.Size = new System.Drawing.Size(20, 18);
			this.chkRegularTime.TabIndex = 136;
			this.chkRegularTime.CheckStateChanged += new System.EventHandler(this.chkRegularTime_CheckStateChanged);
			// 
			// ultraLabel3
			// 
			appearance3.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel3.Appearance = appearance3;
			this.ultraLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel3.Location = new System.Drawing.Point(10, 115);
			this.ultraLabel3.Name = "ultraLabel3";
			this.ultraLabel3.Size = new System.Drawing.Size(66, 14);
			this.ultraLabel3.TabIndex = 135;
			this.ultraLabel3.Text = "Lunch Time";
			// 
			// timeRMET
			// 
			this.timeRMET.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.timeRMET.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.timeRMET.Location = new System.Drawing.Point(186, 90);
			this.timeRMET.Name = "timeRMET";
			this.timeRMET.ShowUpDown = true;
			this.timeRMET.Size = new System.Drawing.Size(90, 21);
			this.timeRMET.TabIndex = 132;
			this.timeRMET.Value = new System.DateTime(2005, 7, 22, 23, 23, 20, 687);
			// 
			// ultraLabel4
			// 
			appearance4.TextHAlign = Infragistics.Win.HAlign.Left;
			appearance4.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel4.Appearance = appearance4;
			this.ultraLabel4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			appearance5.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel4.HotTrackAppearance = appearance5;
			this.ultraLabel4.Location = new System.Drawing.Point(282, 90);
			this.ultraLabel4.Name = "ultraLabel4";
			this.ultraLabel4.Size = new System.Drawing.Size(50, 20);
			this.ultraLabel4.TabIndex = 133;
			this.ultraLabel4.Text = "hh:mm";
			// 
			// ultraLabel5
			// 
			appearance6.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel5.Appearance = appearance6;
			this.ultraLabel5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel5.Location = new System.Drawing.Point(10, 94);
			this.ultraLabel5.Name = "ultraLabel5";
			this.ultraLabel5.Size = new System.Drawing.Size(166, 12);
			this.ultraLabel5.TabIndex = 134;
			this.ultraLabel5.Text = "Regular Market End Time(Local)";
			// 
			// timeRMST
			// 
			this.timeRMST.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.timeRMST.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.timeRMST.Location = new System.Drawing.Point(186, 66);
			this.timeRMST.Name = "timeRMST";
			this.timeRMST.ShowUpDown = true;
			this.timeRMST.Size = new System.Drawing.Size(90, 21);
			this.timeRMST.TabIndex = 129;
			this.timeRMST.Value = new System.DateTime(2005, 7, 22, 23, 23, 20, 687);
			// 
			// ultraLabel6
			// 
			appearance7.TextHAlign = Infragistics.Win.HAlign.Left;
			appearance7.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel6.Appearance = appearance7;
			this.ultraLabel6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			appearance8.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel6.HotTrackAppearance = appearance8;
			this.ultraLabel6.Location = new System.Drawing.Point(282, 68);
			this.ultraLabel6.Name = "ultraLabel6";
			this.ultraLabel6.Size = new System.Drawing.Size(50, 14);
			this.ultraLabel6.TabIndex = 130;
			this.ultraLabel6.Text = "hh:mm";
			// 
			// ultraLabel7
			// 
			appearance9.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel7.Appearance = appearance9;
			this.ultraLabel7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
			this.ultraLabel7.HotTrackAppearance = appearance10;
			this.ultraLabel7.Location = new System.Drawing.Point(10, 70);
			this.ultraLabel7.Name = "ultraLabel7";
			this.ultraLabel7.Size = new System.Drawing.Size(170, 12);
			this.ultraLabel7.TabIndex = 131;
			this.ultraLabel7.Text = "Regular Market Start Time(Local)";
			// 
			// ultraLabel8
			// 
			appearance11.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel8.Appearance = appearance11;
			this.ultraLabel8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel8.Location = new System.Drawing.Point(10, 46);
			this.ultraLabel8.Name = "ultraLabel8";
			this.ultraLabel8.Size = new System.Drawing.Size(72, 14);
			this.ultraLabel8.TabIndex = 128;
			this.ultraLabel8.Text = "RegularTime";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(144, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 6);
			this.label5.TabIndex = 127;
			this.label5.Text = "*";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label6.ForeColor = System.Drawing.Color.Red;
			this.label6.Location = new System.Drawing.Point(44, 208);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(12, 8);
			this.label6.TabIndex = 126;
			this.label6.Text = "*";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label7.ForeColor = System.Drawing.Color.Red;
			this.label7.Location = new System.Drawing.Point(60, 186);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(12, 6);
			this.label7.TabIndex = 125;
			this.label7.Text = "*";
			// 
			// timeLunchET
			// 
			this.timeLunchET.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.timeLunchET.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.timeLunchET.Location = new System.Drawing.Point(186, 156);
			this.timeLunchET.Name = "timeLunchET";
			this.timeLunchET.ShowUpDown = true;
			this.timeLunchET.Size = new System.Drawing.Size(90, 21);
			this.timeLunchET.TabIndex = 115;
			this.timeLunchET.Value = new System.DateTime(2005, 7, 22, 23, 23, 20, 687);
			// 
			// timeLunchST
			// 
			this.timeLunchST.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.timeLunchST.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.timeLunchST.Location = new System.Drawing.Point(186, 132);
			this.timeLunchST.Name = "timeLunchST";
			this.timeLunchST.ShowUpDown = true;
			this.timeLunchST.Size = new System.Drawing.Size(90, 21);
			this.timeLunchST.TabIndex = 114;
			this.timeLunchST.Value = new System.DateTime(2005, 7, 22, 23, 23, 20, 687);
			// 
			// ultraLabel9
			// 
			appearance12.TextHAlign = Infragistics.Win.HAlign.Left;
			appearance12.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel9.Appearance = appearance12;
			this.ultraLabel9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			appearance13.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel9.HotTrackAppearance = appearance13;
			this.ultraLabel9.Location = new System.Drawing.Point(282, 160);
			this.ultraLabel9.Name = "ultraLabel9";
			this.ultraLabel9.Size = new System.Drawing.Size(56, 20);
			this.ultraLabel9.TabIndex = 118;
			this.ultraLabel9.Text = "hh:mm";
			// 
			// ultraLabel10
			// 
			appearance14.TextHAlign = Infragistics.Win.HAlign.Left;
			appearance14.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel10.Appearance = appearance14;
			this.ultraLabel10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			appearance15.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel10.HotTrackAppearance = appearance15;
			this.ultraLabel10.Location = new System.Drawing.Point(282, 138);
			this.ultraLabel10.Name = "ultraLabel10";
			this.ultraLabel10.Size = new System.Drawing.Size(52, 20);
			this.ultraLabel10.TabIndex = 119;
			this.ultraLabel10.Text = "hh:mm";
			// 
			// cmbTimeZone
			// 
			this.cmbTimeZone.DropDownListWidth = 0;
			this.cmbTimeZone.FlatMode = true;
			this.cmbTimeZone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbTimeZone.Location = new System.Drawing.Point(186, 20);
			this.cmbTimeZone.Name = "cmbTimeZone";
			this.cmbTimeZone.Size = new System.Drawing.Size(170, 20);
			this.cmbTimeZone.TabIndex = 113;
			this.cmbTimeZone.Text = "(GMT-08:00) Pacific Time (US & Canada); Tijuana";
			this.cmbTimeZone.GotFocus += new System.EventHandler(this.cmbTimeZone_GotFocus);
			this.cmbTimeZone.LostFocus += new System.EventHandler(this.cmbTimeZone_LostFocus);
			// 
			// cmbCountry
			// 
			this.cmbCountry.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance16.BackColor = System.Drawing.Color.LemonChiffon;
			appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCountry.DisplayLayout.Appearance = appearance16;
			this.cmbCountry.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand3.ColHeadersVisible = false;
			appearance17.BackColor = System.Drawing.Color.LemonChiffon;
			ultraGridColumn7.CellAppearance = appearance17;
			ultraGridColumn7.Header.VisiblePosition = 0;
			ultraGridColumn7.MaxWidth = 160;
			ultraGridColumn7.MinWidth = 155;
			ultraGridColumn7.ProportionalResize = true;
			ultraGridColumn8.Header.VisiblePosition = 1;
			ultraGridColumn8.Hidden = true;
			ultraGridBand3.Columns.AddRange(new object[] {
															 ultraGridColumn7,
															 ultraGridColumn8});
			ultraGridBand3.Header.Enabled = false;
			this.cmbCountry.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
			this.cmbCountry.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCountry.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			this.cmbCountry.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCountry.DisplayLayout.MaxRowScrollRegions = 1;
			appearance18.BackColor = System.Drawing.SystemColors.Window;
			appearance18.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCountry.DisplayLayout.Override.ActiveCellAppearance = appearance18;
			appearance19.BackColor = System.Drawing.SystemColors.Highlight;
			appearance19.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCountry.DisplayLayout.Override.ActiveRowAppearance = appearance19;
			this.cmbCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance20.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCountry.DisplayLayout.Override.CardAreaAppearance = appearance20;
			appearance21.BorderColor = System.Drawing.Color.Silver;
			appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCountry.DisplayLayout.Override.CellAppearance = appearance21;
			this.cmbCountry.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCountry.DisplayLayout.Override.CellPadding = 0;
			appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCountry.DisplayLayout.Override.HeaderAppearance = appearance22;
			this.cmbCountry.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCountry.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance23.BackColor = System.Drawing.SystemColors.Window;
			appearance23.BorderColor = System.Drawing.Color.Silver;
			this.cmbCountry.DisplayLayout.Override.RowAppearance = appearance23;
			this.cmbCountry.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCountry.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
			this.cmbCountry.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCountry.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCountry.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCountry.DisplayMember = "";
			this.cmbCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbCountry.DropDownWidth = 0;
			this.cmbCountry.FlatMode = true;
			this.cmbCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbCountry.LimitToList = true;
			this.cmbCountry.Location = new System.Drawing.Point(186, 182);
			this.cmbCountry.Name = "cmbCountry";
			this.cmbCountry.Size = new System.Drawing.Size(170, 20);
			this.cmbCountry.TabIndex = 116;
			this.cmbCountry.ValueMember = "";
			this.cmbCountry.LostFocus += new System.EventHandler(this.cmbCountry_LostFocus);
			this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
			this.cmbCountry.GotFocus += new System.EventHandler(this.cmbCountry_GotFocus);
			// 
			// ultraLabel11
			// 
			appearance25.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel11.Appearance = appearance25;
			this.ultraLabel11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel11.Location = new System.Drawing.Point(10, 208);
			this.ultraLabel11.Name = "ultraLabel11";
			this.ultraLabel11.Size = new System.Drawing.Size(34, 12);
			this.ultraLabel11.TabIndex = 120;
			this.ultraLabel11.Text = "State";
			// 
			// ultraLabel12
			// 
			appearance26.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel12.Appearance = appearance26;
			this.ultraLabel12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel12.Location = new System.Drawing.Point(10, 185);
			this.ultraLabel12.Name = "ultraLabel12";
			this.ultraLabel12.Size = new System.Drawing.Size(50, 14);
			this.ultraLabel12.TabIndex = 121;
			this.ultraLabel12.Text = "Country";
			// 
			// ultraLabel13
			// 
			appearance27.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel13.Appearance = appearance27;
			this.ultraLabel13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel13.Location = new System.Drawing.Point(10, 159);
			this.ultraLabel13.Name = "ultraLabel13";
			this.ultraLabel13.Size = new System.Drawing.Size(122, 14);
			this.ultraLabel13.TabIndex = 122;
			this.ultraLabel13.Text = "Lunch End Time (Local)";
			// 
			// ultraLabel14
			// 
			appearance28.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel14.Appearance = appearance28;
			this.ultraLabel14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel14.Location = new System.Drawing.Point(10, 135);
			this.ultraLabel14.Name = "ultraLabel14";
			this.ultraLabel14.Size = new System.Drawing.Size(130, 14);
			this.ultraLabel14.TabIndex = 123;
			this.ultraLabel14.Text = "Lunch Start Time (Local)";
			// 
			// ultraLabel15
			// 
			appearance29.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel15.Appearance = appearance29;
			this.ultraLabel15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel15.Location = new System.Drawing.Point(10, 23);
			this.ultraLabel15.Name = "ultraLabel15";
			this.ultraLabel15.Size = new System.Drawing.Size(136, 14);
			this.ultraLabel15.TabIndex = 124;
			this.ultraLabel15.Text = "Relationship to GMT (+/-)";
			// 
			// cmbState
			// 
			this.cmbState.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			this.cmbState.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand4.ColHeadersVisible = false;
			appearance30.BackColor = System.Drawing.Color.LemonChiffon;
			ultraGridColumn9.CellAppearance = appearance30;
			ultraGridColumn9.Header.VisiblePosition = 0;
			ultraGridColumn9.Hidden = true;
			appearance31.BackColor = System.Drawing.Color.LemonChiffon;
			ultraGridColumn10.CellAppearance = appearance31;
			ultraGridColumn10.Header.VisiblePosition = 1;
			ultraGridColumn10.Hidden = true;
			appearance32.BackColor = System.Drawing.Color.LemonChiffon;
			ultraGridColumn11.CellAppearance = appearance32;
			ultraGridColumn11.Header.VisiblePosition = 2;
			ultraGridColumn11.Width = 170;
			ultraGridBand4.Columns.AddRange(new object[] {
															 ultraGridColumn9,
															 ultraGridColumn10,
															 ultraGridColumn11});
			this.cmbState.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
			this.cmbState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance33.BorderColor = System.Drawing.Color.Silver;
			this.cmbState.DisplayLayout.Override.RowAppearance = appearance33;
			this.cmbState.DisplayMember = "";
			this.cmbState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbState.DropDownWidth = 0;
			this.cmbState.FlatMode = true;
			this.cmbState.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbState.LimitToList = true;
			this.cmbState.Location = new System.Drawing.Point(186, 204);
			this.cmbState.Name = "cmbState";
			this.cmbState.Size = new System.Drawing.Size(170, 20);
			this.cmbState.TabIndex = 117;
			this.cmbState.ValueMember = "";
			this.cmbState.LostFocus += new System.EventHandler(this.cmbState_LostFocus);
			this.cmbState.GotFocus += new System.EventHandler(this.cmbState_GotFocus);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Controls.Add(this.txtExchangeNameFull);
			this.groupBox1.Controls.Add(this.ultraLabel1);
			this.groupBox1.Controls.Add(this.ultraLabel2);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox1.Location = new System.Drawing.Point(38, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(466, 70);
			this.groupBox1.TabIndex = 47;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Details";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label8.ForeColor = System.Drawing.Color.Red;
			this.label8.Location = new System.Drawing.Point(130, 14);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(12, 8);
			this.label8.TabIndex = 48;
			this.label8.Text = "*";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(84, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(12, 8);
			this.label1.TabIndex = 47;
			this.label1.Text = "*";
			// 
			// txtShortName
			// 
			this.txtShortName.FlatMode = true;
			this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtShortName.Location = new System.Drawing.Point(182, 36);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(146, 20);
			this.txtShortName.TabIndex = 43;
			// 
			// txtExchangeNameFull
			// 
			this.txtExchangeNameFull.FlatMode = true;
			this.txtExchangeNameFull.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtExchangeNameFull.Location = new System.Drawing.Point(182, 14);
			this.txtExchangeNameFull.Name = "txtExchangeNameFull";
			this.txtExchangeNameFull.Size = new System.Drawing.Size(146, 20);
			this.txtExchangeNameFull.TabIndex = 42;
			// 
			// ultraLabel1
			// 
			this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel1.Location = new System.Drawing.Point(10, 40);
			this.ultraLabel1.Name = "ultraLabel1";
			this.ultraLabel1.Size = new System.Drawing.Size(74, 13);
			this.ultraLabel1.TabIndex = 44;
			this.ultraLabel1.Text = "Short Name";
			// 
			// ultraLabel2
			// 
			this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel2.Location = new System.Drawing.Point(10, 18);
			this.ultraLabel2.Name = "ultraLabel2";
			this.ultraLabel2.Size = new System.Drawing.Size(122, 13);
			this.ultraLabel2.TabIndex = 45;
			this.ultraLabel2.Text = "Full Name of Exchange";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.ForeColor = System.Drawing.Color.Red;
			this.label2.Location = new System.Drawing.Point(224, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 8);
			this.label2.TabIndex = 46;
			this.label2.Text = "*";
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label15.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label15.ForeColor = System.Drawing.Color.Red;
			this.label15.Location = new System.Drawing.Point(68, 358);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(102, 12);
			this.label15.TabIndex = 46;
			this.label15.Text = "* Required Field";
			// 
			// ultraTabPageControl2
			// 
			this.ultraTabPageControl2.Controls.Add(this.grdHoliday);
			this.ultraTabPageControl2.Controls.Add(this.groupBox5);
			this.ultraTabPageControl2.Controls.Add(this.label57);
			this.ultraTabPageControl2.Location = new System.Drawing.Point(1, 20);
			this.ultraTabPageControl2.Name = "ultraTabPageControl2";
			this.ultraTabPageControl2.Size = new System.Drawing.Size(544, 377);
			// 
			// grdHoliday
			// 
			this.grdHoliday.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdHoliday.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn12.AutoEdit = false;
			ultraGridColumn12.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
			ultraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
			appearance34.FontData.BoldAsString = "False";
			appearance34.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn12.CellAppearance = appearance34;
			appearance35.FontData.BoldAsString = "True";
			appearance35.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn12.Header.Appearance = appearance35;
			ultraGridColumn12.Header.VisiblePosition = 0;
			ultraGridColumn12.Width = 244;
			ultraGridColumn13.AutoEdit = false;
			appearance36.FontData.BoldAsString = "False";
			appearance36.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn13.CellAppearance = appearance36;
			appearance37.FontData.BoldAsString = "True";
			appearance37.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn13.Header.Appearance = appearance37;
			ultraGridColumn13.Header.VisiblePosition = 1;
			ultraGridColumn13.Width = 265;
			ultraGridBand5.Columns.AddRange(new object[] {
															 ultraGridColumn12,
															 ultraGridColumn13});
			ultraGridBand5.Header.Enabled = false;
			this.grdHoliday.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
			this.grdHoliday.DisplayLayout.GroupByBox.Hidden = true;
			this.grdHoliday.DisplayLayout.MaxColScrollRegions = 1;
			this.grdHoliday.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdHoliday.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdHoliday.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdHoliday.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdHoliday.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdHoliday.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdHoliday.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdHoliday.FlatMode = true;
			this.grdHoliday.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdHoliday.Location = new System.Drawing.Point(4, 2);
			this.grdHoliday.Name = "grdHoliday";
			this.grdHoliday.Size = new System.Drawing.Size(530, 240);
			this.grdHoliday.SupportThemes = false;
			this.grdHoliday.TabIndex = 56;
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.groupBox5.Controls.Add(this.btnUploadHolidays);
			this.groupBox5.Controls.Add(this.label59);
			this.groupBox5.Controls.Add(this.label58);
			this.groupBox5.Controls.Add(this.dttHolidayDate);
			this.groupBox5.Controls.Add(this.label40);
			this.groupBox5.Controls.Add(this.label29);
			this.groupBox5.Controls.Add(this.txtHolidayDescription);
			this.groupBox5.Controls.Add(this.btnAddHoliday);
			this.groupBox5.Controls.Add(this.btnDeleteHoliday);
			this.groupBox5.Location = new System.Drawing.Point(25, 243);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(487, 105);
			this.groupBox5.TabIndex = 9;
			this.groupBox5.TabStop = false;
			// 
			// btnUploadHolidays
			// 
			this.btnUploadHolidays.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(150)), ((System.Byte)(233)), ((System.Byte)(200)));
			this.btnUploadHolidays.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUploadHolidays.BackgroundImage")));
			this.btnUploadHolidays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnUploadHolidays.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnUploadHolidays.Location = new System.Drawing.Point(279, 72);
			this.btnUploadHolidays.Name = "btnUploadHolidays";
			this.btnUploadHolidays.TabIndex = 27;
			this.btnUploadHolidays.Click += new System.EventHandler(this.btnUploadHolidays_Click);
			// 
			// label59
			// 
			this.label59.ForeColor = System.Drawing.Color.Red;
			this.label59.Location = new System.Drawing.Point(180, 28);
			this.label59.Name = "label59";
			this.label59.Size = new System.Drawing.Size(8, 8);
			this.label59.TabIndex = 26;
			this.label59.Text = "*";
			// 
			// label58
			// 
			this.label58.ForeColor = System.Drawing.Color.Red;
			this.label58.Location = new System.Drawing.Point(198, 48);
			this.label58.Name = "label58";
			this.label58.Size = new System.Drawing.Size(8, 8);
			this.label58.TabIndex = 25;
			this.label58.Text = "*";
			// 
			// dttHolidayDate
			// 
			this.dttHolidayDate.CustomFormat = "\'\'";
			this.dttHolidayDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dttHolidayDate.Location = new System.Drawing.Point(217, 24);
			this.dttHolidayDate.Name = "dttHolidayDate";
			this.dttHolidayDate.Size = new System.Drawing.Size(145, 21);
			this.dttHolidayDate.TabIndex = 5;
			this.dttHolidayDate.Value = new System.DateTime(2006, 3, 7, 0, 0, 0, 0);
			this.dttHolidayDate.ValueChanged += new System.EventHandler(this.dttHolidayDate_ValueChanged);
			// 
			// label40
			// 
			this.label40.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label40.Location = new System.Drawing.Point(110, 28);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(74, 16);
			this.label40.TabIndex = 4;
			this.label40.Text = "Holiday Date";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label29
			// 
			this.label29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label29.Location = new System.Drawing.Point(110, 48);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(94, 18);
			this.label29.TabIndex = 2;
			this.label29.Text = "Short Description";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtHolidayDescription
			// 
			this.txtHolidayDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtHolidayDescription.Location = new System.Drawing.Point(217, 48);
			this.txtHolidayDescription.MaxLength = 200;
			this.txtHolidayDescription.Name = "txtHolidayDescription";
			this.txtHolidayDescription.Size = new System.Drawing.Size(145, 21);
			this.txtHolidayDescription.TabIndex = 1;
			this.txtHolidayDescription.Text = "";
			this.txtHolidayDescription.LostFocus += new System.EventHandler(this.txtHolidayDescription_LostFocus);
			this.txtHolidayDescription.GotFocus += new System.EventHandler(this.txtHolidayDescription_GotFocus);
			// 
			// btnAddHoliday
			// 
			this.btnAddHoliday.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnAddHoliday.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddHoliday.BackgroundImage")));
			this.btnAddHoliday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddHoliday.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnAddHoliday.Location = new System.Drawing.Point(132, 72);
			this.btnAddHoliday.Name = "btnAddHoliday";
			this.btnAddHoliday.Size = new System.Drawing.Size(66, 23);
			this.btnAddHoliday.TabIndex = 3;
			this.btnAddHoliday.Click += new System.EventHandler(this.btnAddHoliday_Click);
			// 
			// btnDeleteHoliday
			// 
			this.btnDeleteHoliday.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDeleteHoliday.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteHoliday.BackgroundImage")));
			this.btnDeleteHoliday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDeleteHoliday.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDeleteHoliday.Location = new System.Drawing.Point(201, 72);
			this.btnDeleteHoliday.Name = "btnDeleteHoliday";
			this.btnDeleteHoliday.TabIndex = 9;
			this.btnDeleteHoliday.Click += new System.EventHandler(this.btnDeleteHoliday_Click);
			// 
			// label57
			// 
			this.label57.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label57.ForeColor = System.Drawing.Color.Red;
			this.label57.Location = new System.Drawing.Point(42, 346);
			this.label57.Name = "label57";
			this.label57.Size = new System.Drawing.Size(96, 20);
			this.label57.TabIndex = 24;
			this.label57.Text = "* Required Field";
			// 
			// stbExchange
			// 
			this.stbExchange.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar;
			this.stbExchange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stbExchange.Dock = System.Windows.Forms.DockStyle.None;
			this.stbExchange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stbExchange.Location = new System.Drawing.Point(0, 438);
			this.stbExchange.Name = "stbExchange";
			this.stbExchange.ShowPanels = true;
			this.stbExchange.Size = new System.Drawing.Size(718, 18);
			this.stbExchange.TabIndex = 20;
			this.stbExchange.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.stbExchange_PanelClick);
			// 
			// trvExchange
			// 
			this.trvExchange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.trvExchange.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.trvExchange.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.trvExchange.FullRowSelect = true;
			this.trvExchange.HideSelection = false;
			this.trvExchange.HotTracking = true;
			this.trvExchange.ImageIndex = -1;
			this.trvExchange.Location = new System.Drawing.Point(6, 4);
			this.trvExchange.Name = "trvExchange";
			this.trvExchange.SelectedImageIndex = -1;
			this.trvExchange.ShowLines = false;
			this.trvExchange.Size = new System.Drawing.Size(158, 396);
			this.trvExchange.TabIndex = 30;
			this.trvExchange.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvExchange_AfterSelect);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// errorProvider2
			// 
			this.errorProvider2.ContainerControl = this;
			// 
			// tabMasterExchangeHolidays
			// 
			appearance38.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance38.BackColor2 = System.Drawing.Color.White;
			appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.tabMasterExchangeHolidays.ActiveTabAppearance = appearance38;
			this.tabMasterExchangeHolidays.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabMasterExchangeHolidays.Controls.Add(this.ultraTabSharedControlsPage1);
			this.tabMasterExchangeHolidays.Controls.Add(this.ultraTabPageControl1);
			this.tabMasterExchangeHolidays.Controls.Add(this.ultraTabPageControl2);
			this.tabMasterExchangeHolidays.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.tabMasterExchangeHolidays.Location = new System.Drawing.Point(166, 2);
			this.tabMasterExchangeHolidays.Name = "tabMasterExchangeHolidays";
			this.tabMasterExchangeHolidays.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.tabMasterExchangeHolidays.Size = new System.Drawing.Size(546, 398);
			this.tabMasterExchangeHolidays.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.tabMasterExchangeHolidays.TabIndex = 40;
			appearance39.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance39.BackColor2 = System.Drawing.Color.White;
			appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.None;
			ultraTab1.Appearance = appearance39;
			ultraTab1.TabPage = this.ultraTabPageControl1;
			ultraTab1.Text = "Exchange";
			appearance40.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance40.BackColor2 = System.Drawing.Color.White;
			appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.None;
			ultraTab2.Appearance = appearance40;
			ultraTab2.TabPage = this.ultraTabPageControl2;
			ultraTab2.Text = "Holidays";
			this.tabMasterExchangeHolidays.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																												ultraTab1,
																												ultraTab2});
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(544, 377);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Location = new System.Drawing.Point(2, 408);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(710, 30);
			this.panel1.TabIndex = 42;
			// 
			// btnClose
			// 
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(416, 4);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 45;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(336, 4);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 44;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Location = new System.Drawing.Point(84, 4);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 43;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Location = new System.Drawing.Point(4, 4);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 42;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// ExchangeForm
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(720, 457);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tabMasterExchangeHolidays);
			this.Controls.Add(this.stbExchange);
			this.Controls.Add(this.trvExchange);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(684, 472);
			this.Name = "ExchangeForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Exchange Details";
			this.Load += new System.EventHandler(this.ExchangeForm_Load);
			this.BackColorChanged += new System.EventHandler(this.txtExchangeNameFull_GotFocus);
			this.ultraTabPageControl1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbFlag)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbExchangeLogo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbTimeZone)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.txtShortName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtExchangeNameFull)).EndInit();
			this.ultraTabPageControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).EndInit();
			this.groupBox5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDataSource4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tabMasterExchangeHolidays)).EndInit();
			this.tabMasterExchangeHolidays.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Load Exchange Form

		private void ExchangeForm_Load(object sender, System.EventArgs e)
		{
			//
			// Bind the Combos first and then the tree ...
			//
			try
			{ 
				BindHolidays();
				BindCountryCombo();
				BindStates();
				BindExchangeTree();				
				BindFlags();
				BindLogos();
			}

				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				# endregion

			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("ExchangeForm_Load", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "ExchangeForm_Load"); 
				Logger.Write(logEntry); 

			}				
			#endregion

		}
		#endregion


	

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	
		#region btnSave_Click
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			//
			// Save Exchange & Holidays.
			//
			try
			{
				NodeDetails nodeDetails = (NodeDetails)trvExchange.SelectedNode.Tag;
				int nodeID = nodeDetails.NodeID;
				
				int holidayID = int.MinValue;
				int selectNode = int.MinValue;
				selectNode = SaveExchangeDetails();
				
				if(selectNode == int.MinValue)
				{
					return;
				}	
				
				holidayID =	SaveHolidays(selectNode);
				
				if(holidayID == int.MinValue)
				{
					return;
				}
				if((selectNode > int.MinValue) && (holidayID > int.MinValue))  
				{
					BindFlags();
					BindLogos();
					BindExchangeTree();
					//					int selnode = int.Parse(trvExchange.Nodes[0].Nodes.Count.ToString());
					//					trvExchange.SelectedNode = trvExchange.Nodes[0].Nodes[selnode-1];
					NodeDetails selectNodeDetails = new NodeDetails(selectNode);
					SelectTreeNode(selectNodeDetails);
				}
				BindFlags();
			}
				#region Catch

			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				# endregion

			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSave_Click"); 
				Logger.Write(logEntry); 
				
				#endregion
			}
		}
		#endregion

		private void SelectTreeNode(NodeDetails nodeDetails)
		{
			foreach(TreeNode node in trvExchange.Nodes[0].Nodes)
			{
				if(((NodeDetails) node.Tag).NodeID == nodeDetails.NodeID)
				{
					trvExchange.SelectedNode = node;
					break;
				}
			}
		}

		private int SaveHolidays(int exchangeID)
		{
			//int result = int.MinValue;
			bool nullHoliday = false;
			Holidays holidays = new Holidays();
			holidays = (Holidays) grdHoliday.DataSource;

			if (holidays.Count == 1)
			{
				foreach(Nirvana.Admin.BLL.Holiday holiday in holidays)
				{
					if(holiday.Description == "")
					{
						nullHoliday = true; 
					}
				}
			}

			//If there are some holidays to save.
			if(nullHoliday == false)
			{
				
				foreach(Nirvana.Admin.BLL.Holiday holiday in holidays)
				{
					holiday.ExchangeID = exchangeID;
					//holiday.HolidayID = int.MinValue;
				}
				ExchangeManager.SaveExchangeHolidays(holidays); // .SaveAUECHolidays(holiday1);
			}
			
			//BindHolidays();			
			return 1;
		}
		
		# region SaveExchange Details
        
		private int SaveExchangeDetails()
		{	
			int result = int.MinValue;
			//
			// Set Error Providers to Null else they may keep flashing
			// frm the previous Save Errors if any
			//
			
			errorProvider1.SetError(cmbCountry, "");

			errorProvider1.SetError(cmbTimeZone, "");
			errorProvider1.SetError(txtShortName, "");
			errorProvider1.SetError(txtExchangeNameFull, "");

			Exchange exchange = new Exchange();
			if(trvExchange.SelectedNode != null)
			{		
				//	Exchange(AUEC) is selecte in Node tree.
				NodeDetails nodeDetails = (NodeDetails)trvExchange.SelectedNode.Tag;
				exchange.ExchangeID = nodeDetails.NodeID;
			}
			else
			{
				errorProvider1.SetError(trvExchange, "Please select Exchange!");
			}

			#region  Enter Fields and Check if the fields are entered correctly

			if(txtExchangeNameFull.Text.Trim().Length > 0)
			{
				exchange.Name = txtExchangeNameFull.Text;					
			}
			else
			{	
				errorProvider1.SetError(txtExchangeNameFull, "Please Enter Exchange Full Name.!");
				txtExchangeNameFull.Focus();
				return result;
			}
				
			if(txtShortName.Text.Trim().Length > 0)
			{
				exchange.DisplayName = txtShortName.Text;					
			}
			else
			{
				errorProvider1.SetError(txtShortName, "Please Enter Exchange Short Name!");

				txtShortName.Focus();
				return result;
			}
			exchange.TimeZone = cmbTimeZone.SelectedIndex;			
			exchange.RegularTimeCheck = (chkRegularTime.Checked==true?1:0);
			
			//If this checkbox is checked then only the RegularMarketStartTime and RegularMarketEndTime value 
			//is stored else for the time being some temporary value equivalent to null is stored in the database, 
			//which in our case is year 2000.
			if(chkRegularTime.Checked == true)
			{
			
				if(timeRMST.Text.Trim().Length > 0)
				{
					exchange.RegularTradingStartTime = timeRMST.Value;					
				}
				else
				{
					Common.ResetStatusPanel(stbExchange);
					Common.SetStatusPanel(stbExchange, "Please Enter Regular market Start Time.");
					timeRMST.Focus();
					return result;
				}

				if(timeRMET.Text.Trim().Length > 0)
				{
					exchange.RegularTradingEndTime = timeRMET.Value;					
				}
				else
				{
					Common.ResetStatusPanel(stbExchange);
					Common.SetStatusPanel(stbExchange, "Please Enter Regular market End Time.");
					timeRMET.Focus();
					return result;
				}
			}
			else
			{
				exchange.RegularTradingStartTime = DateTime.Parse(NULLYEAR);
				exchange.RegularTradingEndTime = DateTime.Parse(NULLYEAR);
			}

			exchange.LunchTimeCheck = (chkLunchTime.Checked==true?1:0);
			
			//If this checkbox is checked then only the LunchMarketStartTime and LunchMarketEndTime value 
			//is stored else for the time being some temporary value equivalent to null is stored in the database, 
			//which in our case is year 2000.
			if(chkLunchTime.Checked == true)
			{
				if(timeLunchST.Text.Trim().Length > 0)
				{
					exchange.LunchTimeStartTime = timeLunchST.Value;					
				}
				else
				{
					Common.ResetStatusPanel(stbExchange);
					Common.SetStatusPanel(stbExchange, "Please Enter Lunch Start Time.");
					timeLunchST.Focus();
					return result;
				}
			
				if(timeLunchET.Text.Trim().Length > 0)
				{
					exchange.LunchTimeEndTime = timeLunchET.Value;					
				}
				else
				{
					Common.ResetStatusPanel(stbExchange);
					Common.SetStatusPanel(stbExchange, "Please Enter Lunch End Time.");
					timeLunchET.Focus();
					return result;
				}
			}
			else
			{
				exchange.LunchTimeStartTime = DateTime.Parse(NULLYEAR);
				exchange.LunchTimeEndTime = DateTime.Parse(NULLYEAR);
			}
			
			if(int.Parse(cmbCountry.Value.ToString()) != int.MinValue)
			{
				exchange.Country = int.Parse(cmbCountry.Value.ToString());
			}
			else
			{
				errorProvider1.SetError(cmbCountry, "Please Select Country!");
				cmbCountry.Focus();
				return result;
			}

			if(int.Parse(cmbState.Value.ToString()) != int.MinValue)
			{
				exchange.StateID = int.Parse(cmbState.Value.ToString());					
			}
			else
			{
				errorProvider1.SetError(cmbState, "Please Select State!");
				cmbState.Focus();
				return result;
			}
			#endregion

			#region FlagSave
			int countryFlagID = int.MinValue;
			byte[] flagImage = null;
			Flag flag = new Flag();
			flag.CountryFlagID = int.Parse(cmbFlag.Value.ToString());
			flag.CountryFlagName = cmbFlag.Text.ToString();
			if (cmbFlag.Text.ToString() != "")
			{
				flag.CountryFlagImage = (byte[])cmbFlag.ActiveRow.Cells["CountryFlagImage"].Value;
			}
			else
			{
				flag.CountryFlagImage = null;
			}
			if(flag.CountryFlagImage != flagImage)
			{
				countryFlagID = GeneralManager.SaveCountryFlag(flag);
			}
			#endregion

			#region LogoSave
			int logoID = int.MinValue;
			byte[] logoImage = null;
			Logo logo = new Logo();
			logo.LogoID = int.Parse(cmbExchangeLogo.Value.ToString());
			logo.LogoName = cmbExchangeLogo.Text.ToString();
			if (cmbExchangeLogo.Text.ToString() != "")
			{
				logo.LogoImage = (byte[])cmbExchangeLogo.ActiveRow.Cells["LogoImage"].Value;
			}
			else
			{
				logo.LogoImage = null;
			}
			if(logo.LogoImage != logoImage)
			{
				logoID = GeneralManager.SaveLogo(logo);
			}
			#endregion
			exchange.CountryFlagID = countryFlagID;
			exchange.LogoID = logoID;

			result = ExchangeManager.SaveExchangeForm(exchange);
	
				
			return result;
		}
		#endregion

		
		
		#region BindExchange Tree
		private void BindExchangeTree()
		{
			//
			// Bind Tree to the form
			//

			trvExchange.Nodes.Clear();
			
			Font font = new Font("Tahoma",11, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

			TreeNode treeNodeParent = new TreeNode("Exchanges");
			treeNodeParent.NodeFont = font;
			treeNodeParent.Tag = new NodeDetails(int.MinValue); 
			Exchanges exchanges = ExchangeManager.GetExchanges();
			foreach(Exchange exchange in exchanges)
			{	
				TreeNode treeNodeExchange = new TreeNode(exchange.DisplayName);
				NodeDetails ExchangeNode = new NodeDetails(exchange.ExchangeID); 
				treeNodeExchange.Tag = ExchangeNode;
				
				treeNodeParent.Nodes.Add(treeNodeExchange);				
				
			}	
			trvExchange.Nodes.Add(treeNodeParent);
			trvExchange.ExpandAll();			
			if(treeNodeParent.Nodes.Count > 0)
			{
				trvExchange.SelectedNode = trvExchange.Nodes[0].Nodes[0];
			}
			else
			{
				trvExchange.SelectedNode = trvExchange.Nodes[0];
			}
		}
		#endregion
	
		private void txtUnits_ValueChanged(object sender, System.EventArgs e)
		{
		
		}
		#region Bind Currency and Bind Country Exchange Form Combo



		/// <summary>
		/// This combo is binded by all the existing countries in the database.
		/// </summary>
		private void BindCountryCombo()
		{
			// BInd Country Combo to the Combo Box
			Nirvana.Admin.BLL.Countries countries = GeneralManager.GetCountries();
			countries.Insert(0, new Country(int.MinValue, C_COMBO_SELECT));
			this.cmbCountry.DataSource = countries;
			this.cmbCountry.DisplayMember = "Name";
			this.cmbCountry.ValueMember = "CountryID";
			cmbCountry.Value= int.MinValue;
			
		}

		/// <summary>
		/// This combo is binded by all the existing states in the database. 
		/// </summary>
		private void BindStates()
		{
			//GetStates method fetches the existing states from the database.
			States states = GeneralManager.GetStates();
			//Inserting the - Select - option in the Combo Box at the top.
			states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
			cmbState.DisplayMember = "StateName";
			cmbState.ValueMember = "StateID";
			cmbState.DataSource = states;			
			cmbState.Value = int.MinValue;
		}

		/// <summary>
		/// This combo is binded by all the existing flags in the database.
		/// </summary>
		private void BindFlags()
		{
			//GetFlags method fetches the existing flags from the database.
			Flags flags = GeneralManager.GetFlags();
			//Inserting the - Select - option in the Combo Box at the top.
			flags.Insert(0, new Flag(int.MinValue, C_COMBO_SELECT, null));
			
			cmbFlag.DataSource = flags;			
			cmbFlag.DisplayMember = "CountryFlagName";
			cmbFlag.ValueMember = "CountryFlagID";
			cmbFlag.Value = int.MinValue;

			ColumnsCollection columns = cmbFlag.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns)
			{
				if(column.Key != "CountryFlagName" && column.Key != "CountryFlagImage")
				{
					column.Hidden = true;
				}
			}
			
		}

		private void BindFlags(Flags flags)
		{
			//Inserting the - Select - option in the Combo Box at the top.
			//flags.Insert(0, new Flag(int.MinValue, C_COMBO_SELECT, null));
			cmbFlag.Refresh();
			cmbFlag.DataSource = null;
			cmbFlag.DataSource = flags;			
			cmbFlag.DisplayMember = "CountryFlagName";
			cmbFlag.ValueMember = "CountryFlagID";
			cmbFlag.Value = int.MinValue;
			cmbFlag.DisplayLayout.Bands[0].Columns["CountryFlagID"].Hidden = true;

			ColumnsCollection columns = cmbFlag.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns)
			{
				if(column.Key != "CountryFlagName" && column.Key != "CountryFlagImage")
				{
					column.Hidden = true;
				}
			}
		}

		/// <summary>
		/// This combo is binded by all the existing logos in the database.
		/// </summary>
		private void BindLogos()
		{
			//GetLogos method fetches the existing logos from the database.
			Logos logos = GeneralManager.GetLogos();
			//Inserting the - Select - option in the Combo Box at the top.
			logos.Insert(0, new Logo(int.MinValue, C_COMBO_SELECT, null));
			
			cmbExchangeLogo.DataSource = logos;			
			cmbExchangeLogo.DisplayMember = "LogoName";
			cmbExchangeLogo.ValueMember = "LogoID";
			cmbExchangeLogo.Value = int.MinValue;

			ColumnsCollection columns = cmbExchangeLogo.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns)
			{
				if(column.Key != "LogoName" && column.Key != "LogoImage")
				{
					column.Hidden = true;
				}
			}
		}

		private void BindLogos(Logos logos)
		{
			//Inserting the - Select - option in the Combo Box at the top.
			cmbExchangeLogo.Refresh();
			cmbExchangeLogo.DataSource = null;
			cmbExchangeLogo.DataSource = logos;			
			cmbExchangeLogo.DisplayMember = "LogoName";
			cmbExchangeLogo.ValueMember = "LogoID";
			cmbExchangeLogo.Value = int.MinValue;
			cmbExchangeLogo.DisplayLayout.Bands[0].Columns["LogoID"].Hidden = true;

			ColumnsCollection columns = cmbExchangeLogo.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns)
			{
				if(column.Key != "LogoName" && column.Key != "LogoImage")
				{
					column.Hidden = true;
				}
			}
		}

		/// <summary>
		/// This method empties the ComboBox from any state by assigning the states object to null value.
		/// </summary>
		private void BindEmptyStates()
		{
			//GetStates method fetches the existing states from the database.
			States states = new States();
			//Inserting the - Select - option in the Combo Box at the top.
			states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
			cmbState.DisplayMember = "StateName";
			cmbState.ValueMember = "StateID";
			cmbState.DataSource = states;			
			cmbState.Value = int.MinValue;
		}

		private void BindHolidays()
		{
			Holidays holidays = ExchangeManager.GetHolidays();
			//Assigning the holiday grid's datasource property to holidays object if it has some values.
			if (holidays.Count != 0)
			{
				this.grdHoliday.DataSource = holidays;
					
				//grdHoliday.Rows.Band.Columns["ExchangeHolidayID"].Hidden = true;
				grdHoliday.Rows.Band.Columns["ExchangeID"].Hidden = true;
				//grdHoliday.Rows.Band.Columns["AUECID"].Hidden = true;
				grdHoliday.Rows.Band.Columns["AUECExchangeID"].Hidden = true;

				foreach(Nirvana.Admin.BLL.Holiday holiday in holidays)
				{
					holiday.HolidayID = int.MinValue;
				}
			}
			else
			{
				Nirvana.Admin.BLL.Holiday holiay = new Nirvana.Admin.BLL.Holiday(int.MinValue,int.MinValue, "", DateTime.MinValue); 
				Holidays nullHolidays = new Holidays();
				nullHolidays.Add(holiay); 
				grdHoliday.DataSource = nullHolidays;	
				//grdHoliday.Rows[0].Hidden = true;
			
				ColumnsCollection columns = grdHoliday.DisplayLayout.Bands[0].Columns;
				foreach (UltraGridColumn column in columns)
				{
					if(column.Key != "Date")
					{
						if(column.Key != "Description")
						{
							column.Hidden = true;
						}
					}
				}
		
			}
			grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Header.VisiblePosition = 0;
			grdHoliday.DisplayLayout.Bands[0].Columns["Description"].Header.VisiblePosition = 1;
			grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
		}

		private void BindHolidays(int exchangeID)
		{
			Holidays holidays = ExchangeManager.GetHolidays(exchangeID);
			//Assigning the holiday grid's datasource property to holidays object if it has some values.
			if (holidays.Count != 0)
			{
				this.grdHoliday.DataSource = holidays;
					
				//grdHoliday.Rows.Band.Columns["ExchangeHolidayID"].Hidden = true;
				grdHoliday.Rows.Band.Columns["AUECExchangeID"].Hidden = true;
				grdHoliday.Rows.Band.Columns["ExchangeID"].Hidden = true;
				
			}
			else
			{
				Nirvana.Admin.BLL.Holiday holiay = new Nirvana.Admin.BLL.Holiday(int.MinValue,int.MinValue, "", DateTime.MinValue); 
				Holidays nullHolidays = new Holidays();
				nullHolidays.Add(holiay); 
						grdHoliday.DataSource = nullHolidays;
				//grdHoliday.Rows[0].Hidden = true;
			}
			ColumnsCollection columns2 = grdHoliday.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns2)
			{
				if(column.Key != "Date" && column.Key != "Description")
				{
					column.Hidden = true;
				}
			}
		
			grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Header.VisiblePosition = 0;
			grdHoliday.DisplayLayout.Bands[0].Columns["Description"].Header.VisiblePosition = 1;
			grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;		
			grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
		}

		#endregion


		private void stbExchange_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
		{
		
		}

		
		#region Delete Click
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			bool deleteResult = false;

			if(trvExchange.SelectedNode == null)
			{
				//Nothing is selected in Node tree.
				Common.ResetStatusPanel(stbExchange);
				Common.SetStatusPanel(stbExchange, "Please select an Exchange");
			}
			else
			{
				NodeDetails nodeDetails = (NodeDetails)trvExchange.SelectedNode.Tag;
																		if ( nodeDetails.NodeID== int.MinValue)
																		{
																										MessageBox.Show(this,"Please Select a valid Exchange to Delete","Nirvana Alert",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
																									}
																										//Delete Exchange
																									else
																																{
																																	if(MessageBox.Show(this, "Are you sure you want to delete the selected Exchange?", "Exchange Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
																										{
																											int exchangeID = nodeDetails.NodeID;
																																																if(!(ExchangeManager.DeleteExchange(exchangeID, false)))
																																																{
																																										//							if(MessageBox.Show(this, "Exchange is referenced in AUEC.\n Do you still want to delete it?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
																																										//							{
																																			//
																																			//								ExchangeManager.DeleteExchange(exchangeID,true);
																												//								Common.ResetStatusPanel(stbExchange);
																												//								Common.SetStatusPanel(stbExchange, "Selected Exchange Deleted!");
//								deleteResult = true;
//							}
							MessageBox.Show(this, "This Exchange is referred in AUEC/Counterparty Venue Master/Company. Please remove references first to delete it.", "Nirvana Alert");
						}
						else
						{
							deleteResult = true;
						}
					}
				}
				if(deleteResult == true)
				{
					BindExchangeTree();
				}
			}
		}

		#endregion

		private void SetExchangeTab(Nirvana.Admin.BLL.Exchange exchange1)
		{
			Exchange exchange = exchange1;
			
			if(exchange != null)
			{
				txtExchangeNameFull.Text = exchange.Name;			
				txtShortName.Text = exchange.DisplayName;
				int currencyID = int.Parse(exchange.Currency.ToString());
				Currencies currencies = new Currencies();

				cmbTimeZone.SelectedIndex = int.Parse(exchange.TimeZone.ToString()); 

				
				
				chkRegularTime.Checked = (int.Parse(exchange.RegularTimeCheck.ToString())==1?true:false);
				
				//The value for RegularMarketStartTime is checked to some temporary value equivalent to null 
				//stored in the database agaist the particular exchange, which in our case is year 2000. If this matches then 
				//the corresponding textbox is disabled or else the actual value is displayed.
				if (int.Parse(exchange.RegularTradingStartTime.Year.ToString()) == 2000)
				{
					timeRMST.Text = exchange.RegularTradingStartTime.Hour + ":" + exchange.RegularTradingStartTime.Minute;
					timeRMET.Text = exchange.RegularTradingEndTime.Hour + ":" + exchange.RegularTradingEndTime.Minute;
					timeRMST.Enabled = false;
					timeRMET.Enabled = false;
				}
				else
				{
					if (timeRMST.Enabled == false)
					{
						timeRMST.Enabled = true;
					}
					if (timeRMET.Enabled == false)
					{
						timeRMET.Enabled = true;
					}
					timeRMST.Text = exchange.RegularTradingStartTime.Hour + ":" + exchange.RegularTradingStartTime.Minute;
					timeRMET.Text = exchange.RegularTradingEndTime.Hour + ":" + exchange.RegularTradingEndTime.Minute;
				}

				chkLunchTime.Checked = (int.Parse(exchange.LunchTimeCheck.ToString())==1?true:false);
				
				//The value for LunchMarketStartTime is checked to some temporary value equivalent to null 
				//stored in the database agaist the particular exchange, which in our case is year 2000. If this matches then 
				//the corresponding textbox is disabled or else the actual value is displayed.
				if (int.Parse(exchange.LunchTimeStartTime.Year.ToString()) == 2000)
				{
					timeLunchST.Text = exchange.LunchTimeStartTime.Hour + ":" + exchange.LunchTimeStartTime.Minute;
					timeLunchET.Text = exchange.LunchTimeEndTime.Hour + ":" + exchange.LunchTimeEndTime.Minute;
					timeLunchST.Enabled = false;
					timeLunchET.Enabled = false;
				}
				else
				{
					if (timeLunchST.Enabled == false)
					{
						timeLunchST.Enabled = true;
					}
					if (timeLunchET.Enabled == false)
					{
						timeLunchET.Enabled = true;
					}
					timeLunchST.Text = exchange.LunchTimeStartTime.Hour + ":" + exchange.LunchTimeStartTime.Minute;
					timeLunchET.Text = exchange.LunchTimeEndTime.Hour + ":" + exchange.LunchTimeEndTime.Minute;
				}
				cmbCountry.Value = exchange.Country	;
				cmbState.Value = exchange.StateID;
				cmbFlag.Value = exchange.CountryFlagID;
				cmbExchangeLogo.Value = exchange.LogoID;

				ColumnsCollection columns = cmbFlag.DisplayLayout.Bands[0].Columns;
				foreach (UltraGridColumn column in columns)
				{
					if(column.Key != "CountryFlagName" && column.Key != "CountryFlagImage")
					{
						column.Hidden = true;
					}
				}

				columns = cmbExchangeLogo.DisplayLayout.Bands[0].Columns;
				foreach (UltraGridColumn column in columns)
				{
					if(column.Key != "LogoName" && column.Key != "LogoImage")
					{
						column.Hidden = true;
					}
				}
			}
		}
		

		# region treeExchange After Selection
        		
		private void trvExchange_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{						
			try
			{
				if(trvExchange.SelectedNode != null)
				{
					if (trvExchange.SelectedNode != trvExchange.Nodes[0])
					{
					
						NodeDetails nodeDetails = (NodeDetails)trvExchange.SelectedNode.Tag;

						//	Exchange is selecte in Node tree.
						int exchangeID = nodeDetails.NodeID;

						//	Get Exchange related data and display in relevent field.
						Nirvana.Admin.BLL.Exchange exchange = ExchangeManager.GetExchange(exchangeID);
						SetExchangeTab(exchange);
						if (exchangeID > 0)
						{
							SetHolidaysTab(exchangeID);
						}
						else
						{
							BindHolidays();
						}
						this.dttHolidayDate.CustomFormat = "\'\'";
						this.dttHolidayDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
					}
					else
											{
						NodeDetails nodeDetail = (NodeDetails)trvExchange.SelectedNode.Tag;
												RefreshExchangeForm();
						BindHolidays();
																														//tabMasterExchangeHolidays.SelectedTab = tabMasterExchangeHolidays.TabPages[0];
						tabMasterExchangeHolidays.Tabs[0].Selected = true;
//						tabMasterExchangeHolidays.TabPages[0].ena = tabMasterExchangeHolidays.TabPages[0];

						
					}
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
						AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
				}
				#endregion

			finally
				{
				#region LogEntry
					LogEntry logEntry = new LogEntry("trvExchange_AfterSelect", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
									FORM_NAME + "trvExchange_AfterSelect"); 
				Logger.Write(logEntry); 
			#endregion
			}
		}

		#endregion

		private void SetHolidaysTab(int exchangeID)
		{
			Holidays holidays = ExchangeManager.GetHolidays(exchangeID);
			//Assigning the holiday grid's datasource property to holidays object if it has some values.
			if (holidays.Count != 0)
			{
				this.grdHoliday.DataSource = holidays;
					
				grdHoliday.Rows.Band.Columns["HolidayID"].Hidden = true;
				grdHoliday.Rows.Band.Columns["ExchangeID"].Hidden = true;
				grdHoliday.Rows.Band.Columns["AUECExchangeID"].Hidden = true;
			}
			else
			{
				Nirvana.Admin.BLL.Holiday holiay = new Nirvana.Admin.BLL.Holiday(int.MinValue, int.MinValue, "", DateTime.MinValue); //10/10/2002
				Holidays nullHolidays = new Holidays();
				nullHolidays.Add(holiay); 
				grdHoliday.DataSource = nullHolidays;	
				//grdHoliday.Rows[0].Hidden = true;
				this.grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			}
		}

		private void RefreshExchangeForm()
		{

			
			txtExchangeNameFull.Text = "";			
			txtShortName.Text = "";			

			cmbTimeZone.SelectedIndex = 1; 

			cmbState.Text = C_COMBO_SELECT;
			cmbCountry.Text = C_COMBO_SELECT;

			
		}
		#region Color Focus
		private void txtShortName_GotFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor= Color.LemonChiffon;
		}

		private void txtShortName_LostFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.White;
		}
				private void txtExchangeNameFull_GotFocus(object sender, System.EventArgs e)
		{
				txtExchangeNameFull.BackColor = Color.LemonChiffon;
		}
							private void txtExchangeNameFull_LostFocus(object sender, System.EventArgs e)
		{
				txtExchangeNameFull.BackColor = Color.White;
		}
			
		private void cmbCountry_GotFocus(object sender, System.EventArgs e)
		{
						
		cmbCountry.Appearance.BackColor= Color.LemonChiffon;
		}

		private void cmbCountry_LostFocus(object sender, System.EventArgs e)
		{
			cmbCountry.Appearance.BackColor = Color.White;
		}

		private void cmbState_GotFocus(object sender, System.EventArgs e)
		
		{
			cmbState.Appearance.BackColor= Color.LemonChiffon;
		}

		private void cmbState_LostFocus(object sender, System.EventArgs e)
		
		{
			cmbState.Appearance.BackColor = Color.White;
		}
		private void cmbTimeZone_GotFocus(object sender, System.EventArgs e)
		
		{
			cmbTimeZone.Appearance.BackColor= Color.LemonChiffon;
		}

		private void cmbTimeZone_LostFocus(object sender, System.EventArgs e)
		{
			cmbTimeZone.Appearance.BackColor = Color.White;
		}
		private void cmbFlag_GotFocus(object sender, System.EventArgs e)
		
		{
			cmbFlag.Appearance.BackColor= Color.LemonChiffon;
		}

		private void cmbFlag_LostFocus(object sender, System.EventArgs e)
		{
			cmbFlag.Appearance.BackColor = Color.White;
		}
		private void cmbExchangeLogo_GotFocus(object sender, System.EventArgs e)
		
		{
			cmbExchangeLogo.Appearance.BackColor= Color.LemonChiffon;
		}

		private void cmbExchangeLogo_LostFocus(object sender, System.EventArgs e)
		{
			cmbExchangeLogo.Appearance.BackColor = Color.White;
		}
		private void txtHolidayDescription_GotFocus(object sender, System.EventArgs e)
		{
			txtHolidayDescription.BackColor= Color.LemonChiffon;
		}

		private void txtHolidayDescription_LostFocus(object sender, System.EventArgs e)
		{
			txtHolidayDescription.BackColor = Color.White;
		}



		#endregion

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			if(trvExchange.SelectedNode != trvExchange.Nodes[0])
			{
				trvExchange.SelectedNode = trvExchange.Nodes[0];
				tabMasterExchangeHolidays.Tabs[0].Selected = true;
			}
			BindHolidays();
		}

		

		private void ultraLabel17_Enter(object sender, System.EventArgs e)
		{
		
		}

		Holidays addHolidays = new Holidays();
		private void btnAddHoliday_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnAdd, "");
				errorProvider1.SetError(txtHolidayDescription, "");
				
				//This check is done to ensure that the holiday is assigned to some already added node. 
				if(trvExchange.SelectedNode == null)
				{
					//
				}
				else
				{
					NodeDetails nodeDetails = (NodeDetails)trvExchange.SelectedNode.Tag;
					
					if(nodeDetails.Type == NodeType.Exchange)
					{
						//Exchange(AUEC) is selected in Node tree.
						int exchangeID = nodeDetails.NodeID;

						Holidays holidays = ((Holidays)grdHoliday.DataSource);
						// Check if Description for the new Holiday is given
						if (txtHolidayDescription.Text.Trim()=="")
						{
							//MessageBox.Show("Please Enter Description");
							errorProvider1.SetError(txtHolidayDescription, "Please Enter Description.");
							txtHolidayDescription.Focus();
						}
						else
						{
							//Check if Date already exists in the Holiday List
							bool flag = false;
							foreach(Nirvana.Admin.BLL.Holiday holiday in holidays)
							{
								if(holiday.Date.ToShortDateString()== dttHolidayDate.Value.ToShortDateString()) 
								{
									flag = true;
									break;
								}
							}
							
							// All Checks Cleared: Save Holidays
							if (flag==false)
							{
								Nirvana.Admin.BLL.Holiday holiday1 = new Nirvana.Admin.BLL.Holiday(int.MinValue, txtHolidayDescription.Text, dttHolidayDate.Value, exchangeID);
								//addHolidays.Add(holiday1);
								holidays.Add(holiday1);
								grdHoliday.DataSource = null;
								//this.grdHoliday.DataSource = addHolidays;
								this.grdHoliday.DataSource = holidays;
								grdHoliday.Rows.Band.Columns["HolidayID"].Hidden = true;
								grdHoliday.Rows.Band.Columns["ExchangeID"].Hidden = true;
								grdHoliday.Rows.Band.Columns["AUECExchangeID"].Hidden = true;

								txtHolidayDescription.Text = "";
								
								ExchangeManager.SaveExchangeHolidays(holiday1); // .SaveAUECHolidays(holiday1);
								BindHolidays(exchangeID);
							}
							else
							{
								// Date already exists in the database ... Error Message
								BindHolidays(exchangeID);
								MessageBox.Show("Date " + dttHolidayDate.Value.ToShortDateString() + " already exists in the List!" )  ;
								errorProvider1.SetError(dttHolidayDate, "Holiday with the same date already exists.");
								btnAdd.Focus();
				
							}
						}
					}
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnAddHoliday_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnAddHoliday_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void btnSaveHoliday_Click(object sender, System.EventArgs e)
		{
		
		}

		private UploadHolidays frmUploadHolidays = null;
		private void btnUploadHolidays_Click(object sender, System.EventArgs e)
		{
			NodeDetails nodeDetails = (NodeDetails)trvExchange.SelectedNode.Tag;
			
			if(frmUploadHolidays == null)
			{
				frmUploadHolidays = new UploadHolidays();
			}
			frmUploadHolidays.ExchangeID = nodeDetails.NodeID;
			frmUploadHolidays.ShowDialog(this);
		}

		private void chkRegularTime_CheckStateChanged(object sender, System.EventArgs e)
		{
			if(chkRegularTime.Checked == true)
			{
				timeRMST.Enabled = true;
				timeRMET.Enabled = true;
			}
			else
			{
				timeRMST.Enabled = false;
				timeRMET.Enabled = false;
			}
		}

		private void chkLunchTime_CheckStateChanged(object sender, System.EventArgs e)
		{
			if(chkLunchTime.Checked == true)
			{
				timeLunchST.Enabled = true;
				timeLunchET.Enabled = true;
			}
			else
			{
				timeLunchST.Enabled = false;
				timeLunchET.Enabled = false;
			}
		}

		private void btnDeleteHoliday_Click(object sender, System.EventArgs e)
		{
			NodeDetails nodeDetails = (NodeDetails)trvExchange.SelectedNode.Tag;
			int exchangeID = nodeDetails.NodeID;
			int exchangeHolidayID = int.Parse(grdHoliday.ActiveRow.Cells["holidayID"].Value.ToString());
			bool result = false;
			result = ExchangeManager.DeleteExchangeHoliday(exchangeHolidayID);
			if(result == true)
			{
				MessageBox.Show("Holiday deleted");
			}
			else
			{
				MessageBox.Show("Holiday is not deleted yet");
			}
			BindHolidays(exchangeID);

		}

		private void cmbCountry_ValueChanged(object sender, System.EventArgs e)
		{
			int countryID = int.Parse(cmbCountry.Value.ToString());
			if (countryID > 0)
			{
				//GetStates method fetches the existing states from the database.
				States states = GeneralManager.GetStates(countryID);
				//System.Data.DataRow dr = new System.Data.DataRow();
				//dr.
				if (states.Count > 0)
				{
					states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
					cmbState.DisplayMember = "StateName";
					cmbState.ValueMember = "StateID";
					cmbState.DataSource = states;			
					cmbState.Value = int.MinValue;
				}
				else
				{
					BindEmptyStates();
				}
			}
		}

		private void ultraTabPageControl1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		public byte[] m_barrImg = null;
		public string _fullName = string.Empty;
		public string _flagName = string.Empty;
		private void btnUploadFlag_Click(object sender, System.EventArgs e)
		{
			try
			{
				long m_lImageFileLength = long.MinValue;
								
				openFileDialogFlag.InitialDirectory = "DeskTop";
				openFileDialogFlag.Filter = "Icon Files (*.ico)|*.ico" ;
				if(openFileDialogFlag.ShowDialog() == DialogResult.OK)
				{
					
					string strFn = openFileDialogFlag.FileName;
					//pictureBox1.Image=Image.FromFile(strFn);
				
					FileInfo fiImage=new FileInfo(strFn);
					m_lImageFileLength=fiImage.Length;
				
					_fullName = fiImage.FullName;
					_flagName = fiImage.Name;

					FileStream fs=new FileStream(strFn,FileMode.Open, 
						FileAccess.Read,FileShare.Read);
				
					m_barrImg= new byte[Convert.ToInt32(m_lImageFileLength)];
					int iBytesRead = fs.Read(m_barrImg,0, 
						Convert.ToInt32(m_lImageFileLength));
					fs.Close();

					Flags flags = (Flags)cmbFlag.DataSource;
					int flagCount = flags.Count;
					flags.Insert(flagCount, new Flag(int.MinValue, _flagName, m_barrImg));
					BindFlags(flags);
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				# endregion

			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("ExchangeForm_Load", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "ExchangeForm_Load"); 
				Logger.Write(logEntry); 

			}				
			#endregion
		}

		public string _logoName = string.Empty;
		private void btnUpoadExchangeLogo_Click(object sender, System.EventArgs e)
		{
			try
			{
				long m_lImageFileLength = long.MinValue;
								
				openFileDialogFlag.InitialDirectory = "DeskTop";
				openFileDialogFlag.Filter = "Icon Files (*.ico)|*.ico" ;
				if(openFileDialogFlag.ShowDialog() == DialogResult.OK)
				{
					
					string strFn = openFileDialogFlag.FileName;
				
					FileInfo fiImage=new FileInfo(strFn);
					m_lImageFileLength=fiImage.Length;
				
					_fullName = fiImage.FullName;
					_logoName = fiImage.Name;

					FileStream fs=new FileStream(strFn,FileMode.Open, 
						FileAccess.Read,FileShare.Read);
				
					m_barrImg= new byte[Convert.ToInt32(m_lImageFileLength)];
					int iBytesRead = fs.Read(m_barrImg,0, 
						Convert.ToInt32(m_lImageFileLength));
					fs.Close();

					Logos logos = (Logos)cmbExchangeLogo.DataSource;
					int logoCount = logos.Count;
					logos.Insert(logoCount, new Logo(int.MinValue, _logoName, m_barrImg));
					BindLogos(logos);
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				# endregion

			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("ExchangeForm_Load", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "ExchangeForm_Load"); 
				Logger.Write(logEntry); 

			}				
			#endregion
		}

	

		private void ultraGroupBox4_Click(object sender, System.EventArgs e)
		{
		
		}

		private void groupBox2_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void cmbFlag_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			e.Layout.Override.DefaultRowHeight = 20;
			Infragistics.Win.EmbeddableImageRenderer aImageRenderer = new Infragistics.Win.EmbeddableImageRenderer();
			aImageRenderer.DrawBorderShadow = false;
			e.Layout.Bands[0].Columns["CountryFlagImage"].Editor = aImageRenderer;
		}

		private void cmbExchangeLogo_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			e.Layout.Override.DefaultRowHeight = 20;
			Infragistics.Win.EmbeddableImageRenderer aImageRenderer = new Infragistics.Win.EmbeddableImageRenderer();
			aImageRenderer.DrawBorderShadow = false;
			e.Layout.Bands[0].Columns["LogoImage"].Editor = aImageRenderer;
		}

		private void dttHolidayDate_ValueChanged(object sender, System.EventArgs e)
		{
			dttHolidayDate.Format = DateTimePickerFormat.Short;
		}

		#region NodeDetails

		class NodeDetails
		{
			private NodeType _type = NodeType.Exchange;
			private int _nodeID = int.MinValue;

			public NodeDetails()
			{
			}

			public NodeDetails(int nodeID)
			{				
				_nodeID = nodeID;
			}
			
			public NodeDetails(NodeType type, int nodeID)
			{
				_type = type;
				_nodeID = nodeID;
			}

			public NodeType Type
			{
				get{return _type;}
				set{_type = value;}
			}
			public int NodeID
			{
				get{return _nodeID;}
				set{_nodeID = value;}
			}
		}

		enum NodeType
		{
			Exchange = 1,
		}

		#endregion		

	
	}
}

