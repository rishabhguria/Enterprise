-- [P_UpdateViewNameWidgetConfigDetails] stored procedure for updating Company user's widget configuration details for RTPNL module in [T_RTPNL_UserWidgetConfigDetails] table based on view name.  
  
CREATE PROCEDURE [dbo].[P_UpdateViewNameWidgetConfigDetails] (      
@userID int,      
@oldViewName varchar(100),      
@newViewName varchar(100),  
@pageID varchar(max))  
AS     
    
 DECLARE @noOfRows int      
  SELECT      
    @noOfRows = COUNT(*)      
  FROM T_RTPNL_UserWidgetConfigDetails      
  WHERE ViewName = @oldViewName and UserID=@userID and PageId=@pageID
      
  IF (@noOfRows <> 0)     
   UPDATE T_RTPNL_UserWidgetConfigDetails      
    SET       
  ViewName = @newViewName,    
        WidgetId  = CONCAT(CAST(@userID as varchar(20)), '_', @pageID, '_', @newViewName, '_', WidgetType, '_', WidgetName)      
      
     WHERE ViewName = @oldViewName and UserID=@userID and PageId=@pageID