using Prana.Admin.BLL;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for NewIdentifier.
    /// </summary>
    public class NewIdentifier : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbIdentifierID;
        private System.Windows.Forms.TextBox txtIdentifer;
        private System.Windows.Forms.Label lblIdentifer;
        private System.Windows.Forms.Label lblIdentifierID;
        private System.Windows.Forms.Label label3;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Private Members
        //private Identifiers _identifiers;
        private int _companyClientID;
        private System.Windows.Forms.Button btnAddNewAccount;
        const string C_COMBO_SELECT = "- Select -";
        private Identifier _addedIdentifier;
        private Identifiers _clientIdentifiers;

        #endregion

        public NewIdentifier(int companyClientID)
        {
            InitializeComponent();
            _companyClientID = companyClientID;
            BindIdentifier();
        }
        public NewIdentifier(int companyClientID, string IdentifierID, string ClientIdentifierName, string formName)
        {
            InitializeComponent();
            this.Text = formName;

            _companyClientID = companyClientID;
            BindIdentifier();
            cmbIdentifierID.Value = IdentifierID;
            cmbIdentifierID.Enabled = false;
            txtIdentifer.Text = ClientIdentifierName;

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
                if (cmbIdentifierID != null)
                {
                    cmbIdentifierID.Dispose();
                }
                if (txtIdentifer != null)
                {
                    txtIdentifer.Dispose();
                }
                if (lblIdentifer != null)
                {
                    lblIdentifer.Dispose();
                }
                if (lblIdentifierID != null)
                {
                    lblIdentifierID.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (btnAddNewAccount != null)
                {
                    btnAddNewAccount.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewIdentifier));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientIdentifierName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryKey", 3);
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddNewAccount = new System.Windows.Forms.Button();
            this.cmbIdentifierID = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtIdentifer = new System.Windows.Forms.TextBox();
            this.lblIdentifer = new System.Windows.Forms.Label();
            this.lblIdentifierID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIdentifierID)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddNewAccount);
            this.groupBox2.Controls.Add(this.cmbIdentifierID);
            this.groupBox2.Controls.Add(this.txtIdentifer);
            this.groupBox2.Controls.Add(this.lblIdentifer);
            this.groupBox2.Controls.Add(this.lblIdentifierID);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox2.Location = new System.Drawing.Point(4, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 106);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Identifier";
            // 
            // btnAddNewAccount
            // 
            this.btnAddNewAccount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddNewAccount.BackColor = System.Drawing.Color.LightCyan;
            this.btnAddNewAccount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddNewAccount.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNewAccount.Image")));
            this.btnAddNewAccount.Location = new System.Drawing.Point(91, 72);
            this.btnAddNewAccount.Name = "btnAddNewAccount";
            this.btnAddNewAccount.Size = new System.Drawing.Size(78, 26);
            this.btnAddNewAccount.TabIndex = 2;
            this.btnAddNewAccount.UseVisualStyleBackColor = false;
            this.btnAddNewAccount.Click += new System.EventHandler(this.btnAddNewAccount_Click);
            // 
            // cmbIdentifierID
            // 
            this.cmbIdentifierID.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbIdentifierID.DisplayLayout.Appearance = appearance1;
            this.cmbIdentifierID.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Header.Enabled = false;
            this.cmbIdentifierID.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbIdentifierID.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbIdentifierID.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbIdentifierID.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbIdentifierID.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbIdentifierID.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbIdentifierID.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbIdentifierID.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbIdentifierID.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbIdentifierID.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbIdentifierID.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbIdentifierID.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbIdentifierID.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbIdentifierID.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbIdentifierID.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbIdentifierID.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbIdentifierID.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbIdentifierID.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbIdentifierID.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbIdentifierID.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbIdentifierID.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbIdentifierID.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbIdentifierID.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbIdentifierID.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbIdentifierID.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbIdentifierID.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbIdentifierID.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbIdentifierID.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbIdentifierID.DropDownWidth = 0;
            this.cmbIdentifierID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbIdentifierID.Location = new System.Drawing.Point(98, 24);
            this.cmbIdentifierID.Name = "cmbIdentifierID";
            this.cmbIdentifierID.Size = new System.Drawing.Size(120, 21);
            this.cmbIdentifierID.TabIndex = 0;
            this.cmbIdentifierID.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // txtIdentifer
            // 
            this.txtIdentifer.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIdentifer.Location = new System.Drawing.Point(98, 46);
            this.txtIdentifer.Name = "txtIdentifer";
            this.txtIdentifer.Size = new System.Drawing.Size(120, 21);
            this.txtIdentifer.TabIndex = 1;
            // 
            // lblIdentifer
            // 
            this.lblIdentifer.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIdentifer.Location = new System.Drawing.Point(12, 49);
            this.lblIdentifer.Name = "lblIdentifer";
            this.lblIdentifer.Size = new System.Drawing.Size(80, 14);
            this.lblIdentifer.TabIndex = 2;
            this.lblIdentifer.Text = "Identifer Name";
            // 
            // lblIdentifierID
            // 
            this.lblIdentifierID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIdentifierID.Location = new System.Drawing.Point(12, 28);
            this.lblIdentifierID.Name = "lblIdentifierID";
            this.lblIdentifierID.Size = new System.Drawing.Size(60, 13);
            this.lblIdentifierID.TabIndex = 1;
            this.lblIdentifierID.Text = "Identifier";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(71, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 8);
            this.label3.TabIndex = 34;
            this.label3.Text = "*";
            // 
            // NewIdentifier
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(264, 113);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewIdentifier";
            this.Text = "NewIdentifier";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbIdentifierID)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion


        #region Private Methods
        private void BindIdentifier()
        {
            Identifiers _identifiers = AUECManager.GetIdentifiers();
            //_identifiers=ClientFixManager.GetClientIdentifiers(_companyClientID);
            _identifiers.Insert(0, new Identifier(int.MinValue, C_COMBO_SELECT));
            cmbIdentifierID.DisplayMember = "IdentifierName";
            cmbIdentifierID.ValueMember = "IdentifierID";
            cmbIdentifierID.DataSource = null;
            cmbIdentifierID.DataSource = _identifiers;
            cmbIdentifierID.Value = int.MinValue;
        }

        #endregion

        private void btnAddNewAccount_Click(object sender, System.EventArgs e)
        {
            if (int.Parse(cmbIdentifierID.Value.ToString()) == int.MinValue)
                return;
            if (txtIdentifer.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Fill the Identifier Name");
                return;
            }
            _addedIdentifier = new Identifier(int.Parse(cmbIdentifierID.Value.ToString()), cmbIdentifierID.SelectedRow.Cells["IdentifierName"].Value.ToString().ToUpper(), txtIdentifer.Text.Trim().ToUpper());
            if (_clientIdentifiers.Contains(_addedIdentifier))
            {
                _addedIdentifier = null;
                MessageBox.Show("Already Contains");
                return;
            }

            if (_clientIdentifiers.ContainsSameName(_addedIdentifier))
            {
                _addedIdentifier = null;
                MessageBox.Show("Already Contains this Identifier Name");

                return;
            }
            else
            {
                _addedIdentifier.PrimaryKey = System.Guid.NewGuid().ToString();
                this.Hide();

            }



        }
        public Identifier AddedIdentifier
        {
            get { return _addedIdentifier; }
            //	set{}
        }
        public Identifiers ClientIdentifiers
        {
            set { _clientIdentifiers = value; }
            get { return _clientIdentifiers; }

        }
    }
}
