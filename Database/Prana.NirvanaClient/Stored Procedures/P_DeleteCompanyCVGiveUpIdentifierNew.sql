CREATE PROCEDURE dbo.P_DeleteCompanyCVGiveUpIdentifierNew
	(
		
		@companyCounterPartyVenueId int		
	)
AS
	Declare @result int
	set @result = 1

	Delete From T_CompanyCVCMTAIdentifier 
                Where CompanyCounterPartyVenueID=@companyCounterPartyVenueId
	
select @result	






