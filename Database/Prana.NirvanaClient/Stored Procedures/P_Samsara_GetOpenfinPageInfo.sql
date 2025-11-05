-- [P_RTPNL_GetOpenfinPageInfo] stored procedure for fetching user's layouts from [T_RTPNL_OpenfinPageInfo] table.

CREATE PROCEDURE [dbo].[P_Samsara_GetOpenfinPageInfo] (
@userID int)
AS
  SELECT
    PageInfo.PageName,
    PageInfo.PageLayout,
    PageInfo.PageId,
    PageInfo.PageTag
  FROM [T_Samsara_OpenfinPageInfo] AS PageInfo
  WHERE 
   UserID = @userID