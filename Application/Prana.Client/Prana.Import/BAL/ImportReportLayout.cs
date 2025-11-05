using Prana.ClientCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Import
{
    [XmlRoot("ImportReportLayout")]
    [Serializable]
    public class ImportReportLayout
    {
        [XmlArray("ImportReportColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> ImportReportColumns = new List<ColumnData>();
    }
}
