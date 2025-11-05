using Infragistics.Win.UltraWinGrid;
using System.Collections.Generic;
using System.Data;

namespace Prana.Utilities.UI.UIUtilities
{
    public class Utils
    {
        /// <summary>
        /// Ultra the combo filter will filter out undesirable columns from Source Combo.
        /// </summary>
        /// <param name="cmbSource">The Source Combo.</param>
        /// <param name="displayKey">Ccolumn name to be displayed and rest will be filtered out.</param>
        public static void UltraComboFilter(UltraCombo cmbSource, string displayKey)
        {
            // Hide Column Header for the Combo Box
            // Only show the displayKey Column

            if (cmbSource.DisplayLayout != null)
            {
                if (cmbSource.DisplayLayout.Bands != null && cmbSource.DisplayLayout.Bands.Count > 0)
                {
                    ColumnsCollection columnsSources = cmbSource.DisplayLayout.Bands[0].Columns;
                    cmbSource.DisplayLayout.Bands[0].ColHeadersVisible = false;

                    foreach (UltraGridColumn column in columnsSources)
                    {
                        if (column.Key != displayKey)
                        {
                            column.Hidden = true;
                        }
                    }
                    cmbSource.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                }

            }

        }


        /// <summary>
        /// Author : Rajat 
        /// Dated : 25 Oct 2006
        /// Filters the Ultrasdropdown and hide all columns except the displaykey. Generally used in the UltraGrid
        /// </summary>
        /// <param name="cmbSource">The CMB source.</param>
        /// <param name="displayKey">The display key.</param>
        public static void UltraDropDownFilter(UltraDropDown cmbSource, string displayKey)
        {
            // Hide Column Header for the Combo Box
            // Only show the displayKey Column
            ColumnsCollection columnsSources = cmbSource.DisplayLayout.Bands[0].Columns;
            cmbSource.DisplayLayout.Bands[0].ColHeadersVisible = false;

            foreach (UltraGridColumn column in columnsSources)
            {
                if (column.Key != displayKey)
                {
                    column.Hidden = true;
                }
            }

            cmbSource.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
        }

        /// <summary>
        /// Gets the column order list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetColumnOrderList(IDataReader reader)
        {
            Dictionary<string, int> columnorderInfo = new Dictionary<string, int>();

            for (int counter = 0; counter < reader.FieldCount; counter++)
            {
                columnorderInfo.Add(reader.GetName(counter), counter);
            }
            return columnorderInfo;
        }

        public void MessageBox()
        {
        }
    }
}
