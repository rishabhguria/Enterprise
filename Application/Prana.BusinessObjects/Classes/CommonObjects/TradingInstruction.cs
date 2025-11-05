using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;
using System.Text;


namespace Prana.BusinessObjects
{
    [Serializable()]
    public class TradingInstruction
    {

        public TradingInstruction()
        {
        }

        public TradingInstruction(string message)
        {
            string[] str = message.Split(',');
            this._msgType = str[0];
            this._TargetUserID = int.Parse(str[1]);
            this._tradingAccID = int.Parse(str[2]);
            this._clOrderID = str[3];
            this._symbol = str[4];
            this._side = str[5];
            this._quantity = Double.Parse(str[6]);
            this._instructions = str[7];
            this._originatorUserID = int.Parse(str[8]);
            this._userID = int.Parse(str[9]);
            this._deskID = str[10];
            this._status = (TradingInstructionEnums.TradingInstStatus)int.Parse(str[11]);
            this._clientName = str[12];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string str = string.Empty;
            sb.Append(this._msgType);
            sb.Append(Seperators.SEPERATOR_1);//0
            sb.Append(this._TargetUserID.ToString());
            sb.Append(Seperators.SEPERATOR_1);//1
            sb.Append(this._tradingAccID.ToString());
            sb.Append(Seperators.SEPERATOR_1);//2
            sb.Append(this._clOrderID);
            sb.Append(Seperators.SEPERATOR_1);//3
            sb.Append(this._symbol);
            sb.Append(Seperators.SEPERATOR_1);//4
            sb.Append(this._side);
            sb.Append(Seperators.SEPERATOR_1);//5
            sb.Append(this._quantity.ToString());
            sb.Append(Seperators.SEPERATOR_1);//6
            sb.Append(this._instructions);
            sb.Append(Seperators.SEPERATOR_1);//7
            sb.Append(this._originatorUserID.ToString());
            sb.Append(Seperators.SEPERATOR_1);//8
            sb.Append(this._userID.ToString());
            sb.Append(Seperators.SEPERATOR_1);//9
            sb.Append(this._deskID.ToString());
            sb.Append(Seperators.SEPERATOR_1);//10
            sb.Append(((int)this._status).ToString());
            sb.Append(Seperators.SEPERATOR_1);//11
            sb.Append(this._clientName);

            str = sb.ToString();
            return str;
        }

        private string _clOrderID = string.Empty;
        [Browsable(false)]
        public string ClOrderID
        {
            get { return _clOrderID; }
            set { _clOrderID = value; }
        }

        private string _deskID = Guid.NewGuid().ToString();
        /// <summary>
        /// Gets or sets the desk ID.
        /// This is the GUID assigned to each instruction
        /// </summary>
        /// <value>The desk ID.</value>
        [Browsable(false)]
        public string DeskID
        {
            get
            {
                return _deskID;
            }
            set { _deskID = value; }
        }

        private string _msgType = string.Empty;
        [Browsable(false)]
        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }
        private string _side;

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }




        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _quantity;

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private string _instructions;

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>The instructions.</value>
        public string Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }
        int _userID = int.MinValue;
        [Browsable(false)]
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }

        }


        int _originatorUserID = int.MinValue;

        /// <summary>
        /// Used to mention the UserID of the sender in case this is an internal message. For e.g the UserID of the PM or Account Manager
        /// </summary>
        [Browsable(false)]
        public int SenderUserID
        {
            get { return _originatorUserID; }
            set { _originatorUserID = value; }

        }

        private string _clientName;
        /// <summary>
        /// To be used to keep the OnBehalfOfCompID (Sender Comp ID of the client that sends the trading instruction)
        /// </summary>
        [Browsable(false)]
        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }


        int _TargetUserID = int.MinValue;
        [Browsable(false)]
        public int TargetUserID
        {
            get { return _TargetUserID; }
            set { _TargetUserID = value; }

        }

        int _tradingAccID = int.MinValue;
        [Browsable(false)]
        public int TradingAccID
        {
            get { return _tradingAccID; }
            set { _tradingAccID = value; }

        }

        private string _tradingAccount;

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>The instructions.</value>
        [Browsable(false)]
        public string TradingAcc
        {
            get { return _tradingAccount; }
            set { _tradingAccount = value; }
        }

        private string _user;

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>The instructions.</value>
        [Browsable(false)]
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }
        private TradingInstructionEnums.TradingInstStatus _status = TradingInstructionEnums.TradingInstStatus.ActionPending;
        public TradingInstructionEnums.TradingInstStatus Status
        {
            get { return _status; }
            set { _status = value; }


        }
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }


        private AssetCategory _assetCategory = AssetCategory.None;
        [Browsable(false)]
        public AssetCategory AssetCategory
        {
            get { return _assetCategory; }
            set { _assetCategory = value; }
        }

        private Underlying _underlying = Underlying.None;
        [Browsable(false)]
        public Underlying UnderLying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }


        int _exchangeID = int.MinValue;
        [Browsable(false)]
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        int _auecID = int.MinValue;
        [Browsable(false)]
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

    }



}