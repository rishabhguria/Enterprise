//using Prana.Reconciliation;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlMasterColumns : UserControl
    {
        DataTable dtNirvanaGridDisplayColumns = new DataTable();

        DataTable dtPBGridDisplayColumns = new DataTable();
        //DataTable dtPBPosMasterColumns = new DataTable();
        //DataTable dtPBTrnMasterColumns = new DataTable();

        ReconTemplate _reconTemplate = new ReconTemplate();
        private bool _isUnsavedChanges = false;
        //bool isAlreadyInitialized = false;
        private const string MASTER_COLUMNS_FILE = "MasterColumns.xml";
        private const string MASTER_COLUMNS_SCHEMA = "MasterColumns.xsd";
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlMasterColumns()
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
                btn_addRow_Nirvana.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btn_addRow_Nirvana.ForeColor = System.Drawing.Color.White;
                btn_addRow_Nirvana.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn_addRow_Nirvana.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btn_addRow_Nirvana.UseAppStyling = false;
                btn_addRow_Nirvana.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        #region unused code
        //public void InitializeMasterColumnsTabForSelectedTemplate(ReconTemplate template)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds.ReadXmlSchema(GetPath(MASTER_COLUMNS_SCHEMA));
        //        ds.ReadXml(GetPath(MASTER_COLUMNS_FILE));


        //        dtPBMasterColumns = GetFilteredMasterColumns(ds, ReconType.Position, DataSourceType.Nirvana);
        //        dtPBMasterColumns.TableName = ReconConstants.TABLENAME_NirvanaMasterColumns;

        //        dtPBMasterColumns = GetFilteredMasterColumns(ds, ReconType.Transaction, DataSourceType.PrimeBroker);
        //        dtPBMasterColumns.TableName = ReconConstants.TABLENAME_PBMasterColumns;

        //        dtPBPosMasterColumns = GetFilteredMasterColumns(ds, ReconType.Position, DataSourceType.PrimeBroker);
        //        dtPBPosMasterColumns.TableName = ReconConstants.TABLENAME_PBPosition;

        //        dtPBTrnMasterColumns = GetFilteredMasterColumns(ds, ReconType.Transaction, DataSourceType.PrimeBroker);
        //        dtPBTrnMasterColumns.TableName = ReconConstants.TABLENAME_PBTrn;

        //        //if (!isAlreadyInitialized)
        //        //{
        //        //    isAlreadyInitialized = true;
        //        //    BindReconTypeCombo();
        //        //}
        //        //else
        //        //{
        //        if (template.ReconType == ReconType.Position)
        //        {
        //            grdMasterColumnsPB.DataSource = dtPBPosMasterColumns;
        //            grdMasterColumnsNirvana.DataSource = dtPBMasterColumns;
        //        }
        //        else if (template.ReconType == ReconType.Transaction)
        //        {
        //            grdMasterColumnsPB.DataSource = dtPBTrnMasterColumns;
        //            grdMasterColumnsNirvana.DataSource = dtPBMasterColumns;
        //        }
        //        grdMasterColumnsNirvana.DataBind();
        //        grdMasterColumnsPB.DataBind();
        //        //    //grdMasterColumnsNirvana.DataMember = ds.Tables[0].ToString();
        //        //    //grdMasterColumnsPB.DataMember = ds.Tables[0].ToString();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}


        //private DataTable GetFilteredMasterColumns(DataSet dsMain, ReconType reconType, DataSourceType dsType)
        //{
        //    DataTable dtFiltered = new DataTable();
        //    if (dsMain.Tables.Count > 0)
        //    {
        //        dtFiltered = dsMain.Tables[0].Clone();
        //        foreach (DataRow row in dsMain.Tables[0].Rows)
        //        {
        //            if ((int.Parse(row[COLUMN_DataSourceType].ToString()).Equals((int)dsType) || int.Parse(row[COLUMN_DataSourceType].ToString()).Equals((int)DataSourceType.Both)) && (int.Parse(row[COLUMN_ReconType].ToString()).Equals((int)reconType) || int.Parse(row[COLUMN_ReconType].ToString()).Equals((int)ReconType.Both)))
        //            {
        //                dtFiltered.Rows.Add(row.ItemArray);
        //            }
        //        }

        //    }
        //    return dtFiltered;
        //}

        #endregion
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="band"></param>
        private void SetGridView(Infragistics.Win.UltraWinGrid.UltraGridBand band)
        {
            try
            {
                band.Columns[ReconConstants.COLUMN_Name].CellActivation = Activation.AllowEdit;
                band.Columns[ReconConstants.COLUMN_Name].Header.Column.Width = 130;

                band.Columns[ReconConstants.COLUMN_ISSelected].Header.VisiblePosition = 2;
                band.Columns[ReconConstants.COLUMN_ISSelected].Header.Caption = "Is Selected";
                //band.Columns[COLUMN_ISSelected].Width = 42;

                band.Columns[ReconConstants.COLUMN_FormulaExpression].Header.VisiblePosition = 2;
                band.Columns[ReconConstants.COLUMN_FormulaExpression].CellActivation = Activation.AllowEdit;
                band.Columns[ReconConstants.COLUMN_FormulaExpression].Header.Caption = "Formula Expression";
                if (band.Columns.Exists(ReconConstants.COLUMN_ISEditable))
                {
                    band.Columns[ReconConstants.COLUMN_ISEditable].Hidden = true;
                }
                band.Columns[ReconConstants.COLUMN_DataSourceType].Hidden = true;
                band.Columns[ReconConstants.COLUMN_GroupType].Hidden = true;
                band.Columns[ReconConstants.COLUMN_ReconId].Hidden = true;
                band.Columns[ReconConstants.COLUMN_Summary].Hidden = true;
                //band.Columns["RowIndex"].Hidden = true;
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


        //private void SetNirvanaMasterColumnsGridFilters(UltraGrid gridNirvana)
        //{
        //    gridNirvana.DisplayLayout.Bands[0].ColumnFilters[COLUMN_DataSourceType].LogicalOperator = FilterLogicalOperator.Or;
        //    gridNirvana.DisplayLayout.Bands[0].ColumnFilters[COLUMN_DataSourceType].FilterConditions.Add(FilterComparisionOperator.Equals, DataSourceType.Nirvana);
        //    gridNirvana.DisplayLayout.Bands[0].ColumnFilters[COLUMN_DataSourceType].FilterConditions.Add(FilterComparisionOperator.Equals, DataSourceType.Both);
        //}


        //private void SetPBMasterColumnsGridFilters(UltraGrid gridPb)
        //{
        //    gridPb.DisplayLayout.Bands[0].ColumnFilters[COLUMN_DataSourceType].LogicalOperator = FilterLogicalOperator.Or;
        //    gridPb.DisplayLayout.Bands[0].ColumnFilters[COLUMN_DataSourceType].FilterConditions.Add(FilterComparisionOperator.Equals, DataSourceType.PrimeBroker);
        //    gridPb.DisplayLayout.Bands[0].ColumnFilters[COLUMN_DataSourceType].FilterConditions.Add(FilterComparisionOperator.Equals, DataSourceType.Both);

        //}


        /// <summary>
        /// usused method in hole project 
        /// Commented by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //private string GetPath(string fileName)
        //{
        //    return Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString() + @"\" + fileName;
        //}
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMasterColumnsNirvana_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

            try
            {
                grdMasterColumnsNirvana.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                SetGridView(e.Layout.Bands[0]);

                //SetGridView(e.Layout.Bands[])

                //List<UltraGridRow> listRowsCollection = new List<UltraGridRow>();
                //foreach (UltraGridRow row in grdMasterColumnsNirvana.Rows)
                //{
                //    listRowsCollection.Add(row);
                //}


                //foreach (UltraGridRow row in listRowsCollection)
                //{


                //}
                ////grdMasterColumnsNirvana.Refresh(RefreshRow.RefreshDisplay);
                //grdMasterColumnsNirvana.Update();
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        private void grdMasterColumnsPB_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                grdMasterColumnsPB.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                SetGridView(e.Layout.Bands[0]);
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
        //new method added for initialize layout of grid of prime broker
        //private void AddRowsAtSpecifiedIndex(DataTable dt)
        //{

        //    foreach(DataRow

        //}



        //private void BindReconTypeCombo()
        //{
        //    List<EnumerationValue> listValues = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(ReconType));
        //    listValues.RemoveAll(delegate(EnumerationValue value)
        //    {
        //        if (value.DisplayText.Equals(ReconType.Both.ToString()))
        //        {
        //            return true;
        //        }

        //        return false;

        //    });
        //    cmbReconType.DataSource = listValues;
        //    cmbReconType.DisplayMember = "DisplayText";
        //    cmbReconType.ValueMember = "Value";
        //    cmbReconType.DataBind();
        //    cmbReconType.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //    cmbReconType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
        //    cmbReconType.Value = 0;

        //}

        /// <summary>
        /// Comment by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        //private void SetVisibleRow(string rowsToBind, string idColumn)
        //{
        //    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in grdMasterColumnsNirvana.Rows)
        //    {
        //        if (row.Cells[idColumn].Value.ToString() == rowsToBind)
        //        {
        //            row.Hidden = false;
        //        }
        //        else
        //        {
        //            row.Hidden = true;
        //        }
        //    }
        //}

        //private void cmbReconType_ValueChanged(object sender, EventArgs e)
        //{
        //    if (cmbReconType.DataSource != null && cmbReconType.Value != null)
        //    {
        //        //SetVisibleRow(cmbReconType.Value.ToString(), COLUMN_DataSourceType);
        //        if (cmbReconType.Value.Equals((int)ReconType.Position))
        //        {
        //            grdMasterColumnsPB.DataSource = dtPBPosMasterColumns;
        //            grdMasterColumnsNirvana.DataSource = dtNirvanaPositonMasterColumns;
        //        }
        //        else if (cmbReconType.Value.Equals((int)ReconType.Transaction))
        //        {
        //            grdMasterColumnsPB.DataSource = dtPBTrnMasterColumns;
        //            grdMasterColumnsNirvana.DataSource = dtNirvanaTrnMasterColumns;
        //        }

        //        grdMasterColumnsNirvana.DataBind();
        //        grdMasterColumnsPB.DataBind();
        //    }
        //}




        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="AvailableColumnList"></param>
        private void AddCustomColumns(List<ColumnInfo> AvailableColumnList)
        {
            try
            {
                //select columns which have not null formula expression
                DataRow[] PBCustomColumns = dtPBGridDisplayColumns.Select(string.Format(ReconConstants.COLUMN_FormulaExpression + " <> ''"));
                DataRow[] NirvanaCustomColumns = dtNirvanaGridDisplayColumns.Select(string.Format(ReconConstants.COLUMN_FormulaExpression + " <> ''"));
                List<string> NirvanaColumnName = new List<string>();
                List<string> PBFormulaExpressionList = new List<string>();
                foreach (DataRow NirvanaRow in NirvanaCustomColumns)
                {
                    NirvanaColumnName.Add((string)NirvanaRow[ReconConstants.COLUMN_Name]);
                }
                foreach (DataRow NirvanaRow in NirvanaCustomColumns)
                {
                    foreach (DataRow PBRow in PBCustomColumns)
                    {
                        //list of column names of pb columns which have formula expression
                        if (!string.IsNullOrWhiteSpace(PBRow[ReconConstants.COLUMN_FormulaExpression].ToString()))
                        {
                            PBFormulaExpressionList.Add((string)PBRow[ReconConstants.COLUMN_Name]);
                        }
                        //following code commented in order to maintain different formula on pb side

                        //if ((string)NirvanaRow[ReconConstants.COLUMN_Name] == (string)PBRow[ReconConstants.COLUMN_Name])
                        //{
                        //    //update changed formula expression in nirvana table
                        //    if (NirvanaRow[ReconConstants.COLUMN_FormulaExpression] != PBRow[ReconConstants.COLUMN_FormulaExpression])
                        //    {
                        //        PBRow[ReconConstants.COLUMN_FormulaExpression] = NirvanaRow[ReconConstants.COLUMN_FormulaExpression];
                        //    }
                        //}
                        //delete the row from pb for which column name is updated in nirvana table
                        if (!NirvanaColumnName.Contains((string)PBRow[ReconConstants.COLUMN_Name]))
                        {
                            dtPBGridDisplayColumns.Rows.Remove(PBRow);
                            dtPBGridDisplayColumns.AcceptChanges();
                        }
                    }
                    //add the row to the pb table if it not exists for the formula expression
                    if (!PBFormulaExpressionList.Contains((string)NirvanaRow[ReconConstants.COLUMN_Name]))
                    {
                        dtPBGridDisplayColumns.Rows.Add(NirvanaRow.ItemArray);
                        //dtPBGridDisplayColumns.Rows[NirvanaRow.ItemArray[0]].ReadOnly = false;
                        dtPBGridDisplayColumns.AcceptChanges();
                        //add item to the available column list of report layout
                        ColumnInfo NirvanaItem = new ColumnInfo();
                        NirvanaItem.ColumnName = (string)NirvanaRow[ReconConstants.COLUMN_Name];
                        NirvanaItem.GroupType = ColumnGroupType.Nirvana;
                        NirvanaItem.FormulaExpression = (string)NirvanaRow[ReconConstants.COLUMN_FormulaExpression];
                        AvailableColumnList.Add(NirvanaItem);

                        ColumnInfo PBItem = new ColumnInfo();
                        PBItem.ColumnName = (string)NirvanaRow[ReconConstants.COLUMN_Name];
                        PBItem.GroupType = ColumnGroupType.PrimeBroker;
                        PBItem.FormulaExpression = (string)NirvanaRow[ReconConstants.COLUMN_FormulaExpression];
                        AvailableColumnList.Add(PBItem);
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
        }
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="template"></param>
        internal void LoadMasterColumns(ReconTemplate template)
        {
            try
            {
                _reconTemplate = template;
                if (_reconTemplate.DsMasterColumns != null)
                {
                    if (_reconTemplate.DsMasterColumns.Tables.Count > 0)
                    {
                        //if (template.ReconType == ReconType.Position)
                        //{

                        if (_reconTemplate.DsMasterColumns.Tables.Contains(ReconConstants.TABLENAME_NirvanaGridColumns))
                        {
                            //dtNirvanaMasterColumns = template.DsMasterColumns.Tables[ReconConstants.TABLENAME_NirvanaMasterColumns];
                            dtNirvanaGridDisplayColumns = _reconTemplate.DsMasterColumns.Tables[ReconConstants.TABLENAME_NirvanaGridColumns];

                            //if (!dtNirvanaMasterColumns.Columns.Contains("RowIndex"))
                            //{
                            //    dtNirvanaMasterColumns.Columns.Add("RowIndex");
                            //}
                            dtNirvanaGridDisplayColumns.AcceptChanges();
                            //dtNirvanaMasterColumns.AcceptChanges();
                            grdMasterColumnsNirvana.DataSource = dtNirvanaGridDisplayColumns;

                            //else
                            //{
                            //    grdMasterColumnsNirvana.DataSource = dtNirvanaMasterColumns;
                            //}
                        }
                        if (_reconTemplate.DsMasterColumns.Tables.Contains(ReconConstants.TABLENAME_PBGridMasterColumns))
                        {
                            //dtPBMasterColumns = template.DsMasterColumns.Tables[ReconConstants.TABLENAME_PBMasterColumns];
                            dtPBGridDisplayColumns = _reconTemplate.DsMasterColumns.Tables[ReconConstants.TABLENAME_PBGridMasterColumns];
                            //if (!dtPBMasterColumns.Columns.Contains("RowIndex"))
                            //{
                            //    dtPBMasterColumns.Columns.Add("RowIndex");
                            //}
                            dtPBGridDisplayColumns.AcceptChanges();
                            //dtPBMasterColumns.AcceptChanges();

                            grdMasterColumnsPB.DataSource = dtPBGridDisplayColumns;
                            //else
                            //{
                            //    grdMasterColumnsPB.DataSource = dtPBMasterColumns;
                            //}
                            //grdMasterColumnsPB.DataSource = template.DsMasterColumns.Tables[ReconConstants.TABLENAME_PBMasterColumns];
                        }
                        //this.AddCustomColumns(dtNirvanaGridDisplayColumns, dtPBGridDisplayColumns, template.AvailableColumnList);

                        //to add custom columns in pb table
                        // bool attribute is to add custom column in the avail list only for master table 

                        //this.AddCustomColumns(dtNirvanaMasterColumns, dtPBMasterColumns, template.AvailableColumnList,true);
                        //chkbxExactlyMatched.Checked = template.IsIncludeMatchedItems;
                        //chkbxMatchedinTol.Checked = template.IsIncludeToleranceMacthedItems;
                        //}
                        //else if (template.ReconType == ReconType.Transaction)
                        //{
                        //    if (template.DsMasterColumns.Tables.Contains(ReconConstants.TABLENAME_PBMasterColumns))
                        //    {
                        //        dtNirvanaMasterColumns = template.DsMasterColumns.Tables[ReconConstants.TABLENAME_PBMasterColumns];
                        //        grdMasterColumnsNirvana.DataSource = template.DsMasterColumns.Tables[ReconConstants.TABLENAME_PBMasterColumns];

                        //    }
                        //    if (template.DsMasterColumns.Tables.Contains(ReconConstants.TABLENAME_PBTrn))
                        //    {
                        //        dtPBMasterColumns = template.DsMasterColumns.Tables[ReconConstants.TABLENAME_PBTrn];
                        //        grdMasterColumnsPB.DataSource = template.DsMasterColumns.Tables[ReconConstants.TABLENAME_PBTrn];
                        //    }
                        //}

                        UltraGridBand bandNirvanaGrid = grdMasterColumnsNirvana.DisplayLayout.Bands[0];
                        UltraGridBand bandPBGrid = grdMasterColumnsPB.DisplayLayout.Bands[0];
                        SetGridView(bandNirvanaGrid);
                        SetGridView(bandPBGrid);
                    }
                    //else
                    //{
                    //    InitializeMasterColumnsTabForSelectedTemplate(template);
                    //}
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
        //modified by: sachin mishra 28 jan 2015
        //Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="template"></param>
        internal void UpdateMasterColumns(ReconTemplate template)
        {
            try
            {

                //if (grdMasterColumnsNirvana.DataSource != null)
                //{
                //UpdateRowIndex(grdMasterColumnsNirvana);
                //UpdateRowIndex(grdMasterColumnsPB);

                //if (rbCSVFormat.Checked) /// csv checked
                //{
                //    template.ExpReportFormat = AutomationEnum.FileFormate.csv;
                //}

                //else if (rbXLSFormat.Checked) // excel checked
                //{
                //    template.ExpReportFormat = AutomationEnum.FileFormate.xls;
                //}



                //grdMasterColumnsNirvana.UpdateData();
                //  grdMasterColumnsPB.UpdateData();
                this.AddCustomColumns(template.AvailableColumnList);
                DataSet dsMasterColumns = new DataSet();

                //dtNirvanaMasterColumns.AcceptChanges();
                dtNirvanaGridDisplayColumns.AcceptChanges();
                dtPBGridDisplayColumns.AcceptChanges();
                //dtPBMasterColumns.AcceptChanges();

                //dsMasterColumns.Tables.Add(dtNirvanaMasterColumns.Copy());
                //dsMasterColumns.Tables.Add(dtPBMasterColumns.Copy());
                dsMasterColumns.Tables.Add(dtNirvanaGridDisplayColumns.Copy());
                dsMasterColumns.Tables.Add(dtPBGridDisplayColumns.Copy());

                template.DsMasterColumns = dsMasterColumns;
                //add custom columns
                //template.IsIncludeMatchedItems = chkbxExactlyMatched.Checked;
                //template.IsIncludeToleranceMacthedItems = chkbxMatchedinTol.Checked;
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

        //private void grdMasterColumnsNirvana_SelectionDrag(object sender, CancelEventArgs e)
        //{
        //    grdMasterColumnsNirvana.DoDragDrop(grdMasterColumnsNirvana.Selected.Rows, DragDropEffects.Move);
        //}

        //private void grdMasterColumnsNirvana_DragDrop(object sender, DragEventArgs e)
        //{

        //    e.Effect = DragDropEffects.Move;
        //    UltraGrid grid = sender as UltraGrid;
        //    Point pointInGridCoords = grid.PointToClient(new Point(e.X, e.Y));

        //    if (pointInGridCoords.Y < 20)
        //        // Scroll up
        //        this.grdMasterColumnsNirvana.ActiveRowScrollRegion.Scroll(RowScrollAction.LineUp);
        //    else if (pointInGridCoords.Y > grid.Height - 20)
        //        // Scroll down
        //        this.grdMasterColumnsNirvana.ActiveRowScrollRegion.Scroll(RowScrollAction.LineDown);

        //}

        //private void grdMasterColumnsNirvana_DragOver(object sender, DragEventArgs e)
        //{
        //    int dropIndex;
        //    //Get the position on the grid where the dragged row(s) are to be dropped.
        //    //get the grid coordinates of the row (the drop zone)
        //    UIElement uieOver = grdMasterColumnsNirvana.DisplayLayout.UIElement.ElementFromPoint(
        //    grdMasterColumnsNirvana.PointToClient(new Point(e.X, e.Y)));
        //    //get the row that is the drop zone/or where the dragged row is to be dropped
        //    UltraGridRow ugrOver = uieOver.GetContext(typeof(UltraGridRow), true) as UltraGridRow;
        //    // ugrOver.Cells["RowIndex"] = 
        //    if (ugrOver != null)
        //    {
        //        dropIndex = ugrOver.Index;    //index/position of drop zone in grid

        //        //get the dragged row(s)which are to be dragged to another position in the grid
        //        SelectedRowsCollection SelRows = (SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection)) as
        //        SelectedRowsCollection;
        //        //get the count of selected rows and drop each starting at the dropIndex
        //        foreach (UltraGridRow aRow in SelRows)
        //        {
        //            //move the selected row(s) to the drop zone
        //            grdMasterColumnsNirvana.Rows.Move(aRow, dropIndex);
        //        }

        //    }

        //}
        private void grdMasterColumnsNirvana_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Row;
                //allow edit only the newly added cell which have formula expression not null
                row.Cells[ReconConstants.COLUMN_FormulaExpression].Activation = Activation.NoEdit;
                row.Cells[ReconConstants.COLUMN_Name].Activation = Activation.NoEdit;
                if (string.IsNullOrWhiteSpace(row.Cells[ReconConstants.COLUMN_Name].Text))
                {
                    row.Cells[ReconConstants.COLUMN_Name].Activation = Activation.AllowEdit;
                    row.Cells[ReconConstants.COLUMN_FormulaExpression].Activation = Activation.AllowEdit;
                }
                else if (string.IsNullOrWhiteSpace(row.Cells[ReconConstants.COLUMN_ReconId].Text))
                {
                    row.Cells[ReconConstants.COLUMN_FormulaExpression].Activation = Activation.AllowEdit;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMasterColumnsPB_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Row;
                row.Cells[ReconConstants.COLUMN_FormulaExpression].Activation = Activation.NoEdit;
                row.Cells[ReconConstants.COLUMN_Name].Activation = Activation.NoEdit;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        public bool IsUnsavedChanges()
        {
            bool isUnsavedChanges = _isUnsavedChanges;
            try
            {
                //if (dtNirvanaMasterColumns.GetChanges() != null)
                //{
                //    dtNirvanaMasterColumns.AcceptChanges();
                //    isUnsavedChanges = true;
                //}

                if (dtNirvanaGridDisplayColumns.GetChanges() != null)
                {
                    dtNirvanaGridDisplayColumns.AcceptChanges();
                    isUnsavedChanges = true;
                }

                //if (dtPBMasterColumns.GetChanges() != null)
                //{
                //    dtPBMasterColumns.AcceptChanges();
                //    isUnsavedChanges = true;
                //}

                if (dtPBGridDisplayColumns.GetChanges() != null)
                {
                    dtPBGridDisplayColumns.AcceptChanges();
                    isUnsavedChanges = true;
                }
                _isUnsavedChanges = false;

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
            return isUnsavedChanges;
        }

        //private void ultraLabel1_Click(object sender, EventArgs e)
        //{

        //}

        //private void chkbxExactlyMatched_CheckedChanged(object sender, EventArgs e)
        //{
        //   // _isUnsavedChanges = true;
        //}

        //private void chkbxMatchedinTol_CheckedChanged(object sender, EventArgs e)
        //{
        //   // _isUnsavedChanges = true;
        //}

        //private void chkbxExactlyMatched_Click(object sender, EventArgs e)
        //{
        //    _isUnsavedChanges = true;
        //}

        //private void chkbxMatchedinTol_Click(object sender, EventArgs e)
        //{
        //    _isUnsavedChanges = true;
        //}
        //custom row:to add new row in the Nirvana table

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addCustomColumn_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = dtNirvanaGridDisplayColumns.NewRow();
                row[ReconConstants.COLUMN_ISSelected] = false;
                row[ReconConstants.COLUMN_DataSourceType] = System.DBNull.Value;
                row[ReconConstants.COLUMN_Name] = System.DBNull.Value;
                row[ReconConstants.COLUMN_FormulaExpression] = System.DBNull.Value;

                //dtNirvanaGridDisplayColumns.Rows[28].Delete();
                dtNirvanaGridDisplayColumns.Rows.Add(row);
                dtNirvanaGridDisplayColumns.AcceptChanges();
                //else if (rbExceptionReport.Checked)
                //{
                //    DataRow row = dtNirvanaMasterColumns.NewRow();
                //    row[ReconConstants.COLUMN_ISSelected] = false;
                //    row[ReconConstants.COLUMN_ReconType] = System.DBNull.Value;
                //    row[ReconConstants.COLUMN_DataSourceType] = System.DBNull.Value;
                //    row[ReconConstants.COLUMN_Name] = System.DBNull.Value;
                //    row[ReconConstants.COLUMN_FormulaExpression] = System.DBNull.Value;
                //    row[ReconConstants.COLUMN_ISCommonColumn] = false;
                //    //dtNirvanaMasterColumns.Rows[28].Delete();
                //    dtNirvanaMasterColumns.Rows.Add(row);
                //    dtNirvanaMasterColumns.AcceptChanges();
                //}
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
        //not to leava blank name and formulae
        //private void grdMasterColumnsNirvana_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        //{
        //    if (e.CancellingEditOperation)
        //        return;
        //    if (this.grdMasterColumnsNirvana.ActiveCell.Text == "")
        //    {
        //        if (e.ForceExit)
        //        {
        //            // If the UltraGrid must exit the edit mode, then cancel the
        //            // cell update so the original value gets restored in the cell.
        //            this.grdMasterColumnsNirvana.ActiveCell.CancelUpdate();
        //            return;
        //        }
        //        e.Cancel = true;
        //    }
        //}

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editToolStripMenuItemNirvana_Click(object sender, EventArgs e)
        {
            try
            {
                this.grdMasterColumnsNirvana.ActiveRow.Cells[ReconConstants.COLUMN_FormulaExpression].Activation = Activation.AllowEdit;
                this.grdMasterColumnsNirvana.ActiveRow.Cells[ReconConstants.COLUMN_Name].Activation = Activation.AllowEdit;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItemNirvana_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable NirvanaTable = (DataTable)this.grdMasterColumnsNirvana.DataSource;
                DataTable PBTable = (DataTable)this.grdMasterColumnsPB.DataSource;
                String ColumnName = this.grdMasterColumnsNirvana.ActiveRow.Cells[ReconConstants.COLUMN_Name].Text;
                //return row of current selection in the grid
                DataRow[] NirvanaRow = NirvanaTable.Select(string.Format(ReconConstants.COLUMN_Name + " ='{0}'", ColumnName));
                DataRow[] PBRow = PBTable.Select(string.Format(ReconConstants.COLUMN_Name + " ='{0}'", ColumnName));
                if (NirvanaRow.Length == 0)
                {
                    this.grdMasterColumnsNirvana.ActiveRow.Delete();
                    _isUnsavedChanges = true;
                }
                if (NirvanaRow.Length > 0)
                {
                    //delete row from nirvana table
                    NirvanaTable.Rows.Remove(NirvanaRow[0]);
                    NirvanaTable.AcceptChanges();

                    //delete item if it is in exception report layout list
                    deleteCustomColumns(ColumnName, _reconTemplate.AvailableColumnList);
                    deleteCustomColumns(ColumnName, _reconTemplate.SelectedColumnList);
                    deleteCustomColumns(ColumnName, _reconTemplate.ListSortByColumns);
                    deleteCustomColumns(ColumnName, _reconTemplate.ListGroupByColumns);
                    _isUnsavedChanges = true;
                }
                if (PBRow.Length > 0)
                {
                    //delete row from PB table
                    PBTable.Rows.Remove(PBRow[0]);
                    PBTable.AcceptChanges();
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

        private void deleteCustomColumns(string ColumnName, List<ColumnInfo> ReconList)
        {
            try
            {
                ColumnInfo NirvanaItemToDelete = new ColumnInfo();
                ColumnInfo PBItemToDelete = new ColumnInfo();
                foreach (ColumnInfo item in ReconList)
                {
                    if (item.ColumnName.Equals(ColumnName) && item.GroupType == ColumnGroupType.Nirvana)
                        NirvanaItemToDelete = item;
                    if (item.ColumnName.Equals(ColumnName) && item.GroupType == ColumnGroupType.PrimeBroker)
                        PBItemToDelete = item;
                }
                if (NirvanaItemToDelete.ColumnName != null)
                    ReconList.Remove(NirvanaItemToDelete);
                if (PBItemToDelete.ColumnName != null)
                    ReconList.Remove(PBItemToDelete);
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

        private void grdMasterColumnsNirvana_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right && string.IsNullOrWhiteSpace(grdMasterColumnsNirvana.ActiveRow.Cells[ReconConstants.COLUMN_DataSourceType].Text))
                {
                    mnuAddCustomColumnsNirvana.Show(grdMasterColumnsNirvana, e.X, e.Y);
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

        private void grdMasterColumnsPB_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right && string.IsNullOrWhiteSpace(grdMasterColumnsPB.ActiveRow.Cells[ReconConstants.COLUMN_DataSourceType].Text))
                {
                    mnuAddCustomColumnsPB.Show(grdMasterColumnsPB, e.X, e.Y);
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
        //not to leave formula expression and column name blank
        private void grdMasterColumnsNirvana_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.OriginalValue != System.DBNull.Value)
                {
                    try
                    {
                        string previousValue = (string)e.Cell.OriginalValue;
                        string newValue = e.Cell.Text;
                        if (string.IsNullOrEmpty(newValue))
                        {
                            e.Cell.Value = previousValue;
                        }
                        //delete row from list as well
                        if (!((string)e.Cell.Column.Key == ReconConstants.COLUMN_FormulaExpression))
                        {
                            deleteCustomColumns(previousValue, _reconTemplate.AvailableColumnList);
                            deleteCustomColumns(previousValue, _reconTemplate.SelectedColumnList);
                            deleteCustomColumns(previousValue, _reconTemplate.ListSortByColumns);
                            deleteCustomColumns(previousValue, _reconTemplate.ListGroupByColumns);
                        }
                    }
                    catch (Exception)
                    { }
                    _isUnsavedChanges = true;
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

        private void grdMasterColumnsNirvana_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                _isUnsavedChanges = true;
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

        private void grdMasterColumnsPB_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                _isUnsavedChanges = true;
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

        private void editToolStripMenuItemPB_Click(object sender, EventArgs e)
        {
            try
            {
                this.grdMasterColumnsPB.ActiveRow.Cells[ReconConstants.COLUMN_FormulaExpression].Activation = Activation.AllowEdit;
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

        private void grdMasterColumnsPB_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.OriginalValue != System.DBNull.Value)
                {
                    try
                    {
                        string previousValue = (string)e.Cell.OriginalValue;
                        string newValue = e.Cell.Text;
                        if (string.IsNullOrEmpty(newValue))
                        {
                            e.Cell.Value = previousValue;
                        }
                        ////delete row from list as well
                        //if (!((string)e.Cell.Column.Key == ReconConstants.COLUMN_FormulaExpression))
                        //{
                        //    deleteCustomColumns(previousValue, currentTemplate.AvailableColumnList);
                        //    deleteCustomColumns(previousValue, currentTemplate.SelectedColumnList);
                        //    deleteCustomColumns(previousValue, currentTemplate.ListSortByColumns);
                        //    deleteCustomColumns(previousValue, currentTemplate.ListGroupByColumns);
                        //}
                    }
                    catch (Exception)
                    { }
                    _isUnsavedChanges = true;
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

        private void grdMasterColumnsNirvana_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdMasterColumnsNirvana);
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

        private void grdMasterColumnsNirvana_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdMasterColumnsPB_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdMasterColumnsPB_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdMasterColumnsPB);
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
