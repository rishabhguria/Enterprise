using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for Test.
	/// </summary>
	public class Test : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbTraderShortName;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCompanyTradingAccount;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label lblExchangeLogo;
		private System.Windows.Forms.Label lblFlag;
		private Infragistics.Win.Misc.UltraLabel ultraLabel39;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtClientTradingAccount;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private    Traders _traders ;	
		private int _companyID;
		private int _companyClientID;		
		private bool _isAddition=true;
		const string C_COMBO_SELECT = "- Select -";

		public Test()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		public Test(int companyID,int companyClientID)
		{
			try
			{
				InitializeComponent();
				//	_traders=traders;
				_companyClientID=companyClientID;
				_companyID=companyID;		
			}
			catch(Exception ex)
			{
				throw ex;
			}
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
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TraderID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FirstName", 1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastName", 2);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 3);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Title", 4);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EMail", 5);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneWork", 6);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneCell", 7);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Pager", 8);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneHome", 9);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientID", 10);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 12);
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
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyTradingAccountsID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountName", 1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingShortName", 2);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountsID", 4);
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Test));
			Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
			this.cmbTraderShortName = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbCompanyTradingAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lblExchangeLogo = new System.Windows.Forms.Label();
			this.lblFlag = new System.Windows.Forms.Label();
			this.ultraLabel39 = new Infragistics.Win.Misc.UltraLabel();
			this.txtClientTradingAccount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			((System.ComponentModel.ISupportInitialize)(this.cmbTraderShortName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTradingAccount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtClientTradingAccount)).BeginInit();
			this.SuspendLayout();
			// 
			// cmbTraderShortName
			// 
			this.cmbTraderShortName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance1.BackColor = System.Drawing.SystemColors.Window;
			appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbTraderShortName.DisplayLayout.Appearance = appearance1;
			this.cmbTraderShortName.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand1.ColHeadersVisible = false;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Hidden = true;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Hidden = true;
			ultraGridColumn4.Header.VisiblePosition = 3;
			ultraGridColumn5.Header.VisiblePosition = 4;
			ultraGridColumn5.Hidden = true;
			ultraGridColumn6.Header.VisiblePosition = 5;
			ultraGridColumn6.Hidden = true;
			ultraGridColumn7.Header.VisiblePosition = 6;
			ultraGridColumn7.Hidden = true;
			ultraGridColumn8.Header.VisiblePosition = 7;
			ultraGridColumn8.Hidden = true;
			ultraGridColumn9.Header.VisiblePosition = 8;
			ultraGridColumn9.Hidden = true;
			ultraGridColumn10.Header.VisiblePosition = 9;
			ultraGridColumn10.Hidden = true;
			ultraGridColumn11.Header.VisiblePosition = 10;
			ultraGridColumn11.Hidden = true;
			ultraGridColumn12.Header.VisiblePosition = 11;
			ultraGridColumn12.Hidden = true;
			ultraGridColumn13.Header.VisiblePosition = 12;
			ultraGridColumn13.Hidden = true;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3,
															 ultraGridColumn4,
															 ultraGridColumn5,
															 ultraGridColumn6,
															 ultraGridColumn7,
															 ultraGridColumn8,
															 ultraGridColumn9,
															 ultraGridColumn10,
															 ultraGridColumn11,
															 ultraGridColumn12,
															 ultraGridColumn13});
			this.cmbTraderShortName.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.cmbTraderShortName.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbTraderShortName.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance2.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbTraderShortName.DisplayLayout.GroupByBox.Appearance = appearance2;
			appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbTraderShortName.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
			this.cmbTraderShortName.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance4.BackColor2 = System.Drawing.SystemColors.Control;
			appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbTraderShortName.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
			this.cmbTraderShortName.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbTraderShortName.DisplayLayout.MaxRowScrollRegions = 1;
			appearance5.BackColor = System.Drawing.SystemColors.Window;
			appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbTraderShortName.DisplayLayout.Override.ActiveCellAppearance = appearance5;
			appearance6.BackColor = System.Drawing.SystemColors.Highlight;
			appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbTraderShortName.DisplayLayout.Override.ActiveRowAppearance = appearance6;
			this.cmbTraderShortName.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbTraderShortName.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance7.BackColor = System.Drawing.SystemColors.Window;
			this.cmbTraderShortName.DisplayLayout.Override.CardAreaAppearance = appearance7;
			appearance8.BorderColor = System.Drawing.Color.Silver;
			appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbTraderShortName.DisplayLayout.Override.CellAppearance = appearance8;
			this.cmbTraderShortName.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbTraderShortName.DisplayLayout.Override.CellPadding = 0;
			appearance9.BackColor = System.Drawing.SystemColors.Control;
			appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance9.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbTraderShortName.DisplayLayout.Override.GroupByRowAppearance = appearance9;
			appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbTraderShortName.DisplayLayout.Override.HeaderAppearance = appearance10;
			this.cmbTraderShortName.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbTraderShortName.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance11.BackColor = System.Drawing.SystemColors.Window;
			appearance11.BorderColor = System.Drawing.Color.Silver;
			this.cmbTraderShortName.DisplayLayout.Override.RowAppearance = appearance11;
			this.cmbTraderShortName.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbTraderShortName.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
			this.cmbTraderShortName.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbTraderShortName.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbTraderShortName.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbTraderShortName.DisplayMember = "";
			this.cmbTraderShortName.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbTraderShortName.DropDownWidth = 0;
			this.cmbTraderShortName.FlatMode = true;
			this.cmbTraderShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbTraderShortName.Location = new System.Drawing.Point(160, 80);
			this.cmbTraderShortName.Name = "cmbTraderShortName";
			this.cmbTraderShortName.Size = new System.Drawing.Size(162, 20);
			this.cmbTraderShortName.TabIndex = 136;
			this.cmbTraderShortName.ValueMember = "";
			// 
			// cmbCompanyTradingAccount
			// 
			this.cmbCompanyTradingAccount.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance13.BackColor = System.Drawing.SystemColors.Window;
			appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCompanyTradingAccount.DisplayLayout.Appearance = appearance13;
			this.cmbCompanyTradingAccount.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
			ultraGridBand2.ColHeadersVisible = false;
			ultraGridColumn14.Header.VisiblePosition = 0;
			ultraGridColumn14.Hidden = true;
			ultraGridColumn15.Header.VisiblePosition = 1;
			ultraGridColumn16.Header.VisiblePosition = 2;
			ultraGridColumn16.Hidden = true;
			ultraGridColumn17.Header.VisiblePosition = 3;
			ultraGridColumn17.Hidden = true;
			ultraGridColumn18.Header.VisiblePosition = 4;
			ultraGridColumn18.Hidden = true;
			ultraGridBand2.Columns.AddRange(new object[] {
															 ultraGridColumn14,
															 ultraGridColumn15,
															 ultraGridColumn16,
															 ultraGridColumn17,
															 ultraGridColumn18});
			this.cmbCompanyTradingAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
			this.cmbCompanyTradingAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCompanyTradingAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance14.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.Appearance = appearance14;
			appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
			this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance16.BackColor2 = System.Drawing.SystemColors.Control;
			appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCompanyTradingAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
			this.cmbCompanyTradingAccount.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCompanyTradingAccount.DisplayLayout.MaxRowScrollRegions = 1;
			appearance17.BackColor = System.Drawing.SystemColors.Window;
			appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.ActiveCellAppearance = appearance17;
			appearance18.BackColor = System.Drawing.SystemColors.Highlight;
			appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.ActiveRowAppearance = appearance18;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance19.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.CardAreaAppearance = appearance19;
			appearance20.BorderColor = System.Drawing.Color.Silver;
			appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.CellAppearance = appearance20;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.CellPadding = 0;
			appearance21.BackColor = System.Drawing.SystemColors.Control;
			appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance21.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.GroupByRowAppearance = appearance21;
			appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.HeaderAppearance = appearance22;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance23.BackColor = System.Drawing.SystemColors.Window;
			appearance23.BorderColor = System.Drawing.Color.Silver;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.RowAppearance = appearance23;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCompanyTradingAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
			this.cmbCompanyTradingAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCompanyTradingAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCompanyTradingAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCompanyTradingAccount.DisplayMember = "";
			this.cmbCompanyTradingAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbCompanyTradingAccount.DropDownWidth = 0;
			this.cmbCompanyTradingAccount.FlatMode = true;
			this.cmbCompanyTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbCompanyTradingAccount.Location = new System.Drawing.Point(160, 48);
			this.cmbCompanyTradingAccount.Name = "cmbCompanyTradingAccount";
			this.cmbCompanyTradingAccount.Size = new System.Drawing.Size(162, 20);
			this.cmbCompanyTradingAccount.TabIndex = 135;
			this.cmbCompanyTradingAccount.ValueMember = "";
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(168, 128);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 134;
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnAdd.Location = new System.Drawing.Point(88, 128);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 133;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// lblExchangeLogo
			// 
			this.lblExchangeLogo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblExchangeLogo.Location = new System.Drawing.Point(16, 80);
			this.lblExchangeLogo.Name = "lblExchangeLogo";
			this.lblExchangeLogo.Size = new System.Drawing.Size(122, 14);
			this.lblExchangeLogo.TabIndex = 132;
			this.lblExchangeLogo.Text = "ClientTraderShortName";
			// 
			// lblFlag
			// 
			this.lblFlag.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblFlag.Location = new System.Drawing.Point(16, 48);
			this.lblFlag.Name = "lblFlag";
			this.lblFlag.Size = new System.Drawing.Size(132, 14);
			this.lblFlag.TabIndex = 131;
			this.lblFlag.Text = "CompanyTradingAccount";
			// 
			// ultraLabel39
			// 
			appearance25.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.ultraLabel39.Appearance = appearance25;
			this.ultraLabel39.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.ultraLabel39.Location = new System.Drawing.Point(16, 16);
			this.ultraLabel39.Name = "ultraLabel39";
			this.ultraLabel39.Size = new System.Drawing.Size(112, 14);
			this.ultraLabel39.TabIndex = 130;
			this.ultraLabel39.Text = "ClientTradingAccount";
			// 
			// txtClientTradingAccount
			// 
			this.txtClientTradingAccount.FlatMode = true;
			this.txtClientTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtClientTradingAccount.Location = new System.Drawing.Point(160, 16);
			this.txtClientTradingAccount.Name = "txtClientTradingAccount";
			this.txtClientTradingAccount.Size = new System.Drawing.Size(162, 20);
			this.txtClientTradingAccount.TabIndex = 129;
			// 
			// Test
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 173);
			this.Controls.Add(this.cmbTraderShortName);
			this.Controls.Add(this.cmbCompanyTradingAccount);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lblExchangeLogo);
			this.Controls.Add(this.lblFlag);
			this.Controls.Add(this.ultraLabel39);
			this.Controls.Add(this.txtClientTradingAccount);
			this.Name = "Test";
			this.Text = "Test";
			((System.ComponentModel.ISupportInitialize)(this.cmbTraderShortName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTradingAccount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtClientTradingAccount)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
		
		}
		public int CompanyClientID


		{
			set 
			{
				_companyClientID=value;
			
			}
			get
			{
				return _companyClientID;
			}
		
		}


		public  Traders Traders 


		{
			set 
			{
				_traders =value;
			
			}
			get
			{
				return _traders;
			}
		
		}
		public bool ISAddition
		{
			get{return _isAddition;}
			set{_isAddition=value;}
		}
	}
}
