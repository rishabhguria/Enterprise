using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlExceptionReport : UserControl
    {
        Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("-Select-", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
        Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo2 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("-Select-", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);

        //SerializableDictionary<string, ReconTemplate> _dictReconTemplates;
        /// <summary>
        /// /// added by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project
        /// </summary>
        public ctrlExceptionReport()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btView.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btView.ForeColor = System.Drawing.Color.White;
                btView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btView.UseAppStyling = false;
                btView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Initialize data sources
        /// </summary>
        public void InitializeDataSources()
        {
            try
            {
                // _dictReconTemplates = ReconPrefManager.ReconPreferences.DictReconTemplates;


                //to restore the default value back to comboclien if not changed
                object clientID = cmbClient.Value;
                object reconTypeID = cmbReconType.Value;
                object templateID = cmbTemplate.Value;
                DateTime fromDate = dtFromDate.DateTime;
                DateTime toDate = dtToDate.DateTime;
                object formatName = cmbFormatName.Value;
                object reconDateType = cmbRunByDate.Value;
                DateTime runDate = dtRunDate.DateTime;

                BindClientCombo();
                //CHMW-2225 Rows on recon dashboard should be unique by the following characteristics: Format Name, Recon Type, Run Date, From Date, End Date, Date Type (trade date, process date, etc) 
                ReconUIUtilities.BindThirdPartyCombo(cmbFormatName, string.Empty, ultraToolTipInfo2, ultraToolTipManager1);
                ReconUIUtilities.BindReconDateTypeCombo(cmbRunByDate, ReconDateType.TradeDate);
                if (clientID != null)
                {
                    cmbClient.Value = clientID;
                    cmbReconType.Value = reconTypeID;
                    cmbTemplate.Value = templateID;
                    dtFromDate.DateTime = fromDate;
                    dtToDate.DateTime = toDate;
                    cmbFormatName.Value = formatName;
                    cmbRunByDate.Value = reconDateType;
                    dtRunDate.DateTime = runDate;
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
        /// sets cmbrecontype and cmbtemplate value on client change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbClient_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbClient.Text.Equals(ApplicationConstants.C_COMBO_SELECT) || string.IsNullOrWhiteSpace(cmbClient.Text))
                {
                    cmbReconType.DataSource = null;
                    cmbTemplate.DataSource = null;
                }
                else
                {
                    //set recontype and template to -select- if client is -select-
                    cmbReconType.Value = -1;
                    cmbTemplate.DataSource = null;
                    BindReconTypeCombo();
                    ultraToolTipInfo1.ToolTipText = cmbClient.Text;
                    ultraToolTipManager1.SetUltraToolTip(cmbClient, ultraToolTipInfo1);
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
        /// sets cmbtemplate value on cmbrecontype change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbReconType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbReconType.Text.Equals(ApplicationConstants.C_COMBO_SELECT) || string.IsNullOrWhiteSpace(cmbReconType.Text))
                {
                    cmbTemplate.DataSource = null;
                }
                if ((!cmbClient.Text.Equals(ApplicationConstants.C_COMBO_SELECT)) && (!cmbReconType.Text.Equals(ApplicationConstants.C_COMBO_SELECT)) && !string.IsNullOrWhiteSpace(cmbReconType.Text))
                {
                    string reconTypeKey = cmbClient.Value.ToString() + Seperators.SEPERATOR_6 + cmbReconType.Text;
                    //modified by amit on 21/04/2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3454
                    ReconUIUtilities.BindTemplateCombo(reconTypeKey, cmbTemplate);
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
        /// bind data on client combobox on startup
        /// </summary>
        private void BindClientCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));

                // To bind all permitted clients for selected user
                Dictionary<int, string> dictClients = new Dictionary<int, string>();
                foreach (KeyValuePair<int, List<int>> clients in CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts())
                {
                    if (CachedDataManager.GetUserPermittedCompanyList().ContainsKey(clients.Key))
                    {
                        if (!dictClients.ContainsKey(clients.Key))
                        {
                            dictClients.Add(clients.Key, CachedDataManager.GetUserPermittedCompanyList()[clients.Key]);
                        }
                    }
                }

                foreach (KeyValuePair<int, string> kvp in dictClients)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                listValues = listValues.OrderBy(e => e.DisplayText).ToList();
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                // cmbClient = new UltraCombo();
                cmbClient.DataSource = null;
                cmbClient.DataSource = listValues;
                cmbClient.DisplayMember = "DisplayText";
                cmbClient.ValueMember = "Value";
                cmbClient.DataBind();
                cmbClient.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbClient.Value = -1;
                cmbClient.DisplayLayout.Bands[0].ColHeadersVisible = false;

                //set tool tip for every row   
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1229            
                foreach (UltraGridRow row in cmbClient.Rows)
                {
                    if (row.Cells["DisplayText"].Text.Equals(ApplicationConstants.C_COMBO_SELECT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    row.ToolTipText = row.Cells["DisplayText"].Text;
                }
                if (string.IsNullOrEmpty(cmbClient.Text) || cmbClient.Text.Equals(ApplicationConstants.C_COMBO_SELECT, StringComparison.InvariantCultureIgnoreCase))
                {
                    ultraToolTipInfo1.ToolTipText = "No client selected";
                }
                //ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Standard;
                ultraToolTipManager1.SetUltraToolTip(cmbClient, ultraToolTipInfo1);
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
        ///  bind data on Template combobox on recontype value change
        ///  /// </summary>       
        //private void BindTemplateCombo(string reconTypeKey)
        //{
        //    try
        //    {
        //        //modified by amit on 21/04/2015
        //        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3454
        //        ReconUtilities.BindTemplateCombo(reconTypeKey, cmbTemplate);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
        //}
        /// <summary>
        /// bind data on ReconType combobox on client value change
        /// </summary>
        private void BindReconTypeCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));

                int i = 0;
                foreach (string reconTemplate in ReconPrefManager.ReconPreferences.getRootTemplates())
                {
                    EnumerationValue value = new EnumerationValue(reconTemplate, i);
                    listValues.Add(value);
                    i++;
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));

                cmbReconType.DataSource = null;
                cmbReconType.DataSource = listValues;
                cmbReconType.DisplayMember = "DisplayText";
                cmbReconType.ValueMember = "Value";
                cmbReconType.DataBind();
                cmbReconType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbReconType.DisplayLayout.Bands[0].Header.Enabled = false;
                cmbReconType.Value = -1;
                cmbReconType.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// loads xml file from the application startup path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btView_Click(object sender, EventArgs e)
        {
            try
            {
                //check if Client or ReconType or Template combo Box is not selected
                if (cmbClient.Text == ApplicationConstants.C_COMBO_SELECT || string.IsNullOrEmpty(cmbClient.Text) || cmbReconType.Text == ApplicationConstants.C_COMBO_SELECT || cmbTemplate.Text == ApplicationConstants.C_COMBO_SELECT)
                {
                    MessageBox.Show("Please Select Exception Report details", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ReconParameters reconParameters = new ReconParameters();
                reconParameters.ClientID = cmbClient.Value.ToString();
                reconParameters.ReconType = cmbReconType.Text;
                reconParameters.TemplateName = cmbTemplate.Text;
                reconParameters.DTFromDate = (DateTime)dtFromDate.Value;
                reconParameters.DTToDate = (DateTime)dtToDate.Value;
                reconParameters.FormatName = cmbFormatName.Text;
                reconParameters.ReconDateType = (ReconDateType)Enum.Parse(typeof(ReconDateType), cmbRunByDate.Value.ToString());
                reconParameters.DTRunDate = (DateTime)dtRunDate.Value;

                ////CHMW-2225 Rows on recon dashboard should be unique by the following characteristics: Format Name, Recon Type, Run Date, From Date, End Date, Date Type (trade date, process date, etc) 
                reconParameters.PBFilePath = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters) + ".xml";
                //if (!checkFile_LongPath(filePath, Path.GetDirectoryName(filePath).Length))
                if (!File.Exists(reconParameters.PBFilePath))
                {
                    MessageBox.Show("File not Available to View the Report", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataSet ds = new DataSet();
                //CHMW-2181	[Reconciliation] [Code Review] Replace dataset read xml with BufferedStream
                //ds.ReadXml(filePath);
                ds = XMLUtilities.ReadXmlUsingBufferedStream(reconParameters.PBFilePath);

                //checks if table exist in dataset
                if (ds.Tables.Count == 0)
                {
                    grdReport.DataSource = null;
                    return;
                }

                txtComment.Text = string.Empty;
                string dashboardFilePath = ReconUtilities.GetDashboardFilePath(cmbClient.Value.ToString(), cmbReconType.Text, cmbTemplate.Text,
                    ((DateTime)dtFromDate.Value).ToString(ApplicationConstants.DateFormat), ((DateTime)dtToDate.Value).ToString(ApplicationConstants.DateFormat),
                    cmbFormatName.Text, cmbRunByDate.Value.ToString(), dtRunDate.DateTime) + ".xml";
                if (File.Exists(dashboardFilePath))
                {
                    DataSet dsDashboard = new DataSet();
                    dsDashboard.ReadXml(dashboardFilePath);
                    if (dsDashboard.Tables != null && dsDashboard.Tables.Contains("Statistics") && dsDashboard.Tables["Statistics"].Columns.Contains("Comments"))
                    {
                        txtComment.Text = dsDashboard.Tables["Statistics"].Rows[0]["Comments"].ToString();
                    }
                }
                grdReport.DataSource = ds.Tables[0];

                //set header caption of ultragrid
                SetColumnsCaptionForUltraGrid(grdReport.DisplayLayout.Bands[0]);

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
        /// opens file save dialog to save the grid table in pdf csv or xls format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Modified by Pranay Deep
                //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-10512
                //Date: 24-08-2015
                contextMenuStrip1.Hide();


                //Modified by faisal shah
                //Todo Handle Comments field in Grouped and non grouped column
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3641
                bool isGroupingApplied = false;
                if (grdReport.DataSource != null)
                {
                    foreach (UltraGridColumn column in grdReport.DisplayLayout.Bands[0].Columns)
                    {
                        if (column.IsGroupByColumn)
                        {
                            isGroupingApplied = true;
                            break;
                        }
                    }
                    //Added By : Manvendra Prajapati
                    // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3347
                    // Date : 15-Apr-1025
                    if (!isGroupingApplied)
                    {
                        foreach (UltraGridRow row in grdReport.Rows)
                        {
                            foreach (UltraGridCell cell in row.Cells)
                            {
                                cell.Value = cell.Text;
                            }
                        }
                    }
                    string pathName = null;
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Application.StartupPath;
                    saveFileDialog1.Filter = "Excel WorkBook File (*.xls)|*.xls|CSV File (*.csv)|*.csv|PDF File (*.PDF)|*.pdf";
                    saveFileDialog1.RestoreDirectory = true;

                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        pathName = saveFileDialog1.FileName;
                        UltraGridColumn col = grdReport.DisplayLayout.Bands[0].Columns[0];
                        col = col.GetRelatedVisibleColumn(VisibleRelation.First);
                        // modified by sachin mishra 19/02/15  purpose :showing comment at last of the exporting file.
                        if (!isGroupingApplied)
                        {
                            UltraGridRow row = grdReport.DisplayLayout.Bands[0].AddNew();
                            UltraGridRow row1 = grdReport.DisplayLayout.Bands[0].AddNew();
                            UltraGridRow row2 = grdReport.DisplayLayout.Bands[0].AddNew();
                            row.ParentCollection.Move(row, grdReport.Rows.Count - 1);
                            row1.ParentCollection.Move(row1, grdReport.Rows.Count - 1);
                            row2.ParentCollection.Move(row2, grdReport.Rows.Count - 1);
                            row1.Cells[col.Index].Value = "Comment:- " + txtComment.Text;
                        }

                        UltraGridFileExporter.ExportFileForFileFormat(grdReport, pathName);
                        if (!isGroupingApplied)
                        {
                            grdReport.Rows[(grdReport.Rows.Count - 1)].Delete(false);
                            grdReport.Rows[(grdReport.Rows.Count - 1)].Delete(false);
                            grdReport.Rows[(grdReport.Rows.Count - 1)].Delete(false);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Remove Grouping first !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void grdReport_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);
                //sets grid grouping and sorting
                ReconTemplate reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(cmbClient.Value.ToString() + Seperators.SEPERATOR_6 + cmbReconType.Text + Seperators.SEPERATOR_6 + cmbTemplate.Text);
                grdReport = ReconUtilities.ApplyGroupingAndSorting(AutomationEnum.FileFormat.xls, reconTemplate.SelectedColumnList, reconTemplate.ListSortByColumns, reconTemplate.ListGroupByColumns, grdReport);

                //colors alternate rows
                grdReport.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Gray;
                grdReport.DisplayLayout.Override.RowAlternateAppearance.BackColor2 = Color.DarkGray;
                grdReport.DisplayLayout.Override.RowAlternateAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;

                //list of all coumns name saved in prefrence in selected columns to be displayed
                List<string> selectedColumn = ReconUtilities.GetSelectedColumnsList(reconTemplate.SelectedColumnList);
                //hide columns not to be displayed and disable editing on grid
                foreach (UltraGridColumn item in e.Layout.Bands[0].Columns)
                {
                    if (!selectedColumn.Contains(item.Key))
                    {
                        item.Hidden = true;
                    }
                    item.CellActivation = Activation.NoEdit;

                    //Added By : Manvendra Prajapati
                    // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3347
                    // Date : 15-Apr-1025
                    //modified by: sachin mishra Purpose-For setting comma separated values on grid and export files.
                    ReconTemplate _reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(reconTemplate.TemplateKey);
                    ReconUIUtilities.CommaSeparatedMethod(e, _reconTemplate);


                }
                //added by amit 02.04.2015 CHMW-3249
                //ReconUtilities.SetThousandSeparatorFormat(grdReport, reconTemplate.TemplateKey);
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
        /// Sets Columns Caption For UltraGrid
        /// </summary>
        /// <param name="band"></param>
        private void SetColumnsCaptionForUltraGrid(UltraGridBand band)
        {
            try
            {
                foreach (UltraGridColumn col in band.Columns)
                {
                    //sets space in column caption whose coulmn name contains Nirvana,Broker,Diff,OriginalValue,ReconStatus,ToleranceType,ToleranceValue
                    string columnKey = col.Key;
                    String caption = columnKey;
                    if (columnKey.Contains(ReconConstants.CONST_Nirvana))
                    {
                        caption = ReconConstants.CONST_Nirvana + " " + columnKey.Substring(ReconConstants.CONST_Nirvana.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_OriginalValue))
                    {
                        caption = ReconConstants.CONST_OriginalValue + " " + columnKey.Substring(ReconConstants.CONST_OriginalValue.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_Broker))
                    {
                        caption = ReconConstants.CONST_Broker + " " + columnKey.Substring(ReconConstants.CONST_Broker.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_ReconStatus))
                    {
                        caption = ReconConstants.CONST_ReconStatus + " " + columnKey.Substring(ReconConstants.CONST_ReconStatus.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_ToleranceType))
                    {
                        caption = ReconConstants.CONST_ToleranceType + " " + columnKey.Substring(ReconConstants.CONST_ToleranceType.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_ToleranceValue))
                    {
                        caption = ReconConstants.CONST_ToleranceValue + " " + columnKey.Substring(ReconConstants.CONST_ToleranceValue.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_Diff))
                    {
                        caption = ReconConstants.CONST_Diff + " " + columnKey.Substring(ReconConstants.CONST_Diff.Length);
                    }
                    col.Header.Caption = caption.ToString();
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
        /// sets the control on UI and displays the data on the grid
        /// </summary>        
        /// <param name="client"></param>
        /// <param name="reconType"></param>
        /// <param name="template"></param>
        /// <param name="toDate"></param>
        internal void SetDataOnControl(ReconParameters reconParameters, string comment)
        {
            try
            {
                InitializeDataSources();
                cmbClient.Value = reconParameters.ClientID;
                cmbReconType.Text = reconParameters.ReconType;
                cmbTemplate.Text = reconParameters.TemplateName;
                dtFromDate.Value = reconParameters.DTFromDate;
                dtToDate.Value = reconParameters.DTToDate;
                cmbFormatName.Text = reconParameters.FormatName;
                cmbRunByDate.Value = reconParameters.ReconDateType;
                dtRunDate.Value = reconParameters.RunDate;
                btView_Click(null, null);
                txtComment.Text = comment;
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
        /// Filter cell value changed for grdReport.
        /// Added by Ankit Gupta on 30 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1328
        /// Filtering grouped data should be smooth.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReport_FilterCellValueChanged(object sender, FilterCellValueChangedEventArgs e)
        {
            try
            {
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3745
                if (UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdReport))
                {
                    String filterCondition = e.FilterCell.Text;
                    grdReport.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Clear();
                    grdReport.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Add(new FilterCondition(FilterComparisionOperator.StartsWith, filterCondition));
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

        private void grdReport_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                #region Fetch Account ID
                int accountID = int.MinValue;
                if (e.Row.Cells.Exists(ReconConstants.COLUMN_AccountName) && e.Row.Cells[ReconConstants.COLUMN_AccountName].Value != null
                    && !string.IsNullOrEmpty(e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString()))
                {
                    accountID = CachedDataManager.GetInstance.GetAccountID(e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString());
                }
                #endregion
                #region Hide Row
                #region When User Dont have permission for account
                if (accountID != int.MinValue && !CachedDataManager.GetInstance.GetUserAccounts().Contains(accountID))
                {
                    int index = e.Row.Index;
                    e.Row.Delete(false);
                    if (grdReport.Rows.Count > index)
                    {
                        grdReport.Rows[index].Refresh(Infragistics.Win.UltraWinGrid.RefreshRow.FireInitializeRow);
                    }
                }
                #endregion
                #endregion
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
        private void cmbFormatName_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                ultraToolTipInfo2.ToolTipText = cmbFormatName.Text;
                ultraToolTipManager1.SetUltraToolTip(cmbFormatName, ultraToolTipInfo2);
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

    }
}
