using Prana.ClientCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Tools
{
    [XmlRoot("PricingPrefs")]
    [Serializable]
    public class PricingLayout
    {
        [XmlArray("PricingInputColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> PricingInputColumns = new List<ColumnData>();
    }
}