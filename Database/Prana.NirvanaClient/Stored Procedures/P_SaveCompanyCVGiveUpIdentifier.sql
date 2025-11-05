
CREATE PROCEDURE [dbo].[P_SaveCompanyCVGiveUpIdentifier]
(
		@companyCounterPartyVenueID int,
        @CompanyCVenueGiveupIdentifierID int,
		@GiveUpIdentifier varchar(50)
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyCVGiveUpIdentifier
Where CompanyCVenueGiveupIdentifierID = @CompanyCVenueGiveupIdentifierID 
AND CompanyCounterPartyVenueID = @companyCounterPartyVenueID

if(@total > 0)
begin	
	--Update T_CompanyCVGiveUpIdentifier
	Update T_CompanyCVGiveUpIdentifier 
	Set GiveUpIdentifier=@GiveUpIdentifier,
        CompanyCounterPartyVenueID = @companyCounterPartyVenueID		
	    Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID 
             AND CompanyCVenueGiveupIdentifierID = @CompanyCVenueGiveupIdentifierID
	
	Select @result = CompanyCVenueGiveupIdentifierID From T_CompanyCVGiveUpIdentifier
                    Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID 
                         AND CompanyCVenueGiveupIdentifierID = @CompanyCVenueGiveupIdentifierID
end

else
--Insert T_CompanyCVGiveUpIdentifier
begin
	INSERT T_CompanyCVGiveUpIdentifier(CompanyCounterPartyVenueID, GiveUpIdentifier)
			
	Values(@companyCounterPartyVenueID, @GiveUpIdentifier)
Set @result = scope_identity()
end
SELECT @result












