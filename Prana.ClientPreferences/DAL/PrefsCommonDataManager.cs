using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.TTPrefs;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace Prana.ClientPreferences
{
    public class PrefsCommonDataManager
    {
        public static ConfirmationPopUp GetConfirmationPopUpPreferences(int _userID)
        {
            ConfirmationPopUp confirmationPopUp = new ConfirmationPopUp();
            confirmationPopUp.CompanyUserID = _userID;
            object[] parameter = new object[1];
            parameter[0] = _userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingTicketPrefrencesPopUps", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        FillConfirmationPopUpPrefrences(row, 0, ref confirmationPopUp);
                    }
                }
            }
            #region Catch
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
            #endregion
            return confirmationPopUp;



        }
        public static void FillConfirmationPopUpPrefrences(object[] row, int offSet, ref ConfirmationPopUp confirmationPopUp)
        {
            int ISNewOrder = 0 + offSet;
            int ISCXL = 1 + offSet;
            int ISCXLReplace = 2 + offSet;
            int ISManualOrder = 3 + offSet;


            try
            {
                if (row[ISNewOrder] != System.DBNull.Value)
                {
                    confirmationPopUp.ISNewOrder = Convert.ToBoolean(row[ISNewOrder].ToString());
                }
                if (row[ISCXL] != System.DBNull.Value)
                {
                    confirmationPopUp.ISCXL = Convert.ToBoolean(row[ISCXL]);
                }
                if (row[ISCXLReplace] != System.DBNull.Value)
                {
                    confirmationPopUp.ISCXLReplace = Convert.ToBoolean(row[ISCXLReplace]);
                }
                if (row[ISManualOrder] != System.DBNull.Value)
                    confirmationPopUp.IsManualOrder = Convert.ToBoolean(row[ISManualOrder]);
            }

            #region Catch
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
            #endregion
        }

        #region Risk and Symbol Validation
        public static PriceSymbolValidation GetRiskValidationPreferences(int _userID)
        {
            Prana.BusinessObjects.PriceSymbolValidation riskValidation = new Prana.BusinessObjects.PriceSymbolValidation();

            object[] parameter = new object[1];
            parameter[0] = _userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTTRiskValidateSettings", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        riskValidation.RiskCtrlCheck = Convert.ToBoolean(row[0]);
                        riskValidation.ValidateSymbolCheck = Convert.ToBoolean(row[1]);
                        riskValidation.RiskValue = double.Parse(row[2].ToString());
                        riskValidation.CompanyUserID = _userID;
                        riskValidation.LimitPriceCheck = Convert.ToBoolean(row[3]);
                        //riskValidation.SetExecutedQtytoZero = Convert.ToBoolean(row[4]);
                        riskValidation.SetExecutedQtytoZero = false;
                    }
                }
            }
            #region Catch
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
            #endregion
            return riskValidation;
        }

        public static bool SaveRiskValidationPreferences(Prana.BusinessObjects.PriceSymbolValidation riskValidate)
        {
            bool result = false;
            object[] parameter = new object[6];

            parameter[0] = riskValidate.RiskCtrlCheck;
            parameter[1] = riskValidate.ValidateSymbolCheck;
            parameter[2] = riskValidate.RiskValue;
            parameter[3] = riskValidate.CompanyUserID;
            parameter[4] = riskValidate.LimitPriceCheck;
            parameter[5] = riskValidate.SetExecutedQtytoZero;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveTradTicketRiskValidationSett", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            #region Catch
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
            #endregion
            return result;
        }
        #endregion

        public static PreferencesUniversalSettingsCollection GetPreferences(int _userID)
        {
            PreferencesUniversalSettingsCollection tradingTicketPrefUnivSettings = new PreferencesUniversalSettingsCollection();
            object[] parameter = new object[1];
            parameter[0] = _userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingTicketPrefrencesUniversal", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingTicketPrefUnivSettings.Add(FillPrefrences(row, 0, _userID));
                    }
                }
            }
            #region Catch
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
            #endregion
            return tradingTicketPrefUnivSettings;
            //return null;
        }


        public static PreferencesUniversalSettings FillPrefrences(object[] row, int offSet, int userID)
        {
            int ASSET_ID = 0 + offSet;
            int UNDERLYING_ID = 1 + offSet;
            int TRADINGACCOUNT_ID = 2 + offSet;
            int FUND_ID = 3 + offSet;
            int STRATEGY_ID = 4 + offSet;
            int BROKER_ID = 5 + offSet;
            int ISDEFAULTCV = 6 + offSet;
            int QUANTITY = 7 + offSet;
            int DISPLAY_QUANTITY = 8 + offSet;
            int QUANTITY_INCREMENT = 9 + offSet;
            int PRICELIMIT_INCREMENT = 10 + offSet;
            int STOPPRICE_INCREMENT = 11 + offSet;
            int PEG_OFFSET = 12 + offSet;
            int DISCR_OFFSET = 13 + offSet;
            int COUNTERPARTY_ID = 14 + offSet;
            int VENUE_ID = 15 + offSet;
            int ORDERTYPE_ID = 16 + offSet;
            int EXECUTIONINSTRUCTION_ID = 17 + offSet;
            int HANDLINGINSTRUCTION_ID = 18 + offSet;
            int TIF = 19 + offSet;
            int CVTRADINGACCOUNT_ID = 20 + offSet;
            int CVFUND_ID = 21 + offSet;
            int CVSTRATEGY_ID = 22 + offSet;
            int BorrowerID = 23 + offSet;
            int CMTAID = 24 + offSet;
            int GIVEUPID = 25 + offSet;
            int OrderSide_ID = 26 + offSet;
            int SettlCurrency = 27 + offSet;
            int IsQuantityDefaultValueChecked = 28 + offSet;
            PreferencesUniversalSettings tradingTicketPrefUnivSettings = new PreferencesUniversalSettings();
            try
            {
                if (row[ASSET_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.AssetID = row[ASSET_ID].ToString();
                }
                if (row[UNDERLYING_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.UnderlyingID = row[UNDERLYING_ID].ToString();
                }
                if (row[TRADINGACCOUNT_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.TradingAccountID = row[TRADINGACCOUNT_ID].ToString();
                }
                if (row[FUND_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.AccountID = row[FUND_ID].ToString();
                }
                if (row[STRATEGY_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.StrategyID = row[STRATEGY_ID].ToString();
                }
                if (row[BROKER_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.BorrowerFirmID = row[BROKER_ID].ToString();
                }
                if (row[ISDEFAULTCV] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.IsDefaultCV = Convert.ToBoolean(row[ISDEFAULTCV]);
                }
                if (row[QUANTITY] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.Quantity = row[QUANTITY].ToString();
                }
                if (row[DISPLAY_QUANTITY] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.DisplayQuantity = row[DISPLAY_QUANTITY].ToString();
                }
                if (row[QUANTITY_INCREMENT] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.QuantityIncrement = row[QUANTITY_INCREMENT].ToString();
                }
                if (row[PRICELIMIT_INCREMENT] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.PriceLimitIncrement = row[PRICELIMIT_INCREMENT].ToString();
                }
                if (row[STOPPRICE_INCREMENT] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.StopPriceIncrement = row[STOPPRICE_INCREMENT].ToString();
                }
                if (row[PEG_OFFSET] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.PegOffset = row[PEG_OFFSET].ToString();
                }
                if (row[DISCR_OFFSET] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.DiscrOffset = row[DISCR_OFFSET].ToString();
                }
                if (row[COUNTERPARTY_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.CounterPartyID = row[COUNTERPARTY_ID].ToString();
                }
                if (row[VENUE_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.VenueID = row[VENUE_ID].ToString();
                }
                if (row[ORDERTYPE_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.OrderTypeID = row[ORDERTYPE_ID].ToString();
                }
                if (row[EXECUTIONINSTRUCTION_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.ExecutionInstructionID = row[EXECUTIONINSTRUCTION_ID].ToString();
                }
                if (row[HANDLINGINSTRUCTION_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.HandlingInstructionID = row[HANDLINGINSTRUCTION_ID].ToString();
                }

                if (row[TIF] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.TIF = row[TIF].ToString();
                }

                if (row[CVSTRATEGY_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.StrategyID = row[CVSTRATEGY_ID].ToString();
                }
                if (row[CVTRADINGACCOUNT_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.TradingAccountID = row[CVTRADINGACCOUNT_ID].ToString();
                }
                if (row[CVFUND_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.AccountID = row[CVFUND_ID].ToString();
                }
                if (row[BorrowerID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.BorrowerFirmID = row[BorrowerID].ToString();
                }
                if (row[CMTAID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.CMTAID = int.Parse(row[CMTAID].ToString());
                }
                if (row[GIVEUPID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.GiveUpID = int.Parse(row[GIVEUPID].ToString());
                }

                if (row[OrderSide_ID] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.OrderSide = row[OrderSide_ID].ToString();
                }
                tradingTicketPrefUnivSettings.CompanyUserID = userID;
                if (row[SettlCurrency] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.SettlCurrency = row[SettlCurrency].ToString();
                }
                if (row[IsQuantityDefaultValueChecked] != System.DBNull.Value)
                {
                    tradingTicketPrefUnivSettings.IsQuantityDefaultValueChecked = Convert.ToBoolean(row[IsQuantityDefaultValueChecked]);
                }
            }

            #region Catch
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
            #endregion
            return tradingTicketPrefUnivSettings;
        }

        /// <summary>
        /// Save the Universal Preferences Setting to the Database. If an entry already exits overwrite it and do not add.
        /// </summary>
        /// <param name="_preferencesUniversalSettingsData"></param>
        /// <returns></returns>
        private static bool SavePreference(PreferencesUniversalSettings _preferencesUniversalSettingsData)
        {
            bool result = false;
            object[] parameter = new object[30];

            parameter[0] = _preferencesUniversalSettingsData.AssetID;
            parameter[1] = _preferencesUniversalSettingsData.UnderlyingID;
            parameter[2] = _preferencesUniversalSettingsData.TradingAccountID;
            parameter[3] = _preferencesUniversalSettingsData.AccountID;
            parameter[4] = _preferencesUniversalSettingsData.StrategyID;
            parameter[5] = _preferencesUniversalSettingsData.BorrowerFirmID;

            parameter[6] = _preferencesUniversalSettingsData.IsDefaultCV;

            parameter[7] = _preferencesUniversalSettingsData.Quantity;
            parameter[8] = _preferencesUniversalSettingsData.DisplayQuantity;
            parameter[9] = _preferencesUniversalSettingsData.QuantityIncrement;

            parameter[10] = _preferencesUniversalSettingsData.PriceLimitIncrement;
            parameter[11] = _preferencesUniversalSettingsData.StopPriceIncrement;
            parameter[12] = _preferencesUniversalSettingsData.PegOffset;
            parameter[13] = _preferencesUniversalSettingsData.DiscrOffset;

            parameter[14] = _preferencesUniversalSettingsData.CounterPartyID;
            parameter[15] = _preferencesUniversalSettingsData.VenueID;
            parameter[16] = _preferencesUniversalSettingsData.OrderTypeID;
            parameter[17] = _preferencesUniversalSettingsData.ExecutionInstructionID;
            parameter[18] = _preferencesUniversalSettingsData.HandlingInstructionID;
            parameter[19] = _preferencesUniversalSettingsData.TIF;
            parameter[20] = _preferencesUniversalSettingsData.TradingAccountID;
            parameter[21] = _preferencesUniversalSettingsData.AccountID;
            parameter[22] = _preferencesUniversalSettingsData.StrategyID;
            parameter[23] = _preferencesUniversalSettingsData.BorrowerFirmID;
            parameter[24] = _preferencesUniversalSettingsData.CompanyUserID;
            parameter[25] = _preferencesUniversalSettingsData.CMTAID;
            parameter[26] = _preferencesUniversalSettingsData.GiveUpID;
            parameter[27] = _preferencesUniversalSettingsData.OrderSide;
            parameter[28] = _preferencesUniversalSettingsData.SettlCurrency;
            parameter[29] = _preferencesUniversalSettingsData.IsQuantityDefaultValueChecked;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveTradingTicketPrefrencesUniversal", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }


            #region Catch
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
            #endregion
            return result;

        }
        public static void SavePrefs(PreferencesUniversalSettings pref)
        {
            try
            {
                if (!pref.VenueID.Equals(int.MinValue) && !pref.CounterPartyID.Equals(int.MinValue) && !pref.CompanyUserID.Equals(int.MinValue))
                {
                    SavePreference(pref);
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

        public static bool DeleteAllTradingTicketPreferences(int _userID, int assetID, int underlyingID, int counterpartyID, int venueID)
        {
            bool result = false;

            object[] parameter = new object[5];
            parameter[0] = _userID;
            parameter[1] = assetID;
            parameter[2] = underlyingID;
            parameter[3] = counterpartyID;
            parameter[4] = venueID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllTradingTicketPreferences", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            #region Catch
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
            #endregion
            return result;
        }

        internal static CounterPartyWiseCommissionBasis GetCounterPartyWiseCommissionBasisPreferences(int userID)
        {
            CounterPartyWiseCommissionBasis commissionBasis = null;
            string startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
            string preferencesFilePath = preferencesDirectoryPath + @"\CounterPartyWiseCommissionBasis.xml";

            try
            {
                if (!Directory.Exists(preferencesDirectoryPath))
                {
                    Directory.CreateDirectory(preferencesDirectoryPath);
                }
                if (File.Exists(preferencesFilePath))
                {
                    using (FileStream fs = File.OpenRead(preferencesFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CounterPartyWiseCommissionBasis));
                        commissionBasis = (CounterPartyWiseCommissionBasis)serializer.Deserialize(fs);
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    commissionBasis = new CounterPartyWiseCommissionBasis();
                }
                return commissionBasis;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            #endregion
        }


        internal static TTGeneralPrefs GetTTGeneralPrefs(int userID)
        {
            TTGeneralPrefs generalPrefs = null;
            string startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
            string preferencesFilePath = preferencesDirectoryPath + @"\TTGeneralPrefs.xml";

            try
            {
                if (!Directory.Exists(preferencesDirectoryPath))
                {
                    Directory.CreateDirectory(preferencesDirectoryPath);
                }
                if (File.Exists(preferencesFilePath))
                {
                    using (FileStream fs = File.OpenRead(preferencesFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(TTGeneralPrefs));
                        generalPrefs = (TTGeneralPrefs)serializer.Deserialize(fs);
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    generalPrefs = new TTGeneralPrefs();
                }
                return generalPrefs;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            #endregion
        }

        public static void SaveGeneralPrefs(TTGeneralPrefs prefs, int userID)
        {
            try
            {
                string startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
                string preferencesFilePath = preferencesDirectoryPath + @"\TTGeneralPrefs.xml";

                using (XmlTextWriter writer = new XmlTextWriter(preferencesFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(TTGeneralPrefs));
                    serializer.Serialize(writer, prefs);
                    writer.Flush();
                    // writer.Close();
                    //TradingTktPrefs.TTGeneralPrefs = prefs;
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Gets the quick tt prefs.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        internal static QuickTTPrefs GetQuickTTPrefs(int userID)
        {
            QuickTTPrefs quickTTPrefs = new QuickTTPrefs();
            try
            {
                string startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
                string preferencesFilePath = preferencesDirectoryPath + @"\QuickTTPrefs.dat";

                if (File.Exists(preferencesFilePath))
                {
                    FileStream fileStream = new FileStream(preferencesFilePath, FileMode.Open, FileAccess.Read);

                    BinaryFormatter serializer = new BinaryFormatter();
                    object obj = null;
                    try
                    {
                        obj = serializer.Deserialize(fileStream);
                    }
                    finally
                    {
                        fileStream.Close();
                    }
                    string str = obj as string;
                    quickTTPrefs.DeSerialize(str);
                }
            }
            #region Catch
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
            return quickTTPrefs;
            #endregion
        }

        /// <summary>
        /// Saves the quick tt prefs.
        /// </summary>
        /// <param name="prefs">The prefs.</param>
        /// <param name="userID">The user identifier.</param>
        internal static void SaveQuickTTPrefs(QuickTTPrefs prefs, int userID)
        {
            try
            {
                string startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
                string preferencesFilePath = preferencesDirectoryPath + @"\QuickTTPrefs.dat";

                if (File.Exists(preferencesFilePath))
                {
                    File.Delete(preferencesFilePath);
                }
                FileStream fs = new FileStream(preferencesFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                try
                {
                    binaryFormatter.Serialize(fs, prefs.Serialize());
                }
                finally
                {
                    fs.Close();
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                //if(fs!=null)
                //    fs.Close();
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Gets the QTT field preference.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        internal static QTTFieldPreference[] GetQTTFieldPreference(int userID)
        {
            try
            {
                QTTFieldPreference[] qTTFieldPreference = null;
                string startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
                string preferencesFilePath = preferencesDirectoryPath + @"\QTTFieldPreference.xml";

                if (!Directory.Exists(preferencesDirectoryPath))
                {
                    Directory.CreateDirectory(preferencesDirectoryPath);
                }
                if (File.Exists(preferencesFilePath))
                {
                    using (FileStream fs = File.OpenRead(preferencesFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(QTTFieldPreference[]));
                        qTTFieldPreference = (QTTFieldPreference[])serializer.Deserialize(fs);
                    }
                }
                else //if No Preferences File Exist Take Default Preferences
                {
                    qTTFieldPreference = new QTTFieldPreference[10];
                }
                return qTTFieldPreference;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
            #endregion
        }

        internal static void SaveQTTFieldPreference(QTTFieldPreference[] prefs, int userID)
        {
            try
            {
                string startPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string preferencesDirectoryPath = startPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
                string preferencesFilePath = preferencesDirectoryPath + @"\QTTFieldPreference.xml";

                using (XmlTextWriter writer = new XmlTextWriter(preferencesFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(QTTFieldPreference[]));
                    serializer.Serialize(writer, prefs);
                    writer.Flush();
                }
            }
            #region catch
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
            #endregion
        }

        internal static TradingTicketUIPrefs GetTTCompanyUIPreferences(int companyID)
        {
            string spName = "P_GetTTCompanyPreferences";
            return GetTTPeferences(companyID, spName, true);
        }

        internal static TradingTicketUIPrefs GetTTUserUIPreferences(int userID)
        {
            string spName = "P_GetTTUserPreferences";
            return GetTTPeferences(userID, spName, false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static TradingTicketUIPrefs GetTTPeferences(int id, string spName, bool isCompanyLevelPref)
        {
            TradingTicketUIPrefs preferences = new TradingTicketUIPrefs();
            try
            {
                DataSet ds = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;
                queryData.DictionaryDatabaseParameter.Add("@id", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@id",
                    ParameterType = DbType.Int32,
                    ParameterValue = id
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables.Count > 1)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DefAssetSide defAssetSide = new DefAssetSide();
                        defAssetSide.Asset = (int?)ReturnValueAfterConversion(row, 0, typeof(Int32));
                        defAssetSide.OrderSide = (int?)ReturnValueAfterConversion(row, 1, typeof(Int32));
                        preferences.DefAssetSides.Add(defAssetSide);
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                        FillPreferenceObject(ds.Tables[1].Rows[0], 0, ref preferences, isCompanyLevelPref);
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
                return null;
            }
            return preferences;
        }

        private static object ReturnValueAfterConversion(DataRow row, int rowIndex, Type type)
        {
            object value = null;
            try
            {
                if (!row.IsNull(rowIndex))
                {
                    if (type == typeof(Int32))
                    {
                        value = Convert.ToInt32(row[rowIndex]);
                    }
                    else if (type == typeof(Double))
                    {
                        value = Convert.ToDouble(row[rowIndex]);
                    }
                    else if ((type == typeof(Boolean)))
                    {
                        value = Convert.ToBoolean(row[rowIndex]);

                    }
                    else if ((type == typeof(String)))
                    {
                        value = row[rowIndex].ToString();
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
            return value;
        }

        private static void FillPreferenceObject(DataRow row, int offset, ref TradingTicketUIPrefs preferences, bool isCompanyLevelPref)
        {
            try
            {
                int CounterPartyID = 0 + offset;
                int VenueID = 1 + offset;
                int OrderTypeID = 2 + offset;
                int TimeInForceID = 3 + offset;
                int ExecutionInstructionID = 4 + offset;
                int HandlingInstructionID = 5 + offset;
                int TradingAccountID = 6 + offset;
                int StrategyID = 7 + offset;
                int AccountID = 8 + offset;
                int SettlementCurrencyID = 9 + offset;
                int Quantity = 10 + offset;
                int IncrementOnQty = 11 + offset;
                int IncrementOnStop = 12 + offset;
                int IncrementOnLimit = 13 + offset;
                int QuantityType = 14 + offset;
                int isShowTargetQTY = 15 + offset;
                int TTControlsMappings = 16 + offset;

                preferences.Broker = (int?)ReturnValueAfterConversion(row, CounterPartyID, typeof(Int32));
                preferences.Venue = (int?)ReturnValueAfterConversion(row, VenueID, typeof(Int32));
                preferences.OrderType = (int?)ReturnValueAfterConversion(row, OrderTypeID, typeof(Int32));
                preferences.TimeInForce = (int?)ReturnValueAfterConversion(row, TimeInForceID, typeof(Int32));
                preferences.ExecutionInstruction = (int?)ReturnValueAfterConversion(row, ExecutionInstructionID, typeof(Int32));
                preferences.HandlingInstruction = (int?)ReturnValueAfterConversion(row, HandlingInstructionID, typeof(Int32));
                preferences.TradingAccount = (int?)ReturnValueAfterConversion(row, TradingAccountID, typeof(Int32));
                preferences.Strategy = (int?)ReturnValueAfterConversion(row, StrategyID, typeof(Int32));
                preferences.Account = (int?)ReturnValueAfterConversion(row, AccountID, typeof(Int32));
                preferences.IsSettlementCurrencyBase = (bool?)ReturnValueAfterConversion(row, SettlementCurrencyID, typeof(Boolean));
                preferences.Quantity = (double?)ReturnValueAfterConversion(row, Quantity, typeof(Double));
                preferences.IncrementOnQty = (double?)ReturnValueAfterConversion(row, IncrementOnQty, typeof(Double));
                preferences.IncrementOnStop = (double?)ReturnValueAfterConversion(row, IncrementOnStop, typeof(Double));
                preferences.IncrementOnLimit = (double?)ReturnValueAfterConversion(row, IncrementOnLimit, typeof(Double));
                preferences.QuantityType = (QuantityTypeOnTT)ReturnValueAfterConversion(row, QuantityType, typeof(Int32));

                if (isCompanyLevelPref)
                {
                    preferences.IsShowTargetQTY = (bool?)ReturnValueAfterConversion(row, isShowTargetQTY, typeof(Boolean));
                    preferences.DefTTControlsMapping = (ReturnValueAfterConversion(row, TTControlsMappings, typeof(String)).ToString());
                }
                else
                {
                    int isUseRoundLot = 15 + offset;
                    preferences.IsUseRoundLots = (bool)ReturnValueAfterConversion(row, isUseRoundLot, typeof(Boolean));
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
        /// to get _company trading rules prefs from the database
        /// </summary>
        ///<param name="companyID"></param>
        /// <returns></returns>
        internal static TradingTicketRulesPrefs GetTradingRulesPreferences(int companyID)
        {
            try
            {
                string spName = "P_GetTradingRulesPreferences";
                return GetTradingRulesPreferences(companyID, spName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal static TradingTicketRulesPrefs GetTradingRulesPreferences(int companyID, string spName)
        {
            TradingTicketRulesPrefs tradingRulesPref = new TradingTicketRulesPrefs();
            try
            {
                DataSet ds = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;
                queryData.DictionaryDatabaseParameter.Add("@id", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@id",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    FillTradingRulesPrefObject(ds.Tables[0].Rows[0], 0, ref tradingRulesPref);
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
                return null;
            }
            return tradingRulesPref;
        }

        private static void FillTradingRulesPrefObject(DataRow row, int offset, ref TradingTicketRulesPrefs tradingRulesPref)
        {
            try
            {
                int IsOversellTradingRule = 0 + offset;
                int IsOverbuyTradingRule = 1 + offset;
                int IsUnallocatedTradeAlert = 2 + offset;
                int IsFatFingerTradingRule = 3 + offset;
                int IsDuplicateTradeAlert = 4 + offset;
                int IsPendingNewTradeAlert = 5 + offset;
                int DefineFatFingerValue = 6 + offset;
                int DuplicateTradeAlertTime = 7 + offset;
                int PendingNewOrderAlertTime = 8 + offset;
                int FatFingerAccountOrMasterFund = 9 + offset;
                int IsAbsoluteAmountOrDefinePercent = 10 + offset;
                int IsInMarketIncluded = 11 + offset;
                int IsSharesOutstandingRule = 12 + offset;
                int SharesOutstandingAccountOrMF = 13 + offset;
                int SharesOutstandingPercent = 14 + offset;

                tradingRulesPref.IsOversellTradingRule = (bool?)ReturnValueAfterConversion(row, IsOversellTradingRule, typeof(Boolean));
                tradingRulesPref.IsOverbuyTradingRule = (bool?)ReturnValueAfterConversion(row, IsOverbuyTradingRule, typeof(Boolean));
                tradingRulesPref.IsUnallocatedTradeAlert = (bool?)ReturnValueAfterConversion(row, IsUnallocatedTradeAlert, typeof(Boolean));
                tradingRulesPref.IsFatFingerTradingRule = (bool?)ReturnValueAfterConversion(row, IsFatFingerTradingRule, typeof(Boolean));
                tradingRulesPref.IsDuplicateTradeAlert = (bool?)ReturnValueAfterConversion(row, IsDuplicateTradeAlert, typeof(Boolean));
                tradingRulesPref.IsPendingNewTradeAlert = (bool?)ReturnValueAfterConversion(row, IsPendingNewTradeAlert, typeof(Boolean));
                tradingRulesPref.DefineFatFingerValue = (double?)ReturnValueAfterConversion(row, DefineFatFingerValue, typeof(double));
                tradingRulesPref.DuplicateTradeAlertTime = (int?)ReturnValueAfterConversion(row, DuplicateTradeAlertTime, typeof(Int32));
                tradingRulesPref.PendingNewOrderAlertTime = (int?)ReturnValueAfterConversion(row, PendingNewOrderAlertTime, typeof(Int32));
                tradingRulesPref.FatFingerAccountOrMasterFund = (int?)ReturnValueAfterConversion(row, FatFingerAccountOrMasterFund, typeof(Int32));
                tradingRulesPref.IsAbsoluteAmountOrDefinePercent = (int?)ReturnValueAfterConversion(row, IsAbsoluteAmountOrDefinePercent, typeof(Int32));
                tradingRulesPref.IsInMarketIncluded = (bool?)ReturnValueAfterConversion(row, IsInMarketIncluded, typeof(Boolean));
                tradingRulesPref.IsSharesOutstandingRule = (bool?)ReturnValueAfterConversion(row, IsSharesOutstandingRule, typeof(Boolean));
                tradingRulesPref.SharesOutstandingAccOrMF = (int?)ReturnValueAfterConversion(row, SharesOutstandingAccountOrMF, typeof(Int32));
                tradingRulesPref.SharesOutstandingValue = (double?)ReturnValueAfterConversion(row, SharesOutstandingPercent, typeof(double));
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
        /// to get the value of TT checkbox for DollarAmountPermission from the database
        /// </summary>
        /// <returns></returns>
        public static bool GetDollarAmountPermission()
        {
            try
            {
                DataSet result = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDollarAmountPermission";

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (result.Tables[0].Rows.Count != 0)
                    return Boolean.Parse(result.Tables[0].Rows[0]["TT"].ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// to get the value of TT checkbox for DollarAmountPTTPermission from the database
        /// </summary>
        /// <returns></returns>
        public static bool GetDollarAmountPTTPermission()
        {
            try
            {
                DataSet result = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetDollarAmountPermission";

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (result.Tables[0].Rows.Count != 0)
                    return Boolean.Parse(result.Tables[0].Rows[0]["PTT"].ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
        /// <summary>
        /// to get the value of company user module permission for pst from the database
        /// </summary>
        /// <returns></returns>
        public static bool GetPSTCompanyModuleForUserPermission(int companyUserID)
        {
            try
            {
                DataSet result = null;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanyModulesForUser";
                queryData.DictionaryDatabaseParameter.Add("@companyUserID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@companyUserID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyUserID
                });

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    // Iterate through the rows to check for the specific ModuleName
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        if (row["ModuleName"] != null && row["ModuleName"].ToString() == "% Trading Tool")
                        {
                            return true; // Module found
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
            return false;
        }
        /// <summary>
        /// Gets the Security List based on restricted or allowed type
        /// </summary>
        internal static List<string> GetRestrictedAllowedSecuritiesList(int companyID, string securitiesListType)
        {
            string securitiesListFetched = string.Empty;
            try
            {
                int AllowedOrRestricted;
                if (securitiesListType == "Restricted")
                    AllowedOrRestricted = 0;
                else
                    AllowedOrRestricted = 1;
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = AllowedOrRestricted;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSecuritiesList", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int Symbol = 0;
                        if (row != null)
                        {
                            if (row[Symbol] != System.DBNull.Value)
                            {
                                securitiesListFetched = Convert.ToString(row[Symbol]);
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

            return securitiesListFetched.Split(',').ToList();
        }

        /// <summary>
        /// Gets the IsTickerSymbology based on restricted or allowed type
        /// </summary>
        internal static bool GetRestrictedAllowedTickerSymbology(int companyID, string securitiesListType)
        {
            bool _isTickerSymbology = true;
            try
            {
                int AllowedOrRestricted;
                if (securitiesListType == "Restricted")
                    AllowedOrRestricted = 0;
                else
                    AllowedOrRestricted = 1;
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = AllowedOrRestricted;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSecuritiesListTickerSymbology", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int IsTickerSymbology = 0;
                        if (row != null)
                        {
                            if (row[IsTickerSymbology] != System.DBNull.Value)
                            {
                                _isTickerSymbology = Convert.ToBoolean(row[IsTickerSymbology]);
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
            return _isTickerSymbology;
        }
    }
}
