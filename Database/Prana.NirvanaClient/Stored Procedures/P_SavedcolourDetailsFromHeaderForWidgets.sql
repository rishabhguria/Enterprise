-- [P_SavedcolourDetailsFromHeaderForWidgets] stored procedure for saving colour from Widget header     
      
CREATE PROCEDURE [dbo].[P_SavedcolourDetailsFromHeaderForWidgets] (        
@userID int,        
@widgetKeys varchar(max),        
@channelDetails varchar(max))      
  
AS       
      
 DECLARE @noOfRows int        
  SELECT        
    @noOfRows = COUNT(*)        
  FROM T_RTPNL_UserWidgetConfigDetails        
  WHERE UserID=@userID      
        
  Create Table #SavedWidgetKeys (ID INT IDENTITY(1, 1) primary key ,WidgetId varchar(max))    
  Insert into #SavedWidgetKeys                                                                      
  Select Items as WidgetId from dbo.Split(@widgetKeys, '~')     
  
  Create Table #ChannelColour (ID INT IDENTITY(1, 1) primary key ,ColourDetail varchar(max))    
  Insert into #ChannelColour                                                                      
  Select Items as ColourDetail from dbo.Split(@channelDetails, '~')  
  
  Create Table #JoinedWidgetTables(WidgetId varchar(max),ColourDetail varchar(max))  
  Insert into #JoinedWidgetTables (WidgetId,ColourDetail)  
  select #SavedWidgetKeys.WidgetId,#ChannelColour.ColourDetail from #SavedWidgetKeys,#ChannelColour  
  where #SavedWidgetKeys.ID=#ChannelColour.ID  
  
  
  select * from #JoinedWidgetTables  
    
  IF (@noOfRows <> 0)       
  update T_RTPNL_UserWidgetConfigDetails  
  Set ChannelDetail = t2.ColourDetail
From T_RTPNL_UserWidgetConfigDetails as t1  
Inner Join #JoinedWidgetTables as t2  
    On t1.WidgetId = t2.WidgetId

DROP TABLE #JoinedWidgetTables
	,#SavedWidgetKeys,#ChannelColour