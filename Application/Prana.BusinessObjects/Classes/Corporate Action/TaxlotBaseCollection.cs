using Csla;
using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// TaxlotBaseCollection used for saving positions in corporate actions.
    /// </summary>
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class TaxlotBaseCollection : BusinessListBase<TaxlotBaseCollection, TaxlotBase>
    {
        public void AddRange(TaxlotBaseCollection taxlots)
        {
            if (taxlots != null)
            {
                foreach (TaxlotBase taxlot in taxlots)
                {
                    this.Add(taxlot);
                }
            }

        }
    }
}
