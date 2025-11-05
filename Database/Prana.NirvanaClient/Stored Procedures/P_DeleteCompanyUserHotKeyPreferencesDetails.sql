CREATE PROCEDURE [dbo].[P_DeleteCompanyUserHotKeyPreferencesDetails]
	(
		@companyUserHotKeyName varchar(max),
		@companyUserID int
	)
AS
	Delete
	From T_CompanyUserHotKeyPreferencesDetails
	Where CompanyUserID = @companyUserID And CompanyUserHotKeyName COLLATE Latin1_General_CS_AS = @companyUserHotKeyName