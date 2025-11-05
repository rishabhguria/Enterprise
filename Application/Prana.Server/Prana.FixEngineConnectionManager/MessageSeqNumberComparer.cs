using System;
using System.Collections.Generic;

namespace Prana.FixEngineConnectionManager
{
    public class MessageSeqNumberComparer : IComparer<Int64>
    {

        #region IComparer<long> Members

        int IComparer<Int64>.Compare(Int64 x, Int64 y)
        {
            return Compare(x, y);
        }
        #endregion

        protected int Compare(Int64 x, Int64 y)
        {
            if (x > y)
                return 1;
            else if (x == y)
                return 0;
            else
                return -1;
        }
    }
}
