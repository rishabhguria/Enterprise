using Prana.BusinessObjects;
namespace Prana.TradingTicket
{
    public class TTPrefSettingsNew : IPreferenceData
    {
        private TradingTicketSettingsCollection _ttSettingsCollection = new TradingTicketSettingsCollection();


        private ConfirmationPopUp _confirmationPopUp = new ConfirmationPopUp();
        private Prana.BusinessObjects.PriceSymbolValidation _riskValidateSettings = new Prana.BusinessObjects.PriceSymbolValidation();




        public TradingTicketSettingsCollection TTSettingsCollection
        {
            get { return this._ttSettingsCollection; }
            set { this._ttSettingsCollection = value; }
        }
        public ConfirmationPopUp ConfirmationPopUp
        {
            get { return this._confirmationPopUp; }
            set { this._confirmationPopUp = value; }
        }
        public Prana.BusinessObjects.PriceSymbolValidation RiskValidateSettings
        {
            get { return this._riskValidateSettings; }
            set { this._riskValidateSettings = value; }
        }


        public TTPrefSettingsNew Clone()
        {
            TTPrefSettingsNew newSetting = new TTPrefSettingsNew();

            #region ConfirmationPopUp Clone
            newSetting.ConfirmationPopUp.CompanyUserID = _confirmationPopUp.CompanyUserID;
            newSetting.ConfirmationPopUp.ISCXL = _confirmationPopUp.ISCXL;
            newSetting.ConfirmationPopUp.ISCXLReplace = _confirmationPopUp.ISCXLReplace;
            newSetting.ConfirmationPopUp.ISNewOrder = _confirmationPopUp.ISNewOrder;
            newSetting.ConfirmationPopUp.IsManualOrder = _confirmationPopUp.IsManualOrder;
            #endregion

            #region TTPrefSetting Clone
            foreach (TradingTicketSettings ttsetting in _ttSettingsCollection)
            {
                TradingTicketSettings newttsetting = new TradingTicketSettings();
                newttsetting.CompanyUserID = ttsetting.CompanyUserID;
                newttsetting.TicketSettingsID = ttsetting.TicketSettingsID;
                newttsetting.AUECID = ttsetting.AUECID;
                newttsetting.ButtonPosition = ttsetting.ButtonPosition;

                newttsetting.Name = ttsetting.Name;
                newttsetting.Description = ttsetting.Description;
                newttsetting.ButtonColor = ttsetting.ButtonColor;
                newttsetting.IsHotButton = ttsetting.IsHotButton;

                newttsetting.AssetID = ttsetting.AssetID;
                newttsetting.UnderLyingID = ttsetting.UnderLyingID;
                newttsetting.CounterpartyID = ttsetting.CounterpartyID;
                newttsetting.VenueID = ttsetting.VenueID;
                newttsetting.SideID = ttsetting.SideID;
                newttsetting.Quantity = ttsetting.Quantity;
                newttsetting.OrderTypeID = ttsetting.OrderTypeID;
                newttsetting.LimitType = ttsetting.LimitType;
                newttsetting.LimitOffset = ttsetting.LimitOffset;
                newttsetting.TIF = ttsetting.TIF;
                newttsetting.HandlingInstructionID = ttsetting.HandlingInstructionID;
                newttsetting.ExecutionInstructionID = ttsetting.ExecutionInstructionID;
                newttsetting.TradingAccountID = ttsetting.TradingAccountID;
                newttsetting.StrategyID = ttsetting.StrategyID;
                newttsetting.AccountID = ttsetting.AccountID;
                newttsetting.Peg = ttsetting.Peg;
                newttsetting.DiscreationOffset = ttsetting.DiscreationOffset;
                newttsetting.DisplayQuantity = ttsetting.DisplayQuantity;
                newttsetting.Random = ttsetting.Random;
                newttsetting.PNP = ttsetting.PNP;

                //newttsetting.ActionButton = ttsetting.ActionButton;
                //newttsetting.ClearingFirmID = ttsetting.ClearingFirmID;
                //newttsetting.ClientCompanyID = ttsetting.ClientCompanyID;
                //newttsetting.ClientAccountID = ttsetting.ClientAccountID;
                //newttsetting.ClientTraderID = ttsetting.CompanyUserID;
                //newttsetting.DefaultTicketID = ttsetting.DefaultTicketID;
                //newttsetting.Display = ttsetting.Display;
                //newttsetting.DisplayName = ttsetting.DisplayName;
                //newttsetting.DisplayPosition = ttsetting.DisplayPosition;

                //newttsetting.OpenClose = ttsetting.OpenClose;
                //newttsetting.Principal = ttsetting.Principal;
                //newttsetting.SettingType = ttsetting.SettingType;
                //newttsetting.ShortExempt = ttsetting.ShortExempt;
                newSetting._ttSettingsCollection.Add(newttsetting);
            }
            #endregion

            #region RiskCtrlValidateSettings Clone
            newSetting.RiskValidateSettings.RiskCtrlCheck = _riskValidateSettings.RiskCtrlCheck;
            newSetting.RiskValidateSettings.RiskValue = _riskValidateSettings.RiskValue;
            newSetting.RiskValidateSettings.ValidateSymbolCheck = _riskValidateSettings.ValidateSymbolCheck;
            newSetting.RiskValidateSettings.CompanyUserID = _riskValidateSettings.CompanyUserID;
            newSetting.RiskValidateSettings.LimitPriceCheck = _riskValidateSettings.LimitPriceCheck;
            #endregion

            return newSetting;
        }

    }
}

