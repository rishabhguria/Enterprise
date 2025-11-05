using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;

namespace Prana.Utilities.UI
{
    /// <summary>
    /// for Showing cell text of multi select combo in Ultragrid cell
    /// if selection One - Name of attribute
    /// if more than one - "Multiple"
    /// if All ="All"
    /// http://www.infragistics.com/community/forums/p/89599/442509.aspx#442509
    /// created by omshiv, oct 16 2014
    /// </summary>
    public class CustomCreationFilter : IUIElementCreationFilter
    {
        public CustomCreationFilter() { }

        List<String> _columnList = new List<string>();
        public CustomCreationFilter(List<String> columnList)
        {
            this._columnList = columnList;
        }

        /// <summary>
        /// After Create Child Elements 
        /// </summary>
        /// <param name="parent"></param>
        public void AfterCreateChildElements(UIElement parent)
        {
            if (parent is TextUIElement)
            {
                var text = (TextUIElement)parent;
                var cell = text.GetAncestor(typeof(CellUIElement)) as CellUIElement;

                //check if cell key exist in _columnList, and discard for other columns - omshiv
                if (cell != null && (_columnList.Contains(cell.Cell.Column.Key)))
                {

                    var list = cell.Cell.Value as List<object>;
                    if (list == null)
                        return;

                    if (list.Count == 0 || list.Count == 1)
                    {
                        return;
                    }
                    if (list.Count == ((UltraCombo)cell.Cell.EditorComponentResolved).Rows.Count)
                    {
                        text.Text = "All";
                        return;
                    }

                    text.Text = "Multiple";
                }
            }
        }

        public bool BeforeCreateChildElements(UIElement parent)
        {
            return false;
        }


    }
}
