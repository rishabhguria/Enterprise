using Prana.LogManager;
using System;
using System.Data;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class TradeAuditEntry
    {
        private DateTime _aUECLocalDate = DateTimeConstants.MinValue;
        public DateTime AUECLocalDate
        {
            get { return _aUECLocalDate; }
            set { _aUECLocalDate = value; }
        }

        private DateTime _originalDate = DateTimeConstants.MinValue;
        public DateTime OriginalDate
        {
            get { return _originalDate; }
            set { _originalDate = value; }
        }

        private string _groupID;
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private string _taxlotID = string.Empty;
        public string TaxLotID
        {
            get { return _taxlotID; }
            set { _taxlotID = value; }
        }

        private string _parentClOrderID = string.Empty;
        public string ParentClOrderID
        {
            get { return _parentClOrderID; }
            set { _parentClOrderID = value; }
        }

        private string _clOrderID = string.Empty;
        public string ClOrderID
        {
            get { return _clOrderID; }
            set { _clOrderID = value; }
        }

        private string _level1AllocationID;

        public string Level1AllocationID
        {
            get { return _level1AllocationID; }
            set { _level1AllocationID = value; }
        }

        private string _taxLotClosingId;
        public string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }

        private Prana.BusinessObjects.TradeAuditActionType.ActionType _action;
        public Prana.BusinessObjects.TradeAuditActionType.ActionType Action
        {
            get { return _action; }
            set { _action = value; }
        }

        private string _originalValue;
        public string OriginalValue
        {
            get { return _originalValue; }
            set { _originalValue = value; }
        }

        private string _newValue;
        public string NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        private int _companyUserId;
        public int CompanyUserId
        {
            get { return _companyUserId; }
            set { _companyUserId = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private int? _level1ID;
        public int? Level1ID
        {
            get { return _level1ID; }
            set { _level1ID = value; }
        }

        private string _orderSideTagValue = string.Empty;
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        public static bool IsActionDeleted(TradeAuditEntry tr)
        {
            if (tr.Action == TradeAuditActionType.ActionType.DELETE)
                return true;
            else
                return false;
        }

        public static bool IsActionEdited(TradeAuditEntry tr)
        {
            if ((int)tr.Action > 5)
                return true;
            else
                return false;
        }

        private Prana.BusinessObjects.TradeAuditActionType.ActionSource _source;
        public Prana.BusinessObjects.TradeAuditActionType.ActionSource Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        /// Assigns the datarow of table T_tradeAudit to the class
        /// </summary>
        /// <param name="dr"></param>
        public void AssignDataRowToEntry(DataRow dr)
        {
            try
            {
                this.Action = (TradeAuditActionType.ActionType)dr["Action"];
                this.OriginalValue = dr["OriginalValue"].ToString();
                this.NewValue = dr["NewValue"].ToString();
                this.AUECLocalDate = DateTime.Parse(dr["ActionDate"].ToString());
                this.Comment = dr["Comment"].ToString();
                this.CompanyUserId = (int)dr["CompanyUserId"];
                this.GroupID = dr["GroupId"].ToString();
                this.TaxLotClosingId = dr["TaxlotClosingId"].ToString();
                this.TaxLotID = dr["TaxlotId"].ToString();
                this.Symbol = dr["Symbol"].ToString();
                if (!String.IsNullOrEmpty(dr["FundID"].ToString()))
                {
                    this.Level1ID = (int)dr["FundID"];
                }
                else
                {
                    this.Level1ID = (int?)null;
                }
                this.OrderSideTagValue = dr["OrderSideTagValue"].ToString();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
