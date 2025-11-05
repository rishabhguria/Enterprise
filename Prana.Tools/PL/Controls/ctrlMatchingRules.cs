using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
//using Prana.Reconciliation;
using Prana.ReconciliationNew;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlMatchingRules : UserControl
    {
        #region CONTSANTS
        private const string MATCHING_RULE_FILE = "XmlMatchingRule.xml";
        private const string MATCHING_RULE_SCHEMA = "XmlMatchingRule.xsd";
        private const string COLUMN_Type = "Type";
        private const string COLUMN_IsIncluded = "IsIncluded";
        private const string COLUMN_Name = "Name";
        private const string COLUMN_IsRoundOff = "IsRoundOff";
        private const string COLUMN_RoundDigits = "RoundDigits";
        private const string COLUMN_IsIntegral = "IsIntegral";
        private const string COLUMN_IsPercentMatch = "IsPercentMatch";
        private const string COLUMN_ErrorTolerance = "ErrorTolerance";
        private const string COLUMN_IsAbsoluteMatch = "IsAbsoluteMatch";
        private const string COLUMN_AbsoluteDifference = "AbsoluteDifference";
        private const string COLUMN_IsVisible = "IsVisible";
        private const string COLUMN_mismatchExactReconColumn = "mismatchExactReconColumn";
        //private const string COLUMN_SP = "SP";
        private const string COLUMN_RECONTYPE = "ReconType";
        #endregion
        ReconTemplate _template = null;
        //bool isAlreadyInitialized = false;
        DataSet ds = null;
        bool _isUnSavedChanges = false;
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlMatchingRules()
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

        //public void InitializeMatchingRulesTabForSelectedTemplate(ReconTemplate template)
        //{
        //    #region FOR CREATING SCHEMA
        //    //DataSet ds1 = new DataSet();
        //    //ds.ReadXml(GetPath(MATCHING_RULE_FILE));
        //    //ds1 = ds.Clone();
        //    //CustomizeDataSet(ds1);
        //    //ds1.WriteXmlSchema(GetPath(MATCHING_RULE_SCHEMA));
        //    #endregion

        //    //if (!isAlreadyInitialized)
        //    //{
        //        ds = new DataSet();
        //        ds.ReadXmlSchema(GetPath(MATCHING_RULE_SCHEMA));
        //        ds.ReadXml(GetPath(MATCHING_RULE_FILE));
        //        RemoveRowsBasedOnTemplateReconType(template.ReconType);
        //        grdMatchingRules.DataSource = ds;
        //        grdMatchingRules.DataBind();
        //        grdMatchingRules.DataMember = ds.Tables[1].ToString();

        //        Infragistics.Win.UltraWinGrid.UltraGridBand band = grdMatchingRules.DisplayLayout.Bands[0];
        //        SetGridView(band);
        //        SetGridCellsView(grdMatchingRules);

        //        //isAlreadyInitialized = true;
        //    //}
        //    //else
        //    //{
        //    //    grdMatchingRules.DataSource = ds;
        //    //    grdMatchingRules.DataBind();
        //    //    grdMatchingRules.DataMember = ds.Tables[1].ToString();
        //    //}
        //}

        private void SetGridCellsView(UltraGrid grdMatchingRules)
        {
            try
            {
                foreach (UltraGridRow row in grdMatchingRules.Rows)
                {
                    if (Int32.Parse(row.Cells[COLUMN_Type].Value.ToString()) == 0)
                    {
                        row.Cells[COLUMN_IsRoundOff].Hidden = true;
                        row.Cells[COLUMN_RoundDigits].Hidden = true;
                        row.Cells[COLUMN_IsIntegral].Hidden = true;
                        row.Cells[COLUMN_IsPercentMatch].Hidden = true;
                        row.Cells[COLUMN_ErrorTolerance].Hidden = true;
                        row.Cells[COLUMN_IsAbsoluteMatch].Hidden = true;
                        row.Cells[COLUMN_AbsoluteDifference].Hidden = true;
                    }

                    if (string.IsNullOrEmpty(row.Cells[COLUMN_mismatchExactReconColumn].Value.ToString()))
                        row.Cells[COLUMN_mismatchExactReconColumn].Value = false;

                    if (!string.IsNullOrWhiteSpace(row.Cells[COLUMN_IsPercentMatch].Value.ToString()) && bool.Parse(row.Cells[COLUMN_IsPercentMatch].Value.ToString()) == false)
                    {
                        row.Cells[COLUMN_ErrorTolerance].Activation = Activation.Disabled;
                    }
                    if (!string.IsNullOrWhiteSpace(row.Cells[COLUMN_IsRoundOff].Value.ToString()) && bool.Parse(row.Cells[COLUMN_IsRoundOff].Value.ToString()) == false)
                    {
                        row.Cells[COLUMN_RoundDigits].Activation = Activation.Disabled;
                    }
                    if (!string.IsNullOrWhiteSpace(row.Cells[COLUMN_IsAbsoluteMatch].Value.ToString()) && bool.Parse(row.Cells[COLUMN_IsAbsoluteMatch].Value.ToString()) == false)
                    {
                        row.Cells[COLUMN_AbsoluteDifference].Activation = Activation.Disabled;
                    }
                    if (bool.Parse(row.Cells[COLUMN_IsIncluded].Value.ToString()) == false)
                    {
                        row.Cells[COLUMN_IsRoundOff].Hidden = true;
                        row.Cells[COLUMN_RoundDigits].Hidden = true;
                        row.Cells[COLUMN_IsIntegral].Hidden = true;
                        row.Cells[COLUMN_IsPercentMatch].Hidden = true;
                        row.Cells[COLUMN_ErrorTolerance].Hidden = true;
                        row.Cells[COLUMN_IsAbsoluteMatch].Hidden = true;
                        row.Cells[COLUMN_AbsoluteDifference].Hidden = true;
                        row.Cells[COLUMN_mismatchExactReconColumn].Value = false;
                        row.Cells[COLUMN_mismatchExactReconColumn].Activation = Activation.Disabled;
                    }
                    if (row.Cells[COLUMN_IsRoundOff].Hidden == false)
                    {
                        row.Cells[COLUMN_IsRoundOff].ToolTipText = "Set to true if rounded off matching is allowed.";
                    }
                    if (row.Cells[COLUMN_RoundDigits].Hidden == false)
                    {
                        row.Cells[COLUMN_RoundDigits].ToolTipText = "Set Round level";
                    }
                    if (row.Cells[COLUMN_IsIntegral].Hidden == false)
                    {
                        row.Cells[COLUMN_IsIntegral].ToolTipText = "Set to true if integral matching is allowed.";
                    }
                    if (row.Cells[COLUMN_IsPercentMatch].Hidden == false)
                    {
                        row.Cells[COLUMN_IsPercentMatch].ToolTipText = "Set to true if positions should be matched within a margin.";
                    }
                    if (row.Cells[COLUMN_ErrorTolerance].Hidden == false)
                    {
                        row.Cells[COLUMN_ErrorTolerance].ToolTipText = "Set % Tolerance level";
                    }
                    if (row.Cells[COLUMN_IsAbsoluteMatch].Hidden == false)
                    {
                        row.Cells[COLUMN_IsAbsoluteMatch].ToolTipText = "Set to true if absolute match is allowed.";
                    }
                    if (row.Cells[COLUMN_AbsoluteDifference].Hidden == false)
                    {
                        row.Cells[COLUMN_AbsoluteDifference].ToolTipText = "Set Allowed Difference";
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
        /// <param name="band"></param>
        private void SetGridView(Infragistics.Win.UltraWinGrid.UltraGridBand band)
        {
            try
            {
                #region Merge columns
                // Add six groups with different keys. 
                UltraGridGroup grpMatch = new UltraGridGroup();
                UltraGridGroup grpName = new UltraGridGroup();
                UltraGridGroup grpRoundOff = new UltraGridGroup();
                UltraGridGroup grpIntegral = new UltraGridGroup();
                UltraGridGroup grpPercentageTolerance = new UltraGridGroup();
                UltraGridGroup grpAbsoluteTolerance = new UltraGridGroup();
                UltraGridGroup grdExactmismatchReconcile = new UltraGridGroup();

                // Clear existing groups if any.
                band.Groups.Clear();
                band.Groups.Add(grpMatch);
                band.Groups.Add(grpName);
                band.Groups.Add(grpRoundOff);
                band.Groups.Add(grpIntegral);
                band.Groups.Add(grpPercentageTolerance);
                band.Groups.Add(grpAbsoluteTolerance);
                band.Groups.Add(grdExactmismatchReconcile);


                grpMatch.Columns.Add(band.Columns[COLUMN_IsIncluded]);
                grpMatch.Header.Caption = "Match";

                grpName.Columns.Add(band.Columns[COLUMN_Name]);
                grpName.Header.Caption = "Name";

                grpRoundOff.Columns.Add(band.Columns[COLUMN_IsRoundOff]);
                grpRoundOff.Columns.Add(band.Columns[COLUMN_RoundDigits]);
                grpRoundOff.Header.Caption = "Round Off";

                grpIntegral.Columns.Add(band.Columns[COLUMN_IsIntegral]);
                grpIntegral.Header.Caption = "Integral";

                grpPercentageTolerance.Columns.Add(band.Columns[COLUMN_IsPercentMatch]);
                grpPercentageTolerance.Columns.Add(band.Columns[COLUMN_ErrorTolerance]);
                grpPercentageTolerance.Header.Caption = "% Tolerance";

                grpAbsoluteTolerance.Columns.Add(band.Columns[COLUMN_IsAbsoluteMatch]);
                grpAbsoluteTolerance.Columns.Add(band.Columns[COLUMN_AbsoluteDifference]);
                grpAbsoluteTolerance.Header.Caption = "Absolute Tolerance";


                grdExactmismatchReconcile.Columns.Add(band.Columns[COLUMN_mismatchExactReconColumn]);
                grdExactmismatchReconcile.Header.Caption = "Exact Match Recon";

                // Prevet the users from moving groups and columns by setting AllowGroupMoving 
                // and AllowColMoving to NotAllowed.
                band.Override.AllowGroupMoving = AllowGroupMoving.NotAllowed;
                band.Override.AllowColMoving = AllowColMoving.NotAllowed;


                band.GroupHeadersVisible = true;
                band.ColHeadersVisible = false;
                #endregion

                band.Columns[COLUMN_Type].Hidden = true;

                band.Columns[COLUMN_IsIncluded].Header.VisiblePosition = 1;
                band.Columns[COLUMN_IsIncluded].Header.Caption = "Match";
                //band.Columns[COLUMN_IsIncluded].Width = 42;

                band.Columns[COLUMN_Name].Header.VisiblePosition = 2;
                band.Columns[COLUMN_Name].CellActivation = Activation.NoEdit;
                // band.Columns[COLUMN_Name].Width = 130;

                band.Columns[COLUMN_IsRoundOff].Header.VisiblePosition = 3;
                band.Columns[COLUMN_IsRoundOff].Header.Caption = "RoundOff";
                // band.Columns[COLUMN_IsRoundOff].Width = 55;

                band.Columns[COLUMN_RoundDigits].Header.VisiblePosition = 4;
                band.Columns[COLUMN_RoundDigits].Header.Caption = "RoundDigits";
                // band.Columns[COLUMN_RoundDigits].Width = 80;

                band.Columns[COLUMN_IsIntegral].Header.VisiblePosition = 5;
                band.Columns[COLUMN_IsIntegral].Header.Caption = "Integral";
                //band.Columns[COLUMN_IsIntegral].Width = 50;

                band.Columns[COLUMN_IsPercentMatch].Header.VisiblePosition = 6;
                band.Columns[COLUMN_IsPercentMatch].Header.Caption = "Is%Tolerance";
                //band.Columns[COLUMN_IsPercentMatch].Width = 75;

                band.Columns[COLUMN_ErrorTolerance].Header.VisiblePosition = 7;
                band.Columns[COLUMN_ErrorTolerance].Header.Caption = "%Tolerance";
                //band.Columns[COLUMN_ErrorTolerance].Width = 70;

                band.Columns[COLUMN_IsAbsoluteMatch].Header.VisiblePosition = 8;
                band.Columns[COLUMN_IsAbsoluteMatch].Header.Caption = "IsTolerance(Abs)";
                //band.Columns[COLUMN_IsAbsoluteMatch].Width = 85;

                band.Columns[COLUMN_AbsoluteDifference].Header.VisiblePosition = 9;
                band.Columns[COLUMN_AbsoluteDifference].Header.Caption = "AbsoluteTolerance";
                //band.Columns[COLUMN_AbsoluteDifference].Width = 100;

                band.Columns[COLUMN_mismatchExactReconColumn].Header.VisiblePosition = 10;
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

        #region FOR CREATING SCHEMA
        //private void CustomizeDataSet(DataSet ds1)
        //{
        //    if (ds1.Tables[1].Columns[COLUMN_IsIncluded] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_IsIncluded].DataType = typeof(bool);
        //    }
        //    if (ds1.Tables[1].Columns[COLUMN_Name] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_Name].DataType = typeof(string);
        //    }
        //    if (ds1.Tables[1].Columns[COLUMN_Type] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_Type].DataType = typeof(string);
        //    }
        //    if (ds1.Tables[1].Columns[COLUMN_IsPercentMatch] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_IsPercentMatch].DataType = typeof(bool);
        //    }
        //    if (ds1.Tables[1].Columns[COLUMN_IsRoundOff] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_IsRoundOff].DataType = typeof(bool);
        //    }
        //    if (ds1.Tables[1].Columns[COLUMN_IsIntegral] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_IsIntegral].DataType = typeof(bool);
        //    }
        //    if (ds1.Tables[1].Columns[COLUMN_ErrorTolerance] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_ErrorTolerance].DataType = typeof(double);
        //    }
        //    if (ds1.Tables[1].Columns[COLUMN_RoundDigits] != null)
        //    {
        //        ds1.Tables[1].Columns[COLUMN_RoundDigits].DataType = typeof(int);
        //    }
        //    if (ds1.Tables[0].Columns[COLUMN_IsVisible] != null)
        //    {
        //        ds1.Tables[0].Columns[COLUMN_IsVisible].DataType = typeof(bool);
        //    }
        //    if (ds1.Tables[0].Columns[COLUMN_mismatchExactReconColumn] != null)
        //    {
        //        ds1.Tables[0].Columns[COLUMN_mismatchExactReconColumn].DataType = typeof(bool);
        //    }
        //}
        #endregion

        //private void BindReconTypeCombo(DataTable dt)
        //{
        //    if (dt != null)
        //    {
        //cmbReconType.DataSource = null;
        //cmbReconType.DataSource = dt;
        //cmbReconType.ValueMember = dt.Columns[3].Caption;
        //cmbReconType.DisplayMember = dt.Columns[0].Caption;
        //cmbReconType.DataBind();
        //cmbReconType.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //cmbReconType.DisplayLayout.Bands[0].Columns[COLUMN_IsVisible].Hidden = true;
        //cmbReconType.DisplayLayout.Bands[0].Columns[COLUMN_SP].Hidden = true;
        //cmbReconType.DisplayLayout.Bands[0].ColumnFilters[COLUMN_IsVisible].FilterConditions.Add(FilterComparisionOperator.Equals, true);
        //if (dt.Rows.Count > 0)
        //{
        //    string value = dt.Rows[0].ItemArray[3].ToString();
        //    cmbReconType.Value = value;
        //}

        //    }
        //}

        //private string GetPath(string fileName)
        //{
        //    return Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString() + @"\" + fileName;
        //}

        //private void cmbReconType_ValueChanged(object sender, EventArgs e)
        //{
        //    if (cmbReconType.DataSource != null && cmbReconType.Value != null)
        //    {
        //        SetVisibleRow(cmbReconType.Value.ToString(), cmbReconType.ValueMember);
        //    }
        //}

        //unused method
        //modified by: sachin mishra 28 jan 2015
        //private void SetVisibleRow(string rowsToBind, string idColumn)
        //{
        //    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in grdMatchingRules.Rows)
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

        //public void SaveMatchingRuleXML()
        //{
        //    ds.WriteXml(GetPath(MATCHING_RULE_FILE));
        //}

        private void grdMatchingRules_CellChange(object sender, CellEventArgs e)
        {
            try
            {

                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == COLUMN_IsPercentMatch)
                {
                    if (bool.Parse(e.Cell.Text) == false)
                    {
                        row.Cells[COLUMN_ErrorTolerance].Activation = Activation.Disabled;
                    }
                    else
                    {
                        row.Cells[COLUMN_RoundDigits].Activation = Activation.Disabled;
                        row.Cells[COLUMN_ErrorTolerance].Activation = Activation.AllowEdit;
                        row.Cells[COLUMN_AbsoluteDifference].Activation = Activation.Disabled;
                        row.Cells[COLUMN_IsRoundOff].Value = false.ToString();
                        row.Cells[COLUMN_IsIntegral].Value = false.ToString();
                        row.Cells[COLUMN_IsAbsoluteMatch].Value = false.ToString();
                    }
                }
                if (e.Cell.Column.Key == COLUMN_IsRoundOff)
                {
                    if (bool.Parse(e.Cell.Text) == false)
                    {
                        row.Cells[COLUMN_RoundDigits].Activation = Activation.Disabled;
                    }
                    else
                    {
                        row.Cells[COLUMN_ErrorTolerance].Activation = Activation.Disabled;
                        row.Cells[COLUMN_RoundDigits].Activation = Activation.AllowEdit;
                        row.Cells[COLUMN_AbsoluteDifference].Activation = Activation.Disabled;
                        row.Cells[COLUMN_IsPercentMatch].Value = false.ToString();
                        row.Cells[COLUMN_IsIntegral].Value = false.ToString();
                        row.Cells[COLUMN_IsAbsoluteMatch].Value = false.ToString();
                    }
                }
                if (e.Cell.Column.Key == COLUMN_IsIntegral)
                {
                    if (bool.Parse(e.Cell.Text) == true)
                    {
                        row.Cells[COLUMN_IsPercentMatch].Value = false.ToString();
                        row.Cells[COLUMN_ErrorTolerance].Activation = Activation.Disabled;
                        row.Cells[COLUMN_AbsoluteDifference].Activation = Activation.Disabled;
                        row.Cells[COLUMN_IsRoundOff].Value = false.ToString();
                        row.Cells[COLUMN_RoundDigits].Activation = Activation.Disabled;
                        row.Cells[COLUMN_IsAbsoluteMatch].Value = false.ToString();
                    }
                }
                if (e.Cell.Column.Key == COLUMN_IsAbsoluteMatch)
                {
                    if (bool.Parse(e.Cell.Text) == false)
                    {
                        row.Cells[COLUMN_AbsoluteDifference].Activation = Activation.Disabled;
                    }
                    else
                    {
                        row.Cells[COLUMN_IsPercentMatch].Value = false.ToString();
                        row.Cells[COLUMN_ErrorTolerance].Activation = Activation.Disabled;
                        row.Cells[COLUMN_IsIntegral].Value = false.ToString();
                        row.Cells[COLUMN_IsRoundOff].Value = false.ToString();
                        row.Cells[COLUMN_RoundDigits].Activation = Activation.Disabled;
                        row.Cells[COLUMN_AbsoluteDifference].Activation = Activation.AllowEdit;
                    }
                }
                //Narendra Jangir 2012/08/20
                //Allow % tollerance in .123 format

                if (e.Cell.Column.Key == COLUMN_IsIncluded)
                {
                    if (bool.Parse(e.Cell.Text) == true && Int32.Parse(row.Cells[COLUMN_Type].Value.ToString()) == 2)
                    {
                        row.Cells[COLUMN_IsRoundOff].Hidden = false;
                        row.Cells[COLUMN_RoundDigits].Hidden = false;
                        row.Cells[COLUMN_IsIntegral].Hidden = false;
                        row.Cells[COLUMN_IsPercentMatch].Hidden = false;
                        row.Cells[COLUMN_ErrorTolerance].Hidden = false;
                        row.Cells[COLUMN_IsAbsoluteMatch].Hidden = false;
                        row.Cells[COLUMN_AbsoluteDifference].Hidden = false;
                    }
                    else
                    {
                        row.Cells[COLUMN_IsRoundOff].Hidden = true;
                        row.Cells[COLUMN_RoundDigits].Hidden = true;
                        row.Cells[COLUMN_IsIntegral].Hidden = true;
                        row.Cells[COLUMN_IsPercentMatch].Hidden = true;
                        row.Cells[COLUMN_ErrorTolerance].Hidden = true;
                        row.Cells[COLUMN_IsAbsoluteMatch].Hidden = true;
                        row.Cells[COLUMN_AbsoluteDifference].Hidden = true;
                    }

                    if (bool.Parse(e.Cell.Text) == true)
                        row.Cells[COLUMN_mismatchExactReconColumn].Activation = Activation.AllowEdit;
                    else
                    {
                        row.Cells[COLUMN_mismatchExactReconColumn].Value = false;
                        row.Cells[COLUMN_mismatchExactReconColumn].Activation = Activation.Disabled;
                    }
                }
                if (row.Cells[COLUMN_IsIntegral].Hidden == false)
                {
                    row.Cells[COLUMN_IsIntegral].ToolTipText = "Set to true if integral or rounded off matching is allowed.";
                }
                if (row.Cells[COLUMN_IsPercentMatch].Hidden == false)
                {
                    row.Cells[COLUMN_IsPercentMatch].ToolTipText = "Set to true if positions should be matched within a margin.";
                }
                if (row.Cells[COLUMN_ErrorTolerance].Hidden == false)
                {
                    row.Cells[COLUMN_ErrorTolerance].ToolTipText = "Set % Tolerance level";
                }

                ////To handle the case .xxxx value make it 0.xxxx
                //if (e.Cell.Column.Key != COLUMN_ErrorTolerance)
                //{
                //    grdMatchingRules.UpdateData();
                //}
                _isUnSavedChanges = true;

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

        internal void LoadMatchingRules(ReconTemplate template)
        {
            try
            {
                _template = template;
                if (template.DsMatchingRules != null)
                {
                    //CHMW-2783
                    //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare && string.IsNullOrEmpty(template.XsltPath))
                    if (string.IsNullOrEmpty(template.XsltPath))
                    {
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-2188
                        //[Recon]Third party file is not selected but matching rules grid display rules
                        grdMatchingRules.DataSource = null;
                        return;
                    }

                    if (template.DsMatchingRules.Tables.Count > 0 && template.DsMatchingRules.Tables[0].Rows.Count > 0)
                    {
                        template.VisibleRules = LoadVisibleColumns();


                        grdMatchingRules.DataSource = null;
                        ds = new DataSet();
                        ds = template.DsMatchingRules;

                        grdMatchingRules.DataSource = ds;
                        grdMatchingRules.DataMember = ds.Tables["Parameter"].ToString();


                        Infragistics.Win.UltraWinGrid.UltraGridBand band = grdMatchingRules.DisplayLayout.Bands[0];
                        SetGridView(band);
                        SetGridCellsView(grdMatchingRules);
                        ds.AcceptChanges();

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

        private List<string> LoadVisibleColumns()
        {
            try
            {
                List<string> visibleRules = new List<string>();
                string reconXSLTPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY
                               + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + @"\" + _template.XsltPath;
                visibleRules = XSLTReader.readNodesUnderGivenNodeName(reconXSLTPath, "PositionMaster");

                if (_template.DsCustomColumns.Tables.Contains(ReconConstants.CustomColumnsTableName))
                {
                    visibleRules.AddRange(_template.DsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].AsEnumerable().Select(s => s.Field<string>(ReconConstants.COLUMN_Name)).ToList<string>());
                }
                return visibleRules;
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
            return new List<string>();
        }

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="template"></param>
        internal void UpdateMatchingRules(ReconTemplate template)
        {
            try
            {
                //DataSet dsMatchingRules = (DataSet)grdMatchingRules.DataSource;
                if (ds != null)
                {
                    template.DsMatchingRules = ds.Copy();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMatchingRules_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                grdMatchingRules.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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

        //private void RemoveRowsBasedOnTemplateReconType(ReconType reconType)
        //{
        //    DataTable dtRules = ds.Tables[0];
        //    string idColumn = dtRules.Columns[3].Caption;
        //    string reconTypeColumn = dtRules.Columns[1].Caption;
        //    string rowsToBind = string.Empty;

        //    foreach (DataRow  dr  in dtRules.Rows)
        //    {
        //        if((int.Parse(dr[reconTypeColumn].ToString())==((int)reconType)))
        //        {
        //            rowsToBind = dr[idColumn].ToString();
        //            break;
        //        }
        //    }
        //    List<DataRow> listRowstoRemove = new List<DataRow>();
        //    foreach (DataRow row in ds.Tables[1].Rows)
        //    {
        //        if (row[idColumn].ToString() != rowsToBind)
        //        {
        //            listRowstoRemove.Add(row);
        //        }

        //    }

        //    foreach (DataRow rowtoRemove in listRowstoRemove)
        //    {
        //        ds.Tables[1].Rows.Remove(rowtoRemove);
        //    }

        //}

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        public bool IsUnsavedChanges()
        {
            try
            {
                if (ds != null && ds.HasChanges())
                {
                    ds.AcceptChanges();
                    return true;
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

            return _isUnSavedChanges;
        }



        /// <summary>
        ///  hides the rows which are not available in XSLT file form the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMatchingRules_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists(COLUMN_AbsoluteDifference))
                {
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1358
                    // Modified by Ankit Gupta on 29 Aug, 2014
                    e.Row.Cells[COLUMN_AbsoluteDifference].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
                }
                if (e.Row.Cells.Exists(COLUMN_RoundDigits))
                {
                    e.Row.Cells[COLUMN_RoundDigits].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerNonNegativeWithSpin;
                }
                if (e.Row.Cells.Exists(COLUMN_ErrorTolerance))
                {
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-2543
                    e.Row.Cells[COLUMN_ErrorTolerance].Column.MaskInput = "nn.nnnn";
                    e.Row.Cells[COLUMN_ErrorTolerance].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
                }
                //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                //{
                if (e.Row.Cells.Exists("Name") && _template.VisibleRules != null && _template.VisibleRules.Contains(e.Row.Cells["Name"].Value.ToString()))
                {
                    e.Row.Hidden = false;
                }
                else
                {
                    e.Row.Hidden = true;
                    if (e.Row.Cells.Exists("IsIncluded"))
                    {
                        e.Row.Cells["IsIncluded"].Value = false;
                    }
                }
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

        /// <summary>
        /// After Cell Update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMatchingRules_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            //added by amit on 11.03.2015
            //http://jira.nirvanasolutions.com:8080/browse/CHMW-2805
            try
            {
                if (e.Cell.Column.Key == COLUMN_AbsoluteDifference || e.Cell.Column.Key == COLUMN_ErrorTolerance || e.Cell.Column.Key == COLUMN_RoundDigits)
                {
                    if (e.Cell.Value.ToString() == String.Empty)
                    {
                        e.Cell.SetValue(0, true);
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
    }
}
