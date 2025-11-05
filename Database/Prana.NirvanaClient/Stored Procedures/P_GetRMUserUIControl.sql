CREATE PROCEDURE dbo.P_GetRMUserUIControl
	
	(
	@companyID int,
	@companyUserID int,
	@auecID int
	)
	
AS
	SELECT         CompanyID,RMCompanyUserUIID, CompanyUserID, NotifyUserWhenLiveFeedsAreDown, 
	                         AllowUsertoOverwrite, PriceDeviation, TicketSize, CompanyUserAUECID
	FROM            T_RMCompanyUserUI
	WHERE        (CompanyID = @companyID) AND (CompanyUserID = @companyUserID) AND (CompanyUserAUECID = @auecID)
