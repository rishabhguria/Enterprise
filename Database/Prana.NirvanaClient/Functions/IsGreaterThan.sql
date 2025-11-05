-- =============================================              
-- Author:  Sandeep              
-- Create date: 23 Sept 2010   
-- Select dbo.[IsGreaterThan]('2010-08-16 09:34:44.000',9,30)          
-- =============================================              
CREATE FUNCTION [dbo].[IsGreaterThan]   
(              
 @Date datetime,          
 @Hour int,         
 @Min int   
)              
RETURNS  Int              
AS              
BEGIN     
Declare @hrAuec int  
Declare @minAuec int  
  
Declare @auec int  
Declare @tableTime int  
Declare @returnVal int  
           
 Set @hrAuec  = Cast(Left(CONVERT(VARCHAR(5),@Date,108),2) as Int)  
 Set @minAuec = Cast(Right(CONVERT(VARCHAR(5),@Date,108),2) as Int)   
  
Set @auec=(@hrAuec*60) + @minAuec  
Set @tableTime = (@Hour*60) + @Min  
  
If(@auec >= @tableTime)  
Set @returnVal=1  
Else  
Set @returnVal=0       
     
       
 RETURN (@returnVal)              
END  
  