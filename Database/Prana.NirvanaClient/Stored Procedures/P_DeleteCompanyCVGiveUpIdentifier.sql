CREATE PROCEDURE dbo.P_DeleteCompanyCVGiveUpIdentifier
	(
		
		@companyCounterPartyVenueId int	,
        @CompanyCVenueGiveupIdentifierID varchar(200) = ''	
	)
AS
	--Declare @result int
	--set @result = 1

if(@CompanyCVenueGiveupIdentifierID = '') 
	begin
	Delete From T_CompanyCVGiveUpIdentifier
           Where CompanyCounterPartyVenueID=@companyCounterPartyVenueId
   end
else
	begin
	
		exec ('Delete T_CompanyCVGiveUpIdentifier
		Where convert(varchar, CompanyCVenueGiveupIdentifierID) NOT IN(' + @CompanyCVenueGiveupIdentifierID + ') 
			 AND CompanyCounterPartyVenueID = ' + @companyCounterPartyVenueId)
	end
	
--select @result	





