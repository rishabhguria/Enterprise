using Csla;
using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// List of the MarkPriceBase class, displayed in the Mark Position Control.
    /// </summary>
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class DayMarkPriceList : BusinessListBase<DayMarkPriceList, DayMarkPrice>
    {
        public DayMarkPriceList()
        {
            MarkAsChild();
        }
    }
}
