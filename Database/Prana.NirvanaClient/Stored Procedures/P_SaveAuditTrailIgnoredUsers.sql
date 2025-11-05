CREATE procedure [dbo].[P_SaveAuditTrailIgnoredUsers]
@userId int,
@companyId int,
@ignoredUsers varchar(200)
as
if exists(SELECT * from T_AuditTrailOtherPermissions where CompanyId=@companyId AND UserId =@userId)
UPDATE T_AuditTrailOtherPermissions set IgnoredUsers=@ignoredUsers where CompanyId=@companyId AND UserId =@userId
else
insert INTO T_AuditTrailOtherPermissions ([CompanyId]
           ,[UserId]
           ,[IgnoredUsers]) values (@companyId,@userId,@ignoredUsers)
