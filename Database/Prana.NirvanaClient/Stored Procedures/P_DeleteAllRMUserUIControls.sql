CREATE PROCEDURE dbo.P_DeleteAllRMUserUIControls 
	
	(
	@userID int 
	
	)
	
AS
	SELECT        RMCompanyUserUIID, CompanyID, CompanyUserID, TicketSize, PriceDeviation, AllowUsertoOverwrite, NotifyUserWhenLiveFeedsAreDown, 
	                         CompanyUserAUECID
	FROM            T_RMCompanyUserUI
	WHERE        (CompanyUserID = @userID)
