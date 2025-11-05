using Bloomberglp.Blpapi;

namespace Bloomberg.Library.Requests
{
    /// <summary>
    /// Class ReferenceDataRequest
    /// </summary>
    public class ReferenceDataRequest : ProductRequest
    {
        private static readonly Name returnEids = Name.GetName("returnEids");
        private static readonly Name returnFormattedValue = Name.GetName("returnFormattedValue");
        private static readonly Name useUTCTime = Name.GetName("useUTCTime");
        private static readonly Name forcedDelay = Name.GetName("forcedDelay");


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDataRequest"></see> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public ReferenceDataRequest(Request request)
            : base(request)
        {

        }
        public ReferenceDataRequest(Request request, bool returnEids, bool returnFormattedValue, bool useUTCTime, bool forcedDelay)
            : base(request)
        {
            ReturnEids = returnEids;
            ReturnFormattedValue = returnFormattedValue;
            UseUTCTime = useUTCTime;
            ForcedDelay = forcedDelay;
        }

        /// <summary>
        /// Setting this to TRUE will populate fieldData with an extra element containing a name and value for the EID date.
        /// </summary>
        /// <value><c>true</c> if [return eids]; otherwise, <c>false</c>.</value>
        public bool ReturnEids
        {
            get
            {
                if (HasElement(returnEids) && GetElement(returnEids).NumValues > 0)
                    return (bool)GetElement(returnEids).GetValue();
                else
                    return false;
            }
            set
            {
                Set(returnEids, value);
            }
        }

        /// <summary>
        /// Setting to true will force all data to be returned as a string.
        /// </summary>
        /// <value><c>true</c> if [return formatted value]; otherwise, <c>false</c>.</value>
        public bool ReturnFormattedValue
        {
            get
            {
                if (HasElement(returnFormattedValue) && GetElement(returnFormattedValue).NumValues > 0)
                    return (bool)GetElement(returnFormattedValue).GetValue();
                else
                    return false;
            }
            set
            {
                Set(returnFormattedValue, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use UTC time].
        /// </summary>
        /// <value><c>true</c> if [use UTC time]; otherwise, <c>false</c>.</value>
        public bool UseUTCTime
        {
            get
            {
                if (HasElement(useUTCTime) && GetElement(useUTCTime).NumValues > 0)
                    return (bool)GetElement(useUTCTime).GetValue();
                else
                    return false;
            }
            set
            {
                Set(useUTCTime, value);
            }
        }

        /// <summary>
        /// Setting to true will return the latest data up to the delay period specified by the exchange for this security.
        /// For example requesting VOD LN Equity and PX_LAST will return a snapshot of the last price from 15mins ago.
        /// </summary>
        /// <value><c>true</c> if [forced delay]; otherwise, <c>false</c>.</value>
        public bool ForcedDelay
        {
            get
            {
                if (HasElement(forcedDelay) && GetElement(forcedDelay).NumValues > 0)
                    return (bool)GetElement(forcedDelay).GetValue();
                else
                    return false;
            }
            set
            {
                Set(forcedDelay, value);
            }
        }
    }
}