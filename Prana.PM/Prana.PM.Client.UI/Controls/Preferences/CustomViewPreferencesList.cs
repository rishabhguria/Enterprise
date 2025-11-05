using Prana.BusinessObjects;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace Prana.PM.BLL
{
    [XmlRoot("CustomViewPreferencesList")]
    [Serializable]
    public class CustomViewPreferencesList : SerializableDictionary<string, CustomViewPreferences>
    {
        public CustomViewPreferencesList() : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        protected CustomViewPreferencesList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
