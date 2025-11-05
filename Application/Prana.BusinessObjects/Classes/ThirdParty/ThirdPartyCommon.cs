using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyCommon
    {
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> _taxLotWithStateDict = new Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState>();
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> _taxLotIgnoreStateDict = new Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState>();

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<Int64, int> pbUniqueIDDict = new Dictionary<Int64, int>();
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, string> _deletedToIgnoreDict = new Dictionary<string, string>();

        [Browsable(false)]
        public Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> TaxLotWithStateDict
        { get { return _taxLotWithStateDict; } set { _taxLotWithStateDict = value; } }

        [Browsable(false)]
        public Dictionary<string, Prana.BusinessObjects.AppConstants.PranaTaxLotState> TaxLotIgnoreStateDict
        { get { return _taxLotIgnoreStateDict; } set { _taxLotIgnoreStateDict = value; } }

        [Browsable(false)]
        public Dictionary<string, string> DeletedToIgnoreDict
        { get { return _deletedToIgnoreDict; } set { _deletedToIgnoreDict = value; } }

        [Browsable(false)]
        public Dictionary<Int64, int> PbUniqueIDDict { get { return pbUniqueIDDict; } set { pbUniqueIDDict = value; } }
    }
}
