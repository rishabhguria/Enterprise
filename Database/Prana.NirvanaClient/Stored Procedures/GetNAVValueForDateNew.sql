CREATE proc GetNAVValueForDateNew   
(      
  @date datetime                        
)      
as      
select distinct FundID,NAVValue from PM_NAVValue where Date = @date      
