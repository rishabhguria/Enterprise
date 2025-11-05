using System;
using System.Collections.Generic;

namespace Prana.PM.BLL
{
    public class ReconLogicBase : IComparer<ReconPosition>
    {
        #region IComparer<ReconPosition> Members

        public int Compare(ReconPosition x, ReconPosition y)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        //public int CompareOrder(ReconPosition mmidX, ReconPosition mmidY)
        //{
        //    int compare = 0;
        //    return compare;
        //}
    }
}
