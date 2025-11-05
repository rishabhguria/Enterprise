using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;
using System.Windows.Forms;
namespace Prana.TradeManager
{
    /// <summary>
    /// Shift this class to a new DLL if required. Also Remove the reference of Business logic from this Project.
    /// </summary>
    public class TradeManagerCore : ITradeManager
    {
        private static int _userID;
        private static Prana.BusinessObjects.PriceSymbolValidation priceSymbolSettings = new Prana.BusinessObjects.PriceSymbolValidation();
        private static ICommunicationManager _communicationManager = null;

        private static TradeManagerCore _tradeManager = null;
        public static TradeManagerCore GetInstance()
        {
            if (_tradeManager == null)
            {
                _tradeManager = new TradeManagerCore();
            }
            return _tradeManager;
        }

        public static int UserID
        {
            get
            {
                //if (_userID != null)
                //{
                return _userID;
                //}
            }
            set
            {
                _userID = value;
                GetTradePrefs(_userID);
            }
        }
        public static ICommunicationManager SetCommunicationManager
        {
            set { _communicationManager = value; }
        }
        public static bool CheckServerStatus()
        {
            if (_communicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                return true;
            }
            else if (_communicationManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.DISCONNECTED)
            {
                MessageBox.Show("Please Connect to Server.");
            }
            else
            {
                MessageBox.Show("No Server is Running at specified Port !");
            }
            return false;
        }

        #region GetPriceSymbol Validation Settings
        public static void GetTradePrefs(int userID)
        {
            priceSymbolSettings = TicketManager.GetPriceSymbolSettings(userID);
        }

        public static void UpdateSymbolSettings(Prana.BusinessObjects.PriceSymbolValidation newPriceSymbolSettings)
        {
            try
            {

                priceSymbolSettings.CompanyUserID = newPriceSymbolSettings.CompanyUserID;
                priceSymbolSettings.RiskCtrlCheck = newPriceSymbolSettings.RiskCtrlCheck;
                priceSymbolSettings.RiskValue = newPriceSymbolSettings.RiskValue;
                priceSymbolSettings.ValidateSymbolCheck = newPriceSymbolSettings.ValidateSymbolCheck;
                priceSymbolSettings.LimitPriceCheck = newPriceSymbolSettings.LimitPriceCheck;
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

        #endregion

        #region ITradeManager Members

        public static bool ValidateAUECandCV(Prana.BusinessObjects.Order order)
        {
            if (CachedDataManager.GetInstance.CheckTradePermissionByCVandAUECID(order.AUECID, order.CounterPartyID, order.VenueID))
            {
                if (TradeManagerExtension.GetInstance().GetCounterPartyConnectionSatus(order.CounterPartyID) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    return true;
                }
                else
                {
                    throw new Exception("CounterParty not Connected.");
                }
            }
            else
            {
                throw new Exception("Check if you have permission for the AUEC. \nAlso Check that the CounterParty Venue you want to trade has permissions to trade the same AUEC.");
            }

        }

        public static bool ValidateAUECandCV(int AUECID, int counterPartyID, int VenueID)//, int userID)
        {
            if (CachedDataManager.GetInstance.CheckTradePermissionByCVandAUECID(AUECID, counterPartyID, VenueID))
            {
                return true;
            }
            else
            {
                throw new Exception("Check if you have permission for the AUEC. \nAlso Check that the CounterParty Venue you want to trade has permissions to trade the same AUEC.");
            }
        }

        public void SendTradeToServer(Prana.BusinessObjects.OrderSingle order)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsWithinLimits(Prana.BusinessObjects.OrderSingle order, double marketPrice)
        {
            bool IsWithinLimit = true;
            if (priceSymbolSettings.RiskCtrlCheck)
            {
                double riskValueUpper = marketPrice + (priceSymbolSettings.RiskValue / 100.00) * marketPrice; // Upper Limit for Price
                double riskValueLower = marketPrice - (priceSymbolSettings.RiskValue / 100.00) * marketPrice; // Upper Limit for Price

                // check for upper limit breach
                if (order.Price > riskValueUpper)
                {
                    //string str = "Price greater than " + (100 + _riskAndValidateSettings.RiskValue) + " % of current Market Price! Proceed?";
                    string str = "Price greater than " + (priceSymbolSettings.RiskValue) + " % limit set! Proceed?";
                    if ((MessageBox.Show(str, "Warning!", MessageBoxButtons.YesNo)) == DialogResult.No)
                    {
                        IsWithinLimit = false;
                        return IsWithinLimit;
                    }
                }

                // check for lower limit breach
                if (order.Price < riskValueLower)
                {
                    //string str = "Price lower than " + (100 - _riskAndValidateSettings.RiskValue) + " % of current Market Price! Proceed?";
                    string str = "Price lower than " + (priceSymbolSettings.RiskValue) + " % limit set! Proceed?";
                    if ((MessageBox.Show(str, "Warning!", MessageBoxButtons.YesNo)) == DialogResult.No)
                    {
                        IsWithinLimit = false;
                        return IsWithinLimit;
                    }
                }
            }
            return IsWithinLimit;

        }
        #endregion

    }


}
