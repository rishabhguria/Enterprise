using System;

namespace Prana.BusinessObjects
{
    public class CashDataEventArgs : EventArgs
    {
        public CashDataEventArgs(TaxLot givenTaxlot)
        {
            _taxlot = givenTaxlot;
        }
        private TaxLot _taxlot;
        public TaxLot SelectedTaxlot
        {
            get { return _taxlot; }
        }

    }
}
