using Prana.ClientCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.ReconciliationNew
{
    [XmlRoot("ReconDashboardLayout")]
    [Serializable]
    public class ReconDashboardLayout
    {
        [XmlArray("grdReconDashboardColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> ReconDashboardDataColumns = new List<ColumnData>();
    }
}