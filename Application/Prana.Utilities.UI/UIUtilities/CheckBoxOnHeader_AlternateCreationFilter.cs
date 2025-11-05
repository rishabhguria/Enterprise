using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Windows.Forms;


namespace Prana.Utilities.UI.UIUtilities
{
    public class CheckBoxOnHeader_AlternateCreationFilter : CheckBoxOnHeader_CreationFilter
    {
        private Infragistics.Win.UltraWinGrid.ColumnHeader _columnHeader;
        private Infragistics.Win.UltraWinGrid.ColumnHeader _alternateColumnHeader;
        Infragistics.Win.UltraWinGrid.ColumnHeader _otherColumnHeader = null;

        public Infragistics.Win.UltraWinGrid.ColumnHeader ColumnHeader
        {
            get { return _columnHeader; }
            set { _columnHeader = value; }
        }

        public Infragistics.Win.UltraWinGrid.ColumnHeader AlternateColumnHeader
        {
            get { return _alternateColumnHeader; }
            set { _alternateColumnHeader = value; }
        }

        protected override void aCheckBoxOnHeader_CreationFilter_HeaderCheckBoxClicked(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            // Check to see if the column is of type boolean.  If it is, set all the cells in that column to
            // whatever value the header checkbox is.
            if (e.Header.Column.DataType == typeof(bool))
            {
                foreach (UltraGridRow aRow in e.Rows)
                {
                    aRow.Cells[e.Header.Column.Index].Value = (e.CurrentCheckState == CheckState.Checked);
                    aRow.Update();
                    //if (_otherColumnHeader != null)
                    //{
                    //    aRow.Cells[_otherColumnHeader.Column.Index].Value = !(e.CurrentCheckState == CheckState.Checked);
                    //}
                }
            }
        }

        protected override void aCheckBoxUIElement_ElementClick(Object sender, Infragistics.Win.UIElementEventArgs e)
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

            if (aCheckBoxUIElement.CheckState == CheckState.Checked)
            {
                if (aColumnHeader == _columnHeader)
                {
                    _otherColumnHeader = _alternateColumnHeader;
                }
                else if (aColumnHeader == _alternateColumnHeader)
                {
                    _otherColumnHeader = _columnHeader;
                }

                CheckState otherCheckState = CheckState.Indeterminate;
                if (_otherColumnHeader != null)
                {
                    switch (aCheckBoxUIElement.CheckState)
                    {
                        case CheckState.Checked:
                            otherCheckState = CheckState.Unchecked;
                            break;
                        case CheckState.Unchecked:
                            otherCheckState = CheckState.Checked;
                            break;
                        case CheckState.Indeterminate:
                            otherCheckState = CheckState.Indeterminate;
                            break;
                    }

                    _otherColumnHeader.Tag = otherCheckState;

                    HeaderCheckBoxEventArgs alternateEventArg = new HeaderCheckBoxEventArgs(_otherColumnHeader, otherCheckState, hRows);
                    ON_CLICKED(alternateEventArg);
                }
            }
        }


    }
}
