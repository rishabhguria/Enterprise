using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CompanyCounterPartyFundLevelTags.
	/// </summary>
	public class CreateCompanyCounterPartyFundLevelTags : System.Windows.Forms.UserControl
	{
		const string C_COMBO_SELECT = "- Select -";
		
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtCompID;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label17;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterPartyVenue;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbFunds;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbStrategy;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCMTAGiveUp;
		private System.Windows.Forms.Label lblCounterParty;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterParty;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCompanyCounterPartyFundLevelTags()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

		}
		
		public void SetData()
		{
			BindCounterPartyVenues();
			BindCounterParty();
			BindIdentifiers();
			BindFunds();
			BindStrategies();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CreateCompanyCounterPartyFundLevelTags));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
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
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
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
			Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtCompID = new System.Windows.Forms.TextBox();
			this.lblCounterParty = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.cmbCounterPartyVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbFunds = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbStrategy = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbCMTAGiveUp = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyVenue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbFunds)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCMTAGiveUp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtCompID);
			this.groupBox1.Controls.Add(this.lblCounterParty);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.cmbCounterPartyVenue);
			this.groupBox1.Controls.Add(this.cmbFunds);
			this.groupBox1.Controls.Add(this.cmbStrategy);
			this.groupBox1.Controls.Add(this.cmbCMTAGiveUp);
			this.groupBox1.Controls.Add(this.cmbCounterParty);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox1.Location = new System.Drawing.Point(4, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(362, 158);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Fund Level Details";
			// 
			// txtCompID
			// 
			this.txtCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtCompID.Location = new System.Drawing.Point(158, 106);
			this.txtCompID.MaxLength = 50;
			this.txtCompID.Name = "txtCompID";
			this.txtCompID.Size = new System.Drawing.Size(174, 21);
			this.txtCompID.TabIndex = 10;
			this.txtCompID.Text = "";
			this.txtCompID.LostFocus += new System.EventHandler(this.txtCompID_LostFocus);
			this.txtCompID.GotFocus += new System.EventHandler(this.txtCompID_GotFocus);
			// 
			// lblCounterParty
			// 
			this.lblCounterParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblCounterParty.Location = new System.Drawing.Point(8, 23);
			this.lblCounterParty.Name = "lblCounterParty";
			this.lblCounterParty.Size = new System.Drawing.Size(80, 14);
			this.lblCounterParty.TabIndex = 0;
			this.lblCounterParty.Text = "CounterParty";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label8.Location = new System.Drawing.Point(8, 135);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(86, 14);
			this.label8.TabIndex = 5;
			this.label8.Text = "CMTA/GiveUp";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label10.Location = new System.Drawing.Point(8, 113);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(132, 14);
			this.label10.TabIndex = 4;
			this.label10.Text = "On Behalf of Comp ID";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.Location = new System.Drawing.Point(8, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 14);
			this.label3.TabIndex = 2;
			this.label3.Text = "CounterPartyVenue";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label4.Location = new System.Drawing.Point(8, 69);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 14);
			this.label4.TabIndex = 3;
			this.label4.Text = "Funds";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label7.Location = new System.Drawing.Point(8, 91);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 14);
			this.label7.TabIndex = 38;
			this.label7.Text = "Strategy";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label9.ForeColor = System.Drawing.Color.Red;
			this.label9.Location = new System.Drawing.Point(64, 92);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(12, 10);
			this.label9.TabIndex = 38;
			this.label9.Text = "*";
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label17.ForeColor = System.Drawing.Color.Red;
			this.label17.Location = new System.Drawing.Point(6, 162);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(98, 16);
			this.label17.TabIndex = 178;
			this.label17.Text = "* Required Field";
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(153)), ((System.Byte)(153)), ((System.Byte)(255)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(144, 160);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 12;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Red;
			this.label2.Location = new System.Drawing.Point(128, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 8);
			this.label2.TabIndex = 35;
			this.label2.Text = "*";
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(52, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 10);
			this.label5.TabIndex = 36;
			this.label5.Text = "*";
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.Color.Red;
			this.label6.Location = new System.Drawing.Point(142, 116);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(12, 8);
			this.label6.TabIndex = 37;
			this.label6.Text = "*";
			// 
			// cmbCounterPartyVenue
			// 
			this.cmbCounterPartyVenue.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance1.BackColor = System.Drawing.SystemColors.Window;
			appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCounterPartyVenue.DisplayLayout.Appearance = appearance1;
			this.cmbCounterPartyVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCounterPartyVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance2.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCounterPartyVenue.DisplayLayout.GroupByBox.Appearance = appearance2;
			appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCounterPartyVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
			this.cmbCounterPartyVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance4.BackColor2 = System.Drawing.SystemColors.Control;
			appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCounterPartyVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
			this.cmbCounterPartyVenue.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCounterPartyVenue.DisplayLayout.MaxRowScrollRegions = 1;
			appearance5.BackColor = System.Drawing.SystemColors.Window;
			appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCounterPartyVenue.DisplayLayout.Override.ActiveCellAppearance = appearance5;
			appearance6.BackColor = System.Drawing.SystemColors.Highlight;
			appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCounterPartyVenue.DisplayLayout.Override.ActiveRowAppearance = appearance6;
			this.cmbCounterPartyVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCounterPartyVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance7.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCounterPartyVenue.DisplayLayout.Override.CardAreaAppearance = appearance7;
			appearance8.BorderColor = System.Drawing.Color.Silver;
			appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCounterPartyVenue.DisplayLayout.Override.CellAppearance = appearance8;
			this.cmbCounterPartyVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCounterPartyVenue.DisplayLayout.Override.CellPadding = 0;
			appearance9.BackColor = System.Drawing.SystemColors.Control;
			appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance9.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCounterPartyVenue.DisplayLayout.Override.GroupByRowAppearance = appearance9;
			appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCounterPartyVenue.DisplayLayout.Override.HeaderAppearance = appearance10;
			this.cmbCounterPartyVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCounterPartyVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance11.BackColor = System.Drawing.SystemColors.Window;
			appearance11.BorderColor = System.Drawing.Color.Silver;
			this.cmbCounterPartyVenue.DisplayLayout.Override.RowAppearance = appearance11;
			this.cmbCounterPartyVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCounterPartyVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
			this.cmbCounterPartyVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCounterPartyVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCounterPartyVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCounterPartyVenue.DisplayMember = "";
			this.cmbCounterPartyVenue.FlatMode = true;
			this.cmbCounterPartyVenue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbCounterPartyVenue.Location = new System.Drawing.Point(158, 40);
			this.cmbCounterPartyVenue.Name = "cmbCounterPartyVenue";
			this.cmbCounterPartyVenue.Size = new System.Drawing.Size(174, 20);
			this.cmbCounterPartyVenue.TabIndex = 179;
			this.cmbCounterPartyVenue.ValueMember = "";
			this.cmbCounterPartyVenue.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbCounterPartyVenue_InitializeLayout);
			// 
			// cmbFunds
			// 
			this.cmbFunds.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance13.BackColor = System.Drawing.SystemColors.Window;
			appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbFunds.DisplayLayout.Appearance = appearance13;
			this.cmbFunds.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbFunds.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance14.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbFunds.DisplayLayout.GroupByBox.Appearance = appearance14;
			appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbFunds.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
			this.cmbFunds.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance16.BackColor2 = System.Drawing.SystemColors.Control;
			appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbFunds.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
			this.cmbFunds.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbFunds.DisplayLayout.MaxRowScrollRegions = 1;
			appearance17.BackColor = System.Drawing.SystemColors.Window;
			appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbFunds.DisplayLayout.Override.ActiveCellAppearance = appearance17;
			appearance18.BackColor = System.Drawing.SystemColors.Highlight;
			appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbFunds.DisplayLayout.Override.ActiveRowAppearance = appearance18;
			this.cmbFunds.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbFunds.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance19.BackColor = System.Drawing.SystemColors.Window;
			this.cmbFunds.DisplayLayout.Override.CardAreaAppearance = appearance19;
			appearance20.BorderColor = System.Drawing.Color.Silver;
			appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbFunds.DisplayLayout.Override.CellAppearance = appearance20;
			this.cmbFunds.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbFunds.DisplayLayout.Override.CellPadding = 0;
			appearance21.BackColor = System.Drawing.SystemColors.Control;
			appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance21.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbFunds.DisplayLayout.Override.GroupByRowAppearance = appearance21;
			appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbFunds.DisplayLayout.Override.HeaderAppearance = appearance22;
			this.cmbFunds.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbFunds.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance23.BackColor = System.Drawing.SystemColors.Window;
			appearance23.BorderColor = System.Drawing.Color.Silver;
			this.cmbFunds.DisplayLayout.Override.RowAppearance = appearance23;
			this.cmbFunds.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbFunds.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
			this.cmbFunds.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbFunds.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbFunds.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbFunds.DisplayMember = "";
			this.cmbFunds.FlatMode = true;
			this.cmbFunds.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbFunds.Location = new System.Drawing.Point(158, 62);
			this.cmbFunds.Name = "cmbFunds";
			this.cmbFunds.Size = new System.Drawing.Size(174, 20);
			this.cmbFunds.TabIndex = 180;
			this.cmbFunds.ValueMember = "";
			// 
			// cmbStrategy
			// 
			this.cmbStrategy.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance25.BackColor = System.Drawing.SystemColors.Window;
			appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbStrategy.DisplayLayout.Appearance = appearance25;
			this.cmbStrategy.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbStrategy.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance26.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbStrategy.DisplayLayout.GroupByBox.Appearance = appearance26;
			appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbStrategy.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
			this.cmbStrategy.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance28.BackColor2 = System.Drawing.SystemColors.Control;
			appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbStrategy.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
			this.cmbStrategy.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbStrategy.DisplayLayout.MaxRowScrollRegions = 1;
			appearance29.BackColor = System.Drawing.SystemColors.Window;
			appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbStrategy.DisplayLayout.Override.ActiveCellAppearance = appearance29;
			appearance30.BackColor = System.Drawing.SystemColors.Highlight;
			appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbStrategy.DisplayLayout.Override.ActiveRowAppearance = appearance30;
			this.cmbStrategy.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbStrategy.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance31.BackColor = System.Drawing.SystemColors.Window;
			this.cmbStrategy.DisplayLayout.Override.CardAreaAppearance = appearance31;
			appearance32.BorderColor = System.Drawing.Color.Silver;
			appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbStrategy.DisplayLayout.Override.CellAppearance = appearance32;
			this.cmbStrategy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbStrategy.DisplayLayout.Override.CellPadding = 0;
			appearance33.BackColor = System.Drawing.SystemColors.Control;
			appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance33.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbStrategy.DisplayLayout.Override.GroupByRowAppearance = appearance33;
			appearance34.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbStrategy.DisplayLayout.Override.HeaderAppearance = appearance34;
			this.cmbStrategy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbStrategy.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance35.BackColor = System.Drawing.SystemColors.Window;
			appearance35.BorderColor = System.Drawing.Color.Silver;
			this.cmbStrategy.DisplayLayout.Override.RowAppearance = appearance35;
			this.cmbStrategy.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbStrategy.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
			this.cmbStrategy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbStrategy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbStrategy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbStrategy.DisplayMember = "";
			this.cmbStrategy.FlatMode = true;
			this.cmbStrategy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbStrategy.Location = new System.Drawing.Point(158, 84);
			this.cmbStrategy.Name = "cmbStrategy";
			this.cmbStrategy.Size = new System.Drawing.Size(174, 20);
			this.cmbStrategy.TabIndex = 181;
			this.cmbStrategy.ValueMember = "";
			// 
			// cmbCMTAGiveUp
			// 
			this.cmbCMTAGiveUp.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance37.BackColor = System.Drawing.SystemColors.Window;
			appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCMTAGiveUp.DisplayLayout.Appearance = appearance37;
			this.cmbCMTAGiveUp.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCMTAGiveUp.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance38.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.Appearance = appearance38;
			appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance40.BackColor2 = System.Drawing.SystemColors.Control;
			appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
			this.cmbCMTAGiveUp.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCMTAGiveUp.DisplayLayout.MaxRowScrollRegions = 1;
			appearance41.BackColor = System.Drawing.SystemColors.Window;
			appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCMTAGiveUp.DisplayLayout.Override.ActiveCellAppearance = appearance41;
			appearance42.BackColor = System.Drawing.SystemColors.Highlight;
			appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCMTAGiveUp.DisplayLayout.Override.ActiveRowAppearance = appearance42;
			this.cmbCMTAGiveUp.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCMTAGiveUp.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance43.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CardAreaAppearance = appearance43;
			appearance44.BorderColor = System.Drawing.Color.Silver;
			appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CellAppearance = appearance44;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CellPadding = 0;
			appearance45.BackColor = System.Drawing.SystemColors.Control;
			appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance45.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCMTAGiveUp.DisplayLayout.Override.GroupByRowAppearance = appearance45;
			appearance46.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderAppearance = appearance46;
			this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance47.BackColor = System.Drawing.SystemColors.Window;
			appearance47.BorderColor = System.Drawing.Color.Silver;
			this.cmbCMTAGiveUp.DisplayLayout.Override.RowAppearance = appearance47;
			this.cmbCMTAGiveUp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCMTAGiveUp.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
			this.cmbCMTAGiveUp.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCMTAGiveUp.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCMTAGiveUp.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCMTAGiveUp.DisplayMember = "";
			this.cmbCMTAGiveUp.FlatMode = true;
			this.cmbCMTAGiveUp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbCMTAGiveUp.Location = new System.Drawing.Point(158, 128);
			this.cmbCMTAGiveUp.Name = "cmbCMTAGiveUp";
			this.cmbCMTAGiveUp.Size = new System.Drawing.Size(174, 20);
			this.cmbCMTAGiveUp.TabIndex = 182;
			this.cmbCMTAGiveUp.ValueMember = "";
			this.cmbCMTAGiveUp.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbCMTAGiveUp_InitializeLayout);
			// 
			// cmbCounterParty
			// 
			this.cmbCounterParty.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance49.BackColor = System.Drawing.SystemColors.Window;
			appearance49.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCounterParty.DisplayLayout.Appearance = appearance49;
			this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance50.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance50.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance50;
			appearance51.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance51;
			this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance52.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance52.BackColor2 = System.Drawing.SystemColors.Control;
			appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance52.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance52;
			this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
			appearance53.BackColor = System.Drawing.SystemColors.Window;
			appearance53.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance53;
			appearance54.BackColor = System.Drawing.SystemColors.Highlight;
			appearance54.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance54;
			this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance55.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance55;
			appearance56.BorderColor = System.Drawing.Color.Silver;
			appearance56.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance56;
			this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
			appearance57.BackColor = System.Drawing.SystemColors.Control;
			appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance57.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance57;
			appearance58.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance58;
			this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance59.BackColor = System.Drawing.SystemColors.Window;
			appearance59.BorderColor = System.Drawing.Color.Silver;
			this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance59;
			this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance60.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance60;
			this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCounterParty.DisplayMember = "";
			this.cmbCounterParty.FlatMode = true;
			this.cmbCounterParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.cmbCounterParty.Location = new System.Drawing.Point(158, 20);
			this.cmbCounterParty.Name = "cmbCounterParty";
			this.cmbCounterParty.Size = new System.Drawing.Size(174, 20);
			this.cmbCounterParty.TabIndex = 179;
			this.cmbCounterParty.ValueMember = "";
			// 
			// CreateCompanyCounterPartyFundLevelTags
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label17);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CreateCompanyCounterPartyFundLevelTags";
			this.Size = new System.Drawing.Size(372, 186);
			this.Load += new System.EventHandler(this.CreateCompanyCounterPartyFundLevelTags_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyVenue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbFunds)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCMTAGiveUp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void BindCounterParty()
		{
			
			CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(_companyID);
//			if (counterParties.Count > 0 )
//			{
				counterParties.Insert(0, new CounterParty(int.MinValue, C_COMBO_SELECT));
				cmbCounterParty.DataSource = counterParties;				
				cmbCounterParty.DisplayMember = "CounterPartyFullName";
				cmbCounterParty.ValueMember = "CounterPartyID";
//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCounterParty.Value= _companyCounterPartyVenueDetailEdit.CompanyCounterPartyID;
			}
		}
		
		private void BindCounterPartyVenues()
		{
			cmbCounterPartyVenue.DataSource = null;
			Venues venues = VenueManager.GetCounterPartyVenueNames(_companyCounterPartyID);
//			if (venues.Count > 0 )
//			{
				venues.Insert(0, new Venue(int.MinValue, C_COMBO_SELECT));
				cmbCounterPartyVenue.DataSource = venues;				
				cmbCounterPartyVenue.DisplayMember = "VenueName";
				cmbCounterPartyVenue.ValueMember = "VenueID";
//			}
		}

		private void BindFunds()
		{
			cmbFunds.DataSource = null;
			Funds funds = CompanyManager.GetFund(_companyID);
//			if (funds.Count > 0 )
//			{
				funds.Insert(0, new Fund(int.MinValue, C_COMBO_SELECT));
				cmbFunds.DataSource = funds;				
				cmbFunds.DisplayMember = "FundName";
				cmbFunds.ValueMember = "FundID";
//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbFunds.Value = _companyCounterPartyVenueDetailEdit.FundID;
			}
		}

		private void BindStrategies()
		{
			cmbStrategy.DataSource = null;
			Strategies strategies = CompanyManager.GetStrategy(_companyID);
			//			if (funds.Count > 0 )
			//			{
			strategies.Insert(0, new Strategy(int.MinValue, C_COMBO_SELECT));
			cmbStrategy.DataSource = strategies;				
			cmbStrategy.DisplayMember = "StrategyName";
			cmbStrategy.ValueMember = "StrategyID";
			//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbStrategy.Value = _companyCounterPartyVenueDetailEdit.StrategyID;
			}
		}
		
		private void BindIdentifiers()
		{
			Identifiers identifiers = AUECManager.GetIdentifiers();
//			if (identifiers.Count > 0 )
//			{
				identifiers.Insert(0, new Identifier(int.MinValue, C_COMBO_SELECT));
				cmbCMTAGiveUp.DataSource = identifiers;				
				cmbCMTAGiveUp.DisplayMember = "IdentifierName";
				cmbCMTAGiveUp.ValueMember = "IdentifierID";
//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCMTAGiveUp.Value = _companyCounterPartyVenueDetailEdit.CMTAGiveUp;
			}
		}
		
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			RefreshForm();

		}

		Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail _companyCounterPartyVenueDetailEdit = null;
		public Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail CompanyCounterPartyVenueDetailEdit
		{
			set{_companyCounterPartyVenueDetailEdit = value;}
		}

		public void BindForEdit()
		{
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCounterParty.Value = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID.ToString());
				cmbCounterPartyVenue.Value= int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID.ToString());	
				cmbFunds.Value = int.Parse(_companyCounterPartyVenueDetailEdit.FundID.ToString());
				cmbStrategy.Value = int.Parse(_companyCounterPartyVenueDetailEdit.StrategyID.ToString());
				cmbCMTAGiveUp.Value = int.Parse(_companyCounterPartyVenueDetailEdit.CMTAGiveUp.ToString());
				

			}
		}

		private int _counterPartyVenueID = int.MinValue;
		public int CounterPartyVenueID
		{
			set
			{
				_counterPartyVenueID = value;
				
				cmbCounterPartyVenue.Value = _counterPartyVenueID;
				txtCompID.Text = "uuuuuuu";
				cmbCounterPartyVenue.Enabled = false;
			}
		}
		
		private int _noData = int.MinValue;
		public int NoData
		{
			set
			{
				_noData = value;
			}
		}
		
		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set
			{
				_companyID = value;
				BindFunds();
				BindIdentifiers();
				BindStrategies();
			}
		}
	
		private int _companyCounterPartyID = int.MinValue;
		public int CompanyCounterPartyID
		{
			set
			{
				_companyCounterPartyID = value; 
				CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail(_companyCounterPartyID);
				//lblCounterParty.Text = _companyCounterPartyID.ToString();
				lblCounterParty.Text = companyCounterPartyVenueDetail.CounterPartyFullName;
				BindCounterPartyVenues();
			}
			
		}

		public event EventHandler SaveData;
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (_noData == 1)
			{
				_companyCounterPartyVenueDetails.Clear();
			}
			else
			{
				_noData = 0;
			}
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				errorProvider1.SetError(cmbCMTAGiveUp, "");
				errorProvider1.SetError(cmbCounterPartyVenue, "");
				errorProvider1.SetError(cmbFunds, "");
				errorProvider1.SetError(cmbStrategy, "");
				if(int.Parse(cmbCounterPartyVenue.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbFunds.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbFunds, "Please select Funds!");
					cmbFunds.Focus();
				}
				else if(int.Parse(cmbStrategy.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbStrategy, "Please select Strategy!");
					cmbStrategy.Focus();
				}
				else if(int.Parse(cmbCMTAGiveUp.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCMTAGiveUp, "Please select CMTA/GiveUp!");
					cmbCMTAGiveUp.Focus();
				}
				else
				{
					//_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
					_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = _companyCounterPartyID;
					_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID = int.Parse(cmbCounterPartyVenue.Value.ToString());
					_companyCounterPartyVenueDetailEdit.FundID = int.Parse(cmbFunds.Value.ToString());
					_companyCounterPartyVenueDetailEdit.StrategyID = int.Parse(cmbStrategy.Value.ToString());
					
					_companyCounterPartyVenueDetailEdit.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.Value.ToString());
										
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");

					RefreshForm();
				}
			}
			else
			{
				errorProvider1.SetError(cmbCMTAGiveUp, "");
				errorProvider1.SetError(cmbCounterPartyVenue, "");
				errorProvider1.SetError(cmbFunds, "");
				errorProvider1.SetError(cmbStrategy, "");
				if(int.Parse(cmbCounterPartyVenue.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbFunds.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbFunds, "Please select Funds!");
					cmbFunds.Focus();
				}
				else if(int.Parse(cmbStrategy.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbStrategy, "Please select Strategy!");
					cmbStrategy.Focus();
				}
				else if(int.Parse(cmbCMTAGiveUp.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCMTAGiveUp, "Please select CMTA/GiveUp!");
					cmbCMTAGiveUp.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail();
					
					//companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyID = _companyCounterPartyID;
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(cmbCounterPartyVenue.Value.ToString());
					companyCounterPartyVenueDetail.FundID = int.Parse(cmbFunds.Value.ToString());
					companyCounterPartyVenueDetail.StrategyID = int.Parse(cmbStrategy.Value.ToString());
					
					companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.Value.ToString());
					_companyCounterPartyVenueDetails.Add(companyCounterPartyVenueDetail);					
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");

					RefreshForm();
				}
			}
			SaveData(_companyCounterPartyVenueDetails, e);
			//this.Hide();
		}
		
		public void RefreshForm()
		{
			cmbCMTAGiveUp.Value = int.MinValue;
			cmbCounterParty.Value= int.MinValue;
			//cmbCounterPartyVenue.SelectedValue = int.MinValue;
			cmbCounterPartyVenue.Value = _counterPartyVenueID; 
			//cmbCounterPartyVenue.Enabled = false;
			cmbFunds.Value = int.MinValue;
			cmbStrategy.Value = int.MinValue;
			txtCompID.Text = "";
		}

		//CreateCompanyCounterPartiesCompanyLevelTags createCompanyCounterPartiesCompanyLevelTags = new CreateCompanyCounterPartiesCompanyLevelTags();
		private void CreateCompanyCounterPartyFundLevelTags_Load(object sender, System.EventArgs e)
		{
			int cpvid = _counterPartyVenueID;
			//int cpvid1 = createCompanyCounterPartiesCompanyLevelTags.CompanyCounterPartyVenueID;
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				BindForEdit();
			}
			else
			{
				RefreshForm();
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
			}
		}
		
		private Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails _companyCounterPartyVenueDetails = new  Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails();
		public Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails CurrentCompanyCounterPartyVenueDetails
		{
			get 
			{
				return _companyCounterPartyVenueDetails; 
			}
			set
			{
				if(value != null)
				{
					_companyCounterPartyVenueDetails = value;
					BindForEdit();
				}
			}			
		}

		#region Focus Colors
		private void txtCompID_GotFocus(object sender, System.EventArgs e)
		{
			txtCompID.BackColor = Color.LemonChiffon;
		}
		private void txtCompID_LostFocus(object sender, System.EventArgs e)
		{
			txtCompID.BackColor = Color.White;
		}
		private void cmbCounterPartyVenue_GotFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.LemonChiffon;
		}
		private void cmbCounterPartyVenue_LostFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.White;
		}
		private void cmbCMTAGiveUp_GotFocus(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.BackColor = Color.LemonChiffon;
		}
		private void cmbCMTAGiveUp_LostFocus(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.BackColor = Color.White;
		}
		private void cmbFunds_GotFocus(object sender, System.EventArgs e)
		{
			cmbFunds.BackColor = Color.LemonChiffon;
		}
		private void cmbFunds_LostFocus(object sender, System.EventArgs e)
		{
			cmbFunds.BackColor = Color.White;
		}
		#endregion

		private void cmbCMTAGiveUp_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
		
		}

		private void cmbCounterPartyVenue_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
		
		}
	}
}
