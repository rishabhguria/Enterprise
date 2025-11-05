-----------------------------------------------------  
--Created By: Bharat raturi  
--Date: 13/may/2014  
--Purpose: Get the user permissions from the database 
--usage:    P_GetAuthKeyValuePairs
-----------------------------------------------------  
CREATE procedure [dbo].[P_GetAuthKeyValuePairs]  
as 
BEGIN
select AuthActionId, AuthActionName from T_AuthAction;
select TypeId, TypeName from T_AuthPrincipalType;
select ResourceDataTypeId, ResourceDataTypeName from T_AuthResourceDataType;
select ModuleID, ModuleName from T_Module order BY ModuleName;
select FundGroupID, GroupName from T_FundGroups;
select RoleID, RoleName from T_AuthRoles;
END
