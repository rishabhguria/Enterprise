-- [P_SaveOpenfinPageInfo] stored procedure for saving Company user's Openfin page information in [T_RTPNL_OpenfinPageInfo]table.

CREATE PROCEDURE [dbo].[P_Samsara_SaveOpenfinPageInfo] (
@userID int,
@pageName varchar(100),
@oldPageName varchar(100),
@pageLayout varbinary(max),
@pageId varchar(100),
@pageTag varchar(100))
AS
  DECLARE @noOfRows int
  SELECT
    @noOfRows = COUNT(*)
  FROM [T_Samsara_OpenfinPageInfo] 
  WHERE UserID = @userID
  AND PageId = @pageId
  IF (@noOfRows = 0)
    INSERT INTO [T_Samsara_OpenfinPageInfo](UserID, PageName, PageLayout, PageId, PageTag,LastSavedTime)
      VALUES (@UserID, @pageName, @pageLayout, @pageId, @pageTag,GETDATE())
  ELSE
    UPDATE [T_Samsara_OpenfinPageInfo]
    SET PageName = @pageName,
        PageLayout = @pageLayout,
        PageId = @pageId,
        PageTag = @pageTag,
        LastSavedTime = GETDATE()
    WHERE UserID = @userID
    AND PageId = @pageId