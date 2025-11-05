using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
namespace Prana.Allocation.BLL
{
    public abstract class BasketAllocationManager
    {
        public abstract void Initilize(int userID, string AllAUECDatesString);
        
        public abstract void UnBundleBasket(Prana.BusinessObjects.BasketDetail basket);
        public abstract void AllocateBasket(BasketDetail basket, object strategies, DateTime AUECLocalDate);
        public abstract void UnAllocateBasket(BasketGroup  basketGroup);
        public abstract void UnGroupBasketGroup(BasketGroup basketGroup);
        public abstract void AllocateBasket(BasketGroup basketGroup, object strategies, DateTime AUECLocalDate);
        public abstract BasketGroup GroupBaskets(Prana.BusinessObjects.BasketCollection baskets, DateTime date);
        //public abstract void UpdateBaskets(DateTime date);
        public  abstract BasketCollection UnallocatedBaskets{get;set;}

        public abstract BasketGroupCollection Groupedbaskets { get;set;}
        public abstract BasketGroupCollection AllocatedBasketGroups { get;set;}

        public abstract void ProrataBasketAllocation(BasketGroup group, DateTime AUECLocalDate);
        public static BasketGroup GetAllocationDate(string BasketID, int typeOfAllocation)
        {
            BasketGroup basketGroup = new BasketGroup();
            basketGroup = BasketAllocationDBManager.GetBasketAllocation(BasketID, typeOfAllocation);
           return basketGroup;
        }
    }
}
