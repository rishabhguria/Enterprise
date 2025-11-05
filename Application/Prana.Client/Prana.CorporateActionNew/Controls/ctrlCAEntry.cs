using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.CorporateActionNew.Classes;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana.CorporateActionNew.Controls
{
    public partial class ctrlCAEntry : UserControl
    {
        private ControlType _controlType;

        CorporateActionType _caType = CorporateActionType.NameChange;
        public event EventHandler CorporateActionModified;
        Dictionary<Guid, DataRow> _selectedCAIDList = new Dictionary<Guid, DataRow>();

        public ctrlCAEntry()
        {
            InitializeComponent();
        }

        private DataTable _caTable;
        internal DataTable CATable
        {
            get { return _caTable; }
        }

        internal void ClearCATable()
        {
            try
            {
                _caTable.Clear();
                if (_controlType == ControlType.Apply && Enum.IsDefined(typeof(CorporateActionType), _caType))
                {
                    AddRowToCATable();
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

        internal void ClearSelectedCAList()
        {
            _selectedCAIDList.Clear();
        }

        internal void ClearSelectedCAs()
        {
            try
            {
                foreach (KeyValuePair<Guid, DataRow> keyValue in _selectedCAIDList)
                {
                    if (_caTable.Rows.Contains(keyValue.Key))
                    {
                        DataRow row = _caTable.Rows.Find(keyValue.Key);
                        row.Delete();
                    }
                }
                _caTable.AcceptChanges();
                _selectedCAIDList.Clear();
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

        //internal void AssignCATable(DataTable caTable)
        //{
        //    try
        //    {
        //        caTable.PrimaryKey = new DataColumn[] { caTable.Columns[CorporateActionConstants.CONST_CorporateActionId] };
        //        _caTable = caTable;
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
        ISecurityMasterServices _smServices = null;

        internal void InitControl(CorporateActionType caType, ControlType controlType, ISecurityMasterServices smServices)
        {
            try
            {
                _smServices = smServices;
                _smServices.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_smServices_SecMstrDataResponse);
                //new SecMasterDataHandler(_smServices_SecMstrDataResponse);
                _smServices.SymbolLookUpDataResponse += new EventHandler<EventArgs<DataSet>>(_smServices_SymbolLookUpDataResponse);
                //new SymbolLookUpDataResponse(_smServices_SymbolLookUpDataResponse);
                _controlType = controlType;
                switch (controlType)
                {
                    case ControlType.Apply:
                        mnuPreviewUndo.Visible = false;
                        mnuPreviewRedo.Visible = false;
                        break;
                    case ControlType.Undo:
                        mnuPreviewUndo.Visible = true;
                        mnuPreviewRedo.Visible = false;
                        break;
                    case ControlType.Redo:
                        mnuPreviewUndo.Visible = false;
                        mnuPreviewRedo.Visible = true;
                        break;
                }

                _caType = caType;

                SetupBinding();
                InitGridSettings();
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

        private void AssignCompanyName(SecMasterBaseObj secMasterObj)
        {
            if (_controlType == ControlType.Apply || _controlType == ControlType.Redo)
            {
                if (_caTable.Rows.Count > 0)
                {
                    if (_caTable.Rows[0][CorporateActionConstants.CONST_OrigSymbolTag].ToString().Equals(secMasterObj.TickerSymbol))
                    {
                        _caTable.Rows[0][CorporateActionConstants.CONST_CompanyName] = secMasterObj.LongName; ;
                    }
                    else if (_caTable.Rows[0][CorporateActionConstants.CONST_NewSymbolTag].ToString().Equals(secMasterObj.TickerSymbol))
                    {
                        _caTable.Rows[0][CorporateActionConstants.CONST_NewCompanyName] = secMasterObj.LongName; ;
                    }
                }
            }
        }

        //JIRA ISSUE 1499: CHECK FOR INVALID DATE, IF FOUND, IT  IS REVERTED TO CURRENT DATE
        void _caTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                if (e.Column.DataType.Equals(typeof(DateTime)))
                {
                    DateTime mindate = DateTimeConstants.MinValue;
                    DateTime value = Convert.ToDateTime(e.ProposedValue);

                    if (value < mindate)
                    {
                        MessageBox.Show("Entered Date cannot be less than 1/1/1800, it would be reverted back to currentdate.", "Corporate Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        grdCorporateActionEntry.DisplayLayout.Rows[0].Cells[e.Column.ColumnName].Value = DateTime.Now.Date;
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


        private void SetupBinding()
        {
            try
            {
                _caTable = XMLCacheManager.Instance.GetFullCATable();
                grdCorporateActionEntry.DataSource = _caTable;
                _caTable.ColumnChanged += new DataColumnChangeEventHandler(_caTable_ColumnChanged);
                BindSymbology();
                BindCashinLieuCombo();
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

        //private void RefreshGrid()
        //{
        //    try
        //    {
        //        grdCorporateActionEntry.Refresh();
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


        private void InitGridSettings()
        {
            try
            {
                UltraGridBand band = this.grdCorporateActionEntry.DisplayLayout.Bands[0];

                band.Override.RowSpacingAfter = 7;
                band.Override.CellSpacing = 7;
                // Turn on the row layout functionality for Table1 band. 
                //band.UseRowLayout = true;
                band.RowLayoutStyle = RowLayoutStyle.ColumnLayout;
                // Create a new row layout with "RowLayout1" as the key. 
                band.RowLayouts.Clear();
                RowLayout rowLayout1 = band.RowLayouts.Add("RowLayout1");

                if (_controlType == ControlType.Apply)
                {
                    rowLayout1.CardView = true;
                    rowLayout1.RowLayoutLabelStyle = RowLayoutLabelStyle.Separate;
                    // CardViewStyle only applies in card-view. 
                    rowLayout1.CardViewStyle = CardStyle.StandardLabels;
                    rowLayout1.RowLayoutLabelPosition = LabelPosition.Left;
                    band.Override.HeaderAppearance.BorderAlpha = Alpha.Transparent;
                    band.Override.HeaderAppearance.FontData.Bold = DefaultableBoolean.True;
                }
                else
                {
                    band.Override.RowSpacingAfter = 0;
                    band.Override.CellSpacing = 0;
                    band.Override.HeaderAppearance.BorderAlpha = Alpha.Default;


                    band.Override.HeaderAppearance.ForeColor = Color.Black;
                    band.Override.HeaderAppearance.BackColor = SystemColors.Control;
                    band.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;

                    band.Override.AllowColSizing = AllowColSizing.Free;
                    band.Override.AllowColSwapping = AllowColSwapping.WithinBand;

                    band.Override.AllowRowLayoutCellSizing = RowLayoutSizing.None;
                    //band.Override.ActiveRowAppearance.BackColor = Color.Gray;
                    band.Override.CellClickAction = CellClickAction.Edit;
                    //band.Override.RowAppearance.TextHAlign = HAlign.Center;
                    band.Override.RowAppearance.BackColor = Color.Black;
                    band.Override.RowAppearance.ForeColor = Color.White;

                    band.Override.RowAlternateAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    band.Override.RowAlternateAppearance.ForeColor = System.Drawing.Color.White;
                    //band.Override.RowAlternateAppearance.TextHAlignAsString = "Right";
                    //band.Override.RowAlternateAppearance.TextVAlignAsString = "Middle";

                    band.Override.SelectedRowAppearance.BackColor = Color.Transparent;
                    band.Override.SelectedRowAppearance.BorderColor = Color.Transparent;
                    band.Override.SelectedRowAppearance.FontData.Bold = DefaultableBoolean.True;

                    band.Override.ActiveRowAppearance.BackColor = Color.LightSlateGray;
                    band.Override.ActiveRowAppearance.BackColor2 = Color.DarkSlateGray;
                    band.Override.ActiveRowAppearance.BackGradientStyle = GradientStyle.VerticalBump;
                    band.Override.ActiveRowAppearance.BorderColor = Color.DimGray;
                    band.Override.ActiveRowAppearance.FontData.Bold = DefaultableBoolean.True;
                    band.Override.ActiveRowAppearance.ForeColor = Color.White;

                    //We will add an additional column by the name Select for undo and redo grid.
                    band.Columns.Add(ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT);
                    band.Columns[ApplicationConstants.C_COMBO_SELECT].DataType = typeof(System.Boolean);
                    band.Columns[ApplicationConstants.C_COMBO_SELECT].CellActivation = Activation.AllowEdit;
                    band.Columns[ApplicationConstants.C_COMBO_SELECT].CellClickAction = CellClickAction.Edit;
                    band.Columns[ApplicationConstants.C_COMBO_SELECT].Header.VisiblePosition = 0;

                    //Grid.creationfilter
                    //CheckBoxOnHeader_CreationFilter

                    band.Override.AllowRowFiltering = DefaultableBoolean.True;
                    band.Override.HeaderClickAction = HeaderClickAction.SortSingle;
                    band.Override.HeaderStyle = HeaderStyle.Default;

                }
                AssignColumnPropertiesForCA(_caType);
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

        internal void FilterForCA(CorporateActionType caType)
        {
            try
            {
                // If the row filter mode is band based, then get the column filters off the band
                ColumnFiltersCollection columnFilters = grdCorporateActionEntry.DisplayLayout.Bands[0].ColumnFilters;
                ///TODO : When we apply the custom filters we need to change the code so the filters won't be on a common field
                columnFilters.ClearAllFilters();
                if (caType == CorporateActionType.All)
                {
                    return;
                }
                else
                {
                    if (grdCorporateActionEntry.DataSource != null)
                    {
                        columnFilters[CorporateActionConstants.CONST_CorporateActionType].FilterConditions.Add(FilterComparisionOperator.Equals, Convert.ToInt32(caType));
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

        internal void AssignColumnPropertiesForCA(CorporateActionType caType)
        {
            try
            {
                _caType = caType;
                UltraGridBand band = this.grdCorporateActionEntry.DisplayLayout.Bands[0];
                RowLayout rowLayout = band.RowLayouts[0];
                if (rowLayout != null)
                {
                    ///Hide all columns by default
                    foreach (UltraGridColumn col in band.Columns)
                    {
                        col.Hidden = true;
                    }

                    if (_controlType != ControlType.Apply)
                    {
                        //Check box select column will always be visible.
                        band.Columns[ApplicationConstants.C_COMBO_SELECT].Hidden = false;
                    }

                    XMLCacheManager.Instance.AssignColumnPropertiesForCA(_caType, rowLayout, _controlType);
                }
                rowLayout.Apply();
                // This is workarround of the issue where columns were not displayed on proper positions. 
                //http://www.infragistics.com/membership/mysupport.aspx?CaseNumber=CAS-30101-VAKKAJ
                //band.UseRowLayout = true;
                band.RowLayoutStyle = RowLayoutStyle.ColumnLayout;
                ClearCATable();
                ClearSelectedCAList();
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

        private DataRow AddRowToCATable()
        {
            DataRow datarow = null;
            try
            {
                datarow = _caTable.NewRow();
                XMLCacheManager.Instance.InitializeCARow(_caType, datarow);
                _caTable.Rows.Add(datarow);
                _caTable.AcceptChanges();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return datarow;
        }

        //TODO : Check if we want to bind enums then how can we do it dynamically using xml.
        private void BindSymbology()
        {
            try
            {
                UltraGridColumn colSymbolgy = grdCorporateActionEntry.DisplayLayout.Bands[0].Columns[CorporateActionConstants.CONST_SymbologyID];

                string[] symbolgyNames = Enum.GetNames(typeof(ApplicationConstants.SymbologyCodes));
                colSymbolgy.ValueList = GetValueList(symbolgyNames, true);
                colSymbolgy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colSymbolgy.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
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

        private void BindCashinLieuCombo()
        {
            try
            {
                UltraGridColumn colCashinlieu = grdCorporateActionEntry.DisplayLayout.Bands[0].Columns[CorporateActionConstants.CONST_Cashinlieu];

                string[] CashInTypes = Enum.GetNames(typeof(ApplicationConstants.CashInLieu));
                colCashinlieu.ValueList = GetValueList(CashInTypes, false);
                colCashinlieu.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colCashinlieu.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
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

        private ValueList GetValueList(string[] array, bool isSymbologyBinding)
        {
            ValueList valueList = new ValueList();
            try
            {
                if (isSymbologyBinding)
                {
                    valueList.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);

                    int i = 0;
                    foreach (string name in array)
                    {
                        valueList.ValueListItems.Add(i, name.Replace("Symbol", ""));
                        i++;
                        // IMPORTANT: Remove this if when all symbolgies are being used, right now only ticker
                        if (i == 1)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        valueList.ValueListItems.Add(i, array[i]);
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
            return valueList;
        }

        private bool _isAllowed = true;
        /// <summary>
        /// If the new symbol/company name already exist, this flag won't allow the preview/saving the CA
        /// </summary>
        public bool ISAllowed
        {
            get { return _isAllowed; }
        }

        SecMasterConstants.SecurityActions _symbolAction = SecMasterConstants.SecurityActions.SEARCH;

        internal string GetNewSymbol()
        {
            return _caTable.Rows[0][CorporateActionConstants.CONST_NewSymbolTag].ToString();
        }

        internal SecMasterConstants.SecurityActions GetSymbolAction()
        {
            return _symbolAction;
        }

        internal event EventHandler AfterCellUpdate;
        private void grdCorporateActionEntry_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (AfterCellUpdate != null)
                    AfterCellUpdate(this, EventArgs.Empty);
                _isAllowed = true;
                if ((_controlType == ControlType.Apply) || (_controlType == ControlType.Redo))
                {
                    switch (e.Cell.Column.Key)
                    {
                        case CorporateActionConstants.CONST_OrigSymbolTag:

                            string symbol = _caTable.Rows[0][CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                            if (_smServices != null && _smServices.IsConnected && !String.IsNullOrEmpty(symbol))
                            {
                                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                reqObj.AddData(symbol, ApplicationConstants.PranaSymbology);
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                reqObj.HashCode = this.GetHashCode();
                                _smServices.SendRequest(reqObj);
                            }

                            break;
                        case CorporateActionConstants.CONST_NewSymbolTag:

                            string newSymbol = _caTable.Rows[0][CorporateActionConstants.CONST_NewSymbolTag].ToString();
                            if (_smServices != null && _smServices.IsConnected && !String.IsNullOrEmpty(newSymbol))
                            {
                                _symbolAction = SecMasterConstants.SecurityActions.ADD;

                                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                reqObj.AddData(newSymbol, ApplicationConstants.PranaSymbology);
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled; ;
                                reqObj.HashCode = this.GetHashCode();
                                _smServices.SendRequest(reqObj);
                            }

                            break;
                        case CorporateActionConstants.CONST_Cashinlieu:
                            int cashinlieuType;
                            int.TryParse(_caTable.Rows[0][CorporateActionConstants.CONST_Cashinlieu].ToString(), out cashinlieuType);
                            if (cashinlieuType.Equals((int)ApplicationConstants.CashInLieu.CloseAtGivenPrice))
                            {
                                grdCorporateActionEntry.DisplayLayout.Bands[0].RowLayouts[0].ColumnInfos[CorporateActionConstants.CONST_ClosingPrice].Column.CellActivation = Activation.AllowEdit;
                            }
                            else
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_ClosingPrice] = 0d;
                                grdCorporateActionEntry.DisplayLayout.Bands[0].RowLayouts[0].ColumnInfos[CorporateActionConstants.CONST_ClosingPrice].Column.CellActivation = Activation.NoEdit;
                            }
                            break;
                        //case CorporateActionConstants.CONST_ExchangeRatio:

                        //    double exchangeRatio;
                        //    Double.TryParse(e.Cell.Text, out exchangeRatio);

                        //    if (exchangeRatio != 0.0)
                        //    {
                        //        _caTable.Rows[0][CorporateActionConstants.CONST_ExchangeRatio] = (1 - exchangeRatio) * 100;
                        //    }
                        //    break;

                        default:
                            break;
                    }
                }

                if (CorporateActionModified != null)
                {
                    CorporateActionModified(this, null);
                }


            }
            catch (Exception ex)
            {
                _isAllowed = true;

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        void _smServices_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (_controlType == ControlType.Apply || (_controlType == ControlType.Redo))
                {
                    _symbolAction = SecMasterConstants.SecurityActions.SEARCH;

                    AssignCompanyName(e.Value);
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

        void _smServices_SymbolLookUpDataResponse(object sender, EventArgs<DataSet> e)
        {
            try
            {
                DataSet ds = e.Value;
                //TODO : Need to apply this check for redo tab as well
                if (_controlType == ControlType.Apply)
                {
                    //Check Added By : Manvendra Prajapai
                    // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3381
                    // Date : 17-Apr-2015
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        _isAllowed = false;
                        //MessageBox.Show("NewSymbol or NewCompanyName is already present in the database. Please check if you are putting the right entry!", "Corporate Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        _isAllowed = true;
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


        private void grdCorporateActionEntry_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (_controlType != ControlType.Apply)
                {
                    switch (e.Cell.Column.Key)
                    {
                        case ApplicationConstants.C_COMBO_SELECT:

                            DataRow selectedCARow = ((DataRowView)e.Cell.Row.ListObject).Row;
                            Guid caID = (Guid)selectedCARow[CorporateActionConstants.CONST_CorporateActionId];
                            if (caID != null && caID != Guid.Empty)
                            {
                                string isSelected = e.Cell.Text.ToString().ToUpper();
                                if (isSelected.Equals("TRUE"))
                                {
                                    if (!_selectedCAIDList.ContainsKey(caID))
                                    {
                                        _selectedCAIDList.Add(caID, selectedCARow);
                                    }
                                }
                                else
                                {
                                    if (_selectedCAIDList.ContainsKey(caID))
                                    {
                                        _selectedCAIDList.Remove(caID);
                                    }
                                }
                            }
                            break;

                        case CorporateActionConstants.CONST_NewSecQtyRatio:
                            double origQty;
                            double newQty;
                            Double.TryParse(_caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio].ToString(), out origQty);
                            Double.TryParse(e.Cell.Text, out newQty);

                            if (origQty != 0.0 && newQty != 0.0)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_Factor] = newQty / origQty;
                            }
                            break;

                        case CorporateActionConstants.CONST_OrigSecQtyRatio:
                            double origQty1;
                            double newQty1;
                            Double.TryParse(e.Cell.Text, out origQty1);
                            Double.TryParse(_caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio].ToString(), out newQty1);

                            if (origQty1 != 0.0 && newQty1 != 0.0)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_Factor] = newQty1 / origQty1;
                            }
                            break;

                        case CorporateActionConstants.CONST_Factor:

                            double factor;

                            Double.TryParse(e.Cell.Text, out factor);

                            if (factor != 0.0 && factor > 1)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio] = 1;
                                _caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio] = factor;
                            }
                            else if (factor != 0.0 && factor <= 1)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio] = Math.Round(1 / factor, 4);
                                _caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio] = 1;
                            }
                            else
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio] = 0.0;
                                _caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio] = 0.0;
                            }
                            break;
                    }
                }
                else
                {
                    switch (e.Cell.Column.Key)
                    {
                        case CorporateActionConstants.CONST_NewSecQtyRatio:
                            double origQty;
                            double newQty;
                            Double.TryParse(_caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio].ToString(), out origQty);
                            Double.TryParse(e.Cell.Text, out newQty);

                            if (origQty != 0.0 && newQty != 0.0)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_Factor] = newQty / origQty;
                            }
                            break;

                        case CorporateActionConstants.CONST_OrigSecQtyRatio:
                            double origQty1;
                            double newQty1;
                            Double.TryParse(e.Cell.Text, out origQty1);
                            Double.TryParse(_caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio].ToString(), out newQty1);

                            if (origQty1 != 0.0 && newQty1 != 0.0)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_Factor] = newQty1 / origQty1;
                            }
                            break;
                        case CorporateActionConstants.CONST_Factor:

                            double factor;

                            Double.TryParse(e.Cell.Text, out factor);

                            if (factor != 0.0 && factor > 1)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio] = 1;
                                _caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio] = factor;
                            }
                            else if (factor != 0.0 && factor <= 1)
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio] = Math.Round(1 / factor, 4);
                                _caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio] = 1;
                            }
                            else
                            {
                                _caTable.Rows[0][CorporateActionConstants.CONST_OrigSecQtyRatio] = 0.0;
                                _caTable.Rows[0][CorporateActionConstants.CONST_NewSecQtyRatio] = 0.0;
                            }
                            break;
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

        internal void TransformAndBindTable(DataTable dbCATable)
        {
            try
            {
                ClearCATable();
                ClearSelectedCAList();

                foreach (DataRow dr in dbCATable.Rows)
                {
                    DataRow row = AddRowToCATable();

                    string corpActionStr = dr["CorporateAction"].ToString();
                    XMLCacheManager.FillDataRowFromXML(row, corpActionStr);

                    //Transformer.FillCARowFromString(dr["CorporateAction"].ToString(), row);
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

        internal void TransformAndBindTable(DataRowCollection rows)
        {
            try
            {
                ClearCATable();
                ClearSelectedCAList();

                foreach (DataRow inputRow in rows)
                {
                    DataRow row = AddRowToCATable();
                    CommonHelper.FillCARowFromInputRow(inputRow, row);
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

        internal event EventHandler OnPreviewUndoClick;
        internal event EventHandler OnPreviewRedoClick;


        private void mnuPreviewUndo_Click(object sender, EventArgs e)
        {
            if (OnPreviewUndoClick != null)
            {
                OnPreviewUndoClick(this, EventArgs.Empty);
            }
        }

        private void mnuPreviewRedo_Click(object sender, EventArgs e)
        {
            if (OnPreviewRedoClick != null)
            {
                OnPreviewRedoClick(this, EventArgs.Empty);
            }
        }

        internal Dictionary<string, DateTime> GetSelectedCAIDsWithDates()
        {
            Dictionary<string, DateTime> caIDsWithDates = null;
            try
            {
                if (_selectedCAIDList != null && _selectedCAIDList.Count > 0)
                {
                    caIDsWithDates = new Dictionary<string, DateTime>();
                    foreach (KeyValuePair<Guid, DataRow> selectedCA in _selectedCAIDList)
                    {
                        caIDsWithDates.Add(selectedCA.Key.ToString(), Convert.ToDateTime(Convert.ToDateTime(selectedCA.Value[0])));
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

            return caIDsWithDates;
        }
        /// <summary>
        /// Function To get all the symbols which are selected for undo
        /// </summary>
        /// <returns>String of symbols, comma seperated</returns>
        internal string getCaSymbols()
        {
            try
            {

                StringBuilder joinedSymbols = new StringBuilder();

                if (!_selectedCAIDList.Equals(null))
                {
                    foreach (DataRow dr in _selectedCAIDList.Values)
                    {
                        if (_caType.Equals(CorporateActionType.NameChange))
                        {
                            joinedSymbols.Append(dr["NewSymbol"].ToString());

                        }
                        else
                        {
                            joinedSymbols.Append(dr["OrigSymbol"].ToString());
                        }
                        joinedSymbols.Append(",");
                    }
                }


                //return joinedSymbols.ToString().TrimEnd(new char[]{','});
                return joinedSymbols.ToString();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
            return null;

        }

        internal string GetSelectedCAIDs()
        {
            string joinedStr = string.Empty;
            try
            {
                Guid[] caIDArr = new Guid[_selectedCAIDList.Count];
                _selectedCAIDList.Keys.CopyTo(caIDArr, 0);

                string[] caIDStrArr = Array.ConvertAll(caIDArr, new Converter<Guid, string>(GUIDtoString));
                joinedStr = String.Join(",", caIDStrArr);
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

            return joinedStr;
        }

        internal DataRowCollection GetSelectedCARows()
        {
            DataTable dt = _caTable.Clone();

            try
            {
                foreach (KeyValuePair<Guid, DataRow> keyValue in _selectedCAIDList)
                {
                    DataRow dr = dt.NewRow();
                    dr.ItemArray = keyValue.Value.ItemArray;
                    dt.Rows.Add(dr);
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

            return dt.Rows;
        }

        private string GUIDtoString(Guid guid)
        {
            try
            {
                return guid.ToString();
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
                return null;
            }
        }

        /// <summary>
        /// Exports the corporate action details.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        internal void ExportCorporateActions(string fileName)
        {
            try
            {
                fileName = String.Format("{0}\\{1}{2}{3}", Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName), "_CorporateActionDetails", Path.GetExtension(fileName));
                this.ultraGridExcelExporter1.Export(this.grdCorporateActionEntry, fileName);
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

        internal void SetAllColumnsEditble()
        {
            try
            {
                UltraGridBand band = this.grdCorporateActionEntry.DisplayLayout.Bands[0];

                foreach (UltraGridColumn col in band.Columns)
                {
                    col.CellActivation = Activation.AllowEdit;
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

        internal void SetAllColumnNonEditble()
        {
            try
            {
                UltraGridBand band = this.grdCorporateActionEntry.DisplayLayout.Bands[0];

                foreach (UltraGridColumn col in band.Columns)
                {
                    if (col.Key == ApplicationConstants.C_COMBO_SELECT)
                    {
                        col.CellActivation = Activation.NoEdit;
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

        internal void DisableContextMenu()
        {
            try
            {
                mnuPreviewUndo.Enabled = false;
                mnuPreviewRedo.Enabled = false;
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

        internal void EnableContextMenu()
        {
            try
            {
                mnuPreviewUndo.Enabled = true;
                mnuPreviewRedo.Enabled = true;
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

        internal void CleanUp()
        {
            _smServices.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_smServices_SecMstrDataResponse);
            _smServices.SymbolLookUpDataResponse -= new EventHandler<EventArgs<DataSet>>(_smServices_SymbolLookUpDataResponse);
        }

        private void grdCorporateActionEntry_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition);
            }
        }

        /// <summary>
        /// Added by faisal Shah
        /// Set access to Grid on the basis of permissions
        /// </summary>
        /// <param name="_hasAccess"></param>
        internal void SetGridAsReadOnly()
        {
            try
            {
                grdCorporateActionEntry.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;

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
        public UltraGrid GetGridInstance()
        {
            return grdCorporateActionEntry;
        }
    }
}
