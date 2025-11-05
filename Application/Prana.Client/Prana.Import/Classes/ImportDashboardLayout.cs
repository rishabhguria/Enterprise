using Prana.ClientCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Import
{
    [XmlRoot("ImportDashboardLayout")]
    [Serializable]
    public class ImportDashboardLayout
    {
        [XmlArray("grdImportDashboardColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> ImportDashboardDataColumns = new List<ColumnData>();
    }
}