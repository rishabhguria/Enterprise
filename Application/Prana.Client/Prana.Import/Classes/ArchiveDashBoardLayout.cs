using Prana.ClientCommon;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Import.Classes
{
    public class ArchiveDashBoardLayout
    {
        [XmlArray("ImportReportColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> archiveGridColumns = new List<ColumnData>();
    }
}
