using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for SymbolMapping.
    /// </summary>
    public class SymbolMapping : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "SymbolMapping : ";
        private System.Windows.Forms.Button btnGrdDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCreate;
        bool _isApplyTheme = false;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private Prana.Admin.BLL.SymbolMappings _symbolMappings = new SymbolMappings();
        public Prana.Admin.BLL.SymbolMappings CurrentSymbolMappings
        {
            get
            {
                foreach (Prana.Admin.BLL.SymbolMapping symbolMapping in (Prana.Admin.BLL.SymbolMappings)dtgrdSymbolMapping.DataSource)
                {
                    //if(symbolMapping.SymbolName.ToString() == "")
                    if (symbolMapping.Symbol.ToString() == "")
                    {
                        _nullRow = true;
                        Prana.Admin.BLL.SymbolMappings nullSymbolMappings = new SymbolMappings();
                        Prana.Admin.BLL.SymbolMapping nullSymbolMapping = new Prana.Admin.BLL.SymbolMapping(int.MinValue, "");
                        nullSymbolMappings.Add(nullSymbolMapping);
                        dtgrdSymbolMapping.DataSource = nullSymbolMappings;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return (Prana.Admin.BLL.SymbolMappings)dtgrdSymbolMapping.DataSource;
            }
        }

        private int _counterPartyVenueID = int.MinValue;
        private System.Windows.Forms.GroupBox grpSymbolMapping;
        private Infragistics.Win.UltraWinGrid.UltraGrid dtgrdSymbolMapping;

        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set
            {
                //_counterPartyVenueID = value;
                //BindDataGrid();
            }
        }

        private int _symbolMappingID = int.MinValue;
        public int SymbolMappingID
        {
            get { return _symbolMappingID; }
            set
            {
                //_symbolMappingID = value;
                //BindDataGrid();
            }
        }

        public void SetupControl(int counterPartyVenueID)
        {
            _counterPartyVenueID = counterPartyVenueID;
            BindDataGrid();
        }

        public SymbolMapping()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            //BindDataGrid();

        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                _isApplyTheme = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
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
                if (btnGrdDelete != null)
                {
                    btnGrdDelete.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (btnCreate != null)
                {
                    btnCreate.Dispose();
                }
                if (grpSymbolMapping != null)
                {
                    grpSymbolMapping.Dispose();
                }
                if (dtgrdSymbolMapping != null)
                {
                    dtgrdSymbolMapping.Dispose();
                }
                if (counterPartyVenueSymbolMapping != null)
                {
                    counterPartyVenueSymbolMapping.Dispose();
                }
                if (counterPartyVenueSymbolMappingEdit != null)
                {
                    counterPartyVenueSymbolMappingEdit.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolMapping));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVSymboMappingID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUEC", 3);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Symbol", 4);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MappedSymbol", 5);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECID", 6);
            this.grpSymbolMapping = new System.Windows.Forms.GroupBox();
            this.btnGrdDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.dtgrdSymbolMapping = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpSymbolMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdSymbolMapping)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSymbolMapping
            // 
            this.grpSymbolMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grpSymbolMapping.Controls.Add(this.btnGrdDelete);
            this.grpSymbolMapping.Controls.Add(this.btnEdit);
            this.grpSymbolMapping.Controls.Add(this.btnCreate);
            this.grpSymbolMapping.Controls.Add(this.dtgrdSymbolMapping);
            this.grpSymbolMapping.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpSymbolMapping.Location = new System.Drawing.Point(2, -3);
            this.grpSymbolMapping.Name = "grpSymbolMapping";
            this.grpSymbolMapping.Size = new System.Drawing.Size(360, 200);
            this.grpSymbolMapping.TabIndex = 1;
            this.grpSymbolMapping.TabStop = false;
            this.grpSymbolMapping.Text = "Symbol Mapping";
            this.grpSymbolMapping.Enter += new System.EventHandler(this.grpSymbolMapping_Enter);
            // 
            // btnGrdDelete
            // 
            this.btnGrdDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnGrdDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGrdDelete.BackgroundImage")));
            this.btnGrdDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGrdDelete.Location = new System.Drawing.Point(220, 172);
            this.btnGrdDelete.Name = "btnGrdDelete";
            this.btnGrdDelete.Size = new System.Drawing.Size(75, 23);
            this.btnGrdDelete.TabIndex = 8;
            this.btnGrdDelete.UseVisualStyleBackColor = false;
            this.btnGrdDelete.Click += new System.EventHandler(this.btnGrdDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Location = new System.Drawing.Point(142, 172);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCreate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreate.BackgroundImage")));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Location = new System.Drawing.Point(64, 172);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 6;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // dtgrdSymbolMapping
            // 
            this.dtgrdSymbolMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 233;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.Width = 233;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 233;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn4.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn4.Header.Appearance = appearance2;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 250;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn5.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn5.Header.Appearance = appearance4;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 130;
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.CellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.Header.Appearance = appearance6;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Width = 130;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn7.Width = 76;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.dtgrdSymbolMapping.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.dtgrdSymbolMapping.DisplayLayout.GroupByBox.Hidden = true;
            this.dtgrdSymbolMapping.DisplayLayout.MaxColScrollRegions = 1;
            this.dtgrdSymbolMapping.DisplayLayout.MaxRowScrollRegions = 1;
            this.dtgrdSymbolMapping.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.dtgrdSymbolMapping.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.dtgrdSymbolMapping.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.dtgrdSymbolMapping.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.dtgrdSymbolMapping.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.dtgrdSymbolMapping.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.dtgrdSymbolMapping.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtgrdSymbolMapping.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dtgrdSymbolMapping.Location = new System.Drawing.Point(3, 21);
            this.dtgrdSymbolMapping.Name = "dtgrdSymbolMapping";
            this.dtgrdSymbolMapping.Size = new System.Drawing.Size(348, 149);
            this.dtgrdSymbolMapping.TabIndex = 73;
            // 
            // SymbolMapping
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpSymbolMapping);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "SymbolMapping";
            this.Size = new System.Drawing.Size(366, 202);
            this.grpSymbolMapping.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdSymbolMapping)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion



        public SymbolMapping SymbolMappingProperty
        {
            get
            {
                SymbolMapping symbolMapping = new SymbolMapping();
                //SymbolMapping symbolMapping = new SymbolMapping();
                GetSymbolMapping(symbolMapping);
                return symbolMapping;
            }
            set
            {
                //SetSymbolMapping(value);

            }
        }

        private void BindDataGrid()
        {
            try
            {
                Prana.Admin.BLL.SymbolMapping nullSymbolMapping = new Prana.Admin.BLL.SymbolMapping(int.MinValue, "");
                Prana.Admin.BLL.SymbolMappings symbolMappings = CounterPartyManager.GetCVSymbolMapping(_counterPartyVenueID);
                if (symbolMappings.Count <= 0)
                {
                    symbolMappings.Add(nullSymbolMapping);
                    _nullRow = true;
                    dtgrdSymbolMapping.DataSource = symbolMappings;
                }
                else
                {
                    _nullRow = false;

                    Prana.Admin.BLL.SymbolMappings newSymbolMappings = new SymbolMappings();
                    foreach (Prana.Admin.BLL.SymbolMapping symbolMapping in symbolMappings)
                    {
                        int auecID = int.Parse(symbolMapping.AUECID.ToString());
                        AUEC auec = AUECManager.GetAUEC(auecID);
                        //SK 2061009 removed Compliance class
                        //Currency currency = new Currency();
                        //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                        //
                        //string auecName = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + auec.Currency.CurrencySymbol.ToString();
                        string auecName = auec.AUECString;
                        symbolMapping.AUEC = auecName;
                        newSymbolMappings.Add(symbolMapping);
                    }
                    dtgrdSymbolMapping.DataSource = newSymbolMappings;
                }

                ResetGrid();
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

        public void GetSymbolMapping(SymbolMapping symbolMapping)
        {
            symbolMapping = (SymbolMapping)dtgrdSymbolMapping.DataSource;
        }

        public void SetSymbolMapping(SymbolMapping symbolMapping)
        {
            dtgrdSymbolMapping.DataSource = symbolMapping;
        }

        //instance of CounterPartyVenueSymbolMapping class file.
        private CounterPartyVenueSymbolMapping counterPartyVenueSymbolMapping = null;
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (counterPartyVenueSymbolMapping == null)
            {
                counterPartyVenueSymbolMapping = new CounterPartyVenueSymbolMapping();
                // Applying theme for CH Release
                if (_isApplyTheme)
                {
                    counterPartyVenueSymbolMapping.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    counterPartyVenueSymbolMapping.ForeColor = System.Drawing.Color.White;
                }
            }

            SymbolMappings symbolMappings = new SymbolMappings();
            counterPartyVenueSymbolMapping.CVID = _counterPartyVenueID;
            counterPartyVenueSymbolMapping.CurrentSymbolMappings = (Prana.Admin.BLL.SymbolMappings)dtgrdSymbolMapping.DataSource;
            counterPartyVenueSymbolMapping.ResetErrorMsg();
            foreach (Prana.Admin.BLL.SymbolMapping symbolMapping in counterPartyVenueSymbolMapping.CurrentSymbolMappings)
            {
                //if(symbolMapping.SymbolName.ToString() == "")
                if (symbolMapping.Symbol.ToString() == "")
                {
                    counterPartyVenueSymbolMapping.NoData = 1;
                }
                else
                {
                    counterPartyVenueSymbolMapping.NoData = 0;
                    break;
                }
            }
            counterPartyVenueSymbolMapping.ShowDialog(this.Parent);
            symbolMappings = counterPartyVenueSymbolMapping.CurrentSymbolMappings;
            dtgrdSymbolMapping.DataSource = symbolMappings;
            if (symbolMappings.Count > 0)
            {
                //				dtgrdSymbolMapping.Select(0);
                _nullRow = false;
                ResetGrid();
            }
            else
            {
                BindDataGrid();
            }
        }

        private CounterPartyVenueSymbolMapping counterPartyVenueSymbolMappingEdit = null;
        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (counterPartyVenueSymbolMappingEdit == null)
            {
                if ((dtgrdSymbolMapping.Rows.Count > 0) && (_nullRow == false))
                {

                    counterPartyVenueSymbolMappingEdit = new CounterPartyVenueSymbolMapping();

                    // Applying theme for CH Release
                    if (_isApplyTheme)
                    {
                        counterPartyVenueSymbolMappingEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                        counterPartyVenueSymbolMappingEdit.ForeColor = System.Drawing.Color.White;
                    }
                    Prana.Admin.BLL.SymbolMapping symbolMappingEdit = new Prana.Admin.BLL.SymbolMapping();

                    counterPartyVenueSymbolMappingEdit.CVID = _counterPartyVenueID;
                    counterPartyVenueSymbolMappingEdit.SymbolMappingEdit = (Prana.Admin.BLL.SymbolMapping)((Prana.Admin.BLL.SymbolMappings)dtgrdSymbolMapping.DataSource)[dtgrdSymbolMapping.ActiveRow.Index];

                    counterPartyVenueSymbolMappingEdit.CurrentSymbolMappings = (Prana.Admin.BLL.SymbolMappings)dtgrdSymbolMapping.DataSource;
                    counterPartyVenueSymbolMappingEdit.ShowDialog(this.Parent);
                    counterPartyVenueSymbolMappingEdit = null;


                    ResetGrid();
                }
            }
        }

        private bool _nullRow = false;
        private void btnGrdDelete_Click(object sender, System.EventArgs e)
        {
            CounterPartyVenueSymbolMapping counterPartyVenueSymbolMapping = new CounterPartyVenueSymbolMapping();
            try
            {
                if (dtgrdSymbolMapping.Rows.Count > 0)
                {
                    string symbol = dtgrdSymbolMapping.ActiveRow.Cells["Symbol"].Text.ToString();
                    if (symbol != "")
                    {
                        if (MessageBox.Show(this, "Do you want to delete this Symbol Mapping?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Prana.Admin.BLL.SymbolMappings symbolMappings = (SymbolMappings)dtgrdSymbolMapping.DataSource;
                            Prana.Admin.BLL.SymbolMapping symbolMapping = new Prana.Admin.BLL.SymbolMapping();

                            symbolMapping.CVSymboMappingID = int.Parse(dtgrdSymbolMapping.ActiveRow.Cells["CVSymboMappingID"].Text.ToString());

                            symbolMapping.CVAUECID = int.Parse(dtgrdSymbolMapping.ActiveRow.Cells["CVAUECID"].Text.ToString());
                            symbolMapping.AUEC = dtgrdSymbolMapping.ActiveRow.Cells["AUEC"].Text.ToString();
                            symbolMapping.CounterPartyVenueID = int.Parse(dtgrdSymbolMapping.ActiveRow.Cells["CounterPartyVenueID"].Text.ToString());
                            symbolMapping.MappedSymbol = dtgrdSymbolMapping.ActiveRow.Cells["MappedSymbol"].Text.ToString();
                            symbolMapping.Symbol = dtgrdSymbolMapping.ActiveRow.Cells["Symbol"].Text.ToString();
                            symbolMapping.AUECID = int.Parse(dtgrdSymbolMapping.ActiveRow.Cells["AUECID"].Text.ToString());

                            int cvSymboMappingID = int.Parse(dtgrdSymbolMapping.ActiveRow.Cells["CVSymboMappingID"].Text.ToString());
                            if (cvSymboMappingID != int.MinValue)
                            {
                                CounterPartyManager.DeleteCVSymbolMappingByID(cvSymboMappingID);
                                symbolMappings.RemoveAt(symbolMappings.IndexOf(symbolMapping));
                                //BindDataGrid();
                            }
                            else
                            {

                                symbolMappings.RemoveAt(symbolMappings.IndexOf(symbolMapping));
                            }

                            SymbolMappings newSymbolMappings = new SymbolMappings();
                            foreach (Prana.Admin.BLL.SymbolMapping tempSymbolMapping in symbolMappings)
                            {
                                newSymbolMappings.Add(tempSymbolMapping);
                            }

                            if (symbolMappings.Count <= 0)
                            {
                                newSymbolMappings.Add(new Prana.Admin.BLL.SymbolMapping(int.MinValue, ""));
                                counterPartyVenueSymbolMapping.NoData = 1;
                                _nullRow = true;
                            }
                            else
                            {
                                counterPartyVenueSymbolMapping.NoData = 0;
                                _nullRow = false;
                            }
                            dtgrdSymbolMapping.DataSource = newSymbolMappings;

                            //BindDataGrid();

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
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnLogin_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnLogin_Click", null);
                #endregion
            }
        }

        private void ResetGrid()
        {
            ColumnsCollection columnsSymbolMappings = dtgrdSymbolMapping.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsSymbolMappings)
            {
                if (column.Key != "AUEC")
                {
                    if (column.Key != "Symbol")
                    {
                        if (column.Key != "MappedSymbol")
                        {
                            column.Hidden = true;
                        }
                    }
                }
            }
            dtgrdSymbolMapping.DisplayLayout.Bands[0].Columns["AUEC"].Width = 250;
            dtgrdSymbolMapping.DisplayLayout.Bands[0].Columns["Symbol"].Width = 130;
            dtgrdSymbolMapping.DisplayLayout.Bands[0].Columns["MappedSymbol"].Width = 130;

            dtgrdSymbolMapping.DisplayLayout.Bands[0].Columns["AUEC"].Header.VisiblePosition = 0;
            dtgrdSymbolMapping.DisplayLayout.Bands[0].Columns["Symbol"].Header.VisiblePosition = 1;
            dtgrdSymbolMapping.DisplayLayout.Bands[0].Columns["MappedSymbol"].Header.VisiblePosition = 2;
        }

        private void grpSymbolMapping_Enter(object sender, System.EventArgs e)
        {

        }
    }
}
