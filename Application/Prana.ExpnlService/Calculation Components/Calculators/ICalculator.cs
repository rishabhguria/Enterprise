using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    public interface ICalculator
    {
        Dictionary<int, DateTime> AUECWiseAdjustedDateTime
        {
            get;
            set;
        }
        Dictionary<int, DateTime> ClearanceDateTime
        {
            get;
            set;
        }
        void Calculate(EPnlOrder order);
    }
}
