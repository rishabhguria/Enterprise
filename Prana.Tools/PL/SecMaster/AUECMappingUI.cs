using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class AUECMappingUI : Form
    {
        ISecurityMasterServices _securityMaster = null;

        public AUECMappingUI()
        {
            try
            {
                InitializeComponent();
                ultraTextEditorHint.Text = "MarketData Symbol Format Keywords: {Root} {1D-YearCode} {2D-YearCode} {1D-MonthCode} {2D-MonthCode} {Day} {YYMMDD-ExpirationDate} {StrikePrice} {6D-StrikePrice} {8D-StrikePrice} {11D-StrikePrice} {OptionType} {OCC-Symbology} {FactSetExchangeCode} {FactSetRegionCode}";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="securityMaster"></param>
        public void SetUp(ISecurityMasterServices securityMaster)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);

                _securityMaster = securityMaster;
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    _securityMaster.AUECMappingGetDataResponse += new EventHandler<EventArgs<DataSet>>(_securityMaster_AUECMappingGetDataResponse);
                    ultraButtonRefresh.Enabled = false;
                    ulblSave.ForeColor = System.Drawing.Color.White;
                    ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Getting Data";

                    _securityMaster.GetAUECMappings();
                }
                else
                {
                    ulblSave.ForeColor = System.Drawing.Color.White;
                    ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] TradeService not connected";
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void _securityMaster_AUECMappingGetDataResponse(object sender, EventArgs<DataSet> e)
        {
            try
            {
                DataSet dsAUECMapping = e.Value;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker del =
                         delegate
                         {
                             _securityMaster_AUECMappingGetDataResponse(sender, new EventArgs<DataSet>(dsAUECMapping));
                         };
                        this.BeginInvoke(del);
                    }
                    else
                    {
                        ultraButtonRefresh.Enabled = true;
                        ulblSave.ForeColor = System.Drawing.Color.White;
                        ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Loaded";

                        ultraGrid.DataSource = null;
                        ultraGrid.DataSource = dsAUECMapping;
                        ultraGrid.Refresh();
                        SetGridCustomisation();
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Customization of the ultragrid
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void SetGridCustomisation()
        {
            try
            {
                foreach (UltraGridBand band in ultraGrid.DisplayLayout.Bands)
                {
                    UltraGridGroup ulGroup;
                    UltraGridGroup ulGroupForOptionSymbol;
                    UltraGridGroup ulGroupForPSSymbol;
                    UltraGridGroup ulGroupForMarketDataSymbolMapping;
                    UltraGridGroup ulNoGroup;

                    if (!band.Groups.Exists(""))
                        ulGroup = band.Groups.Add("");
                    else
                        ulGroup = band.Groups[""];

                    if (!band.Groups.Exists("NoGroup"))
                        ulNoGroup = band.Groups.Add("NoGroup");
                    else
                        ulNoGroup = band.Groups["NoGroup"];

                    if (!ultraGrid.DisplayLayout.Bands[0].Groups.Exists("OptionSymbol"))
                        ulGroupForOptionSymbol = ultraGrid.DisplayLayout.Bands[0].Groups.Add("OptionSymbol");
                    else
                        ulGroupForOptionSymbol = ultraGrid.DisplayLayout.Bands[0].Groups["OptionSymbol"];

                    if (!band.Groups.Exists("PSSymbol"))
                        ulGroupForPSSymbol = band.Groups.Add("PSSymbol");
                    else
                        ulGroupForPSSymbol = band.Groups["PSSymbol"];

                    if (!band.Groups.Exists("MarketDataSymbolMapping"))
                        ulGroupForMarketDataSymbolMapping = band.Groups.Add("MarketDataSymbolMapping");
                    else
                        ulGroupForMarketDataSymbolMapping = band.Groups["MarketDataSymbolMapping"];

                    //AUECID & ExchangeIdentifier Editable false (Unique Fields depends on AUEC details)
                    band.Columns["AUECID"].CellActivation = Activation.NoEdit;
                    band.Columns["ExchangeIdentifier"].CellActivation = Activation.NoEdit;

                    ulNoGroup.Header.Caption = string.Empty;
                    ulGroupForOptionSymbol.Header.Caption = "Option Symbol Mapping";
                    ulGroupForPSSymbol.Header.Caption = "PS Symbol Mapping";
                    ulGroupForMarketDataSymbolMapping.Header.Caption = "Market Data Symbol Mapping";

                    //Groupings
                    band.Columns["AUECID"].Group = ulGroup;
                    band.Columns["ExchangeIdentifier"].Group = ulGroup;
                    band.Columns["ExchangeToken"].Group = ulGroup;

                    band.Columns["EsignalFormatString"].Group = ulNoGroup;

                    band.Columns["EsignalOptionFormatString"].Group = ulGroupForOptionSymbol;
                    band.Columns["BloombergOptionFormatString"].Group = ulGroupForOptionSymbol;
                    band.Columns["EsignalRootToken"].Group = ulGroupForOptionSymbol;
                    band.Columns["BloombergRootToken"].Group = ulGroupForOptionSymbol;

                    band.Columns["PSFormatString"].Group = ulGroupForPSSymbol;
                    band.Columns["PSRootToken"].Group = ulGroupForPSSymbol;
                    band.Columns["Year"].Group = ulGroupForPSSymbol;
                    band.Columns["Month"].Group = ulGroupForPSSymbol;
                    band.Columns["Day"].Group = ulGroupForPSSymbol;
                    band.Columns["Type"].Group = ulGroupForPSSymbol;
                    band.Columns["Strike"].Group = ulGroupForPSSymbol;
                    band.Columns["TranslateRoot"].Group = ulGroupForPSSymbol;
                    band.Columns["TranslateType"].Group = ulGroupForPSSymbol;
                    band.Columns["ExerciseStyle"].Group = ulGroupForPSSymbol;

                    band.Columns["EsignalExchangeCode"].Group = ulGroupForMarketDataSymbolMapping;
                    band.Columns["FactSetExchangeCode"].Group = ulGroupForMarketDataSymbolMapping;
                    band.Columns["FactSetRegionCode"].Group = ulGroupForMarketDataSymbolMapping;
                    band.Columns["FactSetFormatString"].Group = ulGroupForMarketDataSymbolMapping;
                    band.Columns["ActivFormatString"].Group = ulGroupForMarketDataSymbolMapping;
                    band.Columns[BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE].Group = ulGroupForMarketDataSymbolMapping;
                    band.Columns[BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE].Group = ulGroupForMarketDataSymbolMapping;
                    band.Columns[BloombergSapiConstants.CONST_BLOOMBERG_FORMAT_STRING].Group = ulGroupForMarketDataSymbolMapping;

                    ulGroup.Header.Fixed = true;
                    ulGroup.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ulNoGroup.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ulGroupForPSSymbol.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ulGroupForOptionSymbol.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ulGroupForMarketDataSymbolMapping.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                }

                //For auto sizing column width with respect to column data
                foreach (UltraGridColumn column in ultraGrid.DisplayLayout.Bands[0].Columns)
                {
                    column.PerformAutoResize();
                }

                foreach (UltraGridRow row in ultraGrid.DisplayLayout.Bands[0].GetRowEnumerator(GridRowType.DataRow))
                {
                    if (((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.EquityOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FutureOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FXOption || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.ConvertibleBond || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.CreditDefaultSwap || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FixedIncome || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.FXForward || ((AssetCategory)(row.Cells["AssetID"].Value)) == AssetCategory.PrivateEquity)
                    {
                        row.Cells["EsignalOptionFormatString"].Activation = Activation.NoEdit;
                        row.Cells["BloombergOptionFormatString"].Activation = Activation.NoEdit;
                        row.Cells["EsignalRootToken"].Activation = Activation.NoEdit;
                        row.Cells["BloombergRootToken"].Activation = Activation.NoEdit;
                    }
                }

                ultraGrid.DisplayLayout.Bands[0].Groups["OptionSymbol"].Hidden = !ultraCheckEditorOptionMapping.Checked;
                ultraGrid.DisplayLayout.Bands[0].Groups["PSSymbol"].Hidden = !ultraCheckEditorPSMapping.Checked;
                ultraGrid.DisplayLayout.Bands[0].Groups["MarketDataSymbolMapping"].Hidden = !ultraCheckEditorMarketDataSymbolMapping.Checked;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Function to save data on button click to database table
        private void ubtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ulblSave.Text = String.Empty;
                DataSet saveDataSet = ultraGrid.DataSource as DataSet;
                DataTable saveTable = saveDataSet.Tables[0];
                if (saveTable != null)
                {
                    DataTable dtSaveTableTemp = new DataTable();
                    #region DataTable Columns
                    dtSaveTableTemp.Columns.Add(new DataColumn("AUECID"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("AssetID"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("ExchangeIdentifier"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("Year"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("Month"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("Day"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("Type"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("Strike"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("ExchangeToken"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("PSRootToken"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("PSFormatString"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("TranslateRoot"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("TranslateType"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("ExerciseStyle"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("EsignalFormatString"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("EsignalOptionFormatString"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("BloombergOptionFormatString"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("EsignalRootToken"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("BloombergRootToken"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("EsignalExchangeCode"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("FactSetExchangeCode"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("FactSetRegionCode"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("FactSetFormatString"));
                    dtSaveTableTemp.Columns.Add(new DataColumn("ActivFormatString"));
                    dtSaveTableTemp.Columns.Add(new DataColumn(BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE));
                    dtSaveTableTemp.Columns.Add(new DataColumn(BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE));
                    dtSaveTableTemp.Columns.Add(new DataColumn(BloombergSapiConstants.CONST_BLOOMBERG_FORMAT_STRING));
                    #endregion

                    foreach (DataRow dRow in saveTable.Rows)
                    {
                        DataRow dr = dtSaveTableTemp.NewRow();
                        if (dRow.RowState == DataRowState.Modified)
                        {
                            dr["AUECID"] = dRow["AUECID"].ToString().Trim();
                            dr["AssetID"] = dRow["AssetID"].ToString().Trim();
                            dr["ExchangeIdentifier"] = dRow["ExchangeIdentifier"].ToString().Trim();
                            dr["Year"] = dRow["Year"].ToString().Trim();
                            dr["Month"] = dRow["Month"].ToString().Trim();
                            dr["Day"] = dRow["Day"].ToString().Trim();
                            dr["Type"] = dRow["Type"].ToString().Trim();
                            dr["Strike"] = dRow["Strike"].ToString().Trim();
                            dr["ExchangeToken"] = dRow["ExchangeToken"].ToString().Trim();
                            dr["PSRootToken"] = dRow["PSRootToken"].ToString().Trim();
                            dr["PSFormatString"] = dRow["PSFormatString"].ToString().Trim();
                            dr["TranslateRoot"] = dRow["TranslateRoot"].ToString().Trim();
                            dr["TranslateType"] = dRow["TranslateType"].ToString().Trim();
                            dr["ExerciseStyle"] = dRow["ExerciseStyle"].ToString().Trim();
                            dr["EsignalFormatString"] = dRow["EsignalFormatString"].ToString().Trim();
                            dr["EsignalOptionFormatString"] = dRow["EsignalOptionFormatString"].ToString().Trim();
                            dr["BloombergOptionFormatString"] = dRow["BloombergOptionFormatString"].ToString().Trim();
                            dr["EsignalRootToken"] = dRow["EsignalRootToken"].ToString().Trim();
                            dr["BloombergRootToken"] = dRow["BloombergRootToken"].ToString().Trim();
                            dr["EsignalExchangeCode"] = dRow["EsignalExchangeCode"].ToString().Trim();
                            dr["FactSetExchangeCode"] = dRow["FactSetExchangeCode"].ToString().Trim();
                            dr["FactSetRegionCode"] = dRow["FactSetRegionCode"].ToString().Trim();
                            dr["FactSetFormatString"] = dRow["FactSetFormatString"].ToString().Trim();
                            dr["ActivFormatString"] = dRow["ActivFormatString"].ToString().Trim();
                            dr[BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE] = dRow[BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE].ToString().Trim();
                            dr[BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE] = dRow[BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE].ToString().Trim();
                            dr[BloombergSapiConstants.CONST_BLOOMBERG_FORMAT_STRING] = dRow[BloombergSapiConstants.CONST_BLOOMBERG_FORMAT_STRING].ToString().Trim();
                            dtSaveTableTemp.Rows.Add(dr);
                            dtSaveTableTemp.AcceptChanges();
                        }
                    }

                    DataSet saveDataSetTemp = new DataSet();
                    saveDataSetTemp.Tables.Add(dtSaveTableTemp.Copy());
                    if (saveDataSetTemp != null && saveDataSetTemp.Tables[0].Rows.Count > 0)
                    {
                        if (_securityMaster != null && _securityMaster.IsConnected)
                        {
                            _securityMaster.SaveAUECMappings(saveDataSetTemp);

                            //Update Option Symbol Cache
                            OptionSymbolGenerator.UpdateDictOptionSymbolMapper(saveDataSetTemp);

                            ulblSave.ForeColor = System.Drawing.Color.White;
                            ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Updated";
                            saveDataSet.AcceptChanges();
                        }
                        else
                        {
                            ulblSave.ForeColor = System.Drawing.Color.White;
                            ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] TradeService not connected";
                        }
                    }
                    else
                    {
                        ulblSave.ForeColor = System.Drawing.Color.White;
                        ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Nothing to Save";
                    }
                }
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

        private void ultraButtonRefresh_Click(object sender, EventArgs e)
        {
            if (_securityMaster != null && _securityMaster.IsConnected)
            {
                ultraButtonRefresh.Enabled = false;
                ulblSave.ForeColor = System.Drawing.Color.White;
                ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Getting Data";

                _securityMaster.GetAUECMappings();
            }
            else
            {
                ulblSave.ForeColor = System.Drawing.Color.White;
                ulblSave.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] TradeService not connected";
            }
        }

        private void AUECMappingUI_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
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

        private void SetButtonsColor()
        {
            try
            {
                ubtnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                ubtnSave.ForeColor = System.Drawing.Color.White;
                ubtnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubtnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubtnSave.UseAppStyling = false;
                ubtnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraCheckEditorOptionMapping_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraGrid.DisplayLayout.Bands[0].Groups["OptionSymbol"].Hidden = !ultraCheckEditorOptionMapping.Checked;
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

        private void ultraCheckEditorPSMapping_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraGrid.DisplayLayout.Bands[0].Groups["PSSymbol"].Hidden = !ultraCheckEditorPSMapping.Checked;
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

        private void ultraCheckEditorFactSetMapping_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ultraGrid.DisplayLayout.Bands[0].Groups["MarketDataSymbolMapping"].Hidden = !ultraCheckEditorMarketDataSymbolMapping.Checked;
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

        private void AUECMappingUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_securityMaster != null)
                {
                    _securityMaster.AUECMappingGetDataResponse -= new EventHandler<EventArgs<DataSet>>(_securityMaster_AUECMappingGetDataResponse);
                }
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
        /// used to Export Data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        public void ExportGridData(string exportFilePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(exportFilePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                exporter.Export(ultraGrid, exportFilePath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}