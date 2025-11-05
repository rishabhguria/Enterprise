  
CREATE Procedure [dbo].[P_SaveTransferTradeRules]  
(  
 @isTIFChange bit,  
 @isTradingAccChange bit,  
 @isFundChange bit,  
 @isStrategyChange bit,  
 @isHandlingInstrChange bit,  
 @isVenueCPChange bit,  
 @isAllowAllUserToCancelReplaceRemove bit,  
 @isAllowUserToChangeOrderType bit,  
@isExecutionInstrChange bit,  
@companyId int,  
@isAllowAllUserToTransferTrade bit,  
@isAllowAllUserToGenerateSub bit,
@isApplyLimitRulesForReplacingStagedOrders bit,  
@isApplyLimitRulesForReplacingOtherOrders bit,
@isApplyLimitRulesForReplacingSubOrders bit,
@isAllowRestrictedSecuritiesList bit,
@isAllowAllowedSecuritiesList bit,
@MasterUsersIDs VARCHAR(MAX),
@IsDefaultOrderTypeLimitForMultiDay bit
)  
  
as  
Declare @total int   
Set @total = 0  
  
Select @total = Count(*)  
From T_TransferTradeRules   
Where CompanyID = @companyID  
  
if(@total > 0)  
begin   
Update T_TransferTradeRules  
set  
IsTIFChange = @isTIFChange,  
IsTradingAccChange = @isTradingAccChange,  
IsFundChange = @isFundChange,  
IsStrategyChange = @isStrategyChange,  
IsHandlingInstrChange = @isHandlingInstrChange,  
IsVenueCPChange = @isVenueCPChange,  
IsAllowAllUserToCancelReplaceRemove = @isAllowAllUserToCancelReplaceRemove,  
IsAllowAllUserToChangeOrderType = @isAllowUserToChangeOrderType,  
IsExecutionInstrChange = @isExecutionInstrChange,  
IsAllowAllUserToTransferTrade = @isAllowAllUserToTransferTrade,  
IsAllowAllUserToGenerateSub = @isAllowAllUserToGenerateSub ,
IsApplyLimitRulesForReplacingStagedOrders = @isApplyLimitRulesForReplacingStagedOrders,
IsApplyLimitRulesForReplacingOtherOrders = @isApplyLimitRulesForReplacingOtherOrders,
IsApplyLimitRulesForReplacingSubOrders = @isApplyLimitRulesForReplacingSubOrders,
IsAllowRestrictedSecuritiesList = @isAllowRestrictedSecuritiesList,
IsAllowAllowedSecuritiesList = @isAllowAllowedSecuritiesList,
MasterUsersIDs = @MasterUsersIDs,
IsDefaultOrderTypeLimitForMultiDay=@IsDefaultOrderTypeLimitForMultiDay
 Where CompanyID = @companyID   
   
End  
else  
begin  
INSERT T_TransferTradeRules(IsTIFChange, IsTradingAccChange, IsFundChange,IsStrategyChange,IsHandlingInstrChange,IsVenueCPChange,IsAllowAllUserToCancelReplaceRemove,IsAllowAllUserToChangeOrderType,IsExecutionInstrChange,CompanyID,
IsAllowAllUserToTransferTrade,IsAllowAllUserToGenerateSub, IsApplyLimitRulesForReplacingStagedOrders,IsApplyLimitRulesForReplacingOtherOrders,IsApplyLimitRulesForReplacingSubOrders,IsAllowRestrictedSecuritiesList,IsAllowAllowedSecuritiesList,MasterUsersIDs,IsDefaultOrderTypeLimitForMultiDay)  
Values(@isTIFChange, @isTradingAccChange, @isFundChange,@isStrategyChange,@isHandlingInstrChange,@isVenueCPChange,@isAllowAllUserToCancelReplaceRemove,@isAllowUserToChangeOrderType,@isExecutionInstrChange,@companyID,@isAllowAllUserToTransferTrade,@isAllowAllUserToGenerateSub,@isApplyLimitRulesForReplacingStagedOrders,@isApplyLimitRulesForReplacingOtherOrders,@isApplyLimitRulesForReplacingSubOrders,@isAllowRestrictedSecuritiesList,@isAllowAllowedSecuritiesList,@MasterUsersIDs,@IsDefaultOrderTypeLimitForMultiDay)           
end