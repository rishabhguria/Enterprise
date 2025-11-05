using System.ComponentModel;

namespace Prana.BusinessObjects.Classes.ThirdParty.Tables
{
    /// <summary>
    /// Summary description for Event Logs.
    /// </summary>
    public class ThirdPartyBatchEventData
    {
        private string _transmissionTime;
        private string _msgDiscription;
        private string _msgDirection;
        private string _allocID;
        private string _fixMsg;

        /// <summary>
        /// Gets or sets the Transmission Time
        /// </summary>
        public string TransmissionTime
        {
            get { return _transmissionTime; }
            set { _transmissionTime = value; }
        }

        /// <summary>
        /// Gets or sets the MsgDescription
        /// </summary>
        public string MsgDescription
        {
            get { return _msgDiscription; }
            set { _msgDiscription = value; }
        }

        /// <summary>
        /// Gets or sets the MsgDirection
        /// </summary>
        public string MsgDirection
        {
            get { return _msgDirection; }
            set { _msgDirection = value; }
        }

        /// <summary>
        /// Gets or sets the AllocID
        /// </summary>
        public string AllocID
        {
            get { return _allocID; }
            set { _allocID = value; }
        }

        /// <summary>
        /// Gets or sets the Fix Msg
        /// </summary>
        [Browsable(false)]
        public string FixMsg
        {
            get { return _fixMsg; }
            set { _fixMsg = value; }
        }
    }
}
