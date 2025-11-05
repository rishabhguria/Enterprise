
CREATE PROCEDURE [dbo].[P_SaveCompanyCVCMTAIdentifier]
(
		@companyCounterPartyVenueID int,
        @CompanyCVenueCMTAIdentifierID int,
		@CMTAIdentifier varchar(50)
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyCVCMTAIdentifier
Where CompanyCVenueCMTAIdentifierID = @CompanyCVenueCMTAIdentifierID 
AND CompanyCounterPartyVenueID = @companyCounterPartyVenueID

if(@total > 0)
begin	
	--Update T_CompanyCVCMTAIdentifier
	Update T_CompanyCVCMTAIdentifier 
	Set CMTAIdentifier=@CMTAIdentifier,
        CompanyCounterPartyVenueID = @companyCounterPartyVenueID		
	    Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID 
             AND CompanyCVenueCMTAIdentifierID = @CompanyCVenueCMTAIdentifierID
	
	Select @result = CompanyCVenueCMTAIdentifierID From T_CompanyCVCMTAIdentifier
                    Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID 
                         AND CompanyCVenueCMTAIdentifierID = @CompanyCVenueCMTAIdentifierID
end

else
--Insert T_CompanyCVCMTAIdentifier
begin
	INSERT T_CompanyCVCMTAIdentifier(CompanyCounterPartyVenueID,CMTAIdentifier)
			
	Values(@companyCounterPartyVenueID, @CMTAIdentifier)
Set @result = scope_identity()
end
SELECT @result












