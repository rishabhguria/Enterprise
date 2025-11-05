using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using Prana.BusinessObjects;

namespace Prana.PNL.UI.Preferences
{
    [XmlRoot("PNLPreferencesList")]
    public class PNLPreferenceList : SerializableDictionary<string, PNLPreference>
    {
    }
}
