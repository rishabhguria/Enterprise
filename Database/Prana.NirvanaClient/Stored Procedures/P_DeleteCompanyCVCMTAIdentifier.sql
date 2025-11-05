CREATE PROCEDURE dbo.P_DeleteCompanyCVCMTAIdentifier
	(
		
		@companyCounterPartyVenueId int	,
        @CompanyCVenueCMTAIdentifierID varchar(200) = ''	
	)
AS
	--Declare @result int
	--set @result = 1

if(@CompanyCVenueCMTAIdentifierID = '') 
	begin
	Delete T_CompanyCVCMTAIdentifier
           Where CompanyCounterPartyVenueID=@companyCounterPartyVenueId
   end
else
	begin
	
		exec ('Delete T_CompanyCVCMTAIdentifier
		Where convert(varchar, CompanyCVenueCMTAIdentifierID) NOT IN(' + @CompanyCVenueCMTAIdentifierID + ') 
			 AND CompanyCounterPartyVenueID = ' + @companyCounterPartyVenueId)
	end
	
--select @result	







