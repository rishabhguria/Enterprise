using Prana.ClientCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Import
{
    [XmlRoot("SymbolManagementLayout")]
    [Serializable]
    public class SymbolManagementLayout
    {
        [XmlArray("SymbolManagementColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> SymbolManagementColumns = new List<ColumnData>();
    }
}
