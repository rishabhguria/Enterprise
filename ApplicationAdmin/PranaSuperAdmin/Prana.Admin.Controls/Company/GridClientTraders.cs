using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridClientTraders.
    /// </summary>
    public class GridClientTraders : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "GridClientTraders : ";
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.GroupBox groupBox1;

        private System.ComponentModel.Container components = null;
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
                if (btnCreate != null)
                {
                    btnCreate.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (createClientTrader != null)
                {
                    createClientTrader.Dispose();
                }
                if (createClientTraderEdit != null)
                {
                    createClientTraderEdit.Dispose();
                }
                if (grdClientTraders != null)
                {
                    grdClientTraders.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridClientTraders));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TraderID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FirstName", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastName", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 3);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Title", 4);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EMail", 5);
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneWork", 6);
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneCell", 7);
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Pager", 8);
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneHome", 9);
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 10);
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 11);
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdClientTraders = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClientTraders)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCreate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreate.BackgroundImage")));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnCreate.Location = new System.Drawing.Point(207, 198);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(363, 198);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(285, 198);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdClientTraders);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(620, 187);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Traders";
            // 
            // grdClientTraders
            // 
            this.grdClientTraders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 8;
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 100;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 100;
            appearance5.TextHAlignAsString = "Center";
            ultraGridColumn4.CellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            appearance6.TextHAlignAsString = "Center";
            ultraGridColumn4.Header.Appearance = appearance6;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 70;
            appearance7.TextHAlignAsString = "Center";
            ultraGridColumn5.CellAppearance = appearance7;
            appearance8.FontData.BoldAsString = "True";
            appearance8.TextHAlignAsString = "Center";
            ultraGridColumn5.Header.Appearance = appearance8;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 70;
            appearance9.TextHAlignAsString = "Center";
            ultraGridColumn6.CellAppearance = appearance9;
            appearance10.FontData.BoldAsString = "True";
            appearance10.TextHAlignAsString = "Center";
            ultraGridColumn6.Header.Appearance = appearance10;
            ultraGridColumn6.Header.Caption = "E-Mail";
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Width = 100;
            appearance11.TextHAlignAsString = "Center";
            ultraGridColumn7.CellAppearance = appearance11;
            appearance12.FontData.BoldAsString = "True";
            appearance12.TextHAlignAsString = "Center";
            ultraGridColumn7.Header.Appearance = appearance12;
            ultraGridColumn7.Header.Caption = "Work Telephone";
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Width = 100;
            appearance13.TextHAlignAsString = "Center";
            ultraGridColumn8.CellAppearance = appearance13;
            appearance14.FontData.BoldAsString = "True";
            appearance14.TextHAlignAsString = "Center";
            ultraGridColumn8.Header.Appearance = appearance14;
            ultraGridColumn8.Header.Caption = "Cell Telephone";
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Width = 100;
            appearance15.TextHAlignAsString = "Center";
            ultraGridColumn9.CellAppearance = appearance15;
            appearance16.FontData.BoldAsString = "True";
            appearance16.TextHAlignAsString = "Center";
            ultraGridColumn9.Header.Appearance = appearance16;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Width = 60;
            appearance17.TextHAlignAsString = "Center";
            ultraGridColumn10.CellAppearance = appearance17;
            appearance18.FontData.BoldAsString = "True";
            appearance18.TextHAlignAsString = "Center";
            ultraGridColumn10.Header.Appearance = appearance18;
            ultraGridColumn10.Header.Caption = "Home Telephone";
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn10.Width = 105;
            appearance19.TextHAlignAsString = "Center";
            ultraGridColumn11.CellAppearance = appearance19;
            appearance20.FontData.BoldAsString = "True";
            appearance20.TextHAlignAsString = "Center";
            ultraGridColumn11.Header.Appearance = appearance20;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn11.Width = 60;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn12.Width = 62;
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
            ultraGridColumn12});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdClientTraders.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdClientTraders.DisplayLayout.GroupByBox.Hidden = true;
            this.grdClientTraders.DisplayLayout.MaxColScrollRegions = 1;
            this.grdClientTraders.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdClientTraders.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdClientTraders.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdClientTraders.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdClientTraders.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdClientTraders.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdClientTraders.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdClientTraders.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdClientTraders.Location = new System.Drawing.Point(6, 18);
            this.grdClientTraders.Name = "grdClientTraders";
            this.grdClientTraders.Size = new System.Drawing.Size(606, 162);
            this.grdClientTraders.TabIndex = 73;
            this.grdClientTraders.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // GridClientTraders
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "GridClientTraders";
            this.Size = new System.Drawing.Size(644, 228);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClientTraders)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Private Members
        private CreateClientTrader createClientTrader = null;
        //		public  Traders CurrentTraders
        //		{
        //			get 
        //			{
        //				foreach(Prana.Admin.BLL.Trader trader in (Prana.Admin.BLL.Traders) grdClientTraders.DataSource)
        //				{
        //					if(trader.FirstName.ToString() == "")
        //					{
        //						_nullRow = true;
        ////						Prana.Admin.BLL.Traders nullTraders = new Traders();
        ////						grdClientTraders.DataSource = nullTraders;
        //						break;
        //					}
        //					else
        //					{
        //						break;
        //					}
        //				}
        //				Traders currentClientTraders = (Traders) grdClientTraders.DataSource;
        //				if(currentClientTraders.Count > 0)
        //				{				
        //					Traders saveClientTraders = new Traders();
        //					Traders clientTraders = (Prana.Admin.BLL.Traders) grdClientTraders.DataSource; 
        //					foreach(Trader clientTrader in clientTraders)
        //					{
        //						if(clientTrader.FirstName != "")
        //						{
        //							saveClientTraders.Add(clientTrader);
        //						}
        //					}
        //						
        //					return saveClientTraders;
        //					//return (Traders) grdClientTraders.DataSource; 
        //				}
        //				else
        //				{
        //					currentClientTraders = null;
        //					return currentClientTraders;
        //				}
        //			}						
        //		}

        private Traders _traders;
        private int _companyClientID = int.MinValue;
        private string _deletedIDS = String.Empty;
        public int CompanyClientID
        {
            get { return _companyClientID; }

        }

        private int _clientTraderID = int.MinValue;
        private bool _nullRow = false;
        Trader nullTrader;
        private CreateClientTrader createClientTraderEdit = null;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdClientTraders;
        #endregion

        #region Constructor And Setup
        public GridClientTraders()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }

        public void SetupControl(int companyClientID, Traders traders)
        {
            _traders = traders;
            _companyClientID = companyClientID;
            BindDataGrid();

        }

        #endregion

        #region Grid Binding
        private void BindDataGrid()
        {
            try
            {
                nullTrader = new Trader(int.MinValue, "");
                Traders traders = TraderManager.GetTraders(_companyClientID);
                foreach (Trader trader in traders)
                {
                    _traders.Add(trader);
                }
                if (_traders.Count <= 0)
                {
                    _traders.Add(nullTrader);
                    _nullRow = true;
                }
                else
                {
                    _nullRow = false;
                }
                grdClientTraders.DataSource = _traders;
                RefreshGrid();
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

        private void RefreshGrid()
        {
            if (grdClientTraders.Rows.Count > 0)
            {
                grdClientTraders.DisplayLayout.Bands[0].Columns["FirstName"].Header.VisiblePosition = 0;
                grdClientTraders.DisplayLayout.Bands[0].Columns["LastName"].Header.VisiblePosition = 1;
                grdClientTraders.DisplayLayout.Bands[0].Columns["ShortName"].Header.VisiblePosition = 2;
                grdClientTraders.DisplayLayout.Bands[0].Columns["Title"].Header.VisiblePosition = 3;
                grdClientTraders.DisplayLayout.Bands[0].Columns["EMail"].Header.VisiblePosition = 4;
                grdClientTraders.DisplayLayout.Bands[0].Columns["TelephoneWork"].Header.VisiblePosition = 5;
                grdClientTraders.DisplayLayout.Bands[0].Columns["TelephoneCell"].Header.VisiblePosition = 6;
                grdClientTraders.DisplayLayout.Bands[0].Columns["Pager"].Header.VisiblePosition = 7;
                grdClientTraders.DisplayLayout.Bands[0].Columns["TelephoneHome"].Header.VisiblePosition = 8;
                grdClientTraders.DisplayLayout.Bands[0].Columns["Fax"].Header.VisiblePosition = 9;

                grdClientTraders.DisplayLayout.Bands[0].Columns["TraderID"].Hidden = true;
                grdClientTraders.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;

                grdClientTraders.Rows[0].Selected = true;

                //			grdClientTradersAccounts.DisplayLayout.Bands[0].Columns["AccountName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
                //			grdClientTradersAccounts.DisplayLayout.Bands[0].Columns["AccountShortName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;

                //			grdClientTradersAccounts.DisplayLayout.Bands[0].Columns["AccountName"].Header.Caption = "Name";
                //			grdClientTradersAccounts.DisplayLayout.Bands[0].Columns["AccountShortName"].Header.Caption = "ShortName";
            }
        }

        #endregion

        #region Properties
        public int ClientTraderID
        {
            get { return _clientTraderID; }

        }

        public Trader TraderProperty
        {
            get
            {
                Trader trader = new Trader();
                GetTrader(trader);
                return trader;
            }

        }

        public void GetTrader(Trader trader)
        {
            trader = (Trader)grdClientTraders.DataSource;
        }

        public void SetTrader(Trader trader)
        {
            grdClientTraders.DataSource = trader;
        }

        public Traders Traders
        {
            get
            {
                if (_traders.Contains(nullTrader))
                {
                    _traders.Remove(nullTrader);
                }
                return _traders;
            }
        }
        public string DeletedTraders
        {
            get { return _deletedIDS; }
            set { _deletedIDS = value; }

        }
        #endregion

        #region Events
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (createClientTrader == null)
            {
                createClientTrader = new CreateClientTrader();
            }

            createClientTrader.Traders = _traders;


            createClientTrader.ShowDialog(this.Parent);

            if (_nullRow || _traders.Count > 1)
            {
                _traders.Remove(nullTrader);
                _nullRow = false;
            }
            grdClientTraders.Refresh();
            if (_traders.Count == 0)
            {
                //Trader nullTrader = new Trader(int.MinValue, "");
                _traders.Add(nullTrader);
                _nullRow = true;
            }
            grdClientTraders.DataSource = _traders;
            RefreshGrid();

        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                string shortName = "";
                if (grdClientTraders.ActiveRow == null)
                {
                    shortName = grdClientTraders.Rows[0].Cells["ShortName"].Text.ToString();

                }
                else
                {
                    shortName = grdClientTraders.ActiveRow.Cells["ShortName"].Text.ToString();
                }


                if (shortName != "")

                {
                    if (grdClientTraders.Selected.Rows.Count > 0)
                    {
                        if (MessageBox.Show(this, "Do you want to delete this Trader?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //shortName = grdClientTraders.ActiveRow.Cells["ShortName"].Text.ToString();

                            Trader trader = _traders.GetTrader(shortName);

                            if (trader.GetReference())
                            {
                                MessageBox.Show("Can't delete this Trader this is referenced in Trading Accounts");
                                return;
                            }
                            if (_deletedIDS == string.Empty)
                                _deletedIDS = trader.TraderID.ToString();
                            else
                                _deletedIDS = _deletedIDS + "," + trader.TraderID.ToString();

                            _traders.Remove(trader);

                            if (_traders.Count == 0)
                            {
                                _traders.Add(nullTrader);
                                _nullRow = true;
                            }

                            grdClientTraders.DataSource = _traders;
                            RefreshGrid();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (grdClientTraders.ActiveRow == null)
            {
                return;
            }
            if (createClientTraderEdit == null)
            {
                if ((grdClientTraders.Rows.Count > 0) && (_nullRow == false))
                {
                    createClientTraderEdit = new CreateClientTrader();
                    createClientTraderEdit.TraderEdit = (Trader)((Prana.Admin.BLL.Traders)grdClientTraders.DataSource)[grdClientTraders.ActiveRow.Index];

                    createClientTraderEdit.Traders = _traders;
                    createClientTraderEdit.ShowDialog(this.Parent);
                    createClientTraderEdit = null;
                }
            }
        }

        #endregion
    }
}
