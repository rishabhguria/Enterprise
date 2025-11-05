using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global.Utilities;
using Prana.Utilities.StringUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{

    /// <summary>
    /// Summary description for ClientFix.
    /// </summary>
    public class GridClientFix : System.Windows.Forms.UserControl
    {
        #region Windows Controls
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSenderCompID;
        private System.Windows.Forms.Label lblOnBehalfOfCompID;
        private System.Windows.Forms.Label lblTargetCompID;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtSenderCompID;
        private System.Windows.Forms.TextBox txtOnBehalfOfCompID;
        private System.Windows.Forms.TextBox txtTargetCompID;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.GroupBox Fix;
        private System.Windows.Forms.TextBox txtIP1;
        private System.Windows.Forms.TextBox txtIP4;
        private System.Windows.Forms.TextBox txtIP3;
        private System.Windows.Forms.TextBox txtIP2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        #endregion

        #region Private Members
        private ClientFix _clientFix;
        private Identifiers _clientIdentifiers;
        //private Identifiers _identifiers;
        private int _companyClientID;
        const string C_COMBO_SELECT = "- Select -";
        private Identifier nullIdentifier;
        private bool bNullExist = false;



        #endregion
        private Infragistics.Win.UltraWinGrid.UltraGrid grdIdentifier;
        private System.Windows.Forms.Button btnAddNewAccount;
        private IContainer components;

        public GridClientFix()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            //BindIdentifier();

        }
        public void SetUp(int companyClientID)
        {
            _companyClientID = companyClientID;
            SetValues();
            nullIdentifier = new Identifier(0, "");
            _clientIdentifiers = ClientFixManager.GetClientIdentifiers(_companyClientID);
            IdentifierGridBind();
            grdIdentifier.DisplayLayout.Bands[0].Columns["Modify"].CellAppearance.Cursor = Cursors.Hand;
            grdIdentifier.DisplayLayout.Bands[0].Columns["Modify"].CellAppearance.ForeColor = Color.FromArgb(0, 0, 255);
            grdIdentifier.DisplayLayout.Bands[0].Columns["Modify"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
            grdIdentifier.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.Cursor = Cursors.Hand;
            grdIdentifier.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.ForeColor = Color.Red;
            grdIdentifier.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
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
                if (groupBox2 != null)
                {
                    groupBox2.Dispose();
                }
                if (lblSenderCompID != null)
                {
                    lblSenderCompID.Dispose();
                }
                if (lblOnBehalfOfCompID != null)
                {
                    lblOnBehalfOfCompID.Dispose();
                }
                if (lblTargetCompID != null)
                {
                    lblTargetCompID.Dispose();
                }
                if (lblIP != null)
                {
                    lblIP.Dispose();
                }
                if (lblPort != null)
                {
                    lblPort.Dispose();
                }
                if (txtSenderCompID != null)
                {
                    txtSenderCompID.Dispose();
                }
                if (txtOnBehalfOfCompID != null)
                {
                    txtOnBehalfOfCompID.Dispose();
                }
                if (txtTargetCompID != null)
                {
                    txtTargetCompID.Dispose();
                }
                if (txtPort != null)
                {
                    txtPort.Dispose();
                }
                if (Fix != null)
                {
                    Fix.Dispose();
                }
                if (txtIP1 != null)
                {
                    txtIP1.Dispose();
                }
                if (txtIP4 != null)
                {
                    txtIP4.Dispose();
                }
                if (txtIP3 != null)
                {
                    txtIP3.Dispose();
                }
                if (txtIP2 != null)
                {
                    txtIP2.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (grdIdentifier != null)
                {
                    grdIdentifier.Dispose();
                }
                if (btnAddNewAccount != null)
                {
                    btnAddNewAccount.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridClientFix));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierName", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientIdentifierName", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryKey", 3);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 4);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Modify", 5);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.lblSenderCompID = new System.Windows.Forms.Label();
            this.lblOnBehalfOfCompID = new System.Windows.Forms.Label();
            this.lblTargetCompID = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtSenderCompID = new System.Windows.Forms.TextBox();
            this.txtOnBehalfOfCompID = new System.Windows.Forms.TextBox();
            this.txtTargetCompID = new System.Windows.Forms.TextBox();
            this.txtIP1 = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.Fix = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP2 = new System.Windows.Forms.TextBox();
            this.txtIP3 = new System.Windows.Forms.TextBox();
            this.txtIP4 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddNewAccount = new System.Windows.Forms.Button();
            this.grdIdentifier = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.Fix.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSenderCompID
            // 
            this.lblSenderCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSenderCompID.Location = new System.Drawing.Point(12, 21);
            this.lblSenderCompID.Name = "lblSenderCompID";
            this.lblSenderCompID.Size = new System.Drawing.Size(96, 14);
            this.lblSenderCompID.TabIndex = 0;
            this.lblSenderCompID.Text = "Sender Client ID";
            // 
            // lblOnBehalfOfCompID
            // 
            this.lblOnBehalfOfCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOnBehalfOfCompID.Location = new System.Drawing.Point(12, 43);
            this.lblOnBehalfOfCompID.Name = "lblOnBehalfOfCompID";
            this.lblOnBehalfOfCompID.Size = new System.Drawing.Size(120, 12);
            this.lblOnBehalfOfCompID.TabIndex = 1;
            this.lblOnBehalfOfCompID.Text = "On Behalf Of Client ID";
            // 
            // lblTargetCompID
            // 
            this.lblTargetCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTargetCompID.Location = new System.Drawing.Point(12, 65);
            this.lblTargetCompID.Name = "lblTargetCompID";
            this.lblTargetCompID.Size = new System.Drawing.Size(80, 14);
            this.lblTargetCompID.TabIndex = 2;
            this.lblTargetCompID.Text = "TargetCompID";
            // 
            // lblIP
            // 
            this.lblIP.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIP.Location = new System.Drawing.Point(12, 87);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(20, 14);
            this.lblIP.TabIndex = 3;
            this.lblIP.Text = "IP";
            // 
            // lblPort
            // 
            this.lblPort.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPort.Location = new System.Drawing.Point(12, 112);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 12);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port";
            // 
            // txtSenderCompID
            // 
            this.txtSenderCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSenderCompID.Location = new System.Drawing.Point(179, 21);
            this.txtSenderCompID.MaxLength = 8;
            this.txtSenderCompID.Name = "txtSenderCompID";
            this.txtSenderCompID.Size = new System.Drawing.Size(100, 21);
            this.txtSenderCompID.TabIndex = 0;
            this.txtSenderCompID.GotFocus += new System.EventHandler(this.txtSenderCompID_GotFocus);
            this.txtSenderCompID.LostFocus += new System.EventHandler(this.txtSenderCompID_LostFocus);
            // 
            // txtOnBehalfOfCompID
            // 
            this.txtOnBehalfOfCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtOnBehalfOfCompID.Location = new System.Drawing.Point(179, 43);
            this.txtOnBehalfOfCompID.MaxLength = 8;
            this.txtOnBehalfOfCompID.Name = "txtOnBehalfOfCompID";
            this.txtOnBehalfOfCompID.Size = new System.Drawing.Size(100, 21);
            this.txtOnBehalfOfCompID.TabIndex = 1;
            this.txtOnBehalfOfCompID.GotFocus += new System.EventHandler(this.txtOnBehalfOfCompID_GotFocus);
            this.txtOnBehalfOfCompID.LostFocus += new System.EventHandler(this.txtOnBehalfOfCompID_LostFocus);
            // 
            // txtTargetCompID
            // 
            this.txtTargetCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTargetCompID.Location = new System.Drawing.Point(179, 65);
            this.txtTargetCompID.MaxLength = 8;
            this.txtTargetCompID.Name = "txtTargetCompID";
            this.txtTargetCompID.Size = new System.Drawing.Size(100, 21);
            this.txtTargetCompID.TabIndex = 2;
            this.txtTargetCompID.GotFocus += new System.EventHandler(this.txtTargetCompID_GotFocus);
            this.txtTargetCompID.LostFocus += new System.EventHandler(this.txtTargetCompID_LostFocus);
            // 
            // txtIP1
            // 
            this.txtIP1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIP1.Location = new System.Drawing.Point(179, 87);
            this.txtIP1.MaxLength = 3;
            this.txtIP1.Name = "txtIP1";
            this.txtIP1.Size = new System.Drawing.Size(40, 21);
            this.txtIP1.TabIndex = 3;
            this.txtIP1.GotFocus += new System.EventHandler(this.txtIP1_GotFocus);
            this.txtIP1.LostFocus += new System.EventHandler(this.txtIP1_LostFocus);
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPort.Location = new System.Drawing.Point(179, 111);
            this.txtPort.MaxLength = 8;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 21);
            this.txtPort.TabIndex = 7;
            this.txtPort.GotFocus += new System.EventHandler(this.txtPort_GotFocus);
            this.txtPort.LostFocus += new System.EventHandler(this.txtPort_LostFocus);
            // 
            // Fix
            // 
            this.Fix.Controls.Add(this.label6);
            this.Fix.Controls.Add(this.label4);
            this.Fix.Controls.Add(this.label2);
            this.Fix.Controls.Add(this.label1);
            this.Fix.Controls.Add(this.txtIP2);
            this.Fix.Controls.Add(this.txtIP3);
            this.Fix.Controls.Add(this.txtIP4);
            this.Fix.Controls.Add(this.lblSenderCompID);
            this.Fix.Controls.Add(this.lblOnBehalfOfCompID);
            this.Fix.Controls.Add(this.lblTargetCompID);
            this.Fix.Controls.Add(this.lblIP);
            this.Fix.Controls.Add(this.lblPort);
            this.Fix.Controls.Add(this.txtSenderCompID);
            this.Fix.Controls.Add(this.txtOnBehalfOfCompID);
            this.Fix.Controls.Add(this.txtPort);
            this.Fix.Controls.Add(this.txtTargetCompID);
            this.Fix.Controls.Add(this.txtIP1);
            this.Fix.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Fix.Location = new System.Drawing.Point(6, 8);
            this.Fix.Name = "Fix";
            this.Fix.Size = new System.Drawing.Size(436, 138);
            this.Fix.TabIndex = 0;
            this.Fix.TabStop = false;
            this.Fix.Text = "Fix";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(96, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 8);
            this.label6.TabIndex = 39;
            this.label6.Text = "*";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(120, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 37;
            this.label4.Text = "*";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(28, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 8);
            this.label2.TabIndex = 36;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(40, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 8);
            this.label1.TabIndex = 35;
            this.label1.Text = "*";
            // 
            // txtIP2
            // 
            this.txtIP2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIP2.Location = new System.Drawing.Point(221, 87);
            this.txtIP2.MaxLength = 3;
            this.txtIP2.Name = "txtIP2";
            this.txtIP2.Size = new System.Drawing.Size(40, 21);
            this.txtIP2.TabIndex = 4;
            this.txtIP2.GotFocus += new System.EventHandler(this.txtIP2_GotFocus);
            this.txtIP2.LostFocus += new System.EventHandler(this.txtIP2_LostFocus);
            // 
            // txtIP3
            // 
            this.txtIP3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIP3.Location = new System.Drawing.Point(263, 87);
            this.txtIP3.MaxLength = 3;
            this.txtIP3.Name = "txtIP3";
            this.txtIP3.Size = new System.Drawing.Size(40, 21);
            this.txtIP3.TabIndex = 5;
            this.txtIP3.GotFocus += new System.EventHandler(this.txtIP3_GotFocus);
            this.txtIP3.LostFocus += new System.EventHandler(this.txtIP3_LostFocus);
            // 
            // txtIP4
            // 
            this.txtIP4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIP4.Location = new System.Drawing.Point(305, 87);
            this.txtIP4.MaxLength = 3;
            this.txtIP4.Name = "txtIP4";
            this.txtIP4.Size = new System.Drawing.Size(40, 21);
            this.txtIP4.TabIndex = 6;
            this.txtIP4.GotFocus += new System.EventHandler(this.txtIP4_GotFocus);
            this.txtIP4.LostFocus += new System.EventHandler(this.txtIP4_LostFocus);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddNewAccount);
            this.groupBox2.Controls.Add(this.grdIdentifier);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox2.Location = new System.Drawing.Point(6, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 170);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Identifier";
            // 
            // btnAddNewAccount
            // 
            this.btnAddNewAccount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddNewAccount.BackColor = System.Drawing.Color.LightCyan;
            this.btnAddNewAccount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddNewAccount.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNewAccount.Image")));
            this.btnAddNewAccount.Location = new System.Drawing.Point(179, 142);
            this.btnAddNewAccount.Name = "btnAddNewAccount";
            this.btnAddNewAccount.Size = new System.Drawing.Size(78, 26);
            this.btnAddNewAccount.TabIndex = 0;
            this.btnAddNewAccount.UseVisualStyleBackColor = false;
            this.btnAddNewAccount.Click += new System.EventHandler(this.btnAddNewAccount_Click);
            // 
            // grdIdentifier
            // 
            this.grdIdentifier.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 104;
            appearance1.FontData.BoldAsString = "False";
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.Caption = "Identifier";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 117;
            appearance3.FontData.BoldAsString = "False";
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.Caption = "IdentifierName";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 109;
            appearance5.FontData.BoldAsString = "False";
            ultraGridColumn4.CellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            ultraGridColumn4.Header.Appearance = appearance6;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 48;
            appearance7.FontData.BoldAsString = "True";
            ultraGridColumn5.Header.Appearance = appearance7;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.NullText = "Delete";
            ultraGridColumn5.Width = 74;
            appearance8.FontData.BoldAsString = "True";
            ultraGridColumn6.Header.Appearance = appearance8;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.NullText = "Modify";
            ultraGridColumn6.Width = 101;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdIdentifier.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdIdentifier.DisplayLayout.GroupByBox.Hidden = true;
            this.grdIdentifier.DisplayLayout.MaxColScrollRegions = 1;
            this.grdIdentifier.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdIdentifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdIdentifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdIdentifier.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdIdentifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdIdentifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdIdentifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdIdentifier.Location = new System.Drawing.Point(6, 22);
            this.grdIdentifier.Name = "grdIdentifier";
            this.grdIdentifier.Size = new System.Drawing.Size(422, 118);
            this.grdIdentifier.TabIndex = 1;
            this.grdIdentifier.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdIdentifier.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdIdentifier_MouseUp);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GridClientFix
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Fix);
            this.Name = "GridClientFix";
            this.Size = new System.Drawing.Size(448, 326);
            this.Fix.ResumeLayout(false);
            this.Fix.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        #region Focus Colors
        private void txtSenderCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtSenderCompID.BackColor = Color.LemonChiffon;
        }
        private void txtSenderCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtSenderCompID.BackColor = Color.White;
        }
        private void txtOnBehalfOfCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtOnBehalfOfCompID.BackColor = Color.LemonChiffon;
        }
        private void txtOnBehalfOfCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtOnBehalfOfCompID.BackColor = Color.White;
        }
        private void txtTargetCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtTargetCompID.BackColor = Color.LemonChiffon;
        }
        private void txtTargetCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtTargetCompID.BackColor = Color.White;
        }
        private void txtPort_GotFocus(object sender, System.EventArgs e)
        {
            txtPort.BackColor = Color.LemonChiffon;
        }
        private void txtPort_LostFocus(object sender, System.EventArgs e)
        {
            txtPort.BackColor = Color.White;
        }

        private void txtIP1_GotFocus(object sender, System.EventArgs e)
        {
            txtIP1.BackColor = Color.LemonChiffon;
        }
        private void txtIP1_LostFocus(object sender, System.EventArgs e)
        {
            txtIP1.BackColor = Color.White;
        }
        private void txtIP2_GotFocus(object sender, System.EventArgs e)
        {
            txtIP2.BackColor = Color.LemonChiffon;
        }
        private void txtIP2_LostFocus(object sender, System.EventArgs e)
        {
            txtIP2.BackColor = Color.White;
        }
        private void txtIP3_GotFocus(object sender, System.EventArgs e)
        {
            txtIP3.BackColor = Color.LemonChiffon;
        }
        private void txtIP3_LostFocus(object sender, System.EventArgs e)
        {
            txtIP3.BackColor = Color.White;
        }

        private void txtIP4_GotFocus(object sender, System.EventArgs e)
        {
            txtIP4.BackColor = Color.LemonChiffon;
        }
        private void txtIP4_LostFocus(object sender, System.EventArgs e)
        {
            txtIP4.BackColor = Color.White;
        }


        #endregion



        #region  Private Methods
        private void SetValues()
        {
            _clientFix = ClientFixManager.GetClientFix(_companyClientID);

            //			if(_clientFix.SenderCompID!=int.MinValue) 
            txtSenderCompID.Text = _clientFix.SenderCompID.ToString();
            //			else
            //			txtSenderCompID.Text="";
            //			if(_clientFix.OnBehalfOfCompID!=int.MinValue )
            txtOnBehalfOfCompID.Text = _clientFix.OnBehalfOfCompID.ToString();
            //			else 
            //			txtOnBehalfOfCompID.Text="";
            //			if(_clientFix.TargetCompID!=int.MinValue)
            txtTargetCompID.Text = _clientFix.TargetCompID.ToString();
            //			else 
            //			txtTargetCompID.Text="";
            if (_clientFix.IP != String.Empty)

            {
                string[] str = (_clientFix.IP).Split('.');
                txtIP1.Text = str[0];
                txtIP2.Text = str[1];
                txtIP3.Text = str[2];
                txtIP4.Text = str[3];

            }
            else
            {
                txtIP1.Text = "";
                txtIP2.Text = "";
                txtIP3.Text = "";
                txtIP4.Text = "";

            }
            if (_clientFix.Port != int.MinValue)
                txtPort.Text = _clientFix.Port.ToString();
            else
                txtPort.Text = "";


        }

        private ClientFix GetValues()

        {
            _clientFix = new ClientFix();
            errorProvider1.SetError(txtSenderCompID, "");
            errorProvider1.SetError(txtOnBehalfOfCompID, "");
            errorProvider1.SetError(txtTargetCompID, "");
            errorProvider1.SetError(txtIP4, "");
            errorProvider1.SetError(txtPort, "");


            if (txtSenderCompID.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtSenderCompID, "Please Enter a value!");
                txtSenderCompID.Focus();

            }

            else if (txtTargetCompID.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(txtTargetCompID, "Please Enter a numeric value!");
                txtTargetCompID.Focus();


            }
            else if (!((RegularExpressionValidation.IsInteger(txtIP1.Text.Trim())) && (RegularExpressionValidation.IsInteger(txtIP2.Text.Trim())) && (RegularExpressionValidation.IsInteger(txtIP3.Text.Trim())) && (RegularExpressionValidation.IsInteger(txtIP4.Text.Trim()))))

            {
                errorProvider1.SetError(txtIP4, "Please Enter a Value in 172.34.23.123 format !");
                txtIP4.Focus();


            }
            else if (!RegularExpressionValidation.IsInteger(txtPort.Text.Trim()))
            {
                errorProvider1.SetError(txtPort, "Please Enter a numeric value!");
                txtTargetCompID.Focus();


            }


            else
            {
                _clientFix.SenderCompID = txtSenderCompID.Text.Trim();
                _clientFix.OnBehalfOfCompID = txtOnBehalfOfCompID.Text.Trim();
                _clientFix.TargetCompID = txtTargetCompID.Text.Trim();
                _clientFix.IP = txtIP1.Text.Trim() + "." + txtIP2.Text.Trim() + "." + txtIP3.Text.Trim() + "." + txtIP4.Text.Trim();
                _clientFix.Port = int.Parse(txtPort.Text.Trim());

                return _clientFix;
            }

            return null;


        }





        #endregion





        private void btnAddNewAccount_Click(object sender, System.EventArgs e)
        {
            NewIdentifier newIdentifier = new NewIdentifier(_companyClientID);
            newIdentifier.ClientIdentifiers = _clientIdentifiers;
            newIdentifier.ShowDialog(this);
            if (newIdentifier.AddedIdentifier != null)
            {

                _clientIdentifiers.Add(newIdentifier.AddedIdentifier);
            }
            IdentifierGridBind();

        }


        private void IdentifierGridBind()
        {

            if (_clientIdentifiers.Count > 0
                && _clientIdentifiers.Contains(nullIdentifier))
                _clientIdentifiers.Remove(nullIdentifier);
            if (_clientIdentifiers.Count == 0)
            {
                _clientIdentifiers.Add(nullIdentifier);
                bNullExist = true;
            }
            else
                bNullExist = false;
            grdIdentifier.DataSource = _clientIdentifiers;
            grdIdentifier.DataBind();
            if (bNullExist)
            {
                grdIdentifier.DisplayLayout.Bands[0].Columns["Modify"].NullText = "";
                grdIdentifier.DisplayLayout.Bands[0].Columns["Delete"].NullText = "";
            }
            else
            {
                grdIdentifier.DisplayLayout.Bands[0].Columns["Modify"].NullText = "Modify";
                grdIdentifier.DisplayLayout.Bands[0].Columns["Delete"].NullText = "Delete";
            }


        }



        private void grdIdentifier_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (bNullExist)
                return;
            UIElement objUIElement;
            UltraGridCell objUltraGridCell;
            objUIElement = grdIdentifier.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            if (objUIElement == null)
                return;
            objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            if (objUltraGridCell == null)
                return;

            if (objUltraGridCell.Text == "Modify")
            {
                string primaryKey = objUltraGridCell.Row.Cells["PrimaryKey"].Text;
                Identifier identifier = _clientIdentifiers.GetIdentifier(primaryKey);

                string IdentifierID = grdIdentifier.ActiveRow.Cells["IdentifierID"].Value.ToString();
                string formName = "Edit Identifier";
                string ClientIdentifierName = grdIdentifier.ActiveRow.Cells["ClientIdentifierName"].Value.ToString();
                NewIdentifier newIdentifier = new NewIdentifier(_companyClientID, IdentifierID, ClientIdentifierName, formName);
                newIdentifier.ClientIdentifiers = _clientIdentifiers;
                newIdentifier.ShowDialog(this);
                if (newIdentifier.AddedIdentifier != null)
                {
                    _clientIdentifiers.Remove(identifier);
                    _clientIdentifiers.Add(newIdentifier.AddedIdentifier);
                }
                IdentifierGridBind();



            }

            if (objUltraGridCell.Text == "Delete")
            {
                if (MessageBox.Show(this, "Do you want to delete this Identifier?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.No)

                {
                    IdentifierGridBind();
                    return;
                }
                string primaryKey = objUltraGridCell.Row.Cells["PrimaryKey"].Text;
                Identifier identifier = _clientIdentifiers.GetIdentifier(primaryKey);
                grdIdentifier.ActiveRow.Delete(false);
                _clientIdentifiers.Remove(identifier);
                IdentifierGridBind();

            }
        }

        public ClientFix clientFix
        {
            set
            {
                _clientFix = value;



            }
            get { return GetValues(); }
        }

        public Identifiers clientIdentifiers
        {
            set
            {
                _clientIdentifiers = value;

            }
            get
            {
                Identifiers clientIdentifiers1 = new Identifiers();
                foreach (Identifier identifier in _clientIdentifiers)
                {
                    if (!identifier.Equal(nullIdentifier))
                        clientIdentifiers1.Add(identifier);
                }

                return clientIdentifiers1;

            }
        }
        public int ClientID
        {
            //			set
            //			{
            //				_companyClientID=value;
            //				
            //					
            //				
            ////					
            //				
            //				
            //			}
            get { return _companyClientID; }
        }

    }
}
