using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Data;
namespace Prana.TradingTicket
{
    /// <summary>
    /// TicketManager is a singelton manager class.
    /// </summary>
    public class TradingTicketManager
    {
        private static TradingTicketManager _ticketManager = new TradingTicketManager();

        private TradingTicketManager()
        {
        }

        public static TradingTicketManager GetInstance()
        {
            return _ticketManager;
        }




        #region  Trading Ticket Settings
        /// <summary>
        /// Saving Trading TIcket Settings in the database.
        /// </summary>
        /// <param name="_tradingTicketSettings"></param>
        /// <returns>true if saved else false</returns>
        public bool SaveTradingTicketSettings(TradingTicketSettings _tradingTicketSettings)
        {

            bool result = false;
            object[] parameter = new object[29];

            parameter[0] = _tradingTicketSettings.AssetID;
            parameter[1] = _tradingTicketSettings.AUECID;
            parameter[2] = _tradingTicketSettings.ButtonColor;
            parameter[3] = _tradingTicketSettings.ButtonPosition;
            parameter[4] = _tradingTicketSettings.CompanyUserID;
            parameter[5] = _tradingTicketSettings.CounterpartyID;
            parameter[6] = _tradingTicketSettings.Description;
            parameter[7] = _tradingTicketSettings.DiscreationOffset.ToString();
            parameter[8] = _tradingTicketSettings.DisplayQuantity;
            parameter[9] = _tradingTicketSettings.ExecutionInstructionID;
            parameter[10] = _tradingTicketSettings.AccountID;
            parameter[11] = _tradingTicketSettings.HandlingInstructionID;
            parameter[12] = _tradingTicketSettings.IsHotButton.ToString();
            parameter[13] = _tradingTicketSettings.LimitOffset.ToString();
            parameter[14] = _tradingTicketSettings.LimitType;
            parameter[15] = _tradingTicketSettings.Name;
            parameter[16] = _tradingTicketSettings.OrderTypeID;
            parameter[17] = _tradingTicketSettings.Peg.ToString();
            parameter[18] = _tradingTicketSettings.PNP;
            parameter[19] = _tradingTicketSettings.Quantity;
            parameter[20] = _tradingTicketSettings.Random;
            parameter[21] = _tradingTicketSettings.SideID;
            parameter[22] = _tradingTicketSettings.StrategyID;
            parameter[23] = _tradingTicketSettings.TicketSettingsID;
            parameter[24] = _tradingTicketSettings.TIF.ToString();
            parameter[25] = _tradingTicketSettings.TradingAccountID;
            parameter[26] = _tradingTicketSettings.UnderLyingID;
            parameter[27] = _tradingTicketSettings.VenueID;
            parameter[28] = int.MinValue;

            try
            {

                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveTradingTicketSettings", parameter) > 0)
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



        public TradingTicketSettingsCollection GetTradingTicketSettings(int _userID)
        {
            TradingTicketSettingsCollection tradingTicketSettings = new TradingTicketSettingsCollection();
            object[] parameter = new object[1];
            parameter[0] = _userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingTickets", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingTicketSettings.Add(FillTradingTicketPrefrences(row, 0));
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
            return tradingTicketSettings;
        }

        public TradingTicketSettings GetTradingTicketIDSettings(string ID)
        {
            TradingTicketSettings tradingTicketSetting = new TradingTicketSettings();
            object[] parameter = new object[1];
            parameter[0] = ID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradingTicketByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingTicketSetting = FillTradingTicketPrefrences(row, 0);
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
            return tradingTicketSetting;



        }
        public static TradingTicketSettings FillTradingTicketPrefrences(object[] row, int offSet)
        {
            int TICKETSETTINGS_ID = 0 + offSet;
            int ASSET_ID = 1 + offSet;
            int AUEC_ID = 2 + offSet;
            int BUTTON_COLOR = 3 + offSet;
            int BUTTON_POSITION = 4 + offSet;
            int NAME = 5 + offSet;
            int DESCRIPTION = 6 + offSet;
            int ISHOT_BUTTON = 7 + offSet;
            int SIDE_ID = 8 + offSet;
            int FUND_ID = 9 + offSet;
            int STRATEGY_ID = 10 + offSet;
            //int CLEARING_FIRM = 11 + offSet;
            int QUANTITY = 12 + offSet;
            int ORDERTYPE_ID = 13 + offSet;
            int TIF = 14 + offSet;
            int EXECUTIONINSTRUCTION_ID = 15 + offSet;
            int HANDLINGINSTRUCTION_ID = 16 + offSet;
            int TRADINGACCOUNT_ID = 17 + offSet;
            int PEG_OFFSET = 18 + offSet;
            int DISCR_OFFSET = 19 + offSet;
            int DISPLAY_QUANTITY = 20 + offSet;
            int RANDOM_RESERVER = 21 + offSet;
            int PNP = 22 + offSet;
            int COMPANY_USER_ID = 23 + offSet;
            int LIMIT_TYPE = 24 + offSet;
            int LIMIT_OFFSET = 25 + offSet;
            int UNDERLYING_ID = 26 + offSet;
            int COUNTERPARTY_ID = 27 + offSet;
            int VENUE_ID = 28 + offSet;


            //int OPENCLOSE = 37 + offSet;
            TradingTicketSettings tradingTicketSettings = new TradingTicketSettings();
            try
            {

                if (row[ISHOT_BUTTON] != System.DBNull.Value)
                {
                    tradingTicketSettings.IsHotButton = Convert.ToBoolean(row[ISHOT_BUTTON].ToString());
                }
                if (row[TICKETSETTINGS_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.TicketSettingsID = row[TICKETSETTINGS_ID].ToString();
                }
                //if(row[SETTING_TYPE] != System.DBNull.Value)
                //{
                //    tradingTicketSettings.SettingType =  int.Parse(row[SETTING_TYPE].ToString());
                //}
                if (row[NAME] != System.DBNull.Value)
                {
                    tradingTicketSettings.Name = row[NAME].ToString();
                }
                if (row[DESCRIPTION] != System.DBNull.Value)
                {
                    tradingTicketSettings.Description = row[DESCRIPTION].ToString();
                }
                //if(row[DEFAULT_TICKET] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.DefaultTicketID = int.MinValue;
                //}
                //else
                //{
                //    tradingTicketSettings.DefaultTicketID =  int.Parse(row[DEFAULT_TICKET].ToString());
                //}
                //if(row[ACTION_BUTTON] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.ActionButton = int.MinValue;
                //}
                //else
                //{
                //    tradingTicketSettings.ActionButton =  int.Parse(row[ACTION_BUTTON].ToString());
                //}
                if (row[BUTTON_COLOR] == System.DBNull.Value)
                {
                    tradingTicketSettings.ButtonColor = string.Empty;
                }
                else
                {
                    tradingTicketSettings.ButtonColor = row[BUTTON_COLOR].ToString();
                }
                //if(row[DISPLAY_BUTTON] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.DisplayName = string.Empty;
                //}
                //else
                //{
                //    tradingTicketSettings.DisplayName =  row[DISPLAY_BUTTON].ToString();
                //}
                if (row[SIDE_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.SideID = row[SIDE_ID].ToString();
                }

                if (row[FUND_ID] == System.DBNull.Value)
                {
                    tradingTicketSettings.AccountID = int.MinValue;
                }
                else
                {
                    tradingTicketSettings.AccountID = int.Parse(row[FUND_ID].ToString());
                }
                if (row[STRATEGY_ID] == System.DBNull.Value)
                {
                    tradingTicketSettings.StrategyID = int.MinValue;
                }
                else
                {
                    tradingTicketSettings.StrategyID = int.Parse(row[STRATEGY_ID].ToString());
                }



                //if(row[CLIENT_COMPANY] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.ClientCompanyID = int.MinValue;
                //}
                //else

                //{
                //    tradingTicketSettings.ClientCompanyID =  int.Parse(row[CLIENT_COMPANY].ToString());
                //}
                //if(row[CLIENT_TRADER] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.ClientTraderID = int.MinValue;
                //}
                //else

                //{
                //    tradingTicketSettings.ClientTraderID =  int.Parse(row[CLIENT_TRADER].ToString());
                //}
                //if(row[CLIENT_FUND] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.ClientAccountID = int.MinValue;
                //}
                //else

                //{
                //    tradingTicketSettings.ClientAccountID =  int.Parse(row[CLIENT_FUND].ToString());
                //}
                //if(row[CLEARINGFIRM_ID] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.ClearingFirmID = int.MinValue;
                //}
                //else

                //{
                //    tradingTicketSettings.ClearingFirmID =  int.Parse(row[CLEARINGFIRM_ID].ToString());
                //}
                //if(row[AGENCY_PRINCIPAL] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.Principal = int.MinValue;
                //}
                //else

                //{
                //    tradingTicketSettings.Principal =  int.Parse(row[AGENCY_PRINCIPAL].ToString());
                //}

                //if(row[SHORT_EXEMPT] == System.DBNull.Value)
                //{
                //    tradingTicketSettings.ShortExempt = int.MinValue;
                //}
                //else
                //{
                //    tradingTicketSettings.ShortExempt =  int.Parse(row[SHORT_EXEMPT].ToString());
                //}

                if (row[ASSET_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.AssetID = int.Parse(row[ASSET_ID].ToString());
                }
                if (row[UNDERLYING_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.UnderLyingID = int.Parse(row[UNDERLYING_ID].ToString());
                }
                if (row[COUNTERPARTY_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.CounterpartyID = int.Parse(row[COUNTERPARTY_ID].ToString());
                }
                if (row[VENUE_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.VenueID = int.Parse(row[VENUE_ID].ToString());
                }

                if (row[ORDERTYPE_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.OrderTypeID = row[ORDERTYPE_ID].ToString();
                }

                if (row[EXECUTIONINSTRUCTION_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.ExecutionInstructionID = row[EXECUTIONINSTRUCTION_ID].ToString();
                }
                if (row[HANDLINGINSTRUCTION_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.HandlingInstructionID = row[HANDLINGINSTRUCTION_ID].ToString();
                }

                if (row[TIF] != System.DBNull.Value)
                {
                    tradingTicketSettings.TIF = int.Parse(row[TIF].ToString());
                }
                if (row[TRADINGACCOUNT_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.TradingAccountID = int.Parse(row[TRADINGACCOUNT_ID].ToString());
                }
                if (row[QUANTITY] != System.DBNull.Value)
                {
                    tradingTicketSettings.Quantity = int.Parse(row[QUANTITY].ToString());
                }





                if (row[DISPLAY_QUANTITY] != System.DBNull.Value)
                {
                    tradingTicketSettings.DisplayQuantity = int.Parse(row[DISPLAY_QUANTITY].ToString());
                }
                if (row[PEG_OFFSET] != System.DBNull.Value)
                {
                    tradingTicketSettings.Peg = double.Parse(row[PEG_OFFSET].ToString());
                }
                if (row[DISCR_OFFSET] != System.DBNull.Value)
                {
                    tradingTicketSettings.DiscreationOffset = double.Parse(row[DISCR_OFFSET].ToString());
                }
                if (row[RANDOM_RESERVER] != System.DBNull.Value)
                {
                    tradingTicketSettings.Random = int.Parse(row[RANDOM_RESERVER].ToString());
                }

                if (row[PNP] != System.DBNull.Value)
                {
                    tradingTicketSettings.PNP = int.Parse(row[PNP].ToString());
                }
                else
                {
                    tradingTicketSettings.PNP = int.MinValue;
                }
                if (row[COMPANY_USER_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.CompanyUserID = int.Parse(row[COMPANY_USER_ID].ToString());
                }
                if (row[BUTTON_POSITION] != System.DBNull.Value)
                {
                    tradingTicketSettings.ButtonPosition = row[BUTTON_POSITION].ToString();
                }

                if (row[LIMIT_TYPE] != System.DBNull.Value)
                {
                    tradingTicketSettings.LimitType = int.Parse(row[LIMIT_TYPE].ToString());
                }
                else
                {
                    tradingTicketSettings.LimitType = int.MinValue;
                }
                if (row[LIMIT_OFFSET] != System.DBNull.Value)
                {
                    tradingTicketSettings.LimitOffset = double.Parse(row[LIMIT_OFFSET].ToString());
                }
                else
                {
                    tradingTicketSettings.LimitOffset = double.MinValue;
                }
                //if (row[DISPLAY] != System.DBNull.Value)
                //{
                //    tradingTicketSettings.Display = Convert.ToBoolean(row[DISPLAY].ToString());
                //}
                //if (row[OPENCLOSE] != System.DBNull.Value)
                //{
                //    tradingTicketSettings.OpenClose = row[OPENCLOSE].ToString();
                //}
                if (row[AUEC_ID] != System.DBNull.Value)
                {
                    tradingTicketSettings.AUECID = int.Parse(row[AUEC_ID].ToString());
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
            return tradingTicketSettings;
        }
        public bool DeleteTradingTicketByID(int ID, int type)
        {
            bool result = false;
            object[] parameter = new object[2];
            parameter[0] = ID;
            parameter[1] = type;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteTicketSettingByID", parameter) > 0)
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
        public bool DeleteAllTradingTickets(int _userID)
        {
            bool result = false;

            object[] parameter = new object[1];
            parameter[0] = _userID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteAllTicketSetting", parameter) > 0)
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

        #region Confirmation PopUP 

        public bool SaveConfirmationPopUpPreferences(ConfirmationPopUp confirmationPopUp)
        {
            bool result = false;
            object[] parameter = new object[5];

            parameter[0] = confirmationPopUp.ISNewOrder;
            parameter[1] = confirmationPopUp.ISCXL;
            parameter[2] = confirmationPopUp.ISCXLReplace;
            parameter[3] = confirmationPopUp.IsManualOrder;
            parameter[4] = confirmationPopUp.CompanyUserID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveTradingTicketPrefrencesPopUps", parameter) > 0)
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



        //required as cmta and giveup can be called using CPVenueID
        public static int GetCompanyCPVenueIDfromCPIDVenueID(int counterpartyID, int venueID, int companyID)
        {
            object[] parameter = new object[3];
            parameter[0] = counterpartyID;
            parameter[1] = venueID;
            parameter[2] = companyID;

            int CPVenueID = int.MinValue;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCPVenueIDbyCPIDVenueID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        CPVenueID = int.Parse(row[0].ToString());
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
            return CPVenueID;


        }

        // get CMTA values for Options        
        public static CompanyCVCMTAIdentifiers GetCompanyCVCMTAIdentifiers(int CPVenueID)
        {
            object[] parameter = new object[1];
            parameter[0] = CPVenueID;
            CompanyCVCMTAIdentifiers companyCVCMTAIdentifiers = new CompanyCVCMTAIdentifiers();
            CompanyCVCMTAIdentifier companyCVCMTAIdentifier = new CompanyCVCMTAIdentifier();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCVCMTAIdentifier", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCVCMTAIdentifier = FillCompanyCounterPartyVenueCMTAIdentifier(row, 0);
                        companyCVCMTAIdentifiers.Add(companyCVCMTAIdentifier);
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
            return companyCVCMTAIdentifiers;
        }
        public static CompanyCVCMTAIdentifier FillCompanyCounterPartyVenueCMTAIdentifier(object[] row, int offSet)
        {
            int companyCounterPartyVenueID = 0 + offSet;
            int companyCounterPartyVenueIdentifierID = 1 + offSet;
            int cmtaIdentifier = 2 + offSet;

            CompanyCVCMTAIdentifier companyCounterPartyVenueIdentifier = new CompanyCVCMTAIdentifier();
            try
            {
                if (row[companyCounterPartyVenueID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueIdentifier.CompanyCounterPartyVenueId = int.Parse(row[companyCounterPartyVenueID].ToString());
                }
                if (row[companyCounterPartyVenueIdentifierID] != System.DBNull.Value)
                {
                    companyCounterPartyVenueIdentifier.CompanyCVCMTAIdentifierID = int.Parse(row[companyCounterPartyVenueIdentifierID].ToString());
                }
                if (row[cmtaIdentifier] != System.DBNull.Value)
                {
                    companyCounterPartyVenueIdentifier.CMTAIdentifier = row[cmtaIdentifier].ToString();
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
            return companyCounterPartyVenueIdentifier;
        }

        // get GiveUp values for Options

        public static CompanyCVGiveUpIdentifiers GetCompanyCVGiveUpIdentifiers(int CPVenueID)
        {
            object[] parameter = new object[1];
            parameter[0] = CPVenueID;
            CompanyCVGiveUpIdentifiers companyCVGiveUpIdentifiers = new CompanyCVGiveUpIdentifiers();
            CompanyCVGiveUpIdentifier companyCVGiveUpIdentifier = new CompanyCVGiveUpIdentifier();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyCVGiveUpIdentifier", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyCVGiveUpIdentifier = FillCompanyCPVenueGiveUpIdentifier(row, 0);
                        companyCVGiveUpIdentifiers.Add(companyCVGiveUpIdentifier);
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
            return companyCVGiveUpIdentifiers;
        }

        public static CompanyCVGiveUpIdentifier FillCompanyCPVenueGiveUpIdentifier(object[] row, int offSet)
        {

            int companyCPVenueID = 0 + offSet;
            int companyCPVenueGiveUpIdentifierID = 1 + offSet;
            int giveUpIdentifier = 2 + offSet;

            CompanyCVGiveUpIdentifier companyCPVenueGiveUpIdentifier = new CompanyCVGiveUpIdentifier();
            try
            {
                if (row[companyCPVenueID] != System.DBNull.Value)
                {
                    companyCPVenueGiveUpIdentifier.CompanyCounterPartyVenueId = int.Parse(row[companyCPVenueID].ToString());
                }
                if (row[companyCPVenueGiveUpIdentifierID] != System.DBNull.Value)
                {
                    companyCPVenueGiveUpIdentifier.CompanyCVGiveUpIdentifierID = int.Parse(row[companyCPVenueGiveUpIdentifierID].ToString());
                }
                if (row[giveUpIdentifier] != System.DBNull.Value)
                {
                    companyCPVenueGiveUpIdentifier.GiveUpIdentifier = row[giveUpIdentifier].ToString();
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
            return companyCPVenueGiveUpIdentifier;
        }

    }
}
