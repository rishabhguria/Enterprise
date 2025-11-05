using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prana.Allocation.Common.Definitions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Win;

namespace Prana.AllocationNew.Allocation.UI.UserControls
{
    public partial class AccountAllocationControl : UserControl
    {
        private DataTable accountCollection = new DataTable();

        public AccountAllocationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes grid.
        /// Rows contains Account name and value.
        /// Last column in total column that checks if total of all account's value is 100 or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccountAllocationControl_Load(object sender, EventArgs e)
        {
            try
            {
                ultraGrid1.DataSource = accountCollection;
                accountCollection.Columns.Add("AccountId");                
                accountCollection.Columns.Add("AccountName");
                accountCollection.Columns.Add("Value", typeof(decimal));

                //Adding Columns to group.
                UltraGridGroup accountGroup = ultraGrid1.DisplayLayout.Bands[0].Groups.Add("Account", "Account");
                accountGroup.Header.Appearance.TextHAlign = HAlign.Center;
                accountGroup.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns["AccountName"]);
                accountGroup.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns["Value"]);

                UltraGridGroup strategyGroup = ultraGrid1.DisplayLayout.Bands[0].Groups.Add("Strategy", "Strategy");
                strategyGroup.Header.Appearance.TextHAlign = HAlign.Center;

                Dictionary<int, string> accountList = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();
                //Dictionary<int, string> strategyList = CommonDataCache.CachedDataManager.GetInstance.GetAllStrategies();
                StrategyCollection strategyCol = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();
                foreach (int id in accountList.Keys)
                {
                    if (id != -1)
                        accountCollection.Rows.Add(new Object[] { id, accountList[id], 0 });
                }
                foreach (Strategy strategy in strategyCol)
                {
                    if (strategy.StrategyID != int.MinValue)
                    {
                        //Creating Data column using strategy Id to avoid duplications
                        DataColumn baseColumn = new DataColumn();
                        baseColumn.ColumnName = strategy.StrategyID.ToString();
                        baseColumn.Caption = strategy.Name;
                        baseColumn.DataType = typeof(decimal);
                        accountCollection.Columns.Add(baseColumn);
                        strategyGroup.Columns.Add(ultraGrid1.DisplayLayout.Bands[0].Columns[strategy.StrategyID.ToString()]);
                    }
                    //ultraGrid1.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, ultraGrid1.DisplayLayout.Bands[0].Columns[strategyList[id]]);
                }

               
                ultraGrid1.DisplayLayout.Bands[0].Columns["AccountName"].CellActivation = Activation.NoEdit;               
                ultraGrid1.DisplayLayout.Bands[0].SummaryFooterCaption = "";
                ultraGrid1.DisplayLayout.Bands[0].Columns["AccountId"].Hidden = true;
                ultraGrid1.DisplayLayout.Bands[0].Columns["Value"].Format = "##0.00";
                ultraGrid1.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, ultraGrid1.DisplayLayout.Bands[0].Columns["Value"]);
                var spacerColumn = ultraGrid1.DisplayLayout.Bands[0].Columns.Add("SpacerColumn");
                spacerColumn.Width = 1;
                spacerColumn.Header.Caption = " ";
                spacerColumn.CellAppearance.BackColor = SystemColors.Control;
                spacerColumn.Header.Appearance.BackColor = SystemColors.Control;
                spacerColumn.Header.Appearance.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
                spacerColumn.CellActivation = Activation.Disabled;
                spacerColumn.SortIndicator = SortIndicator.Disabled;
                spacerColumn.FilterOperatorLocation = FilterOperatorLocation.Hidden;
                spacerColumn.AllowRowFiltering = DefaultableBoolean.False;
                spacerColumn.FilterClearButtonVisible = DefaultableBoolean.False;
                ultraGrid1.DisplayLayout.Bands[0].Layout.Override.FixedCellSeparatorColor = Color.Black;
                spacerColumn.Group = accountGroup;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Shows error when total is greater than 100.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_SummaryValueChanged(object sender, SummaryValueChangedEventArgs e)
        {
            try
            {
                if ((decimal)e.SummaryValue.Value != 100)
                {
                    //MessageBox.Show(this, "Value should be equal to 100", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    e.SummaryValue.Appearance.ForeColor = Color.Red;

                }
                else
                {
                    e.SummaryValue.Appearance.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Account value grid Different preference operation.
        /// </summary>
        /// <param name="serializableDictionary"></param>
        internal void UpdateGrid(SerializableDictionary<int, AccountValue> serializableDictionary)
        {
            try
            {
                ClearGrid();
                StrategyCollection strategyCol = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();                
                //Dictionary<int, string> strategyList = CommonDataCache.CachedDataManager.GetInstance.GetAllStrategies();
                foreach (int id in serializableDictionary.Keys)
                {                   
                    foreach (DataRow dr in accountCollection.Rows)
                    {
                        if (Convert.ToInt32(dr["AccountId"].ToString()) == serializableDictionary[id].AccountId)
                        {
                            dr["Value"] = decimal.Round(serializableDictionary[id].Value,2,MidpointRounding.AwayFromZero);
                            foreach (StrategyValue strategyValue in serializableDictionary[id].StrategyValueList)
                            {
                                Strategy strategy;
                                if (strategyValue.StrategyId == -1)
                                    strategy = (Strategy)strategyCol[strategyCol.IndexOf(int.MinValue)];
                                else
                                {
                                    if (strategyCol.IndexOf(strategyValue.StrategyId) != int.MinValue)
                                        strategy = (Strategy)strategyCol[strategyCol.IndexOf(strategyValue.StrategyId)];
                                    else
                                        strategy = null;
                                }
                                if (strategy != null)
                                    dr[strategy.StrategyID.ToString()] = decimal.Round(strategyValue.Value, 2, MidpointRounding.AwayFromZero);
                            }
                        }
                        DisableStrategyRow(dr["Value"].ToString(), accountCollection.Rows.IndexOf(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Clears grid and Data table when new prefernce is opened.
        /// </summary>
        private void ClearGrid()
        {
            try
            {
                //Dictionary<int, string> strategyList = CommonDataCache.CachedDataManager.GetInstance.GetAllStrategies();
                StrategyCollection strategyCol = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();   
                foreach (DataRow dr in accountCollection.Rows)
                {
                    dr["Value"] = 0;
                    foreach (Strategy strategy in strategyCol)
                    {
                        if (strategy.StrategyID != int.MinValue)
                            dr[strategy.StrategyID.ToString()] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns current values in account grid.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, AccountValue> GetCurrentValues()
        {
            try
            {
                SerializableDictionary<int, AccountValue> accountValueDict = new SerializableDictionary<int, AccountValue>();
                StrategyCollection strategyCol = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();
                //Dictionary<int, string> strategyList = CommonDataCache.CachedDataManager.GetInstance.GetAllStrategies();
                foreach (DataRow dr in accountCollection.Rows)
                {
                    AccountValue accountValue;
                    if (!string.IsNullOrEmpty(dr["Value"].ToString()))
                        accountValue = new AccountValue(Convert.ToInt32(dr["AccountId"]), Convert.ToDecimal(dr["Value"]));
                    else
                        accountValue = new AccountValue(Convert.ToInt32(dr["AccountId"]), 0);
                    foreach (Strategy strategy in strategyCol)
                    {
                        if (strategy.StrategyID != int.MinValue)
                        {
                            if (!string.IsNullOrEmpty(dr[strategy.StrategyID.ToString()].ToString()) && Convert.ToDecimal(dr[strategy.StrategyID.ToString()]) != 0)
                            {
                                accountValue.StrategyValueList.Add(new StrategyValue(strategy.StrategyID, Convert.ToDecimal(dr[strategy.StrategyID.ToString()])));
                            }
                        }
                    }
                    //foreach (int id in strategyList.Keys)
                    //{
                    //    if (Convert.ToDecimal(dr[strategyList[id]]) != 0)
                    //        accountValue.StrategyValueList.Add(new StrategyValue(id, Convert.ToDecimal(dr[strategyList[id]])));
                    //}

                    if (accountValue.Value != 0)
                    {
                        if (accountValueDict.ContainsKey(accountValue.AccountId))
                        {
                            accountValueDict[Convert.ToInt32(dr["AccountId"])] = accountValue;
                        }
                        else
                        {
                            accountValueDict.Add(Convert.ToInt32(dr["AccountId"]), accountValue);
                        }
                    }
                }
                return accountValueDict;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }

        }

        /// <summary>
        /// On cell update checking value is greater than 0 or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(e.Cell.Value.ToString()))
                {
                    if ((decimal)e.Cell.Value < 0)
                    {
                        MessageBox.Show(this, "Value can not be less than 0.", "Nirvana Preference", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cell.Value = 0;
                    }                    
                }
                if(e.Cell.Column.Header.Group.Key.Equals("Account"))
                {
                    DisableStrategyRow(e.Cell.Value.ToString(), e.Cell.Row.ListIndex);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_Error(object sender, ErrorEventArgs e)
        {
            try
            {
                if (ultraGrid1.ActiveCell.Column.DataType.Equals(typeof(decimal)))
                {
                    ultraGrid1.ActiveCell.Value = 0;
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }         
        }

        /// <summary>
        /// disable strategy row when account value is zero and enable when account value is greater than 0, PRANA-10403
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rowIndex"></param>
        private void DisableStrategyRow(string value,int rowIndex) 
        {
            StrategyCollection strategyCol = CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies();
            foreach (Strategy strategy in strategyCol)
            {
                if(strategy.StrategyID != int.MinValue)
                {
                    if (string.IsNullOrWhiteSpace(value) || Convert.ToDecimal(value) == 0.0M)
                    {
                        ultraGrid1.Rows[rowIndex].Cells[strategy.StrategyID.ToString()].Activation = Activation.Disabled;
                    }
                    else
                    {
                        ultraGrid1.Rows[rowIndex].Cells[strategy.StrategyID.ToString()].Activation = Activation.AllowEdit;
                    }
                }
            }
        }

    }
}
