



/****** Object:  Stored Procedure dbo.P_SaveCompanyCounterPartyVenueIdentifier    Script Date: 01/09/2006 7:10:20 PM ******/
CREATE PROCEDURE [dbo].[P_SaveCompanyCounterPartyVenueIdentifier]
(
		@companyCounterPartyVenueID int,
		@cmtaIdentifier varchar(20),
		@giveUpIdentifier varchar(20)
)
AS 
Declare @result int
Declare @total int 
Set @total = 0
Declare @companyCounterPartyVenueIdentifierID int
Set @companyCounterPartyVenueIdentifierID = 0

Select @total = Count(*)
From T_CompanyCounterPartyVenueIdentifier
Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID

if(@total > 0)
begin	
	--Update CompanyLevelTagDetail
	Update T_CompanyCounterPartyVenueIdentifier 
	Set CompanyCounterPartyVenueID = @companyCounterPartyVenueID,
		CMTAIdentifier = @cmtaIdentifier,
		GiveUpIdentifier = @giveUpIdentifier
				
	Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID
	
	Set @result = @companyCounterPartyVenueIdentifierID
end
else
--Insert CompanyLevelTagDetail
begin
	INSERT T_CompanyCounterPartyVenueIdentifier(CompanyCounterPartyVenueID, CMTAIdentifier, GiveUpIdentifier)
			
	Values(@companyCounterPartyVenueID, @cmtaIdentifier, @giveUpidentifier)
	
	Set @result = scope_identity()
		--	end
end
select @result





