-- [P_SaveCompanyUserLayouts] stored procedure for saving Company user's layouts for different modules in [T_Samsara_CompanyUserLayouts] table.

CREATE PROCEDURE [dbo].[P_Samsara_SaveCompanyUserLayouts] (
@userID int,
@moduleName varchar(100),
@description varchar(2000),
@viewId varchar(100),
@viewName varchar(100),
@viewLayout varbinary(max))
AS
  DECLARE @noOfRows int
  SELECT
    @noOfRows = COUNT(*)
  FROM [T_Samsara_CompanyUserLayouts]
  WHERE UserID = @userID
  AND ModuleName = @moduleName
  AND ViewId = @viewId
  IF (@noOfRows = 0)
    INSERT INTO [T_Samsara_CompanyUserLayouts](UserID, LastSavedTime, ModuleName, Description, ViewId, ViewName,ViewLayout)
      VALUES (@UserID, GETDATE(),@moduleName, @description, @viewId, @viewName,@viewLayout)
  ELSE
    UPDATE [T_Samsara_CompanyUserLayouts]
    SET ViewLayout = @viewLayout,
        LastSavedTime = GETDATE(),
        Description = @description,
        ViewName = @viewName
    WHERE UserID = @userID
    AND ModuleName = @moduleName
    AND ViewId = @viewId
GO
