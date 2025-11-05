

CREATE PROCEDURE dbo.P_DeleteAllUserUIAUEC

	(
		@companyID int,
		@companyUserID int
	)
as
declare @rMCompanyUserUIID int
set @rMCompanyUserUIID = 0

SELECT     @rMCompanyUserUIID = RMCompanyUserUIID
FROM         T_RMCompanyUserUI
WHERE     (CompanyID= @companyID and CompanyUserID = @companyUserID) 

delete from T_RMUserAUECs  where RMCompanyUserUIID = @rMCompanyUserUIID
	


