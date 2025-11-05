-- [P_GetCompanyUserLayouts] stored procedure for fetching Company user's layouts for different modules from [T_CompanyUserLayouts] table.

CREATE PROCEDURE [dbo].[P_Samsara_GetCompanyUserLayouts] (
@userID int)
AS
  SELECT
    ViewInfo.ViewId,
    ViewInfo.ViewName,
    ViewInfo.ViewLayout,
    ViewInfo.Description,
	ViewInfo.ModuleName
  FROM [T_Samsara_CompanyUserLayouts] AS ViewInfo
  WHERE 
   UserID = @userID
GO