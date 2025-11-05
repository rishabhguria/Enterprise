-- [P_RTPNL_GetUserWidgetConfigDetails] stored procedure for fetching the Company user's widget configuration details for RTPNL module in [T_RTPNL_UserWidgetConfigDetails] table.

CREATE PROCEDURE [dbo].[P_RTPNL_GetUserWidgetConfigDetails] (
@userID int)
AS
  SELECT
	DataBaseWidget.PageId,
    DataBaseWidget.WidgetName,
    DataBaseWidget.WidgetType,
    DataBaseWidget.DefaultColumns,
    DataBaseWidget.ColoredColumns,
    DataBaseWidget.GraphType,
    DataBaseWidget.IsFlashColorEnabled,
    DataBaseWidget.ChannelDetail,
    DataBaseWidget.LinkedWidget,
	DataBaseWidget.ViewName,
    DataBaseWidget.WidgetId,
	DataBaseWidget.PrimaryMetric
  FROM T_RTPNL_UserWidgetConfigDetails AS DataBaseWidget
  WHERE 
   UserID = @userID
 --TODO: In future we need to add condition for viewName to fetch the widget details for perticular View.
