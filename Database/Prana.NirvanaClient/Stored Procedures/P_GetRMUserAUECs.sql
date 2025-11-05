

CREATE PROCEDURE dbo.P_GetRMUserAUECs
	(
		
		@companyID int,
		@companyUserID int
		
	)
AS
declare @UIID int
set @UIID = 0

select @UIID = RMCompanyUserUIID from T_RMCompanyUserUI where CompanyID = @companyID and CompanyUserID = @companyUserID

Select CompanyUserAUECID,RMCompanyUserUIID from  T_RMUserAUECs where  RMCompanyUserUIID = @UIID

