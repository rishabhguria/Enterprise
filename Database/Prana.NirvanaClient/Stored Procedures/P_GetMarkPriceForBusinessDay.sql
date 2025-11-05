-- =============================================                    
-- Author:  Sandeep Singh                     
-- Create date: April 23,2010                    
-- Description: This SP returns Mark Prices for all symbols for a given date.                    
                    
--Exec [P_GetMarkPriceForBusinessDay] '07-25-2009'                    
-- =============================================                    
CREATE Procedure [P_GetMarkPriceForBusinessDay]                    
(                     
 @MarkPriceDate datetime                    
)                    
AS                                      
BEGIN      
          
Create Table #AUECYesterDates                          
  (                            
   AUECID INT,                            
   YESTERDAYBIZDATE DATETIME                            
  )                           
                                
INSERT INTO #AUECYesterDates                            
  Select Distinct  V_SymbolAuec.AUECID, dbo.AdjustBusinessDays(@MarkPriceDate,-1, V_SymbolAuec.AUECID)               
  from V_SymbolAuec            
          
--INSERT INTO @DayMarkPrices           
Select             
  FinalMarkPrice ,            
  PM_DayMarkPrice.Symbol          
      From PM_DayMarkPrice               
       INNER JOIN V_SymbolAuec ON PM_DayMarkPrice.Symbol = V_SymbolAuec.Symbol              
       INNER JOIN #AUECYesterDates AUECYesterDates ON AUECYesterDates.AUECID = V_SymbolAuec.AUECID               
       Where DATEDIFF(d,PM_DayMarkPrice.Date,AUECYesterDates.YESTERDAYBIZDATE) = 0                         
                    
--RETURN      
    
Drop Table #AUECYesterDates                   
END 