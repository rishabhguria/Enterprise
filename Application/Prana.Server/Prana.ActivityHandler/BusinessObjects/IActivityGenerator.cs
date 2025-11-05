using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System.Collections.Generic;

namespace Prana.ActivityHandler.BusinessObjects
{
    internal interface IActivityGenerator
    {
        List<CashActivity> CreateCashActivity<T>(T data, CashTransactionType transactionSource);
    }
}
