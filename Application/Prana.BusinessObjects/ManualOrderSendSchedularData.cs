using Prana.LogManager;
using System;
using System.Data;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ManualOrderSendSchedularData : EventArgs, IKeyable
    {
        /// <summary>
        /// The auec identifier
        /// </summary>
        private int _auecID = int.MinValue;
        /// <summary>
        /// Gets or sets the auecid.
        /// </summary>
        /// <value>
        /// The auecid.
        /// </value>
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        /// <summary>
        /// The send manual order trigger time
        /// </summary>
        private DateTime _sendManualOrderTriggerTime = DateTimeConstants.MinValue;
        /// <summary>
        /// Gets or sets the send manual order trigger time.
        /// </summary>
        /// <value>
        /// The send manual order trigger time.
        /// </value>
        public DateTime SendManualOrderTriggerTime
        {
            get { return _sendManualOrderTriggerTime; }
            set { _sendManualOrderTriggerTime = value; }
        }

        /// <summary>
        /// The last manual order run trigger time
        /// </summary>
        private DateTime _lastManualOrderRunTriggerTime = DateTimeConstants.MinValue;
        /// <summary>
        /// Gets or sets the last manual order run trigger time.
        /// </summary>
        /// <value>
        /// The last manual order run trigger time.
        /// </value>
        public DateTime LastManualOrderRunTriggerTime
        {
            get { return _lastManualOrderRunTriggerTime; }
            set { _lastManualOrderRunTriggerTime = value; }
        }

        /// <summary>
        /// The permit manual order send
        /// </summary>
        private bool _permitManualOrderSend;
        /// <summary>
        /// Gets or sets a value indicating whether [permit manual order send].
        /// </summary>
        /// <value>
        /// <c>true</c> if [permit manual order send]; otherwise, <c>false</c>.
        /// </value>
        public bool PermitManualOrderSend
        {
            get { return _permitManualOrderSend; }
            set { _permitManualOrderSend = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualOrderSendSchedularData"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        public ManualOrderSendSchedularData(DataRow row)
        {
            try
            {
                if (row.Table.Columns.Contains("AUECID"))
                    _auecID = Convert.ToInt32(row["AUECID"]);
                if (row.Table.Columns.Contains("IsSendManualOrderViaFIX"))
                    _permitManualOrderSend = Convert.ToBoolean(row["IsSendManualOrderViaFIX"]);
                if (row.Table.Columns.Contains("SendManualOrderTriggerTime") && !(row["SendManualOrderTriggerTime"] is DBNull))
                    _sendManualOrderTriggerTime = Convert.ToDateTime(row["SendManualOrderTriggerTime"]);
                if (row.Table.Columns.Contains("LastManualOrderRunTriggerTime") && !(row["LastManualOrderRunTriggerTime"] is DBNull))
                    _lastManualOrderRunTriggerTime = Convert.ToDateTime(row["LastManualOrderRunTriggerTime"]);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region IKeyable Members
        public string GetKey()
        {
            return _sendManualOrderTriggerTime.ToString();
        }

        public void Update(IKeyable item)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
