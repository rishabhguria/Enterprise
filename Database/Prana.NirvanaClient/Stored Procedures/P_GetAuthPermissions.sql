---------------------------------------------------
--Modfied by: Bharat raturi
--date: 14-may-2014
--purpose: get the auth permissions from db with extra details permissionID
----------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_GetAuthPermissions]
AS	
select 
PrincipalType,
PricipalValue,
ResourceDataType,
ResourceDataValue, 
AuthActionValue,
PermissionId 
from T_AuthPermissions

