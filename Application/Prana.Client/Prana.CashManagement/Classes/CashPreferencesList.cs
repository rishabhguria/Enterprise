using Prana.BusinessObjects;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Prana.CashManagement.Classes
{
    [XmlRoot("CashManagementLayoutDetailsList")]
    [Serializable]
    public class CashPreferencesList : SerializableDictionary<string, CashManagementLayout>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CashPreferencesList"/> class.
        /// </summary>
        public CashPreferencesList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CashPreferencesList"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected CashPreferencesList(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructors
    }
}
