using Bloomberglp.Blpapi;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    public class ReferenceDataResponse
    {
        public string Text;

        /// <summary>  
        /// Gets or sets the response error.  
        /// </summary>  
        /// <value>The response error.</value>  
        public ResponseError ResponseError;

        /// <summary>  
        /// Gets or sets the security data.  
        /// </summary>  
        /// <value>The security data.</value>  
        public List<SecurityData> SecurityData;

        /// <summary>  
        /// Initializes a new instance of the <see cref="DataResponse" /> class.  
        /// </summary>  
        public ReferenceDataResponse()
        {

        }

        /// <summary>  
        /// Initializes a new instance of the <see cref="ReferenceDataResponse" /> class.  
        /// </summary>  
        /// <param name="objMessage">The obj message.</param>  
        public ReferenceDataResponse(object objMessage)
        {
            try
            {
                Message message = objMessage as Message;

                SecurityData = new List<SecurityData>();
                if (message.HasElement(new Name("securityData")))
                {
                    Element securities = message.GetElement(new Name("securityData"));
                    int size = securities.NumValues;

                    for (int i = 0; i < size; i++)
                    {
                        SecurityData.Add(new SecurityData(securities.GetValueAsElement(i)));
                    }
                }
                if (message.HasElement(new Name("responseError")))
                {
                    ResponseError = new ResponseError(message.GetElement(new Name("responseError")));
                }

                Text = message.AsElement.ToString();
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite("Error in Constructor ReferenceDataResponse {0}", ex.Message);
            }

        }
    }
}
