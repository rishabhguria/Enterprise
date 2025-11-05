CREATE PROCEDURE dbo.P_SaveUserLevelUI
(
		@rMCompanyUserUIID int,
		@companyID int,
		@companyUserID int,
		@companyUserAUECID int,
		@ticketSize int,
		@priceDeviation int,
		@allowUsertoOverWrite int,
		@notifyUserWhenLiveFeedsAreDown int,
		@result int
			
	)
AS 
if(@rMCompanyUserUIID >0)

begin	
	Update T_RMCompanyUserUI 
	Set 
		TicketSize = @ticketSize, 
		PriceDeviation = @priceDeviation,
		AllowUsertoOverWrite=@allowUsertoOverWrite,
		NotifyUserWhenLiveFeedsAreDown = @notifyUserWhenLiveFeedsAreDown
			
	Where RMCompanyUserUIID = @rMCompanyUserUIID 
	
	Set @result = -1  
	end 
	else
	begin
		INSERT into T_RMCompanyUserUI ( CompanyID, CompanyUserID,CompanyUserAUECID, TicketSize,PriceDeviation, AllowUsertoOverWrite,NotifyUserWhenLiveFeedsAreDown)
		Values( @companyID,@companyUserID,@companyUserAUECID,@ticketSize, @priceDeviation,@allowUsertoOverWrite,@notifyUserWhenLiveFeedsAreDown)  
			
		Set @result = scope_identity()
	end


select @result
