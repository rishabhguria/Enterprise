using Prana.BusinessObjects.AppConstants;
using System.Xml.Serialization;

namespace Prana.Allocation.Client.Definitions
{
    /// <summary>
    /// General Rules Properties
    /// </summary>
    public class GeneralRules
    {
        #region Members

        /// <summary>
        /// The clear allocation fund control number
        /// </summary>
        [XmlAttribute("ClearAllocationFundControlNumber")]
        public bool ClearAllocationFundControlNumber = false;

        /// <summary>
        /// The allocation methodology revert to default
        /// </summary>
        [XmlAttribute("AllocationMethodologyRevertToDefault")]
        public bool AllocationMethodologyRevertToDefault = false;

        /// <summary>
        /// The keep accounts grid fixed
        /// </summary>
        [XmlAttribute("KeepAccountsGridFixed")]
        public bool KeepAccountsGridFixed = false;


        /// <summary>
        /// The allocate extra share to selected fund
        /// </summary>
        [XmlAttribute("AllocateExtraShareToSelectedFund")]
        public bool AllocateExtraShareToSelectedFund = false;

        /// <summary>
        /// The include savewt state
        /// </summary>
        [XmlAttribute("IncludeSavewtState")]
        public bool IncludeSavewtState = false;

        /// <summary>
        /// The include savewtout state
        /// </summary>
        [XmlAttribute("IncludeSavewtoutState")]
        public bool IncludeSavewtoutState = false;

        /// <summary>
        /// The allocation preference type
        /// </summary>
        [XmlAttribute("AllocationPrefType")]
        public AllocationPreferenceType AllocationPrefType;

        /// <summary>
        /// The is allocation by PTT
        /// </summary>
        [XmlAttribute("IsAllocationByPST")]
        public bool IsAllocationByPST = false;

        #endregion Members
    }
}
