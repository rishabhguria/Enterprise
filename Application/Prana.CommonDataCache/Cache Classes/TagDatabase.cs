using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.Interfaces;
using System.Collections.Generic;
using System.Data;
namespace Prana.CommonDataCache
{
    /// <summary>
    /// Summary description for TagDatabase.
    /// </summary>
    public class TagDatabase
    {
        private static TagDatabase _tagDatabase = null;
        static Dictionary<string, string> _orderSide = new Dictionary<string, string>();
        static DataTable _basicOrderSide = new DataTable("BasicOrderSide");
        static DataTable _orderType = new DataTable("OrderType");
        static DataTable _TIF = new DataTable("TIF");
        static DataTable _orderStatus = new DataTable("OrderStatus");

        static DataTable _handlingInstruction = new DataTable("HandlingInstruction");
        static DataTable __orderSideWithID = new DataTable("OrderSide");
        static DataTable _executionInstruction = new DataTable("ExecutionInstruction");
        static DataTable _putOrCall = new DataTable("PutOrCall");
        static DataTable _openClose = new DataTable("OpenClose");

        static DataTable _CMTA = new DataTable("CMTA");
        static DataTable _GiveUp = new DataTable("GiveUp");
        private static IKeyValueDataManager _keyValueDataManager;
        static TagDatabase()
        {
            _tagDatabase = new TagDatabase();
            _keyValueDataManager = WindsorContainerManager.Container.Resolve<IKeyValueDataManager>();
            SetData();
        }

        public static TagDatabase GetInstance()
        {
            return _tagDatabase;
        }

        private static void SetData()
        {
            _orderSide = _keyValueDataManager.GetAllSides();
            __orderSideWithID = _keyValueDataManager.GetAllSidesWithID();
            _basicOrderSide = _keyValueDataManager.GetAllBasicSides();
            _orderType = _keyValueDataManager.GetAllOrderTypes();
            _handlingInstruction = _keyValueDataManager.GetAllHandlingInstruction();
            _executionInstruction = _keyValueDataManager.GetAllExecutionInstruction();
            _TIF = _keyValueDataManager.GetAllTIFs();

            _CMTA = _keyValueDataManager.GetAllCMTA();
            _GiveUp = _keyValueDataManager.GetAllGiveUp();


            #region OrderStatus
            _orderStatus.Columns.Add("OrderStatusID");
            _orderStatus.Columns.Add("OrderStatus");

            string[] row = new string[2];
            row[0] = CustomFIXConstants.ORDSTATUS_AlgoPreviousPendingReplace;
            row[1] = "Awaiting Previous Cancel";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = CustomFIXConstants.ORDSTATUS_Aborted;
            row[1] = "Aborted";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected;
            row[1] = "Previous Cancel Rejected";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_AcceptedForBidding;
            row[1] = "AcceptedForBidding";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Calculated;
            row[1] = "Calculated";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Cancelled;
            row[1] = "Cancelled";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_RollOver;
            row[1] = "RollOver";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_DoneForDay;
            row[1] = "DoneForDay";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Expired;
            row[1] = "Expired";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Filled;
            row[1] = "Filled";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_New;
            row[1] = "New";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_PartiallyFilled;
            row[1] = "PartiallyFilled";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_PendingCancel;
            row[1] = "PendingCancel";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_PendingRollOver;
            row[1] = "PendingRollOver";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_PendingNew;
            row[1] = "PendingNew";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_PendingReplace;
            row[1] = "PendingReplace";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Rejected;
            row[1] = "Rejected";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Replaced;
            row[1] = "Replaced";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Stopped;
            row[1] = "Stopped";
            _orderStatus.Rows.Add(row);

            row = new string[2];
            row[0] = FIXConstants.ORDSTATUS_Suspended;
            row[1] = "Suspended";
            _orderStatus.Rows.Add(row);
            #endregion

            //#region HandlingInstruction

            //_handlingInstruction.Columns.Add("HandlingInstructionID");
            //_handlingInstruction.Columns.Add("HandlingInstruction");

            //row = new string[2];
            //row[0] = FIXConstants.HANDLINST_AutoPrivateNoBroker;
            //row[1] = "AutoPrivateNoBroker";
            //_handlingInstruction.Rows.Add(row);

            //row = new string[2];
            //row[0] = FIXConstants.HANDLINST_AutoPublicBrokerOK;
            //row[1] = "AutoPublicBrokerOK";
            //_handlingInstruction.Rows.Add(row);

            //row = new string[2];
            //row[0] = FIXConstants.HANDLINST_Manual;
            //row[1] = "Manual";
            //_handlingInstruction.Rows.Add(row);

            //#endregion



            #region PutOrCall
            _putOrCall.Columns.Add("Value");
            _putOrCall.Columns.Add("Description");


            row = new string[2];
            row[0] = "0";
            row[1] = "PUT";
            _putOrCall.Rows.Add(row);

            row = new string[2];
            row[0] = "1";
            row[1] = "CALL";
            _putOrCall.Rows.Add(row);

            #endregion
            _openClose.Columns.Add("Value");
            _openClose.Columns.Add("Description");


            row = new string[2];
            row[0] = "O";
            row[1] = "Open";
            _openClose.Rows.Add(row);

            row = new string[2];
            row[0] = "C";
            row[1] = "Close";
            _openClose.Rows.Add(row);
        }

        public Dictionary<string, string> OrderSide
        {
            get { return _orderSide; }
        }

        public DataTable BasicOrderSide
        {
            get { return _basicOrderSide; }
        }

        public DataTable OrderType
        {
            get { return _orderType; }
        }

        public DataTable TIF
        {
            get { return _TIF; }
        }

        public DataTable OrderStatus
        {
            get { return _orderStatus; }
        }

        public DataTable HandlingInstruction
        {
            get { return _handlingInstruction; }
        }

        public DataTable OrderSideWithID
        {
            get { return __orderSideWithID; }
        }

        public DataTable ExecutionInstruction
        {
            get { return _executionInstruction; }
        }
        public DataTable PutOrCall
        {
            get { return _putOrCall; }
        }
        public DataTable OpenClose
        {
            get { return _openClose; }
        }
        public DataTable CMTA
        {
            get { return _CMTA; }
        }
        public DataTable GiveUp
        {
            get { return _GiveUp; }
        }

    }
}
