CREATE procedure [dbo].[P_GetAuditTrailIgnoredUsers]
@userId int,
@companyId int
as
Select IgnoredUsers from T_AuditTrailOtherPermissions where CompanyId = @companyId and UserId=@userId
