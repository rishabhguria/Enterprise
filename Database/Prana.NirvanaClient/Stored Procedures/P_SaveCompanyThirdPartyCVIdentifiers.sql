/****** Object:  Stored Procedure dbo.P_SaveCompanyThirdPartyCVIdentifiers    Script Date: 22/03/2006 7:10:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyThirdPartyCVIdentifiers
(
		@CompanyThirdPartyID int,
		@CompanyCounterPartyVenueID int,
		@CVIdentifier varchar(50),
		@result int 
)
AS 
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyThirdPartyCVIdentifier 
Where CompanyThirdPartyID_FK = @CompanyThirdPartyID And CompanyCounterPartyVenueID_FK = @CompanyCounterPartyVenueID --And CVIdentifier = @CVIdentifier

if(@total > 0)
begin	
	
	--Update T_CompanyThirdPartyCVIdentifier
	Update T_CompanyThirdPartyCVIdentifier 
	Set 
		CompanyCounterPartyVenueID_FK = @CompanyCounterPartyVenueID, 
		CVIdentifier = @CVIdentifier
			
	Where CompanyThirdPartyID_FK = @CompanyThirdPartyID And CompanyCounterPartyVenueID_FK = @CompanyCounterPartyVenueID --And CVIdentifier = @CVIdentifier 
				
		
	Set @result = -1
	
end
else
--Insert T_CompanyThirdPartyCVIdentifier
begin
	INSERT T_CompanyThirdPartyCVIdentifier(CompanyThirdPartyID_FK, CompanyCounterPartyVenueID_FK, CVIdentifier)
	Values(@CompanyThirdPartyID, @CompanyCounterPartyVenueID, @CVIdentifier)  
					
				Set @result = scope_identity()
end
select @result