      
      
-- =============================================              
-- Author:  <Sandeep>              
-- Create date: <16-May-2008>              
-- Description: <Get NAV values>            
--Select * from PM_NAVValue order by Date desc       
--Select * from GetNAVForDate('2008-05-18')           
-- =============================================              
CREATE FUNCTION [dbo].[GetNAVForDate]       
(        
  @date datetime              
)              
RETURNS @NAVValueDatewise TABLE               
 (       
  Date datetime,             
  NAVValue float,  
  NAVIndicator int        
 )              
AS              
BEGIN        
--Declare @tblMaxDateForNAV Table              
--(           
--   MaxDate datetime        
--)       
--        
--Insert into @tblMaxDateForNAV        
--                                
--     SELECT                                                 
--      Max(Date) as Date                                      
--     FROM                                         
--      PM_NAVValue                                        
--     WHERE                                        
--      Date <= dbo.GetFormattedDatePart(@date) And NavValue>0           
--          
--  INSERT INTO @NAVValueDatewise              
--  Select        
--   @date   ,      
--   PM_NAVValue.NAVValue      
--         
--         From PM_NAVValue                
--         INNER JOIN @tblMaxDateForNAV NAVValue on PM_NAVValue.Date = NAVValue.MaxDate       
  
  
INSERT INTO @NAVValueDatewise              
Select   TOP 1   
 @date,       
 PM_NAVValue.NAVValue,        
 CASE   
 WHEN datediff(d,PM_NAVValue.Date,@date) = 0
 THEN 0        
 ELSE 1        
 END         
From PM_NAVValue                
Where         
 datediff(d,PM_NAVValue.Date,@date) >= 0 and PM_NAVValue.NAVValue >0        
Order by  dbo.GetFormattedDatePart(PM_NAVValue.Date) DESC        
  
      
          
RETURN          
END 

