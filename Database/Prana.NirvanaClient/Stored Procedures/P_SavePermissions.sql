-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 13-may-14
--Purpose: Get the details of the permissions saved in the DB
-----------------------------------------------------------------

CREATE procedure [dbo].[P_SavePermissions]
(
@xmlPermission nText
)
as
declare @handle int
exec sp_xml_preparedocument @handle output, @xmlPermission

CREATE TABLE #TempPermissions                                                                               
(                                                                               
Perm_Id int, 
PType int, 
PValue int,
ResDataType int, 
ResDataValue int, 
AuthActValue int                    
)  
      
insert INTO #TempPermissions                                                                               
( Perm_Id, PType, PValue, ResDataType, ResDataValue, AuthActValue)

SELECT 
PermissionId, 
PrincipalType, 
PricipalValue,
ResourceDataType, 
ResourceDataValue, 
AuthActionValue
from openXML(@handle,'dsPermission/dtPermission',2)
with
(
PermissionId INT, 
PrincipalType int, 
PricipalValue int,
ResourceDataType int, 
ResourceDataValue int, 
AuthActionValue int
)
--------------------------------------------------------------------------
--Delete Permissions
delete from T_AuthPermissions where PermissionId not in(SELECT Perm_Id from #TempPermissions)
--select * from T_AuthPermissions
--------------------------------------------------------------------------
--Update exisiting permissions
update T_AuthPermissions
set 
T_AuthPermissions.PrincipalType=#TempPermissions.PType,
T_AuthPermissions.PricipalValue=#TempPermissions.PValue,
T_AuthPermissions.ResourceDataType=#TempPermissions.ResDataType,
T_AuthPermissions.ResourceDataValue=#TempPermissions.ResDataValue,
T_AuthPermissions.AuthActionValue=#TempPermissions.AuthActValue
from #TempPermissions
where T_AuthPermissions.PermissionId=#TempPermissions.Perm_Id
delete from #TempPermissions where Perm_Id in(SELECT PermissionId from T_AuthPermissions)
---------------------------------------------------------------------------
--Insert new permissions
set IDENTITY_INSERT T_AuthPermissions ON
insert INTO T_AuthPermissions
( PermissionId, PrincipalType, PricipalValue, ResourceDataType, ResourceDataValue, AuthActionValue)
SELECT 
Perm_Id, PType, PValue, ResDataType, ResDataValue, AuthActValue
from #TempPermissions
set IDENTITY_INSERT T_AuthPermissions OFF
--select * from #TempPermissions; 
drop TABLE #TempPermissions
exec sp_xml_removedocument @handle
