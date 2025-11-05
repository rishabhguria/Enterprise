using Prana.ClientCommon;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Dashboard.BLL
{
    public class WorkFlowDetailsLayout
    {
        [XmlArray("ImportReportColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> workFlowGridColumns = new List<ColumnData>();
    }
}
