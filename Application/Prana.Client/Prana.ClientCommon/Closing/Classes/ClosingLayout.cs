using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.ClientCommon
{
    [XmlRoot("ClosingGridPrefs")]
    [Serializable]
    public class ClosingLayout

    {

        [XmlArray("ShortPositionColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> ShortPositionColumns = new List<ColumnData>();

        [XmlArray("LongPositionColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> LongPositionColumns = new List<ColumnData>();

        [XmlArray("NetPositionColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> NetPositionColumns = new List<ColumnData>();

        [XmlArray("CloseOrderNetPositionColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> CloseOrderNetPositionColumns = new List<ColumnData>();
    }
}
