using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// EntitlementData (eidData)
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class Entitlement
    {

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        /// <remarks></remarks>
        public string Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public Entitlement()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Entitlement"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <remarks></remarks>
        public Entitlement(Element element)
        {
            Data = element.ToString();
            //Data = element.GetElementAsString("eidData").ToString();
        }

    }
}
