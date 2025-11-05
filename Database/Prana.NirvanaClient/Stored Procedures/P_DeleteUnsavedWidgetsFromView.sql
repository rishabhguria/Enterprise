-- [P_DeleteUnsavedWidgetsFromView] stored procedure for deleting removed widgets from a view     
CREATE PROCEDURE [dbo].[P_DeleteUnsavedWidgetsFromView] (  
 @userID INT  
 ,@viewName VARCHAR(100)  
 ,@removedWidgets VARCHAR(max)  
 ,@pageId VARCHAR(max)  
 )  
AS  
DECLARE @noOfRows INT  
  
SELECT @noOfRows = COUNT(*)  
FROM T_RTPNL_UserWidgetConfigDetails  
WHERE ViewName = @viewName  
 AND UserID = @userID  
 AND PageId = @pageId  
  
CREATE TABLE #removedWidgets (WidgetId VARCHAR(max))  
  
INSERT INTO #removedWidgets  
SELECT Items AS WidgetId  
FROM dbo.Split(@removedWidgets, '~')  
  
IF (@noOfRows <> 0)  
 INSERT INTO T_RTPNL_UserWidgetConfigDetails_Deleted (  
  UserID  
  ,PageId  
  ,WidgetId  
  ,ViewName  
  ,WidgetName  
  ,WidgetType  
  ,DefaultColumns  
  ,ColoredColumns  
  ,GraphType  
  ,IsFlashColorEnabled  
  ,ChannelDetail  
  ,LinkedWidget  
  ,PrimaryMetric  
  ,DeletedTime  
  )  
 SELECT UserID  
  ,PageId  
  ,WidgetId  
  ,ViewName  
  ,WidgetName  
  ,WidgetType  
  ,DefaultColumns  
  ,ColoredColumns  
  ,GraphType  
  ,IsFlashColorEnabled  
  ,ChannelDetail  
  ,LinkedWidget  
  ,PrimaryMetric  
  ,GETDATE()  
 FROM T_RTPNL_UserWidgetConfigDetails  
 WHERE UserID = @userID  
  AND ViewName = @viewName  
  AND PageId = @pageId  
  AND WidgetId IN (  
   SELECT *  
   FROM #removedWidgets)  
  
DELETE  
FROM T_RTPNL_UserWidgetConfigDetails  
WHERE UserID = @userID  
 AND ViewName = @viewName  
 AND PageId = @pageId  
 AND WidgetId IN (  
  SELECT *  
  FROM #removedWidgets)