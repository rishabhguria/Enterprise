-- =============================================        
-- Author:  Narendra Kumar Jangir        
-- Create date: 29 Nov 2013        
-- Description: Returns Long or Short on the basis of side        
-- =============================================        
CREATE FUNCTION [dbo].[GetLongOrShort] 
(        
 @SideId varchar(1)        
)        
RETURNS  varchar(10)        
AS        
BEGIN      
      
declare @sideMultiplier varchar(10)       
     
set @sideMultiplier = case @sideID  --Find out SIde multiplier        
       When '1' THEN  'Long' --Buy    
       When '2' THEN  'Long' --Sell 
       When 'A' THEN  'Long' --Buy to Open 
       When 'D' THEN  'Long' --Sell to Close    
       When '5' THEN  'Short'--Sell short                  
       When 'B' THEN  'Short'--Buy to Close     
       When 'E' THEN  'Short'--Buy to Cover 
	   When '3' THEN  'Short'--Buy minus  
	   When 'C' THEN  'Short'--Sell to Open         
       end    
      
RETURN (@sideMultiplier)        
        
END   
