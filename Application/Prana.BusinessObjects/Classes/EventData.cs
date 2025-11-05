using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.Classes.ThirdParty.Tables;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    [KnownType(typeof(Position))]
    [KnownType(typeof(List<Position>))]
    [KnownType(typeof(List<IFilterable>))]
    [KnownType(typeof(List<TaxLot>))]
    [KnownType(typeof(TaxLot))]
    [KnownType(typeof(AllocationGroup))]
    [KnownType(typeof(List<TransactionEntry>))]
    [KnownType(typeof(List<CashActivity>))]
    [KnownType(typeof(CashActivity))]
    [KnownType(typeof(TransactionEntry))]
    [KnownType(typeof(Transaction))]
    [KnownType(typeof(List<Transaction>))]
    [KnownType(typeof(CompanyAccountCashCurrencyValue))]
    [KnownType(typeof(SecMasterEquityObj))]
    [KnownType(typeof(SecMasterFixedIncome))]
    [KnownType(typeof(SecMasterFutObj))]
    [KnownType(typeof(SecMasterFXForwardObj))]
    [KnownType(typeof(SecMasterFxObj))]
    [KnownType(typeof(SecMasterIndexObj))]
    [KnownType(typeof(SecMasterOptObj))]
    [KnownType(typeof(UserOptModelInput))]
    [KnownType(typeof(List<UserOptModelInput>))]
    [KnownType(typeof(LiveFeedPreferences))]
    [KnownType(typeof(OMIPublishType))]
    [KnownType(typeof(ProgressInfo))]
    [KnownType(typeof(string))]
    [KnownType(typeof(List<string>))]
    [KnownType(typeof(List<bool>))]
    [KnownType(typeof(int))]
    [KnownType(typeof(ResponseObj))]
    [KnownType(typeof(List<StepAnalysisResponse>))]
    [KnownType(typeof(StageOrderRemovalData))]
    [KnownType(typeof(SubOrderRemovalData))]
    [KnownType(typeof(ModelPortfolioDto))]
    [KnownType(typeof(CustomGroupDto))]
    [KnownType(typeof(FixPartyDetails))]
    [KnownType(typeof(List<DMServiceData>))]
    [KnownType(typeof(SymbolData))]
    [KnownType(typeof(OrderSingle))]
    [KnownType(typeof(PranaMessage))]
    [KnownType(typeof(ThirdPartyBatch))]
    [KnownType(typeof(ThirdPartyCommon))]
    [KnownType(typeof(ThirdPartyFtp))]
    [KnownType(typeof(ThirdPartyGnuPG))]
    [KnownType(typeof(ThirdPartyUserDefinedFormat))]
    [KnownType(typeof(ThirdPartyEmail))]
    [KnownType(typeof(ThirdPartyFlatFileSaveDetail))]
    [KnownType(typeof(ThirdPartyFileFormat))]
    [KnownType(typeof(MessageEventArgs))]
    [KnownType(typeof(PromptEventArgs))]
    [KnownType(typeof(StatusEventArgs))]
    [KnownType(typeof(CompanyUser))]
    [KnownType(typeof(ThirdPartyAllocationMatchDetails))]
    [KnownType(typeof(ApplicationConstants.AllocationMatchStatus))]
    [KnownType(typeof(ThirdPartyBlockLevelDetails))]
    [KnownType(typeof(ThirdPartyAllocationDetailComparison))]
    [KnownType(typeof(ThirdPartyAllocationLevelDetails))]
    [Serializable]
    public class MessageData
    {
        private string _topicName;
        private IList _eventData;
        private int _userId;
        private bool _isRemoveManualExecution;

        public string TopicName { get { return _topicName; } set { _topicName = value; } }
        public IList EventData { get { return _eventData; } set { _eventData = value; } }
        public int UserId { get { return _userId; } set { _userId = value; } }
        // Added to handle Compliance publish use cases
        public bool IsRemoveManualExecution { get { return _isRemoveManualExecution; } set { _isRemoveManualExecution = value; } }
    }
}