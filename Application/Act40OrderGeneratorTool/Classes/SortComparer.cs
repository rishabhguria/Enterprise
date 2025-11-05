using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections;

namespace Act40OrderGeneratorTool
{
    public class SortComparerDouble : IComparer
    {
        public int Compare(object x, object y)
        {
            try
            {
                String firstValue = (x as UltraGridCell).Value.ToString();
                String secondValue = (y as UltraGridCell).Value.ToString();

                if (Convert.ToDouble(firstValue) > Convert.ToDouble(secondValue))
                    return 1;
                if (Convert.ToDouble(firstValue) < Convert.ToDouble(secondValue))
                    return -1;
            }
            catch
            { }
            return 0;
        }
    }
}
