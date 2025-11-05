using Prana.AlgoStrategyControls;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public class ImportHelper : IDisposable
    {
        #region constants
        const string _importTypeCash = "Cash";
        const string _importTypeNetPosition = "Net Position";
        const string _importTypeStagedOrder = "Staged Order";
        const string _importTypeTransaction = "Transaction";
        const string _importTypeMarkPrice = "Mark Price";
        const string _importTypeForexPrice = "Forex Price";
        const string _importTypeActivity = "Activities";
        const string _importTypeDailyBeta = "DailyBeta";
        const string _importTypeDailyCreditLimit = "Credit Limit";
        const string _importTypeSecMasterInsert = "SMInsert";
        const string _importTypeSecMasterUpdate = "SMUpdate";
        const string _importTypeOMI = "Option Model Inputs";
        const string _importTypeAllocationScheme = "Allocation Scheme";
        const string _importTypeAllocationScheme_AppPositions = "Allocation Scheme AppPositions";
        const string _importTypeDoubleEntryCash = "Double Entry Cash";
        const string _importTypeSettlementDateCash = "SettlementDate Cash";
        private const string _importTypeDailyVolatility = "Daily Volatility";
        private const string _importTypeDailyVWAP = "Daily VWAP";
        private const string _importTypeCollateralPrice = "Collateral Price";
        private const string _importTypeDailyDividendYield = "Daily Dividend Yield";
        private const string _importTypeCollateralInterest = "Collateral Interest";
        private const string _importTypeMultilegJournalImport = "Multileg Journal Import";
        const string CAPTION_True = "TRUE";
        #endregion
        public EventHandler SendUpdateAfterSymbolValidation = null;
        public Dictionary<int, Dictionary<string, List<OrderSingle>>> StageSymbolWiseDict { get; set; }
        public ProxyBase<IAllocationManager> AllocationServices { get; set; }
        public List<OrderSingle> StageOrderCollection { get; set; }
        public List<string> CurrencyListForAlloScheme { get; set; }

        public ImportHelper()
        {
            AllocationServices = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
        }

        /// <summary>
        /// This method is for preprocessing the uploaded data
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public DataTable ArrangeTable(DataTable dataSource)
        {
            try
            {
                // what XML we will generate, all the tagname will be like COL1,COL2 .                
                dataSource.TableName = "PositionMaster";

                // update the Table columns value with "*" where columns value blank in the excel sheet
                // when we generate the XML for that table, the blank coluns do not comes in the generated XML
                // the indexing of the generated XML changed because of blank columns
                // so defalut value of the columns will be  "*"
                for (int irow = 0; irow < dataSource.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dataSource.Columns.Count; icol++)
                    {
                        string val = dataSource.Rows[irow].ItemArray[icol].ToString();
                        if (String.IsNullOrEmpty(val.Trim()))
                        {
                            dataSource.Rows[irow][icol] = "*";
                        }
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
            return dataSource;
        }

        /// <summary>
        /// This method is for generating XML from datasource
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="strXSLTName"></param>
        /// <param name="strTableType"></param>
        /// <returns></returns>
        public string GenerateXML(DataTable dataSource, string strXSLTName, string strTableType)
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + @"\xmls\Transformation\Temp"))
                    Directory.CreateDirectory(Application.StartupPath + @"\xmls\Transformation\Temp");
                string serializedPMXml = Application.StartupPath + @"\xmls\Transformation\Temp\serializedXMLforPM.xml";
                dataSource.WriteXml(serializedPMXml);
                // get a new mapped xml
                string mappedxml = Application.StartupPath + @"\xmls\Transformation\Temp\ConvertedXMLforPM.xml";
                // set the XSLT path as StartUp Path
                string dirPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString();
                string strXSLTStartUpPath = Application.StartupPath + "\\" + dirPath + "\\" + strXSLTName;
                //                                                  serializedXML,MappedserXML, XSLTPath             
                string xsdPath = Application.StartupPath + "\\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + "\\" + ApplicationConstants.MappingFileType.PranaXSD.ToString();
                string mappedfilePath = XMLUtilities.GetTransformed(serializedPMXml, mappedxml, strXSLTStartUpPath);

                bool isXmlValidated = ValidateConvertedXML(mappedfilePath, strTableType, xsdPath);
                if (isXmlValidated)
                {
                    return mappedfilePath;
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
            return string.Empty;
        }


        /// <summary>
        /// This method is for validating the XML and creating a transformed xml
        /// </summary>
        /// <param name="convertedXMLPath"></param>
        /// <param name="tableType"></param>
        /// <param name="xsdPath"></param>
        /// <returns></returns>
        private bool ValidateConvertedXML(string convertedXMLPath, string tableType, string xsdPath)
        {
            bool isValidated = false;
            string tmpError;
            try
            {
                switch (tableType)
                {
                    case _importTypeCash:

                        xsdPath = xsdPath + @"\ImportCash.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeTransaction:
                    case _importTypeNetPosition:

                        xsdPath = xsdPath + @"\ImportPositions.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeMarkPrice:

                        xsdPath = xsdPath + @"\ImportMarkPrice.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeForexPrice:

                        xsdPath = xsdPath + @"\ImportForexPrice.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;

                    case _importTypeDailyBeta:

                        xsdPath = xsdPath + @"\ImportBetaValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeActivity:

                        xsdPath = xsdPath + @"\ImportActivities.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeCollateralInterest:
                        xsdPath = xsdPath + @"\ImportCollateralInterestInsert.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeSecMasterInsert:
                    case _importTypeSecMasterUpdate:
                        xsdPath = xsdPath + @"\ImportSecMasterInsertAndUpdate.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeOMI:
                        xsdPath = xsdPath + @"\OptionModelInputs.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    //case _importTypeCashTransactions:
                    //    xsdPath = xsdPath + @"\ImportCashTransactions.xsd";
                    //    isValidated = XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "");
                    //    break;
                    case _importTypeAllocationScheme:
                    case _importTypeAllocationScheme_AppPositions:
                        xsdPath = xsdPath + @"\ImportAllocationScheme.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyCreditLimit:
                        xsdPath = xsdPath + @"\ImportDailyCreditLimit.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDoubleEntryCash:
                        xsdPath = xsdPath + @"\DoubleEntryCash.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeMultilegJournalImport:
                        xsdPath = xsdPath + @"\MultilegJournalImport.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeSettlementDateCash:
                        xsdPath = xsdPath + @"\ImportSettlementDateCash.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyVolatility:
                        xsdPath = xsdPath + @"\ImportVolatilityValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyVWAP:
                        xsdPath = xsdPath + @"\ImportVWAPValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeCollateralPrice:
                        xsdPath = xsdPath + @"\ImportCollateralPrice.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyDividendYield:
                        xsdPath = xsdPath + @"\ImportDividendYieldValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeStagedOrder:
                        xsdPath = xsdPath + @"\ImportBlotterTrades.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    default:
                        isValidated = false;
                        break;
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
            return isValidated;
        }

        /// <summary>
        /// Now we have arranged and updated XML
        /// as above we inserted "*" in the blank columns, but "*" needs extra treatment, so        
        ///again we replace the "*" with blank string, the following looping does the same
        /// </summary>
        /// <param name="ds"></param>
        public void ReArrangeDataSet(DataSet ds)
        {
            try
            {
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string val = ds.Tables[0].Rows[irow].ItemArray[icol].ToString();
                        if (val.Equals("*"))
                        {
                            ds.Tables[0].Rows[irow][icol] = string.Empty;
                        }
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
        /// Update Stage Order Collection
        /// </summary>
        /// <param name="ds"></param>
        public void UpdateStageOrderCollection(DataSet ds, int _userSelectedAccountValue, string _isUserSelectedDate, string _userSelectedDate)
        {
            try
            {
                StageSymbolWiseDict.Clear();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(OrderSingle).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    OrderSingle order = new OrderSingle();
                    order.Symbol = string.Empty;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(order, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(order, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(order, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(order, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(order, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(order, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(order, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(order, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(order, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(order, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(order, 0, null);
                                    }
                                }
                            }
                        }
                    }

                    order.TransactionTime = DateTime.Now.ToUniversalTime();

                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {

                        TimeSpan duration = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                        order.TransactionTime = (Convert.ToDateTime(_userSelectedDate)).Date.Add(duration).ToUniversalTime();
                    }
                    if (order.Account.Equals("-"))
                        order.Account = string.Empty;

                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        order.Account = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                        order.Level1ID = _userSelectedAccountValue;
                    }
                    else if (!string.IsNullOrEmpty(order.Account) && order.Level1ID <= 0)
                    {
                        order.Level1ID = int.MinValue;
                        int accountId = CachedDataManager.GetInstance.GetAccountID(order.Account.Trim());
                        if (accountId != int.MinValue)
                        {
                            order.Level1ID = accountId;
                        }
                        else
                        {
                            order.Level1ID = AllocationServices.InnerChannel.GetAllocationPreferenceIdByName(order.Account.Trim());
                        }
                    }
                    else if (order.Level1ID > 0)
                    {
                        if (!string.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccountText(order.Level1ID)))
                        {
                            order.Account = CachedDataManager.GetInstance.GetAccountText(order.Level1ID);
                        }
                        else if (!string.IsNullOrEmpty(AllocationServices.InnerChannel.GetAllocationPreferenceNameById(order.Level1ID)) && AllocationServices.InnerChannel.GetAllocationPreferenceNameById(order.Level1ID) != "Manual")
                        {
                            order.Account = AllocationServices.InnerChannel.GetAllocationPreferenceNameById(order.Level1ID);
                        }
                        else
                        {
                            order.Account = string.Empty;
                        }
                    }

                    if (order.Level2ID < 0 && !string.IsNullOrEmpty(order.Strategy))
                    {
                        order.Level2ID = CachedDataManager.GetInstance.GetStrategyID(order.Strategy.Trim());
                    }

                    order.CompanyUserID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    order.ActualCompanyUserID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    order.TransactionSourceTag = (int)TransactionSource.TradeImport;
                    order.TransactionSource = TransactionSource.TradeImport;
                    order.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                    order.ClientTime = DateTime.Now.ToUniversalTime().ToLongTimeString();
                    order.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingNew.ToString());
                    if (CommonDataCache.CachedDataManager.GetInstance.GetUserTradingAccounts().Count > 0)
                        order.TradingAccountID = ((Prana.BusinessObjects.TradingAccount)CommonDataCache.CachedDataManager.GetInstance.GetUserTradingAccounts()[0]).TradingAccountID;

                    if (!String.IsNullOrEmpty(order.Symbol))
                    {
                        if (StageSymbolWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<OrderSingle>> stageSameSymbologyDict = StageSymbolWiseDict[0];
                            if (stageSameSymbologyDict.ContainsKey(order.Symbol))
                            {
                                List<OrderSingle> stageSymbolWiseList = stageSameSymbologyDict[order.Symbol];
                                stageSymbolWiseList.Add(order);
                                stageSameSymbologyDict[order.Symbol] = stageSymbolWiseList;
                                StageSymbolWiseDict[0] = stageSameSymbologyDict;
                            }
                            else
                            {
                                List<OrderSingle> orderList = new List<OrderSingle>();
                                orderList.Add(order);
                                StageSymbolWiseDict[0].Add(order.Symbol, orderList);
                            }
                        }
                        else
                        {
                            List<OrderSingle> orderList = new List<OrderSingle>();
                            orderList.Add(order);
                            Dictionary<string, List<OrderSingle>> orderSameSymbolDict = new Dictionary<string, List<OrderSingle>>();
                            orderSameSymbolDict.Add(order.Symbol, orderList);
                            StageSymbolWiseDict.Add(0, orderSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(order.ISINSymbol))
                    {
                        if (StageSymbolWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<OrderSingle>> stageSameSymbologyDict = StageSymbolWiseDict[2];
                            if (stageSameSymbologyDict.ContainsKey(order.ISINSymbol))
                            {
                                List<OrderSingle> stageISINWiseList = stageSameSymbologyDict[order.ISINSymbol];
                                stageISINWiseList.Add(order);
                                stageSameSymbologyDict[order.ISINSymbol] = stageISINWiseList;
                                StageSymbolWiseDict[2] = stageSameSymbologyDict;
                            }
                            else
                            {
                                List<OrderSingle> stagelist = new List<OrderSingle>();
                                stagelist.Add(order);
                                StageSymbolWiseDict[2].Add(order.ISINSymbol, stagelist);
                            }
                        }
                        else
                        {
                            List<OrderSingle> stagelist = new List<OrderSingle>();
                            stagelist.Add(order);
                            Dictionary<string, List<OrderSingle>> stageSameISINDict = new Dictionary<string, List<OrderSingle>>();
                            stageSameISINDict.Add(order.ISINSymbol, stagelist);
                            StageSymbolWiseDict.Add(2, stageSameISINDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(order.SEDOLSymbol))
                    {
                        if (StageSymbolWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<OrderSingle>> stageSameSymbologyDict = StageSymbolWiseDict[3];
                            if (stageSameSymbologyDict.ContainsKey(order.SEDOLSymbol))
                            {
                                List<OrderSingle> stageSEDOLWiseList = stageSameSymbologyDict[order.SEDOLSymbol];
                                stageSEDOLWiseList.Add(order);
                                stageSameSymbologyDict[order.SEDOLSymbol] = stageSEDOLWiseList;
                                StageSymbolWiseDict[3] = stageSameSymbologyDict;
                            }
                            else
                            {
                                List<OrderSingle> stagelist = new List<OrderSingle>();
                                stagelist.Add(order);
                                StageSymbolWiseDict[3].Add(order.SEDOLSymbol, stagelist);
                            }
                        }
                        else
                        {
                            List<OrderSingle> stagelist = new List<OrderSingle>();
                            stagelist.Add(order);
                            Dictionary<string, List<OrderSingle>> stageSEDOLDict = new Dictionary<string, List<OrderSingle>>();
                            stageSEDOLDict.Add(order.SEDOLSymbol, stagelist);
                            StageSymbolWiseDict.Add(3, stageSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(order.CusipSymbol))
                    {
                        if (StageSymbolWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<OrderSingle>> stageSameSymbologyDict = StageSymbolWiseDict[4];
                            if (stageSameSymbologyDict.ContainsKey(order.CusipSymbol))
                            {
                                List<OrderSingle> stageCUSIPWiseList = stageSameSymbologyDict[order.CusipSymbol];
                                stageCUSIPWiseList.Add(order);
                                stageSameSymbologyDict[order.CusipSymbol] = stageCUSIPWiseList;
                                StageSymbolWiseDict[4] = stageSameSymbologyDict;
                            }
                            else
                            {
                                List<OrderSingle> stagelist = new List<OrderSingle>();
                                stagelist.Add(order);
                                StageSymbolWiseDict[4].Add(order.CusipSymbol, stagelist);
                            }
                        }
                        else
                        {
                            List<OrderSingle> stagelist = new List<OrderSingle>();
                            stagelist.Add(order);
                            Dictionary<string, List<OrderSingle>> stageSameCUSIPDict = new Dictionary<string, List<OrderSingle>>();
                            stageSameCUSIPDict.Add(order.CusipSymbol, stagelist);
                            StageSymbolWiseDict.Add(4, stageSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(order.BloombergSymbol))
                    {
                        if (StageSymbolWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<OrderSingle>> stageSameSymbologyDict = StageSymbolWiseDict[5];
                            if (stageSameSymbologyDict.ContainsKey(order.BloombergSymbol))
                            {
                                List<OrderSingle> stageBloombergWiseList = stageSameSymbologyDict[order.BloombergSymbol];
                                stageBloombergWiseList.Add(order);
                                stageSameSymbologyDict[order.BloombergSymbol] = stageBloombergWiseList;
                                StageSymbolWiseDict[5] = stageSameSymbologyDict;
                            }
                            else
                            {
                                List<OrderSingle> stagelist = new List<OrderSingle>();
                                stagelist.Add(order);
                                StageSymbolWiseDict[5].Add(order.BloombergSymbol, stagelist);
                            }
                        }
                        else
                        {
                            List<OrderSingle> stagelist = new List<OrderSingle>();
                            stagelist.Add(order);
                            Dictionary<string, List<OrderSingle>> stageSameBloombergDict = new Dictionary<string, List<OrderSingle>>();
                            stageSameBloombergDict.Add(order.BloombergSymbol, stagelist);
                            StageSymbolWiseDict.Add(5, stageSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(order.IDCOSymbol))
                    {
                        if (StageSymbolWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<OrderSingle>> stageSameSymbologyDict = StageSymbolWiseDict[7];
                            if (stageSameSymbologyDict.ContainsKey(order.IDCOSymbol))
                            {
                                List<OrderSingle> stageIDCOWiseList = stageSameSymbologyDict[order.IDCOSymbol];
                                stageIDCOWiseList.Add(order);
                                stageSameSymbologyDict[order.IDCOSymbol] = stageIDCOWiseList;
                                StageSymbolWiseDict[7] = stageSameSymbologyDict;
                            }
                            else
                            {
                                List<OrderSingle> stagelist = new List<OrderSingle>();
                                stagelist.Add(order);
                                StageSymbolWiseDict[7].Add(order.IDCOSymbol, stagelist);
                            }
                        }
                        else
                        {
                            List<OrderSingle> stagelist = new List<OrderSingle>();
                            stagelist.Add(order);
                            Dictionary<string, List<OrderSingle>> stageSameIDCODict = new Dictionary<string, List<OrderSingle>>();
                            stageSameIDCODict.Add(order.IDCOSymbol, stagelist);
                            StageSymbolWiseDict.Add(7, stageSameIDCODict);
                        }
                    }

                    order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(order.OrderSideTagValue);
                    order.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(order.OrderTypeTagValue);
                    StageOrderCollection.Add(order);
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
        /// Get SM Data for stage
        /// </summary>
        /// <param name="_securityMaster"></param>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        public bool GetSMDataForStageImport(ISecurityMasterServices _securityMaster, int hashCode)
        {
            bool _allSymbolsAvailableInCache = false;
            try
            {
                if (StageSymbolWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<OrderSingle>>> kvp in StageSymbolWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<OrderSingle>> symbolDict = StageSymbolWiseDict[kvp.Key];
                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<OrderSingle>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = hashCode;
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);
                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {                        
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            int requestedSymbologyID = secMasterObj.RequestedSymbology;
                            if (StageSymbolWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<OrderSingle>> dictSymbols = StageSymbolWiseDict[requestedSymbologyID];
                                if (secMasterCollection.Count == dictSymbols.Count)
                                {
                                    _allSymbolsAvailableInCache = true;
                                }
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<OrderSingle> listStageOrders = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (OrderSingle stageOrder in listStageOrders)
                                    {
                                        stageOrder.AssetID = secMasterObj.AssetID;
                                        stageOrder.UnderlyingID = secMasterObj.UnderLyingID;
                                        stageOrder.ExchangeID = secMasterObj.ExchangeID;
                                        stageOrder.CurrencyID = secMasterObj.CurrencyID;
                                        stageOrder.AUECID = secMasterObj.AUECID;
                                        stageOrder.Symbol = secMasterObj.TickerSymbol;
                                        stageOrder.BloombergSymbol = secMasterObj.BloombergSymbol;
                                        stageOrder.ISINSymbol = secMasterObj.ISINSymbol;
                                        stageOrder.CusipSymbol = secMasterObj.CusipSymbol;
                                        stageOrder.SEDOLSymbol = secMasterObj.SedolSymbol;
                                        stageOrder.IDCOSymbol = secMasterObj.IDCOOptionSymbol;
                                        stageOrder.SettlementCurrencyID = secMasterObj.CurrencyID;
                                        stageOrder.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                                        stageOrder.AssetName = CachedDataManager.GetInstance.GetAssetText(stageOrder.AssetID);
                                    }
                                    if (SendUpdateAfterSymbolValidation != null)
                                    {
                                        SendUpdateAfterSymbolValidation(this, null);
                                    }
                                }
                            }
                        }
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
            return _allSymbolsAvailableInCache;
        }

        /// <summary>
        /// Validate And Update Stage order
        /// </summary>
        public void ValidateAndUpdate()
        {
            try
            {
                if (StageOrderCollection.Count > 0)
                {
                    foreach (OrderSingle order in StageOrderCollection)
                    {
                        CheckValidationOnAllFileds(order);
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
        /// Check Validation On All Fileds
        /// </summary>
        /// <param name="orderSingle"></param>
        private void CheckValidationOnAllFileds(OrderSingle orderSingle)
        {
            try
            {
                orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                if (string.IsNullOrEmpty(orderSingle.Symbol) || string.IsNullOrEmpty(orderSingle.OrderSideTagValue) || orderSingle.AUECID <= 0 || orderSingle.Quantity <= 0 || orderSingle.CounterPartyID <= 0 || orderSingle.AssetID <= 0)
                {

                    if (orderSingle.AUECID <= 0 || orderSingle.Quantity < 0 || orderSingle.CounterPartyID <= 0 || orderSingle.AssetID <= 0)
                    {
                        orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.NotExists.ToString();
                    }
                    else
                    {
                        orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(orderSingle.Account) && orderSingle.Level1ID <= 0)
                {
                    orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                }

                if (string.IsNullOrEmpty(orderSingle.Account) && orderSingle.Level1ID > 0)
                {
                    orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                }

                if (String.IsNullOrEmpty(orderSingle.OrderSide))
                {
                    orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                }

                if (String.IsNullOrEmpty(orderSingle.OrderType))
                {
                    orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                }

                if (String.IsNullOrEmpty(orderSingle.ExecutionInstruction))
                {
                    orderSingle.ExecutionInstruction = "G";
                }
                if (String.IsNullOrEmpty(orderSingle.HandlingInstruction))
                {
                    orderSingle.HandlingInstruction = "3";
                }
                if (String.IsNullOrEmpty(orderSingle.TIF))
                {
                    orderSingle.TIF = "0";
                }

                SetExpireTime(orderSingle);

                if (String.IsNullOrEmpty(TagDatabaseManager.GetInstance.GetTIFText(orderSingle.TIF)))
                {
                    orderSingle.ValidationStatus = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                }
                AlgoPropertiesHelper.SetAlgoParameters(orderSingle);
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
        /// Set Expire Time
        /// </summary>
        /// <param name="orderSingle"></param>
        public void SetExpireTime(OrderSingle orderSingle)
        {
            try
            {
                if (!String.IsNullOrEmpty(orderSingle.TIF) && orderSingle.TIF.Equals(FIXConstants.TIF_GTD))
                {
                    DateTime dtValue;
                    if (DateTime.TryParse(orderSingle.ExpireTime, out dtValue))
                    {
                        DateTime Dt = dtValue.Date;
                        DateTime TimeStamp = Prana.ClientCommon.MarketStartEndClearanceTimes.GetInstance().GetAUECMarketEndTime(orderSingle.AUECID);
                        orderSingle.ExpireTime = new DateTime(Dt.Year, Dt.Month, Dt.Day, TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second).ToString();
                    }
                    else
                    {
                        orderSingle.ExpireTime = string.Empty;
                    }
                }
                else
                {
                    orderSingle.ExpireTime = string.Empty;
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
        /// Groups Staged Orders and generates allocation Scheme based on the orders
        /// </summary>
        /// <param name="FileName">Name of imported file</param>
        public void GroupStagedOrders(string FileName, ref DataSet dsAllocationScheme)
        {
            try
            {
                ApplicationConstants.SymbologyCodes code = ApplicationConstants.SymbologyCodes.TickerSymbol;
                int symbology = (int)code;
                AccountCollection accountCollection = CachedDataManager.GetInstance.GetUserAccounts();

                //Grouping Staged Orders
                StageImportDataList stageImportDataList = new StageImportDataList();
                foreach (OrderSingle order in StageOrderCollection)
                {
                    stageImportDataList.Add(symbology, order.Symbol, order, accountCollection);
                }
                StageOrderCollection.Clear();
                foreach (StageImportData data in stageImportDataList)
                {
                    List<OrderSingle> list = data.getOrderSingleList();
                    if (list != null)
                        StageOrderCollection.AddRange(list);
                }

                //Creating new scheme based on imported orders
                DataTable schemeTable = stageImportDataList.GetScheme();
                if (schemeTable != null)
                {
                    foreach (DataRow row in schemeTable.Rows)
                    {
                        row["FundName"] = CachedDataManager.GetInstance.GetAccount(int.Parse(row["FundID"].ToString()));
                    }

                    DataSet schemeSet = new DataSet();
                    schemeSet.Tables.Add(schemeTable);
                    schemeSet.DataSetName = "DocumentElement";
                    dsAllocationScheme = AllocationSchemeImportHelper.UpdateAllocationSchemeValueCollection(schemeSet, CurrencyListForAlloScheme, false);
                    dsAllocationScheme = AllocationSchemeImportHelper.GetUpdatedDataSet(dsAllocationScheme);
                    AllocationFixedPreference fixedPref = new AllocationFixedPreference(int.MinValue, FileName, dsAllocationScheme.GetXml(), DateTime.Now, false, FixedPreferenceCreationSource.StagedOrderImport);
                    int schemeID = AllocationServices.InnerChannel.SaveAllocationScheme(fixedPref);

                    foreach (OrderSingle order in StageOrderCollection)
                    {
                        if (order.Level1ID == -1)
                        {
                            order.OriginalAllocationPreferenceID = schemeID;
                            order.Level1ID = schemeID;
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

        /// <summary>
        /// To Dispose the data.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (AllocationServices != null)
                {
                    AllocationServices.Dispose();
                }
            }
        }

        /// <summary>
        /// To Dispose the data.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
