using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Prana.Allocation.Common.Definitions;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.IO;
using Infragistics.Win.UltraWinMaskedEdit;
using Prana.Utilities.UIUtilities;
using Prana.BusinessObjects.Constants;

namespace Prana.AllocationNew.Allocation.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AccountStrategyAllocationControl : UserControl
    {

        #region Precision digits for allocation need to move it to preferences
        /**
         * For now this is in static constructor of grid control
         * First Part: This should be in preference 
         * Second Part: This can be changeable at runtime(this can be tricky but first part should be done)
         * JIRA for first part is at http://jira.nirvanasolutions.com:8080/browse/PRANA-6387
         */
        static int _precisionDigits = NumberPrecisionConstants.GetPrecisionDigit();
        static string _precisionFormat = "####,###,###,###,##0.";

        /// <summary>
        /// Set precision digit for % and Qty columns format
        /// </summary>
        /// <param name="precisionDigit"></param>
        /// <param name="IsAccountStrategyGrid"></param>
        public void SetPrecisionDigit(int precisionDigit)
        {
            try
            {
                _precisionDigits = precisionDigit;
                SetPrecisionFormat();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
                
        }
        /// <summary>
        /// Set precision format for % and Qty columns format
        /// </summary>
        private void SetPrecisionFormat()
        {
            try
            {
                _precisionFormat = "####,###,###,###,##0.";
                for (int i = 0; i < _precisionDigits; i++)
                {
                    _precisionFormat += "#";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="AccountStrategyAllocationControl"/> class.
        /// </summary>
        public AccountStrategyAllocationControl()
        {
            try
            {
                InitializeComponent();
                _delayTimer.Elapsed += _delayTimer_Elapsed;
                _delayTimer.Start();
                _delayTimer.Enabled = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 800 milisecond delay typing timer, apply before control size change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _delayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { _delayTimer_Elapsed(sender, e); };
                    this.Invoke(del);
                }
                else
                {
                    lock (_delayLockerObject)
                    {
                        if (_fiterChanged && (DateTime.Now - _lastChangedTime).Milliseconds > 800)
                        {
                            if (this.Parent.Parent is IAllocationCalculator)
                            {
                                UltraGridCell aCell = this.ultraGrid1.ActiveCell;

                                int row = ultraGrid1.Rows.GetFilteredInNonGroupByRows().Count();
                                if (RowChanged != null)
                                    RowChanged(this, new EventArgs<int>(row));

                                this.ultraGrid1.ActiveCell = aCell;
                                this.ultraGrid1.Focus();
                                this.ultraGrid1.PerformAction(UltraGridAction.ToggleEditMode);
                                _delayTimer.Enabled = false;
                            }
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// _lastChangedTime to calculate last changed time for control size changed
        /// </summary>
        DateTime _lastChangedTime = DateTime.Now;

        /// <summary>
        /// _delayLoackerObject used in delay typing timer
        /// </summary>
        object _delayLockerObject = new object();

        /// <summary>
        /// timer for 200 miliseconds
        /// </summary>
        System.Timers.Timer _delayTimer = new System.Timers.Timer(200);

        /// <summary>
        /// Contains filter changed state
        /// </summary>
        Boolean _fiterChanged = false;

        /// <summary>
        /// The dt temporary
        /// </summary>
        DataTable dtTemp = new DataTable();

        /// <summary>
        /// Gets or sets the cum qauntity.
        /// </summary>
        /// <value>
        /// The cum qauntity.
        /// </value>
        public decimal CumQauntity { get; set; }



        private int _totalNoOfTrades = 0;

        /// <summary>
        /// Gets or sets the No of Trade.
        /// </summary>
        /// <value>
        /// Total No of Trades
        /// </value>
        public int TotalNoOfTrades
        {
            get
            {
                return _totalNoOfTrades;
            }
            set
            {
                _totalNoOfTrades = value;
            }
        }


        private int _totalNoOfTradeSelected = 0;

        /// <summary>
        /// Gets or sets the No of Trade selected.
        /// </summary>
        /// <value>
        /// Total No of selected Trades
        /// </value>
        public int TotalNoOfTradesSelected
        {
            get
            {
                return _totalNoOfTradeSelected;
            }
            set
            {
                _totalNoOfTradeSelected = value;
            }
        }


        /// <summary>
        /// Sets up.
        /// </summary>
        /// <param name="accountList">The account Cache.</param>
        /// <param name="strategyCollection">The strategy collection.</param>
        internal void SetUp(Dictionary<int, string> accountList, StrategyCollection strategyCollection)
        {
            try
            {
                SetUpDataTable(accountList, strategyCollection);
                SetUpGrid(strategyCollection);
                ultraGrid1.DrawFilter = new MySpacerColumnDrawFilter();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Finding the maximum lengh account
        /// </summary>
        /// <returns>maximum length account</returns>
        internal int GetMaxAccountLength()
        {
            int maxAccountLength = 0;
            try
            {   
                foreach (var r in CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict())
                {
                    if (r.Value.Count() > maxAccountLength)
                        maxAccountLength = r.Value.Count();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return maxAccountLength;
        }
        Dictionary<int, string> _accountList = new Dictionary<int, string>();
        /// <summary>
        /// Sets up data table.
        /// </summary>
        /// <param name="accountList">The account Cache.</param>
        /// <param name="strategyCollection">The strategy collection.</param>
        private void SetUpDataTable(Dictionary<int, string> accountList, StrategyCollection strategyCollection)
        {

            try
            {
                this._accountList = accountList;
                DataColumn accountId = new DataColumn();
                accountId.ColumnName = AllocationUIConstants.FUND_ID;
                accountId.DataType = typeof(int);
                dtTemp.Columns.Add(accountId);

                DataColumn account = new DataColumn();
                account.ColumnName = AllocationUIConstants.FUND;
                account.Caption = "Name";
                dtTemp.Columns.Add(account);

                DataColumn perAccount = new DataColumn();
                perAccount.ColumnName = AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE;
                perAccount.DataType = typeof(Decimal);
                perAccount.Caption = "%";
                perAccount.DefaultValue = 0;
                dtTemp.Columns.Add(perAccount);

                DataColumn qtyAccount = new DataColumn();
                qtyAccount.ColumnName = AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY;
                qtyAccount.DataType = typeof(Decimal);
                qtyAccount.Caption = "Qty";
                qtyAccount.DefaultValue = 0;
                dtTemp.Columns.Add(qtyAccount);

                if (strategyCollection != null)
                {
                    foreach (Strategy strategy in strategyCollection)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {
                            DataColumn perColumn = new DataColumn();
                            perColumn.ColumnName = AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE;
                            perColumn.DataType = typeof(Decimal);
                            perColumn.Caption = "%";
                            perColumn.DefaultValue = 0;
                            dtTemp.Columns.Add(perColumn);

                            DataColumn qtyColumn = new DataColumn();
                            qtyColumn.ColumnName = AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY;
                            qtyColumn.DataType = typeof(Decimal);
                            qtyColumn.Caption = "Qty";
                            qtyColumn.DefaultValue = 0;
                            dtTemp.Columns.Add(qtyColumn);
                            // UltraGridGroup group = ultraGrid1.DisplayLayout.Bands[0].Groups.Add(strategy.StrategyID.ToString(), strategy.Name);

                        }
                    }
                }
                foreach (int id in accountList.Keys)
                {
                    if (id != -1)
                    {
                        dtTemp.Rows.Add(new object[] { id, accountList[id].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Set grid Masking
        /// </summary>
        /// <param name="strategyCollection"></param>
        public void SetGridMasking(StrategyCollection strategyCollection)
        {
            try
            {
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Format = _precisionFormat;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].Format = _precisionFormat;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].MaskInput = "{double:12." + _precisionDigits + "}";
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].MaskInput = "{double:12." + _precisionDigits + "}";
                
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                if (strategyCollection != null)
                {
                    foreach (Strategy strategy in strategyCollection)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].Format = _precisionFormat;
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].Format = _precisionFormat;
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].MaskInput = "{double:12." + _precisionDigits + "}";
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].MaskInput = "{double:12." + _precisionDigits + "}";
                            
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Sets up grid.
        /// </summary>
        /// <param name="strategyCollection">The strategy collection.</param>
        private void SetUpGrid(StrategyCollection strategyCollection)
        {
            try
            {
                ultraGrid1.DataSource = dtTemp;
                ultraGrid1.DisplayLayout.Bands[0].Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
                ultraGrid1.DisplayLayout.Bands[0].Override.UseExternalSummaryCalculator = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND_ID].Hidden = true;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND].Header.Fixed = true;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Header.Fixed = true;
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].Header.Fixed = true;

                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].PromptChar = ' ';
                ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].PromptChar = ' ';

                UltraGridGroup accountGroup = ultraGrid1.DisplayLayout.Bands[0].Groups.Add(AllocationUIConstants.FUND, AllocationUIConstants.FUND);
                accountGroup.Header.Appearance.TextHAlign = HAlign.Center;
                accountGroup.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND_ID]);
                accountGroup.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND]);
                accountGroup.Header.Fixed = true;
                accountGroup.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE]);
                accountGroup.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY]);
                //ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].Width = 50;
                AddSummaries(AllocationUIConstants.TOTAL_NAME, AllocationUIConstants.FUND, 100);
                AddSummaries(AllocationUIConstants.REMAINING_NAME, AllocationUIConstants.FUND, 100);
                AddSummaries(AllocationUIConstants.TOTAL_PERCENTAGE, AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE, 100);
                AddSummaries(AllocationUIConstants.TOTAL_QUANTITY, AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY, this.CumQauntity);
                AddSummaries(AllocationUIConstants.REMAINING_PERCENTAGE, AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE, 100);
                AddSummaries(AllocationUIConstants.REMAINING_QUANTITY, AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY, this.CumQauntity);
                AddSummaries(AllocationUIConstants.TOTAL_NO_OF_TRADES, AllocationUIConstants.FUND, 0);
                AddSummaries(AllocationUIConstants.SELECTED_TRADE, AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE, 0);
                this.AddSpacerGroups(ultraGrid1.DisplayLayout.Bands[0], accountGroup, 1);
                if (strategyCollection != null)
                {
                    this.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.None;
                    this.ultraGrid1.DisplayLayout.UseFixedHeaders = true;
                    
                    foreach (Strategy strategy in strategyCollection)
                    {

                        if (strategy.StrategyID != int.MinValue)
                        {
                            UltraGridGroup group = ultraGrid1.DisplayLayout.Bands[0].Groups.Add(strategy.StrategyID.ToString(), strategy.Name);
                            group.Header.Appearance.TextHAlign = HAlign.Center;
                            group.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE]);
                            
                            group.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY]);
                            this.AddSpacerGroups(ultraGrid1.DisplayLayout.Bands[0], group, strategyCollection.Count);

                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].PromptChar = ' ';
                            ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].PromptChar = ' ';

                            AddSummaries(AllocationUIConstants.TOTAL_QUANTITY + "_" + AllocationUIConstants.STRATEGY_PREFIX + strategy.Name, AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY, this.CumQauntity);
                            AddSummaries(AllocationUIConstants.TOTAL_PERCENTAGE + "_" + AllocationUIConstants.STRATEGY_PREFIX + strategy.Name, AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE, 100);
                            AddSummaries(AllocationUIConstants.REMAINING_QUANTITY + "_" + AllocationUIConstants.STRATEGY_PREFIX + strategy.Name, AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY, this.CumQauntity);
                            AddSummaries(AllocationUIConstants.REMAINING_PERCENTAGE + "_" + AllocationUIConstants.STRATEGY_PREFIX + strategy.Name, AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE, 100);
                        }

                    }

                }
                else
                {
                    this.ultraGrid1.DisplayLayout.UseFixedHeaders = false;
                    this.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
                    //Set the ultragrid column size according to max account length
                    ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND].Width = (GetMaxAccountLength() * 7) + 25;
                    // ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Width = 50;
                    // ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].Width = 50;
                }
                ultraGrid1.DisplayLayout.Bands[0].SummaryFooterCaption = "";
                SetGridMasking(strategyCollection);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        //Kashish (25 March 2015)
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6727 
        private void AddSpacerGroups(UltraGridBand band, UltraGridGroup group, int strategycount)
        {
            try
            {
                int totalstrategies = strategycount;
                int groupIndex = group.Index;

                if (groupIndex != totalstrategies)
                {
                    string spacerColumnName = string.Format("_Spacer Column {0}", groupIndex);
                    var spacerColumn = band.Columns.Add(spacerColumnName);
                    spacerColumn.Width = 12;
                    spacerColumn.Header.Caption = " ";
                    spacerColumn.CellAppearance.BackColor = SystemColors.Control;
                    spacerColumn.Header.Appearance.BackColor = SystemColors.Control;
                    spacerColumn.Header.Appearance.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
                    spacerColumn.CellActivation = Activation.Disabled;
                    spacerColumn.SortIndicator = SortIndicator.Disabled;
                    spacerColumn.FilterOperatorLocation = FilterOperatorLocation.Hidden;
                    spacerColumn.AllowRowFiltering = DefaultableBoolean.False;
                    spacerColumn.FilterClearButtonVisible = DefaultableBoolean.False;
                    band.Layout.Override.FixedCellSeparatorColor = Color.Black;
                    spacerColumn.Group = group;

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }


        /// <summary>
        /// Adds the summaries.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="total">The total.</param>
        private void AddSummaries(string key, string columnName, decimal total)
        {
            try
            {
                //SummarySettings ssRemaining = this.ultraGrid1.DisplayLayout.Bands[0].Summaries.Add(key, SummaryType.Sum, this.ultraGrid1.DisplayLayout.Bands[0].Columns[columnName]);
                //ssRemaining.SummaryType = SummaryType.Custom;
                ////ssRemaining.CustomSummaryCalculator = new CustomSummaries(total, this.CumQauntity);
                //ssRemaining.Appearance.BorderAlpha = Alpha.Opaque;
                //ssRemaining.Appearance.BorderColor = Color.Black;
                //ssRemaining.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                //ssRemaining.DisplayFormat = " {0} ";
                //ssRemaining.SummaryDisplayArea = SummaryDisplayAreas.TopFixed;                

                SummarySettings ssRemaining = this.ultraGrid1.DisplayLayout.Bands[0].Summaries.Add(key, SummaryType.External, this.ultraGrid1.DisplayLayout.Bands[0].Columns[columnName]);
                if (!CustomThemeHelper.ApplyTheme)
                {
                    ssRemaining.Appearance.BorderAlpha = Alpha.Opaque;
                    ssRemaining.Appearance.BorderColor = Color.Black;
                    ssRemaining.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                }
                ssRemaining.DisplayFormat = " {0:" + NumberPrecisionConstants.GetPrecisionFormat() + "} ";
                ssRemaining.Tag = total;
                ssRemaining.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="targetDictionary">The target dictionary.</param>
        /// <param name="level">The level.</param>
        internal void SetValues(SerializableDictionary<int, AccountValue> targetDictionary, int level)
        {
            try
            {
                lock (dtTemp)
                {
                    ClearGrid();
                    List<int> accountList = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Keys.ToList();
                    StrategyCollection strategies = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();
                    List<int> strategyList = new List<int>();
                    foreach (Strategy strategy in strategies)
                    {
                        strategyList.Add(strategy.StrategyID);
                    }
                    if (level == 1)
                    {

                        foreach (int id in targetDictionary.Keys)
                        {
                            if (!accountList.Contains(id))
                                continue;
                            DataRow dr = dtTemp.Select("AccountId = " + id)[0];

                            dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE] = targetDictionary[id].Value;
                            CalculateQuantityForPercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Group.Header.Caption, dtTemp.Rows.IndexOf(dr), targetDictionary[id].Value);
                        }
                    }
                    else if (level == 2)
                    {
                        foreach (int accountId in accountList)
                        {
                            DataRow dr = dtTemp.Select("AccountId = " + accountId)[0];
                            if (targetDictionary.Keys.Contains(accountId))
                            {
                                dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE] = targetDictionary[accountId].Value;
                                CalculateQuantityForPercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Group.Header.Caption, dtTemp.Rows.IndexOf(dr), targetDictionary[accountId].Value);
                                foreach (StrategyValue strategy in targetDictionary[accountId].StrategyValueList)
                                {
                                    if (!strategyList.Contains(strategy.StrategyId))
                                        continue;
                                    string strategyName = CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(strategy.StrategyId);
                                    dr[AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE] = strategy.Value;
                                    CalculateQuantityForPercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE].Group.Header.Caption, dtTemp.Rows.IndexOf(dr), strategy.Value);
                                }
                            }
                            DisableStrategyGroup(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Group.Header.Caption, dtTemp.Rows.IndexOf(dr));
                        }
                        //foreach (int id in targetDictionary.Keys)
                        //{
                        //    if (!accountList.Contains(id))
                        //        continue;

                        //    DataRow dr = dtTemp.Select("AccountId = " + id)[0];
                        //    dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE] = targetDictionary[id].Value;
                        //    CalculateQuantityForPercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Group.Header.Caption, dtTemp.Rows.IndexOf(dr), targetDictionary[id].Value);
                        //    foreach (StrategyValue strategy in targetDictionary[id].StrategyValueList)
                        //    {
                        //        if (!strategyList.Contains(strategy.StrategyId))
                        //            continue;
                        //        string strategyName = CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(strategy.StrategyId);
                        //        dr[AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE] = strategy.Value;
                        //        CalculateQuantityForPercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE].Group.Header.Caption, dtTemp.Rows.IndexOf(dr), strategy.Value);
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Clears the grid.
        /// </summary>
        internal void ClearGrid()
        {
            try
            {
                dtTemp.Clear();
                foreach (int id in _accountList.Keys)
                {
                    if (id != -1)
                    {
                        dtTemp.Rows.Add(new object[] { id, _accountList[id].ToString() });
                    }
                }
                //foreach (DataRow dr in dtTemp.Rows)
                //{
                //    foreach (DataColumn col in dtTemp.Columns)
                //    {
                //        if (col.ColumnName != AllocationUIConstants.FUND_ID && col.ColumnName != AllocationUIConstants.FUND)
                //            dr[col] = 0;
                //    }
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the allocation accounts.
        /// </summary>
        /// <param name="allocationLevelList">The allocation level list.</param>
        internal void SetAllocationAccounts(AllocationLevelList allocationLevelList)
        {
            try
            {
                lock (dtTemp)
                {
                    ClearGrid();
                    foreach (AllocationLevelClass allocation in allocationLevelList.Collection)
                    {

                        DataRow dr = dtTemp.Select("AccountId = " + allocation.LevelnID)[0];
                        dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE] = allocation.Percentage;
                        dr[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY] = allocation.AllocatedQty;
                        if (allocation.Childs != null)
                        {
                            foreach (AllocationLevelClass strategy in allocation.Childs.Collection)
                            {
                                string name = CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(strategy.LevelnID);
                                if (dr.Table.Columns.Contains(AllocationUIConstants.STRATEGY_PREFIX + name + AllocationUIConstants.PERCENTAGE))
                                {
                                    dr[AllocationUIConstants.STRATEGY_PREFIX + name + AllocationUIConstants.PERCENTAGE] = strategy.Percentage;
                                }

                                if (dr.Table.Columns.Contains(AllocationUIConstants.STRATEGY_PREFIX + name + AllocationUIConstants.QUANTITY))
                                {
                                    dr[AllocationUIConstants.STRATEGY_PREFIX + name + AllocationUIConstants.QUANTITY] = strategy.AllocatedQty;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the AfterCellUpdate event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void ultraGrid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
            //try
            //{
            //    if (!(e.Cell is UltraGridFilterCell))
            //    {
            //        if (this.CumQauntity == 0)
            //        {
            //            this.ultraGrid1.AfterCellUpdate -= ultraGrid1_AfterCellUpdate;
            //            e.Cell.Value = 0;
            //            this.ultraGrid1.AfterCellUpdate += ultraGrid1_AfterCellUpdate;
            //            return;
            //        }
            //        if (e.Cell.Value == DBNull.Value)
            //            e.Cell.Value = 0;
            //        //Absolute value if negative
            //        if (Convert.ToDecimal(e.Cell.Value) < 0.0M)
            //            e.Cell.Value = Math.Abs(Convert.ToDecimal(e.Cell.Value));

            //        if (e.Cell.Column.Key.Contains(AllocationUIConstants.PERCENTAGE))
            //        {
            //            CalculateQuantityForPercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption, e.Cell.Row.ListIndex, Convert.ToDecimal(e.Cell.Value));
            //            if (ValidatePercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption))
            //            {
            //                if (ValueChanged != null)
            //                    ValueChanged(this, EventArgs.Empty);
            //            }
            //        }
            //        else if (e.Cell.Column.Key.Contains(AllocationUIConstants.QUANTITY))
            //        {
            //            CalculatePercentageForQuantity(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption, e.Cell.Row.ListIndex, Convert.ToDecimal(e.Cell.Value));
            //            if (ValidatePercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption))
            //            {
            //                if (ValueChanged != null)
            //                    ValueChanged(this, EventArgs.Empty);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        /// <summary>
        /// Calculates the quantity for percentage.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="value">The value.</param>
        private void CalculateQuantityForPercentage(string groupKey, int rowIndex, decimal value)
        {
            try
            {
                if (groupKey.StartsWith(AllocationUIConstants.FUND))
                {
                    lock (dtTemp)
                    {
                        dtTemp.Rows[rowIndex][AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY] = Math.Round(value * this.CumQauntity / 100, 10); //http://jira.nirvanasolutions.com:8080/browse/PRANA-6278
                    }
                    //Calculating quantity for strategy
                    foreach (UltraGridGroup group in ultraGrid1.DisplayLayout.Bands[0].Groups)
                    {
                        if (!group.Key.Equals(AllocationUIConstants.FUND) && !group.Key.Contains("_Spacer Group"))
                        {
                            decimal percentage = 0;
                            lock (dtTemp)
                            {
                                percentage = Convert.ToDecimal(dtTemp.Rows[rowIndex][AllocationUIConstants.STRATEGY_PREFIX + group.Header.Caption + AllocationUIConstants.PERCENTAGE]);
                            }
                            CalculateQuantityForPercentage(group.Header.Caption, rowIndex, percentage);
                        }
                    }
                }
                else
                {
                    lock (dtTemp)
                    {
                        decimal accountValue = Convert.ToDecimal(dtTemp.Rows[rowIndex][AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY]);
                        dtTemp.Rows[rowIndex][AllocationUIConstants.STRATEGY_PREFIX + groupKey + AllocationUIConstants.QUANTITY] = Math.Round(value * accountValue / 100, 10); //http://jira.nirvanasolutions.com:8080/browse/PRANA-6278
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Calculates the percentage for quantity.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="value">The value.</param>
        private void CalculatePercentageForQuantity(string groupKey, int rowIndex, decimal value)
        {
            try
            {
                if (groupKey.StartsWith(AllocationUIConstants.FUND))
                {
                    lock (dtTemp)
                    {
                        if (this.CumQauntity != 0)
                            dtTemp.Rows[rowIndex][AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE] = (value * 100) / this.CumQauntity;
                        else
                            dtTemp.Rows[rowIndex][AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE] = 0;
                    }
                    //Calculating quantity for strategy
                    foreach (UltraGridGroup group in ultraGrid1.DisplayLayout.Bands[0].Groups)
                    {
                        if (!group.Key.Equals(AllocationUIConstants.FUND) && !group.Key.Contains("_Spacer Group"))
                        {
                            decimal percentage = 0;
                            lock (dtTemp)
                            {
                                percentage = Convert.ToDecimal(dtTemp.Rows[rowIndex][AllocationUIConstants.STRATEGY_PREFIX + group.Header.Caption + AllocationUIConstants.PERCENTAGE]);
                            }
                            CalculateQuantityForPercentage(group.Header.Caption, rowIndex, percentage);
                        }
                    }
                }
                else
                {
                    lock (dtTemp)
                    {
                        decimal accountValue = Convert.ToDecimal(dtTemp.Rows[rowIndex][AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY]);
                        if (accountValue != 0)
                            dtTemp.Rows[rowIndex][AllocationUIConstants.STRATEGY_PREFIX + groupKey + AllocationUIConstants.PERCENTAGE] = (value * 100) / accountValue;
                        else
                            dtTemp.Rows[rowIndex][AllocationUIConstants.STRATEGY_PREFIX + groupKey + AllocationUIConstants.PERCENTAGE] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Validates the percentage.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <returns></returns>
        private bool ValidatePercentage(string groupKey)
        {
            try
            {
                if (groupKey.StartsWith(AllocationUIConstants.FUND))
                {
                    decimal total = 0;
                    lock (dtTemp)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            decimal accountValue = Convert.ToDecimal(dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE]);
                            if (Convert.ToDecimal(dr[groupKey + AllocationUIConstants.PERCENTAGE]) != 0)
                                total += Convert.ToDecimal(dr[groupKey + AllocationUIConstants.PERCENTAGE]);
                            if (accountValue == 0)
                            {
                                foreach (DataColumn col in dtTemp.Columns)
                                {
                                    if (!col.ColumnName.StartsWith(AllocationUIConstants.FUND) && Convert.ToDecimal(dr[col]) != 0)
                                        dr[col] = 0;
                                }

                            }
                        }
                    }
                    //if (total != 100 && total != 0)
                    //    return false;
                }
                else
                {

                    lock (dtTemp)
                    {
                        decimal accountTotalValue = 0;
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            decimal total = 0;
                            decimal accountValue = Convert.ToDecimal(dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE]);
                            if (accountValue > 0)
                            {
                                foreach (DataColumn col in dtTemp.Columns)
                                {
                                    if (!col.ColumnName.StartsWith(AllocationUIConstants.FUND) && col.ColumnName.Contains(AllocationUIConstants.PERCENTAGE))
                                        total += Convert.ToDecimal(dr[col]);
                                }
                                //if (total != 100 && total != 0)
                                // return false;
                            }
                            accountTotalValue += accountValue;
                        }
                        //if (accountTotalValue != 100 && accountTotalValue != 0)
                        //    return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the allocation account value.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, AccountValue> GetAllocationAccountValue()
        {
            SerializableDictionary<int, AccountValue> targetPercentage = new SerializableDictionary<int, AccountValue>();
            try
            {
                lock (dtTemp)
                {
                    foreach (DataRow dr in dtTemp.Rows)
                    {

                        int accountId = Convert.ToInt32(dr[AllocationUIConstants.FUND_ID]);
                        decimal percentage = dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE] == null ? 0 : Convert.ToDecimal(dr[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE]);
                        if (percentage != 0)
                        {
                            List<StrategyValue> strategyList = new List<StrategyValue>();
                            foreach (DataColumn col in dtTemp.Columns)
                            {
                                if (!col.ColumnName.StartsWith(AllocationUIConstants.FUND) && col.ColumnName.Contains(AllocationUIConstants.PERCENTAGE))
                                {
                                    int id = Convert.ToInt32(ultraGrid1.DisplayLayout.Bands[0].Columns[col.ColumnName].Group.Key);
                                    decimal value = dr[col] == null ? 0 : Convert.ToDecimal(dr[col]);
                                    if (value != 0)
                                    {
                                        StrategyValue startegy = new StrategyValue(id, value);
                                        strategyList.Add(startegy);
                                    }
                                }
                            }
                            //Added to set strategy value, PRANA-12400
                            if (strategyList.Count == 0)
                            {
                                StrategyValue strategy = new StrategyValue(0, 100);
                                strategyList.Add(strategy);
                            }
                            AccountValue account = new AccountValue(accountId, percentage, strategyList);
                            targetPercentage.Add(accountId, account);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return targetPercentage;
        }

        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Updates the quantity.
        /// </summary>
        /// <param name="cumQauntity">The cum qauntity.</param>
        internal void UpdateQuantity(decimal cumQauntity)
        {
            try
            {
                this.CumQauntity = cumQauntity;
                //foreach (SummarySettings ss in ultraGrid1.DisplayLayout.Bands[0].Summaries)
                //{
                //    if (ss.Key.StartsWith(AllocationUIConstants.TOTAL_QUANTITY) || ss.Key.StartsWith(AllocationUIConstants.REMAINING_QUANTITY))
                //        ss.CustomSummaryCalculator = new CustomSummaries(this.CumQauntity, 0);
                //    else if (ss.Key.StartsWith(AllocationUIConstants.TOTAL_PERCENTAGE) || ss.Key.StartsWith(AllocationUIConstants.REMAINING_PERCENTAGE))
                //        ss.CustomSummaryCalculator = new CustomSummaries(100, this.CumQauntity);
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Updates the Total No. of Trades
        /// </summary>
        /// <param name="cumQauntity">The cum qauntity.</param>
        internal void UpdateTotalNoOfTrades(int selected, int totalTrades)
        {
            try
            {
                this.TotalNoOfTradesSelected = selected;
                this.TotalNoOfTrades = totalTrades;
                ExternalSummaryValueUpdateForTradeCounter(ultraGrid1.Rows.SummaryValues[AllocationUIConstants.SELECTED_TRADE]);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the ItemClicked event of the contextMenuStrip1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolStripItemClickedEventArgs"/> instance containing the event data.</param>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Tag.ToString())
                {
                    case AllocationUIConstants.SAVE_LAYOUT:
                        SaveLayoutAsXML();
                        break;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves the layout as XML.
        /// </summary>
        private void SaveLayoutAsXML()
        {
            try
            {
                /* There is need to add layout version to avoid key not found errors when any columns or summaries are added
                 * Firstly write version no to file in which grid layout has to be saved.
                 * Then use a file stream to append grid layout xml to this file.
                 * http://jira.nirvanasolutions.com:8080/browse/PRANA-8070
                 */
                File.WriteAllText(GetLayoutFilePath(), AllocationUIConstants.LAYOUT_VERSION);
                System.IO.FileStream fs = new System.IO.FileStream(GetLayoutFilePath(), System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);

                // Save the layout to the file.
                this.ultraGrid1.DisplayLayout.SaveAsXml(fs, PropertyCategories.All);
                fs.Close();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the layout file path.
        /// </summary>
        /// <returns></returns>
        private string GetLayoutFilePath()
        {
            try
            {
                string startPath = System.Windows.Forms.Application.StartupPath;
                string allocationPreferencesPath = startPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                string partialFileName = "Allocation_";

                if (this.Parent.Parent is IAllocationCalculator)
                {

                    partialFileName += AllocationUIConstants.FUND + "_";

                }
                else if (this.Parent.Parent is AccountStrategyMappingUserCtrlNew)
                {

                    partialFileName += AllocationUIConstants.STRATEGY_PREFIX;

                }


                if (!Directory.Exists(allocationPreferencesPath))
                {
                    Directory.CreateDirectory(allocationPreferencesPath);
                }

                return allocationPreferencesPath + "\\" + partialFileName + "GridLayout.xml";
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Handles the Load event of the AccountStrategyAllocationControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AccountStrategyAllocationControl_Load(object sender, EventArgs e)
        {
            try
            {
                SetUpLayout();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets up layout.
        /// </summary>
        private void SetUpLayout()
        {
            try
            {
                /* This method loads saved preference xml file to a stream.
                 * Get the version tag as string from this stream.
                 * If layout version stored in file is same as current layout version, then only check for groups
                 * If layout version is different, then do not load the saved layout
                 * http://jira.nirvanasolutions.com:8080/browse/PRANA-8070
                 */
                string path = GetLayoutFilePath();
                UltraGrid gridTest = new UltraGrid();
                UltraGridBand band = ultraGrid1.DisplayLayout.Bands[0];
                bool doLoadLayout = false;
                System.IO.FileStream fs = null;

                if (File.Exists(path))
                {
                    // Open the file where the layout has been saved
                    fs = new System.IO.FileStream(GetLayoutFilePath(), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                    byte[] b = new byte[AllocationUIConstants.LAYOUT_VERSION.Length];
                    fs.Read(b, 0, (int)(AllocationUIConstants.LAYOUT_VERSION.Length));
                    string s = System.Text.Encoding.UTF8.GetString(b);
                    if (s == AllocationUIConstants.LAYOUT_VERSION)
                    {
                        // Reset the Position of the file stream to where the layout data begins, then load the layout from the stream by calling Load method. 
                        fs.Position = (long)(AllocationUIConstants.LAYOUT_VERSION.Length);
                        gridTest.DisplayLayout.LoadFromXml(fs, PropertyCategories.All);
                        doLoadLayout = true;
                    }
                }

                if (doLoadLayout)
                {
                    //First check the no. of groups then check group key
                    if (gridTest.DisplayLayout.Bands[0].Groups.Count == band.Groups.Count)
                    {
                        List<string> keys = new List<string>();
                        foreach (UltraGridGroup grp in gridTest.DisplayLayout.Bands[0].Groups)
                            keys.Add(grp.Key);
                        foreach (UltraGridGroup grp1 in band.Groups)
                        {
                            if (!keys.Contains(grp1.Key))
                                doLoadLayout = false;
                        }
                    }
                    else
                        doLoadLayout = false;
                }

                if (doLoadLayout)
                {
                    fs.Position = (long)(AllocationUIConstants.LAYOUT_VERSION.Length);
                    ultraGrid1.DisplayLayout.LoadFromXml(fs, PropertyCategories.All);
                }
                // close the file
                if (fs != null)
                    fs.Close();
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Calculate the Summary when required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_ExternalSummaryValueRequested(object sender, ExternalSummaryValueEventArgs e)
        {
            try
            {
                SummarySettings summarySettings = e.SummaryValue.SummarySettings;

                decimal total = 0;
                //Decimal.TryParse(summarySettings.Tag.ToString(), out total);
                if (summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_QUANTITY) || summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_QUANTITY))
                    total = this.CumQauntity;
                else if (summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_PERCENTAGE) || summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_PERCENTAGE))
                    total = 100;

                decimal remaining = total;
                decimal allocated = 0;

                foreach (UltraGridRow row in this.ultraGrid1.DisplayLayout.Bands[0].GetRowEnumerator(GridRowType.DataRow))
                {
                    string cellValue = row.Cells[summarySettings.SourceColumn].Value.ToString();
                    decimal val = 0;
                    if (decimal.TryParse(cellValue, out val))
                    {
                        if (val > 0)
                        {

                            if (summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_PERCENTAGE + "_" + AllocationUIConstants.STRATEGY_PREFIX) || summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_PERCENTAGE + "_" + AllocationUIConstants.STRATEGY_PREFIX))
                            {
                                decimal value = Convert.ToDecimal(row.Cells[AllocationUIConstants.FUND + AllocationUIConstants.QUANTITY].Value);
                                if (value > 0)
                                    val = val * value / CumQauntity;
                            }
                            allocated += (val);
                            remaining -= (val);
                        }
                    }
                }

                //Calling RemoveTrailingZero() method to remove trailing zero
                if (summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_PERCENTAGE) || summarySettings.Key.StartsWith(AllocationUIConstants.TOTAL_QUANTITY))
                    e.SummaryValue.SetExternalSummaryValue(NumberPrecisionConstants.RemoveTrailingZeros(decimal.Round(allocated, NumberPrecisionConstants.GetPrecisionDigit())) + "/" + NumberPrecisionConstants.RemoveTrailingZeros(decimal.Round(total, NumberPrecisionConstants.GetPrecisionDigit())));
                else if (summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_PERCENTAGE) || summarySettings.Key.StartsWith(AllocationUIConstants.REMAINING_QUANTITY))
                    e.SummaryValue.SetExternalSummaryValue(NumberPrecisionConstants.RemoveTrailingZeros(decimal.Round(remaining, NumberPrecisionConstants.GetPrecisionDigit())) + "/" + NumberPrecisionConstants.RemoveTrailingZeros(decimal.Round(total, NumberPrecisionConstants.GetPrecisionDigit())));
                else if (summarySettings.Key.Equals(AllocationUIConstants.TOTAL_NAME))
                    e.SummaryValue.SetExternalSummaryValue("Total: ");
                else if (summarySettings.Key.Equals(AllocationUIConstants.REMAINING_NAME))
                    e.SummaryValue.SetExternalSummaryValue("Remaining: ");
                else if (summarySettings.Key.Equals(AllocationUIConstants.SELECTED_TRADE))
                    e.SummaryValue.SetExternalSummaryValue(TotalNoOfTradesSelected + "/" + TotalNoOfTrades);
                else if (summarySettings.Key.Equals(AllocationUIConstants.TOTAL_NO_OF_TRADES))
                    e.SummaryValue.SetExternalSummaryValue("Total # of Trades: ");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updating the summary for Trade Counter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="summaryValue"></param>
        private void ExternalSummaryValueUpdateForTradeCounter(SummaryValue summaryValue)
        {
            try
            {
                SummarySettings summarySettings = summaryValue.SummarySettings;

                if (summarySettings.Key.Equals(AllocationUIConstants.SELECTED_TRADE))
                    summaryValue.SetExternalSummaryValue(TotalNoOfTradesSelected + "/" + TotalNoOfTrades);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Handles the CellChange event of the ultraGrid1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void ultraGrid1_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!(e.Cell is UltraGridFilterCell))
                {
                    if (this.CumQauntity == 0)
                    {
                        this.ultraGrid1.AfterCellUpdate -= ultraGrid1_AfterCellUpdate;
                        e.Cell.Value = 0;
                        this.ultraGrid1.AfterCellUpdate += ultraGrid1_AfterCellUpdate;
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(e.Cell.Text))
                    {
                        decimal num;
                        // If cell value can not be parse to decimal then set to default value 0
                        if (decimal.TryParse(e.Cell.Text, out num))
                            e.Cell.Value = e.Cell.Text;
                        else
                            e.Cell.Value = 0;
                    }
                    // if cell text is empty string then set it's value to 0, PRANA-11237
                    else
                        e.Cell.Value = 0;
                    if (e.Cell.Value == DBNull.Value)
                        e.Cell.Value = 0;
                    //Absolute value if negative
                    if (Convert.ToDecimal(e.Cell.Value) < 0.0M)
                        e.Cell.Value = Math.Abs(Convert.ToDecimal(e.Cell.Value));

                    if (e.Cell.Column.Key.Contains(AllocationUIConstants.PERCENTAGE))
                    {
                        CalculateQuantityForPercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption, e.Cell.Row.ListIndex, Convert.ToDecimal(e.Cell.Value));
                        if (ValidatePercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption))
                        {
                            if (ValueChanged != null)
                                ValueChanged(this, EventArgs.Empty);
                        }
                    }
                    else if (e.Cell.Column.Key.Contains(AllocationUIConstants.QUANTITY))
                    {
                        CalculatePercentageForQuantity(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption, e.Cell.Row.ListIndex, Convert.ToDecimal(e.Cell.Value));
                        if (ValidatePercentage(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption))
                        {
                            if (ValueChanged != null)
                                ValueChanged(this, EventArgs.Empty);
                        }
                    }
                    ultraGrid1.Refresh();
                    DisableStrategyGroup(ultraGrid1.DisplayLayout.Bands[0].Columns[e.Cell.Column.Key].Group.Header.Caption, e.Cell.Row.ListIndex);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To disable the strategy group if account value is zero
        /// </summary>
        /// <param name="groupKey">Caption of account group</param>
        /// <param name="rowIndex">Index of selected row</param>
        private void DisableStrategyGroup(string groupKey, int rowIndex)
        {
            try
            {
                // This code is executed if account value is changed
                if (groupKey.StartsWith(AllocationUIConstants.FUND))
                {
                    StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();
                    foreach (Strategy strategy in strategyCollection)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {
                            // check if strategy column exists
                            if (ultraGrid1.Rows[rowIndex].Cells.Exists(AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY) && ultraGrid1.Rows[rowIndex].Cells.Exists(AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE))
                            {
                                //If account percentage is 0 then disable strategy groups
                                if (Convert.ToDecimal(ultraGrid1.Rows[rowIndex].Cells[AllocationUIConstants.FUND + AllocationUIConstants.PERCENTAGE].Value) == 0)
                                {
                                    ultraGrid1.Rows[rowIndex].Cells[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                                    ultraGrid1.Rows[rowIndex].Cells[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                                }
                                else
                                {
                                    ultraGrid1.Rows[rowIndex].Cells[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                                    ultraGrid1.Rows[rowIndex].Cells[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public event EventHandler<EventArgs<int>> RowChanged;
        /// <summary>
        /// To check if Edit Prefrences check box checked then set value true otherwise false
        /// </summary>
        public bool ShowRule { get; set; }

        /// <summary>
        /// ultraGrid1_AfterRowFilterChanged event while row filter applied and displayed row count change for accountStrategyAllocation grid
        /// Now timer will applied of 800 miliseconds for typing delay before control size changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                //if (this.Parent.Parent is IAllocationCalculator)
                //{
                //    int row = ultraGrid1.Rows.GetFilteredInNonGroupByRows().Count();
                //    if (RowChanged != null)
                //        RowChanged(this, new EventArgs<int>(row));
                //}
                if (this.Parent.Parent is IAllocationCalculator)
                {
                    // TODO: Need to find better way to check Edit prefrences check bx cheked or not
                    if (ShowRule)
                    {  
                    lock (_delayLockerObject)
                    {
                        _lastChangedTime = DateTime.Now;
                        _fiterChanged = true;
                        if (!_delayTimer.Enabled)
                            _delayTimer.Enabled = true;
                    }
                }
            }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// show searched strategy in current view of ultragid
        /// </summary>
        /// <param name="searchStrategy">strategy name</param>
        internal void ShowSearchedStrategy(string searchStrategy)
        {
            try
            {
                foreach(UltraGridGroup group in ultraGrid1.DisplayLayout.Bands[0].Groups)
                {
                    if (group.Header.Caption.Trim().Equals(searchStrategy.Trim()))
                    {
                        ultraGrid1.ActiveColScrollRegion.ScrollGroupIntoView(group,true);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }



    public class MySpacerColumnDrawFilter : IUIElementDrawFilter
    {
        protected bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {

            try
            {
                if (drawParams.Element is HeaderUIElementBase)
                {
                    HeaderUIElementBase header = drawParams.Element as HeaderUIElementBase;
                    Rectangle rect = header.Rect;
                    drawParams.DrawBorders(UIElementBorderStyle.Solid,Border3DSide.Right, Color.Black, rect, rect);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        protected DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            try
            {
                if (drawParams.Element is CellUIElement)
                {
                    var cell = drawParams.Element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (null != cell && cell.Column.Key.StartsWith("_Spacer"))
                        return DrawPhase.BeforeDrawBorders;
                }

                if (drawParams.Element is FilterCellUIElement)
                {
                    var filtercell = drawParams.Element.GetContext(typeof(UltraGridFilterCell)) as UltraGridFilterCell;
                    if (null != filtercell && filtercell.Column.Key.StartsWith("_Spacer"))
                        return DrawPhase.BeforeDrawElement;
                }

                if (drawParams.Element is HeaderUIElementBase)
                {
                    var header = drawParams.Element.GetContext(typeof(HeaderBase)) as HeaderBase;
                    if (null != header)
                    {
                        if (null != header.Column && header.Column.Key.StartsWith("_Spacer"))
                            return DrawPhase.BeforeDrawElement;
                        else if (null != header.Group && header.Group.Key.StartsWith("_Spacer"))
                            return DrawPhase.BeforeDrawElement;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return DrawPhase.BeforeDrawFocus;

        }

        // added functions as per Microsoft Managed Rules Recommendation

        bool IUIElementDrawFilter.DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            return DrawElement(drawPhase, ref drawParams);
        }

        DrawPhase IUIElementDrawFilter.GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            return GetPhasesToFilter(ref drawParams);
        }
    }

}

