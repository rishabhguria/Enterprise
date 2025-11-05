// ***********************************************************************
// Assembly         : Bloomberg Library
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="InstructListRequest.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;


namespace Bloomberg.Library.Requests
{
    /// <summary>
    /// Class InstrumentListRequest
    /// </summary>
    public class InstrumentListRequest : AbstractRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentListRequest"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public InstrumentListRequest(Request request)
            : base(request)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentListRequest"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="security">The security.</param>
        /// <param name="maxResults">The max results.</param>
        public InstrumentListRequest(Request request, string security, int maxResults)
            : base(request)
        {
            Query = security;
            MaxResults = maxResults;
        }

        /// <summary>
        /// Gets or sets the max results.
        /// </summary>
        /// <value>The max results.</value>
        public int MaxResults
        {
            get
            {
                if (HasElement("maxResults") && GetElement("maxResults").NumValues > 0)
                    return (int)GetElement("maxResults").GetValue();
                else
                    return 0;
            }
            set
            {
                Set("maxResults", value);
            }
        }

        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>The security.</value>
        public string Query
        {
            get
            {
                if (HasElement("query") && GetElement("query").NumValues > 0)
                    return (string)GetElement("query").GetValue();
                else
                    return string.Empty;
            }
            set
            {
                Set("query", value);
            }
        }

        /// <summary>
        /// Gets or sets the Language Override
        /// </summary>
        public string LanguageOverride
        {
            get
            {
                if (HasElement("languageOverride") && GetElement("languageOverride").NumValues > 0)
                    return (string)GetElement("languageOverride").GetValue();
                else
                    return string.Empty;
            }
            set
            {
                Set("languageOverride", value);
            }
        }

        /// <summary>
        /// Gets or sets the YellowKeyFilter
        /// </summary>
        public string YellowKeyFilter
        {
            get
            {
                if (HasElement("yellowKeyFilter") && GetElement("yellowKeyFilter").NumValues > 0)
                    return (string)GetElement("yellowKeyFilter").GetValue();
                else
                    return string.Empty;
            }
            set
            {
                Set("yellowKeyFilter", value);
            }
        }

    }
}
