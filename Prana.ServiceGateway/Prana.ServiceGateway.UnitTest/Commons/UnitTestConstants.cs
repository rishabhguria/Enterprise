using Prana.ServiceGateway.Models.RequestDto;

namespace Prana.ServiceGateway.UnitTest.Commons
{
    public class UnitTestConstants
    {
        #region Mock Constants
        public const string CONST_EXPECTED_MOCK_MESSAGE = "Expected mock message";
        public const string CONST_REQUEST_DATA = "Request data";
        public const string CONST_MOCK_TOPIC = "Mock Topic";
        public const string CONST_MOCK_SYMBOL = "MOCK";
        public const string CONST_ORDER_ID = "2021111610291846";
        public const string CONST_LAYOUT = "{\"pageInfo\":{\"pageId\":\"page-c9\",\"pageLayout\":\"\",\"pageName\":\"Demo\",\"pageTag\":\"\",\"oldPageName\":\"Demo\"},\"internalPageInfo\":[{\"title\":\"Demo\",\"oldTitle\":\"\",\"description\":\"\",\"oldViewName\":\"Demo\",\"viewId\":\"page-c9\",\"viewName\":\"Demo\"}]}";
        public const string CONST_ID = "1";
        public const string CONST_TOKEN = "Token";
        public const string CONST_MYTAB = "MyTab";
        public const string CONST_NEWTAB = "NewTab";
        public const string CONST_TAB1 = "Tab1";
        public const string CONST_TAB2 = "Tab2";
        public const string CONST_TAB3 = "Tab3";
        public const string CONST_FUND = "FUND";
        public const string CONST_LOADLAYOUT = "{\"pageId\":\"page1\",\"viewName\":\"View1\"}";
        public static readonly OrdersActionRequestDto CONST_REQUEST_DTO_ORDER = new OrdersActionRequestDto
        {
            CommaSeparatedParentClOrderIds = "2021111610291846"
        };
        public static readonly SaveManualFillsDto CONST_REQUEST_DTO_SAVE_MANUAL = new SaveManualFillsDto
        {
            FillsDataTable = "Request data"
        };
        public static readonly SaveAllocationDetailsDto CONST_REQUEST_DTO_SAVE_ALLOCATION = new SaveAllocationDetailsDto
        {
            AllocationDetails = "Request data"
        };
        #endregion


        public static readonly SymbolAccountPositionRequestDto SymbolAccountPositionRequestDto = new SymbolAccountPositionRequestDto
        {
            CurrencyID = 1,
            RequestID = "RequestID",
            Symbol = "AAPL"
        };

        public static readonly ValidateSymbolRequestDto ValidateSymbolRequestDto= new ValidateSymbolRequestDto
        {
            Symbol = "AAPL"
        };

        public static readonly SymbolSearchRequestDto SymbolSearchReqDto = new SymbolSearchRequestDto
        {
            Symbol = "AAPL"
        };

        #region Error message
        public const string CONST_AN_ERROR_OCCURRED = "An error occurred ";
        #endregion
    }
}
