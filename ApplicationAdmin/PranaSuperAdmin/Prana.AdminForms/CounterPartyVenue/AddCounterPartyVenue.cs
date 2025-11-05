#region Using Namespaces
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for AddCounterPartyVenue.
    /// </summary>
    public class AddCounterPartyVenue : System.Windows.Forms.Form
    {
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "AddCounterPartyVenue : ";

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.StatusBar stbAddCounterPartyVenue;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterParty;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbVenue;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public AddCounterPartyVenue()
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnCancel != null)
                {
                    btnCancel.Dispose();
                }
                if (stbAddCounterPartyVenue != null)
                {
                    stbAddCounterPartyVenue.Dispose();
                }
                if (statusBarPanel1 != null)
                {
                    statusBarPanel1.Dispose();
                }
                if (cmbCounterParty != null)
                {
                    cmbCounterParty.Dispose();
                }
                if (statusBarPanel1 != null)
                {
                    statusBarPanel1.Dispose();
                }
                if (cmbVenue != null)
                {
                    cmbVenue.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCounterPartyVenue));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.stbAddCounterPartyVenue = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbCounterParty);
            this.groupBox1.Controls.Add(this.cmbVenue);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.groupBox1.Location = new System.Drawing.Point(5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label2.Location = new System.Drawing.Point(232, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Venue";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = ApplicationConstants.CONST_BROKER;
            // 
            // cmbCounterParty
            // 
            this.cmbCounterParty.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance1;
            this.cmbCounterParty.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbCounterParty.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.DisplayMember = "";
            this.cmbCounterParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterParty.DropDownWidth = 0;
            this.cmbCounterParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterParty.Location = new System.Drawing.Point(102, 24);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(100, 21);
            this.cmbCounterParty.TabIndex = 6;
            this.cmbCounterParty.ValueMember = "";
            this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
            // 
            // cmbVenue
            // 
            this.cmbVenue.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenue.DisplayLayout.Appearance = appearance13;
            this.cmbVenue.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            this.cmbVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenue.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenue.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenue.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenue.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbVenue.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenue.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenue.DisplayMember = "";
            this.cmbVenue.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenue.DropDownWidth = 0;
            this.cmbVenue.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbVenue.Location = new System.Drawing.Point(286, 24);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(100, 21);
            this.cmbVenue.TabIndex = 7;
            this.cmbVenue.ValueMember = "";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(132, 70);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCancel.Location = new System.Drawing.Point(212, 70);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // stbAddCounterPartyVenue
            // 
            this.stbAddCounterPartyVenue.Location = new System.Drawing.Point(0, 95);
            this.stbAddCounterPartyVenue.Name = "stbAddCounterPartyVenue";
            this.stbAddCounterPartyVenue.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.stbAddCounterPartyVenue.Size = new System.Drawing.Size(414, 22);
            this.stbAddCounterPartyVenue.TabIndex = 3;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
            this.statusBarPanel1.Text = "statusBarPanel1";
            // 
            // AddCounterPartyVenue
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(414, 117);
            this.Controls.Add(this.stbAddCounterPartyVenue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCounterPartyVenue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Boker Venue";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AddCounterPartyVenue_FormClosed);
            this.Load += new System.EventHandler(this.AddCounterPartyVenue_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Various Bind methods are called on the on Load event of the AddCounterPartyVenue form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCounterPartyVenue_Load(object sender, System.EventArgs e)
        {
            try
            {
                BindCounterParties();
                BindVenues();
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("AddCounterPartyVenue_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "AddCounterPartyVenue_Load", null);


                #endregion
            }

        }

        /// <summary>
        /// This method binds the existing <see cref="CounterParties"/> in the ComboBox control by assigning the 
        /// counterParties object to its datasource property.
        /// </summary>
        private void BindCounterParties()
        {
            CounterParties counterParties = CounterPartyManager.GetCounterParties();
            if (counterParties != null)
            {
                if (counterParties.Count > 0)
                {
                    counterParties.Insert(0, new CounterParty(int.MinValue, C_COMBO_SELECT));
                    cmbCounterParty.DataSource = null;
                    cmbCounterParty.DataSource = counterParties;
                    cmbCounterParty.DisplayMember = "CounterPartyFullName";
                    cmbCounterParty.ValueMember = "CounterPartyID";
                    cmbCounterParty.Text = C_COMBO_SELECT;
                }
                if (_counterPartyID > int.MinValue)
                {
                    cmbCounterParty.Value = _counterPartyID;
                }
                ColumnsCollection columns7 = cmbCounterParty.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns7)
                {
                    if (column.Key != "CounterPartyFullName")
                    {
                        column.Hidden = true;
                    }
                }
            }
        }

        /// <summary>
        /// This method binds the existing <see cref="Venues"/> in the ComboBox control by assigning the 
        /// venues object to its datasource property.
        /// </summary>
        private void BindVenues()
        {
            Venues venues = VenueManager.GetVenues();
            venues.Insert(0, new Venue(int.MinValue, C_COMBO_SELECT));
            cmbVenue.DataSource = null;
            cmbVenue.DataSource = venues;
            cmbVenue.DisplayMember = "VenueName";
            cmbVenue.ValueMember = "VenueID";
            cmbVenue.Value = int.MinValue;

            ColumnsCollection columns6 = cmbVenue.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns6)
            {
                if (column.Key != "VenueName")
                {
                    column.Hidden = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counterPartyID"></param>
        private void BindVenues(int counterPartyID)
        {
            Venues independentVenues = new Venues();
            Venues totalVenues = VenueManager.GetVenues();
            Venues venues = VenueManager.GetCounterPartyVenues(counterPartyID);

            bool flag = false;
            foreach (Venue totalVenue in totalVenues)
            {
                flag = false;
                foreach (Venue venue in venues)
                {
                    if (totalVenue.VenueID == venue.VenueID)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    independentVenues.Add(totalVenue);
                    //flag = true;
                }
            }

            independentVenues.Insert(0, new Venue(int.MinValue, C_COMBO_SELECT));
            cmbVenue.DataSource = null;
            cmbVenue.DataSource = independentVenues;
            cmbVenue.DisplayMember = "VenueName";
            cmbVenue.ValueMember = "VenueID";
            cmbVenue.Value = int.MinValue;

            ColumnsCollection columnVenues = cmbVenue.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnVenues)
            {
                if (column.Key != "VenueName")
                {
                    column.Hidden = true;
                }
            }
        }

        /// <summary>
        /// This method closes the form when clicked the close button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            // suppose one record we added, the following variable sometimes does not loose thier values
            // even after closing the Form, so again assign them default vaules.
            _savedCounterPartyVenueID = int.MinValue;
            _counterPartyID = int.MinValue;
            _venueID = int.MinValue;
            this.Close();
        }

        private int _savedCounterPartyVenueID = int.MinValue;
        public int SavedCounterPartyVenueID
        {
            get
            {
                return _savedCounterPartyVenueID;
            }
        }

        private int _counterPartyID = int.MinValue;
        public int CounterPartyID
        {
            set
            {
                _counterPartyID = value;
            }
            get
            {
                return _counterPartyID;
            }
        }

        private int _venueID = int.MinValue;
        public int VenueID
        {
            set
            {
                _venueID = value;
            }
            get
            {
                return _venueID;
            }
        }

        public void SetCounterPartyVenue(CounterPartyVenue counterPartyVenue)
        {

        }

        public void GetCounterPartyVenue(CounterPartyVenue counterPartyVenue)
        {


        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                SaveCounterParty_Venue();
                if (_savedCounterPartyVenueID != int.MinValue)
                {
                    this.Hide();
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion
            }

        }


        /// <summary>
        /// Saves the counter party venue combination while checking for the existing combination in the database.
        /// </summary>
        /// <returns>The saved counterpartyvenue id.</returns>
        private int SaveCounterParty_Venue()
        {
            int result = int.MinValue;

            Prana.Admin.BLL.CounterPartyVenue counterPartyVenue = new Prana.Admin.BLL.CounterPartyVenue();
            if (int.Parse(cmbCounterParty.Value.ToString()) == int.MinValue || int.Parse(cmbVenue.Value.ToString()) == int.MinValue)
            {
                //Common.ResetStatusPanel(stbAddCounterPartyVenue);
                //Common.SetStatusPanel(stbAddCounterPartyVenue, "Can't add this combination.");
                MessageBox.Show("Please Select some valid counter party and venue");
                //Saving min value to _savedCounterPartyVenueID so as to pass the info that nothing meaningful CV is added.

                _counterPartyID = int.MinValue;
                _venueID = int.MinValue;
                _savedCounterPartyVenueID = int.MinValue;
                return result;
            }
            else
            {
                //Common.ResetStatusPanel(stbAddCounterPartyVenue);
                counterPartyVenue.CounterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                _counterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                counterPartyVenue.VenueID = int.Parse(cmbVenue.Value.ToString());
                _venueID = int.Parse(cmbVenue.Value.ToString());

                //Saving 1 to _savedCounterPartyVenueID so as to pass the info that some meaningful CV is added.
                _savedCounterPartyVenueID = 1; //Hard coded for now.
            }

            //The following code is commeted as already existing CV combination cant be made because of the changes. 
            //int newcounterPartyVenueID = CounterPartyManager.SaveCounterPartyVenueDialog(counterPartyVenue);
            //if(newcounterPartyVenueID == -2)
            //{
            //    MessageBox.Show("Counter Party already exists with given venue name. Please select other Counter Party Venue");
            //}	
            //else
            //{
            //    //Common.ResetStatusPanel(stbAddCounterPartyVenue);
            //    //Common.SetStatusPanel(stbAddCounterPartyVenue, "Stored!");		
            //}

            //Returning the min value for int because as per the changes CV cant be saved in the DB before saving the details against it.
            int newcounterPartyVenueID = int.MinValue;
            result = newcounterPartyVenueID;
            //_savedCounterPartyVenueID = result;			
            return result;
        }

        #region Focus Colors		
        private void cmbCounterParty_GotFocus(object sender, System.EventArgs e)
        {
            cmbCounterParty.BackColor = Color.LemonChiffon;
        }
        private void cmbCounterParty_LostFocus(object sender, System.EventArgs e)
        {
            cmbCounterParty.BackColor = Color.White;
        }

        private void cmbVenue_GotFocus(object sender, System.EventArgs e)
        {
            cmbVenue.BackColor = Color.LemonChiffon;
        }
        private void cmbVenue_LostFocus(object sender, System.EventArgs e)
        {
            cmbVenue.BackColor = Color.White;
        }
        #endregion

        private void label2_Click(object sender, System.EventArgs e)
        {

        }

        private void cmbCounterParty_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCounterParty.Value != null)
            {
                int counterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                BindVenues(counterPartyID);
            }
        }

        private void AddCounterPartyVenue_FormClosed(object sender, FormClosedEventArgs e)
        {
            // suppose one record we added, the following variable sometimes does not loose thier values
            // even after closing the Form, so again assign them default vaules.
            _savedCounterPartyVenueID = int.MinValue;
            _counterPartyID = int.MinValue;
            _venueID = int.MinValue;
        }
    }
}
