CREATE PROCEDURE [dbo].[P_GetCompanyUserHotKeyPreferencesDetails]
	(
		@companyUserID int
	)
AS

	Declare @hotKeyOrderChanges bit
	Set @hotKeyOrderChanges = 0

	Select @hotKeyOrderChanges = HotKeyOrderChanged
	From T_CompanyUserHotKeyPreferences
	Where CompanyUserID = @companyUserID

	If(@hotKeyOrderChanges = 1)
	Begin
		Select *
		From T_CompanyUserHotKeyPreferencesDetails
		Where CompanyUserID = @companyUserID
		Order By HotKeySequence
	End
	Else
	Begin
		Select *
		From T_CompanyUserHotKeyPreferencesDetails
		Where CompanyUserID = @companyUserID
		Order By IsFavourites Desc, HotKeySequence
	End