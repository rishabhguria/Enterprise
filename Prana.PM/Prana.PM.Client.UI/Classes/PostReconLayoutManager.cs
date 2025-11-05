using Prana.ClientCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.PM.Client.UI.Classes
{
    [XmlRoot("PostReconLayout")]
    [Serializable]
    public class PostReconLayout
    {
        [XmlArray("grdDataColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> PostReconDataColumns = new List<ColumnData>();

        [XmlArray("grdClosedColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> PostReconClosedColumns = new List<ColumnData>();

        [XmlElement("SplitterLocation")]
        public string SplitterLocation = string.Empty;

        [XmlElement("SplitterHeight")]
        public string SplitterHeight = string.Empty;

    }
}