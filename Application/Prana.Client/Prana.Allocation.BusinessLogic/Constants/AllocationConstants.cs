using System;

namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class AllocationConstants
	{
        public enum SelectedTab
        {
            UnAllocatedOrder,
            GroupedOrder,
            AllocatedOrder,
            UnAllocatedBasket,
            GroupedBasket,
            AllocatedBasket
        }
		  
        
		public  const string UNALLOCATED_GRID="UnAllocated";
		public const string GROUPED_GRID="Grouped";
		public const string ALLOCATED_GRID="Allocated";
		public AllocationConstants()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
