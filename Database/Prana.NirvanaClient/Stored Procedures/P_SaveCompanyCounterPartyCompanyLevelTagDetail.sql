


/****** Object:  Stored Procedure dbo.P_SaveCompanyCounterPartyCompanyLevelTagDetail    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_SaveCompanyCounterPartyCompanyLevelTagDetail
(
		@companyCounterPartyVenueID int,
		@companyClearingFirmPrimeBrokerID int,
		@companyMPID int,
		@deliverToCompanyID varchar(20),
		@senderCompanyID varchar(20),
		@targetCompID varchar(20),
		@clearingFirm varchar(20),
		@companyFundID int,
		@companyStrategyID int,
		@onBehalfOfSubID varchar(20)
)
AS 
Declare @result int
Declare @total int 
Set @total = 0
Declare @companyCounterPartyVenueDetailsID int
Set @companyCounterPartyVenueDetailsID = 0

Select @total = Count(*)
From T_CompanyCounterPartyVenueDetails
Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID

if(@total > 0)
begin	
	--Update CompanyLevelTagDetail
	Update T_CompanyCounterPartyVenueDetails 
	Set CompanyCounterPartyVenueID = @companyCounterPartyVenueID,
		CompanyClearingFirmsPrimeBrokersID = @companyClearingFirmPrimeBrokerID,
		CompanyMPID = @companyMPID,
		DeliverToCompanyID = @deliverToCompanyID,
		SenderCompanyID = @senderCompanyID,
		TargetCompID = @targetCompID,
		ClearingFirm = @clearingFirm,
		CompanyFundID = @companyFundID,
		CompanyStrategyID = @companyStrategyID,
		OnBehalfOfSubID = @onBehalfOfSubID
				
	Where CompanyCounterPartyVenueID = @companyCounterPartyVenueID
	
	Set @result = @companyCounterPartyVenueDetailsID
end
else
--Insert CompanyLevelTagDetail
begin
	INSERT T_CompanyCounterPartyVenueDetails(CompanyCounterPartyVenueID, CompanyClearingFirmsPrimeBrokersID, 
			CompanyMPID, DeliverToCompanyID, SenderCompanyID, TargetCompID, ClearingFirm, CompanyFundID, 
			CompanyStrategyID, OnBehalfOfSubID)
	Values(@companyCounterPartyVenueID, @companyClearingFirmPrimeBrokerID, @companyMPID, @deliverToCompanyID,
				@senderCompanyID, @targetCompID, @clearingFirm, @companyFundID, @companyStrategyID, @onBehalfOfSubID)
	
	Set @result = scope_identity()
		--	end
end
select @result




