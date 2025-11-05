using Prana.BusinessObjects.AppConstants;
using System;
//using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class GroupingCriteria
    {
        private bool _isGroupbyAccount = false;
        public bool IsGroupByAccount
        {
            get { return _isGroupbyAccount; }
            set { _isGroupbyAccount = value; }
        }

        private bool _isGroupbySide = false;
        public bool IsGroupBySide
        {
            get { return _isGroupbySide; }
            set { _isGroupbySide = value; }
        }

        private bool _isGroupbyBroker = false;
        public bool IsGroupByBroker
        {
            get { return _isGroupbyBroker; }
            set { _isGroupbyBroker = value; }
        }

        private bool _isGroupbySymbol = false;
        public bool IsGroupBySymbol
        {
            get { return _isGroupbySymbol; }
            set { _isGroupbySymbol = value; }
        }

        private bool _isGroupbyMasterFund = false;
        public bool IsGroupbyMasterFund
        {
            get { return _isGroupbyMasterFund; }
            set { _isGroupbyMasterFund = value; }
        }


        private SerializableDictionary<int, SymbologyCodesForRecon> _dictGroupingSymbology = new SerializableDictionary<int, SymbologyCodesForRecon>();

        public SerializableDictionary<int, SymbologyCodesForRecon> DictGroupingSymbology
        {
            get { return _dictGroupingSymbology; }
            set { _dictGroupingSymbology = value; }
        }

        public SerializableDictionary<string, bool> _dictGroupingCriteria = new SerializableDictionary<string, bool>();
        public SerializableDictionary<string, bool> DictGroupingCriteria
        {
            get { return _dictGroupingCriteria; }
            set { _dictGroupingCriteria = value; }
        }
    }
}
