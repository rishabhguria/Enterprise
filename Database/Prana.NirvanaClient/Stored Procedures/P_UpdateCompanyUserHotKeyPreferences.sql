/****** Object:  Stored Procedure dbo.P_UpdateCompanyUserHotKeyPreferences ******/
CREATE PROCEDURE [dbo].[P_UpdateCompanyUserHotKeyPreferences]
	(
		@hotKeyPreferenceElements varchar(max),
		@enableBookMarkIcon bit,
		@hotKeyOrderChanged bit,
		@tTTogglePreferenceForWeb bit, 
		@companyUserID int
	)
AS

	Declare @result int
	Declare @total int 
	Set @total = 0
	
	Select @total = Count(*)
	From T_CompanyUserHotKeyPreferences
	Where CompanyUserID = @companyUserID

	if(@total > 0)
	begin	
		Update T_CompanyUserHotKeyPreferences 
		Set HotKeyPreferenceElements = @hotKeyPreferenceElements, 
			EnableBookMarkIcon = @enableBookMarkIcon,
			HotKeyOrderChanged = @hotKeyOrderChanged,
			TTTogglePreferenceForWeb = @tTTogglePreferenceForWeb
		Where CompanyUserID = @companyUserID

		Select @result = CompanyUserHotKeyPreferenceID From T_CompanyUserHotKeyPreferences Where CompanyUserID = @companyUserID
	end
	else
	begin
		Set @result = -1
	end	
select @result