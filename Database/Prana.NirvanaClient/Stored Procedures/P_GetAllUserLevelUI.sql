CREATE PROCEDURE dbo.P_GetAllUserLevelUI
	(
		
		@companyID int,
		@companyUserID int
		
	)
AS
	SELECT        CompanyID, RMCompanyUserUIID, CompanyUserID, NotifyUserWhenLiveFeedsAreDown, AllowUsertoOverwrite, PriceDeviation, TicketSize, 
	                         CompanyUserAUECID
	FROM            T_RMCompanyUserUI
	WHERE        (CompanyID = @companyID) AND (CompanyUserID = @companyUserID)
	