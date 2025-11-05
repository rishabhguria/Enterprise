using Infragistics.Win.CalcEngine;
using Infragistics.Win.UltraWinCalcManager.FormulaBuilder;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Tools
{
    /// <summary>
    /// modified by: sachin mishra,02 Feb 2015
    /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
    /// </summary>
    public partial class frmCustomColumn : Form
    {
        ReconTemplate _reconTemplate = null;
        DataSet _dsCustomColumns = new DataSet();
        DataSet _dsMatchingRules = new DataSet();
        DataSet _dsMasterColumns = new DataSet();
        DataRow _row = null;
        public frmCustomColumn()
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

        internal void LoadData(ReconTemplate reconTemplate)
        {
            try
            {
                _reconTemplate = reconTemplate;
                _dsMasterColumns = _reconTemplate.DsMasterColumns.Copy();
                _dsMatchingRules = _reconTemplate.DsMatchingRules.Copy();
                _dsCustomColumns = _reconTemplate.DsCustomColumns.Copy();
                if (_dsCustomColumns.Tables.Count == 0)
                {
                    _dsCustomColumns.Tables.Add(LoadDefaultColumns());
                }
                grdCustomColumns.DataSource = _dsCustomColumns;
                _reconTemplate = reconTemplate;

                //List<string> columns = reconTemplate.RulesList[0].NumericFields;
                //DataTable dt = new DataTable(ReconConstants.CustomColumnsTableName);
                //foreach (string column in columns)
                //{
                //    dt.Columns.Add(column);
                //}
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

        private DataTable LoadDefaultColumns()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.TableName = ReconConstants.CustomColumnsTableName;

                dt.Columns.Add(ReconConstants.COLUMN_Name);
                //dt.Columns.Add("ColumnType");
                dt.Columns.Add(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_FormulaExpression);
                dt.Columns.Add(ReconConstants.CONST_Broker + ReconConstants.COLUMN_FormulaExpression);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (_row != null)
                {
                    MessageBox.Show("Row already added.", "Alert", MessageBoxButtons.OK);
                    return;
                }
                _row = _dsCustomColumns.Tables[0].NewRow();
                _row[ReconConstants.COLUMN_Name] = string.Empty;
                _dsCustomColumns.Tables[0].Rows.Add(_row);

                //DataRow dr = _dsCustomColumns.Tables[0].NewRow();
                //dr[COLUMN_Name] = Microsoft.VisualBasic.Interaction.InputBox("Enter Column Name:", "Custom Column").Trim();
                //if (string.IsNullOrEmpty(dr[COLUMN_Name].ToString()))
                //{
                //    MessageBox.Show("Column Name cannot be blank spaces.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}
                //if (ReconUtilities.ValueExistInDataSet(_dsMatchingRules, MatchingRulesRuleTableName, COLUMN_Name, dr[COLUMN_Name].ToString()) != null
                //    || ReconUtilities.ValueExistInDataSet(_dsMasterColumns, MasterColumnsPBTableName, COLUMN_Name, dr[COLUMN_Name].ToString()) != null
                //    || ReconUtilities.ValueExistInDataSet(_dsMasterColumns, MasterColumnsNirvanaTableName, COLUMN_Name, dr[COLUMN_Name].ToString()) != null
                //    || ReconUtilities.ValueExistInDataSet(_dsCustomColumns, CustomColumnsTableName, COLUMN_Name, dr[COLUMN_Name].ToString()) != null)
                //{
                //    MessageBox.Show("Column already exists.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}
                //_dsCustomColumns.Tables[0].Rows.Add(dr);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_row != null && string.IsNullOrEmpty(_row[ReconConstants.COLUMN_Name].ToString().Trim()))
                {
                    _dsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].Rows.Remove(_row);
                    _row = null;
                }
                //int length = grdCustomColumns.Rows.Count;
                for (int i = 0; i < grdCustomColumns.Rows.Count; i++)
                {
                    UltraGridRow row = grdCustomColumns.Rows[i];
                    if (row.Cells[ReconConstants.COLUMN_Checkbox].Value != null
                        && row.Cells[ReconConstants.COLUMN_Checkbox].Value.ToString().Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DeleteColumnFromDataSet(_dsMasterColumns, ReconConstants.MasterColumnsPBTableName, row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
                        DeleteColumnFromDataSet(_dsMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
                        DeleteColumnFromDataSet(_dsMatchingRules, ReconConstants.MatchingRulesRuleTableName, row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
                        row.Delete(false);
                        row = null;
                        //length--;
                        i--;

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _reconTemplate.DsCustomColumns = _dsCustomColumns;
                bool isEmptyDetails = false;
                List<string> visibleRules = _reconTemplate.VisibleRules;
                foreach (UltraGridRow row in grdCustomColumns.Rows)
                {
                    visibleRules.Add(row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
                    if (string.IsNullOrEmpty(row.Cells[ReconConstants.CONST_Broker + ReconConstants.COLUMN_FormulaExpression].Value.ToString())
                        || string.IsNullOrEmpty(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_FormulaExpression].Value.ToString()))
                    {
                        isEmptyDetails = true;
                        break;
                    }
                    AddorUpadteDataSet(_dsMatchingRules, ReconConstants.MatchingRulesRuleTableName, row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
                    AddorUpadteDataSet(_dsMasterColumns, ReconConstants.MasterColumnsPBTableName, row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
                    AddorUpadteDataSet(_dsMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
                }
                if (isEmptyDetails)
                {
                    MessageBox.Show("Blank formula expression cannot be saved.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    _reconTemplate.IsDirtyForSaving = true;
                    _reconTemplate.VisibleRules = visibleRules;
                    _reconTemplate.DsCustomColumns = _dsCustomColumns;
                    _reconTemplate.DsMatchingRules = _dsMatchingRules;
                    _reconTemplate.DsMasterColumns = _dsMasterColumns;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }



        private void grdCustomColumns_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

                //Set the HeaderCheckBoxVisibility so it will display the CheckBox whenever a CheckEditor is used within the UltraGridColumn 
                e.Layout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                //Set the HeaderCheckBoxAlignment so the CheckBox will appear to the Right of the caption. 
                e.Layout.Override.HeaderCheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                //Set the HeaderCheckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                e.Layout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;

                //if (band.Columns.Exists("ColumnType"))
                //{
                //    UltraGridColumn colColumnType = band.Columns["ColumnType"];
                //    colColumnType.Header.Caption = "Column Type";
                //    ValueList vlPrincipalType = new ValueList();
                //    vlPrincipalType.ValueListItems.Add(0, ColumnGroupType.Common.ToString());
                //    vlPrincipalType.ValueListItems.Add(1, ColumnGroupType.Nirvana.ToString());
                //    vlPrincipalType.ValueListItems.Add(2, ColumnGroupType.PrimeBroker.ToString());
                //    colColumnType.ValueList = vlPrincipalType;
                //    colColumnType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //}

                if (!band.Columns.Exists("CheckBox"))
                {
                    band.Columns.Add("CheckBox");
                }
                UltraGridColumn colCheckBox = band.Columns["CheckBox"];
                colCheckBox.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                colCheckBox.Header.Caption = string.Empty;
                colCheckBox.Header.VisiblePosition = 0;
                colCheckBox.Width = 20;
                colCheckBox.DataType = typeof(bool);
                colCheckBox.DefaultCellValue = false;

                if (band.Columns.Exists(ReconConstants.COLUMN_Name))
                {
                    UltraGridColumn colBtnView = band.Columns[ReconConstants.COLUMN_Name];
                    //colBtnView.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_FormulaExpression))
                {
                    UltraGridColumn colBtnView = band.Columns[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_FormulaExpression];
                    colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnView.Header.Caption = ReconConstants.CONST_Nirvana + " " + ReconConstants.CAPTION_FormulaExpression;
                    colBtnView.NullText = "Set Formula";
                    colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBtnView.CellActivation = Activation.NoEdit;
                }
                if (band.Columns.Exists(ReconConstants.CONST_Broker + ReconConstants.COLUMN_FormulaExpression))
                {
                    UltraGridColumn colBtnView = band.Columns[ReconConstants.CONST_Broker + ReconConstants.COLUMN_FormulaExpression];
                    colBtnView.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colBtnView.Header.Caption = ReconConstants.CONST_Broker + " " + ReconConstants.CAPTION_FormulaExpression;
                    colBtnView.NullText = "Set Formula";
                    colBtnView.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBtnView.CellActivation = Activation.NoEdit;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void frmCustomColumn_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyTheme((Form)sender);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Applies Theme on Form
        /// </summary>
        /// <param name="form"></param>
        private void ApplyTheme(Form form)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(form.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCustomColumns_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (!e.Cell.Row.Hidden)
                {
                    if (e.Cell.Row.Cells.Exists(ReconConstants.COLUMN_Name) && e.Cell.Row.Cells[ReconConstants.COLUMN_Name].Value != null && !string.IsNullOrEmpty(e.Cell.Row.Cells[ReconConstants.COLUMN_Name].Value.ToString()))
                    {
                        SetColumnsGrid(e);

                        string columnName = e.Cell.Row.Cells[ReconConstants.COLUMN_Name].Value.ToString();
                        if (grdColumns.DisplayLayout.Bands[0].Columns.Exists(columnName))
                        {
                            UltraGridColumn currentColumn = grdColumns.DisplayLayout.Bands[0].Columns[columnName];
                            IFormulaProvider columnFormulaProvider = currentColumn as IFormulaProvider;

                            //load previous formula
                            if (e.Cell.Value.ToString() != e.Cell.Column.NullText)
                            {
                                columnFormulaProvider.Formula = e.Cell.Value.ToString();
                            }

                            // Show the dialog
                            e.Cell.Value = ShowFormulaBuilderDialog(columnFormulaProvider);

                            //set null text if the formula is set to null
                            if (string.IsNullOrEmpty(e.Cell.Value.ToString()))
                            {
                                e.Cell.Value = e.Cell.Column.NullText;
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Column name cannot be empty.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets Columns and datatable in Grid
        /// </summary>
        /// <param name="e"></param>
        private void SetColumnsGrid(CellEventArgs e)
        {
            try
            {
                grdColumns.DisplayLayout.Bands[0].Reset();
                grdColumns.DataSource = new DataTable();
                //hide the grid from formula builder dialog
                grdCustomColumns.CalcManager = null;
                AddColumnsInCustomColumnGrid(e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> GetColumns(CellEventArgs e)
        {
            List<string> columns = new List<string>();
            try
            {
                if (e.Cell.Column.Key.Equals(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_FormulaExpression))
                {
                    columns = _dsMatchingRules.Tables[1].AsEnumerable().Select(s => s.Field<string>(ReconConstants.COLUMN_Name)).ToList<string>();
                }
                else
                {
                    columns = _dsMatchingRules.Tables[1].AsEnumerable().Select(s => s.Field<string>(ReconConstants.COLUMN_Name)).ToList<string>();
                }

                columns.AddRange((from DataRow dr in _dsCustomColumns.Tables[0].Rows
                                  where !string.IsNullOrEmpty(dr[e.Cell.Column.Key].ToString())
                                  select (string)dr[ReconConstants.COLUMN_Name]));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return columns;
        }

        private void AddColumnsInCustomColumnGrid(CellEventArgs e)
        {
            try
            {
                List<string> columns = GetColumns(e);
                //add all the columns to display in formula builder
                foreach (string column in columns.Distinct())
                {
                    if (_reconTemplate.VisibleRules.Contains(column))
                    {
                        grdColumns.DisplayLayout.Bands[0].Columns.Add(column);
                    }
                    else
                    {
                        if (ReconUtilities.ValueExistInDataSet(_dsCustomColumns, ReconConstants.CustomColumnsTableName, ReconConstants.COLUMN_Name, column) != null)
                        {
                            grdColumns.DisplayLayout.Bands[0].Columns.Add(column);
                        }
                    }
                }
                //add current row in ultragrid
                if (!grdColumns.DisplayLayout.Bands[0].Columns.Exists(e.Cell.Row.Cells[ReconConstants.COLUMN_Name].Value.ToString()))
                {
                    grdColumns.DisplayLayout.Bands[0].Columns.Add(e.Cell.Row.Cells[ReconConstants.COLUMN_Name].Value.ToString());
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

        #region ShowFormulaBuilderDialog method
        private string ShowFormulaBuilderDialog(IFormulaProvider formulaProvider)
        {
            // Declare a new FormulaBuilderDialog			
            FormulaBuilderDialog formulaBuilderDialog = new FormulaBuilderDialog(formulaProvider);
            formulaBuilderDialog.Load += new System.EventHandler(this.formulaBuilderDialog_Load);
            formulaBuilderDialog.OperandInitializing += new OperandInitializingEventHandler(this.formulaBuilder_OperandInitializing);
            formulaBuilderDialog.FunctionInitializing += new FunctionInitializingEventHandler(this.formulaBuilder_FunctionInitializing);
            try
            {
                EditFormulabuilderDialogUI(formulaBuilderDialog);
                // formulaBuilderDialog.
                // Show the dialog
                DialogResult dResult = formulaBuilderDialog.ShowDialog(this);
                // If the user cancelled, do nothing
                if (dResult != DialogResult.Cancel)
                {
                    // Apply the formula
                    if (!formulaBuilderDialog.HasSyntaxError && !String.IsNullOrEmpty(formulaBuilderDialog.Formula))
                    {
                        formulaProvider.Formula = formulaBuilderDialog.Formula;
                    }
                    return formulaProvider.Formula;
                }
                if (dResult == DialogResult.Cancel)
                {
                    return formulaProvider.Formula;
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
            finally
            {
                // Whatever happens, make sure we disconnect from 
                // the events
                formulaBuilderDialog.OperandInitializing -= new OperandInitializingEventHandler(this.formulaBuilder_OperandInitializing);
                formulaBuilderDialog.FunctionInitializing -= new FunctionInitializingEventHandler(this.formulaBuilder_FunctionInitializing);
            }
            return string.Empty;
        }

        private static void EditFormulabuilderDialogUI(FormulaBuilderDialog formulaBuilderDialog)
        {
            try
            {
                #region remove FunctionTab
                RemoveFunctionTabFromDialog(formulaBuilderDialog);
                #endregion

                #region update RoundedSizable property of UltraToolbarsManagers
                SetRoundedSizableProperty(formulaBuilderDialog);
                #endregion

                #region Set min/max size of the formula builder dialog
                SetFormSize(formulaBuilderDialog);
                #endregion
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

        private static void SetFormSize(FormulaBuilderDialog formulaBuilderDialog)
        {
            try
            {
                formulaBuilderDialog.MaximumSize = new Size(843, 643);
                formulaBuilderDialog.MinimumSize = new Size(843, 643);
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

        private static void SetRoundedSizableProperty(FormulaBuilderDialog formulaBuilderDialog)
        {
            try
            {
                if (formulaBuilderDialog != null && formulaBuilderDialog.Controls.Count >= 2)
                {
                    UltraToolbarsDockArea ultraToolbarsDockArea1 = (UltraToolbarsDockArea)formulaBuilderDialog.Controls[1];
                    if (ultraToolbarsDockArea1 != null && ultraToolbarsDockArea1.ToolbarsManager != null)
                    {
                        ultraToolbarsDockArea1.ToolbarsManager.FormDisplayStyle = FormDisplayStyle.RoundedSizable;
                    }
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

        private static void RemoveFunctionTabFromDialog(FormulaBuilderDialog formulaBuilderDialog)
        {
            try
            {
                if (formulaBuilderDialog != null && formulaBuilderDialog.Controls.Count >= 1)
                {
                    Panel panel1 = (Panel)formulaBuilderDialog.Controls[0];
                    if (panel1 != null && panel1.Controls.Count >= 3)
                    {
                        UltraTabControl ultraTabControl1 = (UltraTabControl)panel1.Controls[2];
                        if (ultraTabControl1 != null && ultraTabControl1.Tabs.Count >= 2)
                        {
                            ultraTabControl1.Tabs[1].Visible = false;
                        }
                    }
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

        private void formulaBuilderDialog_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyTheme((FormulaBuilderDialog)sender);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //private void AppDomainsd(object sender, ControlEventArgs e)
        //{
        //    // throw new NotImplementedException();
        //}
        #endregion ShowFormulaBuilderDialog method

        #region FormulaBuilderDialog Events

        #region formulaBuilder_OperandInitializing
        // This event fires for each operand added to the list
        // of operands in the FormulaBuilder and provides the 
        // opportunity to cancel them. 
        private void formulaBuilder_OperandInitializing(object sender, OperandInitializingEventArgs e)
        {
            // See if the FormulaProvider is a SummarySettings
            SummarySettings summarySettings = e.FormulaProvider as SummarySettings;
            if (summarySettings != null)
            {
                // Get the source column for the Summary
                UltraGridColumn summaryColumn = summarySettings.SummaryPositionColumn;

                // It doesn't make sense for the Summary formula to use values from any 
                // other grid columns except the source column, so filter those out. 
                if (e.OperandContext is UltraGridColumn
                    && e.OperandContext != summaryColumn)
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion formulaBuilder_OperandInitializing

        #region formulaBuilder_FunctionInitializing
        // This event fires for each function added to the list
        // of functions in the FormulaBuilder and provides the 
        // opportunity to cancel them. 
        private void formulaBuilder_FunctionInitializing(object sender, FunctionInitializingEventArgs e)
        {
            e.Cancel = true;
            // Some of the available functions do not make sense for Integer
            // data. So filter out functions. 
            switch (e.Function.Category)
            {
                case "Information":
                case "Logical":
                case "TextAndData":
                    e.Cancel = true;
                    break;
            }
        }
        #endregion formulaBuilder_FunctionInitializing

        #endregion FormulaBuilderDialog Events
        #region Grid Events

        #region ultraGrid1_InitializeLayout
        private void ultraGrid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            // Set AllowRowSummaries to true. This will create a Summary
            // icon in each column header. 
            e.Layout.Override.AllowRowSummaries = AllowRowSummaries.True;

            // Allow Single-select on columns
            e.Layout.Override.SelectTypeCol = SelectType.Single;
        }
        #endregion ultraGrid1_InitializeLayout

        #region ultraGrid1_BeforeSummaryDialog
        private void ultraGrid1_BeforeSummaryDialog(object sender, Infragistics.Win.UltraWinGrid.BeforeSummaryDialogEventArgs e)
        {
            //Cancel the default Summary dialog. 
            e.Cancel = true;

            // Show the FormulaBuilderDialog instead. 
            // In order to show the FormulaBuilderDialog, it needs to know
            // which formula is being edited, so that it can: 
            //	1) Populate with any existing formula information
            //	2) Use Relative references
            // The Formula being edited is specified by passing in an IFormulaProvider object. 
            // The UltraGridColumn and SummarySettings in the grid both implement this interface. 
            // Here, use a SummarySettings, so create a new SummarySettings.

            // If the column already has a summary, use that. Otherwise
            // create a new one. 
            SummarySettings summarySettings = null;
            string summaryKey = "Summary:" + e.Column.Key;

            if (e.Column.Band.Summaries.Exists(summaryKey))
                summarySettings = e.Column.Band.Summaries[summaryKey];
            else
            {
                summarySettings = e.Column.Band.Summaries.Add(summaryKey, string.Empty, SummaryPosition.UseSummaryPositionColumn, e.Column);
                // Hide the Summary for now, in case the user cancels.
                summarySettings.SummaryDisplayArea = SummaryDisplayAreas.None;
            }

            // Show the dialog
            ShowFormulaBuilderDialog(summarySettings as IFormulaProvider);

            // If the formula is blank for any reason, get rid of the Summary
            if (summarySettings.Formula.Length == 0)
                e.Column.Band.Summaries.Remove(summarySettings);
            else
            {
                //Make sure the summary is no longer hidden, in case it was.
                summarySettings.SummaryDisplayArea = SummaryDisplayAreas.Default;
            }
        }
        #endregion ultraGrid1_BeforeSummaryDialog
        #endregion Grid Events
        //private void grdCustomColumns_DoubleClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (grdCustomColumns.ActiveCell != null && grdCustomColumns.ActiveCell.Column.Key != ReconConstants.COLUMN_Name)
        //        {
        //            string previousName = grdCustomColumns.ActiveCell.Value.ToString();
        //            string newName = Interaction.InputBox("Enter Column Name:", "Custom Column").Trim();
        //            if (newName.Equals(previousName))
        //            {
        //                MessageBox.Show("Column Name cannot be blank spaces.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                return;
        //            }
        //            else
        //            {
        //                grdCustomColumns.ActiveCell.Value = newName;
        //                RenameColumninDataSet(_dsMatchingRules, ReconConstants.MatchingRulesRuleTableName, previousName, newName);
        //                RenameColumninDataSet(_dsMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, previousName, newName);
        //                RenameColumninDataSet(_dsMasterColumns, ReconConstants.MasterColumnsPBTableName, previousName, newName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        private void RenameColumninDataSet(DataSet dataSet, string tableName, string previousName, string newName)
        {
            try
            {
                DataRow row = ReconUtilities.ValueExistInDataSet(dataSet, tableName, ReconConstants.COLUMN_Name, previousName);
                if (row != null)
                {
                    row[ReconConstants.COLUMN_Name] = newName;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void DeleteColumnFromDataSet(DataSet dataSet, string tableName, string columnName)
        {
            try
            {
                DataRow row = ReconUtilities.ValueExistInDataSet(dataSet, tableName, ReconConstants.COLUMN_Name, columnName);
                if (row != null)
                {
                    dataSet.Tables[tableName].Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void AddorUpadteDataSet(DataSet dataSet, string tableName, string columnValue)
        {
            try
            {
                DataRow row = ReconUtilities.ValueExistInDataSet(dataSet, tableName, ReconConstants.COLUMN_Name, columnValue);
                if (row == null)
                {
                    DataRow dr = dataSet.Tables[tableName].NewRow();
                    if (tableName.Equals(ReconConstants.MasterColumnsNirvanaTableName) || tableName.Equals(ReconConstants.MasterColumnsPBTableName))
                    {
                        dr["IsSelected"] = false;
                        dr["DataSourceType"] = 2;
                        dr["GroupType"] = 3;
                        dr["Summary"] = 1;
                        dr["IsEditable"] = false;
                        dr["Recon_Id"] = dataSet.Tables[tableName].Rows[0]["Recon_Id"];
                    }
                    else if (tableName.Equals(ReconConstants.MatchingRulesRuleTableName))
                    {
                        dr["Type"] = 2;
                        dr["IsIncluded"] = false;
                        dr["ErrorTolerance"] = 0;
                        dr["IsRoundOff"] = false;
                        dr["IsIntegral"] = false;
                        dr["IsPercentMatch"] = false;
                        dr["IsAbsoluteMatch"] = false;
                        dr["AbsoluteDifference"] = 0;
                        dr["Rule_Id"] = dataSet.Tables[1].Rows[0]["Rule_Id"];
                    }
                    //dr.ItemArray = ReconUtilities.ValueExistInDataSet(dataSet, tableName, COLUMN_Name, COLUMN_Quantity).ItemArray;
                    dr[ReconConstants.COLUMN_Name] = columnValue;
                    dataSet.Tables[tableName].Rows.Add(dr);
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
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdCustomColumns.ActiveRow.Cells[ReconConstants.COLUMN_Name].Activation = Activation.AllowEdit;
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

        //private void grdCustomColumns_MouseDown(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Button == MouseButtons.Right)
        //        {
        //            UltraGrid grid = sender as UltraGrid;
        //            if (grid != null)
        //            {
        //                UIElement element = grid.DisplayLayout.UIElement.ElementFromPoint(e.Location);
        //                var row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;
        //                if (row != null && row.IsDataRow)
        //                {
        //                    grid.ActiveRow = row;
        //                    if (!row.Selected)
        //                    {
        //                        grid.Selected.Rows.Clear();
        //                        row.Selected = true;
        //                    }
        //                }
        //                else
        //                {
        //                    grid.Selected.Rows.Clear();
        //                }
        //            }
        //        }
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

        private void grdCustomColumns_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                UltraGrid grid = sender as UltraGrid;
                if (grid != null)
                {
                    if (e.Cell.Column.Key != ReconConstants.COLUMN_Name)
                    {
                        return;
                    }
                    string previousName = e.Cell.Value.ToString();
                    //CHMW-2168	[Custom Column] Column name blank is saving when edit and save already added column
                    if (string.IsNullOrEmpty(e.NewValue.ToString().Trim()))
                    {
                        MessageBox.Show("Column Name cannot be empty.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        return;
                    }
                    if (e.NewValue.ToString().Contains(' '))
                    {
                        MessageBox.Show("Column Name cannot have blank spaces.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        if (_row != null)
                        {
                            _dsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].Rows.Remove(_row);
                            _row = null;
                        }
                        e.Cancel = true;
                        return;
                    }
                    if (ReconUtilities.ValueExistInDataSet(_dsMatchingRules, ReconConstants.MatchingRulesRuleTableName, ReconConstants.COLUMN_Name, e.NewValue.ToString().ToString().Replace(" ", "")) != null
                            || ReconUtilities.ValueExistInDataSet(_dsMasterColumns, ReconConstants.MasterColumnsPBTableName, ReconConstants.COLUMN_Name, e.NewValue.ToString().ToString().Replace(" ", "")) != null
                            || ReconUtilities.ValueExistInDataSet(_dsMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, ReconConstants.COLUMN_Name, e.NewValue.ToString().ToString().Replace(" ", "")) != null
                            || ReconUtilities.ValueExistInDataSet(_dsCustomColumns, ReconConstants.CustomColumnsTableName, ReconConstants.COLUMN_Name, e.NewValue.ToString().ToString().Replace(" ", "")) != null)
                    {
                        MessageBox.Show("Column with same name already exists.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        if (_row != null)
                        {
                            _dsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].Rows.Remove(_row);
                            _row = null;
                        }
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        //grdCustomColumns.ActiveCell.Value = e.NewValue.ToString();
                        RenameColumninDataSet(_dsMatchingRules, ReconConstants.MatchingRulesRuleTableName, previousName, e.NewValue.ToString().Replace(" ", ""));
                        RenameColumninDataSet(_dsMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, previousName, e.NewValue.ToString().Replace(" ", ""));
                        RenameColumninDataSet(_dsMasterColumns, ReconConstants.MasterColumnsPBTableName, previousName, e.NewValue.ToString().Replace(" ", ""));
                        //grid.ActiveRow.Cells[COLUMN_Name].Activation = Activation.NoEdit;
                        _row = null;
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

        private void frmCustomColumn_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_row != null)
                {
                    _dsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].Rows.Remove(_row);
                    _row.Delete();
                    _row = null;
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
        /// Disable user to press Space bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCustomColumns_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Space)
                {
                    e.SuppressKeyPress = true;
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
    }
}