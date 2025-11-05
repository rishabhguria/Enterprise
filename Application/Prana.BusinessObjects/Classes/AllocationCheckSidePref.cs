using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes
{
    [Serializable]
    public class AllocationCheckSidePref
    {
        /// <summary>
        /// if Do check side then state will be checked for order side.
        /// </summary>
        public bool DoCheckSideSystem { get; set; }

        /// <summary>
        /// Disable check side preference for each level
        /// </summary>
        public Dictionary<OrderFilterLevels, List<int>> DisableCheckSidePref = new Dictionary<OrderFilterLevels, List<int>>();


    }
}
