/****** Object:  Stored Procedure dbo.P_UpdateCompanyUserHotKeySequenceOrder ******/
CREATE PROCEDURE [dbo].[P_UpdateCompanyUserHotKeySequenceOrder]
	(
		@hotKeySequence int,
		@companyUserID int,
		@companyUserHotKeyID int
	)
AS

	Declare @result int
	Declare @total int 
	Set @total = 0
	
	Select @total = Count(*)
	From T_CompanyUserHotKeyPreferencesDetails
	Where CompanyUserID = @companyUserID AND CompanyUserHotKeyID = @companyUserHotKeyID

	if(@total > 0)
	begin	
		Update T_CompanyUserHotKeyPreferencesDetails 
		Set HotKeySequence = @hotKeySequence
		Where CompanyUserID = @companyUserID AND CompanyUserHotKeyID = @companyUserHotKeyID

		Select @result = CompanyUserHotKeyID From T_CompanyUserHotKeyPreferencesDetails Where CompanyUserID = @companyUserID AND CompanyUserHotKeyID = @companyUserHotKeyID
	end
	else
	begin
		Set @result = -1
	end	
select @result