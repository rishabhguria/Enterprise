using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CounterPartyVenueSymbolMapping.
    /// </summary>
    public class CounterPartyVenueSymbolMapping : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "Login : ";
        const string C_COMBO_SELECT = "- Select -";

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMappedSymbol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private IContainer components;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSymbol;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAUEC;

        Prana.Admin.BLL.SymbolMapping _symbolMappingEdit = null;

        public Prana.Admin.BLL.SymbolMapping SymbolMappingEdit
        {
            set { _symbolMappingEdit = value; }
        }

        public CounterPartyVenueSymbolMapping()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            try
            {
                BindCVAUEC();
                //BindSymbol();
                Refresh();
                ResetErrorMsg();
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
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtSymbol != null)
                {
                    txtSymbol.Dispose();
                }
                if (btnCancel != null)
                {
                    btnCancel.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (txtMappedSymbol != null)
                {
                    txtMappedSymbol.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (cmbAUEC != null)
                {
                    cmbAUEC.Dispose();
                }
                if (_statusBar != null)
                {
                    _statusBar.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CounterPartyVenueSymbolMapping));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMappedSymbol = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAUEC = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMappedSymbol);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbAUEC);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 94);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(46, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 33;
            this.label4.Text = "*";
            // 
            // txtMappedSymbol
            // 
            this.txtMappedSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMappedSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtMappedSymbol.Location = new System.Drawing.Point(116, 68);
            this.txtMappedSymbol.MaxLength = 50;
            this.txtMappedSymbol.Name = "txtMappedSymbol";
            this.txtMappedSymbol.Size = new System.Drawing.Size(208, 21);
            this.txtMappedSymbol.TabIndex = 11;
            this.txtMappedSymbol.LostFocus += new System.EventHandler(this.txtMappedSymbol_LostFocus);
            this.txtMappedSymbol.GotFocus += new System.EventHandler(this.txtMappedSymbol_GotFocus);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(5, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Mapped Symbol";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Symbol";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(5, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "AUEC";
            // 
            // cmbAUEC
            // 
            this.cmbAUEC.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAUEC.DisplayLayout.Appearance = appearance1;
            this.cmbAUEC.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbAUEC.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbAUEC.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAUEC.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbAUEC.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbAUEC.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAUEC.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAUEC.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAUEC.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAUEC.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbAUEC.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAUEC.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbAUEC.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbAUEC.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAUEC.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbAUEC.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbAUEC.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAUEC.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbAUEC.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAUEC.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAUEC.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAUEC.DisplayMember = "";
            this.cmbAUEC.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAUEC.DropDownWidth = 0;
            this.cmbAUEC.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAUEC.Location = new System.Drawing.Point(116, 24);
            this.cmbAUEC.Name = "cmbAUEC";
            this.cmbAUEC.Size = new System.Drawing.Size(208, 21);
            this.cmbAUEC.TabIndex = 36;
            this.cmbAUEC.ValueMember = "";
            this.cmbAUEC.LostFocus += new System.EventHandler(this.cmbAUEC_LostFocus);
            this.cmbAUEC.GotFocus += new System.EventHandler(this.cmbAUEC_GotFocus);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(95, 96);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCancel.Location = new System.Drawing.Point(173, 96);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(62, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 10);
            this.label5.TabIndex = 33;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(108, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 8);
            this.label6.TabIndex = 34;
            this.label6.Text = "*";
            // 
            // txtSymbol
            // 
            this.txtSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSymbol.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtSymbol.Location = new System.Drawing.Point(124, 46);
            this.txtSymbol.MaxLength = 50;
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(208, 21);
            this.txtSymbol.TabIndex = 35;
            this.txtSymbol.LostFocus += new System.EventHandler(this.txtSymbol_LostFocus);
            this.txtSymbol.GotFocus += new System.EventHandler(this.txtSymbol_GotFocus);
            // 
            // CounterPartyVenueSymbolMapping
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(362, 127);
            this.Controls.Add(this.txtSymbol);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.MaximizeBox = false;
            this.Name = "CounterPartyVenueSymbolMapping";
            this.Text = "Symbol Mapping";
            this.Load += new System.EventHandler(this.CounterPartyVenueSymbolMapping_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void RefreshWindow()
        {
            txtMappedSymbol.Text = "";
            txtSymbol.Text = "";
        }
        //		private void BindSymbol()
        //		{
        //			Symbols symbols = SymbolManager.GetSymbols();
        //			if (symbols.Count > 0 )
        //			{
        //				symbols.Insert(0, new Symbol(int.MinValue, "-Select-"));
        //				cmbSymbol.DataSource = symbols;
        //				cmbSymbol.DisplayMember = "CompanySymbol";
        //				cmbSymbol.ValueMember = "SymbolID";
        //			}
        //			if(_symbolMappingEdit != null)
        //			{
        //				cmbSymbol.SelectedValue = _symbolMappingEdit.SymbolID;
        //			}
        //		}

        private void BindCVAUEC()
        {
            //AUECs auecs = AUECManager.GetAUEC();
            AUECs cvAUECs = CounterPartyManager.GetCVAUECs(_cvID);

            System.Data.DataTable dtauec = new System.Data.DataTable();
            dtauec.Columns.Add("Data");
            dtauec.Columns.Add("Value");
            object[] row = new object[2];
            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            dtauec.Rows.Add(row);

            if (cvAUECs.Count > 0)
            {
                foreach (AUEC cvAUEC in cvAUECs)
                {
                    //SK 2061009 removed Compliance class
                    //Currency currency = new Currency();
                    ////currency = AUECManager.GetCurrency(auec.Exchange.Currency);
                    //currency = AUECManager.GetCurrency(cvAUEC.Compliance.BaseCurrencyID);
                    //

                    //string Data = cvAUEC.Asset.Name.ToString() + "/" + cvAUEC.UnderLying.Name.ToString() + "/" + cvAUEC.DisplayName.ToString() + "/" + cvAUEC.Currency.CurrencySymbol.ToString();
                    string Data = cvAUEC.AUECString;
                    int Value = cvAUEC.AUECID;

                    row[0] = Data;
                    row[1] = Value;
                    dtauec.Rows.Add(row);
                }
            }
            cmbAUEC.DataSource = null;
            cmbAUEC.DataSource = dtauec;
            //auecs.Insert(0, new AUEC(int.MinValue, C_COMBO_SELECT));
            cmbAUEC.DisplayMember = "Data";
            cmbAUEC.ValueMember = "Value";

            cmbAUEC.Value = int.MinValue;

            if (_symbolMappingEdit != null)
            {
                int cvAUECID = _symbolMappingEdit.CVAUECID;
                CounterPartyVenue cvAUEC = new CounterPartyVenue();
                cvAUEC = CounterPartyManager.GetCVAUEC(cvAUECID);
                int aUECID = int.Parse(cvAUEC.AUECID.ToString()); //This is being done as the cmbAUEC is binded by the AUEC Ids instead of CVAUECIDs.

                cmbAUEC.Value = aUECID;
                txtSymbol.Text = _symbolMappingEdit.Symbol;
                txtMappedSymbol.Text = _symbolMappingEdit.MappedSymbol;
            }
        }

        private StatusBar _statusBar = null;
        public StatusBar ParentStatusBar
        {
            set { _statusBar = value; }
        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtMappedSymbol.Text = "";
            txtSymbol.Text = "";
        }
        public void ResetErrorMsg()
        {
            errorProvider1.SetError(txtMappedSymbol, "");
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtSymbol, "");
        }

        private void CounterPartyVenueSymbolMapping_Load(object sender, System.EventArgs e)
        {
            BindCVAUEC();
            //BindSymbol();
            if (_symbolMappingEdit != null)
            {
            }
            else
            {
                Refresh(sender, e);
                //stbSymbolMapping.Text = "";
            }
        }

        #region Controls Focus Colors
        private void cmbAUEC_GotFocus(object sender, System.EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbAUEC_LostFocus(object sender, System.EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.White;
        }
        private void txtSymbol_GotFocus(object sender, System.EventArgs e)
        {
            txtSymbol.BackColor = Color.LemonChiffon;
        }
        private void txtSymbol_LostFocus(object sender, System.EventArgs e)
        {
            txtSymbol.BackColor = Color.White;
        }
        private void txtMappedSymbol_GotFocus(object sender, System.EventArgs e)
        {
            txtMappedSymbol.BackColor = Color.LemonChiffon;
        }
        private void txtMappedSymbol_LostFocus(object sender, System.EventArgs e)
        {
            txtMappedSymbol.BackColor = Color.White;
        }
        #endregion


        private int _cvID = int.MaxValue;
        public int CVID
        {
            get
            {
                return _cvID;
            }
            set
            {
                _cvID = value;
            }
        }

        private Prana.Admin.BLL.SymbolMappings _symbolMappings = new SymbolMappings();
        public SymbolMappings CurrentSymbolMappings
        {
            get
            {
                return _symbolMappings;
            }
            set
            {
                if (value != null)
                {
                    _symbolMappings = value;
                }
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

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                int auecID = int.MinValue;
                if (_noData == 1)
                {
                    _symbolMappings.Clear();
                }
                if (_symbolMappingEdit != null)
                {
                    if (int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
                        cmbAUEC.Focus();
                    }
                    else if (txtSymbol.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtSymbol, "Please enter Symbol !");
                        txtSymbol.Focus();
                    }
                    else if (txtMappedSymbol.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtMappedSymbol, "Please enter Mapped Symbol Name!");
                        txtMappedSymbol.Focus();
                    }
                    else
                    {
                        int index = _symbolMappings.IndexOf(_symbolMappingEdit);

                        //if (index == int.MinValue)
                        //{
                        auecID = int.Parse(cmbAUEC.Value.ToString());
                        CounterPartyVenue auecCV = new CounterPartyVenue();
                        auecCV = CounterPartyManager.GetCVAUECDetails(_cvID, auecID);

                        int cout = 0;

                        foreach (Prana.Admin.BLL.SymbolMapping symbolMaps in _symbolMappings)
                        {
                            if (symbolMaps.AUECID == auecID && symbolMaps.Symbol == txtSymbol.Text && symbolMaps.MappedSymbol == txtMappedSymbol.Text && symbolMaps.CVSymboMappingID != _symbolMappingEdit.CVSymboMappingID)
                            {
                                cout = cout + 1;
                                break;
                            }
                        }
                        if (cout > 0)
                        {
                            MessageBox.Show("The combination of AUEC - " + cmbAUEC.Text + " , Symbol - " + txtSymbol.Text + "  and Mapped Symbol - " + txtMappedSymbol.Text + "   Already exists !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }


                        ((Prana.Admin.BLL.SymbolMapping)_symbolMappings[index]).CVAUECID = auecCV.CVAUECID;
                        ((Prana.Admin.BLL.SymbolMapping)_symbolMappings[index]).AUEC = cmbAUEC.Text.ToString();
                        ((Prana.Admin.BLL.SymbolMapping)_symbolMappings[index]).AUECID = int.Parse(cmbAUEC.Value.ToString());
                        ((Prana.Admin.BLL.SymbolMapping)_symbolMappings[index]).Symbol = txtSymbol.Text.ToString();
                        //((Prana.Admin.BLL.SymbolMapping)_symbolMappings[index]).SymbolName = cmbSymbol.SelectedText.ToString();
                        ((Prana.Admin.BLL.SymbolMapping)_symbolMappings[index]).MappedSymbol = txtMappedSymbol.Text.ToString();
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbSymbolMapping);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbSymbolMapping, "Stored!");
                        this.Hide();
                    }
                }
                else
                {
                    errorProvider1.SetError(txtMappedSymbol, "");
                    errorProvider1.SetError(cmbAUEC, "");
                    errorProvider1.SetError(txtSymbol, "");
                    if (int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
                        cmbAUEC.Focus();
                    }
                    else if (txtSymbol.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtSymbol, "Please enter Symbol !");
                        txtSymbol.Focus();
                    }
                    else if (txtMappedSymbol.Text.Trim() == "")
                    {
                        errorProvider1.SetError(txtMappedSymbol, "Please enter Mapped Symbol Name!");
                        txtMappedSymbol.Focus();
                    }
                    else
                    {
                        Prana.Admin.BLL.SymbolMapping symbolMapping = new Prana.Admin.BLL.SymbolMapping();

                        auecID = int.Parse(cmbAUEC.Value.ToString());
                        symbolMapping.AUECID = auecID;
                        CounterPartyVenue auecCV = new CounterPartyVenue();
                        auecCV = CounterPartyManager.GetCVAUECDetails(_cvID, auecID);
                        symbolMapping.CVAUECID = auecCV.CVAUECID;

                        symbolMapping.AUEC = cmbAUEC.Text.ToString();
                        //symbolMapping.SymbolID = int.Parse(cmbSymbol.SelectedValue.ToString());
                        symbolMapping.Symbol = txtSymbol.Text.ToString();
                        symbolMapping.MappedSymbol = txtMappedSymbol.Text.ToString();
                        foreach (Prana.Admin.BLL.SymbolMapping symbolMaps in _symbolMappings)
                        {
                            if (symbolMaps.AUECID == auecID && symbolMaps.Symbol == txtSymbol.Text && symbolMaps.MappedSymbol == txtMappedSymbol.Text)
                            {
                                MessageBox.Show("The combination of AUEC - " + cmbAUEC.Text + " , Symbol - " + txtSymbol.Text + "  and Mapped Symbol - " + txtMappedSymbol.Text + "   Already exists !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        _symbolMappings.Add(symbolMapping);
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbSymbolMapping);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbSymbolMapping, "Stored!");
                        this.Hide();
                    }
                }

                //}
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

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

    }
}
