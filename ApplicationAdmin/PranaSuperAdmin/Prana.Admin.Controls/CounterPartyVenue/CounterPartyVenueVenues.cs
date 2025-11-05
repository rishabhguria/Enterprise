#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CounterPartyVenueVenues.
    /// </summary>
    public class CounterPartyVenueVenues : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "CounterPartyVenueVenues : ";
        const string C_COMBO_SELECT = "- Select -";
        const int VENUE_EXCHANGES = 1;
        const string VENUE_TYPE = "Exchangex";

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtFullNameVenue;
        private System.Windows.Forms.TextBox txtVenueShortName;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbVenueType;
        private IContainer components;

        public CounterPartyVenueVenues()
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
                if (groupBox3 != null)
                {
                    groupBox3.Dispose();
                }
                if (label46 != null)
                {
                    label46.Dispose();
                }
                if (label45 != null)
                {
                    label45.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (txtFullNameVenue != null)
                {
                    txtFullNameVenue.Dispose();
                }
                if (txtVenueShortName != null)
                {
                    txtVenueShortName.Dispose();
                }
                if (cmbVenueType != null)
                {
                    cmbVenueType.Dispose();
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVenueShortName = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.txtFullNameVenue = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVenueType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenueType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtVenueShortName);
            this.groupBox3.Controls.Add(this.label46);
            this.groupBox3.Controls.Add(this.label45);
            this.groupBox3.Controls.Add(this.txtFullNameVenue);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cmbVenueType);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.groupBox3.Location = new System.Drawing.Point(4, -2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(286, 88);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(98, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 10);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // txtVenueShortName
            // 
            this.txtVenueShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVenueShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtVenueShortName.Location = new System.Drawing.Point(116, 40);
            this.txtVenueShortName.MaxLength = 50;
            this.txtVenueShortName.Name = "txtVenueShortName";
            this.txtVenueShortName.Size = new System.Drawing.Size(152, 21);
            this.txtVenueShortName.TabIndex = 7;
            this.txtVenueShortName.LostFocus += new System.EventHandler(this.txtVenueShortName_LostFocus);
            this.txtVenueShortName.GotFocus += new System.EventHandler(this.txtVenueShortName_GotFocus);
            // 
            // label46
            // 
            this.label46.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label46.Location = new System.Drawing.Point(8, 64);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(90, 16);
            this.label46.TabIndex = 3;
            this.label46.Text = "Type of Venue";
            // 
            // label45
            // 
            this.label45.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label45.Location = new System.Drawing.Point(8, 42);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(76, 16);
            this.label45.TabIndex = 2;
            this.label45.Text = "Short Name";
            // 
            // txtFullNameVenue
            // 
            this.txtFullNameVenue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullNameVenue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFullNameVenue.Location = new System.Drawing.Point(116, 18);
            this.txtFullNameVenue.MaxLength = 50;
            this.txtFullNameVenue.Name = "txtFullNameVenue";
            this.txtFullNameVenue.Size = new System.Drawing.Size(152, 21);
            this.txtFullNameVenue.TabIndex = 6;
            this.txtFullNameVenue.LostFocus += new System.EventHandler(this.txtFullNameVenue_LostFocus);
            this.txtFullNameVenue.GotFocus += new System.EventHandler(this.txtFullNameVenue_GotFocus);
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label19.Location = new System.Drawing.Point(6, 20);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(62, 16);
            this.label19.TabIndex = 0;
            this.label19.Text = "Full Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(84, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 6);
            this.label2.TabIndex = 33;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(68, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 6);
            this.label1.TabIndex = 34;
            this.label1.Text = "*";
            // 
            // cmbVenueType
            // 
            this.cmbVenueType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenueType.DisplayLayout.Appearance = appearance1;
            this.cmbVenueType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbVenueType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbVenueType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenueType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenueType.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenueType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbVenueType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenueType.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbVenueType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenueType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenueType.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenueType.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbVenueType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenueType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenueType.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenueType.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbVenueType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenueType.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenueType.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbVenueType.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbVenueType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenueType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenueType.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbVenueType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenueType.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbVenueType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenueType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenueType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenueType.DisplayMember = "";
            this.cmbVenueType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenueType.DropDownWidth = 0;
            this.cmbVenueType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbVenueType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbVenueType.Location = new System.Drawing.Point(116, 62);
            this.cmbVenueType.Name = "cmbVenueType";
            this.cmbVenueType.Size = new System.Drawing.Size(152, 21);
            this.cmbVenueType.TabIndex = 36;
            this.cmbVenueType.ValueMember = "";
            this.cmbVenueType.LostFocus += new System.EventHandler(this.cmbVenueType_LostFocus);
            this.cmbVenueType.GotFocus += new System.EventHandler(this.cmbVenueType_GotFocus);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(8, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 14);
            this.label3.TabIndex = 35;
            this.label3.Text = "* Required Field";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CounterPartyVenueVenues
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CounterPartyVenueVenues";
            this.Size = new System.Drawing.Size(294, 98);
            this.Load += new System.EventHandler(this.CounterPartyVenueVenues_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenueType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        public Venue VenueProperty
        {
            get { return GetVenueDetails(); }
            set
            {
                //SetVenueDetails(value);
            }
        }

        private Venue GetVenueDetails()
        {
            Venue venue = new Venue();
            venue.VenueName = txtVenueShortName.Text.Trim();
            venue.VenueTypeID = int.Parse(cmbVenueType.Value.ToString());
            venue.Route = txtFullNameVenue.Text.Trim();

            return venue;
        }

        private void BindVenueTypesWithoutExchange()
        {
            VenueTypes venueTypes = VenueManager.GetVenueTypes();
            venueTypes.Insert(0, new VenueType(int.MinValue, C_COMBO_SELECT));
            VenueType toRemVenueType = new VenueType();
            toRemVenueType = null;
            foreach (VenueType venueType in venueTypes)
            {
                if (venueType.VenueTypeID == VENUE_EXCHANGES)
                {
                    toRemVenueType = venueType;
                }
            }
            venueTypes.Remove(toRemVenueType);
            cmbVenueType.DataSource = null;
            cmbVenueType.DataSource = venueTypes;
            cmbVenueType.DisplayMember = "Type";
            cmbVenueType.ValueMember = "VenueTypeID";
            cmbVenueType.Text = C_COMBO_SELECT;
            ResetVenueType();
        }

        public void SetVenueDetails(Venue venue)
        {
            //if (_exchangeTypeID != VENUE_EXCHANGES)
            //{
            //    BindVenueTypesWithoutExchange();                 
            //}
            //else
            //{
            //    BindVenueTypes();
            //}
            if (venue != null)
            {
                _venueID = venue.VenueID;
                if (int.Parse(venue.VenueID.ToString()) != int.MinValue)
                {
                    txtVenueShortName.Text = venue.VenueName;
                    cmbVenueType.Value = venue.VenueTypeID;
                    txtFullNameVenue.Text = venue.Route;

                    ColumnsCollection columns = cmbVenueType.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (column.Key != "Type")
                        {
                            column.Hidden = true;
                        }
                    }
                }
                else
                {
                    Exchange exchange = ExchangeManager.GetExchange(_exchangeID);
                    if (exchange != null)
                    {
                        txtVenueShortName.Text = exchange.DisplayName;
                        cmbVenueType.Value = _exchangeTypeID;
                        txtFullNameVenue.Text = exchange.Name;

                        ColumnsCollection columns = cmbVenueType.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            if (column.Key != "Type")
                            {
                                column.Hidden = true;
                            }
                        }
                    }
                    else
                    {
                        VenueRefresh();
                    }
                }
            }
            ResetVenueType();
        }

        private void BindVenueTypes()
        {
            VenueTypes venueTypes = VenueManager.GetVenueTypes();
            //			if(venueTypes.Count > 0)
            //			{
            venueTypes.Insert(0, new VenueType(int.MinValue, C_COMBO_SELECT));
            cmbVenueType.DataSource = null;
            cmbVenueType.DataSource = venueTypes;

            cmbVenueType.DisplayMember = "Type";
            cmbVenueType.ValueMember = "VenueTypeID";
            cmbVenueType.Text = C_COMBO_SELECT;
            ResetVenueType();
            //			}
        }

        private void ResetVenueType()
        {
            ColumnsCollection columnsVenues = cmbVenueType.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsVenues)
            {
                if (column.Key != "Type")
                {
                    column.Hidden = true;
                }
            }
        }

        private void CounterPartyVenueVenues_Load(object sender, System.EventArgs e)
        {
        }

        private StatusBar _statusBar = null;
        public StatusBar ParentStatusBar
        {
            set { _statusBar = value; }
        }

        public void SetupControl(int exchangeTypeID, string venueType, int exchangeID, int venueID)
        {
            _exchangeTypeID = exchangeTypeID;
            _venueType = venueType;
            _exchangeID = exchangeID;
            _venueID = venueID;

            BindVenueTypes();

            if (_exchangeTypeID == int.MinValue && _venueType != "VenueMasterRoot")
            {
                BindVenueTypes();
                cmbVenueType.Value = VENUE_EXCHANGES;
                cmbVenueType.ReadOnly = true;
                txtVenueShortName.ReadOnly = true;
                txtFullNameVenue.ReadOnly = true;
            }
            else
            {
                cmbVenueType.ReadOnly = false;
                txtVenueShortName.ReadOnly = false;
                txtFullNameVenue.ReadOnly = false;
                //BindVenueTypesWithoutExchange();
            }

            //if (_exchangeTypeID == VENUE_EXCHANGES)
            //{
            //    cmbVenueType.ReadOnly = true;
            //    txtVenueShortName.ReadOnly = true;
            //    txtFullNameVenue.ReadOnly = true;
            //}
            //else
            //{
            //    cmbVenueType.ReadOnly = false;
            //    txtVenueShortName.ReadOnly = false;
            //    txtFullNameVenue.ReadOnly = false;
            //}
            //if (_exchangeTypeID == VENUE_EXCHANGES)
            //{
            //    BindVenueTypes();
            //}
            //else
            //{
            //    BindVenueTypesWithoutExchange();
            //}
        }

        private int _exchangeTypeID;
        public int ExchangeTypeID
        {
            set
            {
                //_exchangeTypeID = value;
                //if(_exchangeTypeID == VENUE_EXCHANGES)
                //{
                //    cmbVenueType.ReadOnly = true;
                //    txtVenueShortName.ReadOnly = true;
                //    txtFullNameVenue.ReadOnly = true;
                //}
                //else
                //{
                //    cmbVenueType.ReadOnly = false;
                //    txtVenueShortName.ReadOnly = false;
                //    txtFullNameVenue.ReadOnly = false;
                //}
                //if (_exchangeTypeID == VENUE_EXCHANGES)
                //{
                //    BindVenueTypes();
                //}
                //else
                //{
                //    BindVenueTypesWithoutExchange();
                //}
            }
        }

        private string _venueType;
        public string ExchangeType
        {
            set
            {
                //_exchangeType = value.ToString();
            }
        }

        public void VenueRefresh()
        {
            txtFullNameVenue.ReadOnly = false;
            txtVenueShortName.ReadOnly = false;
            cmbVenueType.ReadOnly = false;

            txtFullNameVenue.Text = "";
            txtVenueShortName.Text = "";
            cmbVenueType.Value = int.MinValue;
        }

        private int _venueID = int.MinValue;

        public int VenueID
        {
            set
            {
                //_venueID = value;
            }
        }

        private int _exchangeID = int.MinValue;
        public int ExchangeID
        {
            set
            {
                //_exchangeID = value;
            }
        }

        private int _venueTypeID = int.MinValue;
        public int VenueTypeID
        {
            get { return _venueTypeID; }
        }

        public int SaveVenues()
        {
            int result = int.MinValue;

            errorProvider1.SetError(txtVenueShortName, "");
            errorProvider1.SetError(cmbVenueType, "");
            errorProvider1.SetError(txtFullNameVenue, "");

            if (txtFullNameVenue.Text.Trim() == "")
            {
                errorProvider1.SetError(txtFullNameVenue, "Please enter Full Name!");
                txtFullNameVenue.Focus();
                return result;
            }
            else if (txtVenueShortName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtVenueShortName, "Please enter Short name!");
                txtVenueShortName.Focus();
                return result;
            }

            if (_exchangeID == int.MinValue)
            {
                if (int.Parse(cmbVenueType.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbVenueType, "Please select Venue Type!");
                    cmbVenueType.Focus();
                    return result;
                }
            }
            Prana.Admin.BLL.Venue venue = new Prana.Admin.BLL.Venue();
            venue.VenueID = _venueID;
            venue.VenueName = txtVenueShortName.Text.Trim();
            venue.VenueTypeID = int.Parse(cmbVenueType.Value.ToString());
            if (int.Parse(cmbVenueType.Value.ToString()) == int.MinValue)
            {
                venue.VenueTypeID = 1;
            }

            _venueTypeID = int.Parse(cmbVenueType.Value.ToString());
            venue.Route = txtFullNameVenue.Text.Trim();
            venue.ExchangeID = _exchangeID;

            int venueID = VenueManager.SaveVenue(venue);
            if (venueID == -1)
            {
                MessageBox.Show("Venue with the same name already exists.", "Alert", MessageBoxButtons.OK);
            }
            else
            {
                //Prana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
                //Prana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Stored!");
            }
            result = venueID;

            return result;
        }

        #region Controls Focus Colors

        private void txtFullNameVenue_GotFocus(object sender, System.EventArgs e)
        {
            txtFullNameVenue.BackColor = Color.LemonChiffon;
        }
        private void txtFullNameVenue_LostFocus(object sender, System.EventArgs e)
        {
            txtFullNameVenue.BackColor = Color.White;
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
        #endregion
    }
}
