using System;
using System.Collections;
using System.Xml.Serialization;

namespace Prana.LiveFeed.UI
{
    ///Conditional Column colors are still left, which we have hardcoded for now.

    /// <summary>
    /// It stores basic grid column related things like AvailableColumns, DisplayedColumns
    /// sort field and sort direction. DisplayedColumns also stores the things like Name, Position and
    /// Font Color 
    /// </summary>
    [Serializable]
    public class GridColoumnPreferences
    {
        public GridColoumnPreferences()
        {
            AvaialbleColumnsList = new ArrayList();
            DisplayedColumnsList = new ArrayList();
        }

        [XmlAttribute("Name")]
        public string Name;

        [XmlElement("SymbolList")]
        public string SymbolList; //Contains the added symbols

        [XmlArray("AvaialbleColumns"), XmlArrayItem("ColumnName", typeof(string))]
        public ArrayList AvaialbleColumnsList;

        [XmlArray("DisplayColumns"), XmlArrayItem("Column", typeof(string))]
        public ArrayList DisplayedColumnsList;

        [XmlElement("SortColumn")]
        public string SortColumn = string.Empty;

        [XmlElement("SortDirection")]
        public string SortDirection = string.Empty;



    }





}
