#region Using
using Prana.Admin.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for ClientCompany.
    /// </summary>
    public class ClientCompany : System.Windows.Forms.UserControl
    {
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "ClientCompany : ";
        #region Private and Protected members.

        private System.Windows.Forms.GroupBox gpbDetails;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.TextBox txtAddress2;
        private System.Windows.Forms.TextBox txtAddress1;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gpbSecondaryContact;
        private System.Windows.Forms.TextBox txtPC2Telephone;
        private System.Windows.Forms.TextBox txtPC2Cell;
        private System.Windows.Forms.TextBox txtPC2Title;
        private System.Windows.Forms.TextBox txtPC2LastName;
        private System.Windows.Forms.TextBox txtPC2FirstName;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox txtPC2Email;
        private System.Windows.Forms.GroupBox gpbPrimaryContact;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtPC1FirstName;
        private System.Windows.Forms.TextBox txtPC1LastName;
        private System.Windows.Forms.TextBox txtPC1Title;
        private System.Windows.Forms.TextBox txtPC1Cell;
        private System.Windows.Forms.TextBox txtPC1Telephone;
        private System.Windows.Forms.TextBox txtPC1Email;

        //private int _companyID = int.MinValue;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCompanyTYpe;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;

        #endregion
        private Label label24;
        private TextBox txtShortName;
        private Label lblShortName;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbState;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCountry;
        private Label label25;
        private Label lblZip;
        private TextBox txtZip;
        private Label lblStateTerritory;
        private Label label26;
        private Label lblCountry;
        private Label label27;
        private IContainer components;

        //		public int CompanyID
        //		{
        //			set{_companyID = value;}
        //		}

        public ClientCompany()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            //BindCompanyType();
            // TODO: Add any initialization after the InitializeComponent call
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
                if (gpbDetails != null)
                {
                    gpbDetails.Dispose();
                }
                if (txtTelephone != null)
                {
                    txtTelephone.Dispose();
                }
                if (txtFax != null)
                {
                    txtFax.Dispose();
                }
                if (txtAddress2 != null)
                {
                    txtAddress2.Dispose();
                }
                if (txtAddress1 != null)
                {
                    txtAddress1.Dispose();
                }
                if (txtCompanyName != null)
                {
                    txtCompanyName.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (gpbSecondaryContact != null)
                {
                    gpbSecondaryContact.Dispose();
                }
                if (txtPC2Telephone != null)
                {
                    txtPC2Telephone.Dispose();
                }
                if (txtPC2Cell != null)
                {
                    txtPC2Cell.Dispose();
                }
                if (txtPC2Title != null)
                {
                    txtPC2Title.Dispose();
                }
                if (txtPC2LastName != null)
                {
                    txtPC2LastName.Dispose();
                }

                if (txtPC2FirstName != null)
                {
                    txtPC2FirstName.Dispose();
                }
                if (label30 != null)
                {
                    label30.Dispose();
                }
                if (label31 != null)
                {
                    label31.Dispose();
                }
                if (label32 != null)
                {
                    label32.Dispose();
                }
                if (label33 != null)
                {
                    label33.Dispose();
                }
                if (label34 != null)
                {
                    label34.Dispose();
                }
                if (label35 != null)
                {
                    label35.Dispose();
                }
                if (txtPC2Email != null)
                {
                    txtPC2Email.Dispose();
                }
                if (gpbPrimaryContact != null)
                {
                    gpbPrimaryContact.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (label15 != null)
                {
                    label15.Dispose();
                }
                if (label16 != null)
                {
                    label16.Dispose();
                }
                if (label17 != null)
                {
                    label17.Dispose();
                }
                if (label18 != null)
                {
                    label18.Dispose();
                }
                if (txtPC1FirstName != null)
                {
                    txtPC1FirstName.Dispose();
                }
                if (txtPC1LastName != null)
                {
                    txtPC1LastName.Dispose();
                }
                if (txtPC1Title != null)
                {
                    txtPC1Title.Dispose();
                }
                if (txtPC1Cell != null)
                {
                    txtPC1Cell.Dispose();
                }
                if (txtPC1Telephone != null)
                {
                    txtPC1Telephone.Dispose();
                }
                if (txtPC1Email != null)
                {
                    txtPC1Email.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label20 != null)
                {
                    label20.Dispose();
                }
                if (label21 != null)
                {
                    label21.Dispose();
                }
                if (cmbCompanyTYpe != null)
                {
                    cmbCompanyTYpe.Dispose();
                }
                if (label22 != null)
                {
                    label22.Dispose();
                }
                if (label23 != null)
                {
                    label23.Dispose();
                }
                if (label24 != null)
                {
                    label24.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (lblShortName != null)
                {
                    lblShortName.Dispose();
                }
                if (cmbState != null)
                {
                    cmbState.Dispose();
                }
                if (cmbCountry != null)
                {
                    cmbCountry.Dispose();
                }
                if (label25 != null)
                {
                    label25.Dispose();
                }
                if (lblZip != null)
                {
                    lblZip.Dispose();
                }
                if (txtZip != null)
                {
                    txtZip.Dispose();
                }
                if (lblStateTerritory != null)
                {
                    lblStateTerritory.Dispose();
                }
                if (label26 != null)
                {
                    label26.Dispose();
                }
                if (lblCountry != null)
                {
                    lblCountry.Dispose();
                }
                if (label27 != null)
                {
                    label27.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 2);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyTypeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1);
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
            this.gpbDetails = new System.Windows.Forms.GroupBox();
            this.cmbState = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCountry = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label25 = new System.Windows.Forms.Label();
            this.lblZip = new System.Windows.Forms.Label();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.lblStateTerritory = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.lblShortName = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.cmbCompanyTYpe = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label19 = new System.Windows.Forms.Label();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.txtAddress2 = new System.Windows.Forms.TextBox();
            this.txtAddress1 = new System.Windows.Forms.TextBox();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.gpbSecondaryContact = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtPC2Telephone = new System.Windows.Forms.TextBox();
            this.txtPC2Cell = new System.Windows.Forms.TextBox();
            this.txtPC2Title = new System.Windows.Forms.TextBox();
            this.txtPC2LastName = new System.Windows.Forms.TextBox();
            this.txtPC2FirstName = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.txtPC2Email = new System.Windows.Forms.TextBox();
            this.gpbPrimaryContact = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtPC1FirstName = new System.Windows.Forms.TextBox();
            this.txtPC1LastName = new System.Windows.Forms.TextBox();
            this.txtPC1Title = new System.Windows.Forms.TextBox();
            this.txtPC1Cell = new System.Windows.Forms.TextBox();
            this.txtPC1Telephone = new System.Windows.Forms.TextBox();
            this.txtPC1Email = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.gpbDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTYpe)).BeginInit();
            this.gpbSecondaryContact.SuspendLayout();
            this.gpbPrimaryContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbDetails
            // 
            this.gpbDetails.Controls.Add(this.cmbState);
            this.gpbDetails.Controls.Add(this.cmbCountry);
            this.gpbDetails.Controls.Add(this.label25);
            this.gpbDetails.Controls.Add(this.lblZip);
            this.gpbDetails.Controls.Add(this.txtZip);
            this.gpbDetails.Controls.Add(this.lblStateTerritory);
            this.gpbDetails.Controls.Add(this.label26);
            this.gpbDetails.Controls.Add(this.lblCountry);
            this.gpbDetails.Controls.Add(this.label24);
            this.gpbDetails.Controls.Add(this.txtShortName);
            this.gpbDetails.Controls.Add(this.lblShortName);
            this.gpbDetails.Controls.Add(this.label22);
            this.gpbDetails.Controls.Add(this.cmbCompanyTYpe);
            this.gpbDetails.Controls.Add(this.label19);
            this.gpbDetails.Controls.Add(this.txtTelephone);
            this.gpbDetails.Controls.Add(this.txtFax);
            this.gpbDetails.Controls.Add(this.txtAddress2);
            this.gpbDetails.Controls.Add(this.txtAddress1);
            this.gpbDetails.Controls.Add(this.txtCompanyName);
            this.gpbDetails.Controls.Add(this.label6);
            this.gpbDetails.Controls.Add(this.label5);
            this.gpbDetails.Controls.Add(this.label4);
            this.gpbDetails.Controls.Add(this.label3);
            this.gpbDetails.Controls.Add(this.label2);
            this.gpbDetails.Controls.Add(this.label1);
            this.gpbDetails.Controls.Add(this.label7);
            this.gpbDetails.Controls.Add(this.label9);
            this.gpbDetails.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.gpbDetails.Location = new System.Drawing.Point(2, 2);
            this.gpbDetails.Name = "gpbDetails";
            this.gpbDetails.Size = new System.Drawing.Size(292, 249);
            this.gpbDetails.TabIndex = 8;
            this.gpbDetails.TabStop = false;
            this.gpbDetails.Text = "Details";
            // 
            // cmbState
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbState.DisplayLayout.Appearance = appearance1;
            this.cmbState.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.cmbState.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbState.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbState.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbState.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbState.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbState.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbState.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbState.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbState.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbState.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbState.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbState.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbState.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbState.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbState.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbState.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbState.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbState.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbState.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbState.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbState.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbState.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbState.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbState.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbState.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbState.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbState.DropDownWidth = 0;
            this.cmbState.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbState.Location = new System.Drawing.Point(122, 128);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(150, 21);
            this.cmbState.TabIndex = 5;
            this.cmbState.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cmbCountry
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCountry.DisplayLayout.Appearance = appearance13;
            this.cmbCountry.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5});
            this.cmbCountry.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbCountry.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCountry.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCountry.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbCountry.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCountry.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbCountry.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCountry.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCountry.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCountry.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbCountry.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCountry.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCountry.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbCountry.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCountry.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCountry.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
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
            this.cmbCountry.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCountry.DropDownWidth = 0;
            this.cmbCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCountry.Location = new System.Drawing.Point(122, 106);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(150, 21);
            this.cmbCountry.TabIndex = 4;
            this.cmbCountry.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCountry.ValueChanged += new System.EventHandler(this.cmbCountry_ValueChanged);
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label25.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label25.ForeColor = System.Drawing.Color.Red;
            this.label25.Location = new System.Drawing.Point(94, 125);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(12, 8);
            this.label25.TabIndex = 80;
            this.label25.Text = "*";
            // 
            // lblZip
            // 
            this.lblZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblZip.Location = new System.Drawing.Point(8, 147);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(32, 22);
            this.lblZip.TabIndex = 79;
            this.lblZip.Text = "Zip";
            this.lblZip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtZip
            // 
            this.txtZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtZip.Location = new System.Drawing.Point(122, 150);
            this.txtZip.MaxLength = 50;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(150, 21);
            this.txtZip.TabIndex = 6;
            // 
            // lblStateTerritory
            // 
            this.lblStateTerritory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStateTerritory.Location = new System.Drawing.Point(8, 125);
            this.lblStateTerritory.Name = "lblStateTerritory";
            this.lblStateTerritory.Size = new System.Drawing.Size(88, 22);
            this.lblStateTerritory.TabIndex = 78;
            this.lblStateTerritory.Text = "State/Territory";
            this.lblStateTerritory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label26.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(62, 106);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(12, 12);
            this.label26.TabIndex = 77;
            this.label26.Text = "*";
            // 
            // lblCountry
            // 
            this.lblCountry.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCountry.Location = new System.Drawing.Point(8, 104);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(54, 22);
            this.lblCountry.TabIndex = 76;
            this.lblCountry.Text = "Country";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.ForeColor = System.Drawing.Color.Red;
            this.label24.Location = new System.Drawing.Point(78, 40);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(12, 8);
            this.label24.TabIndex = 42;
            this.label24.Text = "*";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(122, 40);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(150, 21);
            this.txtShortName.TabIndex = 1;
            // 
            // lblShortName
            // 
            this.lblShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblShortName.Location = new System.Drawing.Point(8, 40);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(68, 16);
            this.lblShortName.TabIndex = 40;
            this.lblShortName.Text = "Short Name";
            // 
            // label22
            // 
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(62, 192);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(12, 8);
            this.label22.TabIndex = 39;
            this.label22.Text = "*";
            // 
            // cmbCompanyTYpe
            // 
            this.cmbCompanyTYpe.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCompanyTYpe.DisplayLayout.Appearance = appearance25;
            this.cmbCompanyTYpe.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn6.Header.VisiblePosition = 0;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn6,
            ultraGridColumn7});
            this.cmbCompanyTYpe.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbCompanyTYpe.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCompanyTYpe.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyTYpe.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyTYpe.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbCompanyTYpe.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCompanyTYpe.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanyTYpe.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbCompanyTYpe.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCompanyTYpe.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCompanyTYpe.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCompanyTYpe.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbCompanyTYpe.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCompanyTYpe.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyTYpe.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCompanyTYpe.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbCompanyTYpe.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCompanyTYpe.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanyTYpe.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.cmbCompanyTYpe.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbCompanyTYpe.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCompanyTYpe.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbCompanyTYpe.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbCompanyTYpe.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCompanyTYpe.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbCompanyTYpe.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCompanyTYpe.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCompanyTYpe.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCompanyTYpe.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCompanyTYpe.DropDownWidth = 0;
            this.cmbCompanyTYpe.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCompanyTYpe.Location = new System.Drawing.Point(122, 172);
            this.cmbCompanyTYpe.MaxDropDownItems = 12;
            this.cmbCompanyTYpe.Name = "cmbCompanyTYpe";
            this.cmbCompanyTYpe.Size = new System.Drawing.Size(150, 21);
            this.cmbCompanyTYpe.TabIndex = 7;
            this.cmbCompanyTYpe.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCompanyTYpe.GotFocus += new System.EventHandler(this.cmbCompanyTYpe_GotFocus);
            this.cmbCompanyTYpe.LostFocus += new System.EventHandler(this.cmbCompanyTYpe_LostFocus);
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label19.Location = new System.Drawing.Point(8, 210);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(100, 16);
            this.label19.TabIndex = 38;
            this.label19.Text = "(1-111-111111)";
            // 
            // txtTelephone
            // 
            this.txtTelephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTelephone.Location = new System.Drawing.Point(122, 195);
            this.txtTelephone.MaxLength = 50;
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(150, 21);
            this.txtTelephone.TabIndex = 8;
            this.txtTelephone.GotFocus += new System.EventHandler(this.txtTelephone_GotFocus);
            this.txtTelephone.LostFocus += new System.EventHandler(this.txtTelephone_LostFocus);
            // 
            // txtFax
            // 
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFax.Location = new System.Drawing.Point(122, 224);
            this.txtFax.MaxLength = 50;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(150, 21);
            this.txtFax.TabIndex = 9;
            this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
            this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
            // 
            // txtAddress2
            // 
            this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress2.Location = new System.Drawing.Point(122, 84);
            this.txtAddress2.MaxLength = 50;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(150, 21);
            this.txtAddress2.TabIndex = 3;
            this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
            this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
            // 
            // txtAddress1
            // 
            this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAddress1.Location = new System.Drawing.Point(122, 62);
            this.txtAddress1.MaxLength = 50;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(150, 21);
            this.txtAddress1.TabIndex = 2;
            this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
            this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCompanyName.Location = new System.Drawing.Point(122, 18);
            this.txtCompanyName.MaxLength = 50;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(150, 21);
            this.txtCompanyName.TabIndex = 0;
            this.txtCompanyName.GotFocus += new System.EventHandler(this.txtCompanyName_GotFocus);
            this.txtCompanyName.LostFocus += new System.EventHandler(this.txtCompanyName_LostFocus);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(8, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Fax #";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(8, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Work #";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(8, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Client Type";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(8, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Address 2";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Address 1";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client Name";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(104, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 8);
            this.label7.TabIndex = 36;
            this.label7.Text = "*";
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(100, 174);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 8);
            this.label9.TabIndex = 37;
            this.label9.Text = "*";
            // 
            // gpbSecondaryContact
            // 
            this.gpbSecondaryContact.Controls.Add(this.label21);
            this.gpbSecondaryContact.Controls.Add(this.txtPC2Telephone);
            this.gpbSecondaryContact.Controls.Add(this.txtPC2Cell);
            this.gpbSecondaryContact.Controls.Add(this.txtPC2Title);
            this.gpbSecondaryContact.Controls.Add(this.txtPC2LastName);
            this.gpbSecondaryContact.Controls.Add(this.txtPC2FirstName);
            this.gpbSecondaryContact.Controls.Add(this.label30);
            this.gpbSecondaryContact.Controls.Add(this.label31);
            this.gpbSecondaryContact.Controls.Add(this.label32);
            this.gpbSecondaryContact.Controls.Add(this.label33);
            this.gpbSecondaryContact.Controls.Add(this.label34);
            this.gpbSecondaryContact.Controls.Add(this.label35);
            this.gpbSecondaryContact.Controls.Add(this.txtPC2Email);
            this.gpbSecondaryContact.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.gpbSecondaryContact.Location = new System.Drawing.Point(296, 2);
            this.gpbSecondaryContact.Name = "gpbSecondaryContact";
            this.gpbSecondaryContact.Size = new System.Drawing.Size(288, 162);
            this.gpbSecondaryContact.TabIndex = 9;
            this.gpbSecondaryContact.TabStop = false;
            this.gpbSecondaryContact.Text = "Secondary Contact";
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label21.Location = new System.Drawing.Point(8, 124);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(98, 16);
            this.label21.TabIndex = 39;
            this.label21.Text = "(1-111-111111)";
            // 
            // txtPC2Telephone
            // 
            this.txtPC2Telephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC2Telephone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC2Telephone.Location = new System.Drawing.Point(116, 104);
            this.txtPC2Telephone.MaxLength = 50;
            this.txtPC2Telephone.Name = "txtPC2Telephone";
            this.txtPC2Telephone.Size = new System.Drawing.Size(150, 21);
            this.txtPC2Telephone.TabIndex = 4;
            this.txtPC2Telephone.GotFocus += new System.EventHandler(this.txtPC2Telephone_GotFocus);
            this.txtPC2Telephone.LostFocus += new System.EventHandler(this.txtPC2Telephone_LostFocus);
            // 
            // txtPC2Cell
            // 
            this.txtPC2Cell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC2Cell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC2Cell.Location = new System.Drawing.Point(116, 138);
            this.txtPC2Cell.MaxLength = 50;
            this.txtPC2Cell.Name = "txtPC2Cell";
            this.txtPC2Cell.Size = new System.Drawing.Size(150, 21);
            this.txtPC2Cell.TabIndex = 5;
            this.txtPC2Cell.GotFocus += new System.EventHandler(this.txtPC2Cell_GotFocus);
            this.txtPC2Cell.LostFocus += new System.EventHandler(this.txtPC2Cell_LostFocus);
            // 
            // txtPC2Title
            // 
            this.txtPC2Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC2Title.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC2Title.Location = new System.Drawing.Point(116, 60);
            this.txtPC2Title.MaxLength = 50;
            this.txtPC2Title.Name = "txtPC2Title";
            this.txtPC2Title.Size = new System.Drawing.Size(150, 21);
            this.txtPC2Title.TabIndex = 2;
            this.txtPC2Title.GotFocus += new System.EventHandler(this.txtPC2Title_GotFocus);
            this.txtPC2Title.LostFocus += new System.EventHandler(this.txtPC2Title_LostFocus);
            // 
            // txtPC2LastName
            // 
            this.txtPC2LastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC2LastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC2LastName.Location = new System.Drawing.Point(116, 38);
            this.txtPC2LastName.MaxLength = 50;
            this.txtPC2LastName.Name = "txtPC2LastName";
            this.txtPC2LastName.Size = new System.Drawing.Size(150, 21);
            this.txtPC2LastName.TabIndex = 1;
            this.txtPC2LastName.GotFocus += new System.EventHandler(this.txtPC2LastName_GotFocus);
            this.txtPC2LastName.LostFocus += new System.EventHandler(this.txtPC2LastName_LostFocus);
            // 
            // txtPC2FirstName
            // 
            this.txtPC2FirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC2FirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC2FirstName.Location = new System.Drawing.Point(116, 16);
            this.txtPC2FirstName.MaxLength = 50;
            this.txtPC2FirstName.Name = "txtPC2FirstName";
            this.txtPC2FirstName.Size = new System.Drawing.Size(150, 21);
            this.txtPC2FirstName.TabIndex = 0;
            this.txtPC2FirstName.GotFocus += new System.EventHandler(this.txtPC2FirstName_GotFocus);
            this.txtPC2FirstName.LostFocus += new System.EventHandler(this.txtPC2FirstName_LostFocus);
            // 
            // label30
            // 
            this.label30.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label30.Location = new System.Drawing.Point(8, 84);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(36, 16);
            this.label30.TabIndex = 15;
            this.label30.Text = "EMail";
            // 
            // label31
            // 
            this.label31.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label31.Location = new System.Drawing.Point(8, 106);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(54, 16);
            this.label31.TabIndex = 16;
            this.label31.Text = "Work #";
            // 
            // label32
            // 
            this.label32.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label32.Location = new System.Drawing.Point(8, 142);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(42, 16);
            this.label32.TabIndex = 17;
            this.label32.Text = "Cell #";
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label33.Location = new System.Drawing.Point(8, 18);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(66, 16);
            this.label33.TabIndex = 12;
            this.label33.Text = "First Name";
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label34.Location = new System.Drawing.Point(8, 40);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(68, 16);
            this.label34.TabIndex = 13;
            this.label34.Text = "Last Name";
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label35.Location = new System.Drawing.Point(8, 62);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(30, 16);
            this.label35.TabIndex = 14;
            this.label35.Text = "Title";
            // 
            // txtPC2Email
            // 
            this.txtPC2Email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC2Email.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC2Email.Location = new System.Drawing.Point(116, 82);
            this.txtPC2Email.MaxLength = 50;
            this.txtPC2Email.Name = "txtPC2Email";
            this.txtPC2Email.Size = new System.Drawing.Size(150, 21);
            this.txtPC2Email.TabIndex = 3;
            this.txtPC2Email.GotFocus += new System.EventHandler(this.txtPC2Email_GotFocus);
            this.txtPC2Email.LostFocus += new System.EventHandler(this.txtPC2Email_LostFocus);
            // 
            // gpbPrimaryContact
            // 
            this.gpbPrimaryContact.Controls.Add(this.label27);
            this.gpbPrimaryContact.Controls.Add(this.label11);
            this.gpbPrimaryContact.Controls.Add(this.label23);
            this.gpbPrimaryContact.Controls.Add(this.label10);
            this.gpbPrimaryContact.Controls.Add(this.label20);
            this.gpbPrimaryContact.Controls.Add(this.label13);
            this.gpbPrimaryContact.Controls.Add(this.label14);
            this.gpbPrimaryContact.Controls.Add(this.label15);
            this.gpbPrimaryContact.Controls.Add(this.label16);
            this.gpbPrimaryContact.Controls.Add(this.label17);
            this.gpbPrimaryContact.Controls.Add(this.label18);
            this.gpbPrimaryContact.Controls.Add(this.txtPC1FirstName);
            this.gpbPrimaryContact.Controls.Add(this.txtPC1LastName);
            this.gpbPrimaryContact.Controls.Add(this.txtPC1Title);
            this.gpbPrimaryContact.Controls.Add(this.txtPC1Cell);
            this.gpbPrimaryContact.Controls.Add(this.txtPC1Telephone);
            this.gpbPrimaryContact.Controls.Add(this.txtPC1Email);
            this.gpbPrimaryContact.Controls.Add(this.label12);
            this.gpbPrimaryContact.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.gpbPrimaryContact.Location = new System.Drawing.Point(2, 253);
            this.gpbPrimaryContact.Name = "gpbPrimaryContact";
            this.gpbPrimaryContact.Size = new System.Drawing.Size(292, 178);
            this.gpbPrimaryContact.TabIndex = 10;
            this.gpbPrimaryContact.TabStop = false;
            this.gpbPrimaryContact.Text = "Primary Contact";
            this.gpbPrimaryContact.Enter += new System.EventHandler(this.gpbPrimaryContact_Enter);
            // 
            // label27
            // 
            this.label27.ForeColor = System.Drawing.Color.Red;
            this.label27.Location = new System.Drawing.Point(55, 136);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(12, 8);
            this.label27.TabIndex = 180;
            this.label27.Text = "*";
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(58, 108);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(12, 8);
            this.label11.TabIndex = 38;
            this.label11.Text = "*";
            // 
            // label23
            // 
            this.label23.ForeColor = System.Drawing.Color.Red;
            this.label23.Location = new System.Drawing.Point(44, 84);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(12, 8);
            this.label23.TabIndex = 179;
            this.label23.Text = "*";
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(77, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 8);
            this.label10.TabIndex = 37;
            this.label10.Text = "*";
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label20.Location = new System.Drawing.Point(8, 120);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(98, 16);
            this.label20.TabIndex = 39;
            this.label20.Text = "(1-111-111111)";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(8, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 16);
            this.label13.TabIndex = 12;
            this.label13.Text = "First Name";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label14.Location = new System.Drawing.Point(8, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 16);
            this.label14.TabIndex = 13;
            this.label14.Text = "Last Name";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label15.Location = new System.Drawing.Point(8, 62);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 16);
            this.label15.TabIndex = 14;
            this.label15.Text = "Title";
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label16.Location = new System.Drawing.Point(8, 84);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(38, 16);
            this.label16.TabIndex = 15;
            this.label16.Text = "E-Mail";
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label17.Location = new System.Drawing.Point(8, 108);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(50, 16);
            this.label17.TabIndex = 16;
            this.label17.Text = "Work #";
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label18.Location = new System.Drawing.Point(8, 136);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(48, 16);
            this.label18.TabIndex = 17;
            this.label18.Text = "Cell #";
            // 
            // txtPC1FirstName
            // 
            this.txtPC1FirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC1FirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC1FirstName.Location = new System.Drawing.Point(122, 16);
            this.txtPC1FirstName.MaxLength = 50;
            this.txtPC1FirstName.Name = "txtPC1FirstName";
            this.txtPC1FirstName.Size = new System.Drawing.Size(150, 21);
            this.txtPC1FirstName.TabIndex = 0;
            this.txtPC1FirstName.GotFocus += new System.EventHandler(this.txtPC1FirstName_GotFocus);
            this.txtPC1FirstName.LostFocus += new System.EventHandler(this.txtPC1FirstName_LostFocus);
            // 
            // txtPC1LastName
            // 
            this.txtPC1LastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC1LastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC1LastName.Location = new System.Drawing.Point(122, 38);
            this.txtPC1LastName.MaxLength = 50;
            this.txtPC1LastName.Name = "txtPC1LastName";
            this.txtPC1LastName.Size = new System.Drawing.Size(150, 21);
            this.txtPC1LastName.TabIndex = 1;
            this.txtPC1LastName.GotFocus += new System.EventHandler(this.txtPC1LastName_GotFocus);
            this.txtPC1LastName.LostFocus += new System.EventHandler(this.txtPC1LastName_LostFocus);
            // 
            // txtPC1Title
            // 
            this.txtPC1Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC1Title.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC1Title.Location = new System.Drawing.Point(122, 60);
            this.txtPC1Title.MaxLength = 50;
            this.txtPC1Title.Name = "txtPC1Title";
            this.txtPC1Title.Size = new System.Drawing.Size(150, 21);
            this.txtPC1Title.TabIndex = 2;
            this.txtPC1Title.GotFocus += new System.EventHandler(this.txtPC1Title_GotFocus);
            this.txtPC1Title.LostFocus += new System.EventHandler(this.txtPC1Title_LostFocus);
            // 
            // txtPC1Cell
            // 
            this.txtPC1Cell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC1Cell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC1Cell.Location = new System.Drawing.Point(122, 134);
            this.txtPC1Cell.MaxLength = 50;
            this.txtPC1Cell.Name = "txtPC1Cell";
            this.txtPC1Cell.Size = new System.Drawing.Size(150, 21);
            this.txtPC1Cell.TabIndex = 12;
            this.txtPC1Cell.GotFocus += new System.EventHandler(this.txtPC1Cell_GotFocus);
            this.txtPC1Cell.LostFocus += new System.EventHandler(this.txtPC1Cell_LostFocus);
            // 
            // txtPC1Telephone
            // 
            this.txtPC1Telephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC1Telephone.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC1Telephone.Location = new System.Drawing.Point(122, 108);
            this.txtPC1Telephone.MaxLength = 50;
            this.txtPC1Telephone.Name = "txtPC1Telephone";
            this.txtPC1Telephone.Size = new System.Drawing.Size(150, 21);
            this.txtPC1Telephone.TabIndex = 11;
            this.txtPC1Telephone.GotFocus += new System.EventHandler(this.txtPC1Telephone_GotFocus);
            this.txtPC1Telephone.LostFocus += new System.EventHandler(this.txtPC1Telephone_LostFocus);
            // 
            // txtPC1Email
            // 
            this.txtPC1Email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPC1Email.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPC1Email.Location = new System.Drawing.Point(122, 82);
            this.txtPC1Email.MaxLength = 50;
            this.txtPC1Email.Name = "txtPC1Email";
            this.txtPC1Email.Size = new System.Drawing.Size(150, 21);
            this.txtPC1Email.TabIndex = 10;
            this.txtPC1Email.GotFocus += new System.EventHandler(this.txtPC1Email_GotFocus);
            this.txtPC1Email.LostFocus += new System.EventHandler(this.txtPC1Email_LostFocus);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(8, 156);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(116, 18);
            this.label12.TabIndex = 178;
            this.label12.Text = "* Required Field";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(72, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 8);
            this.label8.TabIndex = 36;
            this.label8.Text = "*";
            // 
            // ClientCompany
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gpbPrimaryContact);
            this.Controls.Add(this.gpbSecondaryContact);
            this.Controls.Add(this.gpbDetails);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "ClientCompany";
            this.Size = new System.Drawing.Size(599, 435);
            this.Load += new System.EventHandler(this.ClientCompany_Load);
            this.gpbDetails.ResumeLayout(false);
            this.gpbDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCountry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanyTYpe)).EndInit();
            this.gpbSecondaryContact.ResumeLayout(false);
            this.gpbSecondaryContact.PerformLayout();
            this.gpbPrimaryContact.ResumeLayout(false);
            this.gpbPrimaryContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Public Properties

        public void SetupControl(int companyIDForClient, int clientID)
        {
            //_companyID = companyIDForClient;

            BindCompanyType();
            BindCountries();
            BindStates();
            Prana.Admin.BLL.CompanyClient companyClient = CompanyManager.GetCompanyClient(companyIDForClient, clientID);
            SetCompanyClient(companyClient);
        }

        /// <summary>
        /// This method binds the existing <see cref="Countries"/> in the ComboBox control by assigning the 
        /// countries object to its datasource property.
        /// </summary>
        private void BindCountries()
        {
            //GetCountries method fetches the existing countries from the database.
            Countries countries = GeneralManager.GetCountries();
            //Inserting the - Select - option in the Combo Box at the top.
            countries.Insert(0, new Country(int.MinValue, C_COMBO_SELECT));
            cmbCountry.DisplayMember = "Name";
            cmbCountry.ValueMember = "CountryID";
            cmbCountry.DataSource = null;
            cmbCountry.DataSource = countries;
            cmbCountry.Value = int.MinValue;
        }

        /// <summary>
        /// This method binds the existing <see cref="States"/> in the ComboBox control by assigning the 
        /// states object to its datasource property.
        /// </summary>
        private void BindStates()
        {
            //GetStates method fetches the existing states from the database.
            States states = GeneralManager.GetStates();
            //Inserting the - Select - option in the Combo Box at the top.
            states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "StateID";
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
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
            cmbState.DataSource = null;
            cmbState.DataSource = states;
            cmbState.Value = int.MinValue;
        }

        //CompanyClient property sets the CompanyClient form and get the CompanyClient details from it. 
        public CompanyClient CompanyClient
        {
            get { return GetCompanyClient(); }
            //set{SetCompanyClient(value);}			
        }

        /// <summary>
        /// Blanks all the textboxes in the form. 
        /// </summary>
        public void RefreshCompanyClientDetail()
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCompanyName.Text = "";
            txtFax.Text = "";
            txtPC1Cell.Text = "";
            txtPC1Email.Text = "";
            txtPC1FirstName.Text = "";
            txtPC1LastName.Text = "";
            txtPC1Telephone.Text = "";
            txtPC1Title.Text = "";
            txtPC2Cell.Text = "";
            txtPC2Email.Text = "";
            txtPC2FirstName.Text = "";
            txtPC2LastName.Text = "";
            txtPC2Telephone.Text = "";
            txtPC2Title.Text = "";
            txtTelephone.Text = "";
        }

        /// <summary>
        /// Shows all the details in the respective controls pertaining to that paricular client.
        /// </summary>
        /// <param name="companyClient"></param>
        private void SetCompanyClient(CompanyClient companyClient)
        {
            if (companyClient != null)
            {
                txtAddress1.Text = companyClient.Address1;
                txtAddress2.Text = companyClient.Address2;
                cmbCompanyTYpe.Value = companyClient.CompanyTypeID;
                if (int.Parse(companyClient.CountryID.ToString()) <= 0)
                {
                    cmbCountry.Value = int.MinValue;
                    BindEmptyStates();
                }
                else
                {
                    cmbCountry.Value = int.Parse(companyClient.CountryID.ToString());
                }
                cmbState.Value = int.Parse(companyClient.StateID.ToString());
                txtShortName.Text = companyClient.ShortName;
                txtZip.Text = companyClient.Zip;
                txtTelephone.Text = companyClient.Telephone;
                txtFax.Text = companyClient.Fax;
                txtCompanyName.Text = companyClient.Name;
                txtPC1Cell.Text = companyClient.PrimaryContactCell;
                txtPC1FirstName.Text = companyClient.PrimaryContactFirstName;
                txtPC1LastName.Text = companyClient.PrimaryContactLastName;
                txtPC1Email.Text = companyClient.PrimaryContactEMail;
                txtPC1Telephone.Text = companyClient.PrimaryContactTelephone;
                txtPC1Title.Text = companyClient.PrimaryContactTitle;
                txtPC2Cell.Text = companyClient.SecondaryContactCell;
                txtPC2FirstName.Text = companyClient.SecondaryContactFirstName;
                txtPC2LastName.Text = companyClient.SecondaryContactLastName;
                txtPC2Email.Text = companyClient.SecondaryContactEMail;
                txtPC2Telephone.Text = companyClient.SecondaryContactTelephone;
                txtPC2Title.Text = companyClient.SecondaryContactTitle;
            }
        }


        /// <summary>
        /// Get all the information pertaining to the CompanyClient from the controls while checking for 
        /// the validations for the required filed and other requirements.
        /// </summary>
        /// <returns>Returns CompanyClient object if saved successfully.</returns>
        private CompanyClient GetCompanyClient()
        {
            CompanyClient companyClient = new CompanyClient();
            //companyClient = null;
            bool check = false;
            string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex emailRegex = new Regex(emailCheck);
            Match emailMatch = emailRegex.Match(txtPC1Email.Text.ToString());

            errorProvider1.SetError(txtCompanyName, "");
            errorProvider1.SetError(txtAddress1, "");
            errorProvider1.SetError(cmbCompanyTYpe, "");
            errorProvider1.SetError(txtPC1FirstName, "");
            errorProvider1.SetError(txtPC1Email, "");
            errorProvider1.SetError(txtPC1Telephone, "");
            errorProvider1.SetError(txtTelephone, "");
            errorProvider1.SetError(txtPC1Cell, "");

            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(cmbCountry, "");
            errorProvider1.SetError(cmbState, "");
            if (txtCompanyName.Text == "")
            {
                errorProvider1.SetError(txtCompanyName, "Please enter Client name!");
                txtCompanyName.Focus();
                companyClient = null;
                return companyClient;
            }
            if (txtShortName.Text == "")
            {
                errorProvider1.SetError(txtShortName, "Please enter Short name!");
                txtShortName.Focus();
                companyClient = null;
                return companyClient;
            }
            else if (txtAddress1.Text == "")
            {
                errorProvider1.SetError(txtAddress1, "Please enter Address 1 in details!");
                companyClient = null;
                txtAddress1.Focus();
                return companyClient;
            }
            if (int.Parse(cmbCountry.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCountry, "Please select Country!");
                cmbCountry.Focus();
                companyClient = null;
                check = true;
                return companyClient;
            }
            if (int.Parse(cmbState.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbState, "Please select State!");
                cmbState.Focus();
                companyClient = null;
                check = true;
                return companyClient;
            }
            else if (cmbCompanyTYpe.Value == null)
            {
                errorProvider1.SetError(cmbCompanyTYpe, "Please select Client Type!");
                cmbCompanyTYpe.Focus();
                companyClient = null;
                check = true;
                return companyClient;
            }
            if (check == false)
            {
                if (int.Parse(cmbCompanyTYpe.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbCompanyTYpe, "Please select Client Type!");
                    cmbCompanyTYpe.Focus();
                    companyClient = null;
                    check = true;
                    return companyClient;
                }
            }
            if (txtTelephone.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTelephone, "Please enter Work Telephone!");
                txtTelephone.Focus();
                companyClient = null;
                check = true;
                return companyClient;
            }


            //			else if(int.Parse(cmbCompanyTYpe.Value.ToString()) == int.MinValue)
            //			{
            //				errorProvider1.SetError(cmbCompanyTYpe, "Please select Company Type!");
            //				cmbCompanyTYpe.Focus();
            //				companyClient = null;
            //			}

            else if (txtPC1FirstName.Text == "")
            {
                errorProvider1.SetError(txtPC1FirstName, "Please enter Primary Contact First name!");
                txtPC1FirstName.Focus();
                companyClient = null;
                return companyClient;
            }
            else if (!emailMatch.Success)
            {
                errorProvider1.SetError(txtPC1Email, "Please enter valid Primary contact Email !");
                txtPC1Email.Focus();
                companyClient = null;
                return companyClient;
            }
            //if (txtPC1Email.Text == "")
            //{
            //    errorProvider1.SetError(txtPC1Email, "Please enter Primary Contact EMail!");
            //    txtPC1Email.Focus();
            //    companyClient = null;
            //    return companyClient;
            //}
            else if (txtPC1Telephone.Text == "")
            {
                errorProvider1.SetError(txtPC1Telephone, "Please enter Primary contact Work Phone!");
                txtPC1Telephone.Focus();
                companyClient = null;
                return companyClient;
            }
            else if (txtPC1Cell.Text == "")
            {
                errorProvider1.SetError(txtPC1Cell, "Please enter Primary contact Cell Phone!");
                txtPC1Cell.Focus();
                companyClient = null;
                return companyClient;
            }
            else if (check != true)
            {
                companyClient.Address1 = txtAddress1.Text.ToString();
                companyClient.Address2 = txtAddress2.Text.ToString();
                companyClient.CompanyTypeID = int.Parse(cmbCompanyTYpe.Value.ToString());
                companyClient.Telephone = txtTelephone.Text.ToString();
                companyClient.ShortName = txtShortName.Text.ToString();
                companyClient.Zip = txtZip.Text.ToString();
                companyClient.CountryID = int.Parse(cmbCountry.Value.ToString());
                companyClient.StateID = int.Parse(cmbState.Value.ToString());

                companyClient.Fax = txtFax.Text.ToString();
                companyClient.Name = txtCompanyName.Text.ToString();
                companyClient.PrimaryContactCell = txtPC1Cell.Text.ToString();
                companyClient.PrimaryContactFirstName = txtPC1FirstName.Text.ToString();
                companyClient.PrimaryContactLastName = txtPC1LastName.Text.ToString();
                companyClient.PrimaryContactEMail = txtPC1Email.Text.ToString();
                companyClient.PrimaryContactTelephone = txtPC1Telephone.Text.ToString();
                companyClient.PrimaryContactTitle = txtPC1Title.Text.ToString();
                companyClient.SecondaryContactCell = txtPC2Cell.Text.ToString();
                companyClient.SecondaryContactFirstName = txtPC2FirstName.Text.ToString();
                companyClient.SecondaryContactLastName = txtPC2LastName.Text.ToString();
                companyClient.SecondaryContactEMail = txtPC2Email.Text.ToString();
                companyClient.SecondaryContactTelephone = txtPC2Telephone.Text.ToString();
                companyClient.SecondaryContactTitle = txtPC2Title.Text.ToString();

                //companyClient
            }
            return companyClient;
            //return null;
        }

        #endregion

        /// <summary>
        /// This method saves the CompanyClient detail in the database.
        /// </summary>
        /// <param name="companyClient"></param>
        /// <returns>Returns 1 if saved successfully.</returns>
        public int CompanyClientSave(CompanyClient companyClient)
        {
            int result = int.MinValue;
            if (txtCompanyName.Text == "")
            {
                txtCompanyName.Focus();
                return result;
            }
            else if (txtAddress1.Text == "")
            {
                txtAddress1.Focus();
                return result;
            }
            else if (int.Parse(cmbCompanyTYpe.Value.ToString()) == int.MinValue)
            {
                cmbCompanyTYpe.Focus();
                return result;
            }
            else if (int.Parse(txtTelephone.Text.Trim().ToString()) == int.MinValue)
            {
                txtTelephone.Focus();
                return result;
            }
            else if (txtPC1LastName.Text == "")
            {
                txtAddress1.Focus();
                return result;
            }
            else if (txtPC1Email.Text == "")
            {
                txtPC1Email.Focus();
                return result;
            }
            else
            {
                companyClient.Address1 = txtAddress1.Text.Trim();
                companyClient.Address2 = txtAddress2.Text.Trim();
                companyClient.CompanyTypeID = int.Parse(cmbCompanyTYpe.Value.ToString());
                companyClient.Telephone = txtTelephone.Text.Trim();

                companyClient.Fax = txtFax.Text.Trim();
                companyClient.Name = txtCompanyName.Text.Trim();
                companyClient.PrimaryContactCell = txtPC1Cell.Text.Trim();
                companyClient.PrimaryContactFirstName = txtPC1FirstName.Text.Trim();
                companyClient.PrimaryContactLastName = txtPC1LastName.Text.Trim();
                companyClient.PrimaryContactEMail = txtPC1Email.Text.Trim();
                companyClient.PrimaryContactTelephone = txtPC1Telephone.Text.Trim();
                companyClient.PrimaryContactTitle = txtPC1Title.Text.Trim();
                companyClient.SecondaryContactCell = txtPC2Cell.Text.Trim();
                companyClient.SecondaryContactFirstName = txtPC2FirstName.Text.Trim();
                companyClient.SecondaryContactLastName = txtPC2LastName.Text.Trim();
                companyClient.SecondaryContactEMail = txtPC2Email.Text.Trim();
                companyClient.SecondaryContactTelephone = txtPC2Telephone.Text.Trim();
                companyClient.SecondaryContactTitle = txtPC2Title.Text.Trim();

                result = 1;
            }
            return result;
        }

        /// <summary>
        /// Binds the CompanyType type on the form load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientCompany_Load(object sender, System.EventArgs e)
        {
        }

        #region Private Properties

        /// <summary>
        /// This method binds the existing <see cref="CompanyTypes"/> in the ComboBox control by assigning the 
        /// companyTypes object to its datasource property.
        /// </summary>
        private void BindCompanyType()
        {
            CompanyTypes companyTypes = CompanyManager.GetCompanyTypes();
            companyTypes.Insert(0, new CompanyType(int.MinValue, C_COMBO_SELECT));
            cmbCompanyTYpe.DataSource = null;
            cmbCompanyTYpe.DataSource = companyTypes;
            cmbCompanyTYpe.DisplayMember = "Type";
            cmbCompanyTYpe.ValueMember = "CompanyTypeID";
            cmbCompanyTYpe.Text = C_COMBO_SELECT;
        }

        #endregion

        #region Controls Focus Colors
        private void txtTelephone_GotFocus(object sender, System.EventArgs e)
        {
            txtTelephone.BackColor = Color.LemonChiffon;
        }
        private void txtTelephone_LostFocus(object sender, System.EventArgs e)
        {
            txtTelephone.BackColor = Color.White;
        }

        private void txtAddress1_GotFocus(object sender, System.EventArgs e)
        {
            txtAddress1.BackColor = Color.LemonChiffon;
        }
        private void txtAddress1_LostFocus(object sender, System.EventArgs e)
        {
            txtAddress1.BackColor = Color.White;
        }
        private void txtAddress2_GotFocus(object sender, System.EventArgs e)
        {
            txtAddress2.BackColor = Color.LemonChiffon;
        }
        private void txtAddress2_LostFocus(object sender, System.EventArgs e)
        {
            txtAddress2.BackColor = Color.White;
        }
        private void txtFax_GotFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.LemonChiffon;
        }
        private void txtFax_LostFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.White;
        }
        private void txtCompanyName_GotFocus(object sender, System.EventArgs e)
        {
            txtCompanyName.BackColor = Color.LemonChiffon;
        }
        private void txtCompanyName_LostFocus(object sender, System.EventArgs e)
        {
            txtCompanyName.BackColor = Color.White;
        }
        private void txtPC1Cell_GotFocus(object sender, System.EventArgs e)
        {
            txtPC1Cell.BackColor = Color.LemonChiffon;
        }
        private void txtPC1Cell_LostFocus(object sender, System.EventArgs e)
        {
            txtPC1Cell.BackColor = Color.White;
        }
        private void txtPC1Email_GotFocus(object sender, System.EventArgs e)
        {
            txtPC1Email.BackColor = Color.LemonChiffon;
        }
        private void txtPC1Email_LostFocus(object sender, System.EventArgs e)
        {
            txtPC1Email.BackColor = Color.White;
        }
        private void txtPC1FirstName_GotFocus(object sender, System.EventArgs e)
        {
            txtPC1FirstName.BackColor = Color.LemonChiffon;
        }
        private void txtPC1FirstName_LostFocus(object sender, System.EventArgs e)
        {
            txtPC1FirstName.BackColor = Color.White;
        }
        private void txtPC1LastName_GotFocus(object sender, System.EventArgs e)
        {
            txtPC1LastName.BackColor = Color.LemonChiffon;
        }
        private void txtPC1LastName_LostFocus(object sender, System.EventArgs e)
        {
            txtPC1LastName.BackColor = Color.White;
        }
        private void txtPC1Telephone_GotFocus(object sender, System.EventArgs e)
        {
            txtPC1Telephone.BackColor = Color.LemonChiffon;
        }
        private void txtPC1Telephone_LostFocus(object sender, System.EventArgs e)
        {
            txtPC1Telephone.BackColor = Color.White;
        }
        private void txtPC1Title_GotFocus(object sender, System.EventArgs e)
        {
            txtPC1Title.BackColor = Color.LemonChiffon;
        }
        private void txtPC1Title_LostFocus(object sender, System.EventArgs e)
        {
            txtPC1Title.BackColor = Color.White;
        }
        private void txtPC2Cell_GotFocus(object sender, System.EventArgs e)
        {
            txtPC2Cell.BackColor = Color.LemonChiffon;
        }
        private void txtPC2Cell_LostFocus(object sender, System.EventArgs e)
        {
            txtPC2Cell.BackColor = Color.White;
        }
        private void txtPC2Email_GotFocus(object sender, System.EventArgs e)
        {
            txtPC2Email.BackColor = Color.LemonChiffon;
        }
        private void txtPC2Email_LostFocus(object sender, System.EventArgs e)
        {
            txtPC2Email.BackColor = Color.White;
        }
        private void txtPC2FirstName_GotFocus(object sender, System.EventArgs e)
        {
            txtPC2FirstName.BackColor = Color.LemonChiffon;
        }
        private void txtPC2FirstName_LostFocus(object sender, System.EventArgs e)
        {
            txtPC2FirstName.BackColor = Color.White;
        }
        private void txtPC2LastName_GotFocus(object sender, System.EventArgs e)
        {
            txtPC2LastName.BackColor = Color.LemonChiffon;
        }
        private void txtPC2LastName_LostFocus(object sender, System.EventArgs e)
        {
            txtPC2LastName.BackColor = Color.White;
        }
        private void txtPC2Telephone_GotFocus(object sender, System.EventArgs e)
        {
            txtPC2Telephone.BackColor = Color.LemonChiffon;
        }
        private void txtPC2Telephone_LostFocus(object sender, System.EventArgs e)
        {
            txtPC2Telephone.BackColor = Color.White;
        }
        private void txtPC2Title_GotFocus(object sender, System.EventArgs e)
        {
            txtPC2Title.BackColor = Color.LemonChiffon;
        }
        private void txtPC2Title_LostFocus(object sender, System.EventArgs e)
        {
            txtPC2Title.BackColor = Color.White;
        }
        private void cmbCompanyTYpe_GotFocus(object sender, System.EventArgs e)
        {
            cmbCompanyTYpe.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbCompanyTYpe_LostFocus(object sender, System.EventArgs e)
        {
            cmbCompanyTYpe.Appearance.BackColor = Color.White;
        }

        #endregion

        private void gpbPrimaryContact_Enter(object sender, System.EventArgs e)
        {

        }

        private void cmbCountry_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCountry.Value != null)
            {
                int countryID = int.Parse(cmbCountry.Value.ToString());
                if (countryID > 0)
                {
                    //GetStates method fetches the existing states from the database.
                    States states = GeneralManager.GetStates(countryID);
                    if (states.Count > 0)
                    {
                        states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
                        cmbState.DisplayMember = "StateName";
                        cmbState.ValueMember = "StateID";
                        cmbState.DataSource = null;
                        cmbState.DataSource = states;
                        cmbState.Text = C_COMBO_SELECT;
                    }
                }
                else
                {
                    BindEmptyStates();
                }

            }
        }
    }
}
