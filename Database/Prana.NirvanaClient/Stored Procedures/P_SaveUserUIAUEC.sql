

CREATE PROCEDURE dbo.P_SaveUserUIAUEC

	(

--@rMCompanyUserUIID int ,
@companyID int,
@companyUserID int,
@companyUserAUECID int
	)

AS

declare @rMCompanyUserUIID int, @result int
set @rMCompanyUserUIID = 0

SELECT     @rMCompanyUserUIID = RMCompanyUserUIID
FROM         T_RMCompanyUserUI
WHERE     (CompanyID= @companyID and CompanyUserID = @companyUserID) 

--select CompanyUserAUECID from T_RMUserAUECs where RMCompanyUserUIID =@rMCompanyUserUIID
--if((select count(*) from T_RMUserAUECs where RMCompanyUserUIID=@rMCompanyUserUIID )=0)
insert into T_RMUserAUECs(RMCompanyUserUIID,CompanyUserAUECID)  values(@rMCompanyUserUIID,@companyUserAUECID)


