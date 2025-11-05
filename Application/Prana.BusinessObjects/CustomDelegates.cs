using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Reflection;

namespace Prana.BusinessObjects
{
    //For managed rules - shagoon
    //Updated Handlers AlgoValidTradeHandler, OrderToUIThreadHandler, PropertyChangeHandler, FilterConditionChangedHandler, CommissionFormInitializeHandler
    public delegate void ResizedHandler(Object sender, ResizeHandlerEventArgs e);
    //public delegate void AlgoValidTradeHandler(OrderSingle validAlgoFixOrder);
    public delegate void AlgoValidTradeHandler(object sender, EventArgs<OrderSingle> e);
    //public delegate void OrderToUIThreadHandler(OrderSingle order);
    public delegate void OrderToUIThreadHandler(object sender, EventArgs<OrderSingle> e);
    //public delegate bool PropertyChangeHandler(object sender,PropertyInfo property);
    public delegate bool PropertyChangeHandler(object sender, EventArgs<PropertyInfo> e);
    //public delegate void OrderUpdateHandler(object sender, Prana.BusinessObjects.CancelAmend.Order previousOrder);
    //public delegate void AccountGroupUpdateHandler(object sender, Prana.BusinessObjects.CancelAmend.AccountGroup previousAccountGroup);
    //public delegate void FilterConditionChangedHandler(BizDataFilter filterConditions);
    public delegate bool FilterConditionChangedHandler(object sender, EventArgs<BizDataFilter> e);
    //public delegate void CommissionFormInitializeHandler(DateTime selectedDate);
    public delegate bool CommissionFormInitializeHandler(object sender, EventArgs<DateTime> e);
    public delegate void UIThreadMarshellerPublish(MessageData data, string topic);
    public delegate void ParameterizedMethodHandler(TaxLot taxlot, ApplicationConstants.TaxLotState state, string topicName);


    /// <summary>
    /// Methodinvoker is in windows.forms.ui, but we need this delegate without the reference of windows.forms.ui.
    /// Hence put.
    /// </summary>
    public delegate void MethodInvokerVoid();
    /// <summary>
    /// Ilist uses this
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate int AddHandler(object value);

    /// <summary>
    /// Ilist remove uses this
    /// </summary>
    /// <param name="value"></param>
    public delegate void RemoveHandler(object value);
    public delegate void StringHandler(object sender, EventArgs<string> e);
    public delegate void TwoStringHandler(string value1, string value2);
    public delegate void ObjectHandler(object obj);
    public delegate void ObjectsHandler(object[] obj);
    public delegate void DrawControlBySecMasterData(SecMasterBaseObj secMasterObj);


    public delegate void MessageEventHandler(PranaMessage pranaMsg);
    public delegate void ExceptionEventHandler(string message);
    public delegate void ConnectionEventHandler(FixPartyDetails fixConnDetails);
    //public delegate void PranaMessageReceivedHandler(object sender, PranaMessage message);
    public delegate void PranaQueueMessageHandler(QueueMessage queueMsg);
    //public delegate void DashBoardTableUpdatedHandler(DataTable updatedTable, List<int> accountIDs);
    //public delegate void BoolHandler(bool b);
    public delegate void UserPreferencesUpdated(UserPreferencesEventArgs updatedPrefs);
    public delegate void TwoStringOneIntHandler(string value1, string value2, int value3);
    //public delegate void OptionChainHandler(Dictionary<string, List<OptionStaticData>> dictOfOptionData);

    /// <summary>
    /// Denotes a change in Allocation data in binding collection
    /// </summary>
    /// <param name="sender">Change initiator for allocation data</param>
    /// <param name="isStarting">True for starting, False for completed</param>
    public delegate void AllocationDataChangeHandler(object sender, bool isStarting);

    public delegate void LoadCloseTradeUIFromAllocationHandler(object sender, AllocationGroup group);
    public delegate void LoadSymbolLookUpUIFromAllocationHandler(object sender, string symbol);

}
