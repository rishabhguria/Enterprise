-- =============================================              
-- Author:  Sandeep Singh               
-- Create date: 5th Nov, 2008              
-- Description: This function returns a table with Mark Prices for all symbols for a given date.              
              
--select * from dbo.[GetMarkPriceForBusinessDay]('7/9/2015')              
-- =============================================              
CREATE FUNCTION [dbo].[GetMarkPriceForBusinessDay]              
(               
 @MarkPriceDate datetime              
)              
RETURNS @DayMarkPrices TABLE                                 
(                              
 FinalMarkPrice float ,    
 Symbol varchar(50)     
)    
      
                            
AS                                
BEGIN      
    
Declare @AUECYesterDates TABLE                    
  (                      
   AUECID INT,                      
   YESTERDAYBIZDATE DATETIME                      
  )                     
                          
INSERT INTO @AUECYesterDates                      
  Select Distinct  V_SymbolAuec.AUECID, dbo.AdjustBusinessDays(@MarkPriceDate,-1, V_SymbolAuec.AUECID)         
  from V_SymbolAuec      
    
INSERT INTO @DayMarkPrices     
Select       
  FinalMarkPrice ,      
  PM_DayMarkPrice.Symbol    
      From PM_DayMarkPrice         
       INNER JOIN V_SymbolAuec ON PM_DayMarkPrice.Symbol = V_SymbolAuec.Symbol        
       INNER JOIN @AUECYesterDates AUECYesterDates ON AUECYesterDates.AUECID = V_SymbolAuec.AUECID         
       Where DATEDIFF(d,PM_DayMarkPrice.Date,AUECYesterDates.YESTERDAYBIZDATE) = 0 
	   and FundID = 0                  
              
RETURN               
END 

