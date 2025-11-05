/*  
Author : Sumit Kakra  
Date   : 8-8-2008  
Description : It takes the AUECLocaldate and auecid and returns whether this is a business day (1) or not (0)  
*/  
CREATE FUNCTION [dbo].[IsBusinessDay] (  
 @date datetime,  
 @auecid int  
)  
Returns Bit AS    
Begin  
  
 declare @weekDay int  
 declare @isBusinessDay bit  
 declare @nextBusinessDay datetime  
 set @isBusinessDay = 0

 Select @nextBusinessDay = dbo.AdjustBusinessDays(dateadd(d,-1,@date),1,@AUECID)

 If @nextBusinessDay = @date 
 Begin
	Set @isBusinessDay = 1
 End
 return @isBusinessDay   
End  
