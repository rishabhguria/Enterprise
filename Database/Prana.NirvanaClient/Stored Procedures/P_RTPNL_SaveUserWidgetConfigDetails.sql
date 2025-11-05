-- [P_RTPNL_SaveUserWidgetConfigDetails] stored procedure for saving Company user's widget configuration details for RTPNL module in [T_RTPNL_UserWidgetConfigDetails] table.

CREATE PROCEDURE [dbo].[P_RTPNL_SaveUserWidgetConfigDetails] (
@userID int,  
@viewName varchar(100),  
@widgetName varchar(100),  
@widgetType varchar(100),  
@defaultColumns varchar(8000),  
@coloredColumns varchar(8000),  
@graphType varchar(100),  
@isFlashColorEnabled int,  
@channelDetail varchar(100),  
@linkedWidget varchar(100),  
@widgetId varchar(500),   
@primaryMetric varchar(20),  
@pageId varchar(100))  
AS  
  DECLARE @noOfRows int  
  SELECT  
    @noOfRows = COUNT(*)  
  FROM T_RTPNL_UserWidgetConfigDetails  
  WHERE WidgetId = @widgetId   
  IF (@noOfRows = 0)  
    INSERT INTO T_RTPNL_UserWidgetConfigDetails (UserID, ViewName, WidgetName, WidgetType, DefaultColumns, ColoredColumns, GraphType, IsFlashColorEnabled, ChannelDetail, LinkedWidget, WidgetId, PrimaryMetric,PageId, LastSavedTime)  
      VALUES (@userID, @viewName, @widgetName, @widgetType, @defaultColumns , @coloredColumns, @graphType, @isFlashColorEnabled, @channelDetail, @linkedWidget, @widgetId, @primaryMetric,@pageId, GETDATE())  
  ELSE  
    UPDATE T_RTPNL_UserWidgetConfigDetails  
    SET WidgetName = @widgetName,  
        DefaultColumns = @defaultColumns,  
        ColoredColumns = @coloredColumns,  
             GraphType = @graphType,  
  IsFlashColorEnabled  = @isFlashColorEnabled,  
        ChannelDetail  = @channelDetail,  
         LinkedWidget  = @linkedWidget ,    
		 PrimaryMetric = @primaryMetric,
				PageId = @pageId,
                ViewName = @viewName,
                LastSavedTime = GETDATE()
  
    WHERE WidgetId = @widgetId  
