using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class CtrlSettings : UserControl
    {
        public CtrlSettings()
        {
            try
            {
                InitializeComponent();
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

        bool _isAlreadyStarted = false;
        string _viewName = string.Empty;
        private Dictionary<int, string> _dictUDAAssets = null;
        private Dictionary<int, string> _dictUDASectors = null;
        private Dictionary<int, string> _dictUDASubSectors = null;
        private Dictionary<int, string> _dictUDASecurityTypes = null;
        private Dictionary<int, string> _dictUDACountries = null;

        public void SetUp(string stepAnalViewName)
        {
            try
            {
                if (!_isAlreadyStarted)
                {
                    _viewName = stepAnalViewName;
                    BindGroupShocksGrid();
                    SetPreferences();
                    BindGroupFilterCombo();
                    _isAlreadyStarted = true;
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

        private void SetPreferences()
        {
            try
            {
                StepAnalysisPref preferences = RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(_viewName);
                if (preferences != null)
                {
                    chkBoxUseNonParallelShifts.Checked = preferences.UseNonParallelShifts;
                    chkBoxUseNonParallelShifts_CheckedChanged(null, null);
                    checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked = preferences.UseAbsoluteValuesForUnderlyingPrice;
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

        private void BindGroupShocksGrid()
        {
            try
            {
                StepAnalysisPref preferences = RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(_viewName);
                DataTable dtGroupShocks = GetDefaultGroupShocksDataTable();

                if (dtGroupShocks != null && dtGroupShocks.Rows.Count > 0)
                {
                    int rowsCountOld = dtGroupShocks.Rows.Count;
                    int rowsCountNew = 0;

                    if (preferences != null && preferences.DtGroupShocks != null)
                    {
                        dtGroupShocks.PrimaryKey = new DataColumn[] { dtGroupShocks.Columns["Group"], dtGroupShocks.Columns["PositionType"] };
                        dtGroupShocks.Merge(preferences.DtGroupShocks);
                        rowsCountNew = dtGroupShocks.Rows.Count;
                    }

                    //Bharat Kumar Jangir (12 December 2013)
                    //this loop removes extra rows which are added through preference file
                    for (int counter = rowsCountNew - 1; counter >= rowsCountOld; counter--)
                    {
                        dtGroupShocks.Rows[counter].Delete();
                    }

                    if (dtGroupShocks != null && dtGroupShocks.Rows.Count > 0)
                    {
                        grdGroupShocks.DataSource = dtGroupShocks;
                        grdGroupShocks.DataBind();

                        if (preferences != null && preferences.UseAbsoluteValuesForUnderlyingPrice)
                        {
                            grdGroupShocks.DisplayLayout.Bands[0].Columns["UnderlyingPriceShock"].Header.Caption = "Underlying Price";
                        }

                        grdGroupShocks.DisplayLayout.Bands[0].Columns["PositionType"].Hidden = true;
                        grdGroupShocks.DisplayLayout.Bands[0].Columns["PositionType"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        grdGroupShocks.DisplayLayout.Bands[0].ColumnFilters["PositionType"].FilterConditions.Add(FilterComparisionOperator.Equals, preferences.GroupShockFilter);
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

        private void BindGroupFilterCombo()
        {
            try
            {
                List<EnumerationValue> values = Prana.Utilities.UI.MiscUtilities.EnumHelper.ConvertEnumForBindingWithCaption(typeof(GroupShockBasis));
                StepAnalysisPref preferences = RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(_viewName);
                if (values != null)
                {
                    cmbFilter.DataSource = null;
                    cmbFilter.DataSource = values;
                    cmbFilter.DataBind();
                    cmbFilter.DisplayMember = "DisplayText";
                    cmbFilter.ValueMember = "Value";
                    cmbFilter.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                    cmbFilter.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    cmbFilter.DisplayLayout.Bands[0].Columns[0].Width = 158;
                }
                cmbFilter.Value = preferences.GroupShockFilter;
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

        private void grdGroupShocks_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {

                UltraGridLayout gridLayout = grdGroupShocks.DisplayLayout;

                UltraGridColumn columnPositionType = gridLayout.Bands[0].Columns["PositionType"];
                columnPositionType.CellClickAction = CellClickAction.RowSelect;
                columnPositionType.Header.Caption = "Position Type";

                UltraGridColumn columnGroup = gridLayout.Bands[0].Columns["Group"];
                columnGroup.CellClickAction = CellClickAction.RowSelect;

                UltraGridColumn columnVolShock = gridLayout.Bands[0].Columns["VolatilityShock"];
                columnVolShock.Header.Caption = "Volatility %";

                UltraGridColumn columnUnderlyingShock = gridLayout.Bands[0].Columns["UnderlyingPriceShock"];
                columnUnderlyingShock.Header.Caption = "Underlying Price %";


                UltraGridColumn columnIntRateShock = gridLayout.Bands[0].Columns["InteresRateShock"];
                columnIntRateShock.Header.Caption = "Interest Rate %";

                UltraGridColumn columnDaysToExpShock = gridLayout.Bands[0].Columns["DaysToExpShock"];
                columnDaysToExpShock.Header.Caption = "Days T+";

                gridLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
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

        public void SavePreferences()
        {
            try
            {
                RiskPrefernece riskpreference = RiskPreferenceManager.RiskPrefernece;
                StepAnalysisPref stepAnalpreferences = riskpreference.GetStepAnalViewPreferences(_viewName);
                stepAnalpreferences.UseAbsoluteValuesForUnderlyingPrice = checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked;
                DataTable dtNonParallelShifts = grdGroupShocks.DataSource as DataTable;
                if (dtNonParallelShifts != null)
                {
                    stepAnalpreferences.DtGroupShocks = dtNonParallelShifts;
                }
                stepAnalpreferences.UseNonParallelShifts = chkBoxUseNonParallelShifts.Checked;
                if (cmbFilter.Value != null)
                    stepAnalpreferences.GroupShockFilter = cmbFilter.Value.ToString();
                else
                    stepAnalpreferences.GroupShockFilter = string.Empty;
                riskpreference.UpdateStepAnalPrefDict(_viewName, stepAnalpreferences);
                RiskPreferenceManager.SavePreferences(riskpreference);

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

        private void chkBoxUseNonParallelShifts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!chkBoxUseNonParallelShifts.Checked)
                {
                    cmbFilter.Enabled = false;
                    grdGroupShocks.Enabled = false;
                    checkBoxUseAbsoluteValuesForUnderlyingPrice.Enabled = false;
                }
                else
                {
                    cmbFilter.Enabled = true;
                    grdGroupShocks.Enabled = true;
                    checkBoxUseAbsoluteValuesForUnderlyingPrice.Enabled = true;
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

        private void ApplyFilterToGrid(string positionType)
        {
            try
            {
                grdGroupShocks.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                grdGroupShocks.DisplayLayout.Bands[0].ColumnFilters["PositionType"].FilterConditions.Add(FilterComparisionOperator.Equals, positionType);
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

        private void checkBoxUseAbsoluteValuesForUnderlyingPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked)
                {
                    grdGroupShocks.DisplayLayout.Bands[0].Columns["UnderlyingPriceShock"].Header.Caption = "Underlying Price %";
                }
                else
                {
                    grdGroupShocks.DisplayLayout.Bands[0].Columns["UnderlyingPriceShock"].Header.Caption = "Underlying Price";
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

        private DataTable GetDefaultGroupShocksDataTable()
        {
            DataTable dt = new DataTable();
            try
            {
                SetDefaultTableAndSchema(dt);
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
            return dt;
        }

        private void SetDefaultTableAndSchema(DataTable dt)
        {
            try
            {
                dt.TableName = "GroupShocks";
                dt.Columns.Add("Group", typeof(string));
                dt.Columns.Add("PositionType", typeof(string));
                dt.Columns.Add("VolatilityShock", typeof(double));
                dt.Columns.Add("UnderlyingPriceShock", typeof(double));
                dt.Columns.Add("InteresRateShock", typeof(double));
                dt.Columns.Add("DaysToExpShock", typeof(int));

                string[] members = Enum.GetNames(typeof(PositionType));

                string[] positionTypes = Enum.GetNames(typeof(GroupShockBasis));

                foreach (string positionType in positionTypes)
                {
                    if (!positionType.Contains("UDA"))
                    {
                        foreach (string groupValue in members)
                        {
                            if (groupValue != string.Empty && groupValue != "None" && groupValue != "Multiple")
                            {
                                DataRow dr = GetDefaultRow(positionType, groupValue, dt);
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }

                #region UDA fields in Non Parallel shifts
                if (_dictUDAAssets != null && _dictUDAAssets.Count > 0)
                {
                    foreach (KeyValuePair<int, string> kvp in _dictUDAAssets)
                    {
                        DataRow row = GetDefaultRow(GroupShockBasis.UDAAssets.ToString(), kvp.Value, dt);
                        dt.Rows.Add(row);
                    }
                }
                if (_dictUDASectors != null && _dictUDASectors.Count > 0)
                {
                    foreach (KeyValuePair<int, string> kvp in _dictUDASectors)
                    {
                        DataRow row = GetDefaultRow(GroupShockBasis.UDASectors.ToString(), kvp.Value, dt);
                        dt.Rows.Add(row);
                    }
                }
                if (_dictUDASubSectors != null && _dictUDASubSectors.Count > 0)
                {
                    foreach (KeyValuePair<int, string> kvp in _dictUDASubSectors)
                    {
                        DataRow row = GetDefaultRow(GroupShockBasis.UDASubSectors.ToString(), kvp.Value, dt);
                        dt.Rows.Add(row);
                    }
                }
                if (_dictUDASecurityTypes != null && _dictUDASecurityTypes.Count > 0)
                {
                    foreach (KeyValuePair<int, string> kvp in _dictUDASecurityTypes)
                    {
                        DataRow row = GetDefaultRow(GroupShockBasis.UDASecurityTypes.ToString(), kvp.Value, dt);
                        dt.Rows.Add(row);
                    }
                }
                if (_dictUDACountries != null && _dictUDACountries.Count > 0)
                {
                    foreach (KeyValuePair<int, string> kvp in _dictUDACountries)
                    {
                        DataRow row = GetDefaultRow(GroupShockBasis.UDACountries.ToString(), kvp.Value, dt);
                        dt.Rows.Add(row);
                    }
                }
                #endregion
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

        private DataRow GetDefaultRow(string positionType, string groupValue, DataTable dt)
        {
            DataRow row = dt.NewRow();
            try
            {
                row["Group"] = groupValue;
                row["PositionType"] = positionType;
                row["VolatilityShock"] = 0;
                row["UnderlyingPriceShock"] = 0;
                row["InteresRateShock"] = 0;
                row["DaysToExpShock"] = 0;
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
            return row;
        }

        public void SetUpGrid(Dictionary<string, Dictionary<int, string>> UDAData)
        {
            try
            {
                if (UDAData != null)
                {
                    SetUpUDAFields(UDAData);
                }
                BindGroupShocksGrid();
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

        private void SetUpUDAFields(Dictionary<string, Dictionary<int, string>> UDADataCol)
        {
            try
            {
                foreach (KeyValuePair<string, Dictionary<int, string>> UDAData in UDADataCol)
                {
                    switch (UDAData.Key)
                    {
                        case SecMasterConstants.CONST_UDASector:
                            _dictUDASectors = UDAData.Value;
                            break;

                        case SecMasterConstants.CONST_UDASecurityType:
                            _dictUDASecurityTypes = UDAData.Value;
                            break;

                        case SecMasterConstants.CONST_UDASubSector:
                            _dictUDASubSectors = UDAData.Value;
                            break;

                        case SecMasterConstants.CONST_UDAAsset:
                            _dictUDAAssets = UDAData.Value;
                            break;

                        case SecMasterConstants.CONST_UDACountry:
                            _dictUDACountries = UDAData.Value;
                            break;
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

        private void cmbFilter_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFilter.Value != null)
                    ApplyFilterToGrid(cmbFilter.Value.ToString());
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
    }
}
