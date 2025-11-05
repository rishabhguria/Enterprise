using Bloomberglp.Blpapi;
using System;
using System.Collections.Generic;

namespace Bloomberg.Library
{

    /// <summary>
    /// Instrument List Response
    /// </summary>
    public class InstrumentListResponse : EventArgs
    {
        /// <summary>
        /// List of Securities
        /// </summary>
        public List<SecurityInfo> Securities = new List<SecurityInfo>();

        /// <summary>
        /// InstrumentListResponse Construct
        /// </summary>
        /// <param name="message"></param>
        public InstrumentListResponse(Message message)
        {
            Element results = message.AsElement.GetElement(new Name("results"));
            for (int i = 0; i < results.NumValues; i++)
            {
                Element result = results.GetValueAsElement(i);
                string security = result.GetElementAsString(new Name("security"));
                security = security.Replace("<", " ").Replace(">", "").Replace("cmdty", "comdty").Replace("crncy", "curncy");
                SecurityInfo info = new SecurityInfo(security);
                Securities.Add(info);
            }
        }
    }
}
