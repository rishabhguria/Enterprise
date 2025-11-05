using System;
using System.ServiceModel;

namespace Prana.BusinessObjects
{
    [ServiceKnownType(typeof(TaxLot))]
    [ServiceKnownType(typeof(AllocationGroup))]
    public interface IFilterable
    {
        DateTime GetDate();

        // default value is the date for which you want to filter the data.
        DateTime GetDateModified();

        int GetAccountID();

        string GetSymbol();
    }
}
