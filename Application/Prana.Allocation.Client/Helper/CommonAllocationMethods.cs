using Infragistics;
using Infragistics.Controls.Grids;
using Infragistics.Windows.Editors;
using Prana.Allocation.Client.Constants;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace Prana.Allocation.Client.Helper
{
    internal class CommonAllocationMethods
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="gridName">Name of the grid.</param>
        /// <returns></returns>
        internal static string GetFileName(string gridName)
        {
            try
            {
                return "New" + gridName + "Layout";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the directory path.
        /// </summary>
        /// <returns></returns>
        internal static string GetDirectoryPath()
        {
            try
            {
                string _startPath = System.Windows.Forms.Application.StartupPath;
                string _allocationPreferencesDirectoryPath = _startPath + "//" + ApplicationConstants.PREFS_FOLDER_NAME + "//" + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                return _allocationPreferencesDirectoryPath;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets up data table.
        /// </summary>
        /// <param name="accountList">The Account list.</param>
        /// <param name="strategyCollection">The strategy collection.</param>
        /// <returns></returns>
        internal static DataTable SetUpDataTable(Dictionary<int, string> accountList, StrategyCollection strategyCollection)
        {
            DataTable dtTemp = new DataTable();
            try
            {
                DataColumn accountId = new DataColumn();
                accountId.ColumnName = AllocationUIConstants.ACCOUNT_ID;
                accountId.DataType = typeof(int);
                dtTemp.Columns.Add(accountId);

                DataColumn account = new DataColumn();
                account.ColumnName = AllocationUIConstants.ACCOUNT;
                account.Caption = "Name";
                dtTemp.Columns.Add(account);

                DataColumn perAccount = new DataColumn();
                perAccount.ColumnName = AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE;
                perAccount.DataType = typeof(Double);
                perAccount.Caption = "%";
                perAccount.DefaultValue = 0;
                dtTemp.Columns.Add(perAccount);

                DataColumn qtyAccount = new DataColumn();
                qtyAccount.ColumnName = AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY;
                qtyAccount.DataType = typeof(Double);
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
                            perColumn.DataType = typeof(Double);
                            perColumn.Caption = "%";
                            perColumn.DefaultValue = 0;
                            dtTemp.Columns.Add(perColumn);

                            DataColumn qtyColumn = new DataColumn();
                            qtyColumn.ColumnName = AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY;
                            qtyColumn.DataType = typeof(Double);
                            qtyColumn.Caption = "Qty";
                            qtyColumn.DefaultValue = 0;
                            dtTemp.Columns.Add(qtyColumn);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dtTemp;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        internal static ObservableCollection<object> GetCollection(List<int> list, Dictionary<int, string> dictionary)
        {
            ObservableCollection<object> collection = new ObservableCollection<object>();
            try
            {
                foreach (int key in list)
                {
                    if (dictionary.ContainsKey(key))
                    {
                        KeyValuePair<int, string> kvp = new KeyValuePair<int, string>(key, dictionary[key]);
                        collection.Add((object)kvp);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return collection;
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        internal static ObservableCollection<object> GetCollection(bool value, Dictionary<bool, string> dictionary)
        {
            ObservableCollection<object> collection = new ObservableCollection<object>();
            try
            {
                if (dictionary.ContainsKey(value))
                {
                    KeyValuePair<bool, string> kvp = new KeyValuePair<bool, string>(value, dictionary[value]);
                    collection.Add((object)kvp);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return collection;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        internal static List<int> GetList(ObservableCollection<object> collection)
        {
            List<int> list = new List<int>();
            try
            {
                foreach (object obj in collection)
                {
                    KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)obj;
                    list.Add(kvp.Key);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return list;
        }

        /// <summary>
        /// Gets the account value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="isAccountStrategyGrid">if set to <c>true</c> [is account strategy grid].</param>
        /// <returns></returns>
        internal static AccountValue GetAccountValue(DataRow row, bool isAccountStrategyGrid)
        {
            try
            {
                AccountValue accountValue = new AccountValue();
                accountValue.AccountId = Convert.ToInt32(row[AllocationUIConstants.ACCOUNT_ID]);
                accountValue.Value = Convert.ToDecimal(row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE]);

                StrategyCollection strategyCollection = CachedDataManager.GetInstance.GetUserPermittedStrategies();
                if (strategyCollection != null)
                {
                    List<StrategyValue> strategyValueList = new List<StrategyValue>();
                    foreach (Strategy strategy in strategyCollection.Cast<Strategy>().Where(strategy => strategy.StrategyID != int.MinValue))
                    {
                        StrategyValue strategyValue = new StrategyValue();
                        strategyValue.StrategyId = strategy.StrategyID;
                        strategyValue.Value = Convert.ToDecimal(row[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE]);
                        if (row.Table.Columns.Contains(AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY))
                            strategyValue.Quantity = Convert.ToDecimal(row[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY]);
                        else
                            strategyValue.Quantity = 0;

                        if (strategyValue.Value != 0)
                            strategyValueList.Add(strategyValue);
                    }

                    //  Strategy unallocated is not published while reallocating the trade, PRANA-14413
                    if (strategyValueList.Count == 0 && isAccountStrategyGrid)
                    {
                        StrategyValue strategy;
                        if (row.Table.Columns.Contains(AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY))
                        {
                            strategy = new StrategyValue(0, 100, Convert.ToDecimal(row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY]));
                        }
                        else
                        {
                            strategy = new StrategyValue(0, 100, 0);
                        }
                        strategyValueList.Add(strategy);
                    }
                    accountValue.StrategyValueList = strategyValueList;
                }
                return accountValue;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the settlement date.
        /// </summary>
        /// <param name="group">The group.</param>
        internal static DateTime GetSettlementDate(AllocationGroup group)
        {
            DateTime settlementDate = group.SettlementDate;
            try
            {
                int auecID = Convert.ToInt32(group.AUECID);
                string sideText = group.OrderSide;
                if (sideText != "0")
                {
                    string sideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(sideText);
                    int auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(auecID, sideTagValue);
                    DateTime tradeDate = Convert.ToDateTime(group.AUECLocalDate.ToString());
                    settlementDate = auecSettlementPeriod == 0 ? tradeDate : BusinessLogic.BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, auecSettlementPeriod, auecID, true);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return settlementDate;
        }

        /// <summary>
        /// Gets the string from collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        internal static string GetStringFromCollection(ObservableCollection<object> selectedValue)
        {
            try
            {
                if (selectedValue.Count > 0)
                {
                    List<string> values = new List<string>();

                    if (selectedValue[0] is KeyValuePair<int, string>)
                    {
                        foreach (KeyValuePair<int, string> kvp in selectedValue)
                            values.Add(kvp.Value.ToString());
                    }
                    else if (selectedValue[0] is KeyValuePair<string, string>)
                    {
                        foreach (KeyValuePair<string, string> kvp in selectedValue)
                            values.Add(kvp.Value.ToString());
                    }
                    else if (selectedValue[0] is string)
                    {
                        foreach (string kvp in selectedValue)
                            values.Add(kvp);
                    }
                    return string.Join(",", values.ToArray());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// Sets the precision format.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        internal static string SetPrecisionFormat(int precisionDigit)
        {
            string precisionFormat = "nnnn,nnn,nnn,nnn,nnn.";
            try
            {
                for (int i = 0; i < precisionDigit; i++)
                {
                    precisionFormat += "n";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return precisionFormat;
        }

        /// <summary>
        /// Sets the precision string format.
        /// </summary>
        /// <param name="precisionDigit">The precision digit.</param>
        /// <returns></returns>
        internal static string SetPrecisionStringFormatXamGrid(int precisionDigit)
        {
            string precisionFormat = "{0:####,###,###,###,##0.#";
            try
            {
                for (int i = 1; i < precisionDigit; i++)
                {
                    precisionFormat += "#";
                }
                precisionFormat += "}";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return precisionFormat;
        }

        /// <summary>
        /// Sets the precision string format.
        /// </summary>
        /// <param name="precisionDigit">The precision digit.</param>
        /// <returns></returns>
        internal static string SetPrecisionStringFormat(int precisionDigit)
        {
            string precisionFormat = "{0:####,###,###,###,##0.";
            try
            {
                for (int i = 0; i < precisionDigit; i++)
                {
                    precisionFormat += "#";
                }
                precisionFormat += "}";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return precisionFormat;
        }

        /// <summary>
        /// Gets the maximum length of the account.
        /// </summary>
        /// <returns></returns>
        internal static double GetMaxAccountLength()
        {
            double maxAccountLength = 0;
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return maxAccountLength;
        }

        /// <summary>
        /// Creates the item template.
        /// </summary>
        /// <param name="columnKey">The column key.</param>
        /// <returns></returns>
        private static DataTemplate CreateItemTemplate(string columnKey)
        {
            try
            {
                DataTemplate itemTemplate = new DataTemplate();
                itemTemplate.DataType = typeof(Double);

                FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
                textBlock.SetBinding(TextBlock.TextProperty, new Binding(columnKey));

                itemTemplate.VisualTree = textBlock;
                itemTemplate.Seal();
                return itemTemplate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new DataTemplate();
            }
        }

        /// <summary>
        /// Creates the editor template.
        /// </summary>
        /// <param name="columnKey">The column key.</param>
        /// <returns></returns>
        private static DataTemplate CreateEditorTemplate(string columnKey)
        {
            try
            {
                DataTemplate editorTemplate = new DataTemplate();
                editorTemplate.DataType = typeof(Double);

                FrameworkElementFactory numericEditor = new FrameworkElementFactory(typeof(XamNumericEditor));
                numericEditor.SetBinding(XamNumericEditor.ValueProperty, new Binding(columnKey) { Mode = BindingMode.TwoWay });

                editorTemplate.VisualTree = numericEditor;
                editorTemplate.Seal();
                return editorTemplate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new DataTemplate();
            }
        }

        /// <summary>
        /// Creates the filter item template.
        /// </summary>
        /// <returns></returns>
        private static DataTemplate CreateFilterItemTemplate()
        {
            try
            {
                DataTemplate itemTemplate = new DataTemplate();
                itemTemplate.DataType = typeof(Double);

                FrameworkElementFactory filterItem = new FrameworkElementFactory(typeof(TextBlock));
                filterItem.SetBinding(TextBlock.TextProperty, new Binding("Value"));

                itemTemplate.VisualTree = filterItem;
                itemTemplate.Seal();
                return itemTemplate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new DataTemplate();
            }
        }

        /// <summary>
        /// Creates the filter editor template.
        /// </summary>
        /// <returns></returns>
        private static DataTemplate CreateFilterEditorTemplate()
        {
            try
            {
                DataTemplate editorTemplate = new DataTemplate();
                editorTemplate.DataType = typeof(Double);

                FrameworkElementFactory filterEditor = new FrameworkElementFactory(typeof(XamNumericEditor));
                filterEditor.SetBinding(XamNumericEditor.ValueProperty, new Binding("Value") { Mode = BindingMode.TwoWay });

                editorTemplate.VisualTree = filterEditor;
                editorTemplate.Seal();
                return editorTemplate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return new DataTemplate();
            }
        }

        /// <summary>
        /// Gets the default type of the value for data.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static object GetDefaultValueForDataType(Type type)
        {
            try
            {
                switch (type.Name)
                {
                    case "String":
                        return string.Empty;

                    case "Double":
                    case "Float":
                        return 0.0;

                    case "Int":
                    case "Long":
                        return 0;

                    case "Decimal":
                        return 0.0M;

                    case "DateTime":
                        return DateTime.Today;

                    default:
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return string.Empty;
            }
        }

        /// <summary>
        /// Shows the unmodified groups details.
        /// </summary>
        /// <param name="groupsNotUpdated">The groups not updated.</param>
        internal static void ShowUnmodifiedGroupsDetails(StringBuilder groupsNotUpdated)
        {
            try
            {
                String path = System.Windows.Forms.Application.StartupPath + @"\Logs\UnModifiedGroups.txt";
                using (StreamWriter streamWriter = new StreamWriter(path, false))
                {
                    streamWriter.WriteLine(Environment.NewLine + DateTime.Now);
                    streamWriter.Write(groupsNotUpdated.ToString());
                    StringBuilder boxMessage = new StringBuilder();
                    boxMessage.AppendLine("Some groups could not be modified.");
                    boxMessage.Append("Do you want to view details?");
                    MessageBoxResult dr = MessageBox.Show(boxMessage.ToString(), AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (dr == MessageBoxResult.Yes)
                        System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the user choice collection.
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<bool, string> GetUserChoiceCollection()
        {
            Dictionary<bool, string> userChoiceDict = new Dictionary<bool, string>();
            try
            {
                userChoiceDict.Add(true, "Recalculate");
                userChoiceDict.Add(false, "Keep commission values");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return userChoiceDict;
        }

        /// <summary>
        /// Gets the order sides.
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, string> GetOrderSides()
        {
            try
            {
                Dictionary<string, string> orderSides = new Dictionary<string, string>();
                orderSides.Add(FIXConstants.SIDE_Buy, "Buy");
                orderSides.Add(FIXConstants.SIDE_Buy_Closed, "Buy to Close");
                orderSides.Add(FIXConstants.SIDE_Buy_Open, "Buy to Open");
                orderSides.Add(FIXConstants.SIDE_Sell, "Sell");
                orderSides.Add(FIXConstants.SIDE_Sell_Closed, "Sell to Close");
                orderSides.Add(FIXConstants.SIDE_Sell_Open, "Sell to Open");
                orderSides.Add(FIXConstants.SIDE_SellShort, "Sell short");

                return orderSides;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Used to set the captions after splitting the propertyName of the columns on allocation UI, PRANA-13184
        /// </summary>
        /// <param name="colHeader"></param>
        /// <returns></returns>
        internal static string SplitCamelCase(string colHeader)
        {
            try
            {
                string fieldName = System.Text.RegularExpressions.Regex.Replace(colHeader, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
                return System.Text.RegularExpressions.Regex.Replace(fieldName, @"\s+", " ");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the template column.
        /// </summary>
        /// <param name="columnKey">The column key.</param>
        /// <param name="columnHeader">The column header.</param>
        /// <param name="isReadOnly">if set to <c>true</c> [is read only].</param>
        /// <param name="summaryOperands">The summary operands.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns></returns>
        internal static TemplateColumn GetTemplateColumn(string columnKey, string columnHeader, bool isReadOnly, List<object> summaryOperands, Style cellStyle)
        {
            TemplateColumn templateColumn = new TemplateColumn();
            try
            {
                templateColumn.Key = columnKey;
                templateColumn.HeaderText = columnHeader;
                templateColumn.IsReadOnly = isReadOnly;
                templateColumn.MinimumWidth = 50;
                templateColumn.ItemTemplate = CreateItemTemplate(columnKey);
                templateColumn.EditorTemplate = CreateEditorTemplate(columnKey);
                templateColumn.FilterItemTemplate = CreateFilterItemTemplate();
                templateColumn.FilterEditorTemplate = CreateFilterEditorTemplate();
                summaryOperands.ForEach(x => templateColumn.SummaryColumnSettings.SummaryOperands.Add(x as SummaryOperandBase));
                if (cellStyle != null)
                    templateColumn.CellStyle = cellStyle;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return templateColumn;
        }

        /// <summary>
        /// Sets the precision excel format.
        /// </summary>
        /// <param name="precisionDigit">The precision digit.</param>
        /// <returns></returns>
        internal static string SetPrecisionExcelFormat(int precisionDigit)
        {
            string precisionFormat = "####,###,###,###,##0.00";
            try
            {
                for (int i = 1; i < (precisionDigit - 1); i++)
                {
                    precisionFormat += "#";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return precisionFormat;
        }

        /// <summary>
        /// Gets the Master Fund associated accounts.
        /// </summary>
        /// <param name="masterFundId">The master fund identifier.</param>
        /// <returns></returns>
        internal static Dictionary<int, string> GetMFAssociatedAccounts(int masterFundId)
        {
            Dictionary<int, string> accountList = new Dictionary<int, string>();
            try
            {
                Dictionary<int, List<int>> mfAssociatedAccounts = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                List<int> masterFundAccountIds = (mfAssociatedAccounts.ContainsKey(masterFundId)) ? mfAssociatedAccounts[masterFundId] : new List<int>();
                masterFundAccountIds.ForEach(accountId =>
                {
                    if (CachedDataManager.GetInstance.GetUserAccountsAsDict().ContainsKey(accountId))
                        accountList.Add(accountId, CachedDataManager.GetInstance.GetAccount(accountId));
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountList;
        }


        /// <summary>
        /// Gets the XML as string.
        /// </summary>
        /// <param name="myxml">The myxml.</param>
        /// <returns>XML As String</returns>
        internal static string GetXMLAsString(XmlDocument myxml)
        {
            string str = string.Empty;
            try
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter tx = new XmlTextWriter(sw);
                myxml.WriteTo(tx);
                str = sw.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return str;
        }

        /// <summary>
        /// Gets the array of allocation scheme keys.
        /// </summary>
        /// <returns></returns>
        internal static string[] GetAllocationSchemeKeys()
        {
            string[] allocationSchemeKeys = null;
            try
            {
                allocationSchemeKeys = Enum.GetNames(typeof(AllocationSchemeKey));
                //Removing PBSymbolSide from Dropdown as procedures for getting positions from database not available yet
                int numIndex = Array.IndexOf(allocationSchemeKeys, "PBSymbolSide");
                allocationSchemeKeys = allocationSchemeKeys.Where((val, idx) => idx != numIndex).ToArray();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationSchemeKeys;
        }

        /// <summary>
        /// Gets the Commission Prefrence.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        internal static KeyValuePair<bool, string> GetCommissionPreference(bool value, Dictionary<bool, string> dictionary)
        {
            KeyValuePair<bool, string> kvp = new KeyValuePair<bool, string>();
            try
            {
                if (dictionary.ContainsKey(value))
                {
                    kvp = new KeyValuePair<bool, string>(value, dictionary[value]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return kvp;
        }

        /// <summary>
        /// Gets the allocation distribution dictionary.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// <returns></returns>
        internal static SerializableDictionary<int, AccountValue> GetAllocationDistributionDict(AllocationGroup allocationGroup)
        {
            SerializableDictionary<int, AccountValue> targetDict = new SerializableDictionary<int, AccountValue>();
            try
            {
                decimal qty = allocationGroup.Allocations.Collection.Sum(x => Convert.ToDecimal(x.AllocatedQty));
                foreach (AllocationLevelClass allocation in allocationGroup.Allocations.Collection)
                {
                    decimal percentage = qty == 0 ? 0 : ((decimal)allocation.AllocatedQty * 100) / qty;
                    AccountValue account = new AccountValue(allocation.LevelnID, percentage);
                    if (allocation.Childs != null)
                    {
                        decimal accountQty = allocation.Childs.Collection.Sum(x => Convert.ToDecimal(x.AllocatedQty));
                        foreach (AllocationLevelClass strategy in allocation.Childs.Collection)
                        {
                            decimal per = accountQty == 0 ? 0 : ((decimal)strategy.AllocatedQty * 100) / accountQty;
                            StrategyValue strategyValue = new StrategyValue(strategy.LevelnID, per, (decimal)strategy.AllocatedQty);
                            account.StrategyValueList.Add(strategyValue);
                        }
                    }
                    if (targetDict.ContainsKey(account.AccountId))
                        targetDict[account.AccountId] = account;
                    else
                        targetDict.Add(account.AccountId, account);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return targetDict;
        }
    }
}
