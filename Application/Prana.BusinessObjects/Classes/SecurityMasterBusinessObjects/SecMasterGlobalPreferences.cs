using System;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterGlobalPreferences
    {
        private bool _useCutOffTime;

        public bool UseCutOffTime
        {
            get { return _useCutOffTime; }
            set { _useCutOffTime = value; }
        }

        //Auto approve security when security source is E-signal (any external Source)
        // when client do not want Approval Sec feature.
        //If false then we must approve security on symbollook UI if ForceToApproveSec preference TRUE. 
        private bool _isAutoApproved;
        public bool IsAutoApproved
        {
            get { return _isAutoApproved; }
            set { _isAutoApproved = value; }
        }

        //force to approve on adding sec manualy or when trading from TT or Importing positions in DB
        // False when client not want this approval sec feature.
        // private bool _forceToApproveSec;
        public bool ForceToApproveSec
        {
            get { return _isAutoApproved; }
            set { _isAutoApproved = value; }
        }


    }
}
