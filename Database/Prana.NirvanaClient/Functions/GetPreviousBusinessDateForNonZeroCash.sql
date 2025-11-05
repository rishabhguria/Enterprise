          
CREATE function [dbo].[GetPreviousBusinessDateForNonZeroCash]     
(    
 @startDate datetime    
)    
returns datetime as          
          
Begin       
     Declare @minDate datetime    
  Set @minDate=  (Select Min(Date) from T_DayEndBalances where BalanceType=1 )     
     Declare @nextBusDay datetime          
     Declare @weekDay tinyInt          
          
     set @nextBusDay = @startDate          
        
          
if(DateDiff(day,@minDate,@StartDate))>=0    
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
          If EXISTS(Select Date from T_DayEndBalances where BalanceType=1    
   AND CashValueBase <> 0 and DateDiff(day,Date,@nextBusDay) = 0)         
          Begin          
         set @minDate = @nextBusDay           
          End     
               
          End     
End  -- outer If    
     return  @nextBusDay          
End 

