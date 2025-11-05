
CREATE PROCEDURE [dbo].[P_CA_SaveComplianceUserPermissions] 
	
	@CompanyId int , 
	@CompanyUserId int,
	@RuleType varchar(MAX),
    @IsCreate bit,
    @IsRename bit,
    @IsDelete bit,
    @IsEnable bit,
    @IsExport bit,
    @IsImport bit
   
AS
BEGIN
 delete from T_CA_UserReadWritePermission where companyId=@CompanyId and userId=@CompanyUserId and ruleType=@RuleType
 insert into T_CA_UserReadWritePermission (CompanyId,UserId,RuleType,IsCreate,IsRename,IsDelete,IsEnable,IsExport,IsImport) values(
	@CompanyId , 
	@CompanyUserId ,
	@RuleType ,
    @IsCreate ,
    @IsRename ,
    @IsDelete ,
    @IsEnable ,
    @IsExport ,
    @IsImport
)

END

