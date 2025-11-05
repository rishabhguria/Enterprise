SET IDENTITY_INSERT T_AuthRoles ON;
truncate table T_AuthRoles;

	INSERT INTO T_AuthRoles(RoleID, RoleName)VALUES(0,'None');
	INSERT INTO T_AuthRoles(RoleID, RoleName)VALUES(1,'User');
	INSERT INTO T_AuthRoles(RoleID, RoleName)VALUES(2,'PowerUser');
	INSERT INTO T_AuthRoles(RoleID, RoleName)VALUES(3,'Administrator');
	INSERT INTO T_AuthRoles(RoleID, RoleName)VALUES(4,'SystemAdministrator');

SET IDENTITY_INSERT T_AuthRoles OFF;
