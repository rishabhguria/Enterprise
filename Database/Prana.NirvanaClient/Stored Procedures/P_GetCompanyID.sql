
CREATE procedure P_GetCompanyID
(@userID int )
as
declare @CompanyID int 
set @CompanyID=-1

select @CompanyID=CompanyID  from T_CompanyUser

where userID=@userID
select @CompanyID

