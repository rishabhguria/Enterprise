using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// Summary description for CheckBoxOnHeader_CreationFilter.
    /// </summary>
    public class CheckBoxOnHeader_CreationFilter : IUIElementCreationFilter, IDisposable
    {
        // This event will fire when the CheckBox is clicked. 
        public delegate void HeaderCheckBoxClickedHandler(object sender, HeaderCheckBoxEventArgs e);
        public event HeaderCheckBoxClickedHandler _CLICKED;
        protected CheckBoxUIElement aCheckBoxUIElement = null;
        protected Infragistics.Win.UltraWinGrid.ColumnHeader aColumnHeader = null;


        //The following property takes a Boolean value and can be set under following situations
        //true: when one wants to select all check boxes
        //false: when a user wants to check only editable check box cells (except Activation.NoEdit)
        private Boolean _checkDisabledBoxes = true;
        public Boolean checkDisabledBoxes
        {
            get { return _checkDisabledBoxes; }
            set { _checkDisabledBoxes = value; }
        }

        private List<string> _columns = null;
        public List<string> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }


        public CheckBoxOnHeader_CreationFilter()
        {
            _CLICKED += new HeaderCheckBoxClickedHandler(aCheckBoxOnHeader_CreationFilter_HeaderCheckBoxClicked);
        }

        public CheckBoxOnHeader_CreationFilter(List<string> columnList)
        {
            _CLICKED += new HeaderCheckBoxClickedHandler(aCheckBoxOnHeader_CreationFilter_HeaderCheckBoxClicked);
            _columns = columnList;
        }

        protected virtual void aCheckBoxOnHeader_CreationFilter_HeaderCheckBoxClicked(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            // Check to see if the column is of type boolean.  If it is, set all the cells in that column to
            // whatever value the header checkbox is.
            if (e.Header.Column.DataType == typeof(bool))
            {
                UltraGridRow[] rows = e.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow aRow in rows)
                {
                    if (checkDisabledBoxes || !(!checkDisabledBoxes && aRow.Cells[e.Header.Column.Index].Activation == Activation.NoEdit))
                    {
                        aRow.Cells[e.Header.Column.Index].Value = (e.CurrentCheckState == CheckState.Checked);
                        aRow.Update();
                    }
                }
            }
        }

        public virtual void SelectUnSelectAll(UltraGrid grid, bool selectAll, string checkBoxName)
        {
            if (grid.Rows.Count > 0)
            {
                if (selectAll.Equals(true))
                {
                    foreach (UltraGridRow aRow in grid.Rows)
                    {
                        if (aRow.Cells.Exists(checkBoxName))
                        {
                            aRow.Cells[checkBoxName].Value = CheckState.Checked;
                        }
                    }
                }
                else
                {
                    foreach (UltraGridRow aRow in grid.Rows)
                    {
                        if (aRow.Cells.Exists(checkBoxName))
                        {
                            aRow.Cells[checkBoxName].Value = CheckState.Unchecked;
                        }
                    }
                }
            }
        }

        // EventArgs used for the HeaderCheckBoxClicked event. This event has to pass in the CheckState and the ColumnHeader
        #region HeaderCheckBoxEventArgs
        public class HeaderCheckBoxEventArgs : EventArgs
        {
            private Infragistics.Win.UltraWinGrid.ColumnHeader mvarColumnHeader;
            private CheckState mvarCheckState;
            private RowsCollection mvarRowsCollection;

            public HeaderCheckBoxEventArgs(Infragistics.Win.UltraWinGrid.ColumnHeader hdrColumnHeader, CheckState chkCheckState, RowsCollection Rows)
            {
                mvarColumnHeader = hdrColumnHeader;
                mvarCheckState = chkCheckState;
                mvarRowsCollection = Rows;
            }

            // Expose the rows collection for the specific row island that the header belongs to
            public RowsCollection Rows
            {
                get
                {
                    return mvarRowsCollection;
                }
            }

            public Infragistics.Win.UltraWinGrid.ColumnHeader Header
            {
                get
                {
                    return mvarColumnHeader;
                }
            }

            public CheckState CurrentCheckState
            {
                get
                {
                    return mvarCheckState;
                }
                set
                {
                    mvarCheckState = value;
                }
            }
        }
        #endregion
        public CheckState Checked
        {
            //get{return aColumnHeader.Tag;}
            set
            {
                if (aColumnHeader != null)
                    aColumnHeader.Tag = value;
            }
        }

        protected virtual void aCheckBoxUIElement_ElementClick(Object sender, Infragistics.Win.UIElementEventArgs e)
        {
            // Get the CheckBoxUIElement that was clicked
            aCheckBoxUIElement = (CheckBoxUIElement)e.Element;

            // Get the Header associated with this particular element
            aColumnHeader = (Infragistics.Win.UltraWinGrid.ColumnHeader)aCheckBoxUIElement.GetAncestor(typeof(HeaderUIElement)).GetContext(typeof(Infragistics.Win.UltraWinGrid.ColumnHeader));

            // Set the Tag on the Header to the new CheckState
            aColumnHeader.Tag = aCheckBoxUIElement.CheckState;

            // So that we can apply various changes only to the relevant Rows collection that the header belongs to
            HeaderUIElement aHeaderUIElement = aCheckBoxUIElement.GetAncestor(typeof(HeaderUIElement)) as HeaderUIElement;
            RowsCollection hRows = aHeaderUIElement.GetContext(typeof(RowsCollection)) as RowsCollection;

            HeaderCheckBoxEventArgs eventArg = new HeaderCheckBoxEventArgs(aColumnHeader, aCheckBoxUIElement.CheckState, hRows);
            ON_CLICKED(eventArg);
        }

        public void ON_CLICKED(HeaderCheckBoxEventArgs e)
        {
            // Raise an event so the programmer can do something when the CheckState changes
            if (_CLICKED != null)
                _CLICKED(this, e);
        }

        public bool BeforeCreateChildElements(Infragistics.Win.UIElement parent)  // Implements Infragistics.Win.IUIElementCreationFilter.BeforeCreateChildElements
        {
            // Don't need to do anything here
            return false;
        }

        public void AfterCreateChildElements(Infragistics.Win.UIElement parent) // Implements Infragistics.Win.IUIElementCreationFilter.AfterCreateChildElements
        {
            // Check for the HeaderUIElement
            if (parent is HeaderUIElement)
            {
                // Get the HeaderBase object from the HeaderUIElement
                Infragistics.Win.UltraWinGrid.HeaderBase aHeader = ((HeaderUIElement)parent).Header;
                // Only put the checkbox into headers whose DataType is boolean
                if (aHeader.Column != null && aHeader.Column.DataType == typeof(bool))
                {
                    if (this.Columns != null && this.Columns.Count > 0)
                    {
                        foreach (string columnName in this.Columns)
                        {
                            if (aHeader.Column.Key.Equals(columnName))
                            {
                                return;
                            }
                        }
                    }

                    TextUIElement aTextUIElement;
                    aCheckBoxUIElement = (CheckBoxUIElement)parent.GetDescendant(typeof(CheckBoxUIElement));

                    // Since the grid sometimes re-uses UIElements, we need to check to make sure 
                    // the header does not already have a CheckBoxUIElement attached to it.
                    // If it does, we just get a reference to the existing CheckBoxUIElement,
                    // and reset its properties.
                    if (aCheckBoxUIElement == null)
                    {
                        //Create a New CheckBoxUIElement
                        aCheckBoxUIElement = new CheckBoxUIElement(parent);
                    }

                    // Get the TextUIElement - this is where the text for the 
                    // Header is displayed. We need this so we can push it to the right
                    // in order to make room for the CheckBox
                    aTextUIElement = (TextUIElement)parent.GetDescendant(typeof(TextUIElement));

                    // Sanity check
                    if (aTextUIElement == null)
                        return;

                    // Get the Header and see if the Tag has been set. If the Tag is 
                    // set, we will assume it's the stored CheckState. This has to be
                    // done in order to maintain the CheckState when the grid repaints and
                    // UIElement are destroyed and recreated. 
                    Infragistics.Win.UltraWinGrid.ColumnHeader aColumnHeader =
                        (Infragistics.Win.UltraWinGrid.ColumnHeader)aCheckBoxUIElement.GetAncestor(typeof(HeaderUIElement))
                        .GetContext(typeof(Infragistics.Win.UltraWinGrid.ColumnHeader));

                    if (aColumnHeader.Tag == null)
                    //If the tag was nothing, this is probably the first time this 
                    //Header is being displayed, so default to Indeterminate
                    {
                        aColumnHeader.Tag = CheckState.Unchecked;
                    }
                    else
                    {

                        aCheckBoxUIElement.CheckState = (CheckState)aColumnHeader.Tag;
                    }

                    // Hook the ElementClick of the CheckBoxUIElement
                    aCheckBoxUIElement.ElementClick += new UIElementEventHandler(aCheckBoxUIElement_ElementClick);

                    // Add the CheckBoxUIElement to the HeaderUIElement
                    parent.ChildElements.Add(aCheckBoxUIElement);

                    // Push the TextUIElement to the right a little to make 
                    // room for the CheckBox. 3 pixels of padding are used again. 
                    //aTextUIElement.Rect = new Rectangle(aCheckBoxUIElement.Rect.Right + 3, aTextUIElement.Rect.Y, parent.Rect.Width - (aCheckBoxUIElement.Rect.Right - parent.Rect.X), aTextUIElement.Rect.Height);
                    aTextUIElement.Rect = new Rectangle(aTextUIElement.Rect.X, aTextUIElement.Rect.Y, parent.Rect.Width, parent.Rect.Height); // parent.Rect.Width - (aCheckBoxUIElement.Rect.Right - parent.Rect.X), aTextUIElement.Rect.Height);

                    // Position the CheckBoxUIElement. The number 3 here is used for 3
                    // pixels of padding between the CheckBox and the edge of the Header.
                    // The CheckBox is shifted down slightly so it is centered in the header.
                    //					aCheckBoxUIElement.Rect = new Rectangle(parent.Rect.X + 3,  parent.Rect.Y + ((parent.Rect.Height - aCheckBoxUIElement.CheckSize.Height) / 2), aCheckBoxUIElement.CheckSize.Width, aCheckBoxUIElement.CheckSize.Height);
                    //aCheckBoxUIElement.Rect = new Rectangle(parent.Rect.X + ((parent.Rect.Width - aCheckBoxUIElement.CheckSize.Width ) / 2), parent.Rect.Y + ((parent.Rect.Height - aCheckBoxUIElement.CheckSize.Height) / 2), aCheckBoxUIElement.CheckSize.Width, aCheckBoxUIElement.CheckSize.Height);
                    aCheckBoxUIElement.Rect = new Rectangle(aTextUIElement.Rect.X + aTextUIElement.Rect.Width / 2 - 8, parent.Rect.Y + ((parent.Rect.Height - aCheckBoxUIElement.CheckSize.Height) / 2) + 3, aCheckBoxUIElement.CheckSize.Width, aCheckBoxUIElement.CheckSize.Height);


                }
                else
                {
                    // If the column is not a boolean column, we do not want to have a checkbox in it
                    // Since UIElements can be reused by the grid, there is a chance that one of the
                    // HeaderUIElements that we added a checkbox to for a boolean column header
                    // will be reused in a column that is not boolean.  In this case, we must remove
                    // the checkbox so that it will not appear in an inappropriate column header.
                    CheckBoxUIElement aCheckBoxUIElement = (CheckBoxUIElement)parent.GetDescendant(typeof(CheckBoxUIElement));

                    if (aCheckBoxUIElement != null)
                    {
                        parent.ChildElements.Remove(aCheckBoxUIElement);
                        aCheckBoxUIElement.Dispose();
                    }
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            _CLICKED -= new HeaderCheckBoxClickedHandler(aCheckBoxOnHeader_CreationFilter_HeaderCheckBoxClicked);
            if (aCheckBoxUIElement != null)
            {
                aCheckBoxUIElement.Dispose();
            }

            if (aColumnHeader != null)
                aColumnHeader.Dispose();
        }
        #endregion
    }




}
