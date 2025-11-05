using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Prana.PNL.UI.Preferences
{
    /// <summary>
    /// This class is the root class to store the column related preferences based on 
    /// different custom views
    /// </summary>
    [XmlRoot("PNLPreferences")]
    [Serializable]
    public class PNLPreference
    {
        [XmlElement("TabName", typeof(string))]
        public string TabName;

        [XmlArray("DeselectedColumnsCollection"), XmlArrayItem("Column", typeof(string))]
        public ArrayList DeselectedColumnsCollection;

        [XmlArray("GroupByColumnsCollection"), XmlArrayItem("GroupByColumn", typeof(string))]
        public ArrayList GroupByColumnsCollection;

        public PNLPreference()
        {
            TabName = string.Empty;
            DeselectedColumnsCollection = new ArrayList();
            GroupByColumnsCollection = new ArrayList();
        }
    }
}
