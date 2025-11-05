using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinToolTip;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    public partial class AlertGrid : UserControl
    {
        DataSet _alertHistory = new DataSet();
        ValueList _vlAlertPopResponse = new ValueList();
        public AlertGrid()
        {
            try
            {
                InitializeComponent();
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
        /// Initializes alert history.
        /// Clears grid and assign new data to grid.
        /// </summary>
        /// <param name="ruleHistDS"></param>
        internal void InitializeAlertHistory(DataSet ruleHistDS)
        {
            try
            {
                _alertHistory.Clear();

                _alertHistory = ruleHistDS;
                ultraAlertGrid.DataSource = _alertHistory;

                //Checking Condition rule contains in dataset
                if (_alertHistory.Tables[0].Columns.Contains("RuleDeleted"))
                {
                    //Iterating Loop for checking rule deleted field is true or false
                    foreach (DataRow row in _alertHistory.Tables[0].Rows)
                    {
                        //Rule deleted field is true then change the fore color of particular row
                        if (row["RuleDeleted"].Equals("True"))
                        {
                            ultraAlertGrid.DisplayLayout.Rows.GetRowWithListIndex(_alertHistory.Tables[0].Rows.IndexOf(row)).Appearance.ForeColor = Color.Red;
                        }
                    }
                }

                ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].Format = "MM/dd/yyyy HH:mm:ss";
                //ultraAlertGrid.DisplayLayout.Bands[0].Columns["Validation Time"].SortIndicator = SortIndicator.Descending;
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["RuleId"].Hidden = true;
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["RuleId"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                if (ultraAlertGrid.DisplayLayout.Bands[0].Columns.Exists("AlertPopUpResponse"))
                {
                    UltraGridColumn colAlertPopUpResponse = ultraAlertGrid.DisplayLayout.Bands[0].Columns["AlertPopUpResponse"];
                    colAlertPopUpResponse.Header.Caption = "Alert Button Clicked";
                    colAlertPopUpResponse.ValueList = _vlAlertPopResponse;
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
        /// Exports alerts in grid to the folder path selected by user.
        /// If alerts are filetered then filtered alerts are exported.
        /// </summary>
        /// <param name="folderPath"></param>
        internal void ExportAlerts(String folderPath)
        {
            try
            {
                ultraGridAlertExporter.Export(ultraAlertGrid, folderPath);
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
        /// Assigns Tooltip to the row.
        /// RuleType-RuleName as caption
        /// Summary and Current parameters in text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraAlertGrid_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            try
            {
                UltraToolTipInfo infoRow;
                RowUIElement currentRow;
                StringBuilder builder = new StringBuilder();
                if (e.Element.GetAncestor(typeof(RowUIElement)) != null)
                {
                    currentRow = (RowUIElement)e.Element.GetAncestor(typeof(RowUIElement)) as RowUIElement;
                    // UltraGridColumn col = this.ultraAlertGrid.DisplayLayout.Bands[0].Columns[""];
                    String title = currentRow.Row.Cells["Rule Type"].Value.ToString() + "-" + currentRow.Row.Cells["Name"].Value.ToString();
                    builder.Append("Rule Description- ");
                    builder.AppendLine(currentRow.Row.Cells["Summary"].Value.ToString());
                    builder.Append("Current Parameters- ");
                    builder.Append(currentRow.Row.Cells["Parameters"].Value.ToString());

                    if (currentRow != null)
                    {
                        infoRow = new UltraToolTipInfo(builder.ToString(), Infragistics.Win.ToolTipImage.Info, title, Infragistics.Win.DefaultableBoolean.True);
                        ultraToolTipManagerAlert.SetUltraToolTip(ultraAlertGrid, infoRow);
                        ultraToolTipManagerAlert.ShowToolTip(ultraAlertGrid);
                    }
                }
                else
                {
                    ultraToolTipManagerAlert.HideToolTip();
                    ultraToolTipManagerAlert.SetUltraToolTip(ultraAlertGrid, null);

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
        /// Adds new row to grid if current date is selected to grid
        /// </summary>
        /// <param name="dsReceived"></param>
        internal void UpdateAlerts(DataSet dsReceived)
        {
            try
            {
                String defaultFieldsValue = "N/A";
                foreach (DataRow dataRow in dsReceived.Tables[0].Rows)
                {
                    String compressionLevel = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("CompressionLevel"))
                        compressionLevel = dataRow["CompressionLevel"].ToString();

                    String alertComment = dataRow["Summary"].ToString();
                    String validationTime = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("ValidationTime"))
                        validationTime = dataRow["ValidationTime"].ToString();

                    String ruleType = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("RuleType"))
                        ruleType = dataRow["RuleType"].ToString();


                    String ruleName = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("name"))
                        ruleName = dataRow["name"].ToString();

                    String orderId = defaultFieldsValue;
                    if (ruleType.Equals(RulePackage.PreTrade.ToString()))
                    {
                        if (dsReceived.Tables[0].Columns.Contains("OrderId"))
                            orderId = dataRow["OrderId"].ToString();
                    }
                    String userName = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("UserId"))
                    {
                        int id = Convert.ToInt32(dataRow["UserId"].ToString());
                        if (id != 0)
                            userName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(id);
                        if (string.IsNullOrEmpty(userName))
                        {
                            userName = defaultFieldsValue;
                        }

                    }


                    String curParameter = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("Parameters"))
                        curParameter = dataRow["Parameters"].ToString();
                    String status = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("Status"))
                        status = dataRow["Status"].ToString();
                    String alertDescription = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("Description"))
                        alertDescription = dataRow["Description"].ToString();
                    String dimension = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("dimension"))
                        dimension = dataRow["dimension"].ToString();
                    String ruleId = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("RuleId"))
                        ruleId = dataRow["RuleId"].ToString();

                    String preTradeType = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("PreTradeType"))
                        preTradeType = dataRow["PreTradeType"].ToString();

                    String complianceOfficerNotes = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("ComplianceOfficerNotes"))
                        complianceOfficerNotes = dataRow["ComplianceOfficerNotes"].ToString();

                    String userNotes = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("UserNotes"))
                        userNotes = dataRow["UserNotes"].ToString();

                    String tradeDetails = defaultFieldsValue;
                    if (dsReceived.Tables[0].Columns.Contains("TradeDetails"))
                        tradeDetails = dataRow["TradeDetails"].ToString();

                    string alertPopUpResponse = "None";
                    if (dsReceived.Tables[0].Columns.Contains("AlertPopUpResponse"))
                        alertPopUpResponse = dataRow["AlertPopUpResponse"].ToString();
                    int alertPopUpResponseInt = (int)Enum.Parse(typeof(AlertPopUpResponse), alertPopUpResponse);

                    if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                          && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    {
                        curParameter = ComplainceConstants.CONST_CensorValue;
                        alertComment = ComplainceConstants.CONST_CensorValue;
                        alertDescription = ComplainceConstants.CONST_CensorValue;
                    }

                    if (_alertHistory != null && _alertHistory.Tables != null && _alertHistory.Tables.Count > 0)
                        _alertHistory.Tables[0].Rows.Add(new object[] { ruleName, userName, ruleType, alertComment, compressionLevel, curParameter, validationTime, orderId, status, alertDescription, dimension, ruleId, false, preTradeType, complianceOfficerNotes, userNotes, tradeDetails, alertPopUpResponseInt });

                    int length = this.ultraAlertGrid.Rows.All.Length;
                    if (length > 0)
                        ultraAlertGrid.Rows[length - 1].RefreshSortPosition();


                    // _alertHistory.Tables[0].Rows.Add(dr.ItemArray);
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
        /// Returns true if number of row in grid is greater than 0
        /// else reutrn false as there is no row to export.
        /// </summary>
        /// <returns></returns>
        internal bool CanExport()
        {
            try
            {
                if (ultraAlertGrid.Rows.Count > 0)
                    return true;
                else
                    return false;
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
                return false;
            }
        }

        /// <summary>
        /// Save the layout to xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                string gridLayoutFile = Application.StartupPath + "\\Prana Preferences\\" + Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\alertGridLayout.xml";
                ultraAlertGrid.DisplayLayout.SaveAsXml(gridLayoutFile);
                MessageBox.Show(this, "Layout Saved.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Select the row on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UltraAlertGrid_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                ultraAlertGrid.ActiveCell = null;
                if (e.Cell.StyleResolved == Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox)
                {
                    Boolean cellChecked = (Boolean)e.Cell.Row.GetCellValue("Checkbox");
                    if (cellChecked == true)
                        e.Cell.SetValue(false, true);
                    else
                        e.Cell.SetValue(true, true);
                }
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

        /// <summary>
        /// Highlight the row once selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UltraAlertGrid_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.StyleResolved == Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox)
                {
                    Boolean cellChecked = (Boolean)e.Cell.Row.GetCellValue("Checkbox");
                    if (cellChecked == true)
                        e.Cell.Row.Appearance.BackColor = System.Drawing.Color.Yellow;
                    else
                        e.Cell.Row.Appearance.BackColor = System.Drawing.Color.White;
                }
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

        /// <summary>
        /// Return the selected rows as name+parameter+date
        /// </summary>
        /// <returns></returns>
        public List<String> GetSelectedRows()
        {
            List<String> alertCandidateKeys = new List<String>();
            try
            {
                System.Collections.IEnumerable allRowsEnumerator = ultraAlertGrid.Rows.GetRowEnumerator(GridRowType.DataRow, null, null);
                foreach (UltraGridRow row in allRowsEnumerator)
                {
                    if ((Boolean)row.GetCellValue("Checkbox"))
                    {
                        String name = row.GetCellValue("Name").ToString();
                        String parameters = row.GetCellValue("Dimension").ToString();
                        DateTime time = Convert.ToDateTime(row.GetCellValue("Validation Time").ToString());
                        alertCandidateKeys.Add(name + parameters + time.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
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
            return alertCandidateKeys;
        }


        /// <summary>
        /// Rename the alerts in Alert History for given rule
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="newName"></param>
        /// <param name="newId"></param>
        internal void Rename(String oldId, String newName, String newId)
        {
            try
            {
                _alertHistory.Tables[0].Select(string.Format("[RuleId] = '{0}'", oldId)).ToList<DataRow>().ForEach(r => { r["Name"] = newName; r["RuleId"] = newId; });
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

        /// <summary>
        /// Change the color of Deleted rule Alerts to Red
        /// </summary>
        /// <param name="ruleId">Rule ID</param>
        internal void Delete(String ruleId)
        {
            try
            {
                DataRow[] rows = _alertHistory.Tables[0].Select(string.Format("[RuleId] = '{0}'", ruleId));
                foreach (DataRow row in rows)
                {
                    ultraAlertGrid.DisplayLayout.Rows.GetRowWithListIndex(_alertHistory.Tables[0].Rows.IndexOf(row)).Appearance.ForeColor = Color.Red;
                    row["RuleDeleted"] = "True";
                }
                //row.Delete();
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


        private void AlertGrid_Load(object sender, EventArgs e)
        {
            try
            {
                AlertGridDataBind();
                if (!CustomThemeHelper.ApplyTheme)
                {
                    SetAppearanceWithoutTheme();
                }
                IntializeValueListData();
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
        /// Bind the blank data source in the grid and then load the layout, add check box on the grid
        /// </summary>
        private void AlertGridDataBind()
        {
            try
            {
                _alertHistory.Tables.Add(new DataTable());
                _alertHistory.Tables[0].Columns.Add("Name", typeof(string));
                _alertHistory.Tables[0].Columns.Add("User Name", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Rule Type", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Summary", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Compression Level", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Parameters", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Validation Time", typeof(string));
                _alertHistory.Tables[0].Columns.Add("OrderId", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Status", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Description", typeof(string));
                _alertHistory.Tables[0].Columns.Add("Dimension", typeof(string));
                _alertHistory.Tables[0].Columns.Add("RuleId", typeof(string));
                _alertHistory.Tables[0].Columns.Add("RuleDeleted", typeof(string));
                _alertHistory.Tables[0].Columns.Add("PreTradeType", typeof(string));
                _alertHistory.Tables[0].Columns.Add("ComplianceOfficerNotes", typeof(string));
                _alertHistory.Tables[0].Columns.Add("UserNotes", typeof(string));
                _alertHistory.Tables[0].Columns.Add("TradeDetails", typeof(string));
                _alertHistory.Tables[0].Columns.Add("AlertPopUpResponse", typeof(string));

                ultraAlertGrid.DataSource = _alertHistory;

                ultraAlertGrid.DisplayLayout.Bands[0].Columns["TradeDetails"].Hidden = true;
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["TradeDetails"].Header.Caption = "Trade Details";
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["ComplianceOfficerNotes"].Header.Caption = "Compliance Officer Notes";
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["PreTradeType"].Header.Caption = "PreTrade Type";
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["RuleDeleted"].Header.Caption = "Rule Deleted";
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["UserNotes"].Header.Caption = "User Notes";
                ultraAlertGrid.DisplayLayout.Bands[0].Columns["AlertPopUpResponse"].Header.Caption = "Alert Button Clicked";
                string gridLayoutFile = Application.StartupPath + "\\Prana Preferences\\" + Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + "\\alertGridLayout.xml";
                if (File.Exists(gridLayoutFile))
                {
                    ultraAlertGrid.DisplayLayout.LoadFromXml(gridLayoutFile);
                }
                UltraWinGridUtils.AddCheckBox(ultraAlertGrid);
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

        private void SetAppearanceWithoutTheme()
        {
            try
            {
                Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
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
                Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();

                appearance1.BackColor = System.Drawing.Color.Gainsboro;
                appearance1.BackColor2 = System.Drawing.Color.DarkGray;
                appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
                this.ultraAlertGrid.DisplayLayout.Appearance = appearance1;

                appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
                appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
                appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                appearance2.BorderColor = System.Drawing.SystemColors.Window;
                this.ultraAlertGrid.DisplayLayout.GroupByBox.Appearance = appearance2;

                appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
                this.ultraAlertGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;

                appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
                appearance4.BackColor2 = System.Drawing.SystemColors.Control;
                appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
                appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
                this.ultraAlertGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;

                appearance5.BackColor = System.Drawing.SystemColors.Window;
                appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
                this.ultraAlertGrid.DisplayLayout.Override.ActiveCellAppearance = appearance5;

                appearance6.BackColor = System.Drawing.SystemColors.Highlight;
                appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
                this.ultraAlertGrid.DisplayLayout.Override.ActiveRowAppearance = appearance6;

                appearance7.BackColor = System.Drawing.Color.Transparent;
                this.ultraAlertGrid.DisplayLayout.Override.CardAreaAppearance = appearance7;
                appearance8.BorderColor = System.Drawing.Color.Silver;
                appearance8.FontData.Name = "Times New Roman";
                appearance8.FontData.SizeInPoints = 10F;

                appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
                this.ultraAlertGrid.DisplayLayout.Override.CellAppearance = appearance8;

                appearance9.BackColor = System.Drawing.Color.Silver;
                this.ultraAlertGrid.DisplayLayout.Override.GroupByColumnHeaderAppearance = appearance9;

                appearance10.BackColor = System.Drawing.SystemColors.Control;
                appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
                appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
                appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
                appearance10.BorderColor = System.Drawing.SystemColors.Window;
                this.ultraAlertGrid.DisplayLayout.Override.GroupByRowAppearance = appearance10;

                appearance11.BackColor = System.Drawing.Color.DarkGray;
                appearance11.BackColor2 = System.Drawing.Color.Gainsboro;
                appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
                appearance11.FontData.Name = "Times New Roman";
                appearance11.FontData.SizeInPoints = 10F;
                appearance11.TextHAlignAsString = "Left";
                appearance11.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
                this.ultraAlertGrid.DisplayLayout.Override.HeaderAppearance = appearance11;

                appearance12.BackColor = System.Drawing.SystemColors.Window;
                appearance12.BorderColor = System.Drawing.Color.Silver;
                this.ultraAlertGrid.DisplayLayout.Override.RowAppearance = appearance12;

                appearance13.BackColor = System.Drawing.Color.DarkGray;
                appearance13.BackColor2 = System.Drawing.Color.Gainsboro;
                appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
                this.ultraAlertGrid.DisplayLayout.Override.RowSelectorAppearance = appearance13;

                appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
                this.ultraAlertGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
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

        public void ultraAlertGrid_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.ultraAlertGrid);
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

        public void ultraAlertGrid_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary>
        /// Getting Sorted Grid Column
        /// </summary>
        /// <returns></returns>
        internal string SortedColumn()
        {
            try
            {
                SortedColumnsCollection sortedColumns = new SortedColumnsCollection(ultraAlertGrid.DisplayLayout.Bands[0]);
                string sortedColumnName = String.Empty;
                foreach (UltraGridColumn column in ultraAlertGrid.DisplayLayout.Bands[0].Columns)
                {
                    if (ultraAlertGrid.DisplayLayout.Bands[0].HasSortedColumns)
                    {
                        sortedColumns = ultraAlertGrid.DisplayLayout.Bands[0].SortedColumns;
                        if (sortedColumns.Contains(column))
                        {
                            if (sortedColumns[0].SortIndicator.Equals(SortIndicator.Descending))
                                sortedColumnName = "[" + column.Key.ToString() + "] DESC";
                            else
                                sortedColumnName = "[" + column.Key.ToString() + "] ASC";
                            break;
                        }
                    }
                }

                if (String.IsNullOrEmpty(sortedColumnName))
                    sortedColumnName = "[Validation Time] ASC";

                return sortedColumnName;
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
            return String.Empty;
        }

        /// <summary>
        /// Getting Sorted Grid Column
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> FilteredColumn()
        {
            try
            {
                Dictionary<string, string> filteredColumn = new Dictionary<string, string>();
                foreach (UltraGridColumn column in ultraAlertGrid.DisplayLayout.Bands[0].Columns)
                {
                    var res = ultraAlertGrid.DisplayLayout.Bands[0].ColumnFilters[column].FilterConditions;
                    if (res.Count > 0)
                    {
                        if (!filteredColumn.ContainsKey(column.Key) && !String.IsNullOrEmpty(res[0].CompareValue.ToString()))
                            filteredColumn.Add(res[0].Column.ToString(), res[0].CompareValue.ToString());
                    }
                }
                return filteredColumn;
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
            return null;
        }

        /// <summary>
        /// Intialize data in the Value List
        /// </summary>
        private void IntializeValueListData()
        {
            try
            {
                _vlAlertPopResponse.ValueListItems.Clear();
                foreach (int key in Enum.GetValues(typeof(AlertPopUpResponse)))
                {
                    string alertPopUpResponseValue = Enum.GetName(typeof(AlertPopUpResponse), key);
                    _vlAlertPopResponse.ValueListItems.Add(key, alertPopUpResponseValue);
                }
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

        public void ExportData(string gridName, string filePath)
        {
            try
            {
                string folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                if (gridName == "ultraAlertGrid")
                {
                    exporter.Export(ultraAlertGrid, filePath);
                }
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

