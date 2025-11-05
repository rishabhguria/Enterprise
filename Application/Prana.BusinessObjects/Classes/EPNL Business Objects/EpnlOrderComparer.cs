using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// One should use it for comparing the dates.
    /// </summary>
    /// <typeparam name="DateTime"></typeparam>
    public sealed class EpnlOrderComparer : IComparer<EPnlOrder>
    {
        SortingOrder _sortingOrder;

        public EpnlOrderComparer(SortingOrder order)
        {
            _sortingOrder = order;
        }
        #region IComparer<EPnlOrder> Members

        int val = -1;
        int IComparer<EPnlOrder>.Compare(EPnlOrder first, EPnlOrder second)
        {
            DateTime x = first.TransactionDate;
            DateTime y = second.TransactionDate;

            if (x == y)
            {
                val = 0;
            }
            switch (_sortingOrder)
            {
                case SortingOrder.Ascending:
                default:
                    if (x < y)
                    {
                        val = -1;
                    }
                    else if (x > y)
                    {
                        val = 1;
                    }
                    break;
                case SortingOrder.Descending:
                    if (x > y)
                    {
                        val = -1;
                    }
                    else if (x < y)
                    {
                        val = 1;
                    }
                    break;
            }
            return val;
        }

        #endregion
    }
}

