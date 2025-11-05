   
CREATE proc [dbo].[GetFundwisecashvalue]    
(    
-- @AllAUECDatesString varchar(max),    
@localdate datetime    
)    
    
AS    
Begin    
    
Declare @MarkPriceTable Table(Date datetime ,Symbol varchar(100),FinalMarkPrice numeric(8,4))                                                                
                                            
Insert Into @MarkPriceTable Select Date,Symbol,FinalMarkPrice from PM_DayMarkPrice where (datediff(d,PM_DayMarkPrice.Date,@localdate) = 0)                                                               
    
    
    
select * from T_Group left outer join @MarkPriceTable as MarkPriceTable on datediff(d,T_Group.SettlementDate,MarkPriceTable.Date) = 0    
     
select * from PM_CompanyFundCashCurrencyValue where (datediff(d,PM_CompanyFundCashCurrencyValue.Date,@localdate) = 0)   
    
    
--drop table #MarkPriceTable    
    
End    
                                         
             
