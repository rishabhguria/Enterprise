using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.Allocation.Common.Definitions;

namespace Prana.AllocationNew
{
    interface  IAllocationCalculator
    {
        void SetUp(AccountCollection accounts);

        AllocationLevelList GetAllocationAccounts(AllocationGroup group);

        void SetAllocationAccounts(AllocationGroup group,bool shouldClear);

        void SetSelectionStatus(bool multipleSelected);

        void SetAllocationDefault(AllocationDefault allocationDefault);
        void HideControl(bool visible);
        
        void ClearQty();
        void ClearPercentage();
        void SetAllocationDefault(AllocationOperationPreference allocationOperationPreference);
        
        SerializableDictionary<int, AccountValue> GetAllocationAccountValue();
        event EventHandler ChangePreference;


        void SetAllocationAccounts(AllocationGroup allocationGroup);

        void SetQuantity(decimal p);
    }
}
