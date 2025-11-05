using System;
using System.Xml.Serialization;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// It just holds the preferences to be set for the grid.
    /// </summary>
    [Serializable]
    public class GridColorPreferences
    {
        public GridColorPreferences()
        {

        }

        /// Row Color<remarks/>
        [XmlElement("Row")]
        public string Row;

        /// <remarks/>
        [XmlElement("AlternateRow")]
        public string AlternateRow;

        ///Allow to change the column color or condional column text color.
        //		[XmlElement("AlternateRow")]
        //		public string ColumnTextColor;
        //
        //		[XmlElement("AlternateRow")]
        //		public string ConditionalColumnTextColor;

        /// <remarks/>
        [XmlElement("GridHeader")]
        public string GridHeader;

        /// <remarks/>
        [XmlElement("GridBackGround")]
        public string GridBackGround;

        /// <remarks/>
        [XmlElement("GridLine")]
        public string GridLine;
    }
}
