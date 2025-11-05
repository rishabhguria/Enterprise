/****** Object:  Stored Procedure dbo.P_UpdateCompanyUserHotKeyPreferencesDetails ******/
CREATE PROCEDURE [dbo].[P_UpdateCompanyUserHotKeyPreferencesDetails]
	(
		@companyUserHotKeyName varchar(max),
		@hotKeyPreferenceNameValue varchar(max),
		@isFavourites bit,
		@hotKeySequence int,
		@companyUserID int,
		@companyUserHotKeyID int
	)
AS

	Declare @result int
	Declare @total int 
	Declare @duplicateHotKeyName int 
	Set @total = 0
	Set @duplicateHotKeyName = 0
	
	Select @duplicateHotKeyName = Count(*)
	From T_CompanyUserHotKeyPreferencesDetails
	Where CompanyUserID = @companyUserID AND CompanyUserHotKeyName COLLATE Latin1_General_CS_AS = @companyUserHotKeyName AND CompanyUserHotKeyID <> @companyUserHotKeyID

	Select @total = Count(*)
	From T_CompanyUserHotKeyPreferencesDetails
	Where CompanyUserID = @companyUserID AND CompanyUserHotKeyID = @companyUserHotKeyID

	if(@duplicateHotKeyName <= 0 AND @total > 0)
	begin	
		Update T_CompanyUserHotKeyPreferencesDetails 
		Set CompanyUserHotKeyName = @companyUserHotKeyName,
			HotKeyPreferenceNameValue = @hotKeyPreferenceNameValue, 
			IsFavourites = @isFavourites,
			HotKeySequence = @hotKeySequence
		Where CompanyUserID = @companyUserID AND CompanyUserHotKeyID = @companyUserHotKeyID

		Select @result = CompanyUserHotKeyID From T_CompanyUserHotKeyPreferencesDetails Where CompanyUserID = @companyUserID AND CompanyUserHotKeyID = @companyUserHotKeyID
	end
	else
	begin
		Set @result = -1
	end	
select @result