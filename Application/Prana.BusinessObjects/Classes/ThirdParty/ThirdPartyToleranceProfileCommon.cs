using System;

namespace Prana.BusinessObjects.Classes.ThirdParty
{
    [Serializable]
    public class ThirdPartyToleranceProfileCommon
    {
        private int _ThirdPartyBatchId; 
        private string _ThirdPartyName;
        private string _JobName;
        private string _ExecutingBroker;

        public int ThirdPartyBatchId
        {
            get { return _ThirdPartyBatchId; }
            set { _ThirdPartyBatchId = value; }
        }

        public string JobName
        {
            get { return _JobName; }
            set { _JobName = value; }
        }

        public string ExecutingBroker
        {
            get { return _ExecutingBroker; }
            set { _ExecutingBroker = value; }
        }

        public string ThirdPartyName
        {
            get { return _ThirdPartyName; }
            set { _ThirdPartyName = value; }
        }
    }
}
