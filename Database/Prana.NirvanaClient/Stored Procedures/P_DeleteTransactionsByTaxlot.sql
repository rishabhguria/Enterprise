 
/*************************************************  
Author : Bharat Raturi 
Execution Method : exec P_DeleteTransactions null
*************************************************/  
  
CREATE procedure [dbo].[P_DeleteTransactionsByTaxlot]    
(      
	@TaxlotID varchar(100)
)      
As    
BEGIN             
select ActivityID into #FkIDTable from T_AllActivity where FKID=@TaxlotID
delete j FROM T_Journal j inner join #FkIDTable f on j.ActivityId_FK=f.ActivityID
drop table #FkIDTable
--select UniqueKey, ActivityID, ActivityTypeId_FK, FKID, BalanceType, Symbol, FundID,TradeDate,     
--SettlementDate, CurrencyID, LeadCurrencyID, VsCurrencyID, ClosedQty, Amount, Commission, SoftCommission, OtherBrokerFees,    
--ClearingBrokerFee, StampDuty, TransactionLevy, ClearingFee, TaxOnCommissions, MiscFees, SecFee, OccFee, OrfFee, FXRate,     
--FXConversionMethodOperator,ActivitySource,TransactionSource, ActivityNumber, Description, Subactivity, SideMultiplier  
--FROM T_AllActivity
--where FKID=@TaxlotID
END
