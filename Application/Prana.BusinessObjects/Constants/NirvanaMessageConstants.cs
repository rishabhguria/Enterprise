namespace Prana.BusinessObjects
{
    public class PranaMessageConstants
    {
        /// <summary>
        /// This message indicated that container service is down
        /// </summary>
        public const string MSG_AnotherInstanceSubscribed = "Another instance of {0} is connected to the service: {1}";

        public const string MSG_ExpPNLCalc = "EX";
        public const string MSG_ExpPNLCalcSummary = "EX_Sum";
        public const string MSG_IndicesReturnSummary = "EX_Index";
        public const string MSG_ExpPNLCtrl = "EX_Ctrl"; //this message type woould be used to send control info
        public const string MSG_EPNlItemList = "Ex_List";
        public const string MSG_EPNlTaxLotListStart = "Ex_TLStart";
        public const string MSG_EPNlTaxLotList = "Ex_TL";
        public const string MSG_EPNlTaxLotListEnd = "Ex_TLEnd";
        public const string MSG_EPNlRefreshResponse = "Ex_RefeshResponse";
        public const string MSG_UserInputData = "Ex_UserInputData";
        public const string MSG_PMPreferences = "Ex_PMPreferences";
        public const string MSG_Header = "Ex_Header";

        /// <summary>
        /// /// MSG_ExpPNLSubscription indicates Start of Message !!
        /// </summary>
        public const string MSG_ExpPNLStartOfMessage = "EX_SOM";

        public const string MSG_FilterDetails = "EX_FilterDetails";


        /// <summary>
        /// /// MSG_ExpPNLUpdatePreferences indicates UpdatePreference Message !!
        /// </summary>
        public const string MSG_ExpPNLUpdatePreferences = "EX_UpPref";
        /// <summary>
        /// /// MSG_ExpPNLDynamicColumnList indicates List of Columns that will be sent dynamically 
        /// </summary>
        public const string MSG_ExpPNLDynamicColumnList = "EX_DynCol";

        public const string MSG_ExpPNLIndexColumns = "EX_IndCol";

        /// <summary>
        /// /// MSG_ExpPNLEndOfMessage indicates End of Message !!
        /// </summary>
        public const string MSG_ExpPNLEndOfMessage = "EX_EOM";

        /// <summary>
        /// MSG_ExpPNLSubscription wiil be used to communicate subscription information !!
        /// </summary>
        public const string MSG_ExpPNLSubscription = "EX_Sub";
        /// <summary>
        /// MSG_ExpPNLRefreshData is used to ask exposure pnl service to fetch the latest data from db and send to clients.
        /// </summary>
        public const string MSG_ExpPNLRefreshData = "EX_Refresh";
        /// <summary>
        /// MSG_ExpPNLUserBusy is used to communicate to exposure pnl service is a client is busy updating the UI
        /// </summary>
        public const string MSG_ExpPNLUserBusy = "EX_Busy";

        /// <summary>
        /// MSG_GetLiveFeedSnapShot is used to ask exposure pnl service to fetch the latest data from db and send to clients.
        /// </summary>
        public const string MSG_GetLiveFeedSnapShot = "Ex_GetLiveFeedSnapShot";
        /// <summary>
        /// Prana Custom Message type. To be used when user sends an accept corresponding to Client's trading instruction. (MsgType Tag 35= "ATS")   
        /// </summary>
        public const string MSGTradingInstClientAccept = "ATS";

        /// <summary>
        /// Prana Custom Message type. To be used when user sends an accept corresponding to an Internal trading instruction. (MsgType Tag 35= "ATN")   
        /// </summary>
        public const string MSGTradingInstInternalAccept = "ATN";

        /// <summary>
        /// Prana Custom Message type. To be used when the server sends trading instructions to clients. (MsgType Tag 35= "TIS")   
        /// Trading Instructions received from External Sell side clients like MOXY 
        /// </summary>
        public const string MSGTradingInstClient = "TIS";

        /// <summary>
        /// Prana Custom Message type. To be used when the server sends trading instructions to clients. (MsgType Tag 35= "TIN")   
        /// Trading Instructions received internally from the likes of Account Manager using instances of PranaClient.
        /// </summary>
        public const string MSGTradingInstInternal = "TIN";

        /// <summary>
        /// This message indicated that there is some issue while sending the trade from TT
        /// </summary>
        public const string MSG_SendOrderError = "Complete information is not avaiable from Core Service(s) to send this Order. Please contact Administrator.";
    }
}
