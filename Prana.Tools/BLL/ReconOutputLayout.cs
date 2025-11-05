using Prana.ClientCommon;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Tools.BLL
{
    public class ReconOutputLayout
    {
        [XmlArray("ReconOutput"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> reconOutputGridColumns = new List<ColumnData>();
    }
}
