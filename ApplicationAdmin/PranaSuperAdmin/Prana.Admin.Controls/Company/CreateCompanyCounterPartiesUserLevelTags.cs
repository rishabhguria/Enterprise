using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CompanyCounterPartiesUserLevelTags.
	/// </summary>
	public class CreateCompanyCounterPartiesUserLevelTags : System.Windows.Forms.UserControl
	{
		const string C_COMBO_SELECT = "- Select -";
		
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbCounterParty;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtSubID;
		private System.Windows.Forms.Label lblCounterParty;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label17;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterPartyVenue;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbUsers;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbCMTAGiveUp;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCompanyCounterPartiesUserLevelTags()
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
			BindUsers();
			BindIdentifiers();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CreateCompanyCounterPartiesUserLevelTags));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblCounterParty = new System.Windows.Forms.Label();
			this.txtSubID = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbCounterParty = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.cmbCounterPartyVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbUsers = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.cmbCMTAGiveUp = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyVenue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCMTAGiveUp)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblCounterParty);
			this.groupBox1.Controls.Add(this.txtSubID);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.cmbCounterParty);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.cmbCounterPartyVenue);
			this.groupBox1.Controls.Add(this.cmbUsers);
			this.groupBox1.Controls.Add(this.cmbCMTAGiveUp);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.groupBox1.Location = new System.Drawing.Point(2, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(312, 142);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "User Level Details";
			// 
			// lblCounterParty
			// 
			this.lblCounterParty.BackColor = System.Drawing.Color.White;
			this.lblCounterParty.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblCounterParty.Location = new System.Drawing.Point(132, 20);
			this.lblCounterParty.Name = "lblCounterParty";
			this.lblCounterParty.Size = new System.Drawing.Size(172, 22);
			this.lblCounterParty.TabIndex = 14;
			// 
			// txtSubID
			// 
			this.txtSubID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtSubID.Location = new System.Drawing.Point(132, 92);
			this.txtSubID.MaxLength = 50;
			this.txtSubID.Name = "txtSubID";
			this.txtSubID.Size = new System.Drawing.Size(172, 21);
			this.txtSubID.TabIndex = 10;
			this.txtSubID.Text = "";
			this.txtSubID.LostFocus += new System.EventHandler(this.txtSubID_LostFocus);
			this.txtSubID.GotFocus += new System.EventHandler(this.txtSubID_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.Location = new System.Drawing.Point(6, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "CounterParty";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label9.Location = new System.Drawing.Point(6, 117);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(76, 14);
			this.label9.TabIndex = 6;
			this.label9.Text = "CMTA/Give Up";
			// 
			// cmbCounterParty
			// 
			this.cmbCounterParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCounterParty.Location = new System.Drawing.Point(130, 20);
			this.cmbCounterParty.Name = "cmbCounterParty";
			this.cmbCounterParty.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterParty.TabIndex = 7;
			this.cmbCounterParty.Visible = false;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label10.Location = new System.Drawing.Point(6, 95);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(106, 14);
			this.label10.TabIndex = 4;
			this.label10.Text = "On Behalf of Sub ID";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.Location = new System.Drawing.Point(6, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 14);
			this.label3.TabIndex = 2;
			this.label3.Text = "CounterPartyVenue";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label4.Location = new System.Drawing.Point(6, 71);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(34, 14);
			this.label4.TabIndex = 3;
			this.label4.Text = "Users";
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(38, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 8);
			this.label5.TabIndex = 36;
			this.label5.Text = "*";
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label17.ForeColor = System.Drawing.Color.Red;
			this.label17.Location = new System.Drawing.Point(2, 148);
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
			this.btnSave.Location = new System.Drawing.Point(122, 148);
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
			this.label2.Location = new System.Drawing.Point(112, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 8);
			this.label2.TabIndex = 35;
			this.label2.Text = "*";
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.Color.Red;
			this.label6.Location = new System.Drawing.Point(112, 100);
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
			ultraGridBand1.ColHeadersVisible = false;
			this.cmbCounterPartyVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
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
			this.cmbCounterPartyVenue.Location = new System.Drawing.Point(132, 44);
			this.cmbCounterPartyVenue.Name = "cmbCounterPartyVenue";
			this.cmbCounterPartyVenue.Size = new System.Drawing.Size(172, 22);
			this.cmbCounterPartyVenue.TabIndex = 179;
			this.cmbCounterPartyVenue.ValueMember = "";
			// 
			// cmbUsers
			// 
			this.cmbUsers.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance13.BackColor = System.Drawing.SystemColors.Window;
			appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbUsers.DisplayLayout.Appearance = appearance13;
			ultraGridBand2.ColHeadersVisible = false;
			this.cmbUsers.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
			this.cmbUsers.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbUsers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance14.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbUsers.DisplayLayout.GroupByBox.Appearance = appearance14;
			appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbUsers.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
			this.cmbUsers.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance16.BackColor2 = System.Drawing.SystemColors.Control;
			appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbUsers.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
			this.cmbUsers.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbUsers.DisplayLayout.MaxRowScrollRegions = 1;
			appearance17.BackColor = System.Drawing.SystemColors.Window;
			appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbUsers.DisplayLayout.Override.ActiveCellAppearance = appearance17;
			appearance18.BackColor = System.Drawing.SystemColors.Highlight;
			appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbUsers.DisplayLayout.Override.ActiveRowAppearance = appearance18;
			this.cmbUsers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbUsers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance19.BackColor = System.Drawing.SystemColors.Window;
			this.cmbUsers.DisplayLayout.Override.CardAreaAppearance = appearance19;
			appearance20.BorderColor = System.Drawing.Color.Silver;
			appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbUsers.DisplayLayout.Override.CellAppearance = appearance20;
			this.cmbUsers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbUsers.DisplayLayout.Override.CellPadding = 0;
			appearance21.BackColor = System.Drawing.SystemColors.Control;
			appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance21.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbUsers.DisplayLayout.Override.GroupByRowAppearance = appearance21;
			appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbUsers.DisplayLayout.Override.HeaderAppearance = appearance22;
			this.cmbUsers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbUsers.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance23.BackColor = System.Drawing.SystemColors.Window;
			appearance23.BorderColor = System.Drawing.Color.Silver;
			this.cmbUsers.DisplayLayout.Override.RowAppearance = appearance23;
			this.cmbUsers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbUsers.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
			this.cmbUsers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbUsers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbUsers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbUsers.DisplayMember = "";
			this.cmbUsers.FlatMode = true;
			this.cmbUsers.Location = new System.Drawing.Point(132, 68);
			this.cmbUsers.Name = "cmbUsers";
			this.cmbUsers.Size = new System.Drawing.Size(172, 22);
			this.cmbUsers.TabIndex = 180;
			this.cmbUsers.ValueMember = "";
			// 
			// cmbCMTAGiveUp
			// 
			this.cmbCMTAGiveUp.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			appearance25.BackColor = System.Drawing.SystemColors.Window;
			appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.cmbCMTAGiveUp.DisplayLayout.Appearance = appearance25;
			ultraGridBand3.ColHeadersVisible = false;
			this.cmbCMTAGiveUp.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
			this.cmbCMTAGiveUp.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.cmbCMTAGiveUp.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance26.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.Appearance = appearance26;
			appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance28.BackColor2 = System.Drawing.SystemColors.Control;
			appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
			this.cmbCMTAGiveUp.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
			this.cmbCMTAGiveUp.DisplayLayout.MaxColScrollRegions = 1;
			this.cmbCMTAGiveUp.DisplayLayout.MaxRowScrollRegions = 1;
			appearance29.BackColor = System.Drawing.SystemColors.Window;
			appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbCMTAGiveUp.DisplayLayout.Override.ActiveCellAppearance = appearance29;
			appearance30.BackColor = System.Drawing.SystemColors.Highlight;
			appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.cmbCMTAGiveUp.DisplayLayout.Override.ActiveRowAppearance = appearance30;
			this.cmbCMTAGiveUp.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.cmbCMTAGiveUp.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance31.BackColor = System.Drawing.SystemColors.Window;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CardAreaAppearance = appearance31;
			appearance32.BorderColor = System.Drawing.Color.Silver;
			appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CellAppearance = appearance32;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.cmbCMTAGiveUp.DisplayLayout.Override.CellPadding = 0;
			appearance33.BackColor = System.Drawing.SystemColors.Control;
			appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance33.BorderColor = System.Drawing.SystemColors.Window;
			this.cmbCMTAGiveUp.DisplayLayout.Override.GroupByRowAppearance = appearance33;
			appearance34.TextHAlign = Infragistics.Win.HAlign.Left;
			this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderAppearance = appearance34;
			this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.cmbCMTAGiveUp.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance35.BackColor = System.Drawing.SystemColors.Window;
			appearance35.BorderColor = System.Drawing.Color.Silver;
			this.cmbCMTAGiveUp.DisplayLayout.Override.RowAppearance = appearance35;
			this.cmbCMTAGiveUp.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmbCMTAGiveUp.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
			this.cmbCMTAGiveUp.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.cmbCMTAGiveUp.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.cmbCMTAGiveUp.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.cmbCMTAGiveUp.DisplayMember = "";
			this.cmbCMTAGiveUp.FlatMode = true;
			this.cmbCMTAGiveUp.Location = new System.Drawing.Point(132, 115);
			this.cmbCMTAGiveUp.Name = "cmbCMTAGiveUp";
			this.cmbCMTAGiveUp.Size = new System.Drawing.Size(172, 22);
			this.cmbCMTAGiveUp.TabIndex = 181;
			this.cmbCMTAGiveUp.ValueMember = "";
			// 
			// CreateCompanyCounterPartiesUserLevelTags
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label17);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CreateCompanyCounterPartiesUserLevelTags";
			this.Size = new System.Drawing.Size(320, 174);
			this.Load += new System.EventHandler(this.CreateCompanyCounterPartiesUserLevelTags_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbCounterPartyVenue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmbCMTAGiveUp)).EndInit();
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
				//cmbCounterParty.SelectedValue = _companyCounterPartyVenueDetailEdit.CompanyCounterPartyID;
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
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCounterPartyVenue.Value = _companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID;
			}
		}
		
		private void BindUsers()
		{
			cmbUsers.DataSource = null;
			Users users = UserManager.GetUsers(_companyID);
//			if (users.Count > 0 )
//			{
				users.Insert(0, new User(int.MinValue, C_COMBO_SELECT));
				cmbUsers.DataSource = users;				
				cmbUsers.DisplayMember = "FirstName";
				cmbUsers.ValueMember = "UserID";
//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				//cmbUsers.SelectedValue = _companyCounterPartyVenueDetailEdit.UserID;
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
			Refresh(sender, e);
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
				cmbCounterParty.Value= int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID.ToString());
				cmbCounterPartyVenue.Value = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID.ToString());	
				txtSubID.Text = _companyCounterPartyVenueDetailEdit.OnBehalfOfSubID.ToString();
				cmbCMTAGiveUp.Value = int.Parse(_companyCounterPartyVenueDetailEdit.CMTAGiveUp.ToString());
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
				BindIdentifiers();
				BindUsers();
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
				errorProvider1.SetError(cmbUsers, "");
				if(int.Parse(cmbCounterPartyVenue.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbUsers.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbUsers, "Please select User!");
					cmbUsers.Focus();
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
					_companyCounterPartyVenueDetailEdit.OnBehalfOfSubID = txtSubID.Text.ToString();
					_companyCounterPartyVenueDetailEdit.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.Value.ToString());
										
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");
					Refresh(sender, e);
				}
			}
			else
			{
				errorProvider1.SetError(cmbCMTAGiveUp, "");
				errorProvider1.SetError(cmbCounterPartyVenue, "");
				errorProvider1.SetError(cmbUsers, "");
				if(int.Parse(cmbCounterPartyVenue.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbUsers.Value.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbUsers, "Please select User!");
					cmbUsers.Focus();
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
					companyCounterPartyVenueDetail.OnBehalfOfSubID = txtSubID.Text.ToString();
					companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.Value.ToString());
					_companyCounterPartyVenueDetails.Add(companyCounterPartyVenueDetail);
					
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");

					Refresh(sender, e);
				}
			}
			SaveData(_companyCounterPartyVenueDetails, e);
			//this.Hide();
		}

		public void Refresh(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.Value= int.MinValue;
			cmbCounterParty.Value = int.MinValue;
			cmbCounterPartyVenue.Value= int.MinValue;
			cmbUsers.Value = int.MinValue;
			txtSubID.Text = "";
		}

		private void CreateCompanyCounterPartiesUserLevelTags_Load(object sender, System.EventArgs e)
		{
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
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
		private void cmbCMTAGiveUp_GotFocus(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.BackColor = Color.LemonChiffon;
		}
		private void cmbCMTAGiveUp_LostFocus(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.BackColor = Color.White;
		}
		private void cmbCounterPartyVenue_GotFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.LemonChiffon;
		}
		private void cmbCounterPartyVenue_LostFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.White;
		}
		private void cmbUsers_GotFocus(object sender, System.EventArgs e)
		{
			cmbUsers.BackColor = Color.LemonChiffon;
		}
		private void cmbUsers_LostFocus(object sender, System.EventArgs e)
		{
			cmbUsers.BackColor = Color.White;
		}
		private void txtSubID_GotFocus(object sender, System.EventArgs e)
		{
			txtSubID.BackColor = Color.LemonChiffon;
		}
		private void txtSubID_LostFocus(object sender, System.EventArgs e)
		{
			txtSubID.BackColor = Color.White;
		}
		
		#endregion
	}
}
