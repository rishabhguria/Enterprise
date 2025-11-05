namespace Prana.OptionCalculator.CalculationComponent
{
    class IDCDataRequest
    {
        private string m_Request = null;
        private string m_Symbols = string.Empty;
        private string m_Fields = null;
        private string m_Options = null;
        private bool m_ReceiveCRC = false;

        #region Properties
        #region Commented

        #endregion

        public string Query
        {
            get
            {
                return ("Request=" + m_Request +
                        ((m_Symbols == null) ? "" : " (" + m_Symbols + ")") +
                        ((m_Fields == null) ? "" : ",(" + m_Fields + ")") +
                        ((m_Options == null) ? "" : "," + m_Options) +
                        "&Done=" + ((m_ReceiveCRC) ? "flag" : "request"));
            }
        }
        #endregion

        public IDCDataRequest()
        {
            m_Fields = null;
            m_Symbols = null;
        }

        public IDCDataRequest(string CommaSeparatedRequest, string CommaSeparatedSymbols, string CommaSeparatedFields, string CommaSeparatedOptions)
        {
            m_Request = (CommaSeparatedRequest.Trim().Length == 0) ? null : CommaSeparatedRequest;
            m_Symbols = (CommaSeparatedSymbols.Trim().Length == 0) ? null : CommaSeparatedSymbols;
            m_Fields = (CommaSeparatedFields.Trim().Length == 0) ? null : CommaSeparatedFields;
            m_Options = (CommaSeparatedOptions.Trim().Length == 0) ? null : CommaSeparatedOptions;
        }
        public string Symbol
        {
            get { return m_Symbols; }
        }
    }
}
