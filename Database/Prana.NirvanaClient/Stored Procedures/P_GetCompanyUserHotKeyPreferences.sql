CREATE PROCEDURE [dbo].[P_GetCompanyUserHotKeyPreferences]
	(
		@companyUserID int
	)
AS
	Select *
	From T_CompanyUserHotKeyPreferences
	Where CompanyUserID = @companyUserID