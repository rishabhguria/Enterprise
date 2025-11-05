CREATE PROCEDURE [dbo].[P_SaveCompanyUserHotKeyPreferencesDetails]
(
    @companyUserHotKeyName varchar(max),
    @hotKeyPreferenceNameValue varchar(max),
    @isFavourites bit,
    @hotKeySequence int,
    @companyUserID int,
    @module VARCHAR(MAX), 
    @hotButtonType VARCHAR(MAX) 
)
AS

Declare @result int
	Declare @duplicateHotKeyName int 
	Declare @totalHotKeyUserWise int 
	Declare @maxSequence int 
	Set @duplicateHotKeyName = 0
	Set @totalHotKeyUserWise = 0
	Set @maxSequence = 0
    
    Select @duplicateHotKeyName = Count(*)
    From T_CompanyUserHotKeyPreferencesDetails
    Where CompanyUserID = @companyUserID AND CompanyUserHotKeyName COLLATE Latin1_General_CS_AS = @companyUserHotKeyName
    
    Select @totalHotKeyUserWise = Count(*)
    From T_CompanyUserHotKeyPreferencesDetails
    Where CompanyUserID = @companyUserID


    if(@duplicateHotKeyName <= 0 AND @totalHotKeyUserWise <= 99)
    begin 
    
        Select @maxSequence = Max(HotKeySequence)
        From T_CompanyUserHotKeyPreferencesDetails
        Where CompanyUserID = @companyUserID;

        Insert T_CompanyUserHotKeyPreferencesDetails(CompanyUserID, CompanyUserHotKeyName, HotKeyPreferenceNameValue, IsFavourites, HotKeySequence, Module, HotButtonType, LastSavedTime)
        Values(@companyUserID, @companyUserHotKeyName, @hotKeyPreferenceNameValue, @isFavourites, ISNULL(@maxSequence,0)+1, @module, @hotButtonType , GETDATE())

		Set @result = scope_identity()
	end
	else
	begin
		Set @result = -1
	end	
select @result