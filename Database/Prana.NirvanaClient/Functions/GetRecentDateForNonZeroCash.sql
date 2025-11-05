
/* =============================================                              
 Author:  Sandeep Singh                 
 Create date: OCT 01,2010                            
 Description: Get Most Recent Date for Non Zero Cash.                      
 Usage                
 Select dbo.[GetRecentDateForNonZeroCash]('11-01-2010')                      
-- =============================================                              
*/                      
                      
CREATE function [dbo].[GetRecentDateForNonZeroCash]                 
(                
 @startDate datetime                
)                
returns datetime as                      
                      
Begin                   
     Declare @minDate datetime           
          
  Declare @count int          
               
     Set @count = (Select Count(*) from PM_companyFundCashCurrencyValue where BalanceType=1 And CashValueBase <> 0 and DateDiff(day,Date,@startDate) = 0)          
          
     Set @minDate =  (Select Min(Date) from PM_companyFundCashCurrencyValue where BalanceType=1)            
                
     Declare @nextBusDay datetime                      
     Declare @weekDay tinyInt                      
                      
     set @nextBusDay = @startDate                      
                    
                      
if( @count = 0 And DateDiff(day,@minDate,@startDate)>=0)                
Begin                
                
    While DateDiff(day,@minDate,@nextBusDay) != 0                      
      Begin                      
        Begin                      
        set @nextBusDay = dateAdd(d,-1,@nextBusDay)  -- first get the raw previous day                      
        End                      
          SET @weekDay =((@@dateFirst+datePart(dw,@nextBusDay)-2) % 7) + 1                       
       -- Statement above will always return 1 for Monday                      
       -- and 6 and 7 for Sat and Sun respectively                      
                        
        Begin                      
         if @weekDay = 6 set @nextBusDay = @nextBusDay - 1  -- 6 is Saturday so jump to Monday                      
         if @weekDay = 7 set @nextBusDay = @nextBusDay - 2  -- 7 is Sunday so jump to Friday                      
        End                      
                              
       --Check if the Date is in the Cash Table                    
          If EXISTS(Select Date from PM_companyFundCashCurrencyValue where BalanceType=1                 
          And CashValueBase <> 0 and DateDiff(day,Date,@nextBusDay) = 0)                     
          Begin                      
           set @minDate = @nextBusDay                       
          End                 
                           
     End                 
End           
 -- outer If                
     return  @nextBusDay                      
End 


